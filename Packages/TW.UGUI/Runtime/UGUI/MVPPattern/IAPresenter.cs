using System;
using Cysharp.Threading.Tasks;

namespace TW.UGUI.MVPPattern
{
    public interface IAPresenter
    {
        UniTask Initialize(Memory<object> args);
    }
}