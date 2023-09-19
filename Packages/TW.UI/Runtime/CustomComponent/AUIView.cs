using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace TW.UI.CustomComponent
{
    public abstract class AUIView : AUIVisualElement
    {
        [field: SerializeField, InlineEditor] public AUIViewConfig AUIViewConfig { get; private set; }
        [field: SerializeField] public LayoutElement Header { get; private set; }
        [field: SerializeField] public LayoutElement Body { get; private set; }
        [field: SerializeField] public LayoutElement Footer { get; private set; }


        #region View Function

        protected override void Setup()
        {
            Header = GetComponentsInChildren<LayoutElement>().FirstOrDefault(x => x.name == "Header");
            Body = GetComponentsInChildren<LayoutElement>().FirstOrDefault(x => x.name == "Body");
            Footer = GetComponentsInChildren<LayoutElement>().FirstOrDefault(x => x.name == "Footer");
        }

        protected override void Config()
        {

        }

        #endregion
    }

}