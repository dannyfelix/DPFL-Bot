using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DPFL.Bot.Services;

namespace DPFL.Bot.Commands
{
    
    // Commands for Audio functionality, all functionality defined in AudioService.cs
    public class AudioCommands : ModuleBase
    {
        public AudioService AudioService { get; set; }

        [Command("Join")]
        public async Task Join() => 
            await ReplyAsync(embed: await AudioService.JoinAsync(Context.Guild, Context.User as IVoiceState, Context.Channel as ITextChannel));

        [Command("Play")]
        public async Task Play([Remainder] string search) =>
            await ReplyAsync(embed: await AudioService.PlayAsync(Context.User as SocketGuildUser, Context.Guild, search));
        
        [Command("Leave")]
        public async Task Leave() => await ReplyAsync(embed: await AudioService.LeaveAsync(Context.Guild));

        [Command("Pause")]
        public async Task Pause() => await ReplyAsync(embed: await AudioService.PauseAsync(Context.Guild));
        
        [Command("Resume")]
        public async Task Resume() => await ReplyAsync(embed: await AudioService.ResumeAsync(Context.Guild));
        
        [Command("Skip")]
        public async Task Skip() => await ReplyAsync(embed: await AudioService.SkipAsync(Context.Guild));
        
    }
}