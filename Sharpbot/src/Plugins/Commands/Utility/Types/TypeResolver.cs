using System.Text.RegularExpressions;

namespace Sharpbot.Plugins.Commands.Utility
{
    public static class ParameterTypes
    {
        public static Regex Int { get; private set; } = new Regex("[0-9]{1,9}");
        public static Regex Long { get; private set; } = new Regex("[0-9]{1,19}");
        public static Regex Id { get; private set; } = new Regex("[0-9]{1,22}");
        public static Regex DiscordEmote { get; private set; } = new Regex("<{1}a?:{1}.*:{1}[0-9]{1,22}>{1}");
        public static Regex TextChannel { get; private set; } = new Regex("<{1}#{1}[0-9]{1,22}>{1}"); 
        public static Regex RoleMention { get; private set; } = new Regex("<{1}@{1}&{1}[0-9]{1,22}>{1}");
        public static Regex UserMention { get; private set; } = new Regex("<{1}@{1}!?[0-9]{1,22}>{1}");
        public static Regex EveryoneMention { get; private set; } = new Regex("(@+)(([eE][vV][eE][rR][yY][oO][nN][eE])|([hH][eE][rR][eE]))");
    }

    public enum ParamType
    {
        String,
        Int,
        Long,
        User,
        StrictUser,
        Guild,
        TextChannel,
        VoiceChannel,
        GuildUser,
        Role,
        Emote,
        Id,
        EveryoneMention,
        SelfMention
    }

}
