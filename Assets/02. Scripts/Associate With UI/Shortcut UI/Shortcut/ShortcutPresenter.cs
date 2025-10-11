using InventoryService;
using System;

public class ShortcutPresenter
{
    private readonly IShortcutView m_view;
    private readonly IInventoryService m_inventory_service;
    private readonly PlayerCtrl m_player_ctrl;

    public int SelectedIndex { get; private set; } = 0;
    public int ShortcutCount => m_shortcut_count;

    private int m_shortcut_count = 5; // 1~5번 슬롯

    public event Action<int> OnSelectedChanged;
    public event Action<ItemCode> OnSelectedChangedToCode;
    public event Action<int> OnUseShortcutRequested; // UseSelected 전용 이벤트

    public ShortcutPresenter(IShortcutView view, 
                             IInventoryService inventory_service,
                             PlayerCtrl player_ctrl)
    {
        m_view = view;
        m_inventory_service = inventory_service;
        m_player_ctrl = player_ctrl;
        m_view.Inject(this);
    }

    public void Select(int index)
    {
        if(m_player_ctrl.Interacting)
        {
            return;
        }

        SelectedIndex = index;
        OnSelectedChanged?.Invoke(index);

        var transformed_index = 12 + index;
        var item_code = m_inventory_service.GetItem(transformed_index).Code;
        OnSelectedChangedToCode?.Invoke(item_code);
    }

    public void ScrollSelect(int delta)
    {
        if(m_player_ctrl.Interacting)
        {
            return;
        }
        
        SelectedIndex = (SelectedIndex + delta + m_shortcut_count) % m_shortcut_count;
        Select(SelectedIndex);
    }

    public void UseSelected()
    {
        OnUseShortcutRequested?.Invoke(SelectedIndex);
    }
}
