[System.Serializable]
public class InventoryData
{
    public ItemData[] Items;

    // �⺻ ������: ���� 12��
    public InventoryData()
    {
        Items = new ItemData[12];
    }

    // ������: ���� �� ���� ����
    public InventoryData(int slotCount)
    {
        Items = new ItemData[slotCount];
    }

    // ������: �̹� ������ �迭�� �ִ� ���
    public InventoryData(ItemData[] items)
    {
        Items = items;
    }
}