using UnityEngine;

[CreateAssetMenu(fileName = "New Animal", menuName = "SO/Animals/Create Neutrality Animal")]
public class Animal : ScriptableObject
{
    [Header("동물의 이름")]
    [SerializeField] private string m_name;
    public string Name => m_name;

    [Header("동물의 코드")]
    [SerializeField] private AnimalCode m_code;
    public AnimalCode Code => m_code;

    [Space(30f)]
    [Header("동물 특징")]
    [Header("성격")]
    [Range(1f, 4f)][SerializeField] private float m_idle_time;
    public float IdleTime => m_idle_time; 

    [Header("동물 체력")]
    [SerializeField] private float m_hp;
    public float HP => m_hp;

    [Header("걷기 속력")]
    [SerializeField] private float m_walk_speed;
    public float WalkSPD => m_walk_speed;

    [Header("뛰기 속력")]
    [SerializeField] private float m_run_speed;
    public float RunSPD => m_run_speed;

    [Header("총 이동 시간")]
    [SerializeField] private float m_move_time;
    public float MoveTime => m_move_time;
}