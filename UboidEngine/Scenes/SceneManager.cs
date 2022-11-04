using System;
using System.Collections.Generic;
using System.Text;
using UboidEngine.Box2D;

namespace UboidEngine.Scenes
{
    public static class SceneManager
    {
        public static Scene Active { get; private set; } = null;

        public static void LoadScene(Scene scene)
        {
            if (Active != null)
            {
                Active.Destroy();
            }

            Physics.CreateNewWorld();
            Active = scene;

            if(Game.GetInstance().Running)
                Active.Start();
        }

        public static void UpdateScene()
        {
            if (Active == null)
                return;

            if(!Active.Started)
            {
                Active.Start();
            }

            Active.PreUpdate();
            Active.Update();
            Active.LateUpdate();
            Active.Render();

            if(Physics.UsePhysics)
                Physics.CurrentWorld?.Step(Game.DeltaTime);
        }
    }
}
