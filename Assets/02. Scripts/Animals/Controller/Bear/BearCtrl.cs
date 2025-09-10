using System.Collections;
using UnityEngine;

public class BearCtrl : AnimalCtrl
{
    private IState<AnimalCtrl> m_trace_state;
    private IState<AnimalCtrl> m_attack_state;

    public AnimalAttack Attack { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        m_hurt_state = gameObject.AddComponent<BearHurtState>();
        m_trace_state = gameObject.AddComponent<AnimalTraceState>();
        m_attack_state = gameObject.AddComponent<BearAttackState>();
    }

    public override void Initialize(Animal animal)
    {
        base.Initialize(animal);

        Attack.Initialize((animal as AggressiveAnimal).ATK,
                          (animal as AggressiveAnimal).ATKDelay,
                          (animal as AggressiveAnimal).AwarenessRange,
                          (animal as AggressiveAnimal).ATKRange);
    }

    public override void ChangeState(AnimalState state)
    {
        switch(state)
        {
            case AnimalState.TRACE:
                m_state_context.Transition(m_trace_state);
                break;

            case AnimalState.ATTACK:
                m_state_context.Transition(m_attack_state);
                break;
        }
    }
}
