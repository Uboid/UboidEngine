using System;
using System.Collections.Generic;
using System.Text;
using UboidEngine.Extensions;
using UboidEngine.Core.Components;

namespace UboidEngine.Core
{
    public class GameObject
    {
        public double Priority { get; private set; } = 0;

        public bool Active = true;

        private Transform _parent;

        public Transform Parent
        {
            get
            {
                return _parent;
            }
            set
            {
                if (_parent != null)
                {
                    _parent.gameObject.RemoveChildren(this);
                }

                if (value != null)
                {
                    value.gameObject.AddChildren(this);
                }

                _parent = value;
            }
        }

        public string tag { get; set; } = "Default";
        public string name { get; set; } = "GameObject";

        private List<Component> components = new List<Component>();
        private bool started = false;

        public Transform transform { get; private set; }

        private List<GameObject> entities = new List<GameObject>();

        public GameObject(string name = "GameObject", string tag = "Default")
        {
            Active = true;
            this.name = name;
            this.tag = tag;
            transform = new Transform(this);
            AddComponent(transform);
        }

        private void AddChildren(GameObject child)
        {
            entities.Add(child);
        }

        private void RemoveChildren(GameObject child)
        {
            entities.Remove(child);
        }

        public T GetComponent<T>() where T : Component
        {
            foreach(var cmp in components.ToArray())
            {
                if(cmp.Is<T>())
                {
                    return (T)cmp;
                }
            }
            return null;
        }

        public void AddComponent(Component cmp)
        {
            components.Add(cmp);

            if (started)
            {
                cmp.Start();
            }
        }

        public T AddComponent<T>(params object[] constructorArguments) where T : Component
        {   
            T newComponent = (T)Activator.CreateInstance(typeof(T));
            AddComponent(newComponent);
            return newComponent;
        }

        public void ForEach(Action<GameObject> action)
        {
            foreach(var ent in entities)
            {
                action.Invoke(ent);
                ent.ForEach((a) =>
                {
                    action.Invoke(a);
                });
            }
        }

        public void RemoveComponent(Component cmp)
        {
            if(cmp.Is<IPermanentComponent>())
            {
                return;
            }

            components.Remove(cmp);
            cmp.Destroy();
        }

        public void RemoveComponent<T>() where T : Component
        {
            foreach (var cmp in components.ToArray())
            {
                if (cmp.Is<T>())
                {
                    RemoveComponent(cmp);
                    return;
                }
            }
        }

        /// <summary>
        /// Removes all components that is T from all childrens
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void RemoveComponentInChildren<T>(bool recursive = false) where T : Component
        {
            foreach (var ent in entities.ToArray())
            {
                ent.RemoveComponent<T>();
                if(recursive)
                    ent.RemoveComponentInChildren<T>();
            }
        }

        public T GetComponentInChildren<T>(bool recursive = false) where T : Component
        {
            foreach (var ent in entities.ToArray())
            {
                var result = ent.GetComponent<T>();

                if(result != null || !recursive)
                {
                    return result;
                }
                else
                {
                    result = ent.GetComponentInChildren<T>();
                    if (result != null)
                    {
                        return result;
                    }
                }
            }
            return null;
        }

        public virtual void Start()
        {
            started = true;
            foreach (var cmp in components.ToArray())
            {
                cmp.Start();
            }

            foreach (var ent in entities.ToArray())
            {
                if (!ent.Active)
                    continue;

                ent.Start();
            }
        }

        public virtual void PreUpdate()
        {
            foreach (var cmp in components.ToArray())
            {
                if (!cmp.Active)
                    continue;
                cmp.PreUpdate();
            }

            foreach (var ent in entities.ToArray())
            {
                if (!ent.Active)
                    continue;

                ent.PreUpdate();
            }
        }
        public virtual void Update()
        {
            foreach (var cmp in components.ToArray())
            {
                if (!cmp.Active)
                    continue;
                cmp.Update();
            }

            foreach (var ent in entities.ToArray())
            {
                if (!ent.Active)
                    continue;

                ent.Update();
            }
        }

        public virtual void LateUpdate()
        {
            foreach (var cmp in components.ToArray())
            {
                if (!cmp.Active)
                    continue;
                cmp.LateUpdate();
            }

            foreach(var ent in entities.ToArray())
            {
                if (!ent.Active)
                    continue;

                ent.LateUpdate();
            }
        }

        public virtual void Render(Camera camera)
        {
            foreach (var cmp in components.ToArray())
            {
                if (!cmp.Active)
                    continue;

                cmp.Render(camera);
            }

            foreach (var ent in entities.ToArray())
            {
                if (!ent.Active)
                    continue;

                ent.Render(camera);
            }
        }

        public virtual void Destroy()
        {
            foreach (var cmp in components.ToArray())
            {
                cmp.Destroy();
            }
            foreach (var ent in entities.ToArray())
            {
                ent.Destroy();
            }
            components.Clear();
            entities.Clear();
        }

        public GameObject Clone()
        {
            return (GameObject)MemberwiseClone();
        }
    }
}
