public interface ISettingView : IPopupView
{
    void Inject(SettingPresenter presenter);

    void Initialize(SettingData setting_data);
    void OpenUI();
    void CloseUI();

    void ToggleBGMHandle(bool isOn);
    void ToggleSFXHandle(bool isOn);

    void PlaySFX(string sfx_name);
}