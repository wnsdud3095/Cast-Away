using UnityEngine;

public class ModulerTutorialView : MonoBehaviour, IModulerTutorialView
{
    [Header("UI 관련 컴포넌트")]
    [Header("팝업 UI 관리자")]
    [SerializeField] private PopupUIManager m_ui_manager;

    [Header("캔버스 그룹")]
    [SerializeField] private CanvasGroup m_canvas_group;

    private ModulerTutorialPresenter m_presenter;

    private void Update()
    {
        if(m_presenter.Active && Input.GetKeyDown(KeyCode.C))
        {
            m_presenter.CloseUI();
            GameEventBus.Publish(GameEventType.INPLAY);
        }
    }

    public void Inject(ModulerTutorialPresenter presenter)
    {
        m_presenter = presenter;
    }

    public void OpenUI()
    {
        m_ui_manager.CloseAllUI();

        m_canvas_group.alpha = 1f;
        m_canvas_group.blocksRaycasts = true;
        m_canvas_group.interactable = true;
    }

    public void CloseUI()
    {
        m_canvas_group.alpha = 0f;
        m_canvas_group.blocksRaycasts = false;
        m_canvas_group.interactable = false;
    }
}