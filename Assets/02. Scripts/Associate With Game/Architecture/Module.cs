using UnityEngine;

[CreateAssetMenu(fileName = "New Module", menuName = "SO/Create New Module")]
public class Module : ScriptableObject
{
    [Header("아이템 코드")]
    [SerializeField] private ItemCode m_item_code;
    public ItemCode Code => m_item_code;

    [Header("프리뷰")]
    [SerializeField] private GameObject m_preview_prefab;
    public GameObject PreviewPrefab => m_preview_prefab;

    [Header("리얼뷰")]
    [SerializeField] private GameObject m_realview_prefab;
    public GameObject RealviewPrefab => m_realview_prefab;
}