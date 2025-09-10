using UnityEngine;

[RequireComponent(typeof(AnimalCtrl))]
public class AnimalMovement : MonoBehaviour
{
    private AnimalCtrl m_controller;

    [Header("레이의 길이")]
    [SerializeField] private float m_ray_distance;
    
    [Header("레이가 감지할 레이어")]
    [SerializeField] private LayerMask m_ground_mask;

    [Header("보간 크기")]
    [SerializeField] private float m_smooth;
    
    private float m_idle_time;
    private float m_move_time;

    private float m_walk_speed;
    private float m_run_speed;

    public float IdleTime => m_idle_time;
    public float MoveTime => m_move_time;

    public bool IsWalk { get; set; }
    public bool IsRun { get; set; }

    private void Awake()
    {
        m_controller = GetComponent<AnimalCtrl>();
    }
    
    private void Update()
    {
        m_controller.Movement.InclineInterpolation();
    }

    public void Initialize(float idle_time,
                           float walk_speed,
                           float run_speed,
                           float move_time)
    {
        m_idle_time = idle_time;

        m_walk_speed = walk_speed;
        m_run_speed = run_speed;

        m_move_time = move_time;
    }

    public void Move(Vector3 destination)
    {
        m_controller.Agent.speed = IsRun ? m_run_speed : m_walk_speed;
        
        var target_pos = transform.position + destination;
        m_controller.Agent.SetDestination(target_pos);
    } 

    public void InclineInterpolation()
    {
        var agent_position = m_controller.Agent.nextPosition;

        if (Physics.Raycast(agent_position + Vector3.up, Vector3.down, out var hit, m_ray_distance, m_ground_mask))
        {
            transform.position = new Vector3(agent_position.x, hit.point.y, agent_position.z);

            var target_rotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
            transform.rotation = Quaternion.Slerp(transform.rotation, target_rotation, Time.deltaTime * m_smooth);
        }
    }
}
