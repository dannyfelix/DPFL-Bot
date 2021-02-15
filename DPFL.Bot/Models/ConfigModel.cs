using System.Collections.Generic;
using Newtonsoft.Json;

namespace DPFL.Bot.Models
{
    public class ConfigModel
    {
        public string Prefix { get; set; }
        public DiscordConfig Discord { get; init; }
        
    }

    public class DiscordConfig
    {
        public string Token { get; set; } = "";
    }
}