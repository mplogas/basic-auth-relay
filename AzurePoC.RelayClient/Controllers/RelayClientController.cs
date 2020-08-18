using AzurePoC.Library.Infrastructure.BasicAuth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzurePoC.RelayClient.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RelayClientController
    {
        private readonly ILogger<RelayClientController> logger;

        public RelayClientController(ILogger<RelayClientController> logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// API requires Basic auth
        /// </summary>
        /// <returns></returns>
        [HttpGet("basic")]
        [BasicAuth] // You can optionally provide a specific realm --> [BasicAuth("my-realm")]
        public IEnumerable<int> BasicAuth()
        {
            this.logger.LogInformation("basic auth relay");
            var rng = new Random();
            return Enumerable.Range(1, 10).Select(x => rng.Next(0, 100));
        }

        
    }
}
