using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Module DataBase", menuName = "SO/DB/Create Module DataBase")]
public class ModuleDataBase : ScriptableObject, IModuleDataBase
{
    [SerializeField] private Module[] m_module_list;
    private Dictionary<ModuleCode, Module> m_module_dict;

#if UNITY_EDITOR
    private void OnEnable()
    {
        if(m_module_list == null)
        {
            return;
        }

        Initialize();
    }
#endif

    private void Initialize()
    {
        m_module_dict = new();

        foreach(var module in m_module_list)
        {
            m_module_dict.TryAdd(module.Code, module);
        }
    }

    public Module GetModule(ModuleCode module_code)
    {
        if(m_module_dict == null)
        {
            Initialize();
        }

        return m_module_dict.TryGetValue(module_code, out var module) ? module : null;
    }
}
