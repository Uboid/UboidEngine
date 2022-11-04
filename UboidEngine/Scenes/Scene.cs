using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using UboidEngine.Core;

namespace UboidEngine.Scenes
{
    public abstract class Scene
    {
        private List<GameObject> entities = new List<GameObject>();
        private List<Camera> cameras = new List<Camera>();
        public bool Started { get; set; } = false;

        /// <summary>
        /// This function can be VERY intensive, use only in SPECIAL cases.
        ///     This lists all the GameObjects in the current scene.
        /// </summary>
        public List<GameObject> ActiveGameObjects
        {
            get
            {
                List<GameObject> tmp = new List<GameObject>();

                foreach(var ent in entities)
                {
                    tmp.Add(ent);
                    ent.ForEach((a) =>
                    {
                        tmp.Add(a);
                    });
                }

                return tmp;
            }
        }

        public Camera CreateCamera(Vec2 position)
        {
            Camera _tmp = new Camera(position);
            cameras.Add(_tmp);
            return _tmp;
        }

        public void RemoveCamera(Camera cam)
        {
            cameras.Remove(cam);
        }

        public void AddGameObject(GameObject go, GameObject parent = null)
        {
            if(parent == null)
            {
                entities.Add(go);
            }
            else
            {
                go.Parent = parent.transform;
            }
            
            go.Start();
        }

        public void RemoveGameObject(GameObject go)
        {
            entities.Remove(go);
            go.Destroy();
        }

        public virtual void Start()
        {
            Started = true;
            foreach (var ent in entities.ToArray())
            {
                ent.Start();
            }
        }

        public virtual void PreUpdate()
        {
            foreach (var ent in entities.ToArray())
            {
                if (!ent.Active)
                    continue;
                ent.PreUpdate();
            }
        }

        public virtual void Update()
        {
            foreach (var ent in entities.ToArray())
            {
                if (!ent.Active)
                    continue;
                ent.Update();
            }
        }

        public virtual void LateUpdate()
        {
            foreach (var ent in entities.ToArray())
            {
                if (!ent.Active)
                    continue;
                ent.LateUpdate();
            }
        }

        public virtual void Render()
        {
            foreach (var cam in cameras.ToArray())
            {
                if (!cam.Active)
                    continue;

                foreach (var ent in entities.ToArray())
                {
                    if (!ent.Active)
                        continue;
                    ent.Render(cam);
                }
            }
        }

        public virtual void Destroy()
        {
            foreach(var ent in entities.ToArray())
            {
                ent.Destroy();
            }

            cameras.Clear();
            entities.Clear();
        }
    }
}
