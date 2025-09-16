using System.Collections.Generic;
using UnityEngine;

public class PopupUIManagerInstaller : MonoBehaviour, IInstaller
{
    [Header("�˾�UI �Ŵ���")]
    [SerializeField] private PopupUIManager m_popup_manager;

    public void Install()
    {
        var popup_data_list = new List<PopupData>{
            new("Binder", DIContainer.Resolve<KeyBinderPresenter>()),
            // ...
        };

        m_popup_manager.Inject(popup_data_list);
    }
}
