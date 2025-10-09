using InventoryService;
using KeyService;
using UnityEngine;

public class WaterRaycasterInstaller : MonoBehaviour, IInstaller
{
    [Header("물 레이캐스터")]
    [SerializeField] private WaterRaycaster m_water_raycaster;

    [Header("아이템 변경자")]
    [SerializeField] private ItemSwapper m_item_swapper;

    public void Install()
    {
        InstallRaycaster();
    }

    private void InstallRaycaster()
    {
        DIContainer.Register<WaterRaycaster>(m_water_raycaster);

        m_water_raycaster.Inject(ServiceLocator.Get<IKeyService>(),
                                 ServiceLocator.Get<IInventoryService>(),
                                 DIContainer.Resolve<NoticePresenter>(),
                                 DIContainer.Resolve<FishingPresenter>(),
                                 m_item_swapper);
    }
}
