using UnityEngine;

public class NoticeUIInstaller : MonoBehaviour, IInstaller
{
    [Header("알림 뷰")]
    [SerializeField] private NoticeView m_notice_view;

    public void Install()
    {
        InstallNotice();
    }

    private void InstallNotice()
    {
        DIContainer.Register<INoticeView>(m_notice_view);

        var notice_presenter = new NoticePresenter(m_notice_view);
        DIContainer.Register<NoticePresenter>(notice_presenter);
    }
}
