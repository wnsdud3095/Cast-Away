using UnityEngine;

[System.Serializable]
public class UserData 
{
    public Vector3 Position;
    public StatusData Status;

    public UserData() 
    { 
        Position = new Vector3(0,0,33f);
        Status = new StatusData();
    }
    public UserData(Vector3 pos, StatusData status)
    {
        Position = pos;
        Status = status;
    }
}
