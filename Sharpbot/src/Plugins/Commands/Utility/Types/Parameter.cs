using Sharpbot.Services.Clients.Utility;
using Sharpbot.Services.Data.Utility;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sharpbot.Plugins.Commands.Utility
{
    public class Parameter
    {
        public string RawData { get; private set; }
        public Dictionary<ParamType, bool> ParamTypes { get; set;}
        public MessageRequest Request { get; private set; }

        public Parameter(string data, MessageRequest req)
        {
            RawData = data;
            Request = req;
            ResolveParamTypes();
        }

        public void ResolveParamTypes()
        {
            var paramsTypes = new Dictionary<ParamType, bool>();

            paramsTypes.Add(ParamType.String, true);

            paramsTypes.Add(ParamType.Int, ParameterTypes.Int.IsMatch(RawData));

            paramsTypes.Add(ParamType.Long, ParameterTypes.Long.IsMatch(RawData));
            

            if (Request.MsgSource == Source.Discord)
            {
                paramsTypes.Add(ParamType.Id, ParameterTypes.Id.IsMatch(RawData));
          
                paramsTypes.Add(ParamType.Emote, ParameterTypes.DiscordEmote.IsMatch(RawData));

                paramsTypes.Add(ParamType.TextChannel, ParameterTypes.TextChannel.IsMatch(RawData));
                
                paramsTypes.Add(ParamType.EveryoneMention, ParameterTypes.EveryoneMention.IsMatch(RawData));

                paramsTypes.Add(ParamType.SelfMention, RawData.Contains(Request.AuthorId));

            } else if(Request.MsgSource == Source.Twitch)
            {
                paramsTypes.Add(ParamType.SelfMention, RawData.Contains(Request.Username));
            }
            this.ParamTypes =  paramsTypes;
        }

    }
}
