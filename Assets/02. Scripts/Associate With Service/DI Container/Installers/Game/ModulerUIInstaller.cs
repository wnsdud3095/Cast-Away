using UnityEngine;

public class ModulerUIInstaller : MonoBehaviour, IInstaller
{
    [Header("모듈러 뷰")]
    [SerializeField] private ModulerView m_moduler_view;

    public void Install()
    {
        InstallModuler();
    }

    private void InstallModuler()
    {
        DIContainer.Register<IModulerView>(m_moduler_view);

        var moduler_presenter = new ModulerPresenter(m_moduler_view);
        DIContainer.Register<ModulerPresenter>(moduler_presenter);
    }
}
