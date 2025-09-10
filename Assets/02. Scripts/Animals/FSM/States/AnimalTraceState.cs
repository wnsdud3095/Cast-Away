using UnityEngine;

public class AnimalTraceState : MonoBehaviour, IState<AnimalCtrl>
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
        m_controller.Movement.IsRun = true;

        m_controller.Animator.SetBool("Walk", false);
        m_controller.Animator.SetBool("Run", true);
    }
}
