using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftSlotView : MonoBehaviour, ICraftSlotView
{
    [Header("UI 관련 컴포넌트")]
    [Header("제작 아이템의 이름")]
    [SerializeField] private TMP_Text m_name_label;

    [Header("제작 아이템의 이미지")]
    [SerializeField] private Image m_item_image;

    [Header("자세히 버튼")]
    [SerializeField] private Button m_info_button;

    [Header("잠금 이미지")]
    [SerializeField] private GameObject m_unlock_image;

    [Header("잠금 텍스트")]
    [SerializeField] private TMP_Text m_unlock_text;

    private CraftSlotPresenter m_presenter;

    private void OnDisable()
    {
        if (m_presenter == null)
        {
            return;
        }

        m_info_button.onClick.RemoveListener(m_presenter.OnClickedInfo);
        m_presenter.Dispose();
    }

    public void Inject(CraftSlotPresenter presenter)
    {
        m_presenter = presenter;

        m_info_button.onClick.AddListener(m_presenter.OnClickedInfo);
    }

    public void InitUI(string craft_name, Sprite craft_image)
    {
        m_name_label.text = craft_name;

        m_item_image.sprite = craft_image;
        SetColor(1f);
    }

    public void UpdateUI(bool unlock, int level = 0)
    {
        m_unlock_image.SetActive(!unlock);
        m_unlock_text.text = $"해금: 제작 레벨 {level} 이상";
    }

    private void SetColor(float alpha)
    {
        var color = m_item_image.color;
        color.a = alpha;
        m_item_image.color = color;
    }
}
