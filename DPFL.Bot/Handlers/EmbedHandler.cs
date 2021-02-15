using System.Threading.Tasks;
using Discord;

namespace DPFL.Bot.Handlers
{
    public static class EmbedHandler
    {
        // Simple embed builder to save time rather than building new ones each time they are required
        public static async Task<Embed> CreateDefaultEmbed(string title, string description, Color color)
        {
            var embed = await Task.Run(() => new EmbedBuilder()
                .WithTitle(title)
                .WithDescription(description)
                .WithColor(color)
                .WithCurrentTimestamp().Build());
            return embed;
        }

        public static async Task<Embed> CreateErrorEmbed(string source, string error)
        {
            var embed = await Task.Run(() => new EmbedBuilder()
                .WithTitle($"Error: {source}")
                .WithDescription($"Error Details: {error}")
                .WithColor(Color.DarkRed)
                .WithCurrentTimestamp().Build());
            return embed;
        }
    }
}