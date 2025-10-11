using UnityEngine;

public class PlayerRunState : MonoBehaviour, IState<PlayerCtrl>
{
    private PlayerCtrl m_controller;

    private readonly float m_run_speed = 3f;

    public void ExecuteEnter(PlayerCtrl sender)
    {
        if(m_controller == null)
        {
            m_controller = sender;
        }

        Initialize();
    }

    public void ExecuteUpdate()
    {
        if(m_controller.Direction.magnitude > 0f)
        {
            if(!m_controller.Movement.IsDashActive)
            {
                m_controller.ChangeState(PlayerState.WALK);
            }
        }
        else
        {
            m_controller.ChangeState(PlayerState.IDLE);
        }
    }

    public void ExecuteFixedUpdate()
    {
        m_controller.Movement.OnMove(m_run_speed);
    }

    public void ExecuteExit()
    {
        m_controller.Animator.SetBool("Walking", false);
        m_controller.Animator.SetBool("Running", false);
    }

    private void Initialize()
    {
        m_controller.Animator.SetBool("Walking", true);
        m_controller.Animator.SetBool("Running", true);
        m_controller.Animator.SetBool("Working", false);
        m_controller.Animator.SetBool("Spearing", false);
        m_controller.Animator.SetBool("Fishing", false);
    }
}
