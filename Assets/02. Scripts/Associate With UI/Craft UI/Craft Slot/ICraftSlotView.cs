using UnityEngine;

public interface ICraftSlotView
{
    void Inject(CraftSlotPresenter presenter);

    void InitUI(string craft_name, Sprite craft_image);
    void UpdateUI(bool unlock, int level = 0);
}