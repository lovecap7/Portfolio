namespace mySystem.AI
{
    public interface IState
    {
        public void OnEnter();
        public void OnUpdate();
        public void OnExit();
    }

    public interface IStateMachine
    {
        public void OnEnable();
        public void OnUpdate();
        public void OnDisable();
    }
}