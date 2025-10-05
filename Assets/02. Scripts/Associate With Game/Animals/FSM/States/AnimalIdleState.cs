using System.Collections;
using UnityEngine;

public class AnimalIdleState : MonoBehaviour, IState<AnimalCtrl>
{
    private AnimalCtrl m_controller;
    private float m_idle_time;
    private Coroutine m_wait_coroutine;

    public void ExecuteEnter(AnimalCtrl sender)
    {
        if(m_controller == null)
        {
            m_controller = sender;
        }

        Initialize();
        m_wait_coroutine = StartCoroutine(WaitForAction(m_idle_time));
    }

    public void ExecuteUpdate() { }

    public void ExecuteFixedUpdate() { }

    public void ExecuteExit()
    {
        if(m_wait_coroutine != null)
        {
            StopCoroutine(m_wait_coroutine);
            m_wait_coroutine = null;
        }
    }

    private void Initialize()
    {
        m_idle_time = m_controller.Movement.IdleTime;

        m_controller.Movement.IsWalk = false;
        m_controller.Movement.IsRun = false;

        m_controller.Animator.SetBool("Walk", false);
        m_controller.Animator.SetBool("Run", false);
    }

    private IEnumerator WaitForAction(float target_time)
    {
        yield return new WaitForSeconds(target_time);

        SelectAction();
        m_wait_coroutine = null;
    }

    private void SelectAction()
    {
        var states = new AnimalState[] { AnimalState.EAT, 
                                         AnimalState.WANDER, 
                                         AnimalState.WANDER, 
                                         AnimalState.WANDER, 
                                         AnimalState.WANDER };
        var random_state = states[Random.Range(0, states.Length)];
        
        m_controller.ChangeState(random_state);
    }
}