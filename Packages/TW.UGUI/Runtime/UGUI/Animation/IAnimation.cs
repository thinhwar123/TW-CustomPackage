namespace TW.UGUI.Animation
{
    public interface IAnimation
    {
        float Duration { get; }

        void SetTime(float time);
    }
}