using System;
using System.Collections.Generic;
using System.Text;
using SDL2;

namespace UboidEngine.Core.Sprites
{
    public class Sprite : IDisposable
    {
        private IntPtr surface;
        private IntPtr texture;

        public Sprite(string image)
        {
            SetTexture(image);
        }

        public void Dispose()
        {
            SDL.SDL_FreeSurface(surface);
            SDL.SDL_DestroyTexture(texture);
        }

        public void SetTexture(string file)
        {
            if (texture != IntPtr.Zero)
            {
                SDL.SDL_FreeSurface(surface);
                SDL.SDL_DestroyTexture(texture);
            }

            surface = SDL_image.IMG_Load(file);
            texture = SDL.SDL_CreateTextureFromSurface(Game.GetInstance().GetRenderer(), surface);
        }

        public IntPtr GetTexture()
        {
            return texture;
        }
    }
}
