using InventoryService;
using UnityEngine;

public class FishingUIInstaller : MonoBehaviour, IInstaller
{
    [Header("낚시 뷰")]
    [SerializeField] private FishingView m_fishing_view;

    [Header("플레이어 컨트롤러")]
    [SerializeField] private PlayerCtrl m_player_ctrl;

    public void Install()
    {
        InstallFishing();
    }

    private void InstallFishing()
    {
        DIContainer.Register<IFishingView>(m_fishing_view);

        var fishing_presenter = new FishingPresenter(m_fishing_view,
                                                     ServiceLocator.Get<IInventoryService>(),
                                                     m_player_ctrl);
        DIContainer.Register<FishingPresenter>(fishing_presenter);
    }
}
