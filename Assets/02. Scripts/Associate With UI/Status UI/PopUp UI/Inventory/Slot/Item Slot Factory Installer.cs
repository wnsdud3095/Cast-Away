using InventoryService;
using UnityEngine;

public class ItemSlotFactoryInstaller : MonoBehaviour, IInstaller
{
    [Header("아이템 데이터베이스")]
    [SerializeField] private ItemDataBase m_item_db;

    [Header("커서 데이터베이스")]
    [SerializeField] private CursorDataBase m_cursor_db;


    public void Install()
    {
        var item_slot_factory = new ItemSlotFactory(ServiceLocator.Get<IInventoryService>(),
                                                    DIContainer.Resolve<IItemSlotContext>(),
                                                    m_item_db,
                                                    m_cursor_db,
                                                    DIContainer.Resolve<DragSlotPresenter>());
        DIContainer.Register<ItemSlotFactory>(item_slot_factory);
    }
}
