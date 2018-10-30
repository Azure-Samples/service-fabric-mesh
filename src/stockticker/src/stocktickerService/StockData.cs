// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace Microsoft.ServiceFabricMesh.Samples.Stockticker.Service
{
    public class StockData
    {
        public StockData()
        {
            this.Symbol = "Unknown";
            this.CompanyName = "Unknown";
            this.LastKnownValue = "0.0";
        }

        public string Symbol;
        public string CompanyName;
        public string LastKnownValue;
    }

}
