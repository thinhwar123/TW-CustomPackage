using System;
using UnityEngine;

namespace TW.UGUI.Shared
{
    public class MonoBehaviourDestroyedEventDispatcher : MonoBehaviour
    {
        public void OnDestroy()
        {
            OnDispatch?.Invoke();
        }

        public event Action OnDispatch;
    }
}