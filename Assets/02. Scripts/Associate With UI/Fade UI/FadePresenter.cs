using System;

public class FadePresenter
{
    private readonly IFadeView m_view;

    public event Action OnFadeEnd;

    public FadePresenter(IFadeView view)
    {
        m_view = view;
        m_view.Inject(this);
    }

    public void Fade(bool is_in)
    {
        m_view.Fade(is_in);
    }

    public void Alert()
    {
        OnFadeEnd?.Invoke();
    }
}
