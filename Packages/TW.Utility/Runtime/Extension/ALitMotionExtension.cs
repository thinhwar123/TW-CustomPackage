#if LITMOTION_SUPPORT
using LitMotion;
#endif

namespace TW.Utility.Extension
{
    public static class ALitMotionExtension
    {
#if LITMOTION_SUPPORT
        public static void TryCancel(this MotionHandle handle)
        {
            if (handle.IsActive())
            {
                handle.Cancel();
            }
        }
#endif
    }
    
}
