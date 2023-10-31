using System.Collections;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TW.UI.CustomComponent
{
    public class AUIRepeatableButton : AUIButton
    {
        private const float MinTimeInterval = 0.1f;
        private const float MaxTimeInterval = 0.5f;
        private float m_CurrentTimeInterval;
        private Coroutine m_RepeatCoroutine;
        private bool m_IsClickDone;

        protected override void Init()
        {
            MainButton.OnPointerDownAction.AddListener((eventData) =>
            {
                if (eventData.button != PointerEventData.InputButton.Left) return;
                m_IsClickDone = false;
                m_RepeatCoroutine = StartCoroutine(CoRepeat());
            });

            MainButton.OnPointerUpAction.AddListener((eventData) =>
            {
                if (eventData.button != PointerEventData.InputButton.Left) return;
                StopCoroutine(m_RepeatCoroutine);
                if (!m_IsClickDone)
                {
                    PressRepeat();
                }
            });

            MainButton.OnPointerExitAction.AddListener((eventData) =>
            {
                if (!MainButton.IsPointerDown) return;
                StopCoroutine(m_RepeatCoroutine);
                if (!m_IsClickDone)
                {
                    PressRepeat();
                }
            });
        }

        [SuppressMessage("ReSharper", "IteratorNeverReturns")]
        private IEnumerator CoRepeat()
        {
            m_CurrentTimeInterval = MaxTimeInterval;
            while (true)
            {
                yield return new WaitForSeconds(m_CurrentTimeInterval);
                if (m_CurrentTimeInterval > MaxTimeInterval)
                {
                    m_CurrentTimeInterval = MaxTimeInterval;
                }

                if (m_CurrentTimeInterval > MinTimeInterval)
                {
                    m_CurrentTimeInterval -= 0.5f * m_CurrentTimeInterval;
                }

                PressRepeat();
                m_IsClickDone = true;
            }
        }

        private void PressRepeat()
        {
            OnClickButton?.Invoke();
        }

        public void SetTimeInterval(float time)
        {
            m_CurrentTimeInterval = time;
        }
    }
}
