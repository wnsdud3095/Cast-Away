using SettingService;
using UnityEngine;

public class SettingUIInstaller : MonoBehaviour, IInstaller
{
    [Header("설정 뷰")]
    [SerializeField] private SettingView m_setting_view;

    [Header("카메라 컨트롤러")]
    [SerializeField] private CameraCtrl m_camera_ctrl;

    [Header("카메라 셰이커")]
    [SerializeField] private CameraShaker m_camera_shaker;

    public void Install()
    {
        InstallSetting();
        InstallCamera();
    }

    private void InstallSetting()
    {
        DIContainer.Register<ISettingView>(m_setting_view);

        var setting_presetner = new SettingPresenter(m_setting_view,
                                                     ServiceLocator.Get<ISettingService>());
        DIContainer.Register<SettingPresenter>(setting_presetner);
    }

    private void InstallCamera()
    {
        m_camera_ctrl.Inject(ServiceLocator.Get<ISettingService>());
        m_camera_shaker.Inject(ServiceLocator.Get<ISettingService>());
    }
}
