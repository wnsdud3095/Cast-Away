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

    // �ʱ�ȭ
    public void Initialize()
    {
        HP = MaxValue;
        Thirst = MaxValue;
        Hunger = MaxValue;

        // UI ������Ʈ ȣ��
        OnUpdatedHP?.Invoke(HP, MaxValue);
        OnUpdatedThirst?.Invoke(Thirst, MaxValue);
        OnUpdatedHunger?.Invoke(Hunger, MaxValue);
    }

    // HP ����
    public void ChangeHP(float amount)
    {
        HP = Mathf.Clamp(HP + amount, 0, MaxValue);
        OnUpdatedHP?.Invoke(HP, MaxValue);
    }

    // ���� ����
    public void ChangeThirst(float amount)
    {
        Thirst = Mathf.Clamp(Thirst + amount, 0, MaxValue);
        OnUpdatedThirst?.Invoke(Thirst, MaxValue);
    }

    // ��� ����
    public void ChangeHunger(float amount)
    {
        Hunger = Mathf.Clamp(Hunger + amount, 0, MaxValue);
        OnUpdatedHunger?.Invoke(Hunger, MaxValue);
    }
}