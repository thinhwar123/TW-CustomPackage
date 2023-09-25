using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace TW.UI.CustomComponent
{
    public class AUIButton : AUIVisualElement
    {
        [field: SerializeField, InlineEditor] public AUIButtonConfig AUIButtonConfig { get; private set; }
        [field: SerializeField] public RectTransform RectTransform {get; private set;}
        [field: SerializeField] public ACustomButton MainButton { get; private set; }
        [field: ValueDropdown(nameof(CustomChangeButtonPreset), DrawDropdownForListElements = false)]
        [field: SerializeField] public string TextPreset { get; private set; }
        public UnityEvent OnClickButton { get; private set; } = new UnityEvent();
        public List<Tween> AnimTween { get; private set; } = new List<Tween>();

        protected override void Awake()
        {
            base.Awake();
            Init();
        }

        protected virtual void Init()
        {
            MainButton.OnPointerClickAction.AddListener((eventData) =>
            {
                if (eventData.button != PointerEventData.InputButton.Left) return;
                OnClickButton?.Invoke();
            });

            if (AUIButtonConfig != null)
            {
                AUIButtonConfig.SetupSoundEffect(this);
                AUIButtonConfig.SetupAnimEffect(this);
            }
        }

        public bool Interactable
        {
            get => MainButton.interactable;
            set => MainButton.interactable = value;
        }

        protected override void Setup()
        {
            if (MainButton == null)
            {
                MainButton = GetComponentInChildren<ACustomButton>();
            }
        }

        protected override void Config()
        {
            RectTransform.sizeDelta = AUIButtonConfig.Presets.FirstOrDefault(p => p.m_PresetName == TextPreset).m_PresetSize;
        }
        private IEnumerable CustomChangeButtonPreset()
        {
            if (AUIButtonConfig == null) return null;
            return AUIButtonConfig.Presets.Select(x => x.m_PresetName);
        }
    }
}


