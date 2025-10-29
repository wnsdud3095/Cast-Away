using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CompactCraftView : MonoBehaviour, ICompactCraftView
{
    [Header("UI 관련 컴포넌트")]
    [Header("팝업 UI 매니저")]
    [SerializeField] private PopupUIManager m_ui_manager;

    [Header("캔버스 그룹")]
    [SerializeField] private CanvasGroup m_canvas_group;

    [Header("슬롯의 부모 트랜스폼")]
    [SerializeField] private Transform m_slot_root;

    [Header("제작 아이템의 이름")]
    [SerializeField] private TMP_Text m_item_name_label;

    [Header("제작 아이템의 이미지")]
    [SerializeField] private Image m_item_image;

    [Header("제작 버튼")]
    [SerializeField] private Button m_craft_button;

    private CompactCraftPresenter m_presenter;
    private List<GameObject> m_slot_list;

    private void Awake()
    {
        m_slot_list = new();
    }

    private void OnDisable()
    {
        m_craft_button.onClick.RemoveListener(m_presenter.OnClickedCraft);
    }

    public void Inject(CompactCraftPresenter presenter)
    {
        m_presenter = presenter;

        m_craft_button.onClick.AddListener(m_presenter.OnClickedCraft);
    }

    public void OpenUI(string item_name, Sprite item_image)
    {
        m_slot_list.Clear();

        m_canvas_group.alpha = 1f;
        m_canvas_group.blocksRaycasts = true;
        m_canvas_group.interactable = true;

        m_item_name_label.text = item_name;
        m_item_image.sprite = item_image;
    }

    public void UpdateUI(bool active)
    {
        m_craft_button.interactable = active;
    }

    public void CloseUI()
    {
        m_canvas_group.alpha = 0f;
        m_canvas_group.blocksRaycasts = false;
        m_canvas_group.interactable = false;

        Return();
        m_slot_list.Clear();
    }

    public ICraftIngredientSlotView InstantiateSlotView()
    {
        var slot_obj = ObjectManager.Instance.GetObject(ObjectType.CRAFT_INGREDIENT_SLOT);
        slot_obj.transform.SetParent(m_slot_root, false);
        m_slot_list.Add(slot_obj);

        return slot_obj.GetComponent<ICraftIngredientSlotView>();
    }

    private void Return()
    {
        var container = ObjectManager.Instance.GetPool(ObjectType.CRAFT_INGREDIENT_SLOT).Container;

        foreach (var slot_obj in m_slot_list)
        {
            slot_obj.transform.SetParent(container, false);

            ObjectManager.Instance.ReturnObject(slot_obj, ObjectType.CRAFT_INGREDIENT_SLOT);
        }
    }

    public void PlaySFX(string sfx_name)
    {
        SoundManager.Instance.PlaySFX(sfx_name, false, Vector3.zero);
    }
}
