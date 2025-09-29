using UnityEngine;

[RequireComponent(typeof(MeshCollider))]
public abstract class BaseTool : MonoBehaviour
{
    [Header("플레이어 컨트롤러")]
    [SerializeField] protected PlayerCtrl m_player_ctrl;   

    [Space(30f)][Header("도구 관련 정보")] 
    [Header("일반 데미지")]
    [SerializeField] protected float m_default_damage;

    protected MeshCollider m_collider;
    protected bool m_is_working = false;

    private void Awake()
    {
        m_collider = GetComponent<MeshCollider>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if(collider.CompareTag("Breakable"))
        {
            var interactable = collider.GetComponent<IInteratable>();
            OnInteract(interactable);
        }

        if(collider.CompareTag("Animal"))
        {
            var animal = collider.GetComponent<AnimalCtrl>();
            OnInteract(animal);
        }
    }

    protected abstract void OnLeftUse();
    protected abstract void OnRightUse();
    protected abstract void OnInteract(IInteratable target);
    protected abstract void OnInteract(AnimalCtrl animal);
}