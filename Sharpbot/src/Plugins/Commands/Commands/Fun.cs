using Sharpbot.Plugins.Commands.Utility;
using Sharpbot.Services.Data.Utility;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Sharpbot.Plugins.Commands.Commands
{
    public partial class Fun : CommandGroup
    {
        public override string Name { get; set; } = "Fun";
        internal static Random Rand = new Random();

        [CommandName("Hug")]
        [CommandMeta("Some lovey dovey thing that simulates an internet hug because I am sure someone wants to see it's return", "Fun")]
        [Usage(Location.Guild | Location.Channel)]
        public class Hug : Command
        {
            public override Task Default(CommandRequest req)
            {
                throw new NotImplementedException();
            }

            public override Task Excute(CommandRequest req)
            {
                //Can't have the bot using all mentions
                if(req.Params.Count == 0)
                {
                    TargetlessHug(req);
                }
                var target = req.Params.First();
                bool Everyone;
                if (target.ParamTypes.ContainsKey(ParamType.EveryoneMention)){
                    target.ParamTypes.TryGetValue(ParamType.EveryoneMention, out Everyone);
                }
                else
                {
                    Everyone = false;
                }

                if (Everyone)
                {
                    EveryoneReponse(req);
                    return Task.CompletedTask;

                } else
                {
                    if (target.ParamTypes.TryGetValue((ParamType)ParamType.SelfMention, out var self))
                    {
                        if (self)
                        {

                            return Task.CompletedTask;
                        }
                    }
                }
                return Task.CompletedTask;

            }

            private void TargetlessHug(CommandRequest req)
            {
                req.SourceRequest.RespondText("I need a target to hug 😦");
            }

            private void EveryoneReponse(CommandRequest req)
            {
                req.SourceRequest.RespondText("My arms aren't big enough to hug so many people 😦");
            }

            public override Task SendHelp(CommandRequest req)
            {
                throw new NotImplementedException();
            }
        }

        [CommandName("Choose")]
        [CommandName("Pick")]
        [CommandMeta("Bot choose one of the parameters through pseudorandomizer and returns it", "Fun")]
        [Usage(Location.All)]
        public class Choose : Command
        {
            public override Task Default(CommandRequest req)
            {
                req.SourceRequest.RespondText("I need something to choose :<");
                return Task.CompletedTask;
            }

            public override Task Excute(CommandRequest req)
            {
                if (req.Params.Count < 1)
                {
                    Default(req);
                }
                else
                {
                    ChooseText(req);
                } 
                return Task.CompletedTask;

            }

            public override Task SendHelp(CommandRequest req)
            {
                throw new NotImplementedException();
            }

            public Task ChooseText(CommandRequest req)
            {
                var choices = req.Params.ToArray();

                req.SourceRequest.RespondText(choices[Rand.Next(choices.Length)].RawData);
                return Task.CompletedTask;
            }
        }

        [CommandName("Question")]
        [CommandName("LMGTFY")]
        [CommandMeta("Returns a link to a lmgtfy link using DuckDuckGo","Fun")]
        [Usage(Location.All)]
        public class Question : Command
        {
            private static string BaseUrl { get; set; } = "https://lmgtfy.com/?s=d&iie=1&q=";
            public override Task Default(CommandRequest req)
            {
                req.SourceRequest.RespondText("I need something to search for.");
                return Task.CompletedTask;
            }

            public override Task Excute(CommandRequest req)
            {
                if (req.Params.Count < 1)
                {
                    Default(req);
                }
                else
                {
                    QuestionText(req);
                }
                return Task.CompletedTask;
            }

            public override Task SendHelp(CommandRequest req)
            {
                throw new NotImplementedException();
            }

            public Task QuestionText(CommandRequest req)
            {
                var query = req.Params.First().RawData;
                query = query.Replace(" ", "+");

                req.SourceRequest.RespondText(BaseUrl + query);
                return Task.CompletedTask;
            }
        }
    }
}
