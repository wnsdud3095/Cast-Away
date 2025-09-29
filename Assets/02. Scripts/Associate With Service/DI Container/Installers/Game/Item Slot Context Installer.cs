using InventoryService;
using UnityEngine;

public class ItemSlotContextInstaller : MonoBehaviour, IInstaller
{
    [Header("아이템 데이터베이스")]
    [SerializeField] private ItemDataBase m_item_db;

    public void Install()
    {
        var item_slot_context = new ItemSlotContext(m_item_db,
                                                    ServiceLocator.Get<IInventoryService>()
                                                    ); 
        DIContainer.Register<IItemSlotContext>(item_slot_context);
    }
}
