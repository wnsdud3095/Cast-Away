using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AnimalCtrl))]
public class AnimalAttack : MonoBehaviour
{
    private AnimalCtrl m_controller;

    [Header("공격 홀더")]
    [SerializeField] private AnimalAttackHolder m_atk_holder;

    private float m_atk;
    private float m_atk_range;

    public float ATK => m_atk;
    public bool CanTrace { get; private set; }
    public bool CanAttack => SqrMagnitude <= m_atk_range * m_atk_range;

    public AnimalAttackHolder AttackHolder => m_atk_holder;

    public float SqrMagnitude => Vector3.SqrMagnitude(m_controller.Player.transform.position - transform.position);
    public float Magnitude => Vector3.Magnitude(m_controller.Player.transform.position - transform.position);

    private void Awake()
    {
        m_controller = GetComponent<AnimalCtrl>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.CompareTag("Player"))
        {
            CanTrace = true;
            m_controller.ChangeState(AnimalState.TRACE);
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if(collision.CompareTag("Player"))
        {
            CanTrace = false;
            m_controller.ChangeState(AnimalState.IDLE);
        }
    }

    public void Initialize(float atk,
                           float atk_range)
    {
        m_atk = atk;
        m_atk_range = atk_range;
    }
}
