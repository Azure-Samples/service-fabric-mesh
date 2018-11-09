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

        private readonly string stateFilePath;
        private readonly string stateFolderPath;

        private bool disposed = false;
        private long value;


        public Counter()
            : this(GetCounterUpdateInterval())
        {
        }

        public Counter(TimeSpan updateInterval)
        {
            this.UpdateInterval = updateInterval;
            this.stateFolderPath = FileStoreUtility.GetStateFolderPath("counter");

            if (!Directory.Exists(this.stateFolderPath))
            {
                Directory.CreateDirectory(this.stateFolderPath);
            }

            this.stateFilePath = Path.Combine(this.stateFolderPath, "counter.txt");
            if (!File.Exists(this.stateFilePath))
            {
                this.value = 0;
                WriteCounterValue(this.stateFilePath, this.value);
            }
            else
            {
                this.value = ReadCounterValue(this.stateFilePath);
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
                var currentValue = ReadCounterValue(this.stateFilePath);
                if (currentValue != -1)
                {
                    currentValue++;
                    WriteCounterValue(this.stateFilePath, currentValue);
                    Interlocked.Exchange(ref this.value, currentValue);
                }

                await Task.Delay(this.UpdateInterval);
            }
        }

        private static long ReadCounterValue(string stateFilePath)
        {
            try
            {
                if (!long.TryParse(File.ReadAllText(stateFilePath), out long value))
                {
                    value = -1;
                }

                return value;
            }
            catch(Exception e)
            {
                Console.WriteLine("Error {0} in reading counter value from file {1}.", e.Message, stateFilePath);
                return -1;
            }
        }

        private static void WriteCounterValue(string stateFilePath, long value)
        {
            try
            {
                File.WriteAllText(stateFilePath, value.ToString());
            }
            catch(Exception e)
            {
                Console.WriteLine("Error {0} in writer counter value to file {1}.", e.Message, stateFilePath);
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
