using System;
using System.Collections.Generic;
using System.Text;

namespace UboidEngine
{
    public static class EngineGlobal
    {
        public const int MAJOR = 1;
        public const int MINOR = 3;
        public const int PATCH = 1;

        public static string GetVersion() => $"{MAJOR}.{MINOR}.{PATCH}";
    }
}
