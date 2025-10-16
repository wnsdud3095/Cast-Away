public interface IClockView
{
    void Inject(ClockPresenter presenter);
    void UpdateUI(float hour, float min, float sec);
}