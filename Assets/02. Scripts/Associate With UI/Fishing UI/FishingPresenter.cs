using InventoryService;

public class FishingPresenter : IPopupPresenter
{
    private readonly IFishingView m_view;
    private readonly IInventoryService m_inventory_service;
    private readonly PlayerCtrl m_player_ctrl;

    private bool m_is_active;
    private bool m_is_gaming;

    public bool Active => m_is_active;
    public bool Gaming => m_is_gaming;

    public FishingPresenter(IFishingView view,
                            IInventoryService inventory_service,
                            PlayerCtrl player_ctrl)
    {
        m_view = view;
        m_inventory_service = inventory_service;

        m_player_ctrl = player_ctrl;

        m_view.Inject(this);
    }

    public void OpenUI()
    {
        m_player_ctrl.ChangeState(PlayerState.Fishing);

        m_is_active = true;
        m_is_gaming = true;
        m_view.OpenUI();
    }

    public void CloseUI()
    {
        m_player_ctrl.ChangeState(PlayerState.IDLE);
        
        m_is_active = false;
        m_view.CloseUI();
    }

    public void EndGame()
    {
        m_is_gaming = false;
    }

    public void SortDepth()
    {
        m_view.SetDepth();
    }

    public void GetFish()
    {
        m_inventory_service.AddItem(ItemCode.FISH, 1);
    }
}
