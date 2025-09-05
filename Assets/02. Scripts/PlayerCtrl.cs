using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    public PlayerMovement Movement { get; private set; }

    public Rigidbody Rigidbody { get; private set; }
    public Animator Animator { get; private set; }
    public CapsuleCollider Collider { get; private set; }

    [field: SerializeField] public GameObject Model { get; private set; }

    public Vector3 Direction { get; set; }

    private void Awake()
    {
        Movement = GetComponent<PlayerMovement>();

        Rigidbody = GetComponent<Rigidbody>();
        Animator = GetComponentInChildren<Animator>();
        Collider = GetComponent<CapsuleCollider>();
    }
}