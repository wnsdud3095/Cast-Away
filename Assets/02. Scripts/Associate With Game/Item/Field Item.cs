using UnityEngine;

public class FieldItem : MonoBehaviour
{
    [Header("아이템")]
    [SerializeField] private Item m_item;

    public ItemCode Code => m_item.Code;
    public string Name => m_item.Name;
}