// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace VisualObjects.Worker
{
    using Microsoft.ServiceFabric.Mesh.Data.Collections;
    using System.Threading;

    class Program
    {
        static void Main(string[] args)
        {
            ReliableCollectionsExtensions.UseReliableCollectionsService("RCWorkerType");
            Mover.MoveAsync(new RCStateStore(), CancellationToken.None).Wait();
        }
    }
}
