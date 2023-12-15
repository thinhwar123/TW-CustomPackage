using TMPro;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TW.Utility.Tool
{
#if UNITY_EDITOR
    public class RemoveUnnecessaryRayCastTarget : Editor
    {
        private static GameObject[] SelectGameObjects { get; set; }
        [MenuItem("GameObject/Remove Unnecessary RayCast Target", true)]
        private static bool CanMergeTexture2D()
        {
            return Selection.gameObjects != null && Selection.gameObjects.Length > 0;
        }

        [MenuItem("GameObject/Remove Unnecessary RayCast Target")]
        private static void DoMergeTexture2D()
        {
            SelectGameObjects = Selection.gameObjects;
            
            foreach (GameObject gameObject in SelectGameObjects)
            {
                RemoveRayCastTarget(gameObject);
            }

        }

        private static void RemoveRayCastTarget(GameObject gameObject)
        {
            TextMeshProUGUI[] textMesh = gameObject.GetComponentsInChildren<TextMeshProUGUI>(true);
            foreach (TextMeshProUGUI text in textMesh)
            {
                text.raycastTarget = false;
            }
            
            Image[] images = gameObject.GetComponentsInChildren<Image>(true);
            foreach (Image image in images)
            {
                image.raycastTarget = false;
            }
            
            Button[] buttons = gameObject.GetComponentsInChildren<Button>(true);
            foreach (Button button in buttons)
            {
                if (button.targetGraphic.TryGetComponent(out Image image))
                {
                    EditorUtility.SetDirty(image);
                    image.raycastTarget = true;
                }
            }
            
            Toggle[] toggles = gameObject.GetComponentsInChildren<Toggle>(true);
            foreach (Toggle toggle in toggles)
            {
                if (toggle.targetGraphic.TryGetComponent(out Image image))
                {
                    EditorUtility.SetDirty(image);
                    image.raycastTarget = true;
                }
            }
            
            Scrollbar[] scrollbars = gameObject.GetComponentsInChildren<Scrollbar>(true);
            foreach (Scrollbar scrollbar in scrollbars)
            {
                if (scrollbar.targetGraphic.TryGetComponent(out Image image))
                {
                    EditorUtility.SetDirty(image);
                    image.raycastTarget = true;
                }
            }
            
            ScrollRect[] scrollRects = gameObject.GetComponentsInChildren<ScrollRect>(true);
            foreach (ScrollRect scrollRect in scrollRects)
            {
                if (scrollRect.content.TryGetComponent(out Image image))
                {
                    EditorUtility.SetDirty(image);
                    image.raycastTarget = true;
                }
            }
        }
    }
#endif
}
