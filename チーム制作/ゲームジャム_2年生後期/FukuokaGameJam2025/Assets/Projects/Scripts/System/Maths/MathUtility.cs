using UnityEngine;

namespace mySystem.Maths
{
    public class MathUtility
    {
        public static float Normalize(float value, float min = 0.0f, float max = 1.0f)
        {
            float clampedValue = Mathf.Clamp(value, min, max);
            float normalizedValue = (clampedValue - min) / (max - min);
            return normalizedValue;
        }
    }
}

