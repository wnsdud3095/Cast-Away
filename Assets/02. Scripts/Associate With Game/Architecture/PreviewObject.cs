using System.Collections.Generic;
using UnityEngine;

public class PreviewObject : MonoBehaviour
{
    private readonly List<Collider> m_collider_list = new();

    [Header("지형 레이어")]
    [SerializeField] private LayerMask m_layer_mask;

    [Header("초록 머테리얼")]
    [SerializeField] private Material m_green_mat;

    [Header("빨간 머테리얼")]
    [SerializeField] private Material m_red_mat;

    public bool Buildable => m_collider_list.Count == 0;

    private void Update()
    {
        ChangeColor();
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
