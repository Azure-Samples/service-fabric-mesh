using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Microsoft.ServiceFabricMesh.Samples.Stockticker.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private StockTicker ticker;

        public ValuesController(StockTicker ticker)
        {
            this.ticker = ticker;
        }

        // GET api/values
        [HttpGet]
        public string Get()
        {
            return this.ticker.GetStockData();
        }

    }
}
