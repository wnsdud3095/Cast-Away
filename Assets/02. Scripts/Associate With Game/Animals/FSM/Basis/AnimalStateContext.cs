public class AnimalStateContext
{
    private AnimalCtrl m_controller;
    private IState<AnimalCtrl> m_current_state;

    public AnimalStateContext(AnimalCtrl controller)
    {
        m_controller = controller;
    }

    public void Transition(IState<AnimalCtrl> state)
    {
        UnityEngine.Debug.Log(state);
        if(m_current_state == state)
        {
            return;
        }

        m_current_state?.ExecuteExit();
        m_current_state = state;
        m_current_state?.ExecuteEnter(m_controller);
    }
}