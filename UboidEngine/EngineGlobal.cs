using System;
using System.Collections.Generic;
using System.Text;

namespace UboidEngine
{
    public static class EngineGlobal
    {
        public const int MAJOR = 1;
        public const int MINOR = 0;
        public const int PATCH = 0;

        public static string GetVersion() => $"{MAJOR}.{MINOR}.{PATCH}";
    }
}
