using UnityEngine;

public class AnimalSpawnerInstaller : MonoBehaviour, IInstaller
{
    [Header("플레이어 컨트롤러")]
    [SerializeField] private PlayerCtrl m_player_ctrl;

    [Header("동물 스포너")]
    [SerializeField] private AnimalSpawner m_animal_spawner;

    [Header("시간 매니저")]
    [SerializeField] private TimeManager m_time_manager;

    [Header("카메라 셰이커")]
    [SerializeField] private CameraShaker m_camera_shaker;

    public void Install()
    {
        InstallSpawner();
    }

    private void InstallSpawner()
    {
        m_animal_spawner.Inject(m_player_ctrl,
                                m_time_manager,
                                m_camera_shaker);
        DIContainer.Register<AnimalSpawner>(m_animal_spawner);
    }
}
