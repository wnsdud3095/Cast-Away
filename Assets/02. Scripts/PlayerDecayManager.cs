using UnityEngine;
using System.Collections;

public class PlayerDecayManager : MonoBehaviour
{
    [Header("플레이어 컨트롤러")]
    [SerializeField] private PlayerCtrl m_player_ctrl;

    [SerializeField] private float m_decay_interval = 1f;

    [Header("기본 허기 감소 속도")]
    [SerializeField] private float m_hunger_decay = -0.2f;
    
    [Header("달리기 허기 감소 속도")]
    [SerializeField] private float m_running_hunger_decay = -0.4f;

    [Header("갈증 감소 속도")]
    [SerializeField] private float m_thirst_decay = -0.2f;

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

            m_player_ctrl.State.ChangeHunger(hunger_decay);
            m_player_ctrl.State.ChangeThirst(thirst_decay);
        }
    }
}
