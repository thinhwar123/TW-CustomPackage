using System.Linq;
using UnityEngine;

namespace TW.Utility.Extension
{
    public static class ATransformExtension
    {
        /// <summary>
        /// Sets the global scale of a transform while maintaining its current position and rotation.
        /// </summary>
        /// <param name="transform">The transform to set the global scale on.</param>
        /// <param name="globalScale">The desired global scale.</param>
        /// <returns>The original Transform object.</returns>
        public static Transform SetGlobalScale(this Transform transform, Vector3 globalScale)
        {
            transform.localScale = Vector3.one; // reset local scale
            Vector3 lossyScale = transform.lossyScale;
            transform.localScale = new Vector3(
                globalScale.x / lossyScale.x,
                globalScale.y / lossyScale.y,
                globalScale.z / lossyScale.z
            );
            return transform;
        }
        /// <summary>
        /// Searches for a child transform with the specified name in the parent transform's hierarchy.
        /// If not found, creates a new game object with the specified name as a child of the parent transform and returns its transform.
        /// </summary>
        /// <param name="parent">The parent transform to search for the child transform under.</param>
        /// <param name="name">The name of the child transform to search for or create.</param>
        /// <param name="includeInactive">A boolean value that determines whether to include inactive game objects in the search. Defaults to false.</param>
        /// <returns></returns>
        public static Transform FindChildOrCreate(this Transform parent, string name, bool includeInactive = false)
        {
            Transform res = parent.GetComponentsInChildren<Transform>(includeInactive).FirstOrDefault(value => value.name == name);
            if (res != default) return res;

            res = new GameObject(name).transform;
            res.SetParent(parent);
            Transform transform = res.transform;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            return res;
        }
    }

}