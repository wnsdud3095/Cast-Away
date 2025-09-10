using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AnimalCtrl))]
public class AnimalAttack : MonoBehaviour
{
    private AnimalCtrl m_controller;

    private float m_atk;
    private float m_awareness_range;
    private float m_atk_range;
    private float m_atk_delay;

    public float ATK => m_atk;
    public bool CanAttack { get; protected set; }
    public float SqrMagnitude => Vector3.SqrMagnitude((m_controller.Player.transform.position - transform.position));
    public float Magnitude => Vector3.Magnitude((m_controller.Player.transform.position - transform.position));
    

    private void Awake()
    {
        m_controller = GetComponent<AnimalCtrl>();
    }

    public void Initialize(float atk,
                           float attack_delay,
                           float awareness_range,
                           float atk_range)
    {
        m_atk = atk;
        m_atk_delay = attack_delay;
        m_awareness_range = awareness_range;
        m_atk_range = atk_range;
    }

    public bool CheckTrace() => SqrMagnitude <= m_awareness_range * m_awareness_range;

    public bool CheckAttack() => SqrMagnitude <= m_atk_range * m_atk_range;

    public void CoolingDown() => StartCoroutine(ATKCoolingDown());

    private IEnumerator ATKCoolingDown()
    {
        CanAttack = false;

        yield return new WaitForSeconds(m_atk_delay);

        CanAttack = true;
    }

    public void Trace()
    {

    }
}
