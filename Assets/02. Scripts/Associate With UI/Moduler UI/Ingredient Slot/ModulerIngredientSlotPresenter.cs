using InventoryService;
using System;

public class ModulerIngredientSlotPresenter : IDisposable
{
    private readonly IModulerIngredientSlotView m_view;
    private readonly IngredientData m_ingredient_data;
    private readonly IInventoryService m_inventory_service;

    public ModulerIngredientSlotPresenter(IModulerIngredientSlotView view,
                                          IngredientData ingredient_data,
                                          IInventoryService inventory_service)
    {
        m_view = view;
        m_ingredient_data = ingredient_data;
        m_inventory_service = inventory_service;

        m_inventory_service.OnUpdatedSlot += UpdateUI;

        m_view.Inject(this);

        UpdateUI(-1, null);
    }

    public void UpdateUI(int offset, ItemData item_data)
    {
        var active = m_inventory_service.GetItemCount(m_ingredient_data.Item.Code) >= m_ingredient_data.Count;

        m_view.UpdateUI(m_ingredient_data.Item.Name,
                        m_ingredient_data.Item.Sprite,
                        m_ingredient_data.Count,
                        active);
    }

    public void Dispose()
    {
        m_inventory_service.OnUpdatedSlot -= UpdateUI;
    }
}
