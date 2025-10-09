using InventoryService;
using UnityEngine;

public class FishingUIInstaller : MonoBehaviour, IInstaller
{
    [Header("낚시 뷰")]
    [SerializeField] private FishingView m_fishing_view;

    public void Install()
    {
        InstallFishing();
    }

    private void InstallFishing()
    {
        DIContainer.Register<IFishingView>(m_fishing_view);

        var fishing_presenter = new FishingPresenter(m_fishing_view,
                                                     ServiceLocator.Get<IInventoryService>());
        DIContainer.Register<FishingPresenter>(fishing_presenter);
    }
}
