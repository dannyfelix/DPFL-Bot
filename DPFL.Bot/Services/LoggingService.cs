using System;
using System.IO;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace DPFL.Bot.Services
{
    // Simple logging service that hooks to Discord's socket client and command service log events and writes formatted error information to file.
    public class LoggingService
    {
        private readonly DiscordSocketClient _discord;
        private readonly CommandService _commands;

        private string LogDirectory { get; }
        
        private string LogFile => Path.Combine(LogDirectory, $"{DateTime.UtcNow.ToString("yyyy-MM-dd")}.txt");
        
        public LoggingService(DiscordSocketClient discord, CommandService commands)
        {
            LogDirectory = Path.Combine(AppContext.BaseDirectory, "logs" );
            
            _discord = discord;
            _commands = commands;

            _discord.Log += OnLogAsync;
            _commands.Log += OnLogAsync;
        }

        private Task OnLogAsync(LogMessage msg)
        {
            if (!Directory.Exists(LogDirectory))
            {
                Directory.CreateDirectory(LogDirectory);
            }

            if (!File.Exists(LogFile))
            {
                File.Create(LogFile).Dispose();
            }

            var logText = $"{DateTime.UtcNow.ToString("hh:mm:ss")} [{msg.Severity}] {msg.Source}: {msg.Exception?.ToString() ?? msg.Message}";
            File.AppendAllText(LogFile, logText + "\n");

            return Console.Out.WriteLineAsync(logText);
        }
    }
}