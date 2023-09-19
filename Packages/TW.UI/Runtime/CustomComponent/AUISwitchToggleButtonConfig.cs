using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "AUISwitchToggleButtonConfig", menuName = "AUIConfig/AUISwitchToggleButtonConfig")]
public class AUISwitchToggleButtonConfig : ScriptableObject
{
    [field: SerializeField] public Color ActiveSwitchColor {get; private set;}
    [field: SerializeField] public Color ActiveBackgroundColor {get; private set;}
    [field: SerializeField] public Color DeActiveSwitchColor {get; private set;}
    [field: SerializeField] public Color DeActiveBackgroundColor {get; private set;}
    [field: SerializeField] public AudioClip OnClickSound {get; private set;}
    
    //override this to set sound effect when click button
    public virtual void SetupSoundEffect(AUISwitchToggleButton auiButton)
    {
        auiButton.OnClickButton.AddListener((value) =>
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
    public virtual void SetupAnimEffect(AUISwitchToggleButton auiButton)
    {
        auiButton.OnClickButton.AddListener((value) =>
        {
            auiButton.AnimTween.ForEach(t => t?.Kill());
            auiButton.AnimTween.Clear();
            auiButton.AnimTween.Add(auiButton.Switch.DOLocalMove(auiButton.TargetSwitchPosition, 0.3f));
            auiButton.AnimTween.Add(
                DOTween.To(() => auiButton.ImageBackground.color, 
                        x => auiButton.ImageBackground.color = x,
                        value ? ActiveBackgroundColor : DeActiveBackgroundColor, 0.3f));
            auiButton.AnimTween.Add(
                DOTween.To(() => auiButton.ImageSwitch.color, 
                    x => auiButton.ImageSwitch.color = x,
                    value ? ActiveSwitchColor : DeActiveSwitchColor, 0.3f));
        });
    }
}