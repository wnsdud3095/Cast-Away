public interface IItemDetectorView
{
    void Inject(ItemDetectorPresenter presenter);

    void OpenUI();
    void UpdateUI(string item_name, System.Numerics.Vector3 position);
    void CloseUI();
}