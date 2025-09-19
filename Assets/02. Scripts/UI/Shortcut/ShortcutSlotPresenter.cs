using System;
using KeyService;
using ShortcutService;

public class ShortcutSlotPresenter : IDisposable
{
    private IShortcutSlotView m_view;
    private IItemDataBase m_item_db;
    private IKeyService m_key_service;
    private IShortcutService m_shortcut_service;

    private InventoryPresenter m_inventory_presenter;

    private int m_offset;

    public ShortcutSlotPresenter(IShortcutSlotView view,
                                 IItemDataBase item_db,
                                 IKeyService key_service,
                                 IShortcutService shortcut_service,
                                 InventoryPresenter inventory_presenter,
                                 int offset)
    {
        m_view = view;
        m_item_db = item_db;

        m_key_service = key_service;
        m_shortcut_service = shortcut_service;

        m_inventory_presenter = inventory_presenter;
        m_offset = offset;

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
        var code = m_shortcut_service.GetItem(m_offset).Code;
        if (code == ItemCode.NONE)
        {
            return;
        }

        var item = m_item_db.GetItem(code);

        ItemSlotPresenter presenter;

        presenter = m_inventory_presenter.GetPrioritySlotPresenter(code);

        presenter?.OnPointerClick();
    }
}
