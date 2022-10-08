using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace UboidEngine.Font
{
    public static class FontLoader
    {
        public static Dictionary<string, IntPtr> fonts = new Dictionary<string, IntPtr>();

        public static IntPtr LoadFont(string path, int pixelSize, bool cache = true)
        {
            var res = _LoadFont($"{Directory.GetCurrentDirectory()}/{path}", pixelSize, cache);
            if(res == IntPtr.Zero)
            {
                Console.WriteLine(SDL2.SDL_ttf.TTF_GetError());
            }
            return res;
        }

        private static IntPtr _LoadFont(string path, int pixelSize, bool cache = true)
        {
            if (!fonts.ContainsKey(path) && cache)
            {
                fonts[path] = SDL2.SDL_ttf.TTF_OpenFont(path, pixelSize);
            }

            if(!cache)
            {
                return SDL2.SDL_ttf.TTF_OpenFont(path, pixelSize);
            }

            return fonts[path];
        }
    }
}
