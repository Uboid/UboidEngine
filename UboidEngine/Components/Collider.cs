using SDL2;
using System;
using System.Collections.Generic;
using System.Text;
using UboidEngine.DataTypes;
using UboidEngine.Entities;
using UboidEngine.Entities.Sprites;

namespace UboidEngine.Components
{
    public class Collider : Component
    {
        public static List<Collider> Colliders = new List<Collider>();

        public bool CanCollide = false;
        public bool Trigger;

        /// <summary>
        /// Can touch triggers
        /// </summary>
        public bool CanTouch;

        private List<Collider> touching = new List<Collider>();

        /// <summary>
        /// The higher, the more accurate
        /// </summary>
        public int CollisionAccuracy = 1;

        private Rect Hitbox = new Rect();

        private Vector2 _storedPosition = new Vector2();

        public CollisionType LastCollision;

        private Vector2 InitialSize = new Vector2();
        private Vector2 InitialOffset = new Vector2();
        private Vector2 Offset = new Vector2();

        public Action<CollisionType> CollisionChanged;
        public Action<Collider> TriggerEnter;
        public Action<Collider> TriggerExit;

        public bool ShowHitbox { get; set; } = false;

        /// <summary>
        /// Enables all colliders that have a free space on atleast one of the sides.
        /// </summary>
        public static void EnableOnlyCollisionable()
        {
            foreach(var collider in Colliders)
            {
                if(collider.Parent.IsOffscreen)
                {
                    collider.Active = false;
                    continue;
                }

                var position = collider.GetPosition().ToINT();
                var size = collider.GetSize().ToINT();

                Collider upCollider = Collision.GetEntityALL(position.x, position.y - 1, size.x, size.y, new List<Collider>() { collider });
                Collider downCollider = Collision.GetEntityALL(position.x, position.y + (size.y + 1), size.x, size.y, new List<Collider>() { collider });
                Collider leftCollider = Collision.GetEntityALL(position.x - 1, position.y, size.x, size.y, new List<Collider>() { collider });
                Collider rightCollider = Collision.GetEntityALL(position.x + (size.x + 1), position.y, size.x, size.y, new List<Collider>() { collider });

                if(upCollider == null || downCollider == null || leftCollider == null || rightCollider == null)
                {
                    collider.Active = true;
                }
                else
                {
                    collider.Active = false;
                }
            }
        }

        public Vector2 GetPosition()
        {
            if (Parent == null)
                return new Vector2();
            return new Vector2(Hitbox.x, Hitbox.y);
        }

        public Vector2 GetSize()
        {
            if (Parent == null)
                return new Vector2();
            return new Vector2(Hitbox.w, Hitbox.h);
        }

        public void SetOffset(float x, float y, bool initial = false)
        {
            Offset.x = x;
            Offset.y = y;

            if(initial)
            {
                InitialOffset = Offset;
            }
        }

        public void SetSize(float w, float h, bool initial = false)
        {
            Hitbox.w = w;
            Hitbox.h = h;

            if(initial)
            {
                InitialSize.x = w;
                InitialSize.y = h;
            }
        }

        private void RotateHitbox(double angle)
        {
            switch((int)angle)
            {
                case 0:
                case 360:
                    SetOffset(InitialOffset.x, InitialOffset.y, false);
                    SetSize(InitialSize.x, InitialSize.y);
                    break;

                case 90:
                    SetOffset(-InitialOffset.y + InitialOffset.y, InitialOffset.x);
                    SetSize(InitialSize.y, InitialSize.x);
                    break;

                case 180:
                    SetOffset(InitialOffset.x, -InitialOffset.y + InitialOffset.y);
                    SetSize(InitialSize.x, InitialSize.y);
                    break;

                case 270:
                    SetOffset(InitialOffset.y, InitialOffset.x);
                    SetSize(InitialSize.y, InitialSize.x);
                    break;
            }
        }

        public void CalculateCollisions()
        {
            if (!Active || !CanCollide || Parent == null || Parent.IsOffscreen)
                return;

            UpdateHitbox();
            CollisionType collision = CollisionType.None;

            foreach(var obj in Colliders.ToArray())
            {
                if (obj == this)
                    continue;

                if(!obj.Trigger)
                {
                    CollisionType res = Collision.CheckCollider(this, obj);
                    if (Collision.CheckCollision(GetPosition().ToINT(), GetSize().ToINT(), GetPosition().ToINT(), GetSize().ToINT()))
                    {
                        CollisionType flags = res;
                        if (((byte)flags).IsFlag(2) && !((byte)collision).IsFlag(2))
                        {
                            collision |= CollisionType.Horizontal;
                        }
                        if (((byte)flags).IsFlag(4) && !((byte)collision).IsFlag(4))
                        {
                            collision |= CollisionType.HorizontalLeft;
                        }
                        if (((byte)flags).IsFlag(8) && !((byte)collision).IsFlag(8))
                        {
                            collision |= CollisionType.HorizontalRight;
                        }
                        if (((byte)flags).IsFlag(16) && !((byte)collision).IsFlag(16))
                        {
                            collision |= CollisionType.Vertical;
                        }
                        if (((byte)flags).IsFlag(32) && !((byte)collision).IsFlag(32))
                        {
                            collision |= CollisionType.VerticalUp;
                        }
                        if (((byte)flags).IsFlag(64) && !((byte)collision).IsFlag(64))
                        {
                            collision |= CollisionType.VerticalDown;
                        }
                        if (((byte)collision).IsFlag(2) && ((byte)collision).IsFlag(16) && !((byte)collision).IsFlag(128))
                        {
                            collision |= CollisionType.Both;
                        }
                        if (!((byte)flags).IsFlag(1) && ((byte)collision).IsFlag(1))
                        {
                            collision &= ~CollisionType.None;
                        }
                    }
                }
            }

            LastCollision = collision;
        }

        public void UpdateTouch()
        {
            if (!Active || !CanTouch || Parent.IsOffscreen)
                return;

            foreach(var obj in Colliders.ToArray())
            {
                if (!obj.Trigger)
                    continue;

                if (Collision.CheckCollision(GetPosition().ToINT(), GetSize().ToINT(), obj.GetPosition().ToINT(), obj.GetSize().ToINT()))
                {
                    if (!touching.Contains(obj))
                    {
                        touching.Add(obj);
                        obj.TriggerEnter?.Invoke(this);
                    }
                }
                else
                {
                    if (touching.Contains(obj))
                    {
                        touching.Remove(obj);
                        obj.TriggerExit?.Invoke(this);
                    }
                }
            }
        }

        public void UpdateHitbox()
        {
            if (Parent == null)
                return;
            var pos = Parent.GetPosition(false);
            Hitbox.x = pos.x + Offset.x;
            Hitbox.y = pos.y + Offset.y;
        }

        public override void PostUpdate()
        {
            if (!Active)
                return;
            UpdateTouch();
        }

        public bool CheckPoint(int x = 0, int y = 0, bool relative = true)
        {
            if (!Active || Parent == null)
                return false;
            var p = relative ? (GetPosition().ToINT()) : new Vector2Int(0, 0);
            var ignore = new List<Collider>();
            ignore.Add(this);
            return Collision.GetEntityAABB(p.x + x, p.y + y, ignore) != null;
        }

        public override void Update()
        {
            if (!Active || Parent == null)
                return;
            UpdateHitbox();

            if (_storedPosition != Parent.Position && CanCollide)
            {
                CalculateCollisions();
                _storedPosition = Parent.Position;
                CollisionChanged?.Invoke(LastCollision);
            }
        }

        public override void Start()
        {
            base.Start();
            Colliders.Add(this);

            if(Parent is Sprite)
            {
                ((Sprite)Parent).AngleChanged += RotateHitbox;
            }

            UpdateHitbox();
        }

        public override void OnDestroy()
        {
            touching.Clear();
            Colliders.Remove(this);
        }

        public override void Draw()
        {
            if (!ShowHitbox || !Active)
                return;

            var rect = new SDL.SDL_FRect();
            var pos = Parent.GetPosition(true) + Offset;
            rect.x = pos.x;
            rect.y = pos.y;
            rect.w = Hitbox.w;
            rect.h = Hitbox.h;
            SDL.SDL_SetRenderDrawColor(Game.Instance.m_pRenderer, 255, 0, 0, 255);
            SDL.SDL_RenderDrawRectF(Game.Instance.m_pRenderer, ref rect);
        }
    }
}
