// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace Microsoft.ServiceFabricMesh.Samples.Counter.Service.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private Counter counter;

        public ValuesController(Counter counter)
        {
            this.counter = counter;
        }

        // GET api/values
        [HttpGet]
        public string Get()
        {
            return counter.GetValueJson();
        }
    }
}
