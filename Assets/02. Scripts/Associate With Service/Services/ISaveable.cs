using UnityEngine;

public interface ISaveable 
{
    bool Load(); //예외처리를 위해 bool타입
    void Save();
}
