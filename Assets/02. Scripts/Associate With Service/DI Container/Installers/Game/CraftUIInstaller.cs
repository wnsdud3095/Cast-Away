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

    [Header("모듈러")]
    [SerializeField] private Moduler m_moduler;

    [Header("모듈 데이터베이스")]
    [SerializeField] private ModuleDataBase m_module_db;

    [Header("카메라 셰이커")]
    [SerializeField] private CameraShaker m_camera_shaker;

    [Header("아이템-오브젝트 변환자")]
    [SerializeField] private ItemObjectConverter m_item_object_converter;

    public void Install()
    {
        InstallCompact();
        InstallCraftUI();
        InstallModuler();
    }

    private void InstallModuler()
    {
        DIContainer.Register<IModuleDataBase>(m_module_db);
        DIContainer.Register<Moduler>(m_moduler);

        m_moduler.Inject(m_module_db,
                         ServiceLocator.Get<IInventoryService>(),
                         DIContainer.Resolve<ModulerTutorialPresenter>(),
                         m_camera_shaker,
                         m_item_object_converter,
                         DIContainer.Resolve<CraftPresenter>());
    }

    private void InstallCompact()
    {
        DIContainer.Register<ICompactCraftView>(m_compact_craft_view);

        var compact_craft_presenter = new CompactCraftPresenter(m_compact_craft_view,
                                                                    ServiceLocator.Get<IInventoryService>(),
                                                                    DIContainer.Resolve<ModulerTutorialPresenter>(),
                                                                    m_moduler);
        DIContainer.Register<CompactCraftPresenter>(compact_craft_presenter);
    }

    private void InstallCraftUI()
    {
        DIContainer.Register<ICraftView>(m_craft_view);

        var craft_presenter = new CraftPresenter(m_craft_view,
                                                     m_craft_receipe_list,
                                                     ServiceLocator.Get<IUserService>(),
                                                     DIContainer.Resolve<CompactCraftPresenter>(),
                                                     DIContainer.Resolve<ModulerTutorialPresenter>(),
                                                     m_moduler);
        DIContainer.Register<CraftPresenter>(craft_presenter);
    }
}
