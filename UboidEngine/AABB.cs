using SDL2;
using System;
using System.Collections.Generic;
using System.Text;
using UboidEngine.Core;

namespace UboidEngine
{
    public static class AABB
    {
        public static bool Intersecting(Vec2 aa, Vec2 ab, Vec2 ba, Vec2 bb)
        {
            float leftA, leftB;
            float rightA, rightB;
            float topA, topB;
            float bottomA, bottomB;

            leftA = aa.x;
            rightA = aa.x + ab.x;
            topA = aa.y;
            bottomA = aa.y + ab.y;
            leftB = ba.x;
            rightB = ba.x + bb.x;
            topB = ba.y;
            bottomB = ba.y + bb.y;

            if ((bottomA <= topB) || (topA >= bottomB) || (rightA <= leftB) || (leftA >= rightB))
            {
                return false;
            }

            return true;
        }
    }
}
