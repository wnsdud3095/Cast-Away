using UnityEngine;

public class Bootstrapper : MonoBehaviour
{
    private IInstaller[] m_installers;

    protected virtual void Awake()
    {
        // 하위의 IInstaller를 구현하는 모든 자식 오브젝트를 모은다. 
        m_installers = transform.GetComponentsInChildren<IInstaller>();
        Debug.Log("<color=lime>InitServices 실행됨</color>");

        ServiceLocator.InitServices(); // 서비스를 딕셔너리에 등록
    }

    protected virtual void Start()
    {
        SoundManager.Instance.PlayRandBGMLoop();

        // 이들을 순회하며 의존성을 주입한다.
        foreach (var installer in m_installers)
        {
            installer.Install();
        }
    }
}