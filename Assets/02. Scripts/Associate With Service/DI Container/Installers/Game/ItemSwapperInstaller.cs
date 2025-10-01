using UnityEngine;

public class ItemSwapperInstaller : MonoBehaviour, IInstaller
{
    [Header("아이템 스와퍼")]
    [SerializeField] private ItemSwapper m_item_swapper;

    public void Install()
    {
        InstallSwapper();
    }

    private void InstallSwapper()
    {
        DIContainer.Register<ItemSwapper>(m_item_swapper);

        m_item_swapper.Inject(DIContainer.Resolve<ShortcutPresenter>());
    }
}
