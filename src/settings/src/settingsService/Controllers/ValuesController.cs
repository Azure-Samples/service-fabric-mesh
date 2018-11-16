// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace Microsoft.ServiceFabricMesh.Samples.Settings.Service.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;

    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        public ValuesController()
        {
        }

        // GET api/values/containerInfo
        [HttpGet("containerInfo")]
        public string Get()
        {
            return JsonConvert.SerializeObject(ContainerInfo.Get());
        }
    }
}
