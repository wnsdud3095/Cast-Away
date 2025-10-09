using System.Collections;
using TMPro;
using UnityEngine;

public class NoticeView : MonoBehaviour, INoticeView
{
    [Header("UI 관련 컴포넌트")]
    [Header("캔버스 그룹")]
    [SerializeField] private CanvasGroup m_canvas_group;

    [Header("알림 텍스트")]
    [SerializeField] private TMP_Text m_notice_label;

    private Coroutine m_fade_coroutine;

    public void OpenUI()
    {
        ToggleFade(true);
    }

    public void UpdateUI(string notice_text)
    {
        m_notice_label.text = notice_text;
    }

    public void CloseUI()
    {
        ToggleFade(false);
    }

    private void ToggleFade(bool is_in)
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
            var delta = is_in ? elapsed_time / target_time : 1f - elapsed_time / target_time; 

            m_canvas_group.alpha = delta;

            elapsed_time += Time.deltaTime;
            yield return null;
        }

        m_canvas_group.alpha = is_in ? 1f : 0f;
    }
}
