using UnityEngine;

namespace TW.Utility.DesignPattern
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    // Find singleton
                    instance = FindObjectOfType<T>();

                    // Create new instance if one doesn't already exist.
                    if (instance == null)
                    {
                        // Need to create a new GameObject to attach the singleton to.
                        GameObject singletonObject = new GameObject();
                        instance = singletonObject.AddComponent<T>();
                        singletonObject.name = typeof(T).ToString() + " (Singleton)";

                    }

                }
                return instance;
            }
        }
    }

}
