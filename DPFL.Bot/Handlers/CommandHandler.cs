using System;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using DPFL.Bot.Configurations;

namespace DPFL.Bot.Handlers
{
    // Class for handling messages for command input, filters out irrelevant messages.
    public class CommandHandler
    {
        private readonly DiscordSocketClient _discord;
        private readonly CommandService _commands;
        private readonly IServiceProvider _provider;

        public CommandHandler(
            DiscordSocketClient discord,
            CommandService commands,
            IServiceProvider provider)
        {
            _discord = discord;
            _commands = commands;
            _provider = provider;
            _discord.MessageReceived += OnMessageReceivedAsync; // Hooks method to MessageReceived event so every message be filtered to see if response is required.
        }

        private async Task OnMessageReceivedAsync(SocketMessage s)
        {
            // Filters to ignore messages from irrelevant sources
            if (!(s is SocketUserMessage msg)) return;
            if (_discord?.CurrentUser != null && msg.Author.Id == _discord.CurrentUser.Id) return;
            if (msg.Author.IsBot) return;
            
            var context = new SocketCommandContext(_discord, msg);

            var argPos = 0;
            if (msg.HasStringPrefix(ConfigData.Config.Prefix, ref argPos) ||
                msg.HasMentionPrefix(_discord.CurrentUser, ref argPos))
            {
                var result = await _commands.ExecuteAsync(context, argPos, _provider);

                if (!result.IsSuccess)
                    await context.Channel.SendMessageAsync(result.ToString());
            }

        }
    }
}