using UnityEngine;

[RequireComponent(typeof(Animator))]
public class RealviewObject : MonoBehaviour
{
    private Animator m_animator;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
    }

    private void Start()
    {
        m_animator.SetTrigger("Instantiate");
    }
}
