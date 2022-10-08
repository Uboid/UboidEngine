using System;
using System.Collections.Generic;
using System.Text;
using UboidEngine.DataTypes;

namespace UboidEngine.Entities
{
    public class Camera
    {
        public Vector2 Position = new Vector2();

        public static Camera main { get; private set; }

        public Camera()
        {
            if(main != null)
            {
                throw new Exception("A camera already exists.");
            }

            main = this;
        }

        public static Vector2 GetPosition(Vector2 pos)
        {
            if (main != null)
                return pos - main.Position;

            return pos;
        }

        public static Vector2 GetPosition()
        {
            if (main != null)
                return main.Position;

            return new Vector2();
        }

        public static uint GetMouseState(out Vector2 position)
        {
            uint s = Mouse.GetState(out int x, out int y);
            position = new Vector2(x, y) + main.Position;
            return s;
        }
    }
}
