using System.Text.RegularExpressions;

namespace LevyBotSharp.DiscordHandlers.Plugins.Moderation
{
    public static class CensorHandler
    {
        //TODO :: Add Zalgo, Links, and Invites Regex
        private static Regex Zalgo { get; } 
        private static Regex Links { get; }
        private static Regex Emotes { get; }= new Regex("<{1}a?:{1}.*:{1}[0-9]{19,22}>{1}");
        private static Regex Invites { get; }
        private static Regex Nword { get; } = new Regex("[nNИ₦]+(.?)[iI1łlo3aeE]*\\1[gG6₲Б$4bB]+\\1[ɆeEaArRBbⱤ8iI]*");

    }
}
