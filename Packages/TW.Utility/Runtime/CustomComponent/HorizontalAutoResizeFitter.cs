using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace TW.Utility.CustomComponent
{

    [RequireComponent(typeof(LayoutElement))]
    public class HorizontalAutoResizeFitter : ContentAutoResizeFitter
    {
        [SerializeField] private LayoutElement m_LayoutElement;
        [SerializeField] private bool m_ReverseArrangement;
        private readonly List<Vector3> childrenPositionList = new List<Vector3>();

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
            if (rectChildren.Count == 0)
            {
                return;
            }
            for (int i = 0; i < rectChildren.Count; i++)
            {

                rectChildren[i].SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, LayoutUtility.GetPreferredWidth(rectChildren[i]));
            }

            float rectWith = rectChildren.Sum(value => value.rect.width) + padding.left + padding.right;

            if (rectChildren.Count >= 2) rectWith += (rectChildren.Count - 1) * Spacing;

            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, rectWith);
            m_LayoutElement.preferredWidth = rectWith;
            
            float rectHeight = rectChildren.Max(value => value.rect.height) + padding.top + padding.bottom;
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, rectHeight);
            m_LayoutElement.preferredHeight = rectHeight;

            childrenPositionList.Clear();
            
            float offsetX = padding.left;
            float offsetY = padding.top;
            if (m_ReverseArrangement)
            {
                List<RectTransform> reverseRectChildren = Enumerable.Reverse(rectChildren).ToList();
                for (int i = 0; i < reverseRectChildren.Count; i++)
                {
                    reverseRectChildren[i].anchorMax = new Vector2(0, 1);
                    reverseRectChildren[i].anchorMin = new Vector2(0, 1);

                    Vector3 position = reverseRectChildren[i].anchoredPosition;

                    position.x = offsetX + reverseRectChildren[i].rect.width * reverseRectChildren[i].pivot.x;
                    position.y = -offsetY - reverseRectChildren[i].rect.height * (1 - reverseRectChildren[i].pivot.y);
                    childrenPositionList.Add(position);
                    offsetX += reverseRectChildren[i].rect.width + Spacing;

                    reverseRectChildren[i].SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, rectTransform.rect.height - padding.bottom - padding.top);
                }
                
                for (int i = 0; i < reverseRectChildren.Count; i++)
                {
                    reverseRectChildren[i].anchoredPosition = childrenPositionList[i];
                }
            }
            else
            {
                for (int i = 0; i < rectChildren.Count; i++)
                {
                    rectChildren[i].anchorMax = new Vector2(0, 1);
                    rectChildren[i].anchorMin = new Vector2(0, 1);

                    Vector3 position = rectChildren[i].anchoredPosition;

                    position.x = offsetX + rectChildren[i].rect.width * rectChildren[i].pivot.x;
                    position.y = -offsetY - rectChildren[i].rect.height * (1 - rectChildren[i].pivot.y);
                    childrenPositionList.Add(position);
                    offsetX += rectChildren[i].rect.width + Spacing;

                    rectChildren[i].SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, rectTransform.rect.height - padding.bottom - padding.top);
                }
                for (int i = 0; i < rectChildren.Count; i++)
                {
                    rectChildren[i].anchoredPosition = childrenPositionList[i];
                }
            }



        }

    } 
}
