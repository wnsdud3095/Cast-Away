using InventoryService;
using System;

public class CompactModulerPresenter : IDisposable
{
    private readonly ICompactModulerView m_view;
    private readonly IInventoryService m_inventory_service;
    private readonly ModulerTutorialPresenter m_module_tutorial_presenter;
    private readonly Moduler m_moduler;

    private ModuleReceipe m_module_receipe;

    public CompactModulerPresenter(ICompactModulerView view,
                                   IInventoryService inventory_service,
                                   ModulerTutorialPresenter module_tutorial_presenter,
                                   Moduler moduler)
    {
        m_view = view;
        m_inventory_service = inventory_service;
        m_module_tutorial_presenter = module_tutorial_presenter;
        m_moduler = moduler;

        m_view.Inject(this);
    }

    private void Initialize(ModuleReceipe receipe)
    {
        m_module_receipe = receipe;

        foreach(var ingredient in receipe.Ingredients)
        {
            var slot_view = m_view.InstantiateSlotView();
            var slot_presenter = new ModulerIngredientSlotPresenter(slot_view, 
                                                                    ingredient,
                                                                    m_inventory_service);
        }
    }

    public void OpenUI(ModuleReceipe receipe)
    {
        m_inventory_service.OnUpdatedSlot += UpdateUI;

        m_view.OpenUI(receipe.Name, receipe.Image);
        Initialize(receipe);
        UpdateUI(-1, null);
    }

    private void UpdateUI(int offset, ItemData item_data)
    {
        var active = true;
        foreach(var ingredient in m_module_receipe.Ingredients)
        {
            if(m_inventory_service.GetItemCount(ingredient.Item.Code) < ingredient.Count)
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

    public void OnClickedBuild()
    {
        m_module_tutorial_presenter.OpenUI();
        m_moduler.Activate(m_module_receipe);
    }

    public void Dispose()
    {
        m_inventory_service.OnUpdatedSlot -= UpdateUI;
    }
}
