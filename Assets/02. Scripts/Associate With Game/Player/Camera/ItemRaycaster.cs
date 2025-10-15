using InventoryService;
using KeyService;
using UnityEngine;

public class ItemRaycaster : MonoBehaviour
{
    [Header("레이의 길이")]
    [SerializeField] private float m_ray_length;

    [Header("레이가 감지할 레이어")]
    [SerializeField] private LayerMask m_layer_mask;

    private IKeyService m_key_service;
    private IInventoryService m_inventory_service;
    private IItemObjectConverter m_item_object_converter;
    private ItemDetectorPresenter m_item_detector;

    public void Inject(IKeyService key_service,
                       IInventoryService inventory_service,
                       IItemObjectConverter item_object_converter,
                       ItemDetectorPresenter item_detector)
    {
        m_key_service = key_service;
        m_inventory_service = inventory_service;

        m_item_object_converter = item_object_converter;
        m_item_detector = item_detector;
    }

    private void Update()
    {
        var center = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
        var ray = Camera.main.ScreenPointToRay(center);
        Debug.DrawRay(ray.origin, ray.direction * m_ray_length, Color.red);

        if(Physics.Raycast(ray, out var hit, m_ray_length, m_layer_mask))
        {
            var field_item = hit.collider.GetComponent<FieldItem>();

            var world_position = field_item.transform.position + Vector3.up;
            m_item_detector.OpenUI(field_item.Name, 
                                   new System.Numerics.Vector3(world_position.x, 
                                                               world_position.y, 
                                                               world_position.z));

            if(Input.GetKeyDown(m_key_service.GetKeyCode("PickUp")))
            {
                SoundManager.Instance.PlaySFX("Pick Up", true, transform.position);

                m_inventory_service.AddItem(field_item.Code, 1);
                ObjectManager.Instance.ReturnObject(field_item.gameObject, 
                                                    m_item_object_converter.GetObjectType(field_item.Code));

                m_item_detector.CloseUI();
            }
        }
        else
        {
            m_item_detector.CloseUI();
        }
    }
}
