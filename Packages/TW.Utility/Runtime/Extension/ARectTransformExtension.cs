using System.Linq;
using UnityEngine;

namespace TW.Utility.Extension
{
    public static class ARectTransformExtension
    {
        /// <summary>
        /// Determines whether a screen point is contained within the boundaries of the RectTransform.
        /// </summary>
        /// <param name="rectTransform">The RectTransform to check.</param>
        /// <param name="screenPoint">The screen point to check.</param>
        /// <param name="includeChildren">Optional parameter to include children RectTransforms in the check. Default is false.</param>
        /// <returns>True if the screen point is contained within the RectTransform boundaries, false otherwise.</returns>
        public static bool ContainsScreenPoint(this RectTransform rectTransform, Vector2 screenPoint, bool includeChildren = false)
        {
            return !includeChildren
                ? RectTransformUtility.RectangleContainsScreenPoint(rectTransform, screenPoint)
                : rectTransform.GetComponentsInChildren<RectTransform>().Any(x => x.ContainsScreenPoint(screenPoint));
        }
    }
}