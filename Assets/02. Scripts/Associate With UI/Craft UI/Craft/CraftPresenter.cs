using InventoryService;
using UserService;

public class CraftPresenter: IPopupPresenter
{
    private readonly ICraftView m_view;
    private readonly CraftReceipe[] m_receipe_list;
    private readonly IUserService m_user_service;
    private readonly CompactCraftPresenter m_compact_craft_presenter;

    public CraftPresenter(ICraftView view,
                            CraftReceipe[] receipe_list,
                            IUserService user_service,
                            CompactCraftPresenter compact_craft_presenter)
    {
        m_view = view;

        m_receipe_list = receipe_list;
        m_user_service = user_service;
        m_compact_craft_presenter = compact_craft_presenter;

        m_view.Inject(this);
    }

    private void Initialize()
    {
        foreach(var receipe in m_receipe_list)
        {
            var slot_view = m_view.InstantiateSlotView();
            var slot_presenter = new CraftSlotPresenter(slot_view,
                                                          receipe,
                                                          m_user_service,
                                                          m_compact_craft_presenter);
        }
    }

    public void OpenUI()
    {
        m_view.PlaySFX("UI Open");
        m_view.OpenUI();

        Initialize();
    }

    public void CloseUI()
    {
        m_compact_craft_presenter.CloseUI();
        m_view.PlaySFX("UI Close");
        m_view.CloseUI();
    }

    public void SortDepth()
    {
        m_view.SetDepth();
    }
}
