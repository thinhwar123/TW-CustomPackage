using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using TW.Utility.Extension;
using UnityEngine;
using UnityEngine.UI;

namespace TW.UI.CustomComponent
{
    [RequireComponent(typeof(Canvas), typeof(GraphicRaycaster), typeof(CanvasGroup))]
    public class AUIPanel : AVisualElement
    {
        [field: SerializeField] public CanvasGroup CanvasGroup { get; private set; }
        [field: SerializeField] public RectTransform MainView { get; private set; }
        public List<Tween> AnimTween { get; private set; } = new List<Tween>();
        private AWaiter AnimWaiter { get; set; }

        #region Unity Function

        protected virtual void Reset()
        {
            CanvasGroup = GetComponent<CanvasGroup>();
            GetComponent<Canvas>().overrideSorting = true;
            GetComponent<Canvas>().sortingOrder = 0;
        }

        #endregion

        #region Panel Function

        public virtual T OpenPanel<T>() where T : AUIPanel
        {
            SetActive(true);
            return this as T;
        }

        public virtual T ClosePanel<T>() where T : AUIPanel
        {
            SetActive(false);
            return this as T;
        }

        private void SetInteractable(bool interactable)
        {
            CanvasGroup.interactable = interactable;
        }

        #endregion

        #region Debug

        [Button, FoldoutGroup("Debug Functions")]
        public void TestOpenAnim()
        {
            OpenPanel<AUIPanel>();
        }

        [Button, FoldoutGroup("Debug Functions")]
        public void TestCloseAnim()
        {
            ClosePanel<AUIPanel>();
        }

        #endregion
    }
}