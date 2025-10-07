using System;
using UnityEngine;

public class ItemSwapper : MonoBehaviour
{
    [Header("스왑 데이터 목록")]
    [SerializeField] private SwapData[] m_swap_list;

    private BaseTool m_current_tool;
    private ShortcutPresenter m_shortcut_presenter;

    public static Action OnLeftClickDown;
    public static Action OnLeftClickHold;

    public static Action OnRightClickDown;

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
            
            if(active)
            {
                m_current_tool = swap_data.Item.GetComponent<BaseTool>();
            }
        }
    }

    public void TriggerEnter()
    {
        m_current_tool?.TriggerEnter();
    }

    public void EnableHit()
    {
        m_current_tool?.EnableHit();
    }

    public void DisableHit()
    {
        m_current_tool?.DisableHit();
    }

    public void TriggerExit()
    {
        m_current_tool?.TriggerExit();
    }
}