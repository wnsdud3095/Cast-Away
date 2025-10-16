public interface IModulerView: IPopupView
{
    void Inject(ModulerPresenter presenter);

    void OpenUI();
    void CloseUI();

    IModulerSlotView InstantiateSlotView();
    void PlaySFX(string sfx_name);
}