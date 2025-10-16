using UnityEngine;

public class ModelEventSender : MonoBehaviour
{
    [Header("플레이어 컨트롤러")]
    [SerializeField] private PlayerCtrl m_player_ctrl;

    private PlayerWalkState m_player_walk_state;
    private PlayerRunState m_player_run_state;

    private void Start()
    {
        m_player_walk_state = m_player_ctrl.GetComponent<PlayerWalkState>();
        m_player_run_state = m_player_ctrl.GetComponent<PlayerRunState>();
    }

    public void PlayGrassWalkSFX()
    {
        m_player_walk_state.PlaySFX();
    }

    public void PlayGrassRunSFX()
    {
        m_player_run_state.PlaySFX();
    }
}
