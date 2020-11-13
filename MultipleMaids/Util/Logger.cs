using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using UnityInjector.ConsoleUtil;

namespace Util
{
    internal class LogLevel
    {
        public static LogLevel Info = new LogLevel(ConsoleColor.White, "INF");
        public static LogLevel Debug = new LogLevel(ConsoleColor.Gray, "DBG");
        public static LogLevel Warning = new LogLevel(ConsoleColor.Yellow, "WRN");
        public static LogLevel Error = new LogLevel(ConsoleColor.Red, "ERR");

        public ConsoleColor Color { get; }
        public string Tag { get; }

        private LogLevel(ConsoleColor color, string tag)
        {
            Color = color;
            Tag = tag;
        }
    }

    internal static class Logger
    {
        public const string PRE_TAG = "MultipleMaids";

        public static void Log(LogLevel level, string msg)
        {
            SafeConsole.ForegroundColor = level.Color;
            Console.WriteLine($"[{PRE_TAG}][{level.Tag}] {msg}");
            SafeConsole.ForegroundColor = ConsoleColor.White;
        }

        [Conditional("DEBUG")]
        public static void Debug(string msg)
        {
            Log(LogLevel.Debug, msg);
        }
    }
}
