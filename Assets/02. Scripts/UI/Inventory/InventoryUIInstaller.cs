using InventoryService;
using UnityEngine;

public class InventoryUIInstaller : MonoBehaviour, IInstaller
{
    [Header("아이템 데이터베이스")]
    [SerializeField] private ItemDataBase m_item_db;

    [Header("인벤토리 뷰")]
    [SerializeField] private InventoryView m_inventory_view;

    public void Install()
    {
        Debug.Log($"인벤토리 UI인스톨");

        DIContainer.Register<IItemDataBase>(m_item_db);
        DIContainer.Register<IInventoryView>(m_inventory_view);

        var m_inventory_presenter = new InventoryPresenter(m_inventory_view,
                                                           ServiceLocator.Get<IInventoryService>());
        DIContainer.Register<InventoryPresenter>(m_inventory_presenter);

        Inject();
    }

    private void Inject()
    {
        var item_db = DIContainer.Resolve<IItemDataBase>();

        var inventory_service = ServiceLocator.Get<IInventoryService>();
        inventory_service.Inject(item_db);
    }
}
