using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TW.Utility.Component;
using UnityEngine;

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
