using System;

namespace TW.UGUI.Core.Controls
{
    public interface ISimpleControlContainerCallbackReceiver
    {
        void BeforeShow(Control control, Memory<object> args);

        void AfterShow(Control control, Memory<object> args);

        void BeforeHide(Control control, Memory<object> args);

        void AfterHide(Control control, Memory<object> args);
    }
}