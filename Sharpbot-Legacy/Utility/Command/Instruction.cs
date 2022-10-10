using Discord.WebSocket;

namespace LevyBotSharp.Utility.Command
{
    public class Instruction
    {
        public string Header { get; private set; }
        public Parameter Variance { get; private set; }

        public Instruction(string instruct, DiscordSocketClient c)
        {
            var parts = instruct.Substring(1).ToLower().Split("=");
            Header = parts[0];
            if (parts.Length > 1)
            {
                Variance = new Parameter(parts[1],c);
            }
        }

        
    }
}
