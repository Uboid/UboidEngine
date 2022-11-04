using System;
using System.Collections.Generic;
using System.Text;
using UboidEngine.Core.Components;

namespace UboidEngine.Core
{
    public class Camera : GameObject
    {
        public Camera(Vec2 Position)
        {
            transform.Position = Position;
        }

        public Camera()
        {
            transform.Position = Vec2.zero;
        }

        public bool InBounds(Transform transform)
        {
            if (transform == null)
            {
                return false;
            }

            return (transform.AbsolutePosition.x >= this.transform.AbsolutePosition.x && transform.AbsolutePosition.x - transform.Scale.x <= this.transform.AbsolutePosition.x + Game.GetInstance().GetWidth()) 
                || (transform.AbsolutePosition.y >= this.transform.AbsolutePosition.y && transform.AbsolutePosition.y - transform.Scale.y <= this.transform.AbsolutePosition.y + Game.GetInstance().GetHeight());
        }
    }
}
