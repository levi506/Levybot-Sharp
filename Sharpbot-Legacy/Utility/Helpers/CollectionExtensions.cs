using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.WebSocket;
using LevyBotSharp.Utility.Command;

namespace LevyBotSharp.Utility.Helpers
{
    public static class CollectionExtensions
    {
        public static string Join(this IEnumerable<string> arr, string sep)
        {
            return string.Join(sep, arr);
        }

        public static Parameter Merge(this List<Parameter> Params)
        {
            var value = new StringBuilder();
            var gId = Params.First().GuildId;
            var client = Params.First().GetClient();

            value.Append(Params.First().AsString());
            Params.Remove(Params.First());
            foreach (var param in Params)
            {
                value.Append(" " + param.AsString());
            }

            return new Parameter(value.ToString(), client, gId);
        }

        public static int LastUserArg(this List<Parameter> list)
        {
            var index = 0;
            foreach (var par in list)
            {
                if (par.AsString().EndsWith(","))
                    break;
                index++;
            }

            return index;
        }

        public static async Task<List<SocketUser>> ListAsUsers(this List<Parameter> list, bool strict = false)
        {
            var uList = new List<SocketUser>();
            var guild = list.First().GetClient().GetGuild(list.First().GuildId);
            await guild.DownloadUsersAsync();
            foreach (var par in list)
            {
                if (par.AsString().EndsWith(","))
                {
                    uList.Add(await new Parameter(par.AsString().Substring(0, par.AsString().Length - 1), par.GetClient(), guild.Id).AsSocketUser(guild, strict));
                    break;
                }
                else
                {
                    uList.Add(await par.AsSocketUser(guild, strict));
                }
            }
            uList.TrimExcess();

            return uList;
        }

        public static List<SocketUser> ListAsGlobalUsers(this List<Parameter> list)
        {
            var uList = new List<SocketUser>();
            foreach (var par in list)
            {
                uList.Add(par.AsGlobalSocketUser());
            }
            uList.TrimExcess();

            return uList;
        }

        public static List<ulong> ListAsULong(this List<Parameter> list)
        {
            var uList = new List<ulong>();
            foreach (var par in list)
            {
                var unum = par.AsUlong();
                if (unum.HasValue)
                    uList.Add(unum.Value);
            }
            uList.TrimExcess();

            return uList;
        }

        public static TwitchParameter Merge(this List<TwitchParameter> Params)
        {
            var value = new StringBuilder();
            var gId = Params.First().ChannelId;
            var client = Params.First().GetClient();

            value.Append(Params.First().AsString());
            Params.Remove(Params.First());
            foreach (var param in Params)
            {
                value.Append(" " + param.AsString());
            }

            return new TwitchParameter(value.ToString(), client, gId);
        }

    }
}
