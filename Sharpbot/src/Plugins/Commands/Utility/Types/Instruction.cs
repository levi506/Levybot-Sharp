using Sharpbot.Services.Clients.Utility;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sharpbot.Plugins.Commands.Utility
{
    public class Instruction
    {
        public string RawData { get; private set; }
        public MessageRequest SourceRequest { get; private set; }
        public string Instruct { get; private set; }
        public List<Parameter> Variance { get; private set; }

        public Instruction(string data, MessageRequest req)
        {
            RawData = data.Substring(1);
            SourceRequest = req;
            if (data.Contains("="))
            {
                var fission = data.Split("=");
                Instruct = fission[0];
                if (fission.Length > 1)
                {
                    Variance = new List<Parameter>();
                    var partArgs = fission[1].Split(",");
                    foreach (var s in partArgs)
                    {
                        Variance.Add(new Parameter(s, req));
                    }
                }

            }
            else
            {
                Instruct = RawData;
            }

        }
    }

}
