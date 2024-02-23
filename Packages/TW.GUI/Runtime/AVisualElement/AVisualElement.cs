using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif


namespace TW.GUI
{
    public class AVisualElement : MonoBehaviour
    {
        public enum EType
        {
            Text,
            Image,
            Button,
            Rect,
        }
        
#if UNITY_EDITOR

        [field: SerializeField, HorizontalGroup("Action", 100), ToggleLeft, OnValueChanged(nameof(OnAutoApplyChanged))] 
        public bool AutoApply {get; private set;} = true;
        
        [field: SerializeField, HideLabel, HorizontalGroup("MainProperty", 100), OnValueChanged(nameof(OnVisualElementTypeChanged))] 
        public EType VisualElementType {get; private set;}
        [field: SerializeField, HideLabel, HorizontalGroup("MainProperty"), ValueDropdown(nameof(GetPresetList)), OnValueChanged(nameof(OnPresetChanged))] 
        public APreset Preset {get; private set;}
        
        [field: InfoBox("Some properties of this preset are overridden. Delete properties if don't need", InfoMessageType.Warning, nameof(HasOverridePresetProperties))]
        [field: SerializeField]
        public APresetProperties OverridePresetProperties {get; private set;} = new();
        private CancellationTokenSource CancellationTokenSource { get; set; }
        private IEnumerable<ValueDropdownItem> GetPresetList()
        {
            return AVisualElementPresetGlobalConfig.Instance.GetPresets(VisualElementType)
                ?.Select(preset => new ValueDropdownItem(preset.name, preset));
        }
        [Button(SdfIconType.InfoCircle, ""), HorizontalGroup("MainProperty", 20)]
        private void SelectCurrentPreset()
        {
            if (Preset == null) return;
            EditorGUIUtility.PingObject(Preset);
        }
        private void OnVisualElementTypeChanged()
        {
            AVisualElementPresetGlobalConfig.Instance.UpdateAllPreset();
            Preset = AVisualElementPresetGlobalConfig.Instance.GetPresets(VisualElementType)?[0];
            OverridePresetProperties.Init(VisualElementType, this, Preset);
            if (AutoApply)
            {
                ApplyPreset();
            }
        }
        private void OnPresetChanged()
        {
            OverridePresetProperties.Init(VisualElementType, this, Preset);
            if (AutoApply)
            {
                ApplyPreset();
            }
        }
        private void OnAutoApplyChanged()
        {
            OverridePresetProperties.Init(VisualElementType, this, Preset);
            if (AutoApply)
            {
                ApplyPreset();
            }
        }

        private bool HasOverridePresetProperties()
        {
            return OverridePresetProperties.PresetProperties.Count > 0;
        }

        [OnInspectorInit]
        private void OnInspectorInit()
        {
            OverridePresetProperties.Init(VisualElementType, this, Preset);
            CancellationTokenSource = new CancellationTokenSource();
            EditorUtility.SetDirty(this);
            _ = UpdateProperties(CancellationTokenSource.Token);
        }
        [OnInspectorDispose]
        private void OnInspectorDispose()
        {
            CancellationTokenSource?.Cancel();
            CancellationTokenSource?.Dispose();
            CancellationTokenSource = null;
        }
        private async UniTask UpdateProperties(CancellationToken cancellationToken)
        {
            if (CancellationTokenSource.IsCancellationRequested) return;
            OverridePresetProperties.UpdateOverrideProperties();
            await UniTask.Delay(1000, cancellationToken: cancellationToken);
            _ = UpdateProperties(cancellationToken);
        }

        [Button, ShowIf("@!AutoApply"), HorizontalGroup("Action")]
        public void ApplyPreset()
        {
            if (Preset == null) return;
            Preset.Apply(this);
        }
#endif
    }

}