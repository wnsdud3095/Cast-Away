using UnityEngine;

public class Bootstrapper : MonoBehaviour
{
    private IInstaller[] m_installers;

    protected virtual void Awake()
    {
        // ������ IInstaller�� �����ϴ� ��� �ڽ� ������Ʈ�� ������. 
        m_installers = transform.GetComponentsInChildren<IInstaller>();
        ServiceLocator.InitServices(); // ���񽺸� ��ųʸ��� ���
    }

    protected virtual void Start()
    {
        // �̵��� ��ȸ�ϸ� �������� �����Ѵ�.
        foreach (var installer in m_installers)
        {
            installer.Install();
        }
    }
}