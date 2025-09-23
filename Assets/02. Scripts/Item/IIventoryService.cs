using System;

namespace InventoryService
{
    public interface IInventoryService
    {

        void Inject(IItemDataBase item_db);			// 아이템 매니저를 주입받을 메서드

        void InitializeSlot(int offset);			// offset 위치의 슬롯을 초기화하는 메서드


        event Action<int, ItemData> OnUpdatedSlot;	// 슬롯이 갱신된 경우에 실행될 델리게이트
        void AddItem(ItemCode code, int count);
        void RemoveItem(ItemCode code, int count);
        void SetItem(int offset, ItemCode code, int count);
        int UpdateItem(int offset, int count);
        void Clear(int offset);
        int GetItemCount(ItemCode code);
        int GetValidOffset(ItemCode code);
        int GetPriorityOffset(ItemCode code);
        bool HasItem(ItemCode code);
        void UseItem(int offset);

        ItemData GetItem(int offset);
    }
}