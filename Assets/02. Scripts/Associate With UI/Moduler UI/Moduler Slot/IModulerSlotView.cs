using UnityEngine;

public interface IModulerSlotView
{
    void Inject(ModulerSlotPresenter presenter);

    void InitUI(string module_name, Sprite module_image);
    void UpdateUI(bool unlock, int level = 0);
}