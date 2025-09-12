using System.Collections;
using TMPro;
using UnityEngine;

public class AnimalNameTagView : MonoBehaviour, INameTagView
{
    [Header("캔버스 그룹")]
    [SerializeField] private CanvasGroup m_canvas_group;

    [Header("네임 태그 텍스트")]
    [SerializeField] private TMP_Text m_name_tag_label;

    [Header("네임 태그 회전 계수")]
    [SerializeField] private float m_rotation_speed = 3f;

    private Coroutine m_fade_coroutine;

    private void Update()
    {
        if (gameObject.activeSelf)
        {
            var direction = transform.position - Camera.main.transform.position;
            var target_rotation = Quaternion.LookRotation(direction);


            transform.rotation = target_rotation;
        }
    }

    public void OpenUI(string name_text)
    {
        Fade(true);
        m_name_tag_label.text = name_text;
    }

    public void CloseUI()
    {
        Fade(false);
    }

    private void Fade(bool is_in)
    {
        if(m_fade_coroutine != null)
        {
            StopCoroutine(m_fade_coroutine);
            m_fade_coroutine = null;
        }

        m_fade_coroutine = StartCoroutine(FadeGroup(is_in));
    }

    private IEnumerator FadeGroup(bool is_in)
    {
        var elapsed_time = 0f;
        var target_time = 1f;

        while(elapsed_time < target_time)
        {
            var delta = is_in ? elapsed_time / target_time : 1 - elapsed_time / target_time; 

            m_canvas_group.alpha = delta;

            elapsed_time += Time.deltaTime;
            yield return null;
        }

        m_canvas_group.alpha = is_in ? 1f : 0f;
    }
}
