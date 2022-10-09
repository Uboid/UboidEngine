using System;
using System.Collections.Generic;
using System.Text;
using UboidEngine.Components;
using UboidEngine.DataTypes;
using UboidEngine.Scenes;

namespace UboidEngine.Entities
{
    /// <summary>
    /// Base class for all entities
    /// </summary>
    public abstract class Entity : Container
    {
        public bool Active = true;
        public bool FollowCamera = true;
        public string tag = "none";

        private Vector2 _position;
        public Vector2 Position
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

        private Vector2 _size = new Vector2(32, 32);

        private Container _parent = null;
        public Container Parent
        {
            get => _parent;
            set
            {
                if(_parent != value)
                {
                    _parent?.RemoveEntity(this);
                }

                _parent = value;
                value?.AddEntity(this);
            }
        }

        private protected List<Component> components = new List<Component>();

        public bool IsOffscreen
        {
            get
            {
                var rPos = GetPosition(true);
                return (((rPos.x + Size.x) <= 0 || (rPos.x - Size.x) > Game.Instance.m_iScreenW || (rPos.y + Size.y) <= 0 || (rPos.y - Size.y) > Game.Instance.m_iScreenH));
            }
        }

        public Vector2 GetPosition(bool relativeToCam)
        {
            if(relativeToCam)
            {
                return Camera.GetPosition(_position);
            }

            return _position;
        }

        public void AddComponent(Component component)
        {
            component.Parent = this;
            components.Add(component);
            component.Start();
        }

        public void RemoveComponent(Component component)
        {
            component.Parent = null;
            component.OnDestroy();
            components.Remove(component);
        }

        public void RemoveComponent<T>() where T : Component
        {
            var t = typeof(T);
            Component cache = null;
            foreach (var c in components.ToArray())
            {
                if (c.GetType() == t)
                {
                    cache = c;
                    break;
                }
            }

            if (cache != null)
            {
                RemoveComponent(cache);
            }
        }

        public T GetComponent<T>() where T : Component
        {
            foreach(var c in components.ToArray())
            {
                if (c.GetType() == typeof(T))
                    return (T)c;
            }

            return null;
        }

        public Vector2 Size
        {
            get
            {
                return _size;
            }
            set
            {
                _size = value;
            }
        }

        public bool IsTag(string tag)
        {
            return this.tag == tag;
        }

        public override void PreUpdate()
        {
            if (!Active)
                return;

            base.PreUpdate();

            foreach (var c in components.ToArray())
            {
                c.PreUpdate();
            }
        }

        public override void Update()
        {
            if (!Active)
                return;

            base.Update();

            foreach (var c in components.ToArray())
            {
                c.Update();
            }
        }

        public override void PostUpdate()
        {
            if (!Active)
                return;

            base.PostUpdate();

            foreach (var c in components.ToArray())
            {
                c.PostUpdate();
            }
        }

        public override void Draw()
        {
            if (!Active)
                return;

            base.Draw();

            foreach (var c in components.ToArray())
            {
                c.Draw();
            }
        }

        public override void Start()
        {
            base.Start();

            foreach (var c in components.ToArray())
            {
                c.Start();
            }
        }

        public override void OnDestroy()
        {
            base.OnDestroy();

            foreach (var c in components.ToArray())
            {
                c.OnDestroy();
                c.Parent = null;
            }

            components.Clear();
        }

        public void Destroy()
        {
            if (Parent == null)
            {
                OnDestroy();
                return;
            }

            Parent.RemoveEntity(this);
        }
    }
}
