using System;
using System.Collections.Generic;
using System.Text;
using UboidEngine.Entities;

namespace UboidEngine.Scenes
{
    /// <summary>
    /// Base scene class
    /// </summary>
    public abstract class Scene : Container
    {
        private bool _started;

        public bool Started() => _started;

        public override void Start()
        {
            _started = true;
            base.Start();
        }
    }
}
