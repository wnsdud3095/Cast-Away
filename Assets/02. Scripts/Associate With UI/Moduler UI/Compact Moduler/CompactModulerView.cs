using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CompactModulerView : MonoBehaviour, ICompactModulerView
{
    [Header("UI 관련 컴포넌트")]
    [Header("팝업 UI 매니저")]
    [SerializeField] private PopupUIManager m_ui_manager;

    [Header("캔버스 그룹")]
    [SerializeField] private CanvasGroup m_canvas_group;

    [Header("슬롯의 부모 트랜스폼")]
    [SerializeField] private Transform m_slot_root;

    [Header("모듈의 이름")]
    [SerializeField] private TMP_Text m_module_name_label;

    [Header("모듈의 이미지")]
    [SerializeField] private Image m_module_image;

    [Header("제작 버튼")]
    [SerializeField] private Button m_build_button;

    private CompactModulerPresenter m_presenter;
    private List<GameObject> m_slot_list;

    private void Awake()
    {
        m_slot_list = new();
    }

    private void OnDisable()
    {
        m_build_button.onClick.RemoveListener(m_presenter.OnClickedBuild);
    }

    public void Inject(CompactModulerPresenter presenter)
    {
        m_presenter = presenter;

        m_build_button.onClick.AddListener(m_presenter.OnClickedBuild);
    }

    public void OpenUI(string item_name, Sprite item_image)
    {
        m_slot_list.Clear();

        m_canvas_group.alpha = 1f;
        m_canvas_group.blocksRaycasts = true;
        m_canvas_group.interactable = true;

        m_module_name_label.text = item_name;
        m_module_image.sprite = item_image;
    }

    public void UpdateUI(bool active)
    {
        m_build_button.interactable = active;
    }

    public void CloseUI()
    {
        m_canvas_group.alpha = 0f;
        m_canvas_group.blocksRaycasts = false;
        m_canvas_group.interactable = false;

        Return();
        m_slot_list.Clear();
    }

    public IModulerIngredientSlotView InstantiateSlotView()
    {
        var slot_obj = ObjectManager.Instance.GetObject(ObjectType.MOUDLE_INGREDIENT_SLOT);
        slot_obj.transform.SetParent(m_slot_root, false);
        m_slot_list.Add(slot_obj);

        return slot_obj.GetComponent<IModulerIngredientSlotView>();
    }

    private void Return()
    {
        var container = ObjectManager.Instance.GetPool(ObjectType.MOUDLE_INGREDIENT_SLOT).Container;

        foreach(var slot_obj in m_slot_list)
        {    
            slot_obj.transform.SetParent(container, false);

            ObjectManager.Instance.ReturnObject(slot_obj, ObjectType.MOUDLE_INGREDIENT_SLOT);
        }
    }
}
