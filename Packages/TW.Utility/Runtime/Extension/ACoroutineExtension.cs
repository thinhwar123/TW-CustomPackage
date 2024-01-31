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
        public class FrameCustomYieldInstruction : CustomYieldInstruction
        {
            private readonly int frameCount;
            private int currentFrame;

            public FrameCustomYieldInstruction(int frameCount)
            {
                this.frameCount = frameCount;
            }

            public override bool keepWaiting => currentFrame++ < frameCount;
        }
        private static Dictionary<float, WaitForSeconds> WaitForSecondsCache { get; set; } = new Dictionary<float, WaitForSeconds>(new FloatComparer());
        private static Dictionary<int, FrameCustomYieldInstruction> FrameCustomYieldInstructionCache { get; set; } = new Dictionary<int, FrameCustomYieldInstruction>();
        
        
        public static WaitForSeconds SecondsLater(this float seconds)
        {
            if (!WaitForSecondsCache.TryGetValue(seconds, out WaitForSeconds waitForSeconds))
            {
                waitForSeconds = new WaitForSeconds(seconds);
                WaitForSecondsCache.Add(seconds, waitForSeconds);
            }
            return waitForSeconds;
        }
        public static FrameCustomYieldInstruction FramesLater(this int frames)
        {
            if (!FrameCustomYieldInstructionCache.TryGetValue(frames, out FrameCustomYieldInstruction frameCustomYieldInstruction))
            {
                frameCustomYieldInstruction = new FrameCustomYieldInstruction(frames);
                FrameCustomYieldInstructionCache.Add(frames, frameCustomYieldInstruction);
            }
            return frameCustomYieldInstruction;
        }
    }

}