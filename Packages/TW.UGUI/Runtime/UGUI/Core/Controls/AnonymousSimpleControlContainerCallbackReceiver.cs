using System;

namespace TW.UGUI.Core.Controls
{
    public class AnonymousSimpleControlContainerCallbackReceiver : ISimpleControlContainerCallbackReceiver
    {
        public event Action<Control, Memory<object>> OnAfterHide;
        public event Action<Control, Memory<object>> OnAfterShow;
        public event Action<Control, Memory<object>> OnBeforeHide;
        public event Action<Control, Memory<object>> OnBeforeShow;

        public AnonymousSimpleControlContainerCallbackReceiver(
              Action<Control, Memory<object>> onBeforeShow = null
            , Action<Control, Memory<object>> onAfterShow = null
            , Action<Control, Memory<object>> onBeforeHide = null
            , Action<Control, Memory<object>> onAfterHide = null
        )
        {
            OnBeforeShow = onBeforeShow;
            OnAfterShow = onAfterShow;
            OnBeforeHide = onBeforeHide;
            OnAfterHide = onAfterHide;
        }

        void ISimpleControlContainerCallbackReceiver.BeforeShow(Control control, Memory<object> args)
        {
            OnBeforeShow?.Invoke(control, args);
        }

        void ISimpleControlContainerCallbackReceiver.AfterShow(Control control, Memory<object> args)
        {
            OnAfterShow?.Invoke(control, args);
        }

        void ISimpleControlContainerCallbackReceiver.BeforeHide(Control control, Memory<object> args)
        {
            OnBeforeHide?.Invoke(control, args);
        }

        void ISimpleControlContainerCallbackReceiver.AfterHide(Control control, Memory<object> args)
        {
            OnAfterHide?.Invoke(control, args);
        }
    }
}