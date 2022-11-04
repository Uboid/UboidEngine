using System;
using SDL2;
using System.Collections.Generic;
using System.Text;
using UboidEngine.Core;

namespace UboidEngine.Core.Components
{
    public class FontRenderer : Component
    {
        public Font m_Font = null;
        public string m_Text = "";
        public SDL.SDL_Color m_Color = new SDL.SDL_Color() { r = 255, g = 255, b = 255, a = 255 };

        public FontRenderer(GameObject parent) : base(parent) {  }

        public override void Render(Camera camera)
        {
            if(m_Font == null || m_Font.Disposed || !camera.InBounds(transform))
            {
                return;
            }

            var position = transform.GetPositionInCamera(camera);

            var surface = SDL_ttf.TTF_RenderText_Solid(m_Font.texturePointer, m_Text, m_Color);
            var texture = SDL.SDL_CreateTextureFromSurface(Game.GetInstance().GetRenderer(), surface);

            var rect = new SDL.SDL_FRect()
            {
                x = position.x,
                y = position.y,
                w = transform.Scale.x,
                h = transform.Scale.y
            };

            SDL.SDL_RenderCopyExF(Game.GetInstance().GetRenderer(),
                texture,
                IntPtr.Zero,
                ref rect,
                transform.Orientation,
                IntPtr.Zero,
                SDL.SDL_RendererFlip.SDL_FLIP_NONE
            );

            SDL.SDL_FreeSurface(surface);
            SDL.SDL_DestroyTexture(texture);
        }
    }
}
