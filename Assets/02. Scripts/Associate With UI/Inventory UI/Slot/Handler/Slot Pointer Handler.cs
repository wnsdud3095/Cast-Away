using UnityEngine.EventSystems;

public class SlotPointerHandler
{
    private IItemSlotContext m_slot_context;

    private IItemDataBase m_item_db;
    private ICursorDataBase m_cursor_db;

    private SlotType m_slot_type;
    private int m_offset;

    public SlotPointerHandler(IItemSlotContext slot_context,
                              IItemDataBase item_db,
                              ICursorDataBase cursor_db)
    {
        m_slot_context = slot_context;
        m_item_db = item_db;
        m_cursor_db = cursor_db;
    }

    public void OnPointerEnter(SlotType slot_type, int offset)
    {
        var code = m_slot_context.Get(slot_type, offset).Code;
        if (code == ItemCode.NONE)
        {
            return;
        }

        m_slot_type = slot_type;
        m_offset = offset;

        m_cursor_db.SetCursor(CursorMode.CAN_GRAB);
    }

    public void OnPointerExit()
    {
        m_cursor_db.SetCursor(CursorMode.DEFAULT);
    }

    public void OnPointerClick(SlotType slot_type, int offset)
    {
        m_slot_type = slot_type;
        m_offset = offset;

        m_cursor_db.SetCursor(CursorMode.DEFAULT);

        var code = m_slot_context.Get(m_slot_type, m_offset).Code;
        if (code == ItemCode.NONE)
        {
            return;
        }

        if (m_slot_type == SlotType.Shortcut)
        {
            return;
        }


        var item = m_item_db.GetItem(code);

        if (item.Type == ItemType.Consumable)
        {
            var count = m_slot_context.Get(m_slot_type, m_offset).Count;
            if (count > 1)
            {
                m_slot_context.Update(m_slot_type, m_offset, -1);
                if (EventSystem.current.IsPointerOverGameObject())
                {
                    m_cursor_db.SetCursor(CursorMode.CAN_GRAB);
                }
                else
                {
                    m_cursor_db.SetCursor(CursorMode.DEFAULT);
                }
            }
            else
            {
                m_slot_context.Clear(m_slot_type, m_offset);
                m_cursor_db.SetCursor(CursorMode.DEFAULT);
            }
        }      
    }
}
