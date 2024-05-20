using System;
using TW.Utility.CustomComponent;
using UnityEngine;

namespace TW.Utility.DesignPattern
{
    [RequireComponent(typeof(DontDestroyOnLoadMonoBehavior))]
    public class DDOLSingleton<T> : AwaitableCachedMonoBehaviour where T : AwaitableCachedMonoBehaviour
    {
        private static T instance;

        public static T Instance
        {
            get
            {
                if (instance != null) return instance;
                // Find singleton
                instance = FindObjectOfType<T>();

                // Create new instance if one doesn't already exist.
                if (instance != null) return instance;
                
                // Need to create a new GameObject to attach the singleton to.
                GameObject singletonObject = new GameObject();
                instance = singletonObject.AddComponent<T>();
                singletonObject.name = typeof(T).ToString() + " (Singleton)";
                return instance;
            }
        }

        protected virtual void Awake()
        {
            if (Instance != this)
            {
                Destroy(gameObject);
            }
        }
    }
}


