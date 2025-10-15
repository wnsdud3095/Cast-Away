using InventoryService;
using UnityEngine;
using UserService;

public class ModulerUIInstaller : MonoBehaviour, IInstaller
{
    [Header("모듈러 뷰")]
    [SerializeField] private ModulerView m_moduler_view;

    [Header("컴팩트 모듈러 뷰")]
    [SerializeField] private CompactModulerView m_compact_moduler_view;

    [Header("레시피 목록")]
    [SerializeField] private ModuleReceipe[] m_module_receipe_list;

    public void Install()
    {
        InstallCompact();
        InstallModuler();
    }

    private void InstallCompact()
    {
        DIContainer.Register<ICompactModulerView>(m_compact_moduler_view);

        var compact_moduler_presenter = new CompactModulerPresenter(m_compact_moduler_view,
                                                                    ServiceLocator.Get<IInventoryService>());
        DIContainer.Register<CompactModulerPresenter>(compact_moduler_presenter);
    }

    private void InstallModuler()
    {
        DIContainer.Register<IModulerView>(m_moduler_view);

        var moduler_presenter = new ModulerPresenter(m_moduler_view,
                                                     m_module_receipe_list,
                                                     ServiceLocator.Get<IUserService>(),
                                                     DIContainer.Resolve<CompactModulerPresenter>());
        DIContainer.Register<ModulerPresenter>(moduler_presenter);
    }
}
