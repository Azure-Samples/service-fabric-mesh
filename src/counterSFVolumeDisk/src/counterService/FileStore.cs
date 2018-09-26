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

    public static class FileStoreUtility 
    {
        private const string StoreRootFolderEnvVar = "STORE_ROOT";
        private const string StateFolderNameEnvVar = "STATE_FOLDER_NAME";
        private const string FabricIdEnvVar = "Fabric_Id";
        private const string StoreRootFolderName = "data";
        
        public static string GetStateFolderPath(string appName = null)
        {
            var storeRootPath = GetStoreRootPath(appName);

            var subFolderName = Environment.GetEnvironmentVariable(StateFolderNameEnvVar);
            if (string.IsNullOrEmpty(subFolderName))
            {
                subFolderName = Environment.GetEnvironmentVariable(FabricIdEnvVar);
            }

            if (string.IsNullOrEmpty(subFolderName))
            {
                subFolderName = Guid.NewGuid().ToString();
            }

            var stateFolderPath = Path.Combine(storeRootPath, subFolderName);
            return stateFolderPath;
        }

        private static string GetStoreRootPath(string appName)
        {
            var storeRootPath = Environment.GetEnvironmentVariable(StoreRootFolderEnvVar);
            if (string.IsNullOrEmpty(storeRootPath))
            {
                var codeFolderFullPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                storeRootPath = Path.Combine(codeFolderFullPath, StoreRootFolderName);
            }

            if (!string.IsNullOrEmpty(appName))
            {
                storeRootPath = Path.Combine(storeRootPath, appName);
            }

            return storeRootPath;
        }
    }
}
