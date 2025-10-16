using UnityEngine;

public class ItemDetectorInstaller : MonoBehaviour, IInstaller
{
    [Header("필드 아이템 감지자 뷰")]
    [SerializeField] private ItemDetectorView m_item_detector_view;

    public void Install()
    {
        InstallDetector();
    }

    private void InstallDetector()
    {
        DIContainer.Register<IItemDetectorView>(m_item_detector_view);

        var item_detector_presetner = new ItemDetectorPresenter(m_item_detector_view);
        DIContainer.Register<ItemDetectorPresenter>(item_detector_presetner);
    }
}
