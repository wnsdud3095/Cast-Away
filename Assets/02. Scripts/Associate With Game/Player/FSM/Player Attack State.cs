using UnityEngine;

public class PlayerAttackState : MonoBehaviour, IState<PlayerCtrl>
{
    private PlayerCtrl m_controller;

    public void ExecuteEnter(PlayerCtrl sender)
    {
        if(m_controller == null)
        {
            m_controller = sender;
        }

        if(m_controller.State.Hungry || m_controller.State.Starving)
        {
            m_controller.InstantiateNotice("허기로 인하여 창을 사용할 수 없습니다.");
            m_controller.ChangeState(PlayerState.IDLE);
            return;
        }

        Initialize();
    }

    public void ExecuteUpdate() { }

    public void ExecuteFixedUpdate() { }

    public void ExecuteExit()
    {
        m_controller.Interacting = false;
        m_controller.Animator.SetBool("Spearing", false);
    }

    private void Initialize()
    {
        m_controller.Animator.SetBool("Walking", false);
        m_controller.Animator.SetBool("Running", false);
        m_controller.Animator.SetBool("Working", false);
        m_controller.Animator.SetBool("Spearing", true);
        m_controller.Animator.SetBool("Fishing", false);
        
        m_controller.Interacting = true;
    }
}
