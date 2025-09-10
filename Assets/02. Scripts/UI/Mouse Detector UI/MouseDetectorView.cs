using TMPro;
using UnityEngine;

public class MouseDetectorView : MonoBehaviour, IMouseDetectorView
{
    [Header("마우스 감지 패널")]
    [SerializeField] private GameObject m_panel_object;

    [Header("감지 텍스트")]
    [SerializeField] private TMP_Text m_detected_label;

    public void OpenUI(string text)
    {
        m_panel_object.SetActive(true);

        m_detected_label.text = text;
    }

    public void CloseUI()
    {
        m_panel_object.SetActive(false);
    }
}
