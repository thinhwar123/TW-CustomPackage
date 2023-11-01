using Sirenix.OdinInspector;
using UnityEngine;
using Sirenix.Utilities;

namespace TW.UI.CustomStyleSheet
{
    [GlobalConfig("Assets/Resources/GlobalConfig/")]
    public class APropertyGlobalConfig : GlobalConfig<APropertyGlobalConfig>
    {
        [field: SerializeField] public APropertyConfig[] PropertyConfigs {get; private set;}
    }
    [System.Serializable]
    public class APropertyConfig
    {
        public enum EPropertyValueType
        {
            String = 0,
            Float = 1,
            Sprite = 2,
            Color = 3,
            Font = 4,
            FontSprite = 5,
            FontStyle = 6,
            Vector2 = 7,
            Audio = 8,
            Special = 20,
        }
        [field: SerializeField, HideLabel, HorizontalGroup("Config")] public EPropertyValueType PropertyValueType {get; private set;}
        [field: SerializeField, HideLabel, HorizontalGroup("Config")] public string PropertyName {get; private set;}
        [field: SerializeField, HideLabel, HorizontalGroup("Config")] public string Unit {get; private set;}
        [field: SerializeField, ShowIf("@PropertyValueType == EPropertyValueType.Special")] public string[] SpecialOptions {get; private set;}
    }
}