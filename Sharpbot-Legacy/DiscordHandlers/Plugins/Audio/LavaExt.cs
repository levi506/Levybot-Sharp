using Discord.WebSocket;
using LevyBotSharp.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using Victoria;
using Victoria.Interfaces;

namespace LevyBotSharp.DiscordHandlers.Plugins.Audio
{
    public static class LavaQueueExt
    {
        public static List<LavaTrack> GetTop(this DefaultQueue<IQueueable> queue)
        {
            var top = new List<LavaTrack>();
            var enumQueue = queue.Items;
            var i = 0;
            foreach (var track in enumQueue)
            {
                if (i >= 4)
                    break;
                top.Add(track as LavaTrack);
                i++;
            }

            return top;
        }
        public static TimeSpan GetLength(this DefaultQueue<IQueueable> queue)
        {
            var top = new List<LavaTrack>();
            var length = new TimeSpan();
            var enumQueue = queue.Items;

            return enumQueue.Aggregate(length, (current, track) => current.Add((track as LavaTrack).Duration));

        }

        public static SocketTextChannel GetTextChannel(this LavaPlayer player)
        {
            if (player.TextChannel == null)
                return null;
            var txtChannelId = player.TextChannel.Id;
            var guildId = player.VoiceChannel.GuildId;
            var botGuildMem = player.VoiceChannel.Guild.GetCurrentUserAsync().GetAwaiter().GetResult();
            var botId = botGuildMem.Id;
            var bot = ClientController.GetDiscordClient(botId);
            var guild = bot.GetGuild(guildId);
            return guild.GetTextChannel(txtChannelId);

        }

    }

}
