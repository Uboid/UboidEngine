using SDL2;
using System;
using System.Collections.Generic;
using System.Text;
using UboidEngine.DataTypes;
using UboidEngine.Font;

namespace UboidEngine.Entities.Sprites
{
    public class TextSprite : Entity
    {
        private Vector2 _lastStoredSize = new Vector2();
        private IntPtr m_Font;
        private IntPtr m_pTextTexture;

        private SDL.SDL_Color col = new SDL.SDL_Color() { r = 255, g = 255, b = 255, a = 255 };

        public SDL.SDL_Color TextColor
        {
            get
            {
                return col;
            }
            set
            {
                if (col.r == value.r && col.g == value.g && col.b == value.b && col.a == value.a)
                    return;

                col = value;
                RefreshFontData();
            }
        }

        private string _text = null;
        public string Text
        {
            get
            {
                if (_text == null)
                    return "";
                return _text;
            }
            set
            {
                if (_text == value)
                    return;

                RefreshFontData();

                _text = value;
            }
        }

        public TextSprite(string text = "", int fontSize = 18, string fontPath = "/data/font/arial.ttf")
        {
            m_Font = FontLoader.LoadFont(fontPath, fontSize);

            Text = text;
        }

        public override void Update()
        {
            base.Update();
            if(_lastStoredSize != Size)
            {
                _lastStoredSize = Size;
                RefreshFontData();
            }
        }

        void RefreshFontData()
        {
            var surface = SDL_ttf.TTF_RenderText_Solid(m_Font, Text, TextColor);
            m_pTextTexture = SDL.SDL_CreateTextureFromSurface(Game.Instance.m_pRenderer, surface);
            SDL.SDL_FreeSurface(surface);
        }

        public override void Draw()
        {
            if (!Active || m_pTextTexture == IntPtr.Zero)
            {
                return;
            }

            SDL.SDL_FRect pos;

            SDL_ttf.TTF_SizeText(m_Font, Text, out var w, out var h);
            var rPos = GetPosition(true);
            pos.x = rPos.x;
            pos.y = rPos.y;
            pos.w = w;
            pos.h = h;

            SDL.SDL_RenderCopyF(Game.Instance.m_pRenderer, m_pTextTexture, IntPtr.Zero, ref pos);
        }
    }
}
