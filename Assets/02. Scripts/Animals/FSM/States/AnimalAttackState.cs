using UnityEngine;

public class AnimalAttackState : MonoBehaviour, IState<AnimalCtrl>
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

        m_controller.Animator.SetTrigger("Attack");
    }

    public void OnAttackAnimeEnter()
    {
        (m_controller as AggressiveAnimalCtrl).Attack.ATKCollider.enabled = true;
    }

    public void OnAttackAnimeExit()
    {
        (m_controller as AggressiveAnimalCtrl).Attack.ATKCollider.enabled = false;
    }

    public void OnAnimeExit()
    {
        m_controller.ChangeState(AnimalState.TRACE);
    }
}
