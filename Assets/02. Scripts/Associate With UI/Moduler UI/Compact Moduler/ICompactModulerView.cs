using UnityEngine;

public interface ICompactModulerView
{
    void Inject(CompactModulerPresenter presenter);

    void OpenUI(string item_name, Sprite item_image);
    void UpdateUI(bool active);
    void CloseUI();

    IModulerIngredientSlotView InstantiateSlotView();
}