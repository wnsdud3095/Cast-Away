using InventoryService;
using UserService;
using System.Collections.Generic;
using UnityEngine;

public class CraftPresenter: IPopupPresenter
{
    private readonly ICraftView m_view;
    private readonly CraftReceipe[] m_receipe_list;
    private readonly IUserService m_user_service;
    private readonly CompactCraftPresenter m_compact_craft_presenter;
    private IItemDataBase m_item_db;

    private List<CraftReceipe> m_filtered_receipe_list;
    private ItemType m_current_type;

    private HashSet<ItemCode> m_locked_codes = new(); //중복방지 및 Contains 를 활용한 빠른 검색용
    private Dictionary<CraftUnlockTrigger, List<ItemCode>> m_trigger_locked_codes = new();

    public CraftPresenter(ICraftView view,
                            CraftReceipe[] receipe_list,
                            IUserService user_service,
                            CompactCraftPresenter compact_craft_presenter)
    {
        m_view = view;

        m_receipe_list = receipe_list;
        m_user_service = user_service;
        m_compact_craft_presenter = compact_craft_presenter;

        m_filtered_receipe_list = new List<CraftReceipe>();
        m_item_db = DIContainer.Resolve<IItemDataBase>();

        m_current_type = ItemType.Foods;

        InitializeLockedList();

        m_view.Inject(this);
    }

    private void InitializeLockedList()
    {
        foreach (var recipe in m_receipe_list)
        {
            // 기본적으로 잠금, 하지만 항상 제작 가능한 애들은 예외
            if (!recipe.IsDefaultUnlocked)
                m_locked_codes.Add(recipe.Code);
        }
    }
    public void SubscribeTrigger(CraftUnlockTrigger trigger)
    {
        trigger.OnPlayerEnter += HandleTriggerEnter;
        trigger.OnPlayerExit += HandleTriggerExit;
    }

    private void HandleTriggerEnter(List<ItemCode> restricted_codes, CraftUnlockTrigger trigger)
    {
        // 트리거 안에 들어오면 잠금 해제
        foreach (var code in restricted_codes)
            m_locked_codes.Remove(code);

        m_trigger_locked_codes[trigger] = restricted_codes;
        RefreshSlots();
    }

    private void HandleTriggerExit(CraftUnlockTrigger trigger)
    {
        // 트리거 영역 벗어나면 다시 잠금
        if (!m_trigger_locked_codes.TryGetValue(trigger, out var codes)) return;

        foreach (var code in codes)
            m_locked_codes.Add(code);

        m_trigger_locked_codes.Remove(trigger);
        RefreshSlots();
    }

    public void ChangeFilter(ItemType type)
    {
        m_current_type = type;
        m_filtered_receipe_list.Clear();

        foreach (CraftReceipe receipe in m_receipe_list)
        {
            if (m_item_db.GetItem(receipe.Code).Type == type)
            {
                m_filtered_receipe_list.Add(receipe);
            };
        }

        RefreshSlots();
    }

    private void RefreshSlots()
    {
        Debug.Log($"### RefreshSlots 호출됨. 필터:{m_current_type}, 잠금:{m_locked_codes.Count}개");

        // 기존 슬롯 반환
        m_view.ClearSlots();

        // 필터된 레시피 반복
        foreach (var recipe in m_filtered_receipe_list)
        {
            if (m_locked_codes.Contains(recipe.Code))
            {
                Debug.Log($"(잠긴 레시피 코드 {recipe.Code})");
                continue; // 잠긴 레시피는 출력 X
            }

            // 슬롯 생성: 항상 뷰를 통해 만들고 리스트에 추가됨
            var slot_view = m_view.InstantiateSlotView();

            // 슬롯 프레젠터 생성
            var slot_presenter = new CraftSlotPresenter(
                slot_view,
                recipe,
                m_user_service,
                m_compact_craft_presenter
            );

        }
    }


    public void OpenUI()
    {
        m_view.PlaySFX("UI Open");
        m_view.OpenUI();

        ChangeFilter(m_current_type);
    }

    public void CloseUI()
    {
        m_compact_craft_presenter.CloseUI();
        m_view.PlaySFX("UI Close");
        m_view.CloseUI();
    }

    public void SortDepth()
    {
        m_view.SetDepth();
    }
}
