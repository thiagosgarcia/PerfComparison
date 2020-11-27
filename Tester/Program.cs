using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Tester
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hi!");
            Console.WriteLine(string.Join(", ", args));
            if (args.Length < 3)
                args = new[] { "net21", "net31", "net50" };
            Console.WriteLine(string.Join(", ", args));

            Console.WriteLine($"Waiting 5s until everything is loaded");
            Thread.Sleep(TimeSpan.FromSeconds(5));
            var results = new Dictionary<string, string>();
            var warmLoop = 1000;
            const int testLoop = 10000;
            foreach (var s in args)
            {
                Console.WriteLine();
                var st = new Stopwatch();
                Console.WriteLine($"Warming up {s} with {warmLoop} iterations 2x");
                st.Start();
                await GetSync(s, warmLoop);
                await GetAsync(s, warmLoop);
                await GetSync(s, warmLoop);
                await GetAsync(s, warmLoop);
                st.Stop();
                Console.WriteLine($"{s} warm up finished in {st.ElapsedMilliseconds}ms");
                st.Reset();
            }

            warmLoop /= 10;
            foreach (var s in args)
            {
                Console.WriteLine();
                var st = new Stopwatch();
                Console.WriteLine($"re-arming up {s} with {warmLoop} iterations");
                st.Start();
                await GetSync(s, warmLoop);
                await GetAsync(s, warmLoop);
                st.Stop();
                Console.WriteLine($"{s} re-warm up finished in {st.ElapsedMilliseconds}ms");
                st.Reset();

                Console.WriteLine();
                Console.WriteLine($"Starting fullSync run for {s} with {testLoop} iterations");
                st.Start();
                await GetSync(s, testLoop);
                st.Stop();
                Console.WriteLine($"{s} fullSync run finished in {st.ElapsedMilliseconds}ms");
                results.Add(s + "_fullSync", st.ElapsedMilliseconds.ToString());
                st.Reset();

                Console.WriteLine($"Starting fullAsync run for {s} with {testLoop} iterations");
                st.Start();
                await GetAsync(s, testLoop);
                st.Stop();
                Console.WriteLine($"{s} fullAsync run finished in {st.ElapsedMilliseconds}ms");
                results.Add(s + "_fullAsync", st.ElapsedMilliseconds.ToString());
                st.Reset();
            }

            Console.WriteLine();
            Console.WriteLine("Version\t Test\t\t Time\t\t TPS");
            foreach (var result in results)
            {
                double r = (double)long.Parse(result.Value) / (double) testLoop;
                var tps = 1000 / r;
                Console.WriteLine($"{result.Key.Split("_")[0]}\t {result.Key.Split("_")[1]}\t {result.Value}\t\t {tps}");
            }
        }

        private static Task GetAsync(string s, int loopVal) => Get(s, loopVal, "Async");
        private static Task GetSync(string s, int loopVal) => Get(s, loopVal, "Sync");
        private static async Task Get(string s, int loopVal, string ss)
        {
            var loop = 0;
            var factory = new HttpClientHandler()
            {
                CheckCertificateRevocationList = false,
                ServerCertificateCustomValidationCallback = (message, certificate2, arg3, arg4) => true
            };

            var client = new HttpClient(factory)
            {
                //Timeout = TimeSpan.FromSeconds(1)
            };

            do
            {
                var message = new HttpRequestMessage(HttpMethod.Get, $"https://{s}/Test/{ss}");
                var r = await client.SendAsync(message);
                if (!r.IsSuccessStatusCode)
                    Console.WriteLine($"{s} failed with {loop} req left, with {r.StatusCode}");

            } while (++loop < loopVal);
        }
    }
}
