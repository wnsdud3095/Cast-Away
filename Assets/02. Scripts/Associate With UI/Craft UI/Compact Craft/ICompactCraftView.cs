using UnityEngine;

public interface ICompactCraftView
{
    void Inject(CompactCraftPresenter presenter);

    void OpenUI(string item_name, Sprite item_image);
    void UpdateUI(bool active);
    void CloseUI();

    ICraftIngredientSlotView InstantiateSlotView();
    void PlaySFX(string sfx_name);
}