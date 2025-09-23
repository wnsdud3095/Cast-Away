using System;
using InventoryService;
using KeyService;
using ShortcutService;

public class ShortcutSlotPresenter : IDisposable
{
    private IShortcutSlotView m_view;
    private IItemDataBase m_item_db;
    private IKeyService m_key_service;
    private IInventoryService m_inventory_service;

    public int Offset { get; set; }

    public ShortcutSlotPresenter(IShortcutSlotView view,
                                 IItemDataBase item_db,
                                 IKeyService key_service,
                                 IInventoryService inventory_service,
                                 int shortcut_index)
    {
        m_view = view;
        m_item_db = item_db;

        m_key_service = key_service;

        m_inventory_service = inventory_service;

        Offset = shortcut_index;

        m_key_service.OnUpdatedKey += m_view.UpdateUI;
        m_key_service.Initialize();

        m_view.Inject(this);
    }

    public void Dispose()
    {
        m_key_service.OnUpdatedKey -= m_view.UpdateUI;
    }

    public void UseShortcut()
    {
        m_inventory_service.UseItem(Offset);
    }
}
