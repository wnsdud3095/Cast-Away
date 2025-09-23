using System.Collections.Generic;
using UnityEngine;

public class ShortcutSelectManager : MonoBehaviour
{
    public int SelectedIndex { get; private set; } = 0;
    public int m_shortcut_count = 5; // 1~5번 슬롯

    public event System.Action<int> OnSelectedChanged;

    private void Start()
    {
        Select(0);        // 게임 시작 시 1번 슬롯(인덱스 0) 선택
    }

    private void Update()
    {
        // 키보드 1~5번 선택
        for (int i = 0; i < m_shortcut_count; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                Select(i);
            }
        }
        // 마우스 휠 (위: y>0, 아래: y<0)
        if (Input.mouseScrollDelta.y != 0)
        {
            int delta = (Input.mouseScrollDelta.y > 0) ? -1 : 1;
            SelectedIndex = (SelectedIndex + delta + m_shortcut_count) % m_shortcut_count;
            Select(SelectedIndex);
        }

        // 좌클릭 아이템 사용
        if (Input.GetMouseButtonDown(0))
        {
            UseSelected();
        }
    }

    private void Select(int index)
    {
        SelectedIndex = index;
        OnSelectedChanged?.Invoke(index);
    }

    private void UseSelected()
    {
        var presenters = DIContainer.Resolve<List<ShortcutSlotPresenter>>();
        if (SelectedIndex >= 0 && SelectedIndex < presenters.Count)
        {
            presenters[SelectedIndex].UseShortcut();
        }
    }
}
