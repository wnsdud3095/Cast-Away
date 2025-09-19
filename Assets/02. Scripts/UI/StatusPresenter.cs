using System;
using UserService;
using EXPService;

public class StatusPresenter : IDisposable
{
    private readonly StatusModel m_model;
    private readonly IStatusView m_view;
    private readonly IUserService m_user_service;
    private readonly IEXPService m_exp_service;

    public StatusPresenter(StatusModel model, IStatusView view, IUserService user_service, IEXPService exp_service)
    {
        m_model = model;
        m_view = view;
        m_user_service = user_service;
        m_exp_service = exp_service;

        // View에 Presenter 주입
        m_view.Inject(this);

        // Model 이벤트 구독
        m_model.OnUpdatedHP = OnUpdatedHP;
        m_model.OnUpdatedThirst = OnUpdatedThirst;
        m_model.OnUpdatedHunger = OnUpdatedHunger;

        // UserService 이벤트 구독
        m_user_service.OnUpdatedLevel += OnUpdatedLevel;

        // 초기 상태 반영
        m_model.Initialize();
        m_user_service.InitLevel();
    }

    private void OnUpdatedHP(float current, float max)
    {
        m_view.UpdateHP(current / max);
    }

    private void OnUpdatedThirst(float current, float max)
    {
        m_view.UpdateThirst(current / max);
    }

    private void OnUpdatedHunger(float current, float max)
    {
        m_view.UpdateHunger(current / max);
    }

    private void OnUpdatedLevel(int level, int exp)
    {
        int maxExp = m_exp_service.GetEXP(level);
        float expRate = maxExp > 0 ? (float)exp / maxExp : 0f;
        m_view.UpdateLV(level, expRate);
    }

    public void Dispose()
    {
        m_model.OnUpdatedHP = null;
        m_model.OnUpdatedThirst = null;
        m_model.OnUpdatedHunger = null;
        m_user_service.OnUpdatedLevel -= OnUpdatedLevel;
    }
}
