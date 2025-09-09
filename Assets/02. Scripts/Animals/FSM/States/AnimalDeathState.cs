using UnityEngine;

public class AnimalDeathState : MonoBehaviour, IState<AnimalCtrl>
{
    private AnimalCtrl m_controller;

    public void ExecuteEnter(AnimalCtrl sender)
    {
        if(m_controller == null)
        {
            m_controller = sender;
        }

        Initialize();
        m_controller.Status.Death();
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

        m_controller.Animator.SetTrigger("Death");        
    }

    public void OnDeathAnimationEnd()
    {   
        // TODO: 죽은 경우에 대한 처리
    }
}
