using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Pool;

namespace TW.Utility.DesignPattern
{
    public class UnityPool<T> where T : MonoBehaviour
    {
        private T m_Prefab;
        private ObjectPool<T> m_Pool;
        private Transform m_Parent;
        private List<T> m_ActiveObject;

        private ObjectPool<T> Pool
        {
            get
            {
                if (m_Pool == null) throw new InvalidOperationException("You need to call InitPool before using it.");
                return m_Pool;
            }
            set => m_Pool = value;
        }

        protected void InitPool(T prefab, Transform parent = null, int initial = 10, int max = 20, bool collectionChecks = false)
        {
            m_Prefab = prefab;
            m_Parent = parent;
            m_ActiveObject = new List<T>();
            Pool = new ObjectPool<T>(
                CreateSetup,
                GetSetup,
                ReleaseSetup,
                DestroySetup,
                collectionChecks,
                initial,
                max);
        }

        #region Overrides

        protected virtual T CreateSetup() => UnityEngine.Object.Instantiate(m_Prefab, m_Parent);
        protected virtual void GetSetup(T obj) => obj.gameObject.SetActive(true);
        protected virtual void ReleaseSetup(T obj) => obj.gameObject.SetActive(false);
        protected virtual void DestroySetup(T obj) => UnityEngine.Object.Destroy(obj);

        #endregion

        #region Getters

        public T Get()
        {
            T obj = Pool.Get();
            m_ActiveObject.Add(obj);
            return obj;
        }
        public void Release(T obj)
        {
            m_ActiveObject.Remove(obj);
            Pool.Release(obj);
        }
        public void ReleaseAll()
        {
            for (int i = 0; i < m_ActiveObject.Count; i++)
            {
                Pool.Release(m_ActiveObject[i]);
            }
            m_ActiveObject.Clear();
        }
        #endregion

    }
}