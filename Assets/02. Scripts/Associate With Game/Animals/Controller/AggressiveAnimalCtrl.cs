public class AggressiveAnimalCtrl : AnimalCtrl
{
    private IState<AnimalCtrl> m_trace_state;
    private IState<AnimalCtrl> m_attack_state;

    public AnimalAttack Attack { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        m_hurt_state = gameObject.AddComponent<AggressiveAnimalHurtState>();
        m_trace_state = gameObject.AddComponent<AnimalTraceState>();
        m_attack_state = gameObject.AddComponent<AnimalAttackState>();
        m_returned_state = gameObject.AddComponent<AggressiveAnimalReturnedState>();

        Attack = GetComponent<AnimalAttack>();
    }

    private void OnDisable()
    {
        if(TimeManager != null)
        {
            TimeManager.OnSunrise -= Movement.ReturnAtSunrise;
        }
    }

    public override void Initialize(PlayerCtrl player_ctrl, 
                                    TimeManager time_manager)
    {
        base.Initialize(player_ctrl, time_manager);

        Attack.Initialize((SO as AggressiveAnimal).ATK,
                          (SO as AggressiveAnimal).ATKRange);

        TimeManager.OnSunrise += Movement.ReturnAtSunrise;
    }

    public override void ChangeState(AnimalState state)
    {
        base.ChangeState(state);
        
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
