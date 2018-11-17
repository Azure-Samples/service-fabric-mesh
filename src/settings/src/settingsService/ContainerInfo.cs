// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace Microsoft.ServiceFabricMesh.Samples.Settings.Service
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.NetworkInformation;
    using System.Text;

    public class ContainerInfo
    {
        public IList<InfoItem> genericInfo;

        public IList<InfoItem> environmentVariables;

        public IList<InfoItem> settings;

        private ContainerInfo()
        {
        }

        public static ContainerInfo Get()
        {
            return new ContainerInfo()
            {
                genericInfo = GetGenericInfo(),
                environmentVariables = GetEnvironmentVariables(),
                settings = GetSettings()
            };
        }
        
        private static List<InfoItem> GetGenericInfo()
        {
            var genericInfoList = new List<InfoItem>
            {
                new InfoItem()
                {
                    name = "processId",
                    value = GetProcessId()
                },
                new InfoItem()
                {
                    name = "hostname",
                    value = GetHostName()
                },
                new InfoItem()
                {
                    name = "ipAddresses",
                    value = GetIpAddresses()
                }
            };

            return genericInfoList;
        }

        private static string GetProcessId()
        {
            try
            {
                return Process.GetCurrentProcess().Id.ToString();
            }
            catch (Exception e)
            {
                return $"ERROR: {e.ToString()}";
            }
        }

        private static string GetHostName()
        {
            try
            {
                return Dns.GetHostName();
            }
            catch(Exception e)
            {
                return $"ERROR: {e.ToString()}";
            }
        }

        private static string GetIpAddresses()
        {
            try
            {
                var sb = new StringBuilder();
                foreach(var netInterface in NetworkInterface.GetAllNetworkInterfaces())
                {
                    var ipProperties = netInterface.GetIPProperties();
                    foreach (var ipAddress in ipProperties.UnicastAddresses)
                    {
                        sb.AppendFormat("{0}, ", ipAddress.Address.ToString());
                    }
                }

                var retval = sb.ToString();
                return retval.TrimEnd(new[] { ' ', ',' });
            }
            catch(Exception e)
            {
                return $"ERROR: {e.ToString()}";
            }
        }
        
        private static List<InfoItem> GetEnvironmentVariables()
        {
            var envVarList = new List<InfoItem>();
            var processEnvVariables = Environment.GetEnvironmentVariables();
            foreach (var key in processEnvVariables.Keys)
            {
                envVarList.Add(new InfoItem()
                {
                    name = key.ToString(),
                    value = processEnvVariables[key].ToString()
                });
            }

            envVarList.Sort((s, t) => { return string.Compare(s.name, t.name); });
            return envVarList;
        }

        private static List<InfoItem> GetSettings()
        {
            var settings = new List<InfoItem>();
            var settingsPath = Environment.GetEnvironmentVariable("Fabric_SettingPath");

            if (string.IsNullOrEmpty(settingsPath) || !Directory.Exists(settingsPath))
            {
                return settings;
            }

            var settingsFiles = Directory.GetFiles(settingsPath).OrderBy(f => f);
            foreach (var f in settingsFiles)
            {
                try
                {
                    var name = Path.GetFileName(f);
                    var value = File.ReadAllText(f);
                    settings.Add(new InfoItem() { name = name, value = value });
                }
                catch
                {
                }
            }

            return settings;
        }

        public class InfoItem
        {
            public string name { get; set; }

            public string value { get; set; }
        }
    }
}
