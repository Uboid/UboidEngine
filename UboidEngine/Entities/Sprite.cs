using SDL2;
using System;
using UboidEngine.Scenes;

namespace UboidEngine.Entities
{
    public class Sprite : Entity
    {
        public bool DrawOffscreen = false;
        public IntPtr m_pTexture;
        public SDL.SDL_Color Color = new SDL.SDL_Color() { r = 255, g = 255, b = 255, a = 255 };
        private string _mpath;

        public string ImagePath
        {
            get
            {
                return _mpath;
            }
            set
            {
                if (_mpath == value) return;

                if(m_pTexture != IntPtr.Zero)
                {
                    SDL.SDL_DestroyTexture(m_pTexture);
                    m_pTexture = IntPtr.Zero;
                }

                _mpath = value;
                m_pTexture = Game.LoadTexture(value);
            }
        }

        public bool FlipHorizontal;
        public bool FlipVertical;

        public Action<double> AngleChanged;

        private double angle = 0;
        public double Angle
        {
            get => angle;
            set {
                if(value > 360)
                {
                    angle = value - 360;
                }
                else if(value < 0)
                {
                    angle = 360 + value;
                }
                else
                {
                    angle = value;
                }

                AngleChanged?.Invoke(angle);
            }
        }

        public override void Draw()
        {
            var rPos = GetPosition(true);
            if (!Active || m_pTexture == IntPtr.Zero || IsOffscreen && !DrawOffscreen)
            {
                return;
            }

            SDL.SDL_FRect pos;

            pos.x = rPos.x;
            pos.y = rPos.y;
            pos.w = Size.x;
            pos.h = Size.y;

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

            SDL.SDL_RenderCopyExF(Game.Instance.m_pRenderer, m_pTexture, IntPtr.Zero, ref pos, Angle, IntPtr.Zero, flip);

            base.Draw();
        }

        public override void OnDestroy()
        {
            SDL.SDL_DestroyTexture(m_pTexture);
            m_pTexture = IntPtr.Zero;
            base.OnDestroy();
        }
    }
}
