using System;
using System.IO;
using Discord;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using DPFL.Bot.Configurations;
using DPFL.Bot.Handlers;
using DPFL.Bot.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Victoria;


namespace DPFL.Bot
{
    public class Startup
    {
        private IConfigurationRoot Configuration { get; }
        
        public Startup(string[] args)
        {
            var unused = ConfigData.Config; // Declared to create '/configs' directory - can replace with same directory check if statement

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory() + "/configs")
                .AddJsonFile("config.json", true);

            Configuration = builder.Build();
        }
        
        public static async Task RunAsync(string[] args)
        {
            var startup = new Startup(args);
            await startup.RunAsync();
        }

        private async Task RunAsync()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);

            var provider = services.BuildServiceProvider();
            provider.GetRequiredService<LoggingService>();
            provider.GetRequiredService<CommandHandler>();

            await provider.GetRequiredService<StartupService>().StartAsync();
            await Task.Delay(-1);
        }

        // Adds all the services to the service collection
        private void ConfigureServices(IServiceCollection services)
        {
            var discordClient = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Verbose,
                MessageCacheSize = 100
            });

            var commandService = new CommandService(new CommandServiceConfig
            {
                LogLevel = LogSeverity.Verbose,
                DefaultRunMode = RunMode.Async
            });
            
            services
                .AddSingleton(discordClient)
                .AddSingleton(commandService)
                .AddSingleton<CommandHandler>()
                .AddSingleton<StartupService>()
                .AddSingleton<LoggingService>()
                .AddSingleton<LavaNode>()
                .AddSingleton(new LavaConfig())
                .AddSingleton<AudioService>()
                .AddSingleton<Random>()
                .AddSingleton(Configuration);
        }
    }
}