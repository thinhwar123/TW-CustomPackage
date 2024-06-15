using System;
using Cysharp.Threading.Tasks;

namespace TW.UGUI.MVPPattern
{
    public interface IAView
    {
        UniTask Initialize(Memory<object> args);
    }
}