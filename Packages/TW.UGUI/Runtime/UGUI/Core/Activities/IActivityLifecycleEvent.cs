using System;
using Cysharp.Threading.Tasks;

namespace TW.UGUI.Core.Activities
{
    public interface IActivityLifecycleEvent
    {
        /// <summary>
        /// Call this method after the activity is loaded.
        /// </summary>
        /// <returns></returns>
        UniTask Initialize(Memory<object> args);

        /// <summary>
        /// Called just before this activity is displayed by the Show transition.
        /// </summary>
        /// <returns></returns>
        UniTask WillEnter(Memory<object> args);

        /// <summary>
        /// Called just after this activity is displayed by the Show transition.
        /// </summary>
        void DidEnter(Memory<object> args);

        /// <summary>
        /// Called just before this activity is hidden by the Hide transition.
        /// </summary>
        /// <returns></returns>
        UniTask WillExit(Memory<object> args);

        /// <summary>
        /// Called just after this activity is hidden by the Hide transition.
        /// </summary>
        void DidExit(Memory<object> args);

        /// <summary>
        /// Called just before this activity is released.
        /// </summary>
        /// <returns></returns>
        UniTask Cleanup(Memory<object> args);
    }
}