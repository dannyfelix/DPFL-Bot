using System;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Victoria;
using DPFL.Bot.Handlers;
using Victoria.Enums;
using Victoria.EventArgs;

namespace DPFL.Bot.Services
{
    public class AudioService
    {
        private readonly LavaNode _lavaNode;

        public AudioService(LavaNode lavaNode) => _lavaNode = lavaNode;

        public async Task<Embed> JoinAsync(IGuild guild, IVoiceState voiceState, ITextChannel textChannel)
        {
            if (_lavaNode.HasPlayer(guild))
            {
                return await EmbedHandler.CreateErrorEmbed("Join", "I'm already connected to a voice channel.");
            }

            if (voiceState.VoiceChannel is null)
            {
                return await EmbedHandler.CreateErrorEmbed("Join", "You must be connected to a voice channel.");
            }

            try
            {
                await _lavaNode.JoinAsync(voiceState.VoiceChannel, textChannel);
                return await EmbedHandler.CreateDefaultEmbed("Join", $"Joined {voiceState.VoiceChannel.Name}.", Color.Green);
            }
            catch (Exception ex)
            {
                return await EmbedHandler.CreateErrorEmbed("Join", ex.Message);
            }
        }
        
        public async Task<Embed> PlayAsync(SocketGuildUser user, IGuild guild, string query)
        {
            if (user.VoiceChannel is null)
            {
                return await EmbedHandler.CreateErrorEmbed("Play", "You must be connected to a voice channel.");
            }

            if (!_lavaNode.HasPlayer(guild))
            {
                return await EmbedHandler.CreateErrorEmbed("Play", "I'm already connected to a voice channel");
            }

            try
            {
                var player = _lavaNode.GetPlayer(guild);
                var search = Uri.IsWellFormedUriString(query, UriKind.Absolute) ?
                    await _lavaNode.SearchAsync(query) : await _lavaNode.SearchYouTubeAsync(query);

                if (search.LoadStatus == LoadStatus.NoMatches)
                {
                    return await EmbedHandler.CreateErrorEmbed("Search", $"Could not find: {query}");
                }

                var track = search.Tracks.FirstOrDefault();

                // adds track to queue if there is a song already playing / paused.
                if (player.Track != null && player.PlayerState is PlayerState.Playing || player.PlayerState is PlayerState.Paused)
                {
                    player.Queue.Enqueue(track);
                    return await EmbedHandler.CreateDefaultEmbed("Music", $"Queued: {track.Title}", Color.Green);
                }

                await player.PlayAsync(track);
                return await EmbedHandler.CreateDefaultEmbed("Music", $"Now Playing : {track.Title}", Color.Green);
            }
            catch (Exception ex)
            {
                return await EmbedHandler.CreateErrorEmbed("Play", ex.Message);
            }
        }

        public async Task<Embed> LeaveAsync(IGuild guild)
        {
            try
            {
                var player = _lavaNode.GetPlayer(guild);
                if (player.PlayerState is PlayerState.Playing)
                {
                    await player.StopAsync();
                }

                await _lavaNode.LeaveAsync(player.VoiceChannel);

                return await EmbedHandler.CreateDefaultEmbed("Leave", "Successfully disconnected.", Color.Green);
            }
            catch (InvalidOperationException ex)
            {
                return await EmbedHandler.CreateErrorEmbed("Leave", ex.Message);
            }
        }

        public async Task<Embed> PauseAsync(IGuild guild)
        {
            try
            {
                var player = _lavaNode.GetPlayer(guild);
                if (!(player.PlayerState is PlayerState.Playing))
                {
                    await player.PauseAsync();
                    return await EmbedHandler.CreateErrorEmbed("Pause", "Nothing is playing.");
                }

                var currentTrack = player.Track.Title;
                await player.PauseAsync();
                return await EmbedHandler.CreateDefaultEmbed("Pause", $"{currentTrack} is now paused.", Color.Green);

            }
            catch (InvalidOperationException ex)
            {
                return await EmbedHandler.CreateErrorEmbed("Pause", ex.Message);
            }
        }
        
        public async Task<Embed> ResumeAsync(IGuild guild)
        {
            try
            {
                var player = _lavaNode.GetPlayer(guild);
                if (player.PlayerState is PlayerState.Paused)
                {
                    await player.ResumeAsync();
                }
                
                return await EmbedHandler.CreateDefaultEmbed("Resume", $"Resumed playing: {player.Track.Title}", Color.Green);

            }
            catch (InvalidOperationException ex)
            {
                return await EmbedHandler.CreateErrorEmbed("Resume", ex.Message);
            }
        }

        public async Task<Embed> SkipAsync(IGuild guild)
        {
            try
            {
                var player = _lavaNode.GetPlayer(guild);

                if (player is null)
                {
                    return await EmbedHandler.CreateErrorEmbed("Skip", "Player not found, you must be using the bot to use this command");
                }
                
                var currentTrack = player.Track;

                await player.SkipAsync();
                return await EmbedHandler.CreateDefaultEmbed("Skip", $"Successfully skipped {currentTrack}", Color.Green);
            }
            catch (Exception ex)
            {
                return await EmbedHandler.CreateErrorEmbed("Skip", ex.Message);
            }
        }

        public async Task TrackEnded(TrackEndedEventArgs args)
        {
            if (!args.Reason.ShouldPlayNext()) return;
            if (!args.Player.Queue.TryDequeue(out var queueable)) return;

            if (!(queueable is LavaTrack track))
            {
                await args.Player.TextChannel.SendMessageAsync("The following item in the queue is not a track.");
                return;
            }

            await args.Player.PlayAsync(track);
            await args.Player.TextChannel.SendMessageAsync(
                embed: await EmbedHandler.CreateDefaultEmbed("Now Playing", $"[{track.Title}]({track.Url})", Color.Green));

        }
    }
}