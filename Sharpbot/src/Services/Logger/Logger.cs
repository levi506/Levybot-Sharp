using Discord;
using System;
using System.Collections.Generic;

namespace Sharpbot.Services.Logger
{
    public static class LogManager
    {
        public static LogSeverity ConsoleMin { get; private set; }
        public static Queue<LogMessage> PushQueue { get; private set; }

        public static void Build(LogSeverity minimumConsole = LogSeverity.Verbose)
        {
            ConsoleMin = minimumConsole;
            PushQueue = new Queue<LogMessage>();
            ProcessRawLog("Logging Service", "Logging Manager Initalized", LogSeverity.Verbose);
        }
        public static void ProcessRawLog(string source, string message, LogSeverity severity, Exception ex = null)
        {
            var log = new LogMessage(severity, source, message, ex);
            ProcessLog(log);
        }


        public static void ProcessLog(LogMessage message)
        {
            switch (message.Severity)
            {
                case LogSeverity.Critical:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case LogSeverity.Error:
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    break;
                case LogSeverity.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case LogSeverity.Info:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case LogSeverity.Verbose:
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    break;
                case LogSeverity.Debug:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
            }

            if (message.Severity < ConsoleMin)
            {
                Console.WriteLine($"{DateTime.Now,-19} [{message.Severity,8}] <{message.Source,18}> : {message.Message}");
                Console.ResetColor();
            }
            PushQueue.Enqueue(message);

        }

        internal static void Close()
        {
            throw new NotImplementedException();
        }
    }
}
