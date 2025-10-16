using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ModulerIngredientSlotView : MonoBehaviour, IModulerIngredientSlotView
{
    [Header("UI 관련 컴포넌트")]
    [Header("아이템 이름")]
    [SerializeField] private TMP_Text m_item_name_label;

    [Header("아이템 이미지")]
    [SerializeField] private Image m_item_image;

    [Header("아이템 개수")]
    [SerializeField] private TMP_Text m_item_count_label; 

    private ModulerIngredientSlotPresenter m_presenter;

    public void OnDisable()
    {
        if(m_presenter == null)
        {
            return;
        }

        m_presenter.Dispose();
    }

    public void Inject(ModulerIngredientSlotPresenter presenter)
    {
        m_presenter = presenter;
    }

    public void UpdateUI(string item_name, Sprite item_image, int count, bool active)
    {
        m_item_name_label.text = active ? $"<color=white>{item_name}</color>" 
                                        : $"<color=red>{item_name}</color>";

        m_item_image.sprite = item_image;

        m_item_count_label.text = active ? $"<color=white>x {count}</color>" 
                                         : $"<color=red>x {count}</color>";
    }
}
