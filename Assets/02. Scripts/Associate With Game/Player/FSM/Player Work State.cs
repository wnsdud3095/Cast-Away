using UnityEngine;

public class PlayerWorkState : MonoBehaviour, IState<PlayerCtrl>
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

    public void ExecuteUpdate() { }

    public void ExecuteFixedUpdate() { }

    public void ExecuteExit()
    {
        m_controller.Interacting = false;
        m_controller.Animator.SetBool("Working", false);   
    }

    private void Initialize()
    {
        m_controller.Animator.SetBool("Walking", false);
        m_controller.Animator.SetBool("Running", false);
        m_controller.Animator.SetBool("Working", true);  
        m_controller.Animator.SetBool("Spearing", false);      
        m_controller.Animator.SetBool("Fishing", false);
        
        m_controller.Interacting = true;
    }
}
