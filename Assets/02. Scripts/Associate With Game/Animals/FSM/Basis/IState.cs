public interface IState<T>
{
    void ExecuteEnter(T sender);
    void ExecuteUpdate();
    void ExecuteFixedUpdate();
    void ExecuteExit();
}