using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using TW.UI.CustomStyleSheet;
using TW.Utility.CustomComponent;
using UnityEditor;
using UnityEngine;

namespace TW.UI.CustomStyleSheet
{
    public class AVisualElement : AwaitableCachedMonoBehaviour
    {
        public enum Type
        {
            VisualElement = 0,
            Button = 1,
            Text = 2,
            Image = 3,
            Toggle = 4,
            Slider = 5,
            Scrollbar = 6,
            ScrollView = 7,
            Dropdown = 8,
            InputField = 9,
            ScrollRect = 10,
            Mask = 11,
            RawImage = 12,
        }
        public enum State
        {
            Default = 0,
            Open = 1,
            Close = 2,
            Clicked = 3,
            Selected = 4,
            Active = 5,
            Inactive = 6,
        }

        [field: SerializeField, HideLabel] public ACustomStyleSheet ACustomStyleSheet { get; private set; }
        
        private List<Tween> TweenList {get; set;} = new List<Tween>();
        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }
        
#if UNITY_EDITOR
        [OnInspectorGUI]
        public void UpdateStyleSheet()
        {
            if (PrefabUtility.GetCorrespondingObjectFromOriginalSource(gameObject) != null) return;
            ACustomStyleSheet.UpdateStyleSheet();
            ACustomStyleSheet.ApplyStyleSheet(this);
        }
#endif
    }
}
