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
        if(GameManager.Instance.GameType == GameEventType.INPLAY)
        {
            if(Input.GetKeyDown(KeyCode.Mouse0))
            {
                if(m_controller.State.Hungry || m_controller.State.Starving)
                {
                    m_controller.InstantiateNotice("허기로 인하여 도구를 사용할 수 없습니다.");
                    return;
                }

                ItemSwapper.OnLeftClickDown?.Invoke();
            }
            
            if(Input.GetKey(KeyCode.Mouse0))
            {
                if(m_controller.State.Hungry || m_controller.State.Starving)
                {
                    return;
                }
                
                ItemSwapper.OnLeftClickHold?.Invoke();
            }
            else if(Input.GetKeyDown(KeyCode.Mouse1))
            {
                ItemSwapper.OnRightClickDown?.Invoke();
            }
        }

        if(m_controller.Direction.magnitude > 0f)
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
