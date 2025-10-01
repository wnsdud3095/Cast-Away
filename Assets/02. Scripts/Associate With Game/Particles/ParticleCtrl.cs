using UnityEngine;

public class ParticleCtrl : MonoBehaviour
{
    [Header("오브젝트 타입")]
    [SerializeField] private ObjectType m_object_type;

    private void OnParticleSystemStopped()
    {
        ReturnToPool();
    }

    private void ReturnToPool()
    {
        ObjectManager.Instance.ReturnObject(gameObject, m_object_type);
    }
}
