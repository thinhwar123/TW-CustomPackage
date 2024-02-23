using Sirenix.OdinInspector;
using UnityEngine;

namespace TW.GUI
{
    public class APreset : MonoBehaviour
    {
#if UNITY_EDITOR
        [field: SerializeField] public APresetProperties PresetProperties { get; set; } = new APresetProperties();

        public virtual void Reset()
        {
            OnReset();
        }

        public virtual void OnReset()
        {
            
        }
        [OnInspectorInit]
        public virtual void OnInspectorInit()
        {

        }
        public void Apply(AVisualElement aVisualElement)
        {
            PresetProperties.IgnoreProperties(aVisualElement.OverridePresetProperties).PresetProperties.ForEach(presetProperty =>
            {
                presetProperty.Apply(this, aVisualElement);
            });
        }
#endif
    }
}