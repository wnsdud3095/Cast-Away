public interface IFishingView : IPopupView
{
    void Inject(FishingPresenter presenter);

    void OpenUI();
    void CloseUI();

    void StartGame();
}