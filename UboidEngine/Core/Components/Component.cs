using System;
using System.Collections.Generic;
using System.Text;

namespace UboidEngine.Core.Components
{
    public class Component
    {
        public GameObject gameObject { get; private set; }

        public bool Active = true;

        public Component(GameObject parent)
        {
            gameObject = parent;
        }

        public Component GetComponent<T>() where T : Component
        {
            return gameObject.GetComponent<T>();
        }

        public void AddComponent(Component cmp)
        {
            gameObject.AddComponent(cmp);
        }

        public void RemoveComponent(Component cmp)
        {
            gameObject.RemoveComponent(cmp);
        }

        public void RemoveComponent<T>() where T : Component
        {
            gameObject.RemoveComponent<T>();
        }

        public Transform transform
        {
            get
            {
                return gameObject.transform;
            }
        }

        public virtual void Start() { }
        public virtual void PreUpdate() { }
        public virtual void Update() { }
        public virtual void LateUpdate() { }
        public virtual void Render(Camera camera) { }
        public virtual void Destroy() { }
    }
}
