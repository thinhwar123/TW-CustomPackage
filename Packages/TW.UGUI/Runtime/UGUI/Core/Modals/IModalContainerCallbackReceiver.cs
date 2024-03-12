using System;

namespace TW.UGUI.Core.Modals
{
    public interface IModalContainerCallbackReceiver
    {
        void BeforePush(Modal enterModal, Modal exitModal, Memory<object> args);

        void AfterPush(Modal enterModal, Modal exitModal, Memory<object> args);

        void BeforePop(Modal enterModal, Modal exitModal, Memory<object> args);

        void AfterPop(Modal enterModal, Modal exitModal, Memory<object> args);
    }
}