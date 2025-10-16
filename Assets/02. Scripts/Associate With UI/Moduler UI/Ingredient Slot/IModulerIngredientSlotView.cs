using UnityEngine;

public interface IModulerIngredientSlotView
{
    void Inject(ModulerIngredientSlotPresenter presenter);
    
    void UpdateUI(string item_name, Sprite item_image, int count, bool active);
}
