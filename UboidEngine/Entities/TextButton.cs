using System;
using System.Collections.Generic;
using System.Text;
using SDL2;
using UboidEngine.Font;

namespace UboidEngine.Entities
{
    public class TextButton : Button
    {
        public TextSprite Text;

        public TextButton(string text = "", int fontSize = 18, string fontPath = "/data/font/arial.ttf")
        {
            Text = new TextSprite(text, fontSize, fontPath);
            Text.Parent = this;
        }

        public override void Start()
        {
            base.Start();
            Text.Position = Position;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
        }

        public override void Update()
        {
            base.Update();
        }

        public override void Draw()
        {
            base.Draw();
        }
    }
}
