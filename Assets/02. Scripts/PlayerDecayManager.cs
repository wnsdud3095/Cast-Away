using UnityEngine;
using System.Collections;
using UserService;

public class PlayerDecayManager : MonoBehaviour
{
    [SerializeField] private PlayerStatus m_player_status;
    [SerializeField] private PlayerCtrl m_player_ctrl;

    [SerializeField] private float m_decay_interval = 1f;

    private float m_hunger_decay = -0.2f;
    private float m_running_hunger_decay = -0.4f;

    private float m_thirst_decay = -0.2f;

    private void Start()
    {
        StartCoroutine(DecayRoutine());
    }

    private IEnumerator DecayRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(m_decay_interval);


            //상태 패턴으로 변경시 상태 체크 if문 작성
            float hunger_decay = m_player_ctrl.Movement.IsDashActive ? m_running_hunger_decay : m_hunger_decay;
            float thirst_decay = m_thirst_decay;

            m_player_status.ChangeHunger(hunger_decay);
            m_player_status.ChangeThirst(thirst_decay);

            var user_service = ServiceLocator.Get<IUserService>();
            user_service.UpdateLevel(10);
        }
    }
}
