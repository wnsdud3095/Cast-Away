using UnityEngine;

public abstract class BaseBreakable : MonoBehaviour
{
    protected CameraShaker m_camera_shaker; 

    [Header("구조물의 체력")]
    [SerializeField] private float m_default_hp;

    private bool m_is_already_break = false;

    public void Inject(CameraShaker camera_shaker)
    {
        m_camera_shaker = camera_shaker;
    }

    public virtual void UpdateHP(float amount, Vector3 point)
    {
        if(m_is_already_break)
        {
            return;
        }

        m_default_hp += amount;

        InstantiateEffect(point);

        if(m_default_hp <= 0f)
        {
            m_is_already_break = true;

            m_default_hp = 0f;
            Break(point);
        }
    }

    protected abstract void InstantiateEffect(Vector3 point);
    protected abstract void Break(Vector3 point);
}