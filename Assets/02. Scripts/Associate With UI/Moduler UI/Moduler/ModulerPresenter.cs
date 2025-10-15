public class ModulerPresenter: IPopupPresenter
{
    private readonly IModulerView m_view;

    public ModulerPresenter(IModulerView view)
    {
        m_view = view;

        m_view.Inject(this);
    }

    public void OpenUI()
    {
        m_view.PlaySFX("UI Open");
        m_view.OpenUI();
    }

    public void CloseUI()
    {
        m_view.PlaySFX("UI Close");
        m_view.CloseUI();
    }

    public void SortDepth()
    {
        m_view.SetDepth();
    }
}
