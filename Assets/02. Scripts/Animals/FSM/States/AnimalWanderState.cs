using System.Collections;
using UnityEngine;

public class AnimalWanderState : MonoBehaviour, IState<AnimalCtrl>
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
        Wander();
    }

    public void ExecuteExit()
    {
        if(m_move_coroutine != null)
        {
            StopCoroutine(m_move_coroutine);
            m_move_coroutine = null;
        }

        m_controller.Agent.ResetPath();
        m_controller.Agent.isStopped = true;
        m_controller.Agent.velocity = Vector3.zero;
    }

    private void Initialize()
    {
        m_controller.Agent.isStopped = false;
        
        m_controller.Movement.IsWalk = true;
        m_controller.Movement.IsRun = false;

        m_controller.Animator.SetBool("Walk", true);
        m_controller.Animator.SetBool("Run", false);
    }

    private void Wander()
    {
        // 원형 범위 내에서 랜덤한 방향 생성
        var random_angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        var random_distance = Random.Range(2f, 8f);
        
        var destination = new Vector3(
            Mathf.Cos(random_angle) * random_distance,
            0f,
            Mathf.Sin(random_angle) * random_distance
        );

        m_move_coroutine = StartCoroutine(MoveToDirection(destination, m_controller.Movement.MoveTime));
    }

    private IEnumerator MoveToDirection(Vector3 destination, float move_time)
    {
        float elasped_time = 0f;

        while(elasped_time < move_time)
        {
            m_controller.Movement.Move(destination);

            elasped_time += Time.deltaTime;
            yield return null;
        }

        SelectAction();
        m_move_coroutine = null;
    }

    private void SelectAction()
    {
        var states = new AnimalState[] { AnimalState.IDLE, 
                                         AnimalState.IDLE, 
                                         AnimalState.IDLE, 
                                         AnimalState.IDLE, 
                                         AnimalState.EAT };
        var random_state = states[Random.Range(0, states.Length)];
        
        m_controller.ChangeState(random_state);
    }
}
