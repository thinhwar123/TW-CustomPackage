using UnityEngine;
using UnityEngine.UI;

namespace TW.Utility.CustomComponent
{
    public abstract class ContentAutoResizeFitter : LayoutGroup
    {
        [SerializeField] protected float m_Spacing = 0;
        /// <summary>
        /// The spacing to use between layout elements in the layout group.
        /// </summary>
        public float Spacing
        {
            get => m_Spacing;
            set => SetProperty(ref m_Spacing, value);
        }
        protected abstract void Calculate();

#if UNITY_EDITOR

        private int m_Capacity = 10;
        private Vector2[] m_Sizes = new Vector2[10];

        protected virtual void Update()
        {
            if (Application.isPlaying)
                return;

            int count = transform.childCount;

            if (count > m_Capacity)
            {
                if (count > m_Capacity * 2)
                    m_Capacity = count;
                else
                    m_Capacity *= 2;

                m_Sizes = new Vector2[m_Capacity];
            }

            // If children size change in editor, update layout (case 945680 - Child GameObjects in a Horizontal/Vertical Layout Group don't display their correct position in the Editor)
            bool dirty = false;
            for (int i = 0; i < count; i++)
            {
                if (transform.GetChild(i) is not RectTransform t || t.sizeDelta == m_Sizes[i]) continue;

                dirty = true;
                m_Sizes[i] = t.sizeDelta;
            }

            if (dirty)
                LayoutRebuilder.MarkLayoutForRebuild(transform as RectTransform);
        }

#endif
    } 
}
