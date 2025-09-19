public class ItemSlotPresenter
{
    private readonly IItemSlotView m_view;
    private readonly IItemDataBase m_item_db;

    private DragSlotPresenter m_drag_slot_presenter;

    private int m_offset;
    private SlotType m_slot_type;
    private int m_item_count;

    // 상점이나 제작소에서 사용되는 슬롯인지의 여부를 확인한다.
    public bool IsShopOrCraft => m_slot_type == SlotType.Shop || m_slot_type == SlotType.Craft;

    public ItemSlotPresenter(IItemSlotView view,
                             IItemDataBase item_db,
                             int offset,
                             SlotType slot_type = SlotType.Inventory,
                             int item_count = 1)
    {
        m_view = view;
        m_item_db = item_db;
        m_offset = offset;
        m_slot_type = slot_type;
        m_item_count = item_count;

        m_view.Inject(this);
    }

    public void UpdateSlot(int offset, ItemData item_data)
    {
        // 상점이나 제작소에 사용되는 슬롯이거나
        // 오프셋이 다른 경우에는 업데이트하지 않는다.
        if (!IsShopOrCraft && m_offset != offset)
        {
            return;
        }

        // 아이템 코드가 비어있을 경우, 슬롯이 빈 것으로 판단한다.
        if (item_data.Code == ItemCode.NONE)
        {
            m_view.ClearUI();
            return;
        }

        // 아이템 데이터베이스로부터 SO를 얻어 그 정보로 View를 갱신한다.
        var item = m_item_db.GetItem(item_data.Code);
        m_view.UpdateUI(item.Sprite, item.Stackable, item_data.Count);
    }
}