using UnityEngine;

[CreateAssetMenu(fileName = "New Crafting", menuName = "SO/Create New Craft Receipe")]
public class CraftReceipe : ScriptableObject
{
    [Header("레시피의 이름")]
    [SerializeField] private string m_craft_name;
    public string Name => m_craft_name;

    [Header("제작 아이템의 코드")]
    [SerializeField] private ItemCode m_item_code;
    public ItemCode Code => m_item_code;

    [Header("제작 아이템의 이미지")]
    [SerializeField] private Sprite m_craft_image;
    public Sprite Image => m_craft_image;

    [Header("재료의 목록")]
    [SerializeField] private IngredientData[] m_ingredient_list;
    public IngredientData[] Ingredients => m_ingredient_list;

    [Header("해금 제작 레벨")]
    [SerializeField] private int m_unlock_level;
    public int Unlock => m_unlock_level;

    [Header("제작 경험치")]
    [SerializeField] private int m_craft_exp;
    public int EXP => m_craft_exp;

    [Header("기본 제작 가능여부")]
    [SerializeField] private bool m_is_default_unlocked;
    public bool IsDefaultUnlocked => m_is_default_unlocked;
}
