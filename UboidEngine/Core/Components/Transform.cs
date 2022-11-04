﻿using System;
using System.Collections.Generic;
using System.Text;

namespace UboidEngine.Core.Components
{
    public class Transform : Component, IPermanentComponent
    {
        private Vec2 _position;

        public Vec2 Position
        {
            get
            {
                return GetRelativePosition();
            }
            set
            {
                SetRelativePosition(value);
            }
        }

        public Vec2 AbsolutePosition
        {
            get
            {
                return _position;
            }
            set
            {
                _position = value;
            }
        }

        public Vec2 CenteredPosition
        {
            get
            {
                var pos = AbsolutePosition;

                pos.x -= (Scale.x / 2) / 2;
                pos.y -= (Scale.y / 2) / 2;

                return pos;
            }
        }

        public Vec2 CenteredRelativePosition
        {
            get
            {
                var pos = GetRelativePosition();

                pos.x -= (Scale.x / 2) / 2;
                pos.y -= (Scale.y / 2) / 2;

                return pos;
            }
        }

        public Vec2 Scale = new Vec2(32, 32);
        public double Orientation;

        public Transform(GameObject parent) : base(parent) { }

        Vec2 GetRelativePosition()
        {
            if (gameObject.Parent == null)
                return AbsolutePosition;

            return AbsolutePosition - gameObject.Parent.AbsolutePosition;
        }

        void SetRelativePosition(Vec2 pos)
        {
            if (gameObject.Parent == null)
            {
                AbsolutePosition = pos;
                return;
            }

            AbsolutePosition = gameObject.Parent.AbsolutePosition + pos;
        }

        public Vec2 GetPositionInCamera(Camera cam)
        {
            if (cam == null)
            {
                return AbsolutePosition;
            }

            return AbsolutePosition - cam.transform.AbsolutePosition;
        }
    }
}
