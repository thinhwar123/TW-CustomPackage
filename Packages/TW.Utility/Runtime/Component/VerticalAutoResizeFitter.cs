using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace TW.Utility.Component
{
    [RequireComponent(typeof(LayoutElement))]
    public class VerticalAutoResizeFitter : ContentAutoResizeFitter
    {
        [SerializeField] private LayoutElement m_LayoutElement;
        [SerializeField] private float m_ResizeSpeed;
        private readonly List<Vector3> m_ChildrenPositionList = new List<Vector3>();


        protected VerticalAutoResizeFitter()
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
            //GetRectChildren();
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
            //Calculate();
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

                rectChildren[i].SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, LayoutUtility.GetPreferredHeight(rectChildren[i]));
            }

            float rectHeight = rectChildren.Sum(value => value.rect.height) + padding.top + padding.bottom;
            if (rectChildren.Count >= 2) rectHeight += (rectChildren.Count - 1) * Spacing;

            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, rectHeight);
            m_LayoutElement.preferredHeight = rectHeight;

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
                offsetY += rectChildren[i].rect.height + Spacing;

                rectChildren[i].SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, rectTransform.rect.width - padding.left - padding.right);
            }

            for (int i = 0; i < rectChildren.Count; i++)
            {
                rectChildren[i].anchoredPosition = m_ChildrenPositionList[i];
            }

        }

    } 
}
