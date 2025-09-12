using UnityEngine;
using UnityEngine.AI;

public class AnimalSpawner : MonoBehaviour
{
    [Header("스포너 관련 설정")]
    [Header("최소 스폰 범위")]
    [SerializeField] private float m_min_spawn_range;

    [Header("최대 스폰 범위")]
    [SerializeField] private float m_max_spawn_range;

    [Header("동물 무리 최소 크기")]
    [SerializeField] private int m_min_herd_size;

    [Header("동물 무리 최대 크기")]
    [SerializeField] private int m_max_herd_size;

    [Header("스폰될 동물 목록")]
    [SerializeField] private Animal[] m_animal_list;

    [Space(20f)]
    [Header("스폰 지점 탐색 설정")]
    [Header("지면 레이어")]
    [SerializeField] private LayerMask m_ground_layer;

    [Header("탐색 레이의 길이")]
    [SerializeField] private float m_ray_distance;

    private PlayerCtrl m_player_ctrl;
    private MouseDetectorPresenter m_mouse_detector_presenter;
    private TimeManager m_time_manager;

    private int m_max_animal_count = 8;
    [SerializeField] private int m_current_animal_count = 0;

    private void OnDisable()
    {
        m_time_manager.OnHourChanged -= Spawn;
    }

    private void OnTriggerExit(Collider collider)
    {
        if(collider.CompareTag("Animal"))
        {
            Return(collider);
        }
    }

    private void OnDrawGizmos()
    {
        if(m_player_ctrl == null)
        {
            return;
        } 

        Gizmos.color = Color.green;

        Gizmos.DrawWireSphere(m_player_ctrl.transform.position, m_min_spawn_range);
        Gizmos.DrawWireSphere(m_player_ctrl.transform.position, m_max_spawn_range);
    }

    public void Inject(PlayerCtrl player_ctrl,
                       MouseDetectorPresenter mouse_detector_presenter,
                       TimeManager time_manager)
    {
        m_player_ctrl = player_ctrl;
        m_mouse_detector_presenter = mouse_detector_presenter;
        m_time_manager = time_manager;

        m_time_manager.OnHourChanged += Spawn;
        Spawn();
    }

    public void Spawn()
    {
        var spawn_count = Random.Range(m_min_herd_size, m_max_herd_size + 1);
        var spawn_position = GetRandomPosition();

        for(int i = 0; i < spawn_count; i++)
        {
            if(m_current_animal_count >= m_max_animal_count)
            {
                return;
            }

            var random_animal = m_animal_list[Random.Range(0, m_animal_list.Length)];
            var animal_obj = ObjectManager.Instance.GetObject(GetObjectType(random_animal.Code));
            animal_obj.transform.position = spawn_position;

            var animal_ctrl = animal_obj.GetComponent<AnimalCtrl>();
            animal_ctrl.Initialize(m_player_ctrl);
            animal_ctrl.Status.OnAnimalDeath += DecreaseCurrentCount;

            var animal_mouse_detector = animal_obj.GetComponent<AnimalMouseDetector>();
            animal_mouse_detector.Inject(m_mouse_detector_presenter);

            m_current_animal_count++;
        }
    }

    private Vector3 GetRandomPosition()
    {
        var random_position = Vector3.zero;

        var m_max_attempt = 50;
        var m_current_attempt = 0;
        while(m_current_attempt < m_max_attempt)
        {
            var random_circle = Random.insideUnitCircle.normalized * Random.Range(m_min_spawn_range, m_max_spawn_range);
            random_position = m_player_ctrl.transform.position + new Vector3(random_circle.x, transform.position.y, random_circle.y);
            
            if (Physics.Raycast(random_position + (Vector3.up * m_ray_distance * 0.5f), 
                                Vector3.down, 
                                out RaycastHit hit, 
                                m_ray_distance, 
                                m_ground_layer))
            {
                if (NavMesh.SamplePosition(hit.point, 
                                           out NavMeshHit nav_hit, 
                                           2f, 
                                           NavMesh.AllAreas))
                {
                    return nav_hit.position;
                }
            }

            m_current_attempt++;
        }

        return Vector3.zero;
    }

    private void Return(Collider animal_collider)
    {
        var animal_ctrl = animal_collider.GetComponent<AnimalCtrl>();
        animal_ctrl.ChangeState(AnimalState.RETURNED);
        var animal = animal_ctrl.SO;

        var container = ObjectManager.Instance.GetPool(GetObjectType(animal.Code)).Container;
        animal_collider.transform.position = container.transform.position;

        ObjectManager.Instance.ReturnObject(animal_collider.gameObject, GetObjectType(animal.Code));
    }

    private ObjectType GetObjectType(AnimalCode animal_code) => (ObjectType)((int)animal_code + 201);

    private void DecreaseCurrentCount(AnimalCtrl animal_ctrl)
    {
        m_current_animal_count--;
        animal_ctrl.Status.OnAnimalDeath -= DecreaseCurrentCount;
    }
}
