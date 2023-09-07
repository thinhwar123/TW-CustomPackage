using UnityEngine;

namespace TW.Utility.Component
{
    public abstract class CachedMonoBehaviour : MonoBehaviour
    {
        private Transform m_Transform;
        public Transform Transform => m_Transform = m_Transform != null ? m_Transform : transform;
    }

}
