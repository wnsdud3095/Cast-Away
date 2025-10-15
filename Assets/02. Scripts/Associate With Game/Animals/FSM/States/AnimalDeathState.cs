using UnityEngine;

public class AnimalDeathState : MonoBehaviour, IState<AnimalCtrl>
{
    private AnimalCtrl m_controller;

    private readonly int m_min_meat_count = 2;
    private readonly int m_max_meat_count = 4;

    private readonly int m_min_wool_count = 1;
    private readonly int m_max_wool_count = 3;

    public void ExecuteEnter(AnimalCtrl sender)
    {
        if(m_controller == null)
        {
            m_controller = sender;
        }

        Initialize();
        m_controller.CameraShaker.Shaking(0.05f, 0.4f);
        SoundManager.Instance.PlaySFX("Animal Interaction", true, transform.position);
    }

    public void ExecuteUpdate() { }

    public void ExecuteFixedUpdate() { }

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

    public void ShakeCamera()
    {
        m_controller.CameraShaker.Shaking(0.05f, 0.5f);
    }

    public void PlaySFX()
    {
        SoundManager.Instance.PlaySFX("Animal Death", true, transform.position);
    }

    public void OnDeathAnimationEnd()
    {   
        InstantiateEffect();
        InstantiateRawMeat();
        InstantiateWool();
        ObjectManager.Instance.ReturnObject(m_controller.gameObject, GetObjectType(m_controller.SO.Code));
    }

    private void InstantiateEffect()
    {
        var smoke_obj = ObjectManager.Instance.GetObject(ObjectType.DEATH_SMOKE);
        var model_obj = GetComponentInChildren<InclineInterpolation>().gameObject;

        smoke_obj.transform.position = model_obj.transform.position + Vector3.up;
    }

    private void InstantiateRawMeat()
    {
        var random_count = Random.Range(m_min_meat_count, m_max_meat_count + 1);

        while(random_count-- > 0)
        {
            var offset = new Vector3(Random.Range(-0.2f, 0.2f), 1f, Random.Range(-0.2f, 0.2f));

            var raw_meat_obj = ObjectManager.Instance.GetObject(ObjectType.RAW_MEAT);
            raw_meat_obj.transform.position = transform.position + offset;

            var raw_meat_rb = raw_meat_obj.GetComponent<Rigidbody>();
            raw_meat_rb.AddForce(Vector3.up * 1.25f, ForceMode.Impulse);
        }
    }

    private void InstantiateWool()
    {
        if(AnimalCode.WHITE_SHEEP <= m_controller.SO.Code && m_controller.SO.Code <= AnimalCode.DARK_SHEEP)
        {
            var random_count = Random.Range(m_min_wool_count, m_max_wool_count + 1);

            while(random_count-- > 0)
            {
                var offset = new Vector3(Random.Range(-0.2f, 0.2f), 1f, Random.Range(-0.2f, 0.2f));

                var raw_meat_obj = ObjectManager.Instance.GetObject(ObjectType.WOOL);
                raw_meat_obj.transform.position = transform.position + offset;

                var raw_meat_rb = raw_meat_obj.GetComponent<Rigidbody>();
                raw_meat_rb.AddForce(Vector3.up * 1.25f, ForceMode.Impulse);
            }
        }
    }

    private ObjectType GetObjectType(AnimalCode animal_code) => (ObjectType)((int)animal_code + 201);
}
