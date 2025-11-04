using KeyService;
using UnityEngine;

public class ModuleRaycasterInstaller : MonoBehaviour, IInstaller
{
    [Header("모듈 레이캐스터")]
    [SerializeField] private ModuleRaycaster m_module_raycaster;

    [Header("시간 설정자")]
    [SerializeField] private TimeSettings m_time_settings;

    [Header("시간 매니저")]
    [SerializeField] private TimeManager m_time_manager;

    public void Install()
    {
        InstallRaycaster();
    }

    private void InstallRaycaster()
    {
        m_module_raycaster.Inject(ServiceLocator.Get<IKeyService>(),
                                  DIContainer.Resolve<NoticePresenter>(),
                                  m_time_settings,
                                  m_time_manager,
                                  DIContainer.Resolve<FadePresenter>());
    }
}
