using SDL2;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace UboidEngine
{
    public static class Keyboard
    {
        private static byte[] oldKeyboard = new byte[] { };
        private static byte[] newKeyboard = new byte[] { };

        private static bool GetKeyNew(SDL.SDL_Keycode key)
        {
            var keyByte = (byte)SDL.SDL_GetScancodeFromKey(key);
            if (!(newKeyboard.Length >= 0 && keyByte < newKeyboard.Length))
            {
                return false;
            }
            return newKeyboard[keyByte] == 1;
        }

        private static bool GetKeyOld(SDL.SDL_Keycode key)
        {
            var keyByte = (byte)SDL.SDL_GetScancodeFromKey(key);
            if ((!(oldKeyboard.Length >= 0 && keyByte < oldKeyboard.Length)))
            {
                return false;
            }
            return oldKeyboard[keyByte] == 1;
        }

        public static bool GetKey(SDL.SDL_Keycode key)
        {
            return GetKeyNew(key);
        }

        public static bool GetKeyDown(SDL.SDL_Keycode key)
        {
            return !GetKeyOld(key) && GetKeyNew(key);
        }

        public static bool GetKeyUp(SDL.SDL_Keycode key)
        {
            return GetKeyOld(key) && !GetKeyNew(key);
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
