using SDL2;
using System.Collections.Generic;
using UboidEngine.Core;

namespace UboidEngine
{
    public static class Mouse
    {
        private static uint oldMouse;
        private static uint newMouse;

        public static Vec2 mousePosition { get; private set; }

        public static SDL.SDL_Rect Rect
        {
            get
            {
                return new SDL.SDL_Rect()
                {
                    x = (int)mousePosition.x,
                    y = (int)mousePosition.y,
                    h = 4,
                    w = 4
                };
            }
        }

        public static void Update()
        {
            oldMouse = newMouse;
            newMouse = SDL.SDL_GetMouseState(out int x, out int y);
            mousePosition = new Vec2(x, y);
        }

        private static bool GetButtonNew(uint button)
        {
            return (newMouse & button) != 0;
        }

        private static bool GetButtonOld(uint button)
        {
            return (oldMouse & button) != 0;
        }

        public static bool GetButton(uint button)
        {
            return GetButtonNew(button);
        }

        public static bool GetButtonDown(uint button)
        {
            return !GetButtonOld(button) && GetButtonNew(button);
        }

        public static bool GetButtonUp(uint button)
        {
            return GetButtonOld(button) && !GetButtonNew(button);
        }
    }
}
