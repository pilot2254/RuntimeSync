using System;
using System.Net;
using System.Runtime.InteropServices;

namespace RuntimeSync
{
    public static class SystemInfo
    {
        public static string GetSystemReport()
        {
            string ip = GetPublicIP();
            string os = RuntimeInformation.OSDescription;
            string machineName = Environment.MachineName;
            string user = Environment.UserName;

            return $@"
**System Info**
OS: {os}
Machine Name: {machineName}
User: {user}
Public IP: {ip}
";
        }

        private static string GetPublicIP()
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    return client.DownloadString("https://ipinfo.io/ip").Trim();
                }
            }
            catch
            {
                return "Could not retrieve IP";
            }
        }
    }
}