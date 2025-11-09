using System.Collections.Generic;
using UnityEngine;

public class CraftUnlockTrigger : MonoBehaviour
{
    [Header("잠금 해제할 레시피 코드 리스트")]
    [SerializeField] private List<ItemCode> m_item_codes = new();

    public event System.Action<List<ItemCode>, CraftUnlockTrigger> OnPlayerEnter;
    public event System.Action<CraftUnlockTrigger> OnPlayerExit;

    private CraftPresenter m_craft_presenter;

    private void OnDestroy()
    {
        OnPlayerExit?.Invoke(this);
        m_craft_presenter?.UnsubscribeTrigger(this);
    }

    public void Inject(CraftPresenter craft_presenter)
    {
        m_craft_presenter = craft_presenter;
        m_craft_presenter?.SubscribeTrigger(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        OnPlayerEnter?.Invoke(m_item_codes, this);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        OnPlayerExit?.Invoke(this);
    }
}
