using UnityEngine;

[System.Serializable]
public class StatusData
{
    public int Level;
    public int EXP;
    public int Thirst;
    public int Hunger;
    public float HP;

    public StatusData(int level = 1, int exp = 0, int thirst= 100, int hunger = 100, float hp = 100f)
    {
        Level = level;
        EXP = exp;
        Thirst = thirst;
        Hunger = hunger;
        HP = hp;
    }
}
