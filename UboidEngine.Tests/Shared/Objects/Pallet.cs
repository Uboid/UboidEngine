using SDL2;
using System;
using System.Collections.Generic;
using System.Text;
using UboidEngine.Components;
using UboidEngine.DataTypes;
using UboidEngine.Entities.Sprites;

namespace UboidEngine.Tests.Shared.Objects
{
    public class Pallet : Sprite
    {
        /// <summary>
        /// The speed of the pallet.
        /// </summary>
        public float speed = 250;

        private bool _leftSide = false;

        private Collider _collider;

        public Pallet(bool isLeftSide)
        {
            _leftSide = isLeftSide;
            var x = isLeftSide ? 32 : Game.Instance.m_iScreenW - 32;
            Size = new Vector2(16, 72);
            Position = new Vector2(x, Game.Instance.m_iScreenH/2 - (Size.y/2));
        }

        private float GetAxis()
        {
            var keyUp = _leftSide ? SDL.SDL_Keycode.SDLK_w : SDL.SDL_Keycode.SDLK_UP;
            var keyDown = _leftSide ? SDL.SDL_Keycode.SDLK_s : SDL.SDL_Keycode.SDLK_DOWN;

            var up = Keyboard.GetKey(keyUp) ? 1 : 0;
            var down = Keyboard.GetKey(keyDown) ? 1 : 0;

            return down - up;
        }

        public override void Start()
        {
            // -- YOU WOULD USUALLY WANT TO ADD ALL YOUR COMPONENTS BEFORE CALLING START --
            _collider = new Collider();
            _collider.SetSize(Size.x, Size.y, true);
            _collider.CanCollide = false;
            _collider.CanTouch = true;
            _collider.ShowHitbox = true;
            AddComponent(_collider);
            // ----------------

            base.Start();
            ImagePath = "Data/GFX/White.png";
        }

        public override void Update()
        {
            float axis = GetAxis();
            Position += new Vector2(0, (speed * axis) * Game.DeltaTime);
            _collider.UpdateHitbox();

            #region Prevent pallets going out of bounds

            var position = _collider.GetPosition().y;

            if (position < 0)
            {
                while(position < 0)
                {
                    position = (int)(position + 1);
                }
            }
            else if (position > Game.Instance.m_iScreenH - (Size.y))
            {
                while (position > Game.Instance.m_iScreenH - (Size.y))
                {
                    position = (int)(position - 1);
                }
            }

            Position = new Vector2(Position.x, position);

            #endregion

            base.Update();
        }

        public override void PostUpdate()
        {
            // We DONT call post update here since we only have a collider component.
            //  The collider component updates its touch collisions each time the entity is moved so we need
            //  to manually update the touch collisions ourselves.

            _collider.UpdateTouch();
        }
    }
}
