using ItemService;
using UnityEngine;

public class ItemUIInstaller : MonoBehaviour , IInstaller
{
    [Header("아이템 데이터베이스")]
    [SerializeField] private ItemDataBase m_item_db;

    [Header("드래그 슬롯 뷰")]
    [SerializeField] private DragSlotView m_drag_slot_view;

    [Header("툴팁 뷰")]
    [SerializeField] private ToolTipView m_tooltip_view;

    public void Install()
    {
        DIContainer.Register<IItemDataBase>(m_item_db);

        var drag_slot_presenter = new DragSlotPresenter(m_drag_slot_view,
                                                        DIContainer.Resolve<IItemSlotContext>(),
                                                        DIContainer.Resolve<IItemDataBase>());

        DIContainer.Register<DragSlotPresenter>(drag_slot_presenter);

        var tooltip_presenter = new ToolTipPresenter(m_tooltip_view,
                                                    ServiceLocator.Get<IItemDataService>(),
                                                     DIContainer.Resolve<IItemDataBase>());
        DIContainer.Register<ToolTipPresenter>(tooltip_presenter);
    }
}
