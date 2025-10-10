using UnityEngine;
using UnityEngine.Assertions;

namespace mySystem.Maths
{
    public static class Easing
    {
        #region Liner
        public static float Linear(float time, float totalTime, float start, float end)
        {
            AssertTime(totalTime, time);
            return (end - start) * time / totalTime + start; ;
        }
        #endregion

        #region Sine
        public static float SineIn(float time, float totaltime, float start, float end)
        {
            AssertTime(totaltime, time);
            end -= start;
            return -end * Mathf.Cos(time * (Mathf.PI * 90 / 180) / totaltime) + end + start;
        }

        public static float SineOut(float time, float totaltime, float start, float end)
        {
            AssertTime(totaltime, time);
            end -= start;
            return end * Mathf.Sin(time * (Mathf.PI * 90 / 180) / totaltime) + start;
        }

        public static float SineInOut(float time, float totaltime, float start, float end)
        {
            AssertTime(totaltime, time);
            end -= start;
            return -end / 2 * (Mathf.Cos(time * Mathf.PI / totaltime) - 1) + start;
        }
        #endregion

        #region Assert
        private static void AssertTime(float totalTime, float time)
        {
            Assert.IsTrue(totalTime >= 0.0f);
            Assert.IsTrue(time >= 0.0f);
        }
        #endregion
    }
}