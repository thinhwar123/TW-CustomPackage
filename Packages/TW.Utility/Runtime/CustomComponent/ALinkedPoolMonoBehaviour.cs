using Sirenix.OdinInspector;
using UnityEngine;

namespace TW.Utility.CustomComponent
{
    public class ALinkedPoolMonoBehaviour<T> : ABetterMonoBehaviour where T : MonoBehaviour
    {
        private static T Root { get; set; }
        private T NextNode { get; set; }
#if UNITY_EDITOR
        [ShowInInspector] private T Root_Editor => Root;
        [ShowInInspector] private T NextNode_Editor => NextNode;

#endif

        private T Create(Vector3 position, Quaternion rotation, Transform parent)
        {
            return Instantiate(this as T, position, rotation, parent);
        }

        public T Get(Vector3 position, Quaternion rotation, Transform parent)
        {
            if (Root == null)
            {
                T newInstance = Create(position, rotation, parent);
                newInstance.gameObject.SetActive(true);
                return newInstance;
            }

            ALinkedPoolMonoBehaviour<T> node = Root as ALinkedPoolMonoBehaviour<T>;
            Root = node!.NextNode;

            node.NextNode = null;
            node.transform.SetParent(parent);
            node.transform.SetPositionAndRotation(position, rotation);
            node.gameObject.SetActive(true);

            return node as T;
        }

        public void Release()
        {
            T self = this as T;
            NextNode = Root;
            Root = self;
            gameObject.SetActive(false);
        }
    }
}