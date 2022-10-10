using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.Client;
using TwitchLib.Client.Events;

namespace LevyBotSharp.TwitchHandlers
{
    public static partial class TwitchHandler
    {
        public static async Task HandleTimeout(OnUserTimedoutArgs args, TwitchClient client)
        {
        }

        public static async Task HandleBan(OnUserBannedArgs args, TwitchClient client)
        {
        }

        public static async Task HandleStateChanged(OnChannelStateChangedArgs args, TwitchClient client)
        {
            
        }
    }
}
