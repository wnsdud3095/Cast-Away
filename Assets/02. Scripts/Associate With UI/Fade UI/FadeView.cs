using System.Collections;
using UnityEngine;

public class FadeView : MonoBehaviour, IFadeView
{
    [Header("UI 관련 컴포넌트")]
    [Header("캔버스 그룹")]
    [SerializeField] private CanvasGroup m_canvas_group;

    private Coroutine m_fade_coroutine;

    FadePresenter m_presenter;

    public void Inject(FadePresenter presenter)
    {
        m_presenter = presenter;
    }

    public void Fade(bool is_in)
    {
        if(m_fade_coroutine != null)
        {
            StopCoroutine(m_fade_coroutine);
            m_fade_coroutine = null;
        }

        m_fade_coroutine = StartCoroutine(Co_Fade(is_in)); 
    }

    private IEnumerator Co_Fade(bool is_in)
    {
        m_canvas_group.blocksRaycasts = is_in;
        m_canvas_group.interactable = is_in;

        float elapsed_time = 0f;
        float target_time = 1f;

        while(elapsed_time < target_time)
        {
            elapsed_time += Time.deltaTime;

            var delta = is_in ? elapsed_time / target_time : 1f - (elapsed_time - target_time);
            m_canvas_group.alpha = delta;

            yield return null;
        }

        m_canvas_group.alpha = is_in ? 1f : 0f;

        if(!is_in)
        {
            m_presenter.Alert();
        }
    }
}
