using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Animator))]
public class KeyBinderView : MonoBehaviour, IKeyBinderView
{
    [Header("�˾� UI �Ŵ���")]
    [SerializeField] private PopupUIManager m_ui_manager;

    private Animator m_animator;
    private KeyBinderPresenter m_presenter;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
    }

    // Inject()�� ���ؼ� �������͸� ���Թ޴´�.
    public void Inject(KeyBinderPresenter presenter)
    {
        m_presenter = presenter;
    }

    public void OpenUI()
    {
        m_animator.SetBool("Open", true);
    }

    public void CloseUI()
    {
        m_animator.SetBool("Open", false);
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
