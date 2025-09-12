using UnityEngine;

public class AnimalHurtState : MonoBehaviour, IState<AnimalCtrl>
{
    protected AnimalCtrl m_controller;

    public void ExecuteEnter(AnimalCtrl sender)
    {
        if(m_controller == null)
        {
            m_controller = sender;
        }

        Initialize();
    }

    public void ExecuteExit() {}

    private void Initialize()
    {
        m_controller.Movement.IsWalk = false;
        m_controller.Movement.IsRun = false;

        m_controller.Animator.SetBool("Walk", false);
        m_controller.Animator.SetBool("Run", false);

        m_controller.Animator.SetTrigger("Hurt");        
    }

    public virtual void OnHurtAnimationEnd()
    {   
        m_controller.ChangeState(AnimalState.ESCAPE);
    }
}
