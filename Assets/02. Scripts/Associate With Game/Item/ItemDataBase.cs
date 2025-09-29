using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item DataBase", menuName = "SO/DB/Create Item DataBase")]
public class ItemDataBase : ScriptableObject, IItemDataBase
{
    [Header("아이템 목록")]
    [SerializeField] private Item[] m_item_list;

    private Dictionary<ItemCode, Item> m_item_dict;

#if UNITY_EDITOR
    private void OnEnable()
    {
        Initialize();
    }
#endif
    private void Initialize()
    {
        m_item_dict = new();

        // SO의 OnEnable()은 유니티 에디터에서 런타임이 아닌 환경에서도 작동하기 때문에
        // 반드시 리스트의 조건이 널이 아닌 경우에 작동하도록 설정.
        if (m_item_list == null)
        {
            return;
        }

        // 인스펙터를 통해 로드한 아이템 리스트를
        // 아이템 코드를 키로 하여 딕셔너리에 저장한다.
        foreach (var item in m_item_list)
        {
            m_item_dict.TryAdd(item.Code, item);
        }
    }

    public Item GetItem(ItemCode code)
    {
        if (m_item_dict == null)
        {
            Initialize();
        }

        return m_item_dict.TryGetValue(code, out var item) ? item : null;
    }
}