using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace VotingWeb.Controllers
{
    [Route("api/[controller]")]
    public class VotesController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger _logger;
        private DateTime _timer;

        private static Uri s_backendUrl = new Uri($"http://{Environment.GetEnvironmentVariable("Voting_BackendHostName")}:{Environment.GetEnvironmentVariable("Voting_BackendHostPort")}/api/votesdata");

        public VotesController(HttpClient httpClient, ILogger<VotesController> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        private const int PARTITION_COUNT = 3;

        // GET api/votes
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            _timer = DateTime.Now;
            _logger.LogInformation("Getting votes");

            var all = new Dictionary<string, int>();
            for (int i = 0; i < PARTITION_COUNT; ++i)
            {
                var result = new Dictionary<string, int>();

                string voter = ((char)(i + (int)'A')).ToString();
                var request = new HttpRequestMessage()
                {
                    RequestUri = new Uri($"{s_backendUrl}/all/{voter}"),
                    Method = HttpMethod.Get,
                };

                using (HttpResponseMessage response = await _httpClient.SendAsync(request))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        result = JsonConvert.DeserializeObject<Dictionary<string, int>>(await response.Content.ReadAsStringAsync());
                    }
                }

                result.ToList().ForEach(x => all.TryAdd(x.Key, x.Value));
            }

            _logger.LogInformation($"Returning votes in { DateTime.Now.Millisecond - _timer.Millisecond }ms");

            return this.Json(all);
        }


        // PUT api/votes/name
        [HttpPut("{name}")]
        public async Task<ContentResult> Put(string name)
        {
            _timer = DateTime.Now;
            _logger.LogInformation("Adding vote");
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri($"{s_backendUrl}/votes/{name}"),
                Method = HttpMethod.Put,
            };

            using (HttpResponseMessage response = await _httpClient.SendAsync(request))
            {

                _logger.LogInformation($"Added vote in { DateTime.Now.Millisecond - _timer.Millisecond }ms");

                return new ContentResult()
                {
                    StatusCode = (int)response.StatusCode,
                    Content = await response.Content.ReadAsStringAsync()
                };
            }
        }

        // DELETE api/votes/name
        [HttpDelete("{name}")]
        public async Task<IActionResult> Delete(string name)
        {
            _timer = DateTime.Now;
            _logger.LogInformation("Adding vote");
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri($"{s_backendUrl}/votes/{name}"),
                Method = HttpMethod.Delete,
            };
            using (HttpResponseMessage response = await _httpClient.SendAsync(request))
            {
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    return this.StatusCode((int)response.StatusCode);
                }
            }

            _logger.LogInformation($"Added vote in { DateTime.Now.Millisecond - _timer.Millisecond }ms");

            return new OkResult();
        }
    }
}

