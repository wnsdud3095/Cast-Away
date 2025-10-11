public interface INoticeView
{
    void Inject(NoticePresenter presenter);
    
    void OpenUI();
    void UpdateUI(string notice_text);
    void CloseUI();
    void PushUI();
}