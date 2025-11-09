using InventoryService;
using KeyService;
using SettingService;
using System.Collections.Generic;
using UnityEngine;
using UserService;

public class SaveLoadManager : MonoBehaviour
{
    private List<ISaveable> m_saveables = new List<ISaveable>();

    private void Awake()
    {
        Register(ServiceLocator.Get<IUserService>());
        Register(ServiceLocator.Get<IInventoryService>());
        Register(ServiceLocator.Get<IKeyService>());
        Register(ServiceLocator.Get<ISettingService>());

        LoadAll();

        DIContainer.Register<SaveLoadManager>(this);
    }

    public void Register(ISaveable saveable)
    {
        if (!m_saveables.Contains(saveable))
            m_saveables.Add(saveable);
    }

    public void SaveAll()
    {
        foreach (var saveable in m_saveables)
        {
            saveable.Save();
        }
        Debug.Log("SaveManager: 모든 데이터 저장 완료");
    }

    public void LoadAll()
    {
        foreach (var saveable in m_saveables)
        {
            if(!saveable.Load())
            {
                Debug.Log("SaveManager: 데이터 로드 실패");
            }
        }
    }
}
