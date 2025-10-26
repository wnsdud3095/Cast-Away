using InventoryService;
using System;
using UserService;

public class CraftSlotPresenter : IDisposable
{
    private readonly ICraftSlotView m_view;
    private readonly CraftReceipe m_receipe;
    private readonly IUserService m_user_service;
    private readonly CompactCraftPresenter m_compact_craft_presenter;

    public CraftSlotPresenter(ICraftSlotView view,
                                CraftReceipe receipe,
                                IUserService user_service,
                                CompactCraftPresenter compact_moduler_presenter)
    {
        m_view = view;
        m_receipe = receipe;
        m_user_service = user_service;
        m_compact_craft_presenter = compact_moduler_presenter;

        m_user_service.OnUpdatedLevel += Initialize;
        m_user_service.InitLevel();

        m_view.Inject(this);
    }

    private void Initialize(int level, int exp)
    {
        m_view.InitUI(m_receipe.Name, m_receipe.Image);

        var unlock = level >= m_receipe.Unlock;
        m_view.UpdateUI(unlock, m_receipe.Unlock);
    }

    public void OnClickedInfo()
    {
        m_compact_craft_presenter.CloseUI();
        m_compact_craft_presenter.OpenUI(m_receipe);
    }

    public void Dispose()
    {
        m_user_service.OnUpdatedLevel -= Initialize;
    }
}
