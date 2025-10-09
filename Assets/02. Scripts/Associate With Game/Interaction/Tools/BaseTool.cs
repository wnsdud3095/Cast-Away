using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class BaseTool : MonoBehaviour
{
    [Header("플레이어 컨트롤러")]
    [SerializeField] protected PlayerCtrl m_player_ctrl;   

    [Space(30f)][Header("도구 관련 정보")] 
    [Header("일반 데미지")]
    [SerializeField] protected float m_default_damage;

    protected Collider m_collider;
    protected bool m_is_working = false;
    protected bool m_is_attacking = false;

    private void Awake()
    {
        m_collider = GetComponent<Collider>();
    }

    private void OnEnable()
    {
        ItemSwapper.OnLeftClickDown += OnLeftUse;
        ItemSwapper.OnLeftClickHold += OnLeftUse;
        ItemSwapper.OnRightClickDown += OnRightUse;
    }

    private void OnDisable()
    {
        m_collider.enabled = false;

        ItemSwapper.OnLeftClickDown -= OnLeftUse;
        ItemSwapper.OnLeftClickHold -= OnLeftUse;
        ItemSwapper.OnRightClickDown -= OnRightUse; 

        m_player_ctrl.Animator.Play("Drawing");       
    }

    private void OnDestroy()
    {
        ItemSwapper.OnLeftClickDown -= OnLeftUse;
        ItemSwapper.OnLeftClickHold -= OnLeftUse;
        ItemSwapper.OnRightClickDown -= OnRightUse;        
    }

    protected virtual void OnTriggerEnter(Collider collider)
    {
        if(collider.CompareTag("Breakable"))
        {
            var hit_point = collider.ClosestPoint(transform.position);
            var interactable = collider.GetComponent<BaseBreakable>();
            OnInteract(interactable, hit_point);
        }

        if(collider.CompareTag("Animal"))
        {
            Debug.Log(collider.name);
            var animal = collider.GetComponent<AnimalCtrl>();
            OnInteract(animal);
        }
    }

    protected abstract void OnLeftUse();
    protected abstract void OnRightUse();
    protected abstract void OnInteract(BaseBreakable target, Vector3 point);
    protected abstract void OnInteract(AnimalCtrl animal);

    public virtual void TriggerEnter()
    {
        m_is_working = true;
    }

    public virtual void EnableHit()
    {
        m_collider.enabled = true;
    }

    public virtual void DisableHit()
    {
        m_collider.enabled = false;
    }

    public virtual void TriggerExit()
    {
        m_is_working = false;
    }
}