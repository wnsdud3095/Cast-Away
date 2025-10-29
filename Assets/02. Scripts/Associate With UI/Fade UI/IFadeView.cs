public interface IFadeView
{
    void Inject(FadePresenter presenter);
    void Fade(bool is_in);
}