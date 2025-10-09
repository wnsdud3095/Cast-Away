using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item-Object Converter", menuName = "SO/DB/Item-Object Converter")]
public class ItemObjectConverter : ScriptableObject, IItemObjectConverter
{
    [SerializeField] private ConvertData[] m_data_list;
    private Dictionary<ItemCode, ObjectType> m_data_dict;

#if UNITY_EDITOR
    private void OnEnable()
    {
        if(m_data_list == null)
        {
            return;
        }

        Initialize();
    }
#endif 

    private void Initialize()
    {
        m_data_dict = new();

        foreach(var convert_data in m_data_list)
        {
            m_data_dict.TryAdd(convert_data.ItemCode, convert_data.ObjectType);
        }
    }

    public ObjectType GetObjectType(ItemCode item_code)
    {
        return m_data_dict.TryGetValue(item_code, out var object_type) ? object_type : ObjectType.NONE;
    }
}
