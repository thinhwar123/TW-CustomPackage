using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TW.UI.CustomComponent
{
    [CreateAssetMenu(fileName = "AUIButtonConfig", menuName = "AUIConfig/AUIButtonConfig")]

    public class AUIButtonConfig : ScriptableObject
    {
        [Serializable]
        public struct Preset
        {
            public string m_PresetName;
            public Vector2 m_PresetSize;
        }
        
        [field: SerializeField] public AudioClip OnClickSound { get; private set; }
        [field: SerializeField] public Preset[] Presets { get; private set; }

        //override this to set sound effect when click button
        public virtual void SetupSoundEffect(AUIButton auiButton)
        {
            auiButton.OnClickButton.AddListener(() =>
            {
                if (OnClickSound == null) return;
                GameObject buttonSoundEffect = new GameObject("Button Sound Effect");
                AudioSource audioSource = buttonSoundEffect.AddComponent<AudioSource>();
                audioSource.clip = OnClickSound;
                audioSource.Play();
                Destroy(buttonSoundEffect, OnClickSound.length);
            });
        }

        // override this to set animation effect when click button
        public virtual void SetupAnimEffect(AUIButton auiButton)
        {
            auiButton.MainButton.OnPointerDownAction.AddListener((eventData) =>
            {
                if (eventData.button != PointerEventData.InputButton.Left) return;
                auiButton.AnimTween.ForEach(t => t?.Kill());
                auiButton.AnimTween.Clear();
                auiButton.AnimTween.Add(auiButton.MainButton.Transform.DOScale(Vector3.one * 0.95f, 0.3f));
            });

            auiButton.MainButton.OnPointerUpAction.AddListener((eventData) =>
            {
                if (eventData.button != PointerEventData.InputButton.Left) return;
                auiButton.AnimTween.ForEach(t => t?.Kill());
                auiButton.AnimTween.Clear();
                auiButton.AnimTween.Add(auiButton.MainButton.Transform.DOScale(Vector3.one, 0.3f));
            });

            auiButton.MainButton.OnPointerExitAction.AddListener((eventData) =>
            {
                if (!auiButton.MainButton.IsPointerDown) return;
                auiButton.AnimTween.ForEach(t => t?.Kill());
                auiButton.AnimTween.Clear();
                auiButton.AnimTween.Add(auiButton.MainButton.Transform.DOScale(Vector3.one, 0.3f));
            });
            
            auiButton.MainButton.OnDestroyButtonAction.AddListener(() =>
            {
                auiButton.AnimTween.ForEach(t => t?.Kill());
                auiButton.AnimTween.Clear();
            });
        }

    }
}