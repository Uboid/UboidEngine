using System;
using System.Collections.Generic;
using System.Text;

namespace UboidEngine.Entities
{
    public abstract class Container
    {
        protected List<Entity> entities = new List<Entity>();

        public void AddEntity(Entity ent)
        {
            if (ent == null)
                return;
            entities.Add(ent);
            ent.Start();
        }

        public void RemoveEntity(Entity ent)
        {
            if (ent == null)
                return;
            ent.OnDestroy();
            entities.Remove(ent);
        }

        public virtual void Start()
        {
            foreach (var e in entities.ToArray())
            {
                if (e == null)
                {
                    continue;
                }
                e.Start();
            }
        }

        public virtual void PreUpdate()
        {
            foreach (var e in entities.ToArray())
            {
                if (e == null)
                {
                    continue;
                }
                e.PreUpdate();
            }
        }

        public virtual void PostUpdate()
        {
            foreach (var e in entities.ToArray())
            {
                if (e == null)
                {
                    continue;
                }
                e.PostUpdate();
            }
        }

        public virtual void Update()
        {
            foreach (var e in entities.ToArray())
            {
                if (e == null)
                {
                    continue;
                }
                e.Update();
            }
        }
        public virtual void Draw()
        {
            foreach (var e in entities.ToArray())
            {
                if (e == null)
                {
                    continue;
                }
                e.Draw();
            }
        }

        public virtual void OnDestroy()
        {
            foreach(var e in entities.ToArray())
            {
                if (e == null)
                {
                    continue;
                }
                e.OnDestroy();
            }
        }
    }
}
