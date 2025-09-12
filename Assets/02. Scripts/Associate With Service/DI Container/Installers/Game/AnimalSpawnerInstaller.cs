using UnityEngine;

public class AnimalSpawnerInstaller : MonoBehaviour, IInstaller
{
    [Header("플레이어 컨트롤러")]
    [SerializeField] private PlayerCtrl m_player_ctrl;

    [Header("동물 스포너")]
    [SerializeField] private AnimalSpawner m_animal_spawner;

    [Header("시간 매니저")]
    [SerializeField] private TimeManager m_time_manager;

    public void Install()
    {
        m_animal_spawner.Inject(m_player_ctrl,
                                m_time_manager);
        DIContainer.Register<AnimalSpawner>(m_animal_spawner);
    }
}
