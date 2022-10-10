using System.Collections.Generic;
using System.Threading.Tasks;
using LevyBotSharp.DataHandlers.Models;
using LevyBotSharp.Utility.Interfaces;
using TwitchLib.Client;

namespace LevyBotSharp.DataHandlers
{
    public static partial class SettingsCollection
    {

        private static Dictionary<string, ITwitchSettings> _twitchSettings;

        public static Task StartUp(TwitchClient client)
        {
            if(_twitchSettings == null)
                _twitchSettings = new Dictionary<string, ITwitchSettings>();

            return Task.CompletedTask;

        }



        public static ITwitchSettings GetTwitchSettings(string id)
        {
            if (_twitchSettings.TryGetValue(id, out ITwitchSettings settings))
                return settings;
            settings = new ChannelSettings(id);
            _twitchSettings.Add(id, settings);
            return settings;

        }
    }
}
