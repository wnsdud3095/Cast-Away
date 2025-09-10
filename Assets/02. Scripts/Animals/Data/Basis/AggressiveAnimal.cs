using UnityEngine;

[CreateAssetMenu(fileName = "New Animal", menuName = "SO/Animals/Create Aggressive Animal")]
public class AggressiveAnimal : Animal
{
    [Space(30f)]
    [Header("공격 특징")]
    [Header("플레이어 공격 범위")]
    [SerializeField] private float m_atk_range;
    public float ATKRange => m_atk_range;

    [Header("공격력")]
    [SerializeField] private float m_atk;
    public float ATK => m_atk;

    [Header("공격 딜레이")]
    [SerializeField] private float m_atk_delay;
    public float ATKDelay => m_atk_delay;
}
