using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlotView : MonoBehaviour, IItemSlotView
{
    [Header("커서 데이터베이스")]
    [SerializeField] private CursorDataBase m_cursor_db;

    [Space(30f)]
    [Header("UI 관련 트랜스폼")]
    [Header("아이템 이미지")]
    [SerializeField] private Image m_item_image;

    [Header("아이템 개수")]
    [SerializeField] private TMP_Text m_count_label;

    [Header("슬롯 마스크")]
    [SerializeField] private ItemType m_slot_type;

    private ItemSlotPresenter m_presenter;

    private void Update() { }

    private void OnDestroy() { }

    public void Inject(ItemSlotPresenter presenter)
    {
        m_presenter = presenter;
    }

    // 아이템 슬롯을 비운다.
    // 1. 아이템 스프라이트 제거
    // 2. 아이템 개수 텍스트 초기화
    // 3. 쿨타임 이미지 초기화
    public void ClearUI()
    {
        m_item_image.sprite = null;
        SetAlpha(0f);

        m_count_label.text = string.Empty;
        m_count_label.gameObject.SetActive(false);
    }

    // 아이템 슬롯에 아이템을 추가한다.
    // 1. 아이템 스프라이트 추가
    // 2. 중첩 아이템이면 아이템 개수 텍스트를 활성화
    // 3. 쿨타임 이미지 초기화
    public void UpdateUI(Sprite item_image, bool stackable, int count)
    {
        m_item_image.sprite = item_image;
        SetAlpha(1f);

        if (stackable)
        {
            m_count_label.gameObject.SetActive(true);
            m_count_label.text = NumberFormatter.FormatNumber(count);
        }
        else
        {
            m_count_label.gameObject.SetActive(false);
        }

    }

    // 슬롯에 넣을 수 있는 아이템 타입인지 확인한다.
    public bool IsMask(ItemType type)
    {
        return ((int)m_slot_type & (int)type) != 0;
    }

    // 아이템 스프라이트를 특정한 알파값으로 설정한다.
    private void SetAlpha(float alpha)
    {
        var color = m_item_image.color;
        color.a = alpha;
        m_item_image.color = color;
    }

    public void SetCursor(CursorMode mode) { }

    public void OnPointerEnter(PointerEventData eventData) { }

    public void OnPointerExit(PointerEventData eventData) { }

    public void OnBeginDrag(PointerEventData eventData) { }

    public void OnDrag(PointerEventData eventData) { }

    public void OnEndDrag(PointerEventData eventData) { }

    public void OnDrop(PointerEventData eventData) { }

    public void OnPointerClick(PointerEventData eventData) { }
}