using UnityEngine;
using UnityEngine.UI;

public class ClockView : MonoBehaviour, IClockView
{
    [Header("UI 관련 컴포넌트")]
    [Header("시침")]
    [SerializeField] private Image m_hour_hand;

    [Header("분침")]
    [SerializeField] private Image m_minute_hand;

    [Header("초침")]
    [SerializeField] private Image m_second_hand;

    private ClockPresenter m_presenter;

    private void Update()
    {
        m_presenter.UpdateClock();
    }

    public void Inject(ClockPresenter presenter)
    {
        m_presenter = presenter;
    }

    public void UpdateUI(float hour, float min, float sec)
    {
        if(m_hour_hand != null)
        {
            m_hour_hand.transform.localRotation = Quaternion.Euler(0f, 0f, -hour);
        }

        if(m_minute_hand != null)
        {
            m_minute_hand.transform.localRotation = Quaternion.Euler(0f, 0f, -min);
        }

        if(m_second_hand != null)
        {
            m_second_hand.transform.localRotation = Quaternion.Euler(0f, 0f, -sec);
        }
    }
}
