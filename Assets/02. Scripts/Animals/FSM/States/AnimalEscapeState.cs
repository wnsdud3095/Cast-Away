using System.Collections;
using UnityEngine;

public class AnimalEscapeState : MonoBehaviour, IState<AnimalCtrl>
{
    private AnimalCtrl m_controller;

    private Coroutine m_move_coroutine;

    public void ExecuteEnter(AnimalCtrl sender)
    {
        if(m_controller == null)
        {
            m_controller = sender;
        }

        Initialize();
        Escape();
    }

    public void ExecuteExit()
    {
        if(m_move_coroutine != null)
        {
            StopCoroutine(m_move_coroutine);
            m_move_coroutine = null;
        }
    }

    private void Initialize()
    {
        m_controller.Movement.IsWalk = false;
        m_controller.Movement.IsRun = true;

        m_controller.Animator.SetBool("Walk", false);
        m_controller.Animator.SetBool("Run", true);
    }

    private void Escape()
    {
        m_move_coroutine = StartCoroutine(EscapeToDirection(Vector3.zero, m_controller.Movement.MoveTime));
    }

    private IEnumerator EscapeToDirection(Vector3 destination, float move_time)
    {
        float elasped_time = 0f;

        while(elasped_time < move_time)
        {
            // TODO: 도망에 대한 처리

            elasped_time += Time.deltaTime;
            yield return null;
        }

        SelectAction();
        m_move_coroutine = null;
    }

    private void SelectAction()
    {
        var states = new AnimalState[] { AnimalState.IDLE, 
                                         AnimalState.WANDER, 
                                         AnimalState.WANDER, 
                                         AnimalState.WANDER, 
                                         AnimalState.WANDER };
        var random_state = states[Random.Range(0, states.Length)];
        
        m_controller.ChangeState(random_state);
    }
}
