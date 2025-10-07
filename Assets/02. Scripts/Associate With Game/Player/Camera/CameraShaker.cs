using System.Collections;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    private Vector3 m_origin_position;
    private Coroutine m_shaker_coroutine;

    public void Shaking(float magnitude, float duration)
    {
        if(m_shaker_coroutine != null)
        {
            StopCoroutine(m_shaker_coroutine);
            m_shaker_coroutine = null;
        }
        
        m_shaker_coroutine = StartCoroutine(Co_Shake(magnitude, duration));
    }

    private IEnumerator Co_Shake(float magnitude, float duration)
    {
        m_origin_position = transform.localPosition;

        var elapsed_time = 0f; 
        var target_time = duration;

        while(elapsed_time < target_time)
        {
            elapsed_time += Time.deltaTime / target_time;

            var x = Random.Range(-1f, 1f) * magnitude;
            var y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(m_origin_position.x + x, 
                                                  m_origin_position.y + y, 
                                                  m_origin_position.z);

            yield return null;
        }

        transform.localPosition = m_origin_position;
        m_shaker_coroutine = null;
    }
}
