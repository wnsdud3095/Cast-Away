using System;
using InventoryService;

public class InventoryPresenter : IPopupPresenter //IDisposable
{
    private readonly IInventoryView m_view;
    private readonly IInventoryService m_model;

    // 생성자를 통해서 view와 인벤토리 서비스를 주입
    public InventoryPresenter(IInventoryView view, IInventoryService model)
    {
        m_view = view;
        m_model = model;

        m_view.Inject(this);
    }

    // 인벤토리를 열 때
    public void OpenUI()
    {
        m_view.OpenUI();
    }

    public void CloseUI()
    {
        m_view.CloseUI();
    }

    /*
    // 인벤토리 UI를 초기화할 때 사용한다.
    public void Initialize()
    {
        m_model.InitializeGold();
    }

    // 인벤토리 서비스의 델리게이트에 연결된 이벤트를 해제한다.
    public void Dispose()
    {
        m_model.OnUpdatedGold -= m_view.UpdateMoney;
    }
    */
    public void SortDepth()
    {
        m_view.SetDepth();
    }
}