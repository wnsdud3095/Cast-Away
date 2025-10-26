using InventoryService;
using KeyService;
using UnityEngine;

public class ShortcutUIInstaller : MonoBehaviour, IInstaller
{
    [Header("단축키 뷰")]
    [SerializeField] private ShortcutView m_shortcut_view;

    [Header("단축키 슬롯의 부모 트랜스폼")]
    [SerializeField] private Transform m_shortcut_slot_root;

    [Header("플레이어 컨트롤러")]
    [SerializeField] private PlayerCtrl m_player_ctrl;

    public void Install()
    {
        //ShortcutView 등록
        DIContainer.Register<IShortcutView>(m_shortcut_view);

        //숏컷 슬롯 뷰 가져오기
        var shortcut_slot_views = m_shortcut_slot_root.GetComponentsInChildren<ShortcutSlotView>();
        var item_slot_factory = DIContainer.Resolve<ItemSlotFactory>();

        //ShortcutPresenter 생성 및 등록
        var shortcut_presenter = new ShortcutPresenter(m_shortcut_view,
                                                       ServiceLocator.Get<IInventoryService>(),
                                                       m_player_ctrl);
        for (int i = 0; i < shortcut_slot_views.Length; i++)// 숏컷 0~4, 인벤토리 5~16
        {
            //ItemSlotPresenter 생성
            var item_slot_view = shortcut_slot_views[i].GetComponentInChildren<IItemSlotView>();
            item_slot_factory.Instantiate(item_slot_view, i, SlotType.Shortcut);

            //ShortcutSlotPresenter 생성 (키 입력, Shortcut UI 연동)
            new ShortcutSlotPresenter(shortcut_slot_views[i],
                                      DIContainer.Resolve<IItemDataBase>(),
                                      ServiceLocator.Get<IKeyService>(),
                                      ServiceLocator.Get<IInventoryService>(), // 인벤토리 참조
                                      i, shortcut_presenter);
        }
        //shortcut_presenter.Select(0); 아이템 스와퍼가 인스톨 된 이후에 호출하기 위해 아이템 스와퍼에서 호출

        DIContainer.Register<ShortcutPresenter>(shortcut_presenter);
    }
}
