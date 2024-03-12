using System;
using Cysharp.Threading.Tasks;

namespace TW.UGUI.Core.Controls
{
    public interface IControlLifecycleEvent
    {
        /// <summary>
        /// Called just after this control is loaded.
        /// </summary>
        /// <returns></returns>
        UniTask Initialize(Memory<object> args);

        /// <summary>
        /// Called just before this control is displayed by the Show transition.
        /// </summary>
        /// <returns></returns>
        UniTask WillEnter(Memory<object> args);

        /// <summary>
        /// Called just after this control is displayed by the Show transition.
        /// </summary>
        /// <returns></returns>
        void DidEnter(Memory<object> args);

        /// <summary>
        /// Called just before this control is hidden by the Hide transition.
        /// </summary>
        /// <returns></returns>
        UniTask WillExit(Memory<object> args);

        /// <summary>
        /// Called just after this control is hidden by the Hide transition.
        /// </summary>
        /// <returns></returns>
        void DidExit(Memory<object> args);

        /// <summary>
        /// Called just before this control is released.
        /// </summary>
        /// <returns></returns>
        UniTask Cleanup(Memory<object> args);
    }
}