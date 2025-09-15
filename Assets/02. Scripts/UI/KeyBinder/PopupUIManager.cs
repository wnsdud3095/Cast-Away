using UnityEngine;
using System.Collections.Generic;
using KeyService;

public class PopupUIManager : MonoBehaviour
{
    private IKeyService m_key_service;

    private LinkedList<IPopupPresenter> m_active_popup_list;		// Ȱ��ȭ �� UI �������͸� ������ ���� ����Ʈ
    private Dictionary<string, IPopupPresenter> m_presenter_dict;	// �˾� UI �������͸� ���� �����ϴ� ��ųʸ�

    private void Awake()
    {
        m_key_service = ServiceLocator.Get<IKeyService>();
        m_active_popup_list = new();
    }

    private void Update()
    {
        // "Pause"Ű�� KeyCode.Escape�� ���εǾ� �ִ�.
        // ESC Ű�� ������ �� �߻��ϴ� �ൿ�� �����Ѵ�.
        if (Input.GetKeyDown(m_key_service.GetKeyCode("Pause")))
        {
            // Ȱ��ȭ�� �˾� UI�� �ִٸ� �̵��� ���� ������ �ݴ�� ��Ȱ��ȭ�Ѵ�.
            if (m_active_popup_list.Count > 0)
            {
                CloseUI(m_active_popup_list.First.Value);
            }
            else
            {
                // Ȱ��ȭ�� �˾� UI�� ���ٸ� �Ͻ����� UI�� Ȱ��ȭ��Ų��.
                if (m_presenter_dict.TryGetValue("Pause", out var presenter))
                {
                    OpenUI(presenter);
                    // SETTING ���� ����. ���ӸŴ����� ���� ������Ʈ �����ÿ� �߰��� �ۼ�
                }
            }
        }
        
        // SETTING�� ���� Ű �Է��� ���� �ʴ� ���� �Ϲ����̴�.
        //if (GameManager.Instance.Event != GameEventType.SETTING)
        {
            // �� �˾� UI�� �ش��ϴ� ���ڿ��� ���Ͽ� Ű �Է��� ����Ѵ�.
            
        }
        InputToggleKey("Binder");
        InputToggleKey("Crafting");
        InputToggleKey("Inventory");

    }

    // �˾� UI ���������� ����� Inject()�� ���� ���Թ޴´�.
    public void Inject(List<PopupData> popup_data_list)
    {
        m_presenter_dict = new();

        // �˾� UI ���������� ����� �̿��Ͽ� ��ųʸ��� �ʱ�ȭ�Ѵ�.
        foreach (var popup_data in popup_data_list)
        {
            m_presenter_dict.TryAdd(popup_data.Name, popup_data.Presenter);
        }
    }

    // Ű �Է��� ���Ͽ� Ȱ��ȭ/��Ȱ��ȭ ���θ� �����Ѵ�.
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
        // ���� ����Ʈ�� �������Ͱ� ���ԵǾ� �ִٸ� �� �˾� UI�� Ȱ��ȭ ���´�.
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

    // ����͸� ���ؼ� UI�� Ȱ��ȭ��ų �� ���� ��쿡 ����Ѵ�.
    public void AddPresenter(IPopupPresenter presenter)
    {
        if (m_active_popup_list.Contains(presenter))		// Ȱ��ȭ�Ǿ� �ִٸ� �켱������ �ְ�� �ø���
        {
            m_active_popup_list.Remove(presenter);
        }

        m_active_popup_list.AddFirst(presenter);
        //GameEventBus.Publish(GameEventType.INTERACTING);

        SortDepth();										// UI ���� ������ �������Ѵ�.
    }

    // ����͸� ���ؼ� UI�� ��Ȱ��ȭ��ų �� ���� ��쿡 ����Ѵ�.
    public void RemovePresenter(IPopupPresenter presenter)
    {
        if (m_active_popup_list.Contains(presenter))		// Ȱ��ȭ�Ǿ� �ִٸ�
        {
            m_active_popup_list.Remove(presenter);			// ��Ȱ��ȭ�ϰ�
            SortDepth();									// UI ���� ������ �������Ѵ�.

            if (m_active_popup_list.Count == 0)				// �� ���� �ִٸ� PLAYING ���� �����Ѵ�.
            {
                //GameEventBus.Publish(GameEventType.PLAYING);
            }
        }
    }

    // ���� ����Ʈ�� �������͸� �߰��ϰ� UI�� Ȱ��ȭ�Ѵ�.
    public void OpenUI(IPopupPresenter presenter)
    {
        m_active_popup_list.AddFirst(presenter);
        presenter.OpenUI();

        //GameEventBus.Publish(GameEventType.INTERACTING);
    }

    // ���� ����Ʈ���� �������͸� �����ϰ� UI�� ��Ȱ��ȭ�Ѵ�.
    public void CloseUI(IPopupPresenter presenter)
    {
        m_active_popup_list.Remove(presenter);
        presenter.CloseUI();

        if (m_active_popup_list.Count == 0)
        {
            //GameEventBus.Publish(GameEventType.PLAYING);
        }
    }

    // UI ���� ������ �������Ѵ�.
    private void SortDepth()
    {
        foreach (var presenter in m_active_popup_list)
        {
            presenter.SortDepth();
        }
    }
}
