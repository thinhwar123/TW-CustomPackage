using System;
using Cysharp.Threading.Tasks;

namespace TW.UGUI.MVPPattern
{
    public interface IAModel
    {
        UniTask Initialize(Memory<object> args);
    }
}