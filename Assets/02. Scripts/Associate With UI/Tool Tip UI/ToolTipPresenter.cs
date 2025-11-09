using ItemService;

public class ToolTipPresenter
{
    private readonly IToolTipView m_view;
    private readonly IItemDataService m_item_data_service;
    private readonly IItemDataBase m_item_db;

    public ToolTipPresenter(IToolTipView view, IItemDataService item_data_service, IItemDataBase item_db)
    {
        m_view = view;
        m_item_data_service = item_data_service;
        m_item_db = item_db;

        m_view.Inject(this);

    }

    public void OpenUI(ItemCode code)
    {
        var item = m_item_db.GetItem(code);

        var name = m_item_data_service.GetName(code);
        var type = GetTypeName(item.Type);
        var desc = m_item_data_service.GetDescription(code);


        m_view.UpdateUI(item.Sprite, name, type, desc);
        m_view.OpenUI();
    }

    public void CloseUI()
    {
        m_view.CloseUI();
    }

    private string GetTypeName(ItemType type)
    {
        if ((int)(type & ItemType.Foods) != 0)
        {
            return "음식";
        }
        else if((int)(type & ItemType.Tools) != 0)
        {
            return "도구";
        }
        else if ((int)(type & ItemType.Material) != 0)
        {
            return "재료";
        }
        else
        {
            return "재료 아이템";
        }
    }
}