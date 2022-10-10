using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sharpbot.Plugins.Commands.Utility.Extensions
{
    public static class Parameters
    {
        public static string Merge(this List<Parameter> Params)
        {
            var value = new StringBuilder();

            value.Append(Params.First().RawData);
            Params.Remove(Params.First());
            foreach (var param in Params)
            {
                value.Append(" " + param.RawData);
            }

            return value.ToString();
        }
    }
}
