using SDL2;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace UboidEngine.Entities
{
    public class AnimatedSprite : Sprite
    {
        public string currentAnim = "";
        public Dictionary<string, AnimationData> Animations = new Dictionary<string, AnimationData>();

        public void LoadAnim(string anim, string path)
        {
            Animations[anim] = new AnimationData(path);
            if(currentAnim == "")
            {
                currentAnim = anim;
            }
        }

        public void ChangeAnim(string anim, bool force = false)
        {
            if ((currentAnim == anim && !force) || !Animations.ContainsKey(anim))
                return;

            Animations[currentAnim].Reset();
            currentAnim = anim;
        }

        public override void Update()
        {


            if (!Animations.ContainsKey(currentAnim))
            {
                base.Update();
                return;
            }

            Animations[currentAnim].Update();
            if(Animations[currentAnim].JumpTo() != null)
            {
                ChangeAnim(Animations[currentAnim].JumpTo());
            }
            m_pTexture = Animations[currentAnim].GetFrame();

            base.Update();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            foreach(var kv in Animations)
            {
                kv.Value.Dispose();
            }

            Animations.Clear();
        }
    }

    public class AnimationData : IDisposable
    {
        public int AnimationFPS;
        public List<IntPtr> Frames = new List<IntPtr>();
        private string jumpTo = null;
        private int currentFrame = 0;
        private float time = 0;
        private float cur = 0;
        public bool allowJump = false;

        public IntPtr GetFrame()
        {
            return Frames[currentFrame];
        }

        public string JumpTo()
        {
            return allowJump ? jumpTo : null;
        }

        public AnimationData(string animationPath)
        {
            if(!File.Exists(animationPath + "/ANIMATION_DATA"))
            {
                throw new Exception("Failed to load animation, couldn't find metadata.");
            }

            var lines = File.ReadAllLines(animationPath + "/ANIMATION_DATA");
            foreach(var ln in lines)
            {
                var data = ln.Split('=');
                if (data.Length < 2)
                    continue;

                var key = data[0].ToLower();
                var value = data[1];

                switch(key)
                {
                    case "fps":
                        AnimationFPS = int.Parse(value);
                        time = 1f / (float)AnimationFPS;
                        break;
                    case "addframe":
                        var frame = Game.LoadTexture($"{animationPath}/{value}");
                        if(frame != IntPtr.Zero)
                        {
                            Frames.Add(frame);
                        }
                        break;
                    case "jumpto":
                        jumpTo = value;
                        break;
                }
            }
        }

        public void Update()
        {
            cur += Game.DeltaTime;
            if(cur >= time)
            {
                cur = 0;

                if (currentFrame + 1 >= Frames.Count)
                    if (jumpTo == null)
                        currentFrame = 0;
                    else
                        allowJump = true;
                else
                    currentFrame++;
            }
        }

        public void Reset()
        {
            cur = 0;
            currentFrame = 0;
        }

        public void Dispose()
        {
            foreach(var frame in Frames)
            {
                SDL.SDL_DestroyTexture(frame);
            }

            Frames.Clear();
            Frames = null;
        }
    }
}
