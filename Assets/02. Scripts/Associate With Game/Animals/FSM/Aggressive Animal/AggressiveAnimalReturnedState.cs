using UnityEngine;

public class AggressiveAnimalReturnedState : MonoBehaviour, IState<AnimalCtrl>
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
        m_controller.ForceMode = true;

        m_controller.Movement.IsWalk = true;
        m_controller.Movement.IsRun = false;

        m_controller.Animator.Play("Bear_Idle");

        m_controller.Animator.SetBool("Walk", true);
        m_controller.Animator.SetBool("Run", false);

        m_controller.Agent.isStopped = false;

        var target_pos = new Vector3(45.4f, 2.5f, 0.039f);
        m_controller.Movement.Move(target_pos, true);      
    }
}
