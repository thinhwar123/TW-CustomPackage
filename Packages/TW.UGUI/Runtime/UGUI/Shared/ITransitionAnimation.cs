using System;
using Cysharp.Threading.Tasks;
using TW.UGUI.Animation;
using UnityEngine;

namespace TW.UGUI.Shared
{
    public interface ITransitionAnimation : ISimpleAnimation
    {
        void SetPartner(RectTransform partnerRectTransform);
        
        void Setup(RectTransform rectTransform);
    }
}