using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CanvasGroup))]
public class KeyBinderView : MonoBehaviour, IKeyBinderView
{
    [Header("�˾� UI �Ŵ���")]
    [SerializeField] private PopupUIManager m_ui_manager;

    [Header("UI �ݱ� ��ư")]
    [SerializeField] private Button m_close_button;

    private Animator m_animator;
    private KeyBinderPresenter m_presenter;
    private CanvasGroup m_canvas_group; // CanvasGroup ����

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
        m_canvas_group = GetComponent<CanvasGroup>();
        CloseUI();
    }

    // Inject()�� ���ؼ� �������͸� ���Թ޴´�.
    public void Inject(KeyBinderPresenter presenter)
    {
        m_presenter = presenter;

        // �ݱ� ��ư�� PopupUIManager�� ���¸� ����ȭ��ų �̺�Ʈ���� ����Ѵ�.
        m_close_button.onClick.AddListener(m_presenter.CloseUI);
        m_close_button.onClick.AddListener(PopupCloseUI);
    }

    public void OpenUI()
    {
        m_canvas_group.alpha = 1f;          // ������ ���̰�
        m_canvas_group.interactable = true; // Ŭ�� ����
        m_canvas_group.blocksRaycasts = true; // �Է� ���� ���
        //m_animator.SetBool("Open", true);
    }

    public void CloseUI()
    {
        m_canvas_group.alpha = 0f;           // ������ �ʰ�
        m_canvas_group.interactable = false; // Ŭ�� �Ұ�
        m_canvas_group.blocksRaycasts = false; // �Է� ���� ����
        //m_animator.SetBool("Open", false);
    }

    // UI�� ���̸� �����Ѵ�. �󸶸�ŭ �������� �󸶸�ŭ ��������� �����Ѵ�.
    public void SetDepth()
    {
        (transform as RectTransform).SetAsFirstSibling();
    }

    // �������͸� ��ũ�� ����Ʈ���� ������ �����ϴ� ������ �Ѵ�.
    public void PopupCloseUI()
    {
        m_ui_manager.RemovePresenter(m_presenter);
    }
}
