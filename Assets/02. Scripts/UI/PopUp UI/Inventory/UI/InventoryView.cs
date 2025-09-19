using TMPro;
using UnityEngine;
using UnityEngine.UI;

//[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CanvasGroup))]
public class InventoryView : MonoBehaviour, IInventoryView
{
    [Header("UI 관련 컴포넌트")]
    [Header("팝업 UI 매니저")]
    [SerializeField] private PopupUIManager m_ui_manager;

    [Header("닫기 버튼")]
    [SerializeField] private Button m_close_button;

    //private Animator m_animator;
    private CanvasGroup m_canvas_group;

    private InventoryPresenter m_presenter;


    private void Awake()
    {
        //m_animator = GetComponent<Animator>();
        m_canvas_group = GetComponent<CanvasGroup>();
    }

    // 객체가 파괴되는 시점에 프레젠터에서 연결된 돈 갱신 이벤트를 해제한다.
    private void OnDestroy()
    {
        //m_presenter.Dispose();
    }

    // 인벤토리 프레젠터를 Inject()에서 주입받는다.
    public void Inject(InventoryPresenter inventory_presenter)
    {
        m_presenter = inventory_presenter;

        m_close_button.onClick.AddListener(m_presenter.CloseUI);
        m_close_button.onClick.AddListener(PopupCloseUI);
    }

    public void OpenUI()
    {
        m_canvas_group.alpha = 1f;          
        m_canvas_group.interactable = true; 
        m_canvas_group.blocksRaycasts = true; 
        //m_animator.SetBool("Open", true);
    }

    public void CloseUI()
    {
        m_canvas_group.alpha = 0f;           
        m_canvas_group.interactable = false; 
        m_canvas_group.blocksRaycasts = false; 
        //m_animator.SetBool("Open", false);
    }


    public void SetDepth()
    {
        (transform as RectTransform).SetAsFirstSibling();
    }

    public void PopupCloseUI()
    {
        m_ui_manager.RemovePresenter(m_presenter);
    }
}