using System;

namespace LevyBotSharp.DiscordHandlers.Plugins.Moderation
{
    public struct Infraction
    {

        public ulong UserId { get; set; }

        public ulong ModId { get; set; }

        public ulong ServerId { get; set; }

        public string Reason { get; set; }

        public ulong MessageId { get; set; }

        public InfractType Type { get; set; }

    }

    public enum InfractType
    {
        Bad,
        Ban,
        Kick,
        Warn,
        Mute
    }
}
