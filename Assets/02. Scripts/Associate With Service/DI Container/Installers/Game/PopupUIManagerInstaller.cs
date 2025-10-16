using System.Collections.Generic;
using UnityEngine;

public class PopupUIManagerInstaller : MonoBehaviour, IInstaller
{
    [Header("팝업UI 매니저")]
    [SerializeField] private PopupUIManager m_popup_manager;

    public void Install()
    {
        var popup_data_list = new List<PopupData>
        {
            new("Binder", DIContainer.Resolve<KeyBinderPresenter>()),
            new("Inventory", DIContainer.Resolve<InventoryPresenter>()),
            new("Pause", DIContainer.Resolve<SettingPresenter>()),
            new("Build", DIContainer.Resolve<ModulerPresenter>()),
        };

        m_popup_manager.Inject(popup_data_list);
    }
}