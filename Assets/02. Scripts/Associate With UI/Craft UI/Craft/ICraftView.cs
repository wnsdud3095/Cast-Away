using UnityEngine;

public interface ICraftView : IPopupView
{
    void Inject(CraftPresenter presenter);

    void ClearSlots();

    void OpenUI();
    void CloseUI();

    ICraftSlotView InstantiateSlotView();
    void PlaySFX(string sfx_name);
}