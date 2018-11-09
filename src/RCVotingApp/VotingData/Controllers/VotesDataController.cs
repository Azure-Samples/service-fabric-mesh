using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;

namespace VotingData.Controllers
{
    [Route("api/[controller]")]
    public class VotesDataController : Controller
    {
        private readonly static string VOTES_DICT = "votes_data";
        private readonly ILogger _logger;
        private DateTime _timer;
        private IReliableStateManager _stateManager;
        public VotesDataController(ILogger<VotesDataController> logger, IReliableStateManager stateManager)
        {
            _logger = logger;
            _stateManager = stateManager;
        }

        // GET api/votesdata/all/name
        // name is for partition only
        [HttpGet("all/{voter}")]
        public async Task<IActionResult> Get()
        {
            _timer = DateTime.Now;
            _logger.LogInformation("Getting votes");

            Dictionary<string, int> _votes = new Dictionary<string, int>();
            using (ITransaction txn = this._stateManager.CreateTransaction())
            {
                var votes = await this._stateManager.GetOrAddAsync<IReliableDictionary<string, int>>(VOTES_DICT);
                var enumerable = await votes.CreateEnumerableAsync(txn);
                var enumerator = enumerable.GetAsyncEnumerator();
                while (await enumerator.MoveNextAsync(HttpContext.RequestAborted))
                {
                    KeyValuePair<string, int> tuple = enumerator.Current;
                    _votes.Add(tuple.Key, tuple.Value);
                }
                await txn.CommitAsync();
            }

            _logger.LogInformation($"Returning votes in { DateTime.Now.Millisecond - _timer.Millisecond }ms");
            return Json(_votes);
        }

        // PUT api/votesdata/votes/name
        [HttpPut("votes/{name}")]
        public async Task<IActionResult> Put(string name)
        {
            _timer = DateTime.Now;
            _logger.LogInformation("Saving vote");
            var _key = name.ToLower();
            using (ITransaction txn = this._stateManager.CreateTransaction())
            {
                var _votes = await this._stateManager.GetOrAddAsync<IReliableDictionary<string, int>>(VOTES_DICT);
                bool containsKey = await _votes.ContainsKeyAsync(txn, _key);
                await _votes.AddOrUpdateAsync(txn, _key, 1, (key, value) => value + 1);
                if (!containsKey)
                    Console.WriteLine($"Created vote option {_key} and voted successfully..");
                else
                    Console.WriteLine($"Voting for {_key}...");
                await txn.CommitAsync();
            }

            _logger.LogInformation($"Saved vote in { DateTime.Now.Millisecond - _timer.Millisecond }ms");

            return new OkResult();
        }

        // DELETE api/votesdata/votes/name
        [HttpDelete("votes/{name}")]
        public async Task<IActionResult> Delete(string name)
        {
            _timer = DateTime.Now;
            _logger.LogInformation("Delete vote option");

            bool containsKey = false;
            using (ITransaction txn = this._stateManager.CreateTransaction())
            {
                var _votes = await this._stateManager.GetOrAddAsync<IReliableDictionary<string, int>>(VOTES_DICT);
                containsKey = await _votes.ContainsKeyAsync(txn, name.ToLower());
                if (!containsKey)
                {
                    Console.WriteLine($"Didn't find vote option {name}...");
                }
                else
                {
                    Console.WriteLine($"Removed vote option {name}...");
                    await _votes.TryRemoveAsync(txn, name.ToLower());
                }
                await txn.CommitAsync();
            }

            if (!containsKey)
                return new NotFoundObjectResult(name);
            _logger.LogInformation($"Deleted vote option { DateTime.Now.Millisecond - _timer.Millisecond }ms");

            return new OkResult();
        }
    }
}