using System.Collections.Generic;
using UnityEngine;

public class ModulerView : MonoBehaviour, IModulerView
{
    [Header("UI 관련 컴포넌트")]
    [Header("팝업 UI 매니저")]
    [SerializeField] private PopupUIManager m_ui_manager;

    [Header("캔버스 그룹")]
    [SerializeField] private CanvasGroup m_canvas_group;

    [Header("슬롯의 부모 트랜스폼")]
    [SerializeField] private Transform m_slot_root;

    private List<GameObject> m_slot_list;
    private ModulerPresenter m_presenter;

    private void Awake()
    {
        m_slot_list = new();
    }

    public void Inject(ModulerPresenter presenter)
    {
        m_presenter = presenter;
    }

    public IModulerSlotView InstantiateSlotView()
    {
        var slot_obj = ObjectManager.Instance.GetObject(ObjectType.MODULER_SLOT);
        slot_obj.transform.SetParent(m_slot_root, false);
        m_slot_list.Add(slot_obj);

        return slot_obj.GetComponent<IModulerSlotView>();
    }

    public void OpenUI()
    {
        m_slot_list.Clear();

        m_canvas_group.alpha = 1f;
        m_canvas_group.blocksRaycasts = true;
        m_canvas_group.interactable = true;
    }

    public void CloseUI()
    {
        m_canvas_group.alpha = 0f;
        m_canvas_group.blocksRaycasts = false;
        m_canvas_group.interactable = false;

        m_slot_list.Clear();
    }

    public void SetDepth()
    {
        (transform as RectTransform).SetAsFirstSibling();
    }

    public void PopupCloseUI()
    {
        m_ui_manager.RemovePresenter(m_presenter);
    }

    public void PlaySFX(string sfx_name)
    {
        SoundManager.Instance.PlaySFX(sfx_name, false, Vector3.zero);
    }
}
