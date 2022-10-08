using System;
using System.Collections.Generic;
using System.Text;

namespace UboidEngine
{
    public static class MathEx
    {
		public static float Clamp01(float value)
		{
			bool flag = value < 0f;
			float result;
			if (flag)
			{
				result = 0f;
			}
			else
			{
				bool flag2 = value > 1f;
				if (flag2)
				{
					result = 1f;
				}
				else
				{
					result = value;
				}
			}
			return result;
		}

		public static float Lerp(float a, float b, float t)
		{
			return a + (b - a) * Clamp01(t);
		}
	}
}
