using UnityEngine;
using System;

public class PlayerStatus : MonoBehaviour 
{
    public float MaxValue { get; private set; } = 100f;

    public float HP { get; private set; }
    public float Thirst { get; private set; }
    public float Hunger { get; private set; }

    public Action<float, float> OnUpdatedHP;
    public Action<float, float> OnUpdatedThirst;
    public Action<float, float> OnUpdatedHunger;

    // 초기화
    public void Initialize()
    {
        HP = MaxValue;
        Thirst = MaxValue;
        Hunger = MaxValue;

        // UI 업데이트 호출
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
}