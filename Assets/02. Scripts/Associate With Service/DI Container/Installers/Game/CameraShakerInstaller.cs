using UnityEngine;

public class CameraShakerInstaller : MonoBehaviour, IInstaller
{
    [Header("파괴 지형의 부모 트랜스폼")]
    [SerializeField] private Transform m_breakable_root;

    [Header("카메라 셰이커")]
    [SerializeField] private CameraShaker m_camera_shaker;

    public void Install()
    {
        InstallShaker();
    }

    private void InstallShaker()
    {
        DIContainer.Register<CameraShaker>(m_camera_shaker);

        var breakables = m_breakable_root.GetComponentsInChildren<BaseBreakable>();
        
        foreach(var breakable in breakables)
        {
            breakable.Inject(m_camera_shaker);
        }
    }
}
