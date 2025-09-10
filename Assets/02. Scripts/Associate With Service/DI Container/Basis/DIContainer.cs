using System;
using System.Collections.Generic;

public static class DIContainer
{
    private static Dictionary<Type, object> m_instances = new();

    // �������� ����� �� ����Ѵ�.
    public static void Register<T>(object instance)
    {
        m_instances[typeof(T)] = instance;
    }

    // �������� ������ �� ����Ѵ�.
    public static T Resolve<T>()
    {
        if (!IsRegistered<T>())
        {
            throw new Exception($"{typeof(T)}�� DI �����̳ʿ� ��ϵǾ� ���� �ʽ��ϴ�.");
        }
        return (T)m_instances[typeof(T)];
    }

    // �����Ϸ��� ��ü�� �����ϴ��� Ȯ���� �� ����Ѵ�.
    public static bool IsRegistered<T>()
    {
        return m_instances.ContainsKey(typeof(T));
    }

    // ��ųʸ��� ����� �������� ��� �����Ѵ�.
    public static void Clear()
    {
        m_instances.Clear();
    }
}