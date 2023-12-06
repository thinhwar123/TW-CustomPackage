using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TW.Utility.Extension
{
    public static class ACoroutineExtension
    {
        public class FloatComparer : IEqualityComparer<float>
        {
            private readonly float epsilon;

            public FloatComparer(float epsilon = 0.0001f)
            {
                this.epsilon = epsilon;
            }

            public bool Equals(float x, float y)
            {
                return Mathf.Abs(x - y) < epsilon;
            }

            public int GetHashCode(float obj)
            {
                // Use a simple hash code for demonstration purposes
                return obj.GetHashCode();
            }
        }
        
        private static Dictionary<float, WaitForSeconds> WaitForSecondsCache { get; set; } = new Dictionary<float, WaitForSeconds>(new FloatComparer());
        private static WaitForEndOfFrame WaitForEndOfFrame { get; } = new WaitForEndOfFrame();
        private static WaitForFixedUpdate WaitForFixedUpdate { get; } = new WaitForFixedUpdate();
        
        
        public static WaitForSeconds Seconds(this float seconds)
        {
            if (!WaitForSecondsCache.TryGetValue(seconds, out WaitForSeconds waitForSeconds))
            {
                waitForSeconds = new WaitForSeconds(seconds);
                WaitForSecondsCache.Add(seconds, waitForSeconds);
            }
            return waitForSeconds;
        }
    }

}