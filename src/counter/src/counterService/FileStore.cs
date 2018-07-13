// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace Microsoft.ServiceFabricMesh.Samples.Counter.Service
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Threading;

    public class FileStore : IDisposable
    {
        private const string StoreRootFolderEnvVar = "STORE_ROOT";
        private const string StateFolderNameEnvVar = "Fabric_Id";
        private const string StoreCleanupEnabledEnvVar = "STORE_CLEANUP_ENABLED";
        private const string StoreCleanupInternalEnvVar = "STORE_CLEANUP_INTERVAL_MINUTES";
        private const string StoreCleanupStaleFolderIntervalEnvVar = "STORE_CLEANUP_STALE_FOLDER_INTERVAL_MINUTES";

        private const string StoreRootFolderName = "data";

        private bool disposedValue = false;
        private Timer cleanupTimer;
        private readonly string storeRootPath;
        private readonly CleanupSettings cleanupSettings;

        public FileStore(bool createIfDoesNotExist = false)
            : this(createIfDoesNotExist, GetStoreRootPath(), GetCleanupSettings())
        {
        }

        public FileStore(
            bool createIfDoesNotExist,
            string storeRootPath,
            CleanupSettings cleanupSettings)
        {
            this.storeRootPath = storeRootPath;

            if (createIfDoesNotExist && !Directory.Exists(storeRootPath))
            {
                Directory.CreateDirectory(storeRootPath);
            }

            this.cleanupSettings = cleanupSettings;
            if (this.cleanupSettings != CleanupSettings.None)
            {
                this.cleanupTimer = new Timer(
                   new TimerCallback(CleanupStore),
                   null,
                   Timeout.InfiniteTimeSpan,
                   Timeout.InfiniteTimeSpan);

                this.CleanupStore(null);
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        public string GetStateFolder()
        {
            var subFolderName = Environment.GetEnvironmentVariable(StateFolderNameEnvVar);
            if (null == subFolderName)
            {
                subFolderName = Guid.NewGuid().ToString();
            }

            var stateFolderPath = Path.Combine(this.storeRootPath, subFolderName);
            return stateFolderPath;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (this.cleanupTimer != null)
                    {
                        this.cleanupTimer.Dispose();
                    }
                }

                disposedValue = true;
            }
        }

        private void CleanupStore(object state)
        {
            CleanupStore(this.storeRootPath, DateTime.UtcNow.Subtract(this.cleanupSettings.StaleFolderInterval));

            if (!this.disposedValue)
            {
                this.cleanupTimer.Change(this.cleanupSettings.CleanupInterval, Timeout.InfiniteTimeSpan);
            }
        }

        private static string GetStoreRootPath()
        {
            var storeRootPath = Environment.GetEnvironmentVariable(StoreRootFolderEnvVar);
            if (null == storeRootPath)
            {
                var codeFolderFullPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                storeRootPath = Path.Combine(codeFolderFullPath, StoreRootFolderName);
            }

            return storeRootPath;
        }

        private static CleanupSettings GetCleanupSettings()
        {
            if (!bool.TryParse(Environment.GetEnvironmentVariable(StoreCleanupStaleFolderIntervalEnvVar), out bool storeCleanupEnabled))
            {
                storeCleanupEnabled = true;
            }

            if (storeCleanupEnabled)
            {
                if (!int.TryParse(Environment.GetEnvironmentVariable(StoreCleanupInternalEnvVar), out int storeCleanupInternalMinutes))
                {
                    storeCleanupInternalMinutes = 1;
                }

                if (!int.TryParse(Environment.GetEnvironmentVariable(StoreCleanupStaleFolderIntervalEnvVar), out int storeCleanupStaleFolderIntervalMinutes))
                {
                    storeCleanupStaleFolderIntervalMinutes = 2;
                }

                return new CleanupSettings(
                    TimeSpan.FromMinutes(storeCleanupInternalMinutes),
                    TimeSpan.FromMinutes(storeCleanupStaleFolderIntervalMinutes));
            }
            else
            {
                return CleanupSettings.None;
            }
        }

        private static void CleanupStore(string storeRootPath, DateTime notModifiedSinceUtc)
        {
            try
            {
                foreach (var stateFolder in Directory.GetDirectories(storeRootPath))
                {
                    try
                    {
                        if (GetLastModifiedTimeUtc(stateFolder).CompareTo(notModifiedSinceUtc) < 0)
                        {
                            Console.WriteLine("The folder {0} is stale, deleting it.", stateFolder);
                            Directory.Delete(stateFolder, true);
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Error {1} in deleting folder {0}", stateFolder, e.ToString());
                    }
                }
            }
            catch (Exception e1)
            {
                Console.WriteLine("Error {0} in cleaning up store.", e1.ToString());
            }
        }

        private static DateTime GetLastModifiedTimeUtc(string folderPath)
        {
            var folderLastModifiedTimeUtc = Directory.GetLastWriteTimeUtc(folderPath);
            foreach (var filePath in Directory.GetFiles(folderPath))
            {
                var fileLastModifiedTimeUtc = File.GetLastWriteTimeUtc(filePath);
                if (folderLastModifiedTimeUtc.CompareTo(fileLastModifiedTimeUtc) < 0)
                {
                    folderLastModifiedTimeUtc = fileLastModifiedTimeUtc;
                }
            }

            foreach (var subDirPath in Directory.GetDirectories(folderPath))
            {
                var subDirLastModifiedTimeUtc = GetLastModifiedTimeUtc(subDirPath);
                if (folderLastModifiedTimeUtc.CompareTo(subDirLastModifiedTimeUtc) < 0)
                {
                    folderLastModifiedTimeUtc = subDirLastModifiedTimeUtc;
                }
            }

            return folderLastModifiedTimeUtc;
        }

        public class CleanupSettings
        {
            public static CleanupSettings None = null;

            public CleanupSettings(TimeSpan cleanupInterval, TimeSpan stateFolderInterval)
            {
                this.CleanupInterval = cleanupInterval;
                this.StaleFolderInterval = stateFolderInterval;
            }

            public TimeSpan CleanupInterval { get; private set; }

            public TimeSpan StaleFolderInterval { get; private set; }
        }
    }
}
