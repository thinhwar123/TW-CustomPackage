using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
#endif

namespace TW.GUI
{
    [Serializable]
    public class APresetProperties
    {
#if UNITY_EDITOR
        [field: SerializeField]
        [field: OnValueChanged(nameof(OnPresetPropertiesTypeChanged))]
        [field: ValueDropdown(nameof(CustomAddPresetProperties), IsUniqueList = true,
            DrawDropdownForListElements = false,
            DropdownTitle = "Add Property")]
        [field: ListDrawerSettings(CustomRemoveElementFunction = nameof(CustomRemovePresetProperty))]
        [field: DelayedProperty]
        public List<APresetProperty> PresetProperties { get; set; } = new();

        private List<APresetProperty.Type> PresetPropertiesType { get; set; } = new();
        private AVisualElement AVisualElement { get; set; }
        private APreset APreset { get; set; }
        
        public void Init(AVisualElement.EType visualElementType , AVisualElement aVisualElement = null, APreset aPreset = null)
        {
            PresetPropertiesType = AVisualElementPresetGlobalConfig.Instance.GetPresetPropertiesType(visualElementType);
            AVisualElement = aVisualElement;
            APreset = aPreset;
        }
        private IEnumerable CustomAddPresetProperties()
        {
            return PresetPropertiesType
                .Select(presetType => new APresetProperty(presetType))
                .Except(PresetProperties)
                .Select(x => x)
                .AppendWith(PresetProperties)
                .Select(x => new ValueDropdownItem(x.PresetPropertyType.ToString(), x));
        }

        private void OnPresetPropertiesTypeChanged()
        {
            PresetProperties.Sort((x, y) => x.PresetPropertyType.CompareTo(y.PresetPropertyType));
        }

        private void CustomRemovePresetProperty(APresetProperty presetProperty)
        {
            PresetProperties.Remove(presetProperty);
            if (APreset == null || AVisualElement == null) return;
            if (presetProperty.IsOverride(APreset, AVisualElement))
            {
                APreset.Apply(AVisualElement);
            }
        }
        public void UpdateOverrideProperties()
        {
            if (APreset == null || AVisualElement == null) return;
            
            APreset.PresetProperties.PresetProperties.ForEach(p1 =>
            {
                if ( PresetProperties.Any(p2 => p2.PresetPropertyType == p1.PresetPropertyType)) return;
                APresetProperty presetProperty = new(p1.PresetPropertyType);
                if (!presetProperty.IsOverride(APreset, AVisualElement)) return;
                
                PresetProperties.Add(presetProperty);
                PresetProperties.Sort((x, y) => x.PresetPropertyType.CompareTo(y.PresetPropertyType));
            });
        }
        public APresetProperties IgnoreProperties(APresetProperties ignorePresetProperties)
        {
            return new APresetProperties()
            {
                PresetProperties = PresetProperties.Where(p => ignorePresetProperties.PresetProperties
                    .All(ignore => ignore.PresetPropertyType != p.PresetPropertyType)).ToList(),
            };
        }
#endif
    }

#if UNITY_EDITOR
    internal class PresetPropertiesDrawer : OdinValueDrawer<APresetProperties>
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            Property.Children[0].Draw(label);
        }
    }
#endif
}