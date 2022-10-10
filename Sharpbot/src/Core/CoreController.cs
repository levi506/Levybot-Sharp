using Sharpbot.Services.ApiManager;
using Sharpbot.Services.Clients;
using Sharpbot.Services.Data;
using Sharpbot.Services.Logger;
using Sharpbot.Services.PluginManager;
using System.Threading;
using System.Threading.Tasks;

namespace Sharpbot.Core
{
    [Version("0.2.0")]
    public static class CoreController
    {
        private static Config Config { get; set; }
        private static CancellationTokenSource cancellationToken { get; set; }
        public static Task Runtime { get; private set; }

        public static async Task Build(Config config)
        {
            Config = config;
            LogManager.Build();
            DataManager.Build();
            ApiManager.Build();
            ClientManager.Build();
            PluginManager.Build();
            cancellationToken = new CancellationTokenSource();
        }

        public static void Run()
        {
            Runtime = Task.Delay(-1, cancellationToken.Token);
        }

        public static async void Shutdown()
        {
            LogManager.Close();
            DataManager.Close();
            ApiManager.Close();
            ClientManager.Close();
            PluginManager.Close();
            cancellationToken.Cancel();
        }
    }
}
