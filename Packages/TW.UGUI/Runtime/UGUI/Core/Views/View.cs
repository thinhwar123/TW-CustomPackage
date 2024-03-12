using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using TW.UGUI.Shared;
using TW.UGUI.Utility;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TW.UGUI.Core.Views
{
    [RequireComponent(typeof(RectTransform), typeof(CanvasGroup))] 
    public abstract class View : UIBehaviour, IView, ITransform
    {
        [field: SerializeField] private bool UsePrefabNameAsIdentifier { get; set; } = true;
        [field: EnableIf("@!UsePrefabNameAsIdentifier")]
        [field: SerializeField] public string Identifier { get; set; }
        
        public virtual string Name
        {
            get
            {
                if (IsDestroyed() || gameObject == false)
                    return string.Empty;
                return gameObject.name;
            }

            set
            {
                if (IsDestroyed() || gameObject == false)
                    return;
                gameObject.name = value;
            }
        }
        
        private Transform CacheTransform { get; set; }
        public virtual Transform Transform
        {
            get
            {
                if (IsDestroyed())
                    return null;

                if (CacheTransform == false)
                    CacheTransform = gameObject.transform;

                return CacheTransform;
            }
        }
        
        private RectTransform CacheRectTransform { get; set; }
        public virtual RectTransform RectTransform
        {
            get
            {
                if (IsDestroyed())
                    return null;

                if (CacheRectTransform == false)
                    CacheRectTransform = gameObject.GetOrAddComponent<RectTransform>();

                return CacheRectTransform;
            }
        }
        private RectTransform CacheParent { get; set; }
        public virtual RectTransform Parent
        {
            get
            {
                if (IsDestroyed())
                {
                    return null;
                }

                return CacheParent;
            }

            internal set => CacheParent = value;
        }
        
        public virtual GameObject Owner => IsDestroyed() ? null : gameObject;
        
        public virtual bool ActiveSelf
        {
            get
            {
                GameObject o;
                return IsDestroyed() == false
                       && (o = gameObject) == true
                       && o.activeSelf;
            }

            set
            {
                if (IsDestroyed() 
                    || gameObject == false
                    || gameObject.activeSelf == value)
                    return;

                gameObject.SetActive(value);
            }
        }
        
        public virtual float Alpha
        {
            get
            {
                if (IsDestroyed() || gameObject == false)
                    return 0;

                if (CanvasGroup)
                    return CanvasGroup.alpha;

                return 1f;
            }
            set
            {
                if (IsDestroyed() || gameObject == false)
                    return;

                if (CanvasGroup)
                    CanvasGroup.alpha = value;
            }
        }
        
        public virtual bool Interactable
        {
            get
            {
                if (IsDestroyed() || gameObject == false)
                    return false;

                if (CanvasGroup)
                    return CanvasGroup.interactable;

                return true;
            }

            set
            {
                if (IsDestroyed() || gameObject == false)
                    return;

                if (CanvasGroup)
                    CanvasGroup.interactable = value;
            }
        }
        
        private CanvasGroup CacheCanvasGroup { get; set; }
        public virtual CanvasGroup CanvasGroup
        {
            get
            {
                if (IsDestroyed())
                    return null;

                if (CacheCanvasGroup == false)
                    CacheCanvasGroup = gameObject.GetOrAddComponent<CanvasGroup>();
                return CacheCanvasGroup;
            }
        }
        
        public virtual UnityScreenNavigatorSettings Settings { get; set; }

        protected void SetIdentifier()
        {
            Identifier = UsePrefabNameAsIdentifier
                ? gameObject.name.Replace("(Clone)", string.Empty)
                : Identifier;
        }

        protected static async UniTask WaitForAsync(IEnumerable<UniTask> tasks)
        {
            try
            {
                foreach (UniTask task in tasks)
                {
                    try
                    {
                        await task;
                    }
                    catch (System.Exception ex)
                    {
                        Debug.LogException(ex);
                    }
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogException(ex);
            }
        }
    }

}
