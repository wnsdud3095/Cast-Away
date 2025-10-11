using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(PlayerCtrl))]
public class PlayerStatus : MonoBehaviour 
{
    private PlayerCtrl m_controller;
    private Coroutine m_penalty_coroutine;

    public float MaxValue { get; private set; } = 100f;

    public float HP { get; private set; }
    public float Thirst { get; private set; }
    public float Hunger { get; private set; }

    public Action<float, float> OnUpdatedHP;
    public Action<float, float> OnUpdatedThirst;
    public Action<float, float> OnUpdatedHunger;

    public bool Hungry => Hunger <= 30f;
    public bool Starving => Hunger <= 0f;

    public bool Thirsty => Thirst <= 30f;
    public bool Dehydrated => Thirst <= 0f;

    private void Awake()
    {
        m_controller = GetComponent<PlayerCtrl>();
    }

    private void OnDestroy()
    {
        OnUpdatedHP -= UpdateHPState;
        OnUpdatedHunger -= UpdateHungerState;
        OnUpdatedThirst -= UpdateThirstState;        
    }

    public void Initialize()
    {
        HP = MaxValue;
        Thirst = MaxValue;
        Hunger = MaxValue;

        OnUpdatedHP += UpdateHPState;
        OnUpdatedHunger += UpdateHungerState;
        OnUpdatedThirst += UpdateThirstState;

        OnUpdatedHP?.Invoke(HP, MaxValue);
        OnUpdatedThirst?.Invoke(Thirst, MaxValue);
        OnUpdatedHunger?.Invoke(Hunger, MaxValue);
    }

    // HP 변경
    public void ChangeHP(float amount)
    {
        HP = Mathf.Clamp(HP + amount, 0, MaxValue);
        OnUpdatedHP?.Invoke(HP, MaxValue);
    }

    // 갈증 변경
    public void ChangeThirst(float amount)
    {
        Thirst = Mathf.Clamp(Thirst + amount, 0, MaxValue);
        OnUpdatedThirst?.Invoke(Thirst, MaxValue);
    }

    // 허기 변경
    public void ChangeHunger(float amount)
    {
        Hunger = Mathf.Clamp(Hunger + amount, 0, MaxValue);
        OnUpdatedHunger?.Invoke(Hunger, MaxValue);
    }

    private IEnumerator Co_Penalty()
    {
        var decrease_interval = 1f;
        var decrease_amount = 1f;

        while(Starving || Dehydrated)
        {
            yield return new WaitForSeconds(decrease_interval);
            ChangeHP(-decrease_amount);
        }

        m_penalty_coroutine = null;
    }

    private void UpdateHPState(float current, float max)
    {
        if((current / max) <= 0.5f)
        {
            m_controller.Animator.SetBool("Injured", true);
        }
        else
        {
            m_controller.Animator.SetBool("Injured", false);
        }
    }

    private void UpdateHungerState(float current, float max)
    {
        if(Starving)
        {
            m_penalty_coroutine ??= StartCoroutine(Co_Penalty());
        }
    }

    private void UpdateThirstState(float current, float max)
    {
        if(Dehydrated)
        {
            m_penalty_coroutine ??= StartCoroutine(Co_Penalty());
        }
    }
}