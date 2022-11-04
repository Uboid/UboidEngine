using SDL2;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace UboidEngine
{
    public static class Keyboard
    {
        private static Dictionary<SDL.SDL_Keycode, bool> oldKeyboard = new Dictionary<SDL.SDL_Keycode, bool>();
        private static Dictionary<SDL.SDL_Keycode, bool> newKeyboard = new Dictionary<SDL.SDL_Keycode, bool>();

        public static Action<SDL.SDL_Keycode> OnKeyDown;
        public static Action<SDL.SDL_Keycode> OnKeyUp;

        public static bool GetKey(SDL.SDL_Keycode _keycode)
        {
            return newKeyboard.TryGetValue(_keycode, out bool pressed) && pressed;
        }

        public static bool GetKeyDown(SDL.SDL_Keycode _keycode)
        {
            return (!oldKeyboard.TryGetValue(_keycode, out bool pressed) || !pressed) && (newKeyboard.TryGetValue(_keycode, out pressed) && pressed);
        }

        public static bool GetKeyUp(SDL.SDL_Keycode _keycode)
        {
            return (oldKeyboard.TryGetValue(_keycode, out bool pressed) && pressed) && (!newKeyboard.TryGetValue(_keycode, out pressed) || !pressed);
        }

        private static void ReceivedEvent(SDL.SDL_EventType type, SDL.SDL_Event e)
        {
            if(type == SDL.SDL_EventType.SDL_KEYDOWN)
            {
                OnKeyDown?.Invoke(e.key.keysym.sym);

                if (newKeyboard.TryGetValue(e.key.keysym.sym, out var oldPressed))
                {
                    oldKeyboard[e.key.keysym.sym] = oldPressed;
                }

                newKeyboard[e.key.keysym.sym] = true;
            }
            else if (type == SDL.SDL_EventType.SDL_KEYUP)
            {
                OnKeyUp?.Invoke(e.key.keysym.sym);

                if (newKeyboard.TryGetValue(e.key.keysym.sym, out var oldPressed))
                {
                    oldKeyboard[e.key.keysym.sym] = oldPressed;
                }

                newKeyboard[e.key.keysym.sym] = false;
            }
        }

        public static void Start()
        {
            Game.OnEvent += ReceivedEvent;
        }
    }
}
