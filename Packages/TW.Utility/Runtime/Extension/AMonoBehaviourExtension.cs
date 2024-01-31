using UnityEngine;
#if UNITY_EDITOR
#endif

namespace TW.Utility.Extension
{
    public static class AMonoBehaviourExtension
    {
        /// <summary>
        /// Gets the first component of type T in the parent hierarchy, including the current Transform.
        /// Recursively searches through parent Transform objects until no parent with the desired component is found.
        /// </summary>
        /// <typeparam name="T">The type of component to retrieve.</typeparam>
        /// <param name="component">The Transform whose parent hierarchy to search.</param>
        /// <returns>The found component of type T, or null if not found in the hierarchy.</returns>
        public static T GetComponentInParentUntilNoParent<T>(this Transform component) where T : Component
        {
            return component.GetComponentInParent<T>() ?? component.transform.parent?.GetComponentInParentUntilNoParent<T>();
        }

        /// <summary>
        /// Gets the first component of type T in the parent hierarchy, including the current Component's Transform.
        /// Recursively searches through parent Transform objects until no parent with the desired component is found.
        /// </summary>
        /// <typeparam name="T">The type of component to retrieve.</typeparam>
        /// <param name="component">The Component whose parent hierarchy to search.</param>
        /// <returns>The found component of type T, or null if not found in the hierarchy.</returns>
        public static T GetComponentInParentUntilNoParent<T>(this Component component) where T : Component
        {
            return component.GetComponentInParent<T>() ?? component.transform.parent?.GetComponentInParentUntilNoParent<T>();
        }

        /// <summary>
        /// Gets the first component of type T in the parent hierarchy, including the current MonoBehaviour 's Transform.
        /// Recursively searches through parent Transform objects until no parent with the desired component is found.
        /// </summary>
        /// <typeparam name="T">The type of component to retrieve.</typeparam>
        /// <param name="monoBehaviour">The MonoBehaviour whose parent hierarchy to search.</param>
        /// <returns>The found component of type T, or null if not found in the hierarchy.</returns>
        public static T GetComponentInParentUntilNoParent<T>(this MonoBehaviour monoBehaviour) where T : Component
        {
            return monoBehaviour.GetComponentInParent<T>() ?? monoBehaviour.transform.parent?.GetComponentInParentUntilNoParent<T>();
        }
    }

}