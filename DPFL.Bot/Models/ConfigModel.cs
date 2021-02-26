namespace DPFL.Bot.Models
{
    // POCO for config data
    public class ConfigModel
    {
        public string Prefix { get; set; }
        
        public DiscordConfig Discord { get; init; }

        public LastFmConfig LastFm { get; init; }


    }

    public class DiscordConfig
    {
        public string Token { get; set; } = "";
    }

    public class LastFmConfig
    {
        public string Key { get; set; } = "";

        public string Secret { get; set; } = "";
    }
    
}