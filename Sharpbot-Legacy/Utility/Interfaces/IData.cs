using Discord.WebSocket;
using LevyBotSharp.Utility.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace LevyBotSharp.DataHandlers.Models
{
    public interface IData
    {
        DiscordSocketClient Bot { get; }

        ulong GuildId { get; }

        ISettings SettCache { get; }

        SocketGuildChannel GetLog(LogType type);


    }
}
