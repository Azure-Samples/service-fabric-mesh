// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace Microsoft.ServiceFabricMesh.Samples.Counter.Service
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;

    public class Counter : IDisposable
    {
        private const string CounterUpdateInternalEnvVar = "COUNTER_UPDATE_INTERVAL_SECONDS";

        private long value;
        private readonly FileStore store;
        private readonly string dataFilePath;
        private bool disposed = false;
        private readonly bool storeCreated = false;

        public Counter(FileStore store = null)
            : this(store ?? new FileStore(), GetCounterUpdateInterval())
        {
            this.storeCreated = true;
        }

        public Counter(FileStore store, TimeSpan updateInterval)
        {
            this.store = store;
            this.UpdateInterval = updateInterval;
            var stateFolder = store.GetStateFolder();

            if (!Directory.Exists(stateFolder))
            {
                Directory.CreateDirectory(stateFolder);
            }

            this.dataFilePath = Path.Combine(stateFolder, "counter.txt");
            if (File.Exists(this.dataFilePath))
            {
                if (!long.TryParse(File.ReadAllText(this.dataFilePath), out this.value))
                {
                    this.value = 0;
                }
            }
            else
            {
                this.value = 0;
            }

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            this.StartUpdateAsync();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        public TimeSpan UpdateInterval { get; }

        public string GetValueJson()
        {
            return string.Format(CultureInfo.InvariantCulture, "{{\"value\" : {0} }}", GetValue());
        }

        public long GetValue()
        {
            return Interlocked.Read(ref this.value);
        }

        private async Task StartUpdateAsync()
        {
            while (!this.disposed)
            {
                var counterValue = Interlocked.Increment(ref this.value);
                File.WriteAllText(this.dataFilePath, counterValue.ToString());
                await Task.Delay(this.UpdateInterval);
            }
        }

        private static TimeSpan GetCounterUpdateInterval()
        {
            if (!int.TryParse(Environment.GetEnvironmentVariable(CounterUpdateInternalEnvVar), out int counterUpdateInternalSeconds))
            {
                counterUpdateInternalSeconds = 1;
            }

            return TimeSpan.FromSeconds(counterUpdateInternalSeconds);
        }

        
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (this.storeCreated)
                    {
                        this.store.Dispose();
                    }
                }

                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
