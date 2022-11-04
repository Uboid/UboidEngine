using System;
using System.Collections.Generic;
using System.Text;
using tainicom.Aether.Physics2D.Common;
using tainicom.Aether.Physics2D.Dynamics;

namespace UboidEngine.Box2D
{
    public static class Physics
    {
        public static bool UsePhysics { get; set; } = true;

        public static World CurrentWorld { get; set; }

        private static Vector2 _g = new Vector2(0.0f, 9.81f);
        public static Vector2 Gravity
        {
            get => _g;
            set
            {
                _g = value;
                if(CurrentWorld != null)
                {
                    CurrentWorld.Gravity = _g;
                }
            }
        }

        public static void CreateNewWorld()
        {
            if(CurrentWorld != null)
            {
                CurrentWorld.ClearForces();
                CurrentWorld.Clear();
            }

            if(UsePhysics)
                CurrentWorld = new World(Gravity);
        }
    }
}
