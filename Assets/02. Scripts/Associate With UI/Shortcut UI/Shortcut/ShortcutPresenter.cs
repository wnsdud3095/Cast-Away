public class ShortcutPresenter
{
    private readonly IShortcutView m_view;

    public ShortcutPresenter(IShortcutView view, InventoryPresenter inventory_presenter)
    {
        m_view = view;
        m_view.Inject(this);
    }
}
