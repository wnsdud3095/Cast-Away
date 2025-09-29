using System;
using InventoryService;
using TMPro;

public class ItemSlotContext : IItemSlotContext
{
    private readonly IItemDataBase m_item_db;

    private readonly IInventoryService m_inventory_service;

    public ItemSlotContext(IItemDataBase item_db,
                           IInventoryService inventory_service
                           )
    {
        m_item_db = item_db;
        m_inventory_service = inventory_service;
    }

    public void Register(SlotType slot_type, Action<int, ItemData> update_action, int offset = 0, int count = 0)
    {
        switch (slot_type)
        {
            case SlotType.Inventory:
                m_inventory_service.OnUpdatedSlot += update_action;
                break;

            case SlotType.Craft:
                update_action?.Invoke(offset, Get(slot_type, offset, count));
                break;
        }       
    }             
    
    public void Discard(SlotType slot_type, Action<int, ItemData> update_action)
    {
        switch (slot_type)
        {
            case SlotType.Inventory:
                m_inventory_service.OnUpdatedSlot -= update_action;
                break;
        }        
    }    

    public ItemData Get(SlotType slot_type, int offset, int count = 1)
    {
        return slot_type switch
        {
            SlotType.Inventory              => m_inventory_service.GetItem(offset),
            SlotType.Craft => new ItemData(m_item_db.GetItem((ItemCode)offset).Code, count),
            _                               => null,
        };
    }

    public void Set(SlotType slot_type, int offset, ItemCode code, int count = 1)
    {
        var action = slot_type switch
        {
            SlotType.Inventory              => () => m_inventory_service.SetItem(offset, code, count),
            _                               => (Action)(() => {})
        };

        action();
    }

    public void Update(SlotType slot_type, int offset, int count)
    {
        var action = slot_type switch
        {
            SlotType.Inventory              => () => m_inventory_service.UpdateItem(offset, count),
            _                               => (Action)(() => {})
        };

        action();
    }

    public void Clear(SlotType slot_type, int offset)
    {
        var action = slot_type switch
        {
            SlotType.Inventory              => () => m_inventory_service.Clear(offset),
            _                               => (Action)(() => {})
        };

        action();     
    }    
}