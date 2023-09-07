using System.Threading;

namespace TW.Utility.CustomComponent
{
    public abstract class AwaitableCachedMonoBehaviour : CachedMonoBehaviour
    {
        private CancellationTokenSource m_MyCancellationTokenSource;

        public CancellationToken OnDestroyCancellationToken
        {
            get
            {
                m_MyCancellationTokenSource ??= new CancellationTokenSource();
                return m_MyCancellationTokenSource.Token;
            }
        }

        protected virtual void OnDestroy()
        {
            m_MyCancellationTokenSource?.Cancel();
            m_MyCancellationTokenSource?.Dispose();
        }
    }

}