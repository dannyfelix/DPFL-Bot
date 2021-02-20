using System.Threading.Tasks;
using Discord;

namespace DPFL.Bot.Handlers
{
    // Simplistic embed builders to save time rather than building new ones each time they are required.
    public static class EmbedHandler
    {
       
        public static async Task<Embed> CreateDefaultEmbed(string title, string description, Color color)
        {
            var embed = await Task.Run(() => new EmbedBuilder()
                .WithTitle(title)
                .WithDescription(description)
                .WithColor(color).Build());
            return embed;
        }

        public static async Task<Embed> CreateErrorEmbed(string source, string error)
        {
            var embed = await Task.Run(() => new EmbedBuilder()
                .WithTitle($"Error: {source}")
                .WithDescription($"Error Details: {error}")
                .WithColor(Color.DarkRed).Build());
            return embed;
        }
    }
}