using UnityEngine;

[System.Serializable]
public class StatusData
{
    public int Level;
    public int EXP;
    public float Thirst;
    public float Hunger;
    public float HP;

    public StatusData(int level = 1, int exp = 0, float thirst = 100, float hunger = 100, float hp = 100f)
    {
        Level = level;
        EXP = exp;
        Thirst = thirst;
        Hunger = hunger;
        HP = hp;
    }
}
