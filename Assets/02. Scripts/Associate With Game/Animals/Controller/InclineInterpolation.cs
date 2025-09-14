using UnityEngine;
using UnityEngine.AI;

public class InclineInterpolation : MonoBehaviour
{
    [Header("네비메시 에이전트")]
    [SerializeField] private NavMeshAgent m_agent;

    [Header("레이의 길이")]
    [SerializeField] private float m_ray_distance;

    [Header("레이어 마스크")]
    [SerializeField] private LayerMask m_ground_layer;

    [Header("보간 강도")]
    [SerializeField] private float m_interpolation_strength;

    private void Update()
    {
        Rotation();
    }

    private void Rotation()
    {
        if(Physics.Raycast(transform.position + Vector3.up, Vector3.down, out var hit, m_ray_distance, m_ground_layer))
        {
            transform.position = new Vector3(m_agent.nextPosition.x, hit.point.y, m_agent.nextPosition.z);
            var velocity = m_agent.velocity;
            var forward = velocity.sqrMagnitude > 0.01f ? velocity.normalized : transform.forward;
            var targetRotation = Quaternion.LookRotation(forward, hit.normal);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 60f * Time.deltaTime);
        }
    }
}
