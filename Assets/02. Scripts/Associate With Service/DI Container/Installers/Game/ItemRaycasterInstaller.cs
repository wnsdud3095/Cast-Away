using InventoryService;
using KeyService;
using UnityEngine;

public class ItemRaycasterInstaller : MonoBehaviour, IInstaller
{
    [Header("아이템 레이캐스터")]
    [SerializeField] private ItemRaycaster m_item_raycaster;

    public void Install()
    {
        InstallRaycaster();
    }

    private void InstallRaycaster()
    {
        DIContainer.Register<ItemRaycaster>(m_item_raycaster);

        m_item_raycaster.Inject(ServiceLocator.Get<IKeyService>(),
                                ServiceLocator.Get<IInventoryService>(),
                                DIContainer.Resolve<IItemObjectConverter>(),
                                DIContainer.Resolve<ItemDetectorPresenter>());
    }
}
