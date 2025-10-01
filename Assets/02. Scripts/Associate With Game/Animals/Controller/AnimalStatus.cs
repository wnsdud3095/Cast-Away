using UnityEngine;
using System;

[RequireComponent(typeof(AnimalCtrl))]
public class AnimalStatus : MonoBehaviour
{
    private AnimalCtrl m_controller;

    public float CurrentHP { get; private set; }
    public float MaxHP { get; private set; }
    public bool IsDead { get; private set; }

    public event Action<float, float> OnUpdatedHP;
    public event Action<AnimalCtrl> OnDisabledObject;

    private void Awake()
    {
        m_controller = GetComponent<AnimalCtrl>();
    }

    private void OnEnable()
    {
        IsDead = false;
    }

    private void OnDisable()
    {
        OnDisabledObject?.Invoke(m_controller);
    }

    public void Initialize(float max_hp)
    {
        MaxHP = max_hp;
        CurrentHP = MaxHP;
    }

    public void UpdateHP(float amount)
    {
        CurrentHP += amount;
        CurrentHP = Mathf.Clamp(CurrentHP, 0f, MaxHP);

        if(amount < 0f)
        {
            if(CurrentHP <= 0f)
            {
                m_controller.ChangeState(AnimalState.DEATH);
            }
            else
            {
                m_controller.ChangeState(AnimalState.HURT);
            }

            OnUpdatedHP?.Invoke(CurrentHP, MaxHP);
        }
    }

    public void Death()
    {
        if(IsDead)
        {
            return;
        }
        IsDead = true;

        m_controller.Collider.enabled = false;

        OnDisabledObject?.Invoke(m_controller);
    }
}
