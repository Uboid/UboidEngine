using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UboidEngine.Entities;

namespace UboidEngine.Scenes
{
    public static class SceneManager
    {
        public static Scene CurrentScene { get; private set; }

        public static void LoadScene(Scene scene, bool resetCamera = true)
        {
            if (scene == null)
                return;
            CurrentScene?.OnDestroy();
            CurrentScene = scene;
            if (!Game.Instance.Running)
            {
                return;
            }
            if (resetCamera && Camera.main != null)
            {
                Camera.main.Position = new DataTypes.Vector2();
            }
            CurrentScene.Start();
        }

        public static void Update()
        {
            if (CurrentScene == null)
                return;

            if(!CurrentScene.Started())
            {
                CurrentScene.Start();
            }
            CurrentScene.PreUpdate();
            CurrentScene.Update();
            CurrentScene.PostUpdate();
        }

        public static void Draw()
        {
            if (CurrentScene == null)
                return;

            CurrentScene.Draw();
        }

        public static Task LoadSceneTask(Scene scene, bool resetCamera = true)
        {
            return Task.Factory.StartNew(() => LoadScene(scene, resetCamera));
        }
    }
}
