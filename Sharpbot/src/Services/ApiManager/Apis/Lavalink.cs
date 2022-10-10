using Discord;
using Discord.WebSocket;
using Sharpbot.Services.ApiManager.Utility.Lavalink;
using Sharpbot.Services.Data.Utility.Extensions;
using Sharpbot.Services.Logger;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Victoria;
using Victoria.EventArgs;
using Victoria.Responses.Search;

namespace Sharpbot.Services.ApiManager.Apis
{
    public class Lavalink
    {

        public static LavalinkKey GConfig {private get; set;}

        public static ushort DefaultVolume { get; private set; } = 7;
        public Dictionary<ulong,ushort> Volume { get; private set; }
        public Dictionary<ulong, LoopingState> Looping { get; private set; }

        private LavaNode Node { get; set; }
        private LavalinkKey Config { get; set; }
        private Dictionary<string, bool> BrokenTracks { get; set; }


        public Lavalink(BaseSocketClient bot)
        {
            Volume = new Dictionary<ulong, ushort>();
            Looping = new Dictionary<ulong, LoopingState>();
            BrokenTracks = new Dictionary<string, bool>();
            Config = GConfig;
            var config = new LavaConfig();
            config.Hostname = Config.Host;
            config.Port = Config.Port;
            config.SelfDeaf = false;
            config.Authorization = Config.Password;
            if(bot is DiscordShardedClient)
            {
                var tbot = bot as DiscordShardedClient;
                tbot.ShardReady += BotReadyAsync;
                Node = new LavaNode(tbot,config);
            }
            else
            {
                var tbot = bot as DiscordSocketClient;
                tbot.Ready += BotReadyAsync;
                Node = new LavaNode(tbot, config);
            }
            Node.OnLog += LavaNodeLog;
            Node.OnPlayerUpdated += LavaNode_PlayerUpdate;
            Node.OnStatsReceived += LavaNode_StatsReport;
            Node.OnTrackEnded += LavaNode_TrackEnded;
            Node.OnTrackException += LavaNode_TrackException;
            Node.OnTrackStarted += LavaNode_TrackStarted;
            Node.OnTrackStuck += LavaNode_TrackStuck;
            Node.OnWebSocketClosed += LavaNode_SocketClosed;
            Start();
        }

        

        /**
        public Lavalink(BaseSocketClient bot, LavalinkKey configuration)
        {
            Config = configuration;
            var config = new LavaConfig();
            config.Hostname = Config.Host;
            config.Port = Config.Port;
            config.SelfDeaf = false;
            config.Authorization = Config.Password;
            if (bot is DiscordShardedClient)
            {
                var tbot = bot as DiscordShardedClient;
                Node = new LavaNode(tbot, config);
            }
            else
            {
                var tbot = bot as DiscordSocketClient;
                Node = new LavaNode(tbot, config);
            }
            Start();
        }
        */

        private async Task Start()
        {
            await Node.ConnectAsync();
        }

        public async Task<LavaPlayer> JoinChannel(IVoiceChannel vc,ITextChannel text)
        {
            var player = await Node.JoinAsync(vc,text);
            return player;
            
        }

        public async Task LeaveChannel(IVoiceChannel vc)
        {
            if (Volume.TryGetValue(vc.GuildId,out var _))
            {
                Volume.Remove(vc.GuildId);
            }
            if(Looping.TryGetValue(vc.GuildId, out var _))
            {
                Looping.Remove(vc.GuildId);
            }
            await Node.LeaveAsync(vc);
        }

        public void SetVolume(IGuild guild, ushort volume)
        {
            if (Node.TryGetPlayer(guild, out var player)){
                player.UpdateVolumeAsync(volume);
                if (Volume.ContainsKey(guild.Id))
                {
                    Volume.Remove(guild.Id);
                } 
                Volume.Add(guild.Id,volume);
            }
        }

        public void SetLoop(IGuild guild, LoopingState toSet)
        {
            if (Node.TryGetPlayer(guild, out var _))
            {
                if (Looping.ContainsKey(guild.Id))
                {
                    Looping.Remove(guild.Id);
                }
                Looping.Add(guild.Id, toSet);
            }
        }

        internal LoopingState GetLoop(IGuild guild)
        {
            if (Node.TryGetPlayer(guild, out var player))
            {
                LoopingState looping;
                if (!Looping.TryGetValue(player.VoiceChannel.GuildId, out looping))
                {
                    looping = LoopingState.None;
                }
                return looping;
            }
            return LoopingState.None;
        }

        public LavaPlayer GetPlayer(IGuild guild)
        {
            if(Node.TryGetPlayer(guild,out var player))
            {
                return player;
            }
            return null;
        }

        internal Task ShuffleQueue(IGuild guild)
        {
            if (Node.TryGetPlayer(guild, out var player))
            {
                player.Queue.Shuffle();
            }
            return Task.CompletedTask;
        }

        public int GetVolume(ulong guildId)
        {
            if(Volume.TryGetValue(guildId,out var vol))
            {
                return vol;
            }
            return DefaultVolume;
        }

        public bool PlayerExists(IGuild guild)
        {
            return Node.TryGetPlayer(guild, out var _);
        }

        public async Task<LavaTrack> QueryYTAsync(string query)
        {
            var response = await Node.SearchYouTubeAsync(query);
            var track = response.Tracks.First();
            return track;
        }

        public async Task<LavaTrack> QuerySCAsync(string query)
        {
            var response = await Node.SearchSoundCloudAsync(query);
            var track = response.Tracks.First();
            return track;
        }

        public async Task<LavaTrack> QueryAudioAsync(string query)
        {
            var response = await Node.SearchAsync(SearchType.Direct ,query);
            var track = response.Tracks.First();
            return track;
        }

        public async Task SkipAsync(IGuild guild)
        {
            if (Node.TryGetPlayer(guild, out var player))
            {
                await player.StopAsync();
            }
        }

        public async Task QueueTrack(LavaTrack track,IGuild guild)
        {
            if (Node.TryGetPlayer(guild,out var player))
            {
                if(player.Track != null)
                {
                    await TrackAdded(track, player);
                    player.Queue.Enqueue(track);
                } 
                else
                {
                    await player.PlayAsync(track);
                }
            } 
            return;
        }

        private async Task NotifyChannel(LavaTrack track, LavaPlayer player)
        {
            var embed = new EmbedBuilder();
            var bot = await player.VoiceChannel.Guild.GetCurrentUserAsync();
            LoopingState looping;
            if (!Looping.TryGetValue(player.VoiceChannel.GuildId, out looping))
            {
                looping = LoopingState.None;
            }
            embed.WithAuthor(bot.Nickname ?? bot.Username, bot.GetAvatarUrl());
            embed.WithTitle($"{track.Title}");
            embed.WithUrl(track.Url.ToString());
            embed.AddField("Author", track.Author, true);
            string footer = "Playing Now";
            switch (looping)
            {
                case LoopingState.None:
                    footer += "!";
                    break;
                case LoopingState.Playlist:
                    footer += ", 🔁 Looping Playlist";
                    break;
                case LoopingState.Song:
                    footer += ", 🔂 Looping Song";
                    break;
            }
            embed.WithFooter(footer);
            var artwork = await track.FetchArtworkAsync();
            embed.WithThumbnailUrl(artwork);
            if (track.IsStream)
            {
                embed.AddField("Livestream! 🔴", "This must be skipped or end to move on", true);
            }
            else
            {
                embed.AddField("Length", track.Duration.NumericString(), true);
            }
            var info = embed.Build();
            await player.TextChannel.SendMessageAsync(embed: info);
        }

        private async Task TrackAdded(LavaTrack track, LavaPlayer player)
        {
            var embed = new EmbedBuilder();
            var bot = await player.VoiceChannel.Guild.GetCurrentUserAsync();
            embed.WithAuthor(bot.Nickname ?? bot.Username, bot.GetAvatarUrl());
            embed.WithTitle($"{track.Title}");
            embed.WithUrl(track.Url.ToString());
            embed.AddField("Author", track.Author, true);
            var queue = player.Queue;
            embed.AddField("Position", queue.Count + 1, true);
            embed.WithFooter($"Plays in {queue.GetLength().Add(player.Track.Duration.Subtract(player.Track.Position)).NumericString()}");
            var artwork = await track.FetchArtworkAsync();
            embed.WithThumbnailUrl(artwork);
            if (track.IsStream)
            {
                embed.AddField("Livestream! 🔴", "This must be skipped or end to move on", true);
            }
            else
            {
                embed.AddField("Length", track.Duration.NumericString(), true);
            }
            var info = embed.Build();
            await player.TextChannel.SendMessageAsync(embed: info);
        }


        private Task LavaNode_SocketClosed(WebSocketClosedEventArgs arg)
        {
            System.Console.WriteLine("Websocket Close");
            return Task.CompletedTask;
        }

        private Task LavaNode_TrackStuck(TrackStuckEventArgs arg)
        {
            return Task.CompletedTask;
        }

        private Task LavaNode_TrackException(TrackExceptionEventArgs arg)
        {
            LogManager.ProcessRawLog("Lavalink", arg.Exception.Message, LogSeverity.Error);
            var trackId = arg.Track.Id;
            BrokenTracks.TryAdd(trackId, true);
            return Task.CompletedTask;
        }

        private Task LavaNode_TrackStarted(TrackStartEventArgs arg)
        {
            ushort volume;
            if (!Volume.TryGetValue(arg.Player.VoiceChannel.GuildId, out volume))
            {
               volume = DefaultVolume;
            }
            arg.Player.UpdateVolumeAsync(volume);
            NotifyChannel(arg.Track, arg.Player);
            return Task.CompletedTask;
        }

        private Task LavaNode_TrackEnded(TrackEndedEventArgs arg)
        {
            var player = arg.Player;
            var track = arg.Track;
            if (player != null) { 
                lock (player)
                {
                    LavaTrack next = null;
                    if (player.Queue?.Count > 0)
                    {
                        next = player.Queue.Peek() as LavaTrack;
                    };
                    Looping.TryGetValue(player.VoiceChannel.GuildId, out var loopState);
                    switch (loopState)
                    {
                        case LoopingState.None:
                            if (next != null)
                            {
                                player.PlayAsync(next);
                                player.Queue.TryDequeue(out var playing);
                            }
                            break;
                        case LoopingState.Playlist:
                            if (next != null)
                            {
                                if (BrokenTracks.ContainsKey(track.Id))
                                    goto case LoopingState.None;
                                player.PlayAsync(next);
                                player.Queue.Enqueue(track);
                                player.Queue.TryDequeue(out var playing);
                            }
                            else
                            {
                                if (BrokenTracks.ContainsKey(track.Id))
                                    goto case LoopingState.None;
                                player.PlayAsync(track);
                            }
                            break;
                        case LoopingState.Song:
                            if (BrokenTracks.ContainsKey(track.Id))
                                goto case LoopingState.None;
                            player.PlayAsync(track);
                            break;
                        default:
                            Looping.TryAdd(player.VoiceChannel.GuildId, LoopingState.None);
                            goto case LoopingState.None;
                    }

                } 
            }
            return Task.CompletedTask;
        }

        private Task LavaNode_StatsReport(StatsEventArgs arg)
        {
            return Task.CompletedTask;
        }

        private Task LavaNode_PlayerUpdate(PlayerUpdateEventArgs arg)
        {
            return Task.CompletedTask;
        }

        private Task LavaNodeLog(LogMessage arg)
        {
            LogManager.ProcessLog(arg);
            return Task.CompletedTask;
        }

        private async Task BotReadyAsync(DiscordSocketClient bot)
        {
            if (!Node.IsConnected)
            {
                await Node.ConnectAsync();
            }
        }
        private async Task BotReadyAsync()
        {
            if (!Node.IsConnected)
            {
                await Node.ConnectAsync();
            }
        }

    }

    public enum LoopingState
    {
        None,
        Song,
        Playlist
    }
}
