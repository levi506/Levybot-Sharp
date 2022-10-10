using Discord;
using Sharpbot.Plugins.Audio.Commands;
using Sharpbot.Plugins.Commands.Utility;
using Sharpbot.Services.ApiManager;
using Sharpbot.Services.ApiManager.Apis;
using Sharpbot.Services.ApiManager.Utility.Lavalink;
using Sharpbot.Services.Data.Utility.Extensions;
using Sharpbot.Services.PluginManager;
using Sharpbot.Services.PluginManager.Utility;
using System.Collections.Generic;
using System.Threading.Tasks;
using Victoria;
using Victoria.Enums;

namespace Sharpbot.Plugins.Audio
{

    [PluginMeta("Audio", "Plugin Responsible for controlling Audio Queues and related Player Data")]
    [PPriority]
    public class AudioPlugin : IPlugin
    {
        public Music MusicCommands { get; private set; }
        public const char BarFill = '▰';
        public const char BarEmpty = '▱';
        public const string Position = "🔘";

        public AudioPlugin()
        {
            var CommandPlugin = PluginManager.GetCommandPlugin();
            if (CommandPlugin != null)
            {
                var MusicComs = new Music(this);
                CommandPlugin.LoadCommands(MusicComs);
            }
        }

        internal async Task CreateConnection(CommandRequest req)
        {
            await ApiManager.GetLavalinkClient(req.SourceRequest.DiscordBot);
        }

        internal async Task RegisterConnection(CommandRequest req, IVoiceChannel voice, ITextChannel text)
        {
            var Lavalink = await ApiManager.GetLavalinkClient(req.SourceRequest.DiscordBot);
            await Lavalink.JoinChannel(voice,text);
        }

        internal async Task DisconnectPlayer(CommandRequest req, IVoiceChannel voice)
        {
            var Lavalink = await ApiManager.GetLavalinkClient(req.SourceRequest.DiscordBot);
            Lavalink.LeaveChannel(voice);
        }

        internal async Task SetPlayerVolume(CommandRequest req,IGuild guild,ushort vol)
        {
            var Lavalink = await ApiManager.GetLavalinkClient(req.SourceRequest.DiscordBot);
            Lavalink.SetVolume(guild, vol);
        }

        internal async Task<int> GetPlayerVolume(CommandRequest req, IGuild guild)
        {
            var Lavalink = await ApiManager.GetLavalinkClient(req.SourceRequest.DiscordBot);
            return Lavalink.GetVolume(guild.Id);
        }

        internal async Task<bool> DoesPlayerExist(CommandRequest req,IGuild guild)
        {
            var Lavalink = await ApiManager.GetLavalinkClient(req.SourceRequest.DiscordBot);
            return Lavalink.PlayerExists(guild);
        }

        internal async Task<LavaTrack> QuerySoundcloud(CommandRequest req, string query)
        {
            var Lavalink = await ApiManager.GetLavalinkClient(req.SourceRequest.DiscordBot);
            var track = await Lavalink.QuerySCAsync(query);
            return track;
        }
        internal async Task<LavaTrack> QueryYoutube(CommandRequest req, string query)
        {
            var Lavalink = await ApiManager.GetLavalinkClient(req.SourceRequest.DiscordBot);
            var track = await Lavalink.QueryYTAsync(query);
            return track;
        }
        internal async Task<LavaTrack> QueryAny(CommandRequest req, string query)
        {
            var Lavalink = await ApiManager.GetLavalinkClient(req.SourceRequest.DiscordBot);
            var track = await Lavalink.QueryAudioAsync(query);
            return track;
        }
        internal async Task QueueAudio(CommandRequest req, LavaTrack track,IGuild guild)
        {
            var Lavalink = await ApiManager.GetLavalinkClient(req.SourceRequest.DiscordBot);
            Lavalink.QueueTrack(track, guild);
        }

        internal async Task SkipTrack(CommandRequest req, IGuild guild)
        {
            var Lavalink = await ApiManager.GetLavalinkClient(req.SourceRequest.DiscordBot);
            await Lavalink.SkipAsync(guild);
        }

        internal async Task SetLoopState(CommandRequest req, IGuild guild,LoopingState state)
        {
            var Lavalink = await ApiManager.GetLavalinkClient(req.SourceRequest.DiscordBot);
            Lavalink.SetLoop(guild, state);
        }

        internal async Task<LoopingState> GetLoopState(CommandRequest req, IGuild guild)
        {
            var Lavalink = await ApiManager.GetLavalinkClient(req.SourceRequest.DiscordBot);
            return Lavalink.GetLoop(guild);
        }

        internal async Task ShuffleQueue(CommandRequest req, IGuild guild)
        {
            var Lavalink = await ApiManager.GetLavalinkClient(req.SourceRequest.DiscordBot);
            await Lavalink.ShuffleQueue(guild);
        }

        private async Task<LavaPlayer> GetPlayer(CommandRequest req, IGuild guild)
        {
            var Lavalink = await ApiManager.GetLavalinkClient(req.SourceRequest.DiscordBot);
            var player = Lavalink.GetPlayer(guild);
            return player;
        }

        internal async Task<bool> IsPlaying(CommandRequest req, IGuild guild)
        {
            var player = await GetPlayer(req, guild);
            return player.Track != null;
        }

        internal async Task<LavaTrack> GetPlaying(CommandRequest req, IGuild guild)
        {
            var player = await GetPlayer(req, guild);
            return player.Track;
        }

        internal async Task<List<LavaTrack>> GetQueue(CommandRequest req, IGuild guild)
        {
            var player = await GetPlayer(req, guild);
            return player.Queue.GetTop();

        }

        internal async Task<string> GetLength(CommandRequest req, IGuild guild)
        {
            var player = await GetPlayer(req, guild);
            var QueueTime = player.Queue.GetLength();
            var CurrentTrackTime = player.Track.Duration;
            var aggTime = QueueTime + CurrentTrackTime;
            return aggTime.NumericString();

        }

        internal async Task<string> MakeInfoLine(CommandRequest req, IGuild guild)
        {
            var player = await GetPlayer(req, guild);
            var vol = player.Volume;
            var track = player.Track;
            var loop = await GetLoopState(req, guild);
            var info = "";
            switch (player.PlayerState)
            {
                case PlayerState.Playing:
                    info += "▶️";
                    break;
                case PlayerState.Paused:
                    info += "⏸️";
                    break;
                case PlayerState.Stopped:
                    info += "⏹️";
                    break;
                default:
                    info += "⁉️";
                    break;
            }
            info += " ";

            info += MakeProgressBar(track);

            info += " ";

            if(vol >= 0 && vol < 34)
            {
                info += "🔈";
            }
            else if (vol >= 34 && vol < 67)
            {
                info += "🔉";
            } else if (vol >= 67)
            {
                info += "🔊";
            }

            info += " ";

            switch (loop)
            {
                case LoopingState.Playlist:
                    info += "🔁";
                    break;
                case LoopingState.Song:
                    info += "🔂";
                    break;
                default:
                    break;
            }

            return info;
        }
        
        private string MakeProgressBar(LavaTrack track)
        {
            var progressBar = "";
            var res = (track.Position.TotalSeconds / track.Duration.TotalSeconds) * 100;
            for (var i = 0; i < 10; i++)
            {
                if (res == 0)
                {
                    progressBar += BarEmpty;
                }
                else if (res >= 10)
                {
                    progressBar += BarFill;
                    res -= 10;
                }
                else
                {
                    progressBar += Position;
                    res = 0;
                }
            }
            return progressBar;
        }
    }
}
