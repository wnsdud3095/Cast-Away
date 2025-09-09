using EXPService;
using UnityEngine;
using UserService;

public class StatusUIInstaller : MonoBehaviour
{
    [Header("플레이어 상태 컴포넌트")]
    [SerializeField] private PlayerStatus m_player_status;

    [Header("스테이터스 UI 뷰")]
    [SerializeField] private StatusView m_status_view;

    public void Install()
    {
        DIContainer.Register<PlayerStatus>(m_player_status);
        DIContainer.Register<IStatusView>(m_status_view);

        var status_model = new StatusModel(m_player_status);
        DIContainer.Register<StatusModel>(status_model);

        var status_presenter = new StatusPresenter(status_model,
                                                   m_status_view,                                                  
                                                   ServiceLocator.Get<IUserService>(),
                                                   ServiceLocator.Get<IEXPService>()
                                                   );
        DIContainer.Register<StatusPresenter>(status_presenter);
    }
}