using SDL2;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace UboidEngine
{
    public static class Keyboard
    {
        private static byte[] oldKeyboard;
        private static byte[] newKeyboard;

        public static bool GetKey(SDL.SDL_Keycode _keycode)
        {
            var kc = (byte)SDL.SDL_GetScancodeFromKey(_keycode);
            if (newKeyboard == null || kc + 1 >= newKeyboard.Length)
            {
                return false;
            }
            return newKeyboard[kc] == 1;
        }

        public static bool GetKeyDown(SDL.SDL_Keycode _keycode)
        {
            var kc = (byte)SDL.SDL_GetScancodeFromKey(_keycode);
            if ((newKeyboard == null || kc + 1 >= newKeyboard.Length) || (oldKeyboard == null || kc + 1 >= oldKeyboard.Length))
            {
                return false;
            }

            return oldKeyboard[kc] == 0 && newKeyboard[kc] == 1;
        }

        public static bool GetKeyUp(SDL.SDL_Keycode _keycode)
        {
            var kc = (byte)SDL.SDL_GetScancodeFromKey(_keycode);
            if ((newKeyboard == null || kc + 1 >= newKeyboard.Length) || (oldKeyboard == null || kc + 1 >= oldKeyboard.Length))
            {
                return false;
            }

            return oldKeyboard[kc] == 1 && newKeyboard[kc] == 0;
        }

        public static void Update()
        {
            int arraySize;
            IntPtr origArray = SDL.SDL_GetKeyboardState(out arraySize);
            byte[] keys = new byte[arraySize];
            Marshal.Copy(origArray, keys, 0, arraySize);

            oldKeyboard = newKeyboard;
            newKeyboard = keys;
        }
    }
}
