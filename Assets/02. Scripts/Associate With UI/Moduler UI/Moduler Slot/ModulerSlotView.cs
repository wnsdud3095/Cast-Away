using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ModulerSlotView : MonoBehaviour, IModulerSlotView
{
    [Header("UI 관련 컴포넌트")]
    [Header("모듈의 이름")]
    [SerializeField] private TMP_Text m_name_label;

    [Header("모듈의 이미지")]
    [SerializeField] private Image m_module_image;

    [Header("자세히 버튼")]
    [SerializeField] private Button m_info_button;

    [Header("잠금 이미지")]
    [SerializeField] private GameObject m_unlock_image;

    [Header("잠금 텍스트")]
    [SerializeField] private TMP_Text m_unlock_text;

    private ModulerSlotPresenter m_presenter;

    private void OnDisable()
    {
        if(m_presenter == null)
        {
            return;
        }

        m_info_button.onClick.RemoveListener(m_presenter.OnClickedInfo);
        m_presenter.Dispose();
    }

    public void Inject(ModulerSlotPresenter presenter)
    {
        m_presenter = presenter;

        m_info_button.onClick.AddListener(m_presenter.OnClickedInfo);
    }

    public void InitUI(string module_name, Sprite module_image)
    {
        m_name_label.text = module_name;

        m_module_image.sprite = module_image;
        SetColor(1f);
    }

    public void UpdateUI(bool unlock, int level = 0)
    {
        m_unlock_image.SetActive(!unlock);
        m_unlock_text.text = $"해금: 제작 레벨 {level} 이상";
    }

    private void SetColor(float alpha)
    {
        var color = m_module_image.color;
        color.a = alpha;
        m_module_image.color = color;
    }
}
