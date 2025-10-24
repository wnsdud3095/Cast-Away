using UnityEngine;

public interface ICraftIngredientSlotView
{
    void Inject(CraftIngredientSlotPresenter presenter);

    void UpdateUI(string item_name, Sprite item_image, int count, bool active);
}
