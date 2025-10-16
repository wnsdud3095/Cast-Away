using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FishingView : MonoBehaviour, IFishingView
{
    [Header("UI 관련 컴포넌트")]
    [Header("팝업 UI 매니저")]
    [SerializeField] private PopupUIManager m_ui_manager;

    [Header("캔버스 그룹")]
    [SerializeField] private CanvasGroup m_canvas_group;

    [Header("내부 원")]
    [SerializeField] private Image m_inner_circle;

    [Header("내부 원")]
    [SerializeField] private Image m_outer_circle;

    [Header("히트 애니메이터")]
    [SerializeField] private Animator m_hit_animator;
    
    private Coroutine m_game_coroutine;
    private FishingPresenter m_presenter;


    public void Inject(FishingPresenter presenter)
    {
        m_presenter = presenter;
    }

    public void OpenUI()
    {
        m_ui_manager.AddPresenter(m_presenter);

        m_canvas_group.alpha = 1f;
        m_canvas_group.blocksRaycasts = true;
        m_canvas_group.interactable = true;

        StartGame();
    }

    public void CloseUI()
    {
        if (m_game_coroutine != null)
        {
            StopCoroutine(m_game_coroutine);
            m_game_coroutine = null;
        }

        m_canvas_group.alpha = 0f;
        m_canvas_group.blocksRaycasts = false;
        m_canvas_group.interactable = false;

        Initialize(0f, 0f);
    }

    public void StartGame()
    {
        var inner_z = Random.Range(0f, 360f);
        Initialize(inner_z, 0f);

        var rotation_speed = Random.Range(1f, 3f);

        if(m_game_coroutine != null)
        {
            StopCoroutine(m_game_coroutine);
            m_game_coroutine = null;
        }

        m_game_coroutine = StartCoroutine(Co_Game(rotation_speed));
    }

    private void EndGame()
    {
        m_ui_manager.RemovePresenter(m_presenter);
        m_presenter.CloseUI();
    }

    private void CheckState()
    {
        var inner_z = m_inner_circle.transform.eulerAngles.z;
        var outer_z = m_outer_circle.transform.eulerAngles.z;

        float diff = Mathf.Abs(Mathf.DeltaAngle(inner_z, outer_z));

        if (diff <= 12f)
        {
            m_hit_animator.SetTrigger("Hit");
            m_presenter.GetFish();

            SoundManager.Instance.PlaySFX("Fishing Success", false, Vector3.zero);
        }
        else
        {
            SoundManager.Instance.PlaySFX("Fishing Fail", false, Vector3.zero);
        }

        StartCoroutine(Co_EndGameDelay());
    }

    private void Initialize(float inner_z, float outter_z)
    {
        m_inner_circle.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, inner_z));
        m_outer_circle.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, outter_z));
    }

    private IEnumerator Co_Game(float rotation_speed)
    {
        var current_angle = 0f;

        while(m_presenter.Gaming)
        {
            current_angle += Time.deltaTime * rotation_speed * 100f;
            current_angle %= 360f;

            m_outer_circle.transform.rotation = Quaternion.Euler(0f, 0f, current_angle);

            if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse0))
            {
                CheckState();
                m_game_coroutine = null;
                yield break;
            }

            yield return null;
        }

        m_game_coroutine = null;
    }

    private IEnumerator Co_EndGameDelay()
    {
        m_presenter.EndGame();

        yield return new WaitForSeconds(1f);
        EndGame();
    }

    public void SetDepth()
    {
        (transform as RectTransform).SetAsFirstSibling();
    }

    public void PopupCloseUI()
    {
        m_ui_manager.RemovePresenter(m_presenter);
    }
}
