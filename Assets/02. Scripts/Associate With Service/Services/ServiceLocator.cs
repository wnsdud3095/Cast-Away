using UnityEngine;
using System.Collections.Generic;
using System;
using EXPService;
using UserService;
using InventoryService;
using KeyService;

public static class ServiceLocator
{
    private static Dictionary<Type, object> m_services = new();
    public static IDictionary<Type, object> Services => m_services;

    public static void InitServices()
    {
        Register<IEXPService>(new EXPDataService());
        Register<IUserService>(new UserDataService());
        Register<IInventoryService>(new IventoryDataService());
        Register<IKeyService>(new KeyDataService());
    }

    public static void Register<T>(T service)
    {
        if (!Services.ContainsKey(typeof(T)))
        {
            Services.TryAdd(typeof(T), service);
        }
    }

    public static T Get<T>()
    {
        if (!Services.TryGetValue(typeof(T), out var service))
        {
            throw new Exception($"{typeof(T)} 서비스가 존재하지 않습니다.");
        }
        else
        {
            return (T)service;
        }

    }
}
