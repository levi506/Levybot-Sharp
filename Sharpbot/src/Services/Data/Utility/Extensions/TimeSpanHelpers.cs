using System;
using System.Collections.Generic;
using System.Text;

namespace Sharpbot.Services.Data.Utility.Extensions
{
    public static class TimeSpanHelpers
    {

        public static string NumericString(this TimeSpan span)
        {
            string result;
            if (span.Days > 0)
            {
                result = $"{span.Days}:{(span.Hours > 9 ? "" + span.Hours : "0" + span.Hours)}:{(span.Minutes > 9 ? "" + span.Minutes : "0" + span.Minutes)}:{(span.Seconds > 9 ? "" + span.Seconds : "0" + span.Seconds)}";
            }
            else if (span.Hours > 0)
            {
                result = $"{span.Hours}:{(span.Minutes > 9 ? "" + span.Minutes : "0" + span.Minutes)}:{(span.Seconds > 9 ? "" + span.Seconds : "0" + span.Seconds)}";
            }
            else if (span.Minutes > 0)
            {
                result = $"{span.Minutes}:{(span.Seconds > 9 ? "" + span.Seconds : "0" + span.Seconds)}";
            }
            else
            {
                result = $"{span.Seconds}";
            }

            return result;
        }
    }
}
