public class PlayerStateContext
{
    private readonly PlayerCtrl m_controller;
    private IState<PlayerCtrl> m_current_state;

    public PlayerStateContext(PlayerCtrl controller)
    {
        m_controller = controller;
    }

    public void Transition(IState<PlayerCtrl> state)
    {
        if(m_current_state == state)
        {
            return;
        }
        
        m_current_state?.ExecuteExit();
        m_current_state = state;
        m_current_state?.ExecuteEnter(m_controller);
    }
}