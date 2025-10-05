using System.Collections;
using UnityEngine;

public class AnimalTraceState : MonoBehaviour, IState<AnimalCtrl>
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
        Trace();
    }

    public void ExecuteUpdate() { }

    public void ExecuteFixedUpdate() { }

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
        m_controller.Movement.IsWalk = false;
        m_controller.Movement.IsRun = true;

        m_controller.Animator.SetBool("Walk", false);
        m_controller.Animator.SetBool("Run", true);

        m_controller.Agent.isStopped = false;
        m_controller.Agent.ResetPath();
    }

    private void Trace()
    {
        m_move_coroutine = StartCoroutine(TracePlayerLoop());
    }

    private IEnumerator TracePlayerLoop()
    {
        while((m_controller as AggressiveAnimalCtrl).Attack.CanTrace)
        {
            if((m_controller as AggressiveAnimalCtrl).Attack.CanAttack)
            {
                m_controller.ChangeState(AnimalState.ATTACK);
                yield break;
            }

            var player_position = m_controller.Player.transform.position;
            
            var direction = player_position - transform.position;
            direction.y = 0f;

            m_controller.Movement.Move(direction.normalized);

            yield return null;
        }

        m_move_coroutine = null;
    }
}
