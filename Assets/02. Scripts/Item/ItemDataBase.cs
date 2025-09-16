using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item DataBase", menuName = "SO/DB/Create Item DataBase")]
public class ItemDataBase : ScriptableObject, IItemDataBase
{
    [Header("������ ���")]
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

        // SO�� OnEnable()�� ����Ƽ �����Ϳ��� ��Ÿ���� �ƴ� ȯ�濡���� �۵��ϱ� ������
        // �ݵ�� ����Ʈ�� ������ ���� �ƴ� ��쿡 �۵��ϵ��� ����.
        if (m_item_list == null)
        {
            return;
        }

        // �ν����͸� ���� �ε��� ������ ����Ʈ��
        // ������ �ڵ带 Ű�� �Ͽ� ��ųʸ��� �����Ѵ�.
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