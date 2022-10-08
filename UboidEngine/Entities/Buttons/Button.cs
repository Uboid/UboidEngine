using System;
using System.Collections.Generic;
using System.Text;
using UboidEngine.DataTypes;
using UboidEngine.Entities.Sprites;

namespace UboidEngine.Entities.Buttons
{
    public class Button : Sprite
    {
        bool clicked = false;

        public Action Clicked;

        public override void Update()
        {
            var mb = SDL2.SDL.SDL_GetMouseState(out int x, out int y);

            if (mb == 1)
            {
                if (clicked)
                    return;

                clicked = true;

                /*
                if (Collision.CheckCollision(new Vector2Int(x, y), new Vector2Int(4,4), Position.ToINT(), Size.ToINT()))
                {
                    Clicked?.Invoke();
                }
                */
            }
            else
            {
                clicked = false;
            }
        }
    }
}
