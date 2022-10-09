using System;
using System.Collections.Generic;
using System.Text;
using UboidEngine.Audio;
using UboidEngine.Components;
using UboidEngine.DataTypes;
using UboidEngine.Entities;
using UboidEngine.Entities.Sprites;
using UboidEngine.Scenes;
using UboidEngine.Tests.Shared.Objects;

namespace UboidEngine.Tests.Shared.Scenes
{
    public class PongScene : Scene
    {
        public Pallet LeftPallet { get; set; }
        public Pallet RightPallet { get; set; }
        public Sprite Ball { get; set; }

        private bool goingUp = false;
        private bool goingLeft = false;
        private Collider _ballCollider = null;

        public Sound hitSound = null;
        public Sound scoreSound = null;

        public float ballSpeed = 150;

        public override void Start()
        {
            LeftPallet = new Pallet(true);
            LeftPallet.Parent = this;

            RightPallet = new Pallet(false);
            RightPallet.Parent = this;

            hitSound = new Sound("Data/SFX/Hit.wav");
            scoreSound = new Sound("Data/SFX/Score.wav");

            Ball = new Sprite()
            {
                Size = new Vector2(16, 16),
                Position = new Vector2(Game.Instance.m_iScreenW / 2 - (16 / 2), Game.Instance.m_iScreenH / 2 - (16 / 2)),
                Parent = this,
                ImagePath = "Data/GFX/White.png"
            };

            _ballCollider = new Collider();
            _ballCollider.Trigger = true;
            _ballCollider.ShowHitbox = true;
            _ballCollider.SetSize(Ball.Size.x, Ball.Size.y, true);
            _ballCollider.TriggerEnter += (Collider collider) =>
            {
                hitSound.Play();
                goingLeft = !goingLeft;
            };
            Ball.AddComponent(_ballCollider);

            base.Start();
        }

        public override void Update()
        {
            float y = goingUp ? -ballSpeed : ballSpeed;
            float x = goingLeft ? -ballSpeed : ballSpeed;

            Ball.Position += new Vector2(x * Game.DeltaTime, y * Game.DeltaTime);
            _ballCollider.UpdateHitbox();

            var position = _ballCollider.GetPosition();

            if((position.x + Ball.Size.x < 0) || (position.x - Ball.Size.x > Game.Instance.m_iScreenW))
            {
                Ball.Position = new Vector2(Game.Instance.m_iScreenW / 2 - (16 / 2), Game.Instance.m_iScreenH / 2 - (16 / 2));
                scoreSound.Play();
                position = Ball.Position;
            }

            if (position.y < 0)
            {
                goingUp = false;
                hitSound.Play();
                while (position.y < 0)
                {
                    position.y = (int)(position.y + 1);
                }
            }
            else if (position.y > Game.Instance.m_iScreenH - (Ball.Size.y))
            {
                goingUp = true;
                hitSound.Play();
                while (position.y > Game.Instance.m_iScreenH - (Ball.Size.y))
                {
                    position.y = (int)(position.y - 1);
                }
            }

            Ball.Position = new Vector2(position.x, position.y);
            _ballCollider.UpdateHitbox();

            base.Update();
        }
    }
}
