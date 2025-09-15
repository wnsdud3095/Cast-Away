using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "SO/Create Item")]
public class Item : ScriptableObject
{
    [Header("������ ����")]
    [Header("������ �ڵ�")]
    [SerializeField] private ItemCode m_code;
    public ItemCode Code => m_code;

    [Header("������ Ÿ��")]
    [SerializeField] private ItemType m_type;
    public ItemType Type => m_type;

    [Header("������ ��")]
    [SerializeField] private string m_name;
    public string Name => m_name;

    [Header("���� ��ø ����")]
    [SerializeField] private bool m_stackable;
    public bool Stackable => m_stackable;

    [Header("������ ��Ÿ��")]
    [SerializeField] private float m_cool = -1f;
    public float Cool => m_cool;

    [Header("������ �̹���")]
    [SerializeField] private Sprite m_sprite;
    public Sprite Sprite => m_sprite;
}
