using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Linq;
using UnityEngine.Events;

namespace TW.Utility.CustomType
{

    [System.Serializable]
    public class Probability<T>
    {
        [ShowInInspector, PropertyOrder(-1)]
        public int Total => m_ProbabilityValueList?.Sum(value => value.m_Chance) ?? 0;

        [OnInspectorInit(nameof(OnInspectorInit))]
        [ListDrawerSettings(CustomAddFunction = nameof(CustomAddFunction), CustomRemoveElementFunction = nameof(CustomRemoveElementFunction))]
        public List<ProbabilityValue<T>> m_ProbabilityValueList = new List<ProbabilityValue<T>>();

        public Probability(List<T> objectList)
        {
            for (int i = 0; i < objectList.Count; i++)
            {
                m_ProbabilityValueList.Add(new ProbabilityValue<T>(objectList[i], 1, UpdateTotalValue));
                
            }
        }

        private ProbabilityValue<T> CustomAddFunction()
        {
            return new ProbabilityValue<T>(UpdateTotalValue);
        }
        private void CustomRemoveElementFunction(ProbabilityValue<T> removeObject)
        {
            m_ProbabilityValueList.Remove(removeObject);
            UpdateTotalValue();
        }
        private void OnInspectorInit()
        {
            if (m_ProbabilityValueList == null) return;
            for (int i = 0; i < m_ProbabilityValueList.Count; i++)
            {
                m_ProbabilityValueList[i].OnValueChangeCallback = UpdateTotalValue;
            }
            UpdateTotalValue();
        }
        public void UpdateTotalValue()
        {
            m_ProbabilityValueList.ForEach(value =>
            {
                value.m_Percentage = value.m_Chance * 100.0f / Total;
            });

#if UNITY_EDITOR
            UnityEditor.AssetDatabase.SaveAssets();
            UnityEditor.AssetDatabase.Refresh();
#endif
        }
        public T GetItem(int num)
        {
            int stackChance = 0;
            for (int i = 0; i < m_ProbabilityValueList.Count; i++)
            {
                ProbabilityValue<T> value = m_ProbabilityValueList[i];
                stackChance += value.m_Chance;
                if (num < stackChance)
                {
                    return value.m_Object;
                }
            }
            return default;
        }

        public T GetRandomItem()
        {
            int r = Random.Range(0, this.Total);
            return GetItem(r);
        }
        public void ClearTable()
        {
            this.m_ProbabilityValueList.Clear();
        }
        public void Clone(Probability<T> otherProbability)
        {
            ClearTable();
            for (int i = 0; i < otherProbability.m_ProbabilityValueList.Count; i++)
            {
                m_ProbabilityValueList.Add(new ProbabilityValue<T>(otherProbability.m_ProbabilityValueList[i].m_Object, otherProbability.m_ProbabilityValueList[i].m_Chance, UpdateTotalValue));
            }
        }
        public int GetValue(T obj)
        {
            return m_ProbabilityValueList.FirstOrDefault(value => EqualityComparer<T>.Default.Equals(value.m_Object, obj))!.m_Chance;
        }
        public float GetPercentage(T obj)
        {
            return m_ProbabilityValueList.FirstOrDefault(value => EqualityComparer<T>.Default.Equals(value.m_Object, obj))!.m_Percentage;
        }


    }
    [System.Serializable]
    public class ProbabilityValue<T>
    {
        [HideLabel]
        public T m_Object;
        [HorizontalGroup("Split", LabelWidth = 50)]
        [OnValueChanged(nameof(OnValueChange))]
        public int m_Chance;
        [HorizontalGroup("Split/Right", LabelWidth = 70)]
        [SuffixLabel("%", true)]
        [ReadOnly]
        public float m_Percentage;

        public UnityAction OnValueChangeCallback;

        public ProbabilityValue(T obj, int chance, UnityAction onValueChangeCallback)
        {
            this.m_Object = obj;
            this.m_Chance = chance;
            this.OnValueChangeCallback = onValueChangeCallback;
        }
        public void AddChance(int chance)
        {
            this.m_Chance += chance;
        }
        public ProbabilityValue(UnityAction onValueChangeCallback)
        {
            this.OnValueChangeCallback = onValueChangeCallback;
        }
        private void OnValueChange()
        {
#if UNITY_EDITOR
            OnValueChangeCallback?.Invoke();
#endif
        }
    } 
}