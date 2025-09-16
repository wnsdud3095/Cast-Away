using System;
using System.Collections.Generic;

public static class DIContainer
{
    private static Dictionary<Type, object> m_instances = new();

    // 의존성을 등록할 때 사용한다.
    public static void Register<T>(object instance)
    {
        m_instances[typeof(T)] = instance;
    }

    // 의존성을 주입할 때 사용한다.
    public static T Resolve<T>()
    {
        if (!IsRegistered<T>())
        {
            throw new Exception($"{typeof(T)}가 DI 컨테이너에 등록되어 있지 않습니다.");
        }
        return (T)m_instances[typeof(T)];
    }

    // 의존하려는 객체가 존재하는지 확인할 때 사용한다.
    public static bool IsRegistered<T>()
    {
        return m_instances.ContainsKey(typeof(T));
    }

    // 딕셔너리를 지우며 의존성을 모두 제거한다.
    public static void Clear()
    {
        m_instances.Clear();
    }
}