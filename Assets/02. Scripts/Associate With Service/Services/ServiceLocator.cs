using UnityEngine;
using System.Collections.Generic;
using System;
using EXPService;
using UserService;

public static class ServiceLocator
{
    private static Dictionary<Type, object> m_services = new();
    public static IDictionary<Type, object> Services => m_services;

    public static void InitServices()
    {
        Register<IEXPService>(new EXPDataService());
        Register<IUserService>(new UserDataService());
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
            throw new Exception($"{typeof(T)} ���񽺰� �������� �ʽ��ϴ�.");
        }
        else
        {
            return (T)service;
        }

    }
}
