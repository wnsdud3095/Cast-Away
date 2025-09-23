using System;
using InventoryService;

public class InventoryPresenter : IPopupPresenter //IDisposable
{
    private readonly IInventoryView m_view;
    private readonly IInventoryService m_model;
    private ItemSlotPresenter[] m_slot_presenters;

    // 생성자를 통해서 view와 인벤토리 서비스를 주입
    public InventoryPresenter(IInventoryView view, IInventoryService model, ItemSlotPresenter[] slot_presenters)
    {
        m_view = view;
        m_model = model;
        m_slot_presenters = slot_presenters;

        m_view.Inject(this);
    }

    // 인벤토리를 열 때
    public void OpenUI()
    {
        Initialize();
        m_view.OpenUI();
    }

    public void CloseUI()
    {
        m_view.CloseUI();
    }
    public ItemSlotPresenter GetPrioritySlotPresenter(ItemCode code)
    {
        var offset = m_model.GetPriorityOffset(code);

        return offset != -1 ? m_slot_presenters[offset] : null;
    }
    
    // 인벤토리 UI를 초기화할 때 사용한다.
    public void Initialize()
    {
        for (int i = 0; i < 12; i++)
        {
            m_model.InitializeSlot(i);
        }
    }

    // 인벤토리 서비스의 델리게이트에 연결된 이벤트를 해제한다.
    public void Dispose()
    {
        //m_model.OnUpdatedGold -= m_view.UpdateMoney;
    }
    
    public void SortDepth()
    {
        m_view.SetDepth();
    }
}