using InventoryService;
using System;
using Unity.Android.Gradle.Manifest;

public class CompactCraftPresenter : IDisposable
{
    private readonly ICompactCraftView m_view;
    private readonly IInventoryService m_inventory_service;
    private CraftReceipe m_craft_receipe;

    public CompactCraftPresenter(ICompactCraftView view,
                                   IInventoryService inventory_service)
    {
        m_view = view;
        m_inventory_service = inventory_service;

        m_view.Inject(this);
    }

    private void Initialize(CraftReceipe receipe)
    {
        m_craft_receipe = receipe;

        foreach (var ingredient in receipe.Ingredients)
        {
            var slot_view = m_view.InstantiateSlotView();
            var slot_presenter = new CraftIngredientSlotPresenter(slot_view,
                                                                    ingredient,
                                                                    m_inventory_service);
        }
    }

    public void OpenUI(CraftReceipe receipe)
    {
        m_inventory_service.OnUpdatedSlot += UpdateUI;

        m_view.OpenUI(receipe.Name, receipe.Image);
        Initialize(receipe);
        UpdateUI(-1, null);
    }

    private void UpdateUI(int offset, ItemData item_data)
    {
        var active = true;
        foreach (var ingredient in m_craft_receipe.Ingredients)
        {
            if (m_inventory_service.GetItemCount(ingredient.Item.Code) < ingredient.Count)
            {
                active = false;
            }
        }

        m_view.UpdateUI(active);
    }

    public void CloseUI()
    {
        m_inventory_service.OnUpdatedSlot -= UpdateUI;

        m_view.CloseUI();
    }

    public void OnClickedCraft()
    {
        var crafted_item_code = m_craft_receipe.Code;

        if (m_inventory_service.GetValidOffset(crafted_item_code) < 0) 
        {
            return;
        }

        foreach (var ingredient in m_craft_receipe.Ingredients)
        {
            m_inventory_service.ConsumeItem(ingredient.Item.Code, ingredient.Count);
        }
        m_inventory_service.AddItem(crafted_item_code, 1);
    }
    public void Dispose()
    {
        m_inventory_service.OnUpdatedSlot -= UpdateUI;
    }
}
