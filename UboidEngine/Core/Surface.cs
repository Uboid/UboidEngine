using System;
using System.Collections.Generic;
using System.Text;

namespace UboidEngine.Core
{
    public class Surface : IDisposable
    {
        public bool Disposed { get; private set; }
        public void Dispose()
        {
            Disposed = true;
        }
    }
}
