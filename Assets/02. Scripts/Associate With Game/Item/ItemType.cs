[System.Flags]
public enum ItemType
{
    NONE = 0,

    Consumable = 1 << 0,
    Material = 1 << 1,
    Tools = 1 << 2,
    Foods = 1 << 3,
    Structure = 1 << 4,
}