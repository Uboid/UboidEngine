using System;
using System.Collections.Generic;
using System.Text;
using UboidEngine.DataTypes;

namespace UboidEngine
{
    public static class Mouse
    {
        public static uint GetState(out int x, out int y) => SDL2.SDL.SDL_GetMouseState(out x, out y);

        public static Vector2 GetPosition()
        {
            GetState(out int x, out int y);
            return new Vector2(x, y);
        }
    }
}
