using Sharpbot.Services.Clients.Utility;
using Sharpbot.Services.Data.Utility;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sharpbot.Plugins.Commands.Utility
{
    public class CommandRequest
    {
        public MessageRequest SourceRequest { get; private set; }
        public string TechnicalContent { get; private set; }
        public string CommandHead { get; private set; }
        public List<Parameter> Params { get; private set; }
        public Location Loc { get; private set; }
        public Dictionary<string, Instruction> SpInstructions {get; private set;}
        public new string ToString => $"CommandRequest -- {CommandHead}";


        public CommandRequest(MessageRequest req, string technicalContent)
        {
            TechnicalContent = technicalContent;
            SourceRequest = req;
            Params = new List<Parameter>();
            Loc = req.MessageMedium;
            SpInstructions = new Dictionary<string, Instruction>();
            var parts = technicalContent.Split(" ");
            CommandHead = parts[0];

            var i = 1;
            while(i < parts.Length)
            {
                if (parts[i].StartsWith("-"))
                {
                    var inst = new Instruction(parts[i].Substring(1),req);
                    SpInstructions.TryAdd(inst.Instruct, inst);
                    i++;
                    continue;
                }
                if((parts[i].StartsWith("\"")&&!(parts[i].EndsWith("\"") && parts[i].Length > 1)) || (parts[i].StartsWith("|") && !(parts[i].EndsWith("|") && parts[i].Length > 1)))
                {
                    var j = i + 1;
                    while(j < parts.Length)
                    {
                        if(parts[j].EndsWith("\"") || parts[j].EndsWith("|"))
                        {

                            break;
                        }
                        j++;
                    }
                    if(j > parts.Length)
                    {
                        continue;
                    }
                    var largeArg = "";
                    for(var k = i;k < j; k++)
                    {
                        largeArg += parts[k] + " ";
                    }
                    largeArg += parts[j];
                    var cleanedUpArg = largeArg.Substring(1, largeArg.Length - 2).Trim();
                    var lgParam = new Parameter(cleanedUpArg, req);

                    //lgParam.ResolveParamTypes();
                    Params.Add(lgParam);
                    i = j + 1;
                    continue;
                }
                var param = new Parameter(parts[i], req);
                //param.ResolveParamTypes();
                Params.Add(param);
                i++;
            }

        }

    }
}
