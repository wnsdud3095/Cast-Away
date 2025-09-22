using System.Collections.Generic;
using InventoryService;
using KeyService;
using ShortcutService;
using UnityEngine;

public class ShortcutUIInstaller : MonoBehaviour, IInstaller
{
    [Header("단축키 뷰")]
    [SerializeField] private ShortcutView m_shortcut_view;

    [Header("단축키 슬롯의 부모 트랜스폼")]
    [SerializeField] private Transform m_shortcut_slot_root;

    public void Install()
    {
        // 1. ShortcutView 등록
        DIContainer.Register<IShortcutView>(m_shortcut_view);

        // 2. 숏컷 슬롯 뷰 가져오기
        var shortcut_slot_views = m_shortcut_slot_root.GetComponentsInChildren<ShortcutSlotView>();
        var item_slot_factory = DIContainer.Resolve<ItemSlotFactory>();

        for (int i = 0; i < shortcut_slot_views.Length; i++)
        {
            int offset = 12 + i; // 인벤토리 0~11, 숏컷 12~16

            // 3. ItemSlotPresenter 생성 (Factory 사용, 생성자 안에서 뷰 Inject 완료)
            var item_slot_view = shortcut_slot_views[i].GetComponentInChildren<IItemSlotView>();
            item_slot_factory.Instantiate(item_slot_view, offset, SlotType.Inventory);

            // 4. ShortcutSlotPresenter 생성 (키 입력, Shortcut UI 연동)
            new ShortcutSlotPresenter(shortcut_slot_views[i],
                                      DIContainer.Resolve<IItemDataBase>(),
                                      ServiceLocator.Get<IKeyService>(),
                                      ServiceLocator.Get<IInventoryService>(), // 인벤토리 참조
                                      offset);
        }

        // 5. ShortcutPresenter 생성 및 등록
        var shortcut_presenter = new ShortcutPresenter(m_shortcut_view,
                                                       DIContainer.Resolve<InventoryPresenter>());
        DIContainer.Register<ShortcutPresenter>(shortcut_presenter);
    }
}
