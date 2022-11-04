using System;
using System.Collections.Generic;
using System.Text;
using tainicom.Aether.Physics2D.Common;
using tainicom.Aether.Physics2D.Dynamics;
using UboidEngine.Core.Box2D;

namespace UboidEngine.Core.Components
{
    public class Rigidbody : Component
    {
        public Body body;
        public Fixture CollisionFixture;

        public void UpdateCollider()
        {
            if(CollisionFixture != null)
            {
                body.Remove(CollisionFixture);
            }
            CollisionFixture = body.CreateRectangle(transform.Scale.x, transform.Scale.y, 0, new Vector2(transform.Scale.x/2, 0));
        }

        public Rigidbody(GameObject parent, BodyType bodyType = BodyType.Static) : base(parent)
        {
            body = Physics.CurrentWorld.CreateBody(new Vector2(transform.Position.x, transform.Position.y), (float)transform.Orientation, bodyType);
            UpdateCollider();
        }

        public override void PreUpdate()
        {
            var position = body.Position;
            var angle = (double)body.Rotation;

            transform.AbsolutePosition = new Vec2(position.X, position.Y);
            transform.Orientation = angle;
        }
    }
}
