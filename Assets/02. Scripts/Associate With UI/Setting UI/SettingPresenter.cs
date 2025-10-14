using SettingService;

public class SettingPresenter : IPopupPresenter
{
    private readonly ISettingView m_view;
    private readonly ISettingService m_setting_service;

    public SettingPresenter(ISettingView view,
                            ISettingService setting_service)
    {
        m_view = view;
        m_setting_service = setting_service;

        m_view.Inject(this);
    }

    public void OpenUI()
    {
        m_view.Initialize(m_setting_service.Data);
        m_view.OpenUI();
        m_view.PlaySFX("UI Open");
    }

    public void CloseUI()
    {
        m_view.CloseUI();
        m_view.PlaySFX("UI Close");
    }

    public void OnValueChangedMouseSensitivity(float value)
    {
        m_setting_service.MouseSensitivity = value;
    }

    public void OnValueChangedMouseInversion(bool isOn)
    {
        m_setting_service.MouseInversion = isOn;
        m_view.PlaySFX("Button Click");
    }

    public void OnValueChangedCameraShaking(bool isOn)
    {
        m_setting_service.CameraShaking = isOn;
        m_view.PlaySFX("Button Click");
    }

    public void OnValueChangedBGM(bool isOn)
    {
        m_setting_service.BGMPrint = isOn;
        
        m_view.ToggleBGMHandle(isOn);
        m_view.PlaySFX("Button Click");

        SoundManager.Instance.BGM.volume = isOn ? m_setting_service.BGMRate : 0f;
    }

    public void OnValueChangedBGMRate(float value)
    {
        m_setting_service.BGMRate = value;
        SoundManager.Instance.BGM.volume = value;
    }

    public void OnValueChangedSFX(bool isOn)
    {
        m_setting_service.SFXPrint = isOn;

        m_view.ToggleSFXHandle(isOn);
        m_view.PlaySFX("Button Click");
    }

    public void OnValueChangedSFXRate(float value)
    {
        m_setting_service.SFXRate = value;
    }

    public void OnClickedTitle()
    {
        // TODO: 저장 후 타이틀씬으로 이동
    }

    public void SortDepth()
    {
        m_view.SetDepth();
    }
}
