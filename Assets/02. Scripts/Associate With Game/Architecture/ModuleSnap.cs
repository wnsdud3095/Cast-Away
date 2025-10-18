using UnityEngine;

public class ModuleSnap : MonoBehaviour
{
    [Header("바닥면 스냅 여부")]
    [SerializeField] private bool m_floor_snap = true;

    [Header("바닥면이 스냅 될 위치")]
    [SerializeField] private Vector3 m_floor_snap_position;

    [Header("벽면 스냅 여부")]
    [SerializeField] private bool m_wall_snap = true;

    [Header("벽면이 스냅 될 위치")]
    [SerializeField] private Vector3 m_wall_snap_position; 

    private void OnTriggerEnter(Collider collider)
    {
        var preview_obj = collider.GetComponent<PreviewObject>();
        if(preview_obj == null)
        {
            return;
        }

        if(m_wall_snap && collider.CompareTag("Preview Wall"))
        {
            preview_obj.SnapTo(transform.parent.TransformPoint(m_wall_snap_position));
        }

        if(m_floor_snap && collider.CompareTag("Preview Floor"))
        {
            preview_obj.SnapTo(transform.parent.TransformPoint(m_floor_snap_position));
        }
    }
}