using UnityEngine;

public class ModulerTutorialInstaller : MonoBehaviour, IInstaller
{
    [Header("모듈러 튜토리얼 뷰")]
    [SerializeField] private ModulerTutorialView m_moduler_tutorial_view;
    
    public void Install()
    {
        InstallTutorial();
    }

    private void InstallTutorial()
    {
        DIContainer.Register<IModulerTutorialView>(m_moduler_tutorial_view);

        var moduler_tutorial_presenter = new ModulerTutorialPresenter(m_moduler_tutorial_view);
        DIContainer.Register<ModulerTutorialPresenter>(moduler_tutorial_presenter);
    }
}
