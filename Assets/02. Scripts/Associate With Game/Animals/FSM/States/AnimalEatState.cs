using UnityEngine;

public class AnimalEatState : MonoBehaviour, IState<AnimalCtrl>
{
    private AnimalCtrl m_controller;

    public void ExecuteEnter(AnimalCtrl sender)
    {
        if(m_controller == null)
        {
            m_controller = sender;
        }

        Initialize();
    }

    public void ExecuteExit()
    {

    }

    private void Initialize()
    {
        m_controller.Movement.IsWalk = false;
        m_controller.Movement.IsRun = false;

        m_controller.Animator.SetBool("Walk", false);
        m_controller.Animator.SetBool("Run", false);

        m_controller.Animator.SetTrigger("Eat");
    }

    public void OnEatAnimationEnd()
    {
        if(!m_controller.ForceMode)
        {
            var states = new AnimalState[] { AnimalState.IDLE, AnimalState.WANDER };
            var random_state = states[Random.Range(0, states.Length)];
            
            m_controller.ChangeState(random_state);
        }
        else
        {
            m_controller.ChangeState(AnimalState.RETURNED);
        }
    }
}