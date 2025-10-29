public class Bed : RealviewObject
{
    private TimeSettings m_time_settings;
    private TimeManager m_time_manager;
    private FadePresenter m_fade_presenter;

    public void Interaction(TimeSettings time_settings,
                            TimeManager time_manager,
                            FadePresenter fade_presenter)
    {
        m_time_settings = time_settings;
        m_time_manager = time_manager;
        m_fade_presenter = fade_presenter;

        Initialize();

        fade_presenter.Fade(true);
        time_settings.Multiplier = 10000;
        GameEventBus.Publish(GameEventType.INTERACTING);
    }

    private void Initialize()
    {
        m_time_manager.OnSunrise += FadeOut;
        m_time_manager.OnSunrise += TimeReset;
        m_time_manager.OnSunrise += ChangeState;

        m_fade_presenter.OnFadeEnd += Reset;
    }

    private void Reset()
    {
        m_fade_presenter.OnFadeEnd -= Reset;

        m_time_manager.OnSunrise -= FadeOut;
        m_time_manager.OnSunrise -= TimeReset;
        m_time_manager.OnSunrise -= ChangeState;
    }

    private void FadeOut()
    {
        m_fade_presenter.Fade(false);
    }

    private void TimeReset()
    {
        m_time_settings.Multiplier = 75;
    }

    private void ChangeState()
    {
        GameEventBus.Publish(GameEventType.INPLAY);
    }
}
