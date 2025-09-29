using System;
using InventoryService;
using KeyService;

public class ShortcutSlotPresenter : IDisposable
{
    private IShortcutSlotView m_view;
    private IItemDataBase m_item_db;
    private IKeyService m_key_service;
    private IInventoryService m_inventory_service;
    private ShortcutPresenter m_shortcut_presenter;

    public event Action<int> OnSelectedChanged;

    public int Offset { get; set; }

    public ShortcutSlotPresenter(IShortcutSlotView view,
                                 IItemDataBase item_db,
                                 IKeyService key_service,
                                 IInventoryService inventory_service,
                                 int shortcut_index,
                                 ShortcutPresenter shortcut_presenter)
    {
        m_view = view;
        m_item_db = item_db;

        m_key_service = key_service;

        m_inventory_service = inventory_service;

        Offset = shortcut_index - 12;

        m_key_service.OnUpdatedKey += m_view.UpdateUI;
        m_key_service.Initialize();

        m_view.Inject(this);
        m_shortcut_presenter = shortcut_presenter;
        // 선택 상태 이벤트 구독
        m_shortcut_presenter.OnSelectedChanged += HandleSelectedChanged;

        // Use 요청 이벤트 구독
        m_shortcut_presenter.OnUseShortcutRequested += HandleUseShortcutRequested;
    }
    private void HandleSelectedChanged(int selectedIndex)
    {
        OnSelectedChanged?.Invoke(selectedIndex);       
    }

    private void HandleUseShortcutRequested(int selectedIndex)
    {
        if (Offset == selectedIndex)
        {
            UseShortcut();
        }
    }
    public void Dispose()
    {
        m_key_service.OnUpdatedKey -= m_view.UpdateUI;
        m_shortcut_presenter.OnSelectedChanged -= HandleSelectedChanged;
        m_shortcut_presenter.OnUseShortcutRequested -= HandleUseShortcutRequested;
    }

    public void UseShortcut()
    {
        m_inventory_service.UseItem(Offset + 12);
    }
}
