using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Pool
{
    public ObjectType Type;
    public int Count;
    public GameObject Prefab;
    public Transform Container;
    public Queue<GameObject> Queue = new();
}