namespace TW.Utility
{
    using UnityEngine;
    public class DontDestroyOnLoadMonoBehavior : MonoBehaviour
    {
        private void Awake()
        {
            transform.SetParent(null);
            DontDestroyOnLoad(gameObject);
        }
    }

}