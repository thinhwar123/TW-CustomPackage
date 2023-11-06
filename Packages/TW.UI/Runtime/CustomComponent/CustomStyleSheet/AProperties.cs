using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace TW.UI.CustomStyleSheet
{
    [System.Serializable]
    public class AProperties
    {
        [field: ValueDropdown(nameof(CustomAddPropertiesButton), IsUniqueList = true, DrawDropdownForListElements = false, DropdownTitle = "Modify Properties")]
        // [field: OnValueChanged(nameof(CustomChangeProperties))]
        [field: SerializeField, APropertyEditor] public List<AProperty> Properties {get; private set;}
        public int Count => Properties.Count();
        public AProperties()
        {
            Properties = new List<AProperty>();
        }
        public AProperty this[int i]
        {
            get => Properties[i];
            set => Properties[i] = value;
        }
        public AProperty this[string propertyName]
        {
            get => Properties.FirstOrDefault(x => x.PropertyName == propertyName);
            set => Properties[Properties.FindIndex(x => x.PropertyName == propertyName)] = value;
        }
        public bool Contains(string propertyName)
        {
            return Properties.Any(x => x.PropertyName == propertyName);
        }
        public bool TryGetProperty(string propertyName, out AProperty property)
        {
            property = Properties.FirstOrDefault(x => x.PropertyName == propertyName);
            return property != null && property.StringUnit != "auto";
        }
        public static implicit operator AProperties(List<AProperty> properties)
        {
            return new AProperties() {Properties = properties};
        }
        public List<AProperty> ToList()
        {
            return Properties;
        }
        public void TryAddProperty(AProperty property)
        {
            if (Properties.Any(x => x.PropertyName == property.PropertyName))
            {
                Properties[Properties.FindIndex(x => x.PropertyName == property.PropertyName)] = property;
                return;
            }
            Properties.Add(property);
        }
        public void TryAddProperties(AProperties properties)
        {
            if (properties == null) return;
            foreach (AProperty property in properties.Properties)
            {
                TryAddProperty(property);
            }
        }
        public void TryAddProperties(IEnumerable<AProperty> properties)
        {
            foreach (AProperty property in properties)
            {
                TryAddProperty(property);
            }
        }
        private IEnumerable CustomAddPropertiesButton()
        {
            return APropertyGlobalConfig.Instance.PropertyConfigs.Select(x => x.PropertyName)
                .Except(Properties.Select(x => x.PropertyName))
                .Select(x => new AProperty() { PropertyName = x })
                .AppendWith(Properties)
                .Select(x => new ValueDropdownItem(x.PropertyName.ToString(), x));
        }
        // private void CustomChangeProperties()
        // {
        //     List<string> propertyConfigsList = APropertyGlobalConfig.Instance.PropertyConfigs.Select(y => y.PropertyName).ToList();
        //     Properties = Properties.OrderBy(x => propertyConfigsList.IndexOf(x.PropertyName)).ToList();
        // }
    }
#if UNITY_EDITOR
    internal class APropertiesValueDrawer : Sirenix.OdinInspector.Editor.OdinValueDrawer<AProperties>
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            this.Property.Children[0].Draw(label);
        }
    }
#endif
}
