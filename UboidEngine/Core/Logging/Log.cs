﻿using System;
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
            var prevColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(text);
            Console.ForegroundColor = prevColor;

            try
            {
                if (Game.PlayerLog != null)
                    Game.PlayerLog.WriteLine($"[!] {text}");
            }
            catch (Exception ex)
            {
            }
        }

        public static void Error(string text)
        {
            var prevColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(text);
            Console.ForegroundColor = prevColor;

            try
            {
                if (Game.PlayerLog != null)
                    Game.PlayerLog.WriteLine($"[-] {text}");
            }
            catch (Exception ex)
            {
            }
        }

        public static bool Assert(bool condition, string message = null, bool halt = true)
        {
            if (!condition)
            {
                Error(message ?? "Assert condition is false.");

                if (halt)
                {
                    Game.OnEvent.Invoke(SDL2.SDL.SDL_EventType.SDL_QUIT, new SDL2.SDL.SDL_Event());
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
