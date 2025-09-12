using System.Collections;
using UnityEngine;

public class AnimalAttackState : MonoBehaviour, IState<AnimalCtrl>
{
    private AnimalCtrl m_controller;

    private Coroutine m_rotate_coroutine;

    public void ExecuteEnter(AnimalCtrl sender)
    {
        if(m_controller == null)
        {
            m_controller = sender;
        }

        Initialize();
        m_rotate_coroutine = StartCoroutine(RotateToPlayer());
    }

    public void ExecuteExit()
    {
        if(m_rotate_coroutine != null)
        {
            StopCoroutine(m_rotate_coroutine);
            m_rotate_coroutine = null;
        }

        m_controller.Agent.updateRotation = true;
    }

    private void Initialize()
    {
        m_controller.Movement.IsWalk = false;
        m_controller.Movement.IsRun = false;

        m_controller.Animator.SetBool("Walk", false);
        m_controller.Animator.SetBool("Run", false);

        m_controller.Agent.updateRotation = false;

        m_controller.Animator.SetTrigger("Attack");
    }

    private IEnumerator RotateToPlayer()
    {
        while((m_controller as AggressiveAnimalCtrl).Attack.CanAttack)
        {
            var player_position = m_controller.Player.transform.position;
            
            var look_direction = player_position - transform.position;
            look_direction.y = 0f;
            
            var target_rotation = Quaternion.LookRotation(look_direction.normalized, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, target_rotation, Time.deltaTime * 5f);

            yield return null;         
        }
    }

    public void OnAttackAnimeEnter()
    {
        (m_controller as AggressiveAnimalCtrl).Attack.AttackHolder.Collider.enabled = true;
    }

    public void OnAttackAnimeExit()
    {
        (m_controller as AggressiveAnimalCtrl).Attack.AttackHolder.Collider.enabled = false;
    }

    public void OnAnimeExit()
    {
        if(!m_controller.ForceMode)
        {
            m_controller.ChangeState(AnimalState.TRACE);
        }
        else
        {
            m_controller.ChangeState(AnimalState.RETURNED);
        }
    }
}
