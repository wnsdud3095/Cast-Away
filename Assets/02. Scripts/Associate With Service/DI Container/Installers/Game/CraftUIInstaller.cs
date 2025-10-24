using InventoryService;
using UnityEngine;
using UserService;

public class CraftUIInstaller : MonoBehaviour, IInstaller
{
    [Header("Craft 뷰")]
    [SerializeField] private CraftView m_craft_view;

    [Header("컴팩트 Craft 뷰")]
    [SerializeField] private CompactCraftView m_compact_craft_view;

    [Header("레시피 목록")]
    [SerializeField] private CraftReceipe[] m_craft_receipe_list;

    public void Install()
    {
        InstallCompact();
        InstallCraftUI();
    }

    private void InstallCompact()
    {
        DIContainer.Register<ICompactCraftView>(m_compact_craft_view);

        var compact_craft_presenter = new CompactCraftPresenter(m_compact_craft_view,
                                                                    ServiceLocator.Get<IInventoryService>());
        DIContainer.Register<CompactCraftPresenter>(compact_craft_presenter);
    }

    private void InstallCraftUI()
    {
        DIContainer.Register<ICraftView>(m_craft_view);

        var craft_presenter = new CraftPresenter(m_craft_view,
                                                     m_craft_receipe_list,
                                                     ServiceLocator.Get<IUserService>(),
                                                     DIContainer.Resolve<CompactCraftPresenter>());
        DIContainer.Register<CraftPresenter>(craft_presenter);
    }
}
