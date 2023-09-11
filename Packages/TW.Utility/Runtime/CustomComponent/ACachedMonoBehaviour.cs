using UnityEngine;

namespace TW.Utility.CustomComponent
{
    public abstract class ACachedMonoBehaviour : MonoBehaviour
    {
        private Transform m_Transform;
        public Transform Transform => m_Transform = m_Transform != null ? m_Transform : transform;
    }

}
