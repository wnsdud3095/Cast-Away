using UnityEngine;

public class KeyBinderPresenter : IPopupPresenter
{
    private readonly IKeyBinderView m_view;

    // 생성자를 통해 View를 주입받는다.
    public KeyBinderPresenter(IKeyBinderView view)
    {
        m_view = view;
        m_view.Inject(this);
    }

    public void OpenUI()
    {
        m_view.OpenUI();
    }

    public void CloseUI()
    {
        m_view.CloseUI();
    }

    // View의 깊이를 설정한다.
    public void SortDepth()
    {
        m_view.SetDepth();
    }
}
