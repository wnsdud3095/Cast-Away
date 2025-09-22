using UnityEngine;

public class DragSlotInstaller : MonoBehaviour , IInstaller
{
    [Header("아이템 데이터베이스")]
    [SerializeField] private ItemDataBase m_item_db;

    [Header("드래그 슬롯 뷰")]
    [SerializeField] private DragSlotView m_drag_slot_view;

    public void Install()
    {
        DIContainer.Register<IItemDataBase>(m_item_db);

        var drag_slot_presenter = new DragSlotPresenter(m_drag_slot_view,
                                DIContainer.Resolve<IItemSlotContext>(),
                                DIContainer.Resolve<IItemDataBase>());

        DIContainer.Register<DragSlotPresenter>(drag_slot_presenter);
    }
}
