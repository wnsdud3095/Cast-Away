using UnityEngine;

[RequireComponent(typeof(AnimalCtrl))]
public class AnimalMouseDetector : MouseDetector
{
    private AnimalCtrl m_animal_ctrl;

    private void Awake()
    {
        m_animal_ctrl = GetComponent<AnimalCtrl>();
    }

    protected override void OnMouseEnter()
    {
        m_mouse_detector_presenter.OpenUI(m_animal_ctrl.SO.Name,
                                          m_animal_ctrl.Status.CurrentHP,
                                          m_animal_ctrl.Status.MaxHP);
    }
}