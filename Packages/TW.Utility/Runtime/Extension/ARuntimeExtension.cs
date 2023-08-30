using UnityEngine;
using UnityEngine.EventSystems;

namespace TW.Utility.Extension
{
    public static class ARuntimeExtension
    {
        /// <summary>
        /// This is a public static boolean variable that can be used to check if the UI is currently being touched by the user.
        /// </summary>
        private static bool isTouchingUI;
        /// <summary>
        /// This method checks if the mouse pointer or touch is currently over a UI element and returns a boolean value indicating whether it is or not.
        /// </summary>
        /// <returns></returns>

        public static bool IsPointerOverUIGameObject()
        {
            //check mouse
            if (EventSystem.current.IsPointerOverGameObject())
                return true;

            //check touch
            if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
            {
                if (EventSystem.current.IsPointerOverGameObject(Input.touches[0].fingerId))
                {
                    isTouchingUI = true;
                    return true;
                }
            }
            if (Input.GetKeyUp(KeyCode.Mouse0) && isTouchingUI)
            {
                isTouchingUI = false;
                return true;
            }

            return isTouchingUI;
        }
    } 
}
