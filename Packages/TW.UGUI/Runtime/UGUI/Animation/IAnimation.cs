namespace TW.UGUI.Animation
{
    public interface IAnimation
    {
        float Delay { get; }
        float Duration { get; }
        void Setup();
        void SetTime(float time);
    }
}