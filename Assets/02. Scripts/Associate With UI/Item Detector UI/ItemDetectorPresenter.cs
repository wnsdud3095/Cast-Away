public class ItemDetectorPresenter
{
    private readonly IItemDetectorView m_view;
    private bool m_is_active;

    public bool Active => m_is_active;

    public ItemDetectorPresenter(IItemDetectorView view)
    {
        m_view = view;
        
        m_view.Inject(this);
    }

    public void OpenUI(string item_name, System.Numerics.Vector3 position)
    {
        if(!m_is_active)
        {
            m_view.OpenUI();
            m_view.UpdateUI(item_name, position);
            m_is_active = true;
        }
    }

    public void CloseUI()
    {
        m_is_active = false;
        m_view.CloseUI();
    }
}
