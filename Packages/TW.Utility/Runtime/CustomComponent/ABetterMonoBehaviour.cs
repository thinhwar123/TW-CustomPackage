using UnityEngine;

namespace TW.Utility.CustomComponent
{
    public class ABetterMonoBehaviour : MonoBehaviour
    {
        private Transform cachedTransform;
        private GameObject cachedGameObject;

        private Transform CachedTransform
        {
            get
            {
                if (cachedTransform == null)
                    cachedTransform = base.transform;
                return cachedTransform;
            }
        }

        private GameObject CachedGameObject
        {
            get
            {
                if (cachedGameObject == null)
                    cachedGameObject = base.gameObject;
                return cachedGameObject;
            }
        }

        public new Transform transform => CachedTransform;

        public new GameObject gameObject => CachedGameObject;
    }
}