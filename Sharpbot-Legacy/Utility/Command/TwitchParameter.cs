using TwitchLib.Client;

namespace LevyBotSharp.Utility.Command
{
    public class TwitchParameter
    {

        private readonly string _value;

        public string ChannelId { get; private set; }

        public TwitchClient Bot;

        public TwitchParameter(string value, TwitchClient bot, string channel = "0")
        {
            ChannelId = channel;
            _value = value;
            Bot = bot;
        }

        public string AsString()
        {
            return _value;
        }

        public TwitchClient GetClient()
        {
            return Bot;
        }

    }
}
