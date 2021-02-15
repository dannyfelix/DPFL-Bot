using System;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using DPFL.Bot.Configurations;
using Microsoft.Extensions.Configuration;

namespace DPFL.Bot.Handlers
{
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
            _discord.MessageReceived += OnMessageReceivedAsync;
        }

        private async Task OnMessageReceivedAsync(SocketMessage s)
        {
            /* Previous iteration of next line
            var msg = s as SocketUserMessage;
            if (msg is null) return;        */
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