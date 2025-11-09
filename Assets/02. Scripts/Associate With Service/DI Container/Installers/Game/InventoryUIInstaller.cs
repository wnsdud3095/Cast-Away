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

    [Header("쓰레기통 뷰")]
    [SerializeField] private ItemSlotView m_trash_view;

    public void Install()
    {
        DIContainer.Register<IItemDataBase>(m_item_db);
        DIContainer.Register<IInventoryView>(m_inventory_view);

        DIContainer.Register<IInventoryService>(ServiceLocator.Get<IInventoryService>());

        var slot_views = m_item_slot_root.GetComponentsInChildren<IItemSlotView>();

        var item_slot_factory = DIContainer.Resolve<ItemSlotFactory>();

        var slot_presenters = new ItemSlotPresenter[slot_views.Length];
        for (int i = 0; i < slot_presenters.Length; i++) // 숏컷 0~4, 인벤토리 5~16
        {
            int index = i + 5;
            slot_presenters[i] = item_slot_factory.Instantiate(slot_views[i], index, SlotType.Inventory);
        }

        item_slot_factory.Instantiate(m_trash_view, 17, SlotType.TrashCan);

        var m_inventory_presenter = new InventoryPresenter(m_inventory_view,
                                                           ServiceLocator.Get<IInventoryService>(),
                                                           slot_presenters);
        DIContainer.Register<InventoryPresenter>(m_inventory_presenter);
        m_inventory_presenter.Initialize();
        Inject();
    }

    private void Inject()
    {
        var item_db = DIContainer.Resolve<IItemDataBase>();

        var inventory_service = ServiceLocator.Get<IInventoryService>();
        inventory_service.Inject(item_db);
    }
}
