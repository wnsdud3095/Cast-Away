[System.Serializable]
public class InventoryData
{
    public ItemData[] Items;

    // 기본 생성자: 슬롯 12개
    public InventoryData()
    {
        Items = new ItemData[12];
    }

    // 생성자: 슬롯 수 지정 가능
    public InventoryData(int slotCount)
    {
        Items = new ItemData[slotCount];
    }

    // 생성자: 이미 아이템 배열이 있는 경우
    public InventoryData(ItemData[] items)
    {
        Items = items;
    }
}