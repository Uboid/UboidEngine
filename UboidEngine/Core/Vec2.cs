using System;
using System.Collections.Generic;
using System.Text;

namespace UboidEngine.Core
{
    public struct Vec2
    {
        public float x, y;

        public Vec2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public static Vec2 operator +(Vec2 a, Vec2 b)
        {
            a.x = a.x + b.x;
            a.y = a.y + b.y;
            return a;
        }

        public static Vec2 operator -(Vec2 a, Vec2 b)
        {
            a.x = a.x - b.x;
            a.y = a.y - b.y;
            return a;
        }

        public static Vec2 operator /(Vec2 a, Vec2 b)
        {
            a.x = a.x / b.x;
            a.y = a.y / b.y;
            return a;
        }

        public static Vec2 operator /(Vec2 a, float b)
        {
            a.x = a.x / b;
            a.y = a.y / b;
            return a;
        }

        public static Vec2 operator *(Vec2 a, Vec2 b)
        {
            a.x = a.x * b.x;
            a.y = a.y * b.y;
            return a;
        }

        public static Vec2 operator *(Vec2 a, float b)
        {
            a.x = a.x * b;
            a.y = a.y * b;
            return a;
        }

        public static bool operator ==(Vec2 a, Vec2 b)
        {
            return a.x == b.x && a.y == b.y;
        }

        public static bool operator !=(Vec2 a, Vec2 b)
        {
            return a.x != b.x || a.y != b.y;
        }

        public override bool Equals(object obj)
        {
            if(obj is Vec2)
            {
                var vec = ((Vec2)obj);
                return x == vec.x && y == vec.y;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return (int)MathF.Ceiling((int)(x * y) ^ 2);
        }

        public override string ToString()
        {
            return $"Vec2({x}, {y})";
        }

        public static Vec2 zero { get; } = new Vec2(0, 0);
        public static Vec2 one { get; } = new Vec2(1, 1);
        public static Vec2 right { get; } = new Vec2(1, 0);
        public static Vec2 up { get; } = new Vec2(0, 1);
    }
}
