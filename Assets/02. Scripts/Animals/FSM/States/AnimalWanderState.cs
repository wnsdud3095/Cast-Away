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
    }

    private void Initialize()
    {
        m_controller.Movement.IsWalk = true;
        m_controller.Movement.IsRun = false;

        m_controller.Animator.SetBool("Walk", true);
        m_controller.Animator.SetBool("Run", false);
    }

    private void Wander()
    {
        var euler_angles = transform.rotation.eulerAngles;
        var direction = new Vector3(euler_angles.x, Random.Range(0f, 360f), euler_angles.z);

        m_move_coroutine = StartCoroutine(MoveToDirection(direction, m_controller.Movement.MoveTime));
    }

    private IEnumerator MoveToDirection(Vector3 direction, float move_time)
    {
        float elasped_time = 0f;

        while(elasped_time < move_time)
        {
            m_controller.Movement.Move();
            m_controller.Movement.Rotation(direction);

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
