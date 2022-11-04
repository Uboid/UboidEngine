using SDL2;
using System;

namespace UboidEngine.Core.Components
{
    public class Button : Component
    {
        protected bool Hovered { get; private set; } = false;

        public Action OnClick;
        public Action<bool> OnHover;

        public Button(GameObject parent) : base(parent) { }

        bool IsHovering()
        {
            return AABB.Intersecting(transform.AbsolutePosition, transform.Scale, Mouse.mousePosition, new Vec2(4, 4));
        }

        public override void PreUpdate()
        {
            bool oldHover = Hovered;
            Hovered = IsHovering();
            if (Hovered)
            {
                if(Mouse.GetButtonDown(SDL.SDL_BUTTON(1)))
                {
                    OnClick?.Invoke();
                }
            }

            if(Hovered && !oldHover)
            {
                OnHover?.Invoke(true);
            }
            else if(!Hovered && oldHover)
            {
                OnHover?.Invoke(false);
            }
        }
    }
}
