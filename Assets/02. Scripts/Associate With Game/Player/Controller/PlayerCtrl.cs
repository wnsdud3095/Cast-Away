using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    #region FSM States
    private PlayerStateContext m_state_context;

    private IState<PlayerCtrl> m_idle_state;
    private IState<PlayerCtrl> m_walk_state;
    private IState<PlayerCtrl> m_run_state;
    private IState<PlayerCtrl> m_work_state;
    #endregion FSM States

    public PlayerMovement Movement { get; private set; }
    public PlayerStatus State { get; private set; }

    public Rigidbody Rigidbody { get; private set; }
    public Animator Animator { get; private set; }
    public CapsuleCollider Collider { get; private set; }

    [field: SerializeField] public GameObject Model { get; private set; }

    public Vector3 Direction { get; set; }

    private void Awake()
    {
        Movement = GetComponent<PlayerMovement>();
        State = GetComponent<PlayerStatus>();

        Rigidbody = GetComponent<Rigidbody>();
        Animator = GetComponentInChildren<Animator>();
        Collider = GetComponent<CapsuleCollider>();

        m_state_context = new PlayerStateContext(this);

        m_idle_state = gameObject.AddComponent<PlayerIdleState>();
        m_walk_state = gameObject.AddComponent<PlayerWalkState>();
        m_run_state = gameObject.AddComponent<PlayerRunState>();
        m_work_state = gameObject.AddComponent<PlayerWorkState>();

        ChangeState(PlayerState.IDLE);
    }

    private void Update()
    {
        m_state_context?.UpdateState();
    }

    private void FixedUpdate()
    {
        m_state_context?.FixedUpdateState();
    }

    public void ChangeState(PlayerState state)
    {
        var target_state = state switch
        {
            PlayerState.IDLE        => m_idle_state,
            PlayerState.WALK        => m_walk_state,
            PlayerState.RUN         => m_run_state,
            PlayerState.WORK        => m_work_state,
            _                       => null
        };

        m_state_context.Transition(target_state);
    }
}