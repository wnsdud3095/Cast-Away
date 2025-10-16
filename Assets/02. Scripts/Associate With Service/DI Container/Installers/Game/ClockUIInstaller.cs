using UnityEngine;

public class ClockUIInstaller : MonoBehaviour, IInstaller
{
    [Header("시계 뷰")]
    [SerializeField] private ClockView m_clock_view;

    [Header("시간 관리자")]
    [SerializeField] private TimeManager m_time_manager;

    public void Install()
    {
        InstallClock();
    }

    private void InstallClock()
    {
        DIContainer.Register<IClockView>(m_clock_view);

        var clock_presenter = new ClockPresenter(m_clock_view,
                                                 m_time_manager);
        DIContainer.Register<ClockPresenter>(clock_presenter);
    }
}
