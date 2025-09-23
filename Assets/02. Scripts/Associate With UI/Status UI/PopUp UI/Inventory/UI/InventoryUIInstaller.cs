using InventoryService;
using UnityEngine;

public class InventoryUIInstaller : MonoBehaviour, IInstaller
{
    [Header("아이템 데이터베이스")]
    [SerializeField] private ItemDataBase m_item_db;

    [Header("인벤토리 뷰")]
    [SerializeField] private InventoryView m_inventory_view;

    [Header("아이템 슬롯의 루트")]
    [SerializeField] private Transform m_item_slot_root;

    public void Install()
    {
        Debug.Log($"인벤토리 UI인스톨");

        DIContainer.Register<IItemDataBase>(m_item_db);
        DIContainer.Register<IInventoryView>(m_inventory_view);

        DIContainer.Register<IInventoryService>(ServiceLocator.Get<IInventoryService>());

        var slot_views = m_item_slot_root.GetComponentsInChildren<IItemSlotView>();

        var item_slot_factory = DIContainer.Resolve<ItemSlotFactory>();

        var slot_presenters = new ItemSlotPresenter[slot_views.Length];
        for (int i = 0; i < slot_presenters.Length; i++)
        {
            slot_presenters[i] = item_slot_factory.Instantiate(slot_views[i], i, SlotType.Inventory);
        }

        var m_inventory_presenter = new InventoryPresenter(m_inventory_view,
                                                           ServiceLocator.Get<IInventoryService>(),
                                                           slot_presenters);
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
