public class ClockPresenter
{
    private readonly IClockView m_view;
    private readonly TimeManager m_time_manager;

    public ClockPresenter(IClockView view,
                          TimeManager time_manager)
    {
        m_view = view;
        m_time_manager = time_manager;

        m_view.Inject(this);
    }

    public void UpdateClock()
    {
        var current_time = m_time_manager.CurrentTime;

        var hour = (current_time.Hour % 12 + current_time.Minute / 60f) * 30f;
        var min = (current_time.Minute + current_time.Second / 60f) * 6f;
        var sec = current_time.Second * 6f;        

        m_view.UpdateUI(hour, min, sec);
    }
}
