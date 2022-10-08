using System;
using System.Collections.Generic;
using System.Text;

namespace UboidEngine.DataTypes
{
    public struct Rect
    {
        public float x, y, w, h;

        public Rect(float x = 0, float y = 0, float w = 0, float h = 0)
        {
            this.x = x;
            this.y = y;
            this.w = w;
            this.h = h;
        }

        public Rect GetCentered(Vector2 pos) {
            return GetCentered(pos.x, pos.y);
        }

        public Rect GetCentered(float centerX, float centerY) {
            return new Rect(centerX - w/2, centerY - h/2, w, h);
        }

        public Vector2 Center() {
            return new Vector2(x + w/2, y + h/2);
        }
}
}
