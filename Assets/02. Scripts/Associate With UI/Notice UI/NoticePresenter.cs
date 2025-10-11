public class NoticePresenter
{
    private readonly INoticeView m_view;
    private bool m_is_active;

    public bool Active
    {
        get => m_is_active;
        set => m_is_active = value;
    }

    public bool Blocked { get; set; }

    public NoticePresenter(INoticeView view)
    {
        m_view = view;
        m_view.Inject(this);
    }

    public void OpenUI(string notice_text)
    {
        if(!Blocked && !m_is_active)
        {
            m_is_active = true;
            m_view.OpenUI();
            m_view.UpdateUI(notice_text);
        }
    }

    public void CloseUI()
    {
        if(!Blocked && m_is_active)
        {
            m_is_active = false;
            m_view.CloseUI();
        }
    }

    public void PushUI(string notice_text)
    {
        if(!Blocked && !m_is_active)
        {
            m_view.PushUI();
            m_view.UpdateUI(notice_text);
        }
    }
}
