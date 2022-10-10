using SDL2;
using System;
using System.Collections.Generic;
using System.Text;
using UboidEngine.DataTypes;

namespace UboidEngine.Entities.Sprites
{
    public class BackgroundSprite : Sprite
    {
        public BackgroundSprite() : base()
        {
            Priority = -100;
        }

        public void FitScreenSize()
        {
            Position = new Vector2(0, 0);
            Size = new Vector2(Game.Instance.m_iScreenW, Game.Instance.m_iScreenH);
        }

        public void FitImageSize()
        {
            Position = new Vector2(0, 0);
            SDL.SDL_QueryTexture(m_pTexture, out uint format, out int access, out int w, out int h);
            Size = new Vector2(w, h);
        }
    }
}
