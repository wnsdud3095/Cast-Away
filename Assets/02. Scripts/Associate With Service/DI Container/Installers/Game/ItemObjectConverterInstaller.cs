using UnityEngine;

public class ItemObjectConverterInstaller : MonoBehaviour, IInstaller
{
    [Header("아이템 오브젝트 변환자")]
    [SerializeField] private ItemObjectConverter m_item_object_converter;

    public void Install()
    {
        InstallConverter();
    }

    private void InstallConverter()
    {
        DIContainer.Register<IItemObjectConverter>(m_item_object_converter);
    }
}
