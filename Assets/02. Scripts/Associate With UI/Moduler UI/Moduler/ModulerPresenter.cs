using InventoryService;
using UserService;

public class ModulerPresenter: IPopupPresenter
{
    private readonly IModulerView m_view;
    private readonly ModuleReceipe[] m_receipe_list;
    private readonly IUserService m_user_service;
    private readonly CompactModulerPresenter m_compact_moduler_presenter;

    public ModulerPresenter(IModulerView view,
                            ModuleReceipe[] receipe_list,
                            IUserService user_service,
                            CompactModulerPresenter compact_moduler_presenter)
    {
        m_view = view;

        m_receipe_list = receipe_list;
        m_user_service = user_service;
        m_compact_moduler_presenter = compact_moduler_presenter;

        m_view.Inject(this);
    }

    private void Initialize()
    {
        foreach(var receipe in m_receipe_list)
        {
            var slot_view = m_view.InstantiateSlotView();
            var slot_presenter = new ModulerSlotPresenter(slot_view,
                                                          receipe,
                                                          m_user_service,
                                                          m_compact_moduler_presenter);
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
        m_compact_moduler_presenter.CloseUI();
        m_view.PlaySFX("UI Close");
        m_view.CloseUI();
    }

    public void SortDepth()
    {
        m_view.SetDepth();
    }
}
