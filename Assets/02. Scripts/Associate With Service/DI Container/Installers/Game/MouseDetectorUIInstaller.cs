using UnityEngine;

public class MouseDetectorUIInstaller : MonoBehaviour, IInstaller
{
    [Header("마우스 감지자")]
    [SerializeField] private MouseDetectorView m_mouse_detector_view;

    public void Install()
    {
        DIContainer.Register<IMouseDetectorView>(m_mouse_detector_view);

        var mouse_detector_presenter = new MouseDetectorPresenter(m_mouse_detector_view);
        DIContainer.Register<MouseDetectorPresenter>(mouse_detector_presenter);
    }
}
