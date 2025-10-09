using System.Collections;
using TMPro;
using UnityEngine;

public class ItemDetectorView : MonoBehaviour, IItemDetectorView
{
    [Header("캔버스 그룹")]
    [SerializeField] private CanvasGroup m_canvas_group;

    [Header("디텍터 텍스트")]
    [SerializeField] private TMP_Text m_item_text;

    [Header("플레이어 컨트롤러")]
    [SerializeField] private PlayerCtrl m_player_ctrl;

    private Coroutine m_fade_coroutine;
    private ItemDetectorPresenter m_presenter;

    public void Update()
    {
        if(m_presenter.Active)
        {
            m_canvas_group.transform.rotation = Quaternion.LookRotation(m_canvas_group.transform.position - Camera.main.transform.position);
        }
    }

    public void Inject(ItemDetectorPresenter presenter)
    {
        m_presenter = presenter;
    }

    public void OpenUI()
    {
        ToggleFade(true);
    }

    public void UpdateUI(string item_name, System.Numerics.Vector3 position)
    {
        m_item_text.text = item_name;
        m_canvas_group.transform.position = new Vector3(position.X, position.Y, position.Z);
    }

    public void CloseUI()
    {
        m_item_text.text = string.Empty;
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
            var delta = is_in ? elapsed_time / target_time : 1 - elapsed_time / target_time; 

            m_canvas_group.alpha = delta;

            elapsed_time += Time.deltaTime;
            yield return null;
        }

        m_canvas_group.alpha = is_in ? 1f : 0f;
    }
}
