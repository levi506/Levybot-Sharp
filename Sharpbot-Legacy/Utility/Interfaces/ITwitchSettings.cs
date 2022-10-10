using LevyBotSharp.DataHandlers.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LevyBotSharp.Utility.Interfaces
{
    public interface ITwitchSettings
    {

        string ChannelId { get; set; }

        string Prefix { get; set; }

        ulong DiscordRelation { get; set; }

        IData GetDiscordSettings();
    }
}
