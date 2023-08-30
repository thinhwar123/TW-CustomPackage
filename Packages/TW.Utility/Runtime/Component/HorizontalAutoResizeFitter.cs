using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace TW.Utility.Component
{

    [RequireComponent(typeof(LayoutElement))]
    public class HorizontalAutoResizeFitter : ContentAutoResizeFitter
    {
        [SerializeField] private LayoutElement m_LayoutElement;
        [SerializeField] private float m_ResizeSpeed;
        private readonly List<Vector3> m_ChildrenPositionList = new List<Vector3>();

        protected HorizontalAutoResizeFitter()
        {

        }
        /// <summary>
        /// Called by the layout system. Also see ILayoutElement
        /// </summary>
        public override void CalculateLayoutInputHorizontal()
        {
            base.CalculateLayoutInputHorizontal();
            Calculate();
        }

        /// <summary>
        /// Called by the layout system. Also see ILayoutElement
        /// </summary>
        public override void CalculateLayoutInputVertical()
        {
            Calculate();
        }

        /// <summary>
        /// Called by the layout system. Also see ILayoutElement
        /// </summary>
        public override void SetLayoutHorizontal()
        {
        }

        /// <summary>
        /// Called by the layout system. Also see ILayoutElement
        /// </summary>
        public override void SetLayoutVertical()
        {
        }
#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
        }
#endif
        [Sirenix.OdinInspector.Button]
        protected override void Calculate()
        {
            if (m_LayoutElement == null)
            {
                m_LayoutElement = GetComponent<LayoutElement>();
            }
            for (int i = 0; i < rectChildren.Count; i++)
            {

                rectChildren[i].SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, LayoutUtility.GetPreferredWidth(rectChildren[i]));
            }

            float rectWith = rectChildren.Sum(value => value.rect.width) + padding.left + padding.right;

            if (rectChildren.Count >= 2) rectWith += (rectChildren.Count - 1) * Spacing;

            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, rectWith);
            m_LayoutElement.preferredWidth = rectWith;

            m_ChildrenPositionList.Clear();
            float offsetX = padding.left;
            float offsetY = padding.top;
            for (int i = 0; i < rectChildren.Count; i++)
            {
                rectChildren[i].anchorMax = new Vector2(0, 1);
                rectChildren[i].anchorMin = new Vector2(0, 1);

                Vector3 position = rectChildren[i].anchoredPosition;

                position.x = offsetX + rectChildren[i].rect.width * rectChildren[i].pivot.x;
                position.y = -offsetY - rectChildren[i].rect.height * (1 - rectChildren[i].pivot.y);
                m_ChildrenPositionList.Add(position);
                offsetX += rectChildren[i].rect.width + Spacing;

                rectChildren[i].SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, rectTransform.rect.height - padding.bottom - padding.top);
            }

            for (int i = 0; i < rectChildren.Count; i++)
            {
                rectChildren[i].anchoredPosition = m_ChildrenPositionList[i];
            }
        }

    } 
}
