using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[RequireComponent(typeof(PlayerCtrl))]
public class PlayerMovement : MonoBehaviour
{
    public bool IsDashActive { get; set; }

    private PlayerCtrl m_controller;

    [Header("카메라 리그")]
    [SerializeField] private Transform m_camera_rig;

    private float m_walk_speed = 1.5f;
    private float m_run_speed = 2.5f;

    private void Awake()
    {
        m_controller = GetComponent<PlayerCtrl>();
    }

    private void Update()
    {
        CheckInput();
    }

    private void FixedUpdate()
    {
        OnMove();
    }

    private void CheckInput()
    {
        var input_vector = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
        m_controller.Direction = input_vector.normalized;

        if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            IsDashActive = true;
        }
        else
        {
            IsDashActive = false;
        }
    }

    private void OnMove()
    {
        SetAnimation();

        var forward_direction = new Vector3(m_camera_rig.forward.x, 0f, m_camera_rig.forward.z);
        var right_direction = new Vector3(m_camera_rig.right.x, 0f, m_camera_rig.right.z);

        var final_direction = ((forward_direction * m_controller.Direction.z) +
                               (right_direction * m_controller.Direction.x)).normalized;

        var velocity = final_direction * (IsDashActive ? m_run_speed : m_walk_speed);
        m_controller.Model.transform.forward = Vector3.Lerp(m_controller.Model.transform.forward,
                                                            final_direction,
                                                            Time.deltaTime * 5f);

        var target_position = transform.position + velocity * Time.deltaTime;
        m_controller.Rigidbody.MovePosition(target_position);
    }

    private void SetAnimation()
    {
        if(m_controller.Direction.magnitude > 0f)
        {
            m_controller.Animator.applyRootMotion = false;

            m_controller.Animator.SetBool("Walking", true);
            m_controller.Animator.SetBool("Running", IsDashActive);
        }
        else
        {
            m_controller.Animator.SetBool("Walking", false);
        }
    }
}
