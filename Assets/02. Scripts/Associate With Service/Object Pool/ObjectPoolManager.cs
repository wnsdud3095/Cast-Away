using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : Singleton<ObjectManager>
{
    [Header("UI 오브젝트 풀 목록")]
    [SerializeField] private List<Pool> m_ui_pool_list;

    [Header("아이템 오브젝트 풀 목록")]
    [SerializeField] private List<Pool> m_item_pool_list;

    [Header("동물 오브젝트 풀 목록")]
    [SerializeField] private List<Pool> m_animal_pool_list;

    [Header("이펙트 오브젝트 풀 목록")]
    [SerializeField] private List<Pool> m_effect_pool_list;

    private Dictionary<ObjectType, Pool> m_pool_dict;

    protected override void Awake()
    {
        base.Awake();

        Initialize();
    }

    public void Initialize()
    {
        m_pool_dict = new();

        InitializePool(m_ui_pool_list);
        InitializePool(m_item_pool_list);
        InitializePool(m_animal_pool_list);
        InitializePool(m_effect_pool_list);
    }

    private void InitializePool(List<Pool> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            m_pool_dict.Add(list[i].Type, list[i]);
            for (int j = 0; j < list[i].Count; j++)
            {
                list[i].Queue.Enqueue(CreateNewObject(list[i]));
            }
        }
    }

    private GameObject CreateNewObject(Pool pool)
    {
        var new_obj = Instantiate(pool.Prefab, pool.Container);
        new_obj.SetActive(false);

        return new_obj;
    }

    public Pool GetPool(ObjectType type)
    {
        return m_pool_dict.ContainsKey(type) ? m_pool_dict[type] : null;
    }

    public GameObject GetObject(ObjectType type)
    {
        var pool = GetPool(type);

        GameObject obj;
        if (pool.Queue.Count > 0)
        {
            obj = pool.Queue.Dequeue();
        }
        else
        {
            obj = CreateNewObject(pool);
        }
        obj.SetActive(true);

        return obj;
    }

    public void ReturnObject(GameObject obj, ObjectType type)
    {
        if (!obj)
        {
            Destroy(obj);
            return;
        }

        var pool = GetPool(type);

        if (pool.Queue.Count < pool.Count)
        {
            pool.Queue.Enqueue(obj);
            obj.SetActive(false);
        }
        else
        {
            Destroy(obj);
        }
    }

    public void ReturnObjectsAll()
    {
        foreach (var pair in m_pool_dict)
        {
            var pool = pair.Value;

            for (int i = 0; i < pool.Container.childCount; i++)
            {
                Transform child = pool.Container.GetChild(i);
                GameObject obj = child.gameObject;

                if (obj.activeSelf)
                {
                    ReturnObject(obj, pool.Type);
                }
            }
        }
    }
}