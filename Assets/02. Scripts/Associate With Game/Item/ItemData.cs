[System.Serializable]
public class ItemData
{
    public ItemCode Code;
    public int Count;

    public ItemData(ItemCode code = ItemCode.NONE, int count = 0)
    {
        Code = code;
        Count = count;
    }
}