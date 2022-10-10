using Sharpbot.Plugins.Commands.Utility;
using Sharpbot.Services.Data.Utility;
using System;
using System.Threading.Tasks;

namespace Sharpbot.Plugins.Commands.Commands
{
    public class Debug : CommandGroup
    {
        public override string Name { get; set; } = "Debug";

        [CommandName("Test")]
        [Usage(Location.All)]
        public class TestCom : Command
        {
            public override Task Default(CommandRequest req)
            {
                req.SourceRequest.RespondText("Command Execution Successful");
                return Task.CompletedTask;
            }
            public override Task Excute(CommandRequest req)
            {
                Default(req);
                return Task.CompletedTask;

            }
            public override Task SendHelp(CommandRequest req)
            {
                throw new NotImplementedException();
            }
        }
    }
}
