using System;
using System.Collections.Generic;
using System.Text;
using SDL2;
using UboidEngine.Enums;
using UboidEngine.Core.Sprites;

namespace UboidEngine.Core.Components
{
    public class SpriteRenderer : Component
    {
        public Sprite Sprite { get; set; } = null;

        public bool DrawOffscreen { get; set; } = false;

        public bool FlipHorizontal { get; set; } = false;

        public bool FlipVertical { get; set; } = false;

        public SpriteRenderer(GameObject parent) : base(parent) { }

        public override void Render(Camera cam)
        {
            if (Sprite == null || (DrawOffscreen && cam.InBounds(transform)))
                return;

            var camPos = transform.GetPositionInCamera(cam);

            SDL.SDL_FRect pos;

            pos.x = camPos.x;
            pos.y = camPos.y;
            pos.w = transform.Scale.x;
            pos.h = transform.Scale.y;

            var flip = SDL.SDL_RendererFlip.SDL_FLIP_NONE;

            if (FlipHorizontal)
            {
                if (flip == SDL.SDL_RendererFlip.SDL_FLIP_NONE)
                {
                    flip = SDL.SDL_RendererFlip.SDL_FLIP_HORIZONTAL; 
                }
                else
                {
                    flip |= SDL.SDL_RendererFlip.SDL_FLIP_HORIZONTAL;
                }
            }

            if (FlipVertical)
            {
                if (flip == SDL.SDL_RendererFlip.SDL_FLIP_NONE)
                {
                    flip = SDL.SDL_RendererFlip.SDL_FLIP_VERTICAL;
                }
                else
                {
                    flip |= SDL.SDL_RendererFlip.SDL_FLIP_VERTICAL;
                }
            }

            SDL.SDL_RenderCopyExF(Game.GetInstance().GetRenderer(), Sprite.GetTexture(), IntPtr.Zero, ref pos, transform.Orientation, IntPtr.Zero, flip);
        }
    }
}
