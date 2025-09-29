using UnityEngine;

public class ShortcutView : MonoBehaviour, IShortcutView
{
    [Header("팝업 UI 매니저")]
    [SerializeField] private PopupUIManager m_ui_manager;

    private ShortcutPresenter m_presenter;

    public void Inject(ShortcutPresenter presenter)
    {
        m_presenter = presenter;
    }
}
