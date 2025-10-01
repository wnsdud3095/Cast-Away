using UnityEngine;

public class AnimalDeathState : MonoBehaviour, IState<AnimalCtrl>
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

    public void ExecuteExit() {}

    private void Initialize()
    {
        m_controller.Agent.velocity = Vector3.zero;
        m_controller.Agent.ResetPath();

        m_controller.Movement.IsWalk = false;
        m_controller.Movement.IsRun = false;

        m_controller.Animator.SetBool("Walk", false);
        m_controller.Animator.SetBool("Run", false);

        m_controller.Animator.SetTrigger("Death");    
        m_controller.Status.Death();    
    }

    public void OnDeathAnimationEnd()
    {   
        InstantiateEffect();
        ObjectManager.Instance.ReturnObject(m_controller.gameObject, GetObjectType(m_controller.SO.Code));
    }

    private void InstantiateEffect()
    {
        var smoke_obj = ObjectManager.Instance.GetObject(ObjectType.DEATH_SMOKE);
        var model_obj = GetComponentInChildren<InclineInterpolation>().gameObject;

        smoke_obj.transform.position = model_obj.transform.position + Vector3.up;
    }

    private ObjectType GetObjectType(AnimalCode animal_code) => (ObjectType)((int)animal_code + 201);
}
