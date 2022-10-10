using SDL2;
using System;
using System.Collections.Generic;
using System.Text;
using UboidEngine.DataTypes;

namespace UboidEngine.Entities.Sprites
{
    public class BackgroundSprite : Sprite
    {
        public bool Parallax { get; set; }

        public BackgroundSprite(bool parallax = false) : base()
        {
            Parallax = parallax;
            Priority = -100;
        }

        public void FitScreenSize()
        {
            if(!Parallax)
            {
                Position = new Vector2(0, 0);
                Size = new Vector2(Game.Instance.m_iScreenW, Game.Instance.m_iScreenH);
            }
            else
            {
                Position = new Vector2(-7, -7);
                Size = new Vector2(Game.Instance.m_iScreenW + 14, Game.Instance.m_iScreenH + 14);
            }
        }

        public override void Update()
        {
            var centerX = Game.Instance.m_iScreenW / 2;
            var centerY = Game.Instance.m_iScreenH / 2;

            var mousePos = Mouse.GetPosition();

            var offset = (new Vector2(centerX, centerY) - mousePos) / new Vector2(centerX, centerY) * 7;

            Position = new Vector2(-7, -7) + offset;
            base.Update();
        }
    }
}
