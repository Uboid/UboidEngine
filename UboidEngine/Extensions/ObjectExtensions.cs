using System;
using System.Collections.Generic;
using System.Text;

namespace UboidEngine.Extensions
{
    public static class ObjectExtensions
    {
        public static bool Is<T>(this object _obj, out T result)
        {
            result = (T)_obj;
            return Is<T>(_obj);
        }

        public static bool Is<T>(this object _obj)
        {
            return _obj is T;
        }
    }
}
