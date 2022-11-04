using System;
using System.Collections.Generic;
using System.Text;

namespace UboidEngine.Core.Logging
{
    public static class Log
    {
        public static void Info(string text, ConsoleColor color = ConsoleColor.White)
        {
            var prevColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ForegroundColor = prevColor;

            try
            {
                if (Game.PlayerLog != null)
                    Game.PlayerLog.WriteLine(text);
            }
            catch (Exception ex)
            {
            }
        }

        public static void Warning(string text)
        {
            Info($"[!] {text}", ConsoleColor.Yellow);
        }

        public static void Error(string text)
        {
            Info($"[-] {text}", ConsoleColor.Red);
        }

        public static bool Assert(bool condition, string message = null, bool halt = true)
        {
            if (condition)
            {
                Error(message ?? "Assert condition is false.");

                if (halt)
                {
                    Game.Instance?.HaltExecution();
                }

                return true;
            }

            return false;
        }

        public static bool Assert(Func<bool> condition, string message = null, bool exception = true)
        {
            return Assert(condition(), message, exception);
        }
    }
}
