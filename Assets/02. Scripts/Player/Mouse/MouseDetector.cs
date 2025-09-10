using UnityEngine;

public abstract class MouseDetector : MonoBehaviour
{
    protected MouseDetectorPresenter m_mouse_detector_presenter;

    public void Inject(MouseDetectorPresenter mouse_detector_presenter)
    {
        m_mouse_detector_presenter = mouse_detector_presenter;
    }

    protected abstract void OnMouseEnter();

    protected virtual void OnMouseExit()
    {
        m_mouse_detector_presenter.CloseUI();
    }
}