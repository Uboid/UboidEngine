using System;
using System.Collections.Generic;
using System.Text;

namespace UboidEngine.DataTypes
{
    public struct Vector2
    {
        public float x, y;

        public float this[int key]
        {
            get
            {
                switch(key)
                {
                    case 0:
                        return x;
                    case 1:
                        return y;
                }

                return 0;
            }
            set
            {
                switch (key)
                {
                    case 0:
                        x = value;
                        break;
                    case 1:
                        y = value;
                        break;
                }
            }
        }

        public void SetX(float x, bool additive = false)
        {
            if (!additive)
            {
                this.x = x;
                return;
            }
            this.x += x;
        }

        public void SetY(float y, bool additive = false)
        {
            if (!additive)
            {
                this.y = y;
                return;
            }
            this.y += y;
        }

        public Vector2Int ToINT() => new Vector2Int(this);

        public Vector2(float x = 0, float y = 0)
        {
            this.x = x;
            this.y = y;
        }

        public Vector2(Vector2 v)
        {
            x = v.x;
            y = v.y;
        }

        public static float Distance(Vector2 a, Vector2 b)
        {
            float x = a.x - b.x;
            float y = a.y - b.y;
            return (float)Math.Sqrt((double)(x * x + y * y));
        }

        public static Vector2 Lerp(Vector2 a, Vector2 b, float t)
        {
            t = MathEx.Clamp01(t);
            return new Vector2(a.x + (b.x - a.x) * t, a.y + (b.y - a.y) * t);
        }

        public static Vector2 operator +(Vector2 a, Vector2 b)
        {
            return new Vector2(a.x + b.x, a.y + b.y);
        }

        public static Vector2 operator -(Vector2 a, Vector2 b)
        {
            return new Vector2(a.x - b.x, a.y - b.y);
        }

        public static Vector2 operator *(Vector2 a, Vector2 b)
        {
            return new Vector2(a.x * b.x, a.y * b.y);
        }

        public static Vector2 operator *(Vector2 a, float b)
        {
            return new Vector2(a.x * b, a.y * b);
        }

        public static Vector2 operator /(Vector2 a, Vector2 b)
        {
            return new Vector2(a.x / b.x, a.y / b.y);
        }

        public static Vector2 operator /(Vector2 a, float b)
        {
            return new Vector2(a.x / b, a.y / b);
        }

        public static bool operator ==(Vector2 a, Vector2 b)
        {
            return a.x == b.x && a.y == b.y;
        }

        public static bool operator !=(Vector2 a, Vector2 b)
        {
            return a.x != b.x || a.y != b.y;
        }

        public override bool Equals(object obj)
        {
            return obj is Vector2 && this == (Vector2)obj;
        }

        public override int GetHashCode()
        {
            return (int)(x * x + y * y) ^ 2;
        }

        public override string ToString()
        {
            return $"Vector2 ( {x}, {y} )";
        }
    }

    public struct Vector2Int
    {
        public int x, y;

        public Vector2Int(Vector2 v)
        {
            x = (int)v.x;
            y = (int)v.y;
        }

        public Vector2Int(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
