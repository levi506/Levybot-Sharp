namespace Sharpbot.Services.Data.Utility
{
    public enum Socials {
        Switch,
        Twitch,
        TwitchId,
        Twitter,
        Xbox,
        PSN,
        Steam,
        SteamUrl
    }

    public enum Location : ushort
    {
        Global = 1,
        Guild = 2,
        Channel = 4,
        DM = 8,
        Whisper = 16,
        All = 31
    }

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

    public enum Permission { 
        Owner,
        Admin,
        Moderator,
        Helper,
        Regular,
        Normal,
        Ignored,
        Banned
    }

    public enum Infraction
    {
        Bad,
        Ban,
        Kick,
        Warn,
        Mute
    }

    public enum Source
    {
        Discord,
        Twitch
    }

    public enum SocialType : ushort
    {
        Custom,
        DiscordId,
        Twitch,
        Website,
        Email,
        Xbox,
        PSN,
        Switch,
        N3DS,
        WiiU,
        Steam,
        Twitter = 20,
        Youtube,
        Facebook,
        Mastodon,
        Pixiv = 30,

    }
}
