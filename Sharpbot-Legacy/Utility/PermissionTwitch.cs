using System.Collections.Generic;
using System.Threading.Tasks;
using LevyBotSharp.DataHandlers.Models;
using TwitchLib.Api.Helix.Models.Users;
using TwitchLib.Client.Models;

namespace LevyBotSharp.Utility
{
    public static partial class PermissionsHandler
    {
        public static async Task<PermLevel> ResolveTGlobalPerm(User user)
        {
            return user.Id == $"{Program.GetOwnerIdT()}" ? PermLevel.Owner : PermLevel.Normal;
        }

        public static async Task<PermLevel> ResolveChannelPerm(ChatMessage msg, ChannelSettings set)
        {
            if (msg.UserId == $"{Program.GetOwnerIdT()}" || msg.IsBroadcaster)
                return PermLevel.Owner;
            if (msg.IsModerator)
            {
                return PermLevel.Moderator;
            }

            
            return msg.Badges.Contains(new KeyValuePair<string, string>("vip","1")) ? PermLevel.Regular : PermLevel.Normal;
        }
    }
}
