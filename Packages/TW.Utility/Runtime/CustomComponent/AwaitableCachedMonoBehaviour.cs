using System.Threading;

namespace TW.Utility.CustomComponent
{
    public abstract class AwaitableCachedMonoBehaviour : ACachedMonoBehaviour
    {
        protected CancellationTokenSource OnDestroyCancellationTokenSource;

        public CancellationToken OnDestroyCancellationToken
        {
            get
            {
                OnDestroyCancellationTokenSource ??= new CancellationTokenSource();
                return OnDestroyCancellationTokenSource.Token;
            }
        }

        protected virtual void OnDestroy()
        {
            OnDestroyCancellationTokenSource?.Cancel();
            OnDestroyCancellationTokenSource?.Dispose();
        }
    }

}