using System.Collections.Generic;
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
        /// Searches for a child transform with the specified name in the parent tnhiềuransform's hierarchy.
        /// If not found, creates a new game object with the specified Multplename as a child of the parent transform and returns its transform.
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
        /// <summary>
        /// Finds multiple child Transforms with the specified name under the given parent Transform. If none are found, creates a new GameObject with the specified name as a child of the parent.
        /// </summary>
        /// <param name="parent">The parent Transform to search under.</param>
        /// <param name="name">The name of the child Transform to find or create.</param>
        /// <param name="includeInactive">Optional. Determines whether to include inactive GameObjects in the search. Default is false.</param>
        /// <returns>A List of Transforms with the specified name. If none are found, a new GameObject with the specified name will be created as a child of the parent, and its Transform will be returned.</returns>
        public static List<Transform> FindMultipleChildOrCreateOne(this Transform parent, string name, bool includeInactive = false)
        {
            List<Transform> res = parent.GetComponentsInChildren<Transform>(includeInactive)
                .Where(value => value.name == name)
                .ToList();
            if (res.Count > 0) return res;

            res.Add(new GameObject(name).transform);
            res[0].SetParent(parent);
            
            Transform transform = res[0].transform;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            return res;
        }
        
        /// <summary>
        /// Gets the component of type T from the transform. If it doesn't exist, adds a new component of type T to the transform.
        /// </summary>
        /// <typeparam name="T">The type of component to get or add.</typeparam>
        /// <param name="transform">The transform to get the component from.</param>
        /// <returns>The component of type T.</returns>
        public static T GetOrAddComponent<T>(this Transform transform) where T : Component
        {
            T component = transform.GetComponent<T>();
            if (component == null)
            {
                component = transform.gameObject.AddComponent<T>();
            }
            return component;
        }
    }

}