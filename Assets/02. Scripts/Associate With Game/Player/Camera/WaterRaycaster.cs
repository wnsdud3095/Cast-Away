using InventoryService;
using KeyService;
using UnityEngine;

public class WaterRaycaster : MonoBehaviour
{
    [Header("레이의 길이")]
    [SerializeField] private float m_ray_length;

    [Header("레이가 감지할 레이어")]
    [SerializeField] private LayerMask m_layer_mask;

    private IKeyService m_key_service;
    private IInventoryService m_inventory_service;
    private NoticePresenter m_notice_presenter;
    private FishingPresenter m_fishing_presenter;
    private ItemSwapper m_item_swapper;

    public void Inject(IKeyService key_service,
                       IInventoryService inventory_service,
                       NoticePresenter notice_presenter,
                       FishingPresenter fishing_presenter,
                       ItemSwapper item_swapper)
    {
        m_key_service = key_service;
        m_inventory_service = inventory_service;
        
        m_notice_presenter = notice_presenter;
        m_fishing_presenter = fishing_presenter;

        m_item_swapper = item_swapper;
    }

    private void Update()
    {
        if(!m_inventory_service.HasItem(ItemCode.FISHING_ROD))
        {
            m_notice_presenter.CloseUI();
            return;
        }

        if(m_item_swapper.CurrentTool is not FishingRod)
        {
            m_notice_presenter.CloseUI();
            return;
        }

        if(m_fishing_presenter.Active)
        {
            m_notice_presenter.CloseUI();
            return;
        }

        var center = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
        var ray = Camera.main.ScreenPointToRay(center);
        Debug.DrawRay(ray.origin, ray.direction * m_ray_length, Color.green);

        if(Physics.Raycast(ray, out var hit, m_ray_length, m_layer_mask))
        {
            if(hit.collider.CompareTag("Water"))
            {
                m_notice_presenter.OpenUI($"낚시를 하려면 [{m_key_service.GetKeyCode("PickUp").ToString().ToUpper()}]를 누르세요.");

                if(Input.GetKeyDown(m_key_service.GetKeyCode("PickUp")))
                {
                    m_fishing_presenter.OpenUI();
                }
            }
            else
            {
                m_notice_presenter.CloseUI();
            }
        }
        else
        {
            m_notice_presenter.CloseUI();
        }
    }
}
