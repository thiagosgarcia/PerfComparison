using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Tester
{
    class Program
    {
        static public int WarmLoop { get; set; } = 100;
        static public int TestLopp { get; set; } = 10000;
        static public int Concurrency { get; set; } = 10;
        static public int Delay { get; set; } = 20;
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hi!");
            if (args.Length < 1)
                args = new[] { "-n", "10000", "-c", "10", "net21", "net31", "net50" };
            Console.WriteLine(string.Join(", ", args));

            args = ReadParams(args).ToArray();

            Console.WriteLine($"Waiting {Delay}s until everything is loaded");
            Thread.Sleep(TimeSpan.FromSeconds(Delay));
            var results = new Dictionary<string, string>();
            var st = new Stopwatch();
            foreach (var s in args)
            {
                Console.WriteLine();
                Console.WriteLine($"Warming up {s} with {WarmLoop} iterations");
                st.Restart();
                await GetSync(s);
                Console.WriteLine($"{s} warm up finished in {st.ElapsedMilliseconds}ms");
            }

            foreach (var s in args)
            {
                Console.WriteLine();
                Console.WriteLine($"Starting fullSync run for {s} with {TestLopp} iterations");
                st.Restart();
                await GetSync(s);
                Console.WriteLine($"{s} fullSync run finished in {st.ElapsedMilliseconds}ms");
                results.Add(s + "_fullSync", st.ElapsedMilliseconds.ToString());

                Console.WriteLine($"Starting fullAsync run for {s} with {TestLopp} iterations");
                st.Restart();
                await GetAsync(s);
                Console.WriteLine($"{s} fullAsync run finished in {st.ElapsedMilliseconds}ms");
                results.Add(s + "_fullAsync", st.ElapsedMilliseconds.ToString());
            }

            Console.WriteLine();
            Console.WriteLine("Version\t Test\t\t Concurrency\t Time\t\t TPS");
            foreach (var result in results)
            {
                double r = (double)long.Parse(result.Value) / (double)TestLopp;
                int tps = (int)(1000 / r);
                Console.WriteLine($"{result.Key.Split("_")[0]}\t {result.Key.Split("_")[1]}\t {(result.Key.EndsWith("Async") ? Concurrency : 1)}\t\t {result.Value}\t\t {tps}");
            }
        }

        private static IEnumerable<string> ReadParams(string[] args)
        {
            for (var i = 0; i < args.Count(); i++)
            {
                switch (args[i])
                {
                    case "-n":
                        TestLopp = int.Parse(args[++i]);
                        continue;
                    case "-c":
                        Concurrency = int.Parse(args[++i]);
                        continue;
                    case "-d":
                        Delay = int.Parse(args[++i]);
                        continue;
                    case { } a when a.StartsWith("net"):
                        yield return args[i];
                        continue;
                    default:
                        continue;
                }
            }
        }

        private static Task GetAsync(string s) => Get(s, "Async", Concurrency);
        private static Task GetSync(string s) => Get(s, "Sync", 1);
        private static async Task Get(string s, string ss, int concurrency)
        {
            int loopVal = TestLopp / concurrency;
            var factory = new HttpClientHandler()
            {
                CheckCertificateRevocationList = false,
                ServerCertificateCustomValidationCallback = (message, certificate2, arg3, arg4) => true
            };

            var tasks = new List<Task>();
            do
            {
                for (var i = 0; i < concurrency; i++)
                {
                    tasks.Add(Task.Run(async () =>
                    {
                        var client = new HttpClient(factory);
                        var message = new HttpRequestMessage(HttpMethod.Get, $"https://{s}/Test/{ss}");
                        var r = await client.SendAsync(message);
                        if (!r.IsSuccessStatusCode)
                            Console.WriteLine($"{s} failed with {loopVal} req left, with {r.StatusCode}");
                    }));
                }

                Task.WaitAll(tasks.ToArray());


            } while (0 < --loopVal);
        }
    }
}
