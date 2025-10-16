using SettingService;
using UnityEngine;

public class CameraCtrl : MonoBehaviour
{
    [Header("플레이어")]
    [SerializeField] private Transform m_player_transform;

    [Header("카메라")]
    [SerializeField] private Transform m_camera_transform;

    private float m_camera_distance;
    private ISettingService m_setting_service;

    public Vector2 Delta { get; set; }

    private void Awake()
    {
        m_camera_distance = Vector3.Distance(m_player_transform.position, m_camera_transform.position);
    }

    private void Update()
    {
        if(GameManager.Instance.GameType == GameEventType.INPLAY)
        {
            Rotation();
            Translation();
        }
    }

    public void Inject(ISettingService setting_service)
    {
        m_setting_service = setting_service;
    }

    private void Rotation()
    {
        Delta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        var current_direction = transform.rotation.eulerAngles;
        
        var final_x = !m_setting_service.MouseInversion ? (current_direction.x - Delta.y) : (current_direction.x + Delta.y);
        final_x = final_x < 180f ? Mathf.Clamp(final_x, -1f, 60f) : Mathf.Clamp(final_x, 340f, 361f);

        transform.rotation = Quaternion.Euler(final_x, current_direction.y + Delta.x, current_direction.z);
    }

    private void Translation()
    {
        var ray_direction = (m_camera_transform.position - transform.position).normalized;

        Debug.DrawRay(transform.position, ray_direction * m_camera_distance, Color.red);
        if(Physics.Raycast(transform.position, ray_direction, out var ray_info, m_camera_distance))
        {
            if(CheckObstacle(ray_info.collider))
            {
                m_camera_transform.position = Vector3.Lerp(m_camera_transform.position, 
                                                           ray_info.point - ray_direction * 0.3f, 
                                                           Time.deltaTime * 10f);
                return;
            }
        }

        m_camera_transform.position = Vector3.Lerp(m_camera_transform.position, 
                                                   transform.position + ray_direction * m_camera_distance, 
                                                   Time.deltaTime * 10f);
    }

    private bool CheckObstacle(Collider collider)
    {
        if(!collider.CompareTag("Player") &&
           !collider.CompareTag("Animal") &&
           !collider.CompareTag("Enemy") &&
           !collider.CompareTag("Item") &&
           !collider.CompareTag("Tool") &&
           !collider.CompareTag("UI") &&
           collider != null)
        {
            return true;
        }

        return false;
    }
}
