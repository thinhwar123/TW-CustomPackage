namespace DesignPattern.StateMachine.MonoState
{
    public interface IState
    {
        void OnEnter();
        void OnUpdate();
        void OnExit();
    }
}