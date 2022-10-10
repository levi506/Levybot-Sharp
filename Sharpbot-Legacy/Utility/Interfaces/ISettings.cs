using Discord.WebSocket;

namespace LevyBotSharp.Utility.Interfaces
{
    public enum LogType
    {
        Infraction,
        Delete,
        Edit,
        ChannelDel,
        ChannelAdd,
        ChannelEdit,
        RoleDel,
        RoleAdd,
        RoleEdit,
        UBan,
        UUnban,
        ULeft,
        UJoin,
        MemberEdit,
        UVoice,
        GuildEdit
    }


    public interface ISettings
    {
        //Identifying Ids

        ulong GuildId { get; }

        ulong BotId { get; }

        //Command Prefixs

        string PriPrefix { get; }
    
        string SecPrefix { get; }

        //Log Enables

        bool[] EnableLog { get; }

        //Log Channel Ids

        ulong[] Logs { get; }

        //Etc Settings

        bool EnableStarboard { get; }

        ulong StarboardChannel { get; }

        PermLevel ValidStars { get; }

        string GetDatabaseId();
        bool GetLogCheck(LogType type);
        SocketTextChannel GetLog(LogType log);
        SocketTextChannel GetStarboard();
        DiscordSocketClient GetBot();

    }
}
