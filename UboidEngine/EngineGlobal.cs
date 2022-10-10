using System;
using System.Collections.Generic;
using System.Text;

namespace UboidEngine
{
    public static class EngineGlobal
    {
        public const int MAJOR = 1;
        public const int MINOR = 3;
        public const int PATCH = 1;

        public static string GetVersion() => $"{MAJOR}.{MINOR}.{PATCH}";

        public static void Log(string text, ConsoleColor color = ConsoleColor.White)
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
            catch(Exception ex)
            {
            }
        }
    }
}
