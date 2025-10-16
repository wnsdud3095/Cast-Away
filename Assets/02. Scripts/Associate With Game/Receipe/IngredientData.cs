using UnityEngine;

[System.Serializable]
public class IngredientData
{
    [Header("재료 아이템")]
    [SerializeField] private Item m_item;
    public Item Item => m_item;

    [Header("수량")]
    [SerializeField] private int m_count;
    public int Count => m_count;
}