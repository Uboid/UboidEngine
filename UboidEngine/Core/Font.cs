using System;
using SDL2;
using System.Collections.Generic;
using System.Text;

namespace UboidEngine.Core
{
    public class Font : IDisposable
    {
        public IntPtr texturePointer;

        public bool Disposed { get; private set; }

        public void Dispose()
        {
            if(texturePointer == IntPtr.Zero)
            {
                return;
            }

            SDL_ttf.TTF_CloseFont(texturePointer);
            texturePointer = IntPtr.Zero;
            Disposed = true;
        }

        public static Font Load(string file, int pixels = 12)
        {
            return new Font() { texturePointer = SDL_ttf.TTF_OpenFont(file, pixels) };
        }
    }
}
