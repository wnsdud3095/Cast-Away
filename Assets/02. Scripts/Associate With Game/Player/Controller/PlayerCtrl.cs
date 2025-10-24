using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    #region FSM States
    private PlayerStateContext m_state_context;

    private IState<PlayerCtrl> m_idle_state;
    private IState<PlayerCtrl> m_walk_state;
    private IState<PlayerCtrl> m_run_state;
    private IState<PlayerCtrl> m_work_state;
    private IState<PlayerCtrl> m_attack_state;
    private IState<PlayerCtrl> m_fishing_state;
    #endregion FSM States

    public PlayerMovement Movement { get; private set; }
    public PlayerStatus State { get; private set; }

    public Rigidbody Rigidbody { get; private set; }
    public Animator Animator { get; private set; }
    public CapsuleCollider Collider { get; private set; }

    [field: SerializeField] public GameObject Model { get; private set; }

    public Vector3 Direction { get; set; }
    public bool Interacting { get; set; }

    private void OnEnable()
    {
        GameEventBus.Subscribe(GameEventType.INPLAY, GameManager.Instance.InPlay);
        GameEventBus.Subscribe(GameEventType.INTERACTING, GameManager.Instance.Interacting);
        GameEventBus.Subscribe(GameEventType.CRAFTING, GameManager.Instance.Crafting);
        GameEventBus.Subscribe(GameEventType.PAUSE, GameManager.Instance.Pause);
        GameEventBus.Subscribe(GameEventType.GAMEOVER, GameManager.Instance.GameOver);
        GameEventBus.Subscribe(GameEventType.GAMECLEAR, GameManager.Instance.GameClear);

        GameEventBus.Publish(GameEventType.INPLAY);
    }

    private void OnDisable()
    {
        GameEventBus.Unsubscribe(GameEventType.INPLAY, GameManager.Instance.InPlay);
        GameEventBus.Unsubscribe(GameEventType.INTERACTING, GameManager.Instance.Interacting);
        GameEventBus.Unsubscribe(GameEventType.CRAFTING, GameManager.Instance.Crafting);
        GameEventBus.Unsubscribe(GameEventType.PAUSE, GameManager.Instance.Pause);
        GameEventBus.Unsubscribe(GameEventType.GAMEOVER, GameManager.Instance.GameOver);
        GameEventBus.Unsubscribe(GameEventType.GAMECLEAR, GameManager.Instance.GameClear);
    }

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
        m_attack_state = gameObject.AddComponent<PlayerAttackState>();
        m_fishing_state = gameObject.AddComponent<PlayerFishingState>();

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
            PlayerState.ATTACK      => m_attack_state,
            PlayerState.Fishing     => m_fishing_state,
            _                       => null
        };

        m_state_context.Transition(target_state);
    }

    public void InstantiateNotice(string notice_text)
    {
        var notice_obj = ObjectManager.Instance.GetObject(ObjectType.POP_UP_NOTICE);
        var notice_ui = notice_obj.GetComponent<PopupNoticeView>();
        notice_ui.SetLabel(notice_text);
    }
}