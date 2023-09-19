using Sirenix.OdinInspector;
using TW.Utility.CustomComponent;

namespace TW.UI.CustomComponent
{
    public abstract class AUIVisualElement : AwaitableCachedMonoBehaviour
    {
        #region Unity Function

        protected virtual void Awake()
        {
            Setup();
            Config();
        }

        protected void OnValidate()
        {
            Config();
        }

        protected void Reset()
        {
            Setup();
            Config();
        }

        #endregion

        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        [Button, PropertyOrder(200)]
        protected abstract void Setup();

        [Button, PropertyOrder(200)]
        protected abstract void Config();
    }
}
