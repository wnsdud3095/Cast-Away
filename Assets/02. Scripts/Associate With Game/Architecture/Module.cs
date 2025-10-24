using UnityEngine;

[CreateAssetMenu(fileName = "New Module", menuName = "SO/Create New Module")]
public class Module : ScriptableObject
{
    [Header("모듈 코드")]
    [SerializeField] private ModuleCode m_module_code;
    public ModuleCode Code => m_module_code;

    [Header("프리뷰")]
    [SerializeField] private GameObject m_preview_prefab;
    public GameObject PreviewPrefab => m_preview_prefab;

    [Header("리얼뷰")]
    [SerializeField] private GameObject m_realview_prefab;
    public GameObject RealviewPrefab => m_realview_prefab;
}