using System.Collections.Generic;
using UnityEngine;

public class PreviewObject : MonoBehaviour
{
    private readonly List<Collider> m_collider_list = new();
    private Vector3 m_snap_position;

    private Vector2 m_accumulate_move;
    private float m_unsnap_threshold = 10f;


    [Header("지형 레이어")]
    [SerializeField] private LayerMask m_layer_mask;

    [Header("초록 머테리얼")]
    [SerializeField] private Material m_green_mat;

    [Header("빨간 머테리얼")]
    [SerializeField] private Material m_red_mat;

    public bool Buildable => m_collider_list.Count == 0;
    public bool IsSnapped { get; private set; }
    public Vector3 SnapPosition => m_snap_position;

    private void Update()
    {
        ChangeColor();
    }

    public void SnapTo(Vector3 position)
    {
        if (IsSnapped) 
        {
            return;
        }

        m_snap_position = position;
        IsSnapped = true;

        transform.position = m_snap_position;
    }

    public void TryUnsnap()
    {
        if (!IsSnapped) 
        {
            return;
        }

        m_accumulate_move += new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

        if (m_accumulate_move.magnitude > m_unsnap_threshold)
        {
            Unsnap();
        }
    }

    public void Unsnap()
    {
        m_accumulate_move = Vector2.zero;
        IsSnapped = false;
    }

    private void ChangeColor()
    {
        if(Buildable)
        {
            SetMaterial(m_green_mat);
        }
        else
        {
            SetMaterial(m_red_mat);
        }
    }

    private void SetMaterial(Material mat)
    {
        transform.GetComponent<Renderer>().material = mat;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if(!IsMask(collider))
        {
            m_collider_list.Add(collider);
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if(!IsMask(collider))
        {
            m_collider_list.Remove(collider);
        }        
    }

    private bool IsMask(Collider collider)
    {
        return (m_layer_mask & (1 << collider.gameObject.layer)) != 0;
    }
}
