using System;

namespace InventoryService
{
    public interface IInventoryService
    {

        void Inject(IItemDataBase item_db);			// ������ �Ŵ����� ���Թ��� �޼���

        void InitializeSlot(int offset);			// offset ��ġ�� ������ �ʱ�ȭ�ϴ� �޼���


        event Action<int, ItemData> OnUpdatedSlot;	// ������ ���ŵ� ��쿡 ����� ��������Ʈ
        void AddItem(ItemCode code, int count);
        void RemoveItem(ItemCode code, int count);
        void SetItem(int offset, ItemCode code, int count);
        int UpdateItem(int offset, int count);
        void Clear(int offset);
        int GetItemCount(ItemCode code);
        int GetValidOffset(ItemCode code);
        int GetPriorityOffset(ItemCode code);
        bool HasItem(ItemCode code);
        ItemData GetItem(int offset);
    }
}