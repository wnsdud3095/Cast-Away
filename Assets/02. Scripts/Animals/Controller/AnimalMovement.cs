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

    public Vector3 Velocity => transform.forward * (IsWalk ? m_walk_speed : m_run_speed);

    private void Awake()
    {
        m_controller = GetComponent<AnimalCtrl>();
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

    public void Move()
    {
        m_controller.Rigidbody.MovePosition(transform.position +
                                            Velocity *
                                            Time.deltaTime);
    } 

    public void Rotation(Vector3 direction)
    {
        var yaw = new Vector3(0f, direction.y, 0f);
        var desired_forward = Quaternion.Euler(yaw) * Vector3.forward;

        if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out RaycastHit hit, m_ray_distance, m_ground_mask))
        {
            var projected_forward = Vector3.ProjectOnPlane(desired_forward, hit.normal).normalized;
            if (projected_forward.sqrMagnitude < 1e-4f) 
            {
                projected_forward = transform.forward;
            }

            var target_rotation = Quaternion.LookRotation(projected_forward, hit.normal);
            m_controller.Rigidbody.MoveRotation(Quaternion.Slerp(transform.rotation, target_rotation, Time.deltaTime * m_smooth));
        }
        else
        {
            var rotation = Vector3.Lerp(transform.rotation.eulerAngles, direction, Time.deltaTime);
            m_controller.Rigidbody.MoveRotation(Quaternion.Euler(rotation));
        }
    }
}
