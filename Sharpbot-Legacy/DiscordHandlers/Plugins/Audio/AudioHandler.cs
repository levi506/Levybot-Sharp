using Discord;
using Discord.WebSocket;
using LevyBotSharp.DiscordHandlers.Plugins.Commands;
using LevyBotSharp.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Victoria;

namespace LevyBotSharp.DiscordHandlers.Plugins.Audio
{
    public struct LavaTrackQ
    {
        public LavaTrack Track { get; set; }
        public string Requester { get; set; }
    }

    public static class AudioHandler
    {
        private const ushort DefaultVol = 7;
        public static Dictionary<string, LavaNode> SoundCores { get; private set; }
        public static Dictionary<ulong, ushort> Volume { get; private set; }
        public static Task Initialize()
        {
            SoundCores = new Dictionary<string, LavaNode>();
            Volume = new Dictionary<ulong, ushort>();
            return Task.CompletedTask;
        }

        public static LavaNode AddSoundCore(string key, DiscordSocketClient bot)
        {
            var config = new LavaConfig();
            config.Hostname = "10.0.0.17";
            config.EnableResume = true;
            config.LogSeverity = LogSeverity.Warning;
            config.SelfDeaf = false;
            var node = new LavaNode(bot, config);
            node.ConnectAsync().GetAwaiter().GetResult();
            SoundCores.Add(key, node);
            node.OnTrackEnded += async (endingArgs) => { await QueueHandler.TrackFinished(endingArgs); };
            foreach (var guild in bot.Guilds)
            {
                Volume.Add(guild.Id, DefaultVol);
            }
            return node;
            
        }

        public static async Task RegisterConnection(UserRequest req)
        {
            var key = $"{req.Bot.CurrentUser.Id}";
            var identify = new Identifier
            {
                BotId = req.Bot.CurrentUser.Id,
                GuildId = req.GuildId
            };
            if (!SoundCores.TryGetValue(key, out var node))
            {
                node = AddSoundCore(key, req.Bot);
            }

            node.OnTrackEnded += async (args) => { await QueueHandler.TrackFinished(args); };

            await node.JoinAsync(req.GetMember().VoiceChannel);
            QueueHandler.SetChannel(identify, req.Channel);
        }

        public static async Task<LavaTrack> QueueYtAudio(UserRequest req, string query)
        {
            var lava = await GetPlayer(req);

            var res = await lava.node.SearchYouTubeAsync(query);
            var trackSelection = new List<LavaTrack>(5);
            foreach (var track in res.Tracks)
            {
                trackSelection.Add(track);

                if (trackSelection.Count == 5)
                    break;
            }

            await QueueTrack(trackSelection.First(), lava.player);

            return trackSelection.First();
        }

        public static LavaTrack GetPlaying(UserRequest req)
        {
            var key = $"{req.Bot.CurrentUser.Id}";
            LavaPlayer player;
            var identify = new Identifier
            {
                BotId = req.Bot.CurrentUser.Id,
                GuildId = req.GuildId
            };
            if (!SoundCores.TryGetValue(key, out var node))
            {
                AddSoundCore(key, req.Bot);
                return null;
            }
            else
            {
                player = node.GetPlayer(req.Guild);
                return player.Track;
            }
        }

        public static async Task<LavaTrack> QueueScAudio(UserRequest req, string query)
        {
            var lava = await GetPlayer(req);

            var res = await lava.node.SearchSoundCloudAsync(query);
            var trackSelection = new List<LavaTrack>(5);
            foreach (var track in res.Tracks)
            {
                trackSelection.Add(track);

                if (trackSelection.Count == 5)
                {
                    break;
                }
            }

            await QueueTrack(trackSelection.First(), lava.player);

            return trackSelection.First();
        }

        public static async Task<LavaTrack> QueueRanAudio(UserRequest req, string query)
        {
            var lava = await GetPlayer(req);

            var res = await lava.node.SearchAsync(query);
            var trackSelection = new List<LavaTrack>(5);
            foreach (var track in res.Tracks)
            {
                trackSelection.Add(track);

                if (trackSelection.Count == 5)
                {
                    break;
                }
            }

            await QueueTrack(trackSelection.First(), lava.player);

            return trackSelection.First();
        }

        public static async Task SetVolume(UserRequest req, int volum)
        {
            var key = $"{req.Bot.CurrentUser.Id}";
            ushort vol = (ushort) volum;
            if (SoundCores.TryGetValue(key, out var node))
            {
                var player = node.GetPlayer(req.Guild);
                if (player != null)
                    await player.UpdateVolumeAsync(vol);
                if (Volume.ContainsKey(req.GuildId))
                    Volume.Remove(req.GuildId);
                Volume.Add(req.GuildId, vol);
            }
        }

        public static int? GetVolume(UserRequest req)
        {
            var key = $"{req.Bot.CurrentUser.Id}";
            int? vol = null;
            if (SoundCores.TryGetValue(key, out var node))
            {
                vol = node.GetPlayer(req.Guild)?.Volume;
            }

            return vol;
        }

        public static async Task PausePlayer(UserRequest req)
        {
            var key = $"{req.Bot.CurrentUser.Id}";
            if (SoundCores.TryGetValue(key, out var node))
            {
                var player = node.GetPlayer(req.Guild);
                if (player != null)
                    await player.PauseAsync();
            }
        }

        public static async Task ResumePlayer(UserRequest req)
        {
            var key = $"{req.Bot.CurrentUser.Id}";
            if (SoundCores.TryGetValue(key, out var node))
            {
                var player = node.GetPlayer(req.Guild);
                if (player != null)
                    await player.ResumeAsync();
            }
        }


        public static async Task Disconnect(UserRequest req)
        {
            var key = $"{req.Bot.CurrentUser.Id}";
            if (SoundCores.TryGetValue(key, out var node))
            {
                node.GetPlayer(req.Guild)?.Queue.Clear();
                await node.LeaveAsync(req.Guild.CurrentUser.VoiceChannel);
                SetLoopState(req, LoopingState.None);
            }
        }

        public static void ShuffleQueue(UserRequest req)
        {
            var player = GetPlayer(req).GetAwaiter().GetResult().player;
            if (player.Queue.Count > 1)
            {
                player.Queue.Shuffle();
            }
        }

        public static async Task SkipCurrent(UserRequest req)
        {
            var serverLava = await GetPlayer(req);
            await serverLava.player.StopAsync();
        }

        private static async Task QueueTrack(LavaTrack track, LavaPlayer player)
        {
            if (player.Track != null)
            {
                QueueHandler.NotifyChannel(track, player, adding: true);
                player.Queue.Enqueue(track);
            }
            else
            {
                QueueHandler.NotifyChannel(track, player, adding: true);
                await player.PlayAsync(track);
                await Task.Delay(10);
                await player.UpdateVolumeAsync( Volume.TryGetValue(player.VoiceChannel.GuildId, out var vol) ? vol : DefaultVol);
            }
        }

        private static async Task<ServerLava> GetPlayer(UserRequest req)
        {
            var key = $"{req.Bot.CurrentUser.Id}";
            LavaPlayer player;
            var identify = new Identifier
            {
                BotId = req.Bot.CurrentUser.Id,
                GuildId = req.GuildId
            };
            if (!SoundCores.TryGetValue(key, out var node))
            {
                node = AddSoundCore(key, req.Bot);
                player = await node.JoinAsync(req.GetMember().VoiceChannel, req.Channel as ITextChannel);
                QueueHandler.SetChannel(identify, req.Channel);
            }
            else
            {
                player = node.GetPlayer(req.Guild);
                if (player != null) return new ServerLava { player = player, node = node };
                player = await node.JoinAsync(req.GetMember().VoiceChannel, req.Channel as ITextChannel);
                QueueHandler.SetChannel(identify, req.Channel);
            }

            return new ServerLava { player = player, node = node };
        }

        public static void SetLoopState(UserRequest req,LoopingState state)
        {
            var id = new Identifier
            {
                BotId = req.Bot.CurrentUser.Id,
                GuildId = req.GuildId
            };
            var loopRequest = new LoopRequest
            {
                Id = id,
                rep = state
            };
            QueueHandler.SetLoopingState(loopRequest);
        }

        public static LoopingState GetLoopState(UserRequest req)
        {
            var id = new Identifier
            {
                BotId = req.Bot.CurrentUser.Id,
                GuildId = req.GuildId
            };
            return QueueHandler.GetState(id);
        }

        public static void Seek(UserRequest req,TimeSpan Pos)
        {
            var player = GetPlayer(req).GetAwaiter().GetResult().player;
            player.SeekAsync(Pos);
        }

    }

    public struct ServerLava
    {
        public LavaNode node;
        public LavaPlayer player;
    }
}
