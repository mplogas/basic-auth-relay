using AzurePoC.Library.Infrastructure.BasicAuth;
using AzurePoC.Library.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AzurePoC.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ValuesController : ControllerBase
    {
        private readonly ILogger<ValuesController> logger;
        private readonly IUserService userService;
        private readonly HttpClient client;
        private readonly string remotepath;

        public ValuesController(ILogger<ValuesController> logger, IConfiguration config, IHttpClientFactory httpClientFactory)
        {
            this.logger = logger;
            this.userService = userService;
            this.client = httpClientFactory.CreateClient();
            this.client.BaseAddress = new Uri(config.GetValue<string>("relayclient:baseuri"));
            this.remotepath = config.GetValue<string>("relayclient:path");
        }

        /// <summary>
        /// API allows anonymous
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public IEnumerable<int> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 3).Select(x => rng.Next(0, 100));
        }

        /// <summary>
        /// API requires Basic auth
        /// </summary>
        /// <returns></returns>
        [HttpGet("basic")]
        [BasicAuth] // You can optionally provide a specific realm --> [BasicAuth("my-realm")]
        public IEnumerable<int> BasicAuth()
        {
            this.logger.LogInformation("basic auth");
            var rng = new Random();
            return Enumerable.Range(1, 10).Select(x => rng.Next(0, 100));
        }

        /// <summary>
        /// API requires Basic auth
        /// </summary>
        /// <returns></returns>
        [HttpGet("basicrelay")]
        [BasicAuth] // You can optionally provide a specific realm --> [BasicAuth("my-realm")]
        public async Task<IActionResult> BasicAuthRelay()
        {
            this.logger.LogInformation("basic auth relay");

            string authHeader = HttpContext.Request.Headers["Authorization"];
            if (authHeader != null)
            {
                var authHeaderValue = AuthenticationHeaderValue.Parse(authHeader);
                if (authHeaderValue.Scheme.Equals(AuthenticationSchemes.Basic.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    //we have a good header
                    this.client.DefaultRequestHeaders.Authorization = authHeaderValue;
                    var response = await this.client.GetAsync(this.remotepath);

                    response.EnsureSuccessStatusCode();

                    var result = await response.Content.ReadAsStringAsync();
                    return Ok(result);
                }
            }

            return BadRequest();

        }

        [HttpGet("basic-logout")]
        [BasicAuth]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult BasicAuthLogout()
        {
            this.logger.LogInformation("basic auth logout");
            // NOTE: there's no good way to log out basic authentication. This method is a hack.
            HttpContext.Response.Headers["WWW-Authenticate"] = "Basic realm=\"My Realm\"";
            return new UnauthorizedResult();
        }
    }
}
