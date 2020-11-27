using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace net50.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        public TestController()
        {
        }

        [HttpGet]
        [Route("sync")]
        public string GetSync()
        {
            return "I'm here!";
        }

        [HttpGet]
        [Route("async")]
        public async Task<string> GetAsync()
        {
            return await Task.FromResult("I'm here!");
        }
    }
}
