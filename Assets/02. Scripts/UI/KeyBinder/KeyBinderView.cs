using UnityEngine;
using UnityEngine.UI;


//[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CanvasGroup))]
public class KeyBinderView : MonoBehaviour, IKeyBinderView
{
    [Header("팝업 UI 매니저")]
    [SerializeField] private PopupUIManager m_ui_manager;

    //private Animator m_animator;
    private KeyBinderPresenter m_presenter;
    private CanvasGroup m_canvas_group; // CanvasGroup 참조

    private void Awake()
    {
        //m_animator = GetComponent<Animator>();
        m_canvas_group = GetComponent<CanvasGroup>();
        CloseUI();
    }

    // Inject()를 통해서 프레젠터를 주입받는다.
    public void Inject(KeyBinderPresenter presenter)
    {
        m_presenter = presenter;
    }

    public void OpenUI()
    {
        m_canvas_group.alpha = 1f;          // 완전히 보이게
        m_canvas_group.interactable = true; // 클릭 가능
        m_canvas_group.blocksRaycasts = true; // 입력 차단 허용
        //m_animator.SetBool("Open", true);
    }

    public void CloseUI()
    {
        m_canvas_group.alpha = 0f;           // 보이지 않게
        m_canvas_group.interactable = false; // 클릭 불가
        m_canvas_group.blocksRaycasts = false; // 입력 차단 안함
        //m_animator.SetBool("Open", false);
    }

    // UI의 깊이를 설정한다. 얼마만큼 가려지고 얼마만큼 노출될지를 결정한다.
    public void SetDepth()
    {
        (transform as RectTransform).SetAsFirstSibling();
    }

    // 프레젠터를 링크드 리스트에서 강제로 제거하는 역할을 한다.
    public void PopupCloseUI()
    {
        m_ui_manager.RemovePresenter(m_presenter);
    }
}
