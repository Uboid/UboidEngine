using System;
using System.Collections.Generic;
using System.Text;
using UboidEngine.Components;
using UboidEngine.DataTypes;
using UboidEngine.Entities;
using UboidEngine.Scenes;

namespace UboidEngine
{
    public static class Collision
    {
        public static bool CheckCollision(Vector2Int aa, Vector2Int ab, Vector2Int ba, Vector2Int bb)
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

            if (bottomA <= topB || topA >= bottomB || rightA <= leftB || leftA >= rightB)
            {
                return false;
            }

            return true;
        }

        public static Collider GetEntityAABB(int x, int y, List<Collider> ignore = null)
        {
            return GetEntity(x, y, 1, 1, ignore);
        }

        public static Collider GetEntity(int x, int y, int w, int h, List<Collider> ignore = null)
        {
            if (SceneManager.CurrentScene == null)
            {
                return null;
            }

            foreach (var collider in Collider.Colliders)
            {
                if (!collider.Active || ignore != null && ignore.Contains(collider) || (collider.Parent != null && collider.Parent.IsOffscreen))
                {
                    continue;
                }

                if (CheckCollision(new Vector2Int(x, y), new Vector2Int(w, h), collider.GetPosition().ToINT(), collider.GetSize().ToINT()))
                {
                    return collider;
                }
            }

            return null;
        }

        public static Collider GetEntityALL(int x, int y, int w, int h, List<Collider> ignore = null)
        {
            if (SceneManager.CurrentScene == null)
            {
                return null;
            }

            foreach (var collider in Collider.Colliders)
            {
                if (ignore != null && ignore.Contains(collider) || (collider.Parent != null && collider.Parent.IsOffscreen))
                {
                    continue;
                }

                if (CheckCollision(new Vector2Int(x, y), new Vector2Int(w, h), collider.GetPosition().ToINT(), collider.GetSize().ToINT()))
                {
                    return collider;
                }
            }

            return null;
        }

        public static CollisionType CheckCollider(Collider a, Collider b)
        {
            if (!a.Active || !b.Active || !a.CanCollide || (b.Parent != null && b.Parent.IsOffscreen))
                return CollisionType.None;

            var aa = a.GetPosition().ToINT();
            var ab = a.GetSize().ToINT();
            var ba = b.GetPosition().ToINT();
            var bb = b.GetSize().ToINT();

            if (!CheckCollision(aa, ab, ba, bb))
                return CollisionType.None;

            CollisionType result = CollisionType.None;

            var ignore = new List<Collider>();
            ignore.Add(a);

            bool vUp = false;
            bool vDown = false;

            bool hLeft = false;
            bool hRight = false;

            for (int x = 0; x < ab.x; x++)
            {
                var ent = Collision.GetEntityAABB(aa.x + x, (aa.y + ab.y) + 1, ignore);
                if (ent == b)
                {
                    vDown = true;
                }

                ent = Collision.GetEntityAABB(aa.x + x, aa.y - 1, ignore);
                if (ent == b)
                {
                    vUp = true;
                }
            }

            for (int y = 0; y < ab.y; y++)
            {
                var ent = Collision.GetEntityAABB(aa.x - 1, aa.y + y, ignore);
                if (ent == b)
                {
                    hLeft = true;
                }

                ent = Collision.GetEntityAABB(aa.x + ab.x + 1, aa.y + y, ignore);
                if (ent == b)
                {
                    hRight = true;
                }
            }

            if (hLeft || hRight || vUp || vDown)
            {
                result &= ~CollisionType.None;
            }

            if (hLeft || hRight)
            {
                result |= CollisionType.Horizontal;

                if (hLeft)
                {
                    result |= CollisionType.HorizontalLeft;
                }

                if (hRight)
                {
                    result |= CollisionType.HorizontalRight;
                }
            }

            if (vUp || vDown)
            {
                result |= CollisionType.Vertical;

                if (vUp)
                {
                    result |= CollisionType.VerticalUp;
                }

                if (vDown)
                {
                    result |= CollisionType.VerticalDown;
                }
            }


            if (b.Trigger)
            {
                result = CollisionType.None;
            }

            return result;
        }

        public static bool IsFlag(this int flags, int flag)
        {
            return (flags & flag) == flag;
        }

        public static bool IsFlag(this byte flags, byte flag)
        {
            return (flags & flag) == flag;
        }

        public static bool IsFlag(this Enum flags, Enum flag)
        {
            var _fs = Convert.ToInt32(flags);
            var _f = Convert.ToInt32(flag);
            return (_fs & _f) == _f;
        }
    }

    [Flags]
    public enum CollisionType : byte
    {
        /// <summary>
        /// No Collision
        /// </summary>
        None = 1,
        /// <summary>
        /// Detected Horizontal Collision
        /// </summary>
        Horizontal = 2,
        /// <summary>
        /// It touched the left side of the main entity
        /// </summary>
        HorizontalLeft = 4,
        /// <summary>
        /// It touched the right side of the main entity
        /// </summary>
        HorizontalRight = 8,
        /// <summary>
        /// Detected Vertical Collision
        /// </summary>
        Vertical = 16,
        /// <summary>
        /// It touched above the main entity
        /// </summary>
        VerticalUp = 32,
        /// <summary>
        /// It touched below the main entity
        /// </summary>
        VerticalDown = 64,
        /// <summary>
        /// There is horizontal and vertical collision
        /// </summary>
        Both = 128
    }
}
