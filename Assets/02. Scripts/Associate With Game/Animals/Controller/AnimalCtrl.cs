using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(BoxCollider), typeof(Animator), typeof(NavMeshAgent))]
public class AnimalCtrl : MonoBehaviour
{
    #region FSM States
    protected AnimalStateContext m_state_context;

    protected IState<AnimalCtrl> m_returned_state;
    private IState<AnimalCtrl> m_idle_state;
    private IState<AnimalCtrl> m_eat_state;
    private IState<AnimalCtrl> m_wander_state;
    private IState<AnimalCtrl> m_escape_state;
    protected IState<AnimalCtrl> m_hurt_state;
    private IState<AnimalCtrl> m_death_state;
    #endregion FSM States

    public bool ForceMode { get; set; }

    public BoxCollider Collider { get; protected set; }
    public Animator Animator { get; protected set; }
    public NavMeshAgent Agent { get; protected set; }

    public AnimalMovement Movement { get; private set; }
    public AnimalStatus Status { get; private set; }

    [field: SerializeField] public Animal SO { get; private set; }
    public PlayerCtrl Player { get; protected set; }
    public TimeManager TimeManager { get; protected set; }

    protected virtual void Awake()
    {
        Collider = GetComponent<BoxCollider>();
        Animator = GetComponent<Animator>();
        Agent = GetComponent<NavMeshAgent>();

        Movement = GetComponent<AnimalMovement>();
        Status = GetComponent<AnimalStatus>();

        m_state_context = new AnimalStateContext(this);

        m_idle_state = gameObject.AddComponent<AnimalIdleState>();
        m_eat_state = gameObject.AddComponent<AnimalEatState>();
        m_wander_state = gameObject.AddComponent<AnimalWanderState>();
        m_escape_state = gameObject.AddComponent<AnimalEscapeState>();
        m_hurt_state = gameObject.AddComponent<AnimalHurtState>();
        m_death_state = gameObject.AddComponent<AnimalDeathState>();
    }

    public virtual void Initialize(PlayerCtrl player_ctrl,
                                   TimeManager time_manager)
    {
        Player = player_ctrl;
        TimeManager = time_manager;

        Movement.Initialize(SO.IdleTime, SO.WalkSPD, SO.RunSPD, SO.MoveTime);
        Status.Initialize(SO.HP);

        ForceMode = false;

        Collider.enabled = true;
        ChangeState(AnimalState.IDLE);
    }

    public virtual void ChangeState(AnimalState state)
    {
        switch(state)
        {
            case AnimalState.RETURNED:
                m_state_context.Transition(m_returned_state);
                break;

            case AnimalState.IDLE:
                m_state_context.Transition(m_idle_state);
                break;
            
            case AnimalState.EAT:
                m_state_context.Transition(m_eat_state);
                break;
            
            case AnimalState.WANDER:
                m_state_context.Transition(m_wander_state);
                break;

            case AnimalState.ESCAPE:
                m_state_context.Transition(m_escape_state);
                break;
            
            case AnimalState.HURT:
                m_state_context.Transition(m_hurt_state);
                break;
            
            case AnimalState.DEATH:
                m_state_context.Transition(m_death_state);
                break;
        }
    }
}
