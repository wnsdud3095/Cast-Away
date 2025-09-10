using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatusView : MonoBehaviour, IStatusView
{
    [Header("UI References")]
    [SerializeField] private Image m_hp_image;
    [SerializeField] private Image m_hunger_image;
    [SerializeField] private Image m_thirst_image;
    [SerializeField] private Image m_exp_image;

    [SerializeField] private TMP_Text m_hp_text;
    [SerializeField] private TMP_Text m_hunger_text;
    [SerializeField] private TMP_Text m_thirst_text;
    [SerializeField] private TMP_Text m_level_text;

    [SerializeField] private PlayerStatus m_player_status;

    private StatusPresenter m_presenter;

    private Coroutine m_hp_routine;
    private Coroutine m_thirst_routine;
    private Coroutine m_hunger_routine;
    private Coroutine m_exp_routine;

    public void Inject(StatusPresenter presenter)
    {
        this.m_presenter = presenter;
    }
    private void OnDestroy()
    {
        m_presenter.Dispose();
    }

    public void UpdateLV(int level, float exp_rate)
    {
        if (m_level_text != null)
            m_level_text.text = $"Lv.{level}";

        if (m_exp_image != null)
        {
            if (m_exp_routine != null) StopCoroutine(m_exp_routine);
            m_exp_routine = StartCoroutine(SmoothUpdate(m_exp_image, exp_rate));
        }
    }

    public void UpdateHP(float hp_rate)
    {
        if (m_hp_text != null)
            m_hp_text.text = $"{Mathf.CeilToInt(m_player_status.HP)}";

        if (m_hp_image != null)
        {
            if (m_hp_routine != null) StopCoroutine(m_hp_routine);
            m_hp_routine = StartCoroutine(SmoothUpdate(m_hp_image, hp_rate));
        }
    }

    public void UpdateThirst(float thirst_rate)
    {
        if (m_thirst_text != null)
            m_thirst_text.text = $"{Mathf.CeilToInt(Mathf.CeilToInt(m_player_status.Thirst))}";

        if (m_thirst_image != null)
        {
            if (m_thirst_routine != null) StopCoroutine(m_thirst_routine);
            m_thirst_routine = StartCoroutine(SmoothUpdate(m_thirst_image, thirst_rate));
        }
    }

    public void UpdateHunger(float hunger_rate)
    {
        if (m_hunger_text != null)
            m_hunger_text.text = $"{Mathf.CeilToInt(m_player_status.Hunger)}";

        if (m_hunger_image != null)
        {
            if (m_hunger_routine != null) StopCoroutine(m_hunger_routine);
            m_hunger_routine = StartCoroutine(SmoothUpdate(m_hunger_image, hunger_rate));
        }
    }

    private IEnumerator SmoothUpdate(Image image, float target_value)
    {
        float start_value = image.fillAmount;
        float elapsed = 0f;
        const float duration = 0.3f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            image.fillAmount = Mathf.Lerp(start_value, target_value, t);
            yield return null;
        }

        image.fillAmount = target_value;
    }
}
