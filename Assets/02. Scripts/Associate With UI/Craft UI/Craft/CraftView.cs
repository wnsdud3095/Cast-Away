using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftView : MonoBehaviour, ICraftView
{
    [Header("UI 관련 컴포넌트")]
    [Header("팝업 UI 매니저")]
    [SerializeField] private PopupUIManager m_ui_manager;

    [Header("캔버스 그룹")]
    [SerializeField] private CanvasGroup m_canvas_group;

    [Header("슬롯의 부모 트랜스폼")]
    [SerializeField] private Transform m_slot_root;

    [Header("스크롤바")]
    [SerializeField] private Scrollbar m_scroll_bar;

    [Header("아이템 분류 토글")]
    [SerializeField] private Toggle[] m_toggles;

    [Header("아이템 분류 타입")]
    [SerializeField] private ItemType[] m_filter_types;

    private CraftPresenter m_presenter;

    public void Inject(CraftPresenter presenter)
    {
        m_presenter = presenter;

        if (m_toggles.Length != m_filter_types.Length)
            Debug.LogError("Filter 토글 개수와 타입 배열 길이가 다릅니다!");

        for (int i = 0; i < m_toggles.Length; i++)
        {
            var index = i; 
            m_toggles[i].onValueChanged.AddListener(isOn =>
            {
                if (isOn)
                    m_presenter.ChangeFilter(m_filter_types[index]);
            });
        }
    }

    public ICraftSlotView InstantiateSlotView()
    {
        var slot_obj = ObjectManager.Instance.GetObject(ObjectType.CRAFT_SLOT);
        slot_obj.transform.SetParent(m_slot_root, false);

        return slot_obj.GetComponent<ICraftSlotView>();
    }


    public void ClearSlots()
    {
        Return();
    }

    public void OpenUI()
    {
        m_canvas_group.alpha = 1f;
        m_canvas_group.blocksRaycasts = true;
        m_canvas_group.interactable = true;
    }

    public void CloseUI()
    {
        m_canvas_group.alpha = 0f;
        m_canvas_group.blocksRaycasts = false;
        m_canvas_group.interactable = false;

        m_scroll_bar.value = 0f;

        Return();
    }

    public void Return()
    {
        var container = ObjectManager.Instance.GetPool(ObjectType.CRAFT_SLOT).Container;

        for (int i = m_slot_root.childCount - 1; i >= 0; i--)
        {
            var child = m_slot_root.GetChild(i).gameObject;

            ObjectManager.Instance.ReturnObject(child, ObjectType.CRAFT_SLOT);

            child.transform.SetParent(container, false);
        }

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
