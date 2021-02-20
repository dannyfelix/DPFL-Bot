using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DPFL.Bot.Configurations;
using Victoria;

namespace DPFL.Bot.Services
{
    public class StartupService
    {
        private readonly IServiceProvider _provider;
        private readonly DiscordSocketClient _discord;
        private readonly CommandService _commands;
        private readonly LavaNode _lavaNode;
        private readonly AudioService _audioService;
        
        public StartupService(
            IServiceProvider provider,
            DiscordSocketClient discord,
            CommandService commands,
            LavaNode lavaNode,
            AudioService audioService)
        {
            _provider = provider;
            _discord = discord;
            _commands = commands;
            _lavaNode = lavaNode;
            _audioService = audioService;
            
            SubscribeLavaLinkEvents();
        }

        // Login method for bot using Discord Token from config.json, then adding command modules 
        public async Task StartAsync()
        {
            var discordToken = ConfigData.Config.Discord.Token;
            if (string.IsNullOrWhiteSpace(discordToken))
            {
                throw new Exception("No Token Found: \n Please enter your bot token in '/configs/config.json'.");
            }

            Console.WriteLine("Logging into Discord");
            await _discord.LoginAsync(TokenType.Bot, discordToken);
            await _discord.StartAsync();

            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _provider);
        }

        // Method for hooking up LavaLink related events
        private void SubscribeLavaLinkEvents()
        {
            _lavaNode.OnTrackEnded += _audioService.TrackEnded;
        }
    }
}