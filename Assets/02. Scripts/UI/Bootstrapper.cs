using UnityEngine;

public class Bootstrapper : MonoBehaviour
{
    private IInstaller[] m_installers;

    protected virtual void Awake()
    {
        // 하위의 IInstaller를 구현하는 모든 자식 오브젝트를 모은다. 
        m_installers = transform.GetComponentsInChildren<IInstaller>();
    }

    protected virtual void Start()
    {
        // 이들을 순회하며 의존성을 주입한다.
        foreach (var installer in m_installers)
        {
            installer.Install();
        }
    }
}