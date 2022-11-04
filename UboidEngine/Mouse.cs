using SDL2;
using System.Collections.Generic;
using UboidEngine.Core;

namespace UboidEngine
{
    public static class Mouse
    {
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

        private static Dictionary<uint, bool> newMouse = new Dictionary<uint, bool>();
        private static Dictionary<uint, bool> oldMouse = new Dictionary<uint, bool>();

        private static void ReceivedEvent(SDL.SDL_EventType et, SDL.SDL_Event ev)
        {
            if(et == SDL.SDL_EventType.SDL_MOUSEMOTION)
            {
                mousePosition = new Vec2(ev.motion.x, ev.motion.y);
            }
            else if (et == SDL.SDL_EventType.SDL_MOUSEBUTTONDOWN)
            {
                var btnEv = ev.button;

                if (newMouse.TryGetValue(btnEv.button, out bool pressed))
                {
                    oldMouse[btnEv.button] = pressed;
                }
                newMouse[btnEv.button] = false;
            }
            else if (et == SDL.SDL_EventType.SDL_MOUSEBUTTONUP)
            {
                var btnEv = ev.button;

                if (newMouse.TryGetValue(btnEv.button, out bool pressed))
                {
                    oldMouse[btnEv.button] = pressed;
                }
                newMouse[btnEv.button] = true;
            }
        }

        public static void Initialize()
        {
            Game.OnEvent += ReceivedEvent;
        }

        public static bool GetButton(uint button)
        {
            return newMouse.TryGetValue(button, out bool pressed) && pressed;
        }

        public static bool GetButtonDown(uint button)
        {
            return (!oldMouse.TryGetValue(button, out bool pressed) || !pressed) && (newMouse.TryGetValue(button, out pressed) && pressed);
        }

        public static bool GetButtonUp(uint button)
        {
            return (oldMouse.TryGetValue(button, out bool pressed) && pressed) && (!newMouse.TryGetValue(button, out pressed) || !pressed);
        }
    }
}
