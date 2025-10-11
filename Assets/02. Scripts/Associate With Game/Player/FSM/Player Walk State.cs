using UnityEngine;

public class PlayerWalkState : MonoBehaviour, IState<PlayerCtrl>
{
    private PlayerCtrl m_controller;

    private readonly float m_walk_speed = 1.5f;

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
            if(m_controller.Movement.IsDashActive)
            {
                m_controller.ChangeState(PlayerState.RUN);
            }
        }
        else
        {
            m_controller.ChangeState(PlayerState.IDLE);
        }
    }

    public void ExecuteFixedUpdate()
    {
        m_controller.Movement.OnMove(m_walk_speed);
    }

    public void ExecuteExit()
    {
        m_controller.Animator.SetBool("Walking", false);
    }

    private void Initialize()
    {
        m_controller.Animator.SetBool("Walking", true);
        m_controller.Animator.SetBool("Running", false);
        m_controller.Animator.SetBool("Working", false);
        m_controller.Animator.SetBool("Spearing", false);
        m_controller.Animator.SetBool("Fishing", false);
    }
}
