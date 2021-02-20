using System;
using System.IO;
using System.Threading;
using DPFL.Bot.Models;
using Newtonsoft.Json;

namespace DPFL.Bot.Configurations
{
    // Class for initialising the Config data
    public static class ConfigData
    {
        private const string ConfigFolder = "configs";
        private const string ConfigFile = "config.json";
        public static ConfigModel Config { get; }

        static ConfigData()
        {
            if (!Directory.Exists(ConfigFolder))
            {
                Directory.CreateDirectory(ConfigFolder);
            }

            if (!File.Exists(ConfigFolder + "/" + ConfigFile))
            {
                var json = JsonConvert.SerializeObject(GenerateConfig(), Formatting.Indented);
                File.WriteAllText(ConfigFolder + "/" + ConfigFile, json);

                Console.WriteLine(
                    $"Created new configuration file in {ConfigFolder}/{ConfigFile} (within the build directory) - Please add your API keys before running the bot again.",
                    ConsoleColor.Red);

                Thread.Sleep(10000);
                Environment.Exit(0);
            }
            else
            {
                var json = File.ReadAllText(ConfigFolder + "/" + ConfigFile);
                Config = JsonConvert.DeserializeObject<ConfigModel>(json);
            }
        }

        // Method for giving config.json default values to be serialised.
        private static ConfigModel GenerateConfig() => new()
        {
            Prefix = "`",
            Discord = new DiscordConfig()
        };
    }
}