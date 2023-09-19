using DG.Tweening;
using UnityEngine;

namespace TW.UI.CustomComponent
{
    [CreateAssetMenu(fileName = "AUIPanelConfig", menuName = "AUIConfig/AUIPanelConfig")]
    public class AUIPanelConfig : ScriptableObject
    {
        [field: SerializeField] public float OpenAnimDuration { get; private set; }
        [field: SerializeField] public float CloseAnimDuration { get; private set; }

        public void SetupAnimEffect(AUIPanel auiPanel)
        {
            auiPanel.OnStartOpenPanel.AddListener(() =>
            {
                auiPanel.AnimTween.ForEach(t => t?.Kill());
                auiPanel.AnimTween.Clear();
                auiPanel.MainView.localPosition = Vector3.up * -2000;
                auiPanel.CanvasGroup.alpha = 0;

                auiPanel.AnimTween.Add(auiPanel.MainView.DOLocalMoveY(0, OpenAnimDuration).SetEase(Ease.OutBack));
                auiPanel.AnimTween.Add(DOTween.To(() => auiPanel.CanvasGroup.alpha, x => auiPanel.CanvasGroup.alpha = x,
                    1, OpenAnimDuration));
            });

            auiPanel.OnStartClosePanel.AddListener(() =>
            {
                auiPanel.AnimTween.ForEach(t => t?.Kill());
                auiPanel.AnimTween.Clear();
                auiPanel.AnimTween.Add(auiPanel.MainView.DOLocalMoveY(-2000, CloseAnimDuration).SetEase(Ease.InBack));
                auiPanel.AnimTween.Add(DOTween.To(() => auiPanel.CanvasGroup.alpha, x => auiPanel.CanvasGroup.alpha = x,
                    0, CloseAnimDuration));
            });
        }
    }
}
