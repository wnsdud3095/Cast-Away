using UnityEngine;

[RequireComponent(typeof(AnimalCtrl))]
public class AnimalStatus : MonoBehaviour
{
    private AnimalCtrl m_controller;

    private float m_max_hp;
    private float m_current_hp;

    private bool m_is_dead;

    public float CurrentHP => m_current_hp;
    public float MaxHP => m_max_hp;
    public bool IsDead => m_is_dead;

    private void Awake()
    {
        m_controller = GetComponent<AnimalCtrl>();
    }

    private void OnEnable()
    {
        m_is_dead = false;
    }

    public void Initialize(float max_hp)
    {
        m_max_hp = max_hp;
        m_current_hp = m_max_hp;
    }

    public void UpdateHP(float amount)
    {
        m_current_hp += amount;

        if(amount < 0f)
        {
            if(m_current_hp <= 0f)
            {
                m_controller.ChangeState(AnimalState.DEATH);
            }
            else
            {
                m_controller.ChangeState(AnimalState.HURT);
            }
        }
    }

    public void Death()
    {
        if(m_is_dead)
        {
            return;
        }
        m_is_dead = true;

        m_controller.Collider.enabled = false;
    }
}
