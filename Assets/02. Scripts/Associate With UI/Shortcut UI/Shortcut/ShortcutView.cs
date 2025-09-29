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

    private void Update()
    {
        // 키보드 입력 전달
        for (int i = 0; i < m_presenter.ShortcutCount; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                m_presenter.Select(i);
            }
        }

        // 마우스 휠 전달
        if (Input.mouseScrollDelta.y != 0)
        {
            int delta = (Input.mouseScrollDelta.y > 0) ? -1 : 1;
            m_presenter.ScrollSelect(delta);
        }

        // 좌클릭 전달
        if (Input.GetMouseButtonDown(0))
        {
            m_presenter.UseSelected();
        }
    }
}
