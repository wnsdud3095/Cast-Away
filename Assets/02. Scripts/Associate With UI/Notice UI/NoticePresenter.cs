public class NoticePresenter
{
    private readonly INoticeView m_view;
    private bool m_is_active;

    public NoticePresenter(INoticeView view)
    {
        m_view = view;
    }

    public void OpenUI(string notice_text)
    {
        if(!m_is_active)
        {
            m_is_active = true;
            m_view.OpenUI();
            m_view.UpdateUI(notice_text);
        }
    }

    public void CloseUI()
    {
        if(m_is_active)
        {
            m_is_active = false;
            m_view.CloseUI();
        }
    }
}
