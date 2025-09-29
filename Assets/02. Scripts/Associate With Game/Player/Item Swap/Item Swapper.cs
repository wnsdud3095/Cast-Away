using UnityEngine;

public class ItemSwapper : MonoBehaviour
{
    [Header("스왑 데이터 목록")]
    [SerializeField] private SwapData[] m_swap_list;

    private ShortcutPresenter m_shortcut_presenter;

    private void OnDestroy()
    {
        m_shortcut_presenter.OnSelectedChangedToCode -= Swap;
    }

    public void Inject(ShortcutPresenter shortcut_presenter)
    {
        m_shortcut_presenter = shortcut_presenter;

        m_shortcut_presenter.OnSelectedChangedToCode += Swap;
    }

    private void Swap(ItemCode item_code)
    {
        foreach(var swap_data in m_swap_list)
        {
            var active = swap_data.Code == item_code;
            swap_data.Item.SetActive(active);
        }
    }
}