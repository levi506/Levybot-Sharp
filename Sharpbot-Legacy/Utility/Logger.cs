using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord;
using TwitchLib.Client.Events;

namespace LevyBotSharp.Utility
{
    public static class Logger
    {
        public static Task Log(LogMessage message, string key)
        {
            var cc = Console.ForegroundColor;
            switch (message.Severity)
            {
                case LogSeverity.Critical:
                case LogSeverity.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case LogSeverity.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case LogSeverity.Info:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case LogSeverity.Verbose:
                case LogSeverity.Debug:
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    break;
            }
            if(key != "Lavalink -- Global" && message.Severity != LogSeverity.Verbose)
                Console.WriteLine($"{DateTime.Now,-19} [{message.Severity,8}] <{key}> {message.Source}: {message.Message}");
            Console.ForegroundColor = cc;

            return Task.CompletedTask;
        }

        public static Task DebugLog(string message, string key)
        {
            Console.WriteLine($"{DateTime.Now,-19} <{key}> [Developer Debug]: {message}");
            return Task.CompletedTask;
        }

        public static void TwitchLog(object sender, OnLogArgs args)
        {
            Console.WriteLine($"{DateTime.Now,-19} <{args.BotUsername}> Twitch : {args.Data}");
        }
    }
}
