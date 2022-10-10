using System.Threading.Tasks;
using LevyBotSharp.DataHandlers;
using LevyBotSharp.TwitchHandlers.Plugins.Commands;
using LevyBotSharp.Utility;
using TwitchLib.Client;
using TwitchLib.Client.Events;

namespace LevyBotSharp.TwitchHandlers
{
    public static partial class TwitchHandler
    {
        public static async Task HandleMessage(OnMessageReceivedArgs args, TwitchClient client)
        {
            if (args.ChatMessage.BotUsername == args.ChatMessage.Username) return;

            var id = await GetId(args.ChatMessage.Channel);
            var sett = SettingsCollection.GetTwitchSettings(id);
            var req = await UserRequest.ReqBuilder(args.ChatMessage, client, sett);
            if (req?.Command != null)
            {
                if (req.Permissions != PermLevel.Banned)
                    await TwitchCommandHandler.ExecuteCom(req);
            }

        }


        public static async Task HandleWhisper(OnWhisperReceivedArgs args, TwitchClient client)
        {

        }
    }
}
