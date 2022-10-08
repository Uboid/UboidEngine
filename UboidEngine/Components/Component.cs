using System;
using System.Collections.Generic;
using System.Text;
using UboidEngine.Entities;

namespace UboidEngine.Components
{
    public abstract class Component
    {
        public Entity Parent { get; set; } = null;

        public virtual void Start() { }
        public virtual void PreUpdate() { }
        public virtual void Update() { }
        public virtual void PostUpdate() { }
        public virtual void Draw() { }
        public virtual void OnDestroy() { }
    }
}
