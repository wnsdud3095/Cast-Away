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

    [Header("스폰될 중립 동물 목록")]
    [SerializeField] private Animal[] m_neutrality_animal_list;

    [Header("스폰될 공격 동물 목록")]
    [SerializeField] private AggressiveAnimal[] m_aggressive_animal_list;

    [Space(20f)]
    [Header("스폰 지점 탐색 설정")]
    [Header("지면 레이어")]
    [SerializeField] private LayerMask m_ground_layer;

    [Header("탐색 레이의 길이")]
    [SerializeField] private float m_ray_distance;

    private PlayerCtrl m_player_ctrl;
    private TimeManager m_time_manager;

    private int m_max_animal_count = 8;
    [SerializeField] private int m_current_animal_count = 0;

    private void OnDisable()
    {
        m_time_manager.OnHourChanged -= SpawnNeutrality;
        m_time_manager.OnSunset -= SpawnAggressive;
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
                       TimeManager time_manager)
    {
        m_player_ctrl = player_ctrl;
        m_time_manager = time_manager;

        m_time_manager.OnHourChanged += SpawnNeutrality;
        SpawnNeutrality();

        m_time_manager.OnSunset += SpawnAggressive;
    }

    public void SpawnNeutrality()
    {
        var spawn_count = Random.Range(m_min_herd_size, m_max_herd_size + 1);
        var spawn_position = GetSpawnerRandomPosition();

        for(int i = 0; i < spawn_count; i++)
        {
            if(m_current_animal_count >= m_max_animal_count)
            {
                return;
            }

            var random_animal = m_neutrality_animal_list[Random.Range(0, m_neutrality_animal_list.Length)];
            var animal_obj = ObjectManager.Instance.GetObject(GetObjectType(random_animal.Code));
            animal_obj.transform.position = spawn_position;

            var name_tag_view = animal_obj.GetComponentInChildren<INameTagView>();
            var name_tag_presenter = new NameTagPresenter(name_tag_view);

            var animal_ctrl = animal_obj.GetComponent<AnimalCtrl>();
            animal_ctrl.Initialize(m_player_ctrl, m_time_manager, name_tag_presenter);
            animal_ctrl.Status.OnAnimalDeath += DecreaseCurrentCount;

            m_current_animal_count++;
        }
    }

    public void SpawnAggressive()
    {
        var spawn_count = Random.Range(8, 10);

        for(int i = 0; i < spawn_count; i++)
        {
            var spawn_position = GetWorldRandomPosition();

            var random_animal = m_aggressive_animal_list[Random.Range(0, m_aggressive_animal_list.Length)];
            var animal_obj = ObjectManager.Instance.GetObject(GetObjectType(random_animal.Code));
            animal_obj.transform.position = spawn_position;

            var name_tag_view = animal_obj.GetComponentInChildren<INameTagView>();
            var name_tag_presenter = new NameTagPresenter(name_tag_view);

            var animal_ctrl = animal_obj.GetComponent<AggressiveAnimalCtrl>();
            animal_ctrl.Initialize(m_player_ctrl, m_time_manager, name_tag_presenter);
        }
    }

    private Vector3 GetSpawnerRandomPosition()
    {
        var random_position = Vector3.zero;

        var m_max_attempt = 50;
        var m_current_attempt = 0;
        while(m_current_attempt < m_max_attempt)
        {
            var random_circle = Random.insideUnitCircle.normalized * Random.Range(m_min_spawn_range, m_max_spawn_range);
            random_position = m_player_ctrl.transform.position + new Vector3(random_circle.x, transform.position.y, random_circle.y);
            
            if (Physics.Raycast(random_position + (0.5f * m_ray_distance * Vector3.up), 
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

    private Vector3 GetWorldRandomPosition()
    {
        var random_position = Vector3.zero;

        var m_max_attempt = 50;
        var m_current_attempt = 0;
        while(m_current_attempt < m_max_attempt)
        {
            var random_circle = Random.insideUnitCircle.normalized * Random.Range(60f, 75f);
            random_position = new Vector3(random_circle.x, transform.position.y, random_circle.y);
            
            if (Physics.Raycast(random_position + (0.5f * m_ray_distance * Vector3.up), 
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
