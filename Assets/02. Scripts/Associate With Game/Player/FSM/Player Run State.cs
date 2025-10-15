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
        if(m_controller.State.Thirsty)
        {
            m_controller.InstantiateNotice("탈수로 인하여 달릴 수 없습니다.");
            m_controller.ChangeState(PlayerState.WALK);
        }

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

    public void PlaySFX()
    {
        var random_index = Random.Range(1, 11);
        SoundManager.Instance.PlaySFX($"Grass Run {random_index}", true, transform.position + Vector3.down);
    }
}
