using System.Threading.Tasks;
using LevyBotSharp.Utility;
using LevyBotSharp.Utility.Attributes;
using LevyBotSharp.Utility.Helpers;

namespace LevyBotSharp.DiscordHandlers.Plugins.Commands.Groups
{
    public partial class Fun : CommandGroupBase
    {
        public override string Name { get; } = "Fun";

        [CommandMeta(new[] {"Question"}, "Answers a question intellegently")]
        [GlobalContext]
        [GuildContext]
        [PMContext]
        public async Task QuestionCom(UserRequest req)
        {
            var query = req.Args.Merge().AsString().Replace(" ", "+");
            await req.Channel.SendMessageAsync($"http://www.lmgtfy.com/?s=d&q={query}");
        }
    }
}
