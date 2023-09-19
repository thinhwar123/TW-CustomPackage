using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TW.Utility.CustomComponent;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace TW.UI.CustomComponent
{
    public class AUIFitterView : AUIView
    {
        [field: SerializeField] public VerticalAutoResizeFitter VerticalAutoResizeFitter { get; private set; }
        [field: SerializeField] public bool CanResizeHeader { get; private set; }
        [field: SerializeField] public bool CanResizeBody { get; private set; }
        [field: SerializeField] public bool CanResizeFooter { get; private set; }


        #region View Function

        protected override void Setup()
        {
            base.Setup();
            VerticalAutoResizeFitter = GetComponentsInChildren<VerticalAutoResizeFitter>()
                .FirstOrDefault(x => x.name == "LayoutGroup");
            CanResizeHeader = !Header.TryGetComponent<ContentAutoResizeFitter>(out _);
            CanResizeBody = !Body.TryGetComponent<ContentAutoResizeFitter>(out _);
            CanResizeFooter = !Footer.TryGetComponent<ContentAutoResizeFitter>(out _);
        }

        protected override void Config()
        {
            if (AUIViewConfig == null) return;
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
            if (VerticalAutoResizeFitter == null) return;
            VerticalAutoResizeFitter.padding = AUIViewConfig.Padding;
            VerticalAutoResizeFitter.Spacing = AUIViewConfig.Spacing;

            if (CanResizeHeader)
            {
                Header.preferredHeight = AUIViewConfig.HeaderHeight;
            }

            if (CanResizeBody)
            {
                Body.preferredHeight = AUIViewConfig.BodyHeight;
            }

            if (CanResizeFooter)
            {
                Footer.preferredHeight = AUIViewConfig.FooterHeight;
            }
#if UNITY_EDITOR
            LayoutRebuilder.ForceRebuildLayoutImmediate(VerticalAutoResizeFitter.GetComponent<RectTransform>());
#endif
        }

        #endregion
    }

}