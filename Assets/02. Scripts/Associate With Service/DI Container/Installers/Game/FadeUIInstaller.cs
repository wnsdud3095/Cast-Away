using UnityEngine;

public class FadeUIInstaller : MonoBehaviour, IInstaller
{
    [Header("페이드 뷰")]
    [SerializeField] private FadeView m_fade_view;

    public void Install()
    {
        InstallFade();
    }

    private void InstallFade()
    {
        DIContainer.Register<IFadeView>(m_fade_view);

        var fade_presenter = new FadePresenter(m_fade_view);
        DIContainer.Register<FadePresenter>(fade_presenter);
    }
}
