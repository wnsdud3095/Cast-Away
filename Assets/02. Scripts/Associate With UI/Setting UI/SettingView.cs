using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class SettingView : MonoBehaviour, ISettingView
{
    [Header("UI 관련 컴포넌트")]
    [Header("팝업 UI 매니저")]
    [SerializeField] private PopupUIManager m_ui_manager;

    [Header("마우스 감도")]
    [SerializeField] private Slider m_sensitivity_slider;

    [Header("마우스 반전")]
    [SerializeField] private Toggle m_inversion_toggle;

    [Header("카메라 흔들림")]
    [SerializeField] private Toggle m_shaking_toggle;

    [Header("BGM 출력")]
    [SerializeField] private Toggle m_bgm_toggle;

    [Header("BGM 제어")]
    [SerializeField] private Slider m_bgm_slider;

    [Header("SFX 출력")]
    [SerializeField] private Toggle m_sfx_toggle;

    [Header("SFX 제어")]
    [SerializeField] private Slider m_sfx_slider;

    [Header("타이틀 버튼")]
    [SerializeField] private Button m_title_button;

    private Animator m_animator;
    private SettingPresenter m_presenter;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
    }

    private void OnDestroy()
    {
        m_sensitivity_slider.onValueChanged.RemoveListener((value) => m_presenter.OnValueChangedMouseSensitivity(value));
        m_inversion_toggle.onValueChanged.RemoveListener((isOn) => m_presenter.OnValueChangedMouseInversion(isOn));
        m_shaking_toggle.onValueChanged.RemoveListener((isOn) => m_presenter.OnValueChangedCameraShaking(isOn)); 

        m_bgm_toggle.onValueChanged.RemoveListener((isOn) => m_presenter.OnValueChangedBGM(isOn));
        m_bgm_slider.onValueChanged.RemoveListener((value) => m_presenter.OnValueChangedBGMRate(value));
        
        m_sfx_toggle.onValueChanged.RemoveListener((isOn) => m_presenter.OnValueChangedSFX(isOn));
        m_sfx_slider.onValueChanged.RemoveListener((value) => m_presenter.OnValueChangedSFXRate(value));

        m_title_button.onClick.RemoveListener(m_presenter.OnClickedTitle);        
    }

    public void Inject(SettingPresenter presenter)
    {
        m_presenter = presenter;

        m_sensitivity_slider.onValueChanged.AddListener((value) => m_presenter.OnValueChangedMouseSensitivity(value));
        m_inversion_toggle.onValueChanged.AddListener((isOn) => m_presenter.OnValueChangedMouseInversion(isOn));
        m_shaking_toggle.onValueChanged.AddListener((isOn) => m_presenter.OnValueChangedCameraShaking(isOn)); 

        m_bgm_toggle.onValueChanged.AddListener((isOn) => m_presenter.OnValueChangedBGM(isOn));
        m_bgm_slider.onValueChanged.AddListener((value) => m_presenter.OnValueChangedBGMRate(value));
        
        m_sfx_toggle.onValueChanged.AddListener((isOn) => m_presenter.OnValueChangedSFX(isOn));
        m_sfx_slider.onValueChanged.AddListener((value) => m_presenter.OnValueChangedSFXRate(value));

        m_title_button.onClick.AddListener(m_presenter.OnClickedTitle);
    }

    public void Initialize(SettingData setting_data)
    {
        m_sensitivity_slider.value = setting_data.MouseSensitivity;
        m_inversion_toggle.isOn = setting_data.MouseInversion;
        m_shaking_toggle.isOn = setting_data.CameraShaking;

        m_bgm_toggle.isOn = setting_data.BGMPrint;
        m_bgm_slider.value = setting_data.BGMRate;
        m_sfx_toggle.isOn = setting_data.SFXPrint;
        m_sfx_slider.value = setting_data.SFXRate;
    }

    public void OpenUI()
    {
        m_animator.SetBool("Open", true);
    }

    public void CloseUI()
    {
        m_animator.SetBool("Open", false);
    }

    public void PlaySFX(string sfx_name)
    {
        SoundManager.Instance.PlaySFX(sfx_name, false, Vector3.zero);
    }

    public void ToggleBGMHandle(bool isOn)
    {
        m_bgm_slider.interactable = isOn;
    }

    public void ToggleSFXHandle(bool isOn)
    {
        m_sfx_slider.interactable = isOn;
    }

    public void PopupCloseUI()
    {
        if (m_ui_manager)
        {
            m_ui_manager.RemovePresenter(m_presenter);
        }
    }

    public void SetDepth()
    {
        (transform as RectTransform).SetAsFirstSibling();
    }
}
