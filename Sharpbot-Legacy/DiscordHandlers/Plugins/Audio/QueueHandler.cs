using Discord;
using Discord.WebSocket;
using LevyBotSharp.Utility;
using LevyBotSharp.Utility.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;
using Victoria;
using Victoria.EventArgs;

namespace LevyBotSharp.DiscordHandlers.Plugins.Audio
{
    public enum LoopingState
    {
        None,
        Playlist,
        Song
    }

    public struct LoopRequest
    {
        public LoopingState rep;
        public Identifier Id;
    }

    public static class QueueHandler
    {
        public static Dictionary<Identifier, LoopingState> IsLoopingColl = new Dictionary<Identifier, LoopingState>();
        public static Dictionary<Identifier, bool> Skipping = new Dictionary<Identifier, bool>();
        public static Dictionary<Identifier, ulong> TextChannels = new Dictionary<Identifier, ulong>();
        public static async Task TrackFinished(TrackEndedEventArgs args)
        {
            var player = args.Player;
            var track = args.Track;
            var bot = await player.VoiceChannel.Guild.GetCurrentUserAsync();
            var identifier = new Identifier
            {
                BotId = bot.Id,
                GuildId = player.VoiceChannel.GuildId
            };
            lock (player)
            {
                LavaTrack next = null;
                if (player.Queue.Count > 0)
                {
                    next = player.Queue.Peek() as LavaTrack;
                };
                IsLoopingColl.TryGetValue(identifier, out var loopState);
                switch (loopState)
                {
                    case LoopingState.None:
                        if (next != null)
                        {
                            player.PlayAsync(next);
                            NotifyChannel(next, player, LoopingState.None);
                            player.Queue.TryDequeue(out var playing);
                        }
                        break;
                    case LoopingState.Playlist:
                        if (next != null)
                        {
                            NotifyChannel(next, player, LoopingState.Playlist);
                            player.PlayAsync(next);
                            player.Queue.Enqueue(track);
                            player.Queue.TryDequeue(out var playing);
                        }
                        else
                        {
                            NotifyChannel(track, player, LoopingState.Playlist);
                            player.PlayAsync(track);
                        }
                        break;
                    case LoopingState.Song:
                        NotifyChannel(track, player, LoopingState.Song);
                        player.PlayAsync(track);
                        break;
                    default:
                        IsLoopingColl.TryAdd(identifier, LoopingState.None);
                        goto case LoopingState.None;
                }
            }
        }

        public static Embed MakeTrackEmbed(LavaTrack track, IGuildUser bot, LavaPlayer player = null, LoopingState state = LoopingState.None, bool adding = false)
        {
            var embed = new EmbedBuilder();
            embed.WithAuthor(bot.Nickname??bot.Username,bot.GetAvatarUrl());
            embed.WithTitle($"{track.Title}");
            embed.WithUrl(track.Url.ToString());
            embed.AddField("Author", track.Author, true);
            if (player != null)
            {
                var queue = player.Queue;
                if (queue.Count > 0 && adding)
                {
                    embed.AddField("Position", queue.Count, true);
                    embed.WithFooter($"Plays in {queue.GetLength().Add(player.Track.Duration.Subtract(player.Track.Position)).ToString()}");
                } else
                {
                    embed.WithFooter("Playing Now!");
                }
            }
            if (track.Url.Contains("youtube")){
                embed.WithThumbnailUrl($"https://i.ytimg.com/vi/{track.Id}/mqdefault.jpg");
            }
            if (track.IsStream)
            {
                embed.AddField("Livestream! 🔴", "This must be skipped or end to move on", true);
            }
            else
            {
                embed.AddField("Length", track.Duration.NumericString(), true);
            }
            return embed.Build();
        }

        public static async Task NotifyChannel(LavaTrack track, LavaPlayer player, LoopingState state = LoopingState.None, bool adding = false)
        {

            var bot = await player.VoiceChannel.Guild.GetCurrentUserAsync();
            var embed = MakeTrackEmbed(track, bot, player, state, adding);
            var identity = new Identifier
            {
                BotId = bot.Id,
                GuildId = bot.GuildId
            };
            var channel = GetChannel(identity);
            await channel.SendMessageAsync(string.Empty, embed: embed);
        }

        public static void SetLoopingState(LoopRequest req)
        {
            if (IsLoopingColl.ContainsKey(req.Id))
            {
                IsLoopingColl.Remove(req.Id);
            }
            IsLoopingColl.TryAdd(req.Id, req.rep);
        }

        public static void SetSkip(ulong botId,ulong guildId)
        {

        }

        public static LoopingState GetState(Identifier identity)
        {
            if (IsLoopingColl.TryGetValue(identity, out var state))
            {
                return state;
            }
            IsLoopingColl.Add(identity, LoopingState.None);
            return LoopingState.None;
        }

        private static ISocketMessageChannel GetChannel(Identifier identity)
        {
            var bot = ClientController.GetDiscordClient(identity.BotId);
            var guild = bot.GetGuild(identity.GuildId);
            return TextChannels.TryGetValue(identity, out var channel) ? guild.GetTextChannel(channel) : null;
        }

        public static void SetChannel(Identifier identity, ISocketMessageChannel channel)
        {
            if (TextChannels.ContainsKey(identity))
            {
                TextChannels.Remove(identity);
            }
            TextChannels.Add(identity, channel.Id);
        }

    }
}
