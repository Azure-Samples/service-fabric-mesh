// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace VisualObjects.Worker
{
    using Microsoft.ServiceFabric.Data;
    using Microsoft.ServiceFabric.Data.Collections;
    using Microsoft.ServiceFabric.Mesh.Data.Collections;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using VisualObjects.Common;

    internal class RCStateStore : IStateStore
    {
        private readonly static string VISUAL_OBJECT_STORE = "VisualObjectStore";

        public RCStateStore()
        {
        }

        public async Task<VisualObject> ReadAsync(CancellationToken cancellationToken)
        {
            VisualObject visualObject = null;
            IReliableStateManager stateManager = ReliableCollectionsExtensions.GetReliableStateManager();
            if (stateManager == null)
            {
                Console.WriteLine($"Failed to get StateManager");
                return VisualObject.CreateRandom(Guid.NewGuid().ToString());
            }

            using (var txn = stateManager.CreateTransaction())
            {
                var visualObjectStore = await stateManager.GetOrAddAsync<IReliableDictionary<string, VisualObject>>(VISUAL_OBJECT_STORE);

                var enumerable = await visualObjectStore.CreateEnumerableAsync(txn);
                var enumerator = enumerable.GetAsyncEnumerator();

                while (await enumerator.MoveNextAsync(cancellationToken))
                {
                    visualObject = enumerator.Current.Value;
                }
                if (visualObject == null)
                {
                    Console.WriteLine($"Visual object does not exist. Creating new object.");
                    visualObject = VisualObject.CreateRandom(Guid.NewGuid().ToString());
                    await visualObjectStore.AddAsync(txn, visualObject.Name, visualObject);
                }
                await txn.CommitAsync();
            }

            return visualObject;
        }

        public async Task WriteAsync(VisualObject state, CancellationToken cancellationToken)
        {
            IReliableStateManager stateManager = ReliableCollectionsExtensions.GetReliableStateManager();
            if (stateManager == null)
            {
                Console.WriteLine($"Failed to get StateManager");
                return;
            }

            using (var txn = stateManager.CreateTransaction())
            {
                var visualObjectStore = await stateManager.GetOrAddAsync<IReliableDictionary<string, VisualObject>>(VISUAL_OBJECT_STORE);
                await visualObjectStore.AddOrUpdateAsync(txn, state.Name, state, (k, v) => state);
                await txn.CommitAsync();
            }
        }
    }
}
