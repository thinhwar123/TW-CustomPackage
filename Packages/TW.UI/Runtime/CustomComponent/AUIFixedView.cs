using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace TW.UI.CustomComponent
{
    public class AUIFixedView : AUIView
    {
        [field: SerializeField] public VerticalLayoutGroup VerticalLayoutGroup { get; private set; }

        #region View Function

        protected override void Setup()
        {
            base.Setup();
            VerticalLayoutGroup = GetComponentsInChildren<VerticalLayoutGroup>()
                .FirstOrDefault(x => x.name == "LayoutGroup");
        }

        protected override void Config()
        {
            if (AUIViewConfig == null) return;
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
            if (VerticalLayoutGroup == null) return;
            VerticalLayoutGroup.padding = AUIViewConfig.Padding;
            VerticalLayoutGroup.spacing = AUIViewConfig.Spacing;
            Header.preferredHeight = AUIViewConfig.HeaderHeight;
            Body.preferredHeight = AUIViewConfig.BodyHeight;
            Footer.preferredHeight = AUIViewConfig.FooterHeight;
        }

        #endregion
    }

}