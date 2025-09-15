using KeyService;
using UnityEngine;

public class KeyBinderUIInstaller : MonoBehaviour, IInstaller
{
    [Header("Ű ���δ� ��")]
    [SerializeField] private KeyBinderView m_key_binder_view;

    [Header("Ű ���δ� ������ �θ� Ʈ������")]
    [SerializeField] private Transform m_key_binder_slot_root;

    public void Install()
    {
        DIContainer.Register<IKeyBinderView>(m_key_binder_view);

        var key_binder_presenter = new KeyBinderPresenter(m_key_binder_view);
        DIContainer.Register<KeyBinderPresenter>(key_binder_presenter);

        var key_binder_slots = m_key_binder_slot_root.GetComponentsInChildren<KeyBinderSlotView>();
        foreach (var slot in key_binder_slots)
        {
            slot.Inject(ServiceLocator.Get<IKeyService>());
        }
    }
}
