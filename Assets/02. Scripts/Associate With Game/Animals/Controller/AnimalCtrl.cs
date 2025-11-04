using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(BoxCollider), typeof(Animator), typeof(NavMeshAgent))]
[RequireComponent(typeof(AnimalMovement), typeof(AnimalStatus))]
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
    public CameraShaker CameraShaker { get; protected set; }

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
                                   TimeManager time_manager,
                                   CameraShaker camera_shaker)
    {
        Player = player_ctrl;
        TimeManager = time_manager;

        Movement.Initialize(SO.IdleTime, SO.WalkSPD, SO.RunSPD, SO.MoveTime);
        Status.Initialize(SO.HP);

        ForceMode = false;

        Collider.enabled = true;
        ChangeState(AnimalState.IDLE);

        CameraShaker = camera_shaker;
    }

    public virtual void ChangeState(AnimalState state)
    {
        var target_state = state switch
        {
            AnimalState.RETURNED        => m_returned_state,
            AnimalState.IDLE            => m_idle_state,
            AnimalState.EAT             => m_eat_state,
            AnimalState.WANDER          => m_wander_state,
            AnimalState.ESCAPE          => m_escape_state,
            AnimalState.HURT            => m_hurt_state,
            AnimalState.DEATH           => m_death_state,
            _                           => null
        };

        m_state_context.Transition(target_state);
    }
}
