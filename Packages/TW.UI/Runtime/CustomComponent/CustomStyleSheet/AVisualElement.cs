using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TW.UI.CustomStyleSheet;
using TW.Utility.CustomComponent;
using UnityEditor;
using UnityEngine;

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
        Hover = 1,
        Active = 2,
        Focus = 3,
        Checked = 4,
        Disabled = 5,
    }
    [field: SerializeField, HideLabel] public ACustomStyleSheet ACustomStyleSheet {get; private set;}
    
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
