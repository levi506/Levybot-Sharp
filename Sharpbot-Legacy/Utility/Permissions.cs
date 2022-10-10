using System.Threading.Tasks;
using Discord.WebSocket;
using LevyBotSharp.DataHandlers;
using LevyBotSharp.DataHandlers.Database;
using LevyBotSharp.DataHandlers.Models;
using TwitchLib.Api.Helix.Models.Users;

namespace LevyBotSharp.Utility
{
    public enum PermLevel
    {
        Owner,
        Admin,
        Moderator,
        Helper,
        Regular,
        Normal,
        Ignored,
        Banned
    }

    public static partial class PermissionsHandler
    {
        //Guild Permissions is Controled by GuildData class

        public static async Task<PermLevel> ResolveGlobalPerm(SocketUser u)
        {
            return (PermLevel) await DataHandler.GetGlobalPerms(u.Id);
        }
    }


}
