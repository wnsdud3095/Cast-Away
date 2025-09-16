using UnityEngine;

public class KeyBinderPresenter : IPopupPresenter
{
    private readonly IKeyBinderView m_view;

    // �����ڸ� ���� View�� ���Թ޴´�.
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

    // View�� ���̸� �����Ѵ�.
    public void SortDepth()
    {
        m_view.SetDepth();
    }
}
