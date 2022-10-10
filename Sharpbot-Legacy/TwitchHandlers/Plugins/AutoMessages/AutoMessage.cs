using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;

namespace LevyBotSharp.TwitchHandlers.Plugins.AutoMessages
{
    public class AutoMessage
    {
        public string Channel { get; private set; }
        public int ChannelId { get; private set; }
        public string Message { get; private set; }
        public Timer Timer { get; private set; }
        public int Spacing { get; private set; }
        public int Since { get; private set; }


        public AutoMessage(string message, string channel, int interval = 500,int spacing = 25)
        {

        }
    }

}
