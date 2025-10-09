using UnityEngine;

public class PlayerIdleState : MonoBehaviour, IState<PlayerCtrl>
{
    private PlayerCtrl m_controller;

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
        if(GameManager.Instance.GameType != GameEventType.INPLAY)
        {
            return;
        }

        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            ItemSwapper.OnLeftClickDown?.Invoke();
        }
        else if(Input.GetKey(KeyCode.Mouse0))
        {
            ItemSwapper.OnLeftClickHold?.Invoke();
        }
        else if(Input.GetKeyDown(KeyCode.Mouse1))
        {
            ItemSwapper.OnRightClickDown?.Invoke();
        }
        else if(m_controller.Direction.magnitude > 0f)
        {
            if(m_controller.Movement.IsDashActive)
            {
                m_controller.ChangeState(PlayerState.RUN);
            }
            else
            {
                m_controller.ChangeState(PlayerState.WALK);
            }
        }
    }

    public void ExecuteFixedUpdate() { }

    public void ExecuteExit() { }

    private void Initialize()
    {
        m_controller.Animator.SetBool("Walking", false);
        m_controller.Animator.SetBool("Running", false);
        m_controller.Animator.SetBool("Working", false);
        m_controller.Animator.SetBool("Spearing", false);
        m_controller.Animator.SetBool("Fishing", false);
    }
}
