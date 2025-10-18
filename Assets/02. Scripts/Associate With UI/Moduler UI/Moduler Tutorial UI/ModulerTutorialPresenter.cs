using UnityEngine;

public class ModulerTutorialPresenter
{
    private readonly IModulerTutorialView m_view;

    public bool Active { get; private set; }

    public ModulerTutorialPresenter(IModulerTutorialView view)
    {
        m_view = view;

        m_view.Inject(this);
    }

    public void OpenUI()
    {
        Active = true;

        m_view.OpenUI();
        GameEventBus.Publish(GameEventType.CRAFTING);
    }

    public void CloseUI()
    {
        Active = false;

        m_view.CloseUI();
    }
}
