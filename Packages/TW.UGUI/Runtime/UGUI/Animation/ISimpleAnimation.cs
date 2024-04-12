using System;
using Cysharp.Threading.Tasks;

namespace TW.UGUI.Animation
{
    public interface ISimpleAnimation
    {
        float TotalDuration { get; }
        void Setup();
        UniTask PlayAsync(IProgress<float> progress = null);
        void Play(IProgress<float> progress = null);
        void Stop();
    }
}