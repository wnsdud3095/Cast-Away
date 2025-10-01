using KeyService;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShortcutSlotView : MonoBehaviour, IShortcutSlotView
{
    [Header("하이라이트 이미지")]
    [SerializeField] private Image m_highlightImage;

    private ShortcutSlotPresenter m_presenter;

    private void OnDestroy()
    {
        m_presenter.OnSelectedChanged -= HandleSelectedChanged;
        m_presenter.Dispose();
    }

    [System.Obsolete]
    public void Inject(ShortcutSlotPresenter presenter)
    {
        m_presenter = presenter;
        m_highlightImage.enabled = false;

        // 선택 매니저 이벤트 구독
        m_presenter.OnSelectedChanged += HandleSelectedChanged;
    }
    private void HandleSelectedChanged(int index)
    {
        // 내 offset이 선택된 인덱스와 같으면 하이라이트 아니면 자동으로 취소
        m_highlightImage.enabled = (m_presenter.Offset == index);
    }
    public void UpdateUI(KeyCode code, string name)
    {

    }
}
