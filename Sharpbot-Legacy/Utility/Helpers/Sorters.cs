using System;
using System.Collections.Generic;
using System.Linq;
using Discord.WebSocket;

namespace LevyBotSharp.Utility.Helpers
{
    public static class Sorters
    {

        public static List<SocketGuildUser> SortMemByDate(this IReadOnlyCollection<SocketGuildUser> coll)
        {
            return coll.OrderBy(o => o.JoinedAt).ToList();
        }

        public static List<SocketRole> OrderbyName(this List<SocketRole> roles, string val)
        {
            return roles.OrderBy(o => o.Name.IndexOf(val, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public static List<SocketTextChannel> OrderbyName(this List<SocketTextChannel> channels, string val)
        {
            return channels.OrderBy(o => o.Name.IndexOf(val, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public static List<SocketVoiceChannel> OrderbyName(this List<SocketVoiceChannel> channels, string val)
        {
            return channels.OrderBy(o => o.Name.IndexOf(val, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public static List<SocketUser> OrderbyName(this List<SocketUser> users, string val)
        {
            return users.OrderBy(o => o.Username.IndexOf(val, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public static List<SocketGuildUser> OrderbyName(this List<SocketGuildUser> users, string val)
        {
            return users.OrderBy(o => o.DisplayName().IndexOf(val, StringComparison.OrdinalIgnoreCase)).ToList();
        }
    }
}
