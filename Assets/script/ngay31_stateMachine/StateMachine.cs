public class StateMachine
{
    public IEnemyState CurrentState { get; private set; }

    // Khởi tạo trạng thái ban đầu của AI
    public void Initialize(IEnemyState startingState)
    {
        CurrentState = startingState;
        CurrentState.Enter();
    }

    // Chuyển đổi trạng thái của AI
    public void ChangeState(IEnemyState newState)
    {
        if (newState == CurrentState) return; // nếu trạng thái mới giống với trạng thái hiện tại thì không làm gì cả

        CurrentState?.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }

    public void Tick() => CurrentState?.Tick();
}