using System;
using System.IO;
using Newtonsoft.Json;

namespace RuntimeSync
{
    public class Config
    {
        public string WebhookUrl { get; set; }
        public bool SendSystemInfo { get; set; }
        public bool Installed { get; set; }
    }

    public static class ConfigManager
    {
        private static readonly string AppDataPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "RuntimeSync");
        private static readonly string ConfigPath = Path.Combine(AppDataPath, "config.json");

        public static Config LoadConfig()
        {
            if (!File.Exists(ConfigPath))
                return new Config { SendSystemInfo = true, Installed = false, WebhookUrl = "" };

            var json = File.ReadAllText(ConfigPath);
            return JsonConvert.DeserializeObject<Config>(json);
        }

        public static void SaveConfig(Config config)
        {
            Directory.CreateDirectory(AppDataPath);
            var json = JsonConvert.SerializeObject(config, Formatting.Indented);
            File.WriteAllText(ConfigPath, json);
        }

        public static string GetInstallPath()
        {
            return Path.Combine(AppDataPath, "RuntimeSync.exe");
        }
    }
}