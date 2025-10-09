using UnityEngine;
using System.Collections.Generic;
using KeyService;

public class PopupUIManager : MonoBehaviour
{
    private IKeyService m_key_service;

    private LinkedList<IPopupPresenter> m_active_popup_list;		// 활성화 된 UI 프레젠터를 저장할 연결 리스트
    private Dictionary<string, IPopupPresenter> m_presenter_dict;	// 팝업 UI 프레젠터를 전부 보관하는 딕셔너리

    private void Awake()
    {
        m_key_service = ServiceLocator.Get<IKeyService>();
        m_active_popup_list = new();
    }

    private void Update()
    {
        // "Pause"키는 KeyCode.Escape에 매핑되어 있다.
        // ESC 키를 눌렀을 때 발생하는 행동을 정의한다.
        if (Input.GetKeyDown(m_key_service.GetKeyCode("Pause")))
        {
            // 활성화된 팝업 UI가 있다면 이들을 켜진 순서와 반대로 비활성화한다.
            if (m_active_popup_list.Count > 0)
            {
                CloseUI(m_active_popup_list.First.Value);
            }
            else
            {
                // 활성화된 팝업 UI가 없다면 일시정지 UI를 활성화시킨다.
                if (m_presenter_dict.TryGetValue("Pause", out var presenter))
                {
                    OpenUI(presenter);
                    GameEventBus.Publish(GameEventType.PAUSE);
                    // SETTING 모드로 변경. 게임매니저로 게임 스테이트 관리시에 추가로 작성
                }
            }
        }
        
        // SETTING일 때는 키 입력을 받지 않는 것이 일반적이다.
        //if (GameManager.Instance.Event != GameEventType.SETTING)
        {
            // 각 팝업 UI에 해당하는 문자열을 통하여 키 입력을 대기한다.
            
        }
        InputToggleKey("Binder");
        InputToggleKey("Crafting");
        InputToggleKey("Inventory");

    }

    // 팝업 UI 프레젠터의 목록을 Inject()를 통해 주입받는다.
    public void Inject(List<PopupData> popup_data_list)
    {
        m_presenter_dict = new();

        // 팝업 UI 프레젠터의 목록을 이용하여 딕셔너리를 초기화한다.
        foreach (var popup_data in popup_data_list)
        {
            m_presenter_dict.TryAdd(popup_data.Name, popup_data.Presenter);
        }
    }

    // 키 입력을 통하여 활성화/비활성화 여부를 결정한다.
    private void InputToggleKey(string key_name)
    {
        if (Input.GetKeyDown(m_key_service.GetKeyCode(key_name)))
        {
            if (m_presenter_dict.TryGetValue(key_name, out var presenter))
            {
                ToggleUI(presenter);
            }
        }
    }

    private void ToggleUI(IPopupPresenter presenter)
    {
        // 연결 리스트에 프레젠터가 포함되어 있다면 그 팝업 UI는 활성화 상태다.
        if (m_active_popup_list.Contains(presenter))
        {
            CloseUI(presenter);
        }
        else
        {
            OpenUI(presenter);
        }

        SortDepth();
    }

    // 어댑터를 통해서 UI를 활성화시킬 수 없는 경우에 사용한다.
    public void AddPresenter(IPopupPresenter presenter)
    {
        if (m_active_popup_list.Contains(presenter))		// 활성화되어 있다면 우선순위를 최고로 올리고
        {
            m_active_popup_list.Remove(presenter);
        }

        m_active_popup_list.AddFirst(presenter);
        GameEventBus.Publish(GameEventType.INTERACTING);

        SortDepth();										// UI 깊이 순서를 재정렬한다.
    }

    // 어댑터를 통해서 UI를 비활성화시킬 수 없는 경우에 사용한다.
    public void RemovePresenter(IPopupPresenter presenter)
    {
        if (m_active_popup_list.Contains(presenter))		// 활성화되어 있다면
        {
            m_active_popup_list.Remove(presenter);			// 비활성화하고
            SortDepth();									// UI 깊이 순서를 재정렬한다.

            if (m_active_popup_list.Count == 0)				// 다 꺼져 있다면 PLAYING 모드로 변경한다.
            {
                GameEventBus.Publish(GameEventType.INPLAY);
            }
        }
    }

    // 연결 리스트에 프레젠터를 추가하고 UI를 활성화한다.
    public void OpenUI(IPopupPresenter presenter)
    {
        m_active_popup_list.AddFirst(presenter);
        presenter.OpenUI();

        GameEventBus.Publish(GameEventType.INTERACTING);
    }

    // 연결 리스트에서 프레젠터를 삭제하고 UI를 비활성화한다.
    public void CloseUI(IPopupPresenter presenter)
    {
        m_active_popup_list.Remove(presenter);
        presenter.CloseUI();

        if (m_active_popup_list.Count == 0)
        {
            GameEventBus.Publish(GameEventType.INPLAY);
        }
    }

    // UI 깊이 순서를 재정렬한다.
    private void SortDepth()
    {
        foreach (var presenter in m_active_popup_list)
        {
            presenter.SortDepth();
        }
    }
}
