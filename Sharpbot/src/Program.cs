using Sharpbot.Core;
using System.Threading.Tasks;

namespace Sharpbot
{
    internal static class Program
    {

        private static Config Config { get; set; }

        static void Main(string[] args)
        {
            MainAsync().GetAwaiter().GetResult();
        }
        private static async Task MainAsync()
        {
            Config = Config.ReadJson("config.json");
            CoreController.Build(Config);
            CoreController.Run();
            await CoreController.Runtime;
        }

    }
}
