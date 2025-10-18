using UnityEngine;

[CreateAssetMenu(fileName = "New Module", menuName = "SO/Create New Module Receipe")]
public class ModuleReceipe : ScriptableObject
{
    [Header("모듈의 이름")]
    [SerializeField] private string m_module_name;
    public string Name => m_module_name;

    [Header("모듈의 코드")]
    [SerializeField] private ModuleCode m_module_code;
    public ModuleCode Code => m_module_code;

    [Header("모듈의 이미지")]
    [SerializeField] private Sprite m_module_image;
    public Sprite Image => m_module_image;

    [Header("재료의 목록")]
    [SerializeField] private IngredientData[] m_ingredient_list;
    public IngredientData[] Ingredients => m_ingredient_list;

    [Header("해금 제작 레벨")]
    [SerializeField] private int m_unlock_level;
    public int Unlock => m_unlock_level; 
}
