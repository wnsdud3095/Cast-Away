using InventoryService;
using UnityEngine;
using UserService;

public class Moduler : MonoBehaviour
{
    private IInventoryService m_inventory_service;
    private IUserService m_user_service;
    private IModuleDataBase m_module_db;
    private ModulerTutorialPresenter m_moduler_tutorial_presenter;
    private CraftReceipe m_module_receipe;
    private CameraShaker m_camera_shaker;
    private IItemObjectConverter m_item_object_converter;
    private CraftPresenter m_craft_presenter;

    private bool m_is_active;

    private GameObject m_preview_object;
    private GameObject m_realview_object;

    private PreviewObject m_preview;

    [Header("지형 레이어")]
    [SerializeField] LayerMask m_layer_mask;

    [Header("모듈러 레이의 길이")]
    [SerializeField] private float m_ray_length;

    private void Update()
    {
        if(!m_is_active || m_preview_object == null)
        {
            return;
        }

        Translation();

        if(Input.GetKeyDown(KeyCode.R))
        {
            var current_rotation = m_preview_object.transform.eulerAngles; 
            m_preview_object.transform.rotation = Quaternion.Euler(current_rotation.x,
                                                                    current_rotation.y + 90f,
                                                                    current_rotation.z);
        }

        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            Build();
        }

        if(Input.GetKeyDown(KeyCode.C))
        {
            Deactivate(true);
        }
    }

    public void Inject(IModuleDataBase module_db,
                       IInventoryService inventory_service,
                       IUserService user_service,
                       ModulerTutorialPresenter moduler_tutorial_presenter,
                       CameraShaker camera_shaker,
                       IItemObjectConverter item_object_converter,
                       CraftPresenter craft_presenter)
    {
        m_module_db = module_db;
        m_inventory_service = inventory_service;
        m_user_service = user_service;
        m_moduler_tutorial_presenter = moduler_tutorial_presenter;
        m_camera_shaker = camera_shaker;
        m_item_object_converter = item_object_converter;
        m_craft_presenter = craft_presenter;
    }

    public void Activate(CraftReceipe craft_receipe)
    {
        var center = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
        var ray = Camera.main.ScreenPointToRay(center);

        var module = m_module_db.GetModule(craft_receipe.Code);
        m_module_receipe = craft_receipe;

        m_preview_object = Instantiate(module.PreviewPrefab, ray.GetPoint(m_ray_length), Quaternion.identity);
        m_realview_object = module.RealviewPrefab;

        m_preview = m_preview_object.GetComponent<PreviewObject>();

        m_is_active = true;
    }

    public void Deactivate(bool change_state)
    {
        if(m_is_active)
        {
            Destroy(m_preview_object);
        }

        m_preview_object = null;
        m_realview_object = null;
        m_preview = null;

        m_is_active = false;
        m_moduler_tutorial_presenter.CloseUI();

        if(change_state)
        {
            GameEventBus.Publish(GameEventType.INPLAY);
        }
    }

    private void Translation()
    {
        if (m_preview != null && m_preview.IsSnapped)
        {
            m_preview.TryUnsnap();

            if (m_preview.IsSnapped)
            {
                m_preview_object.transform.position = m_preview.SnapPosition;
                return;
            }
        }

        var center = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
        var ray = Camera.main.ScreenPointToRay(center);

        m_preview_object.transform.position = ray.GetPoint(m_ray_length);
    }

    private void Build()
    {
        if(GameManager.Instance.GameType != GameEventType.CRAFTING || !m_is_active)
            return;

        var preview = m_preview_object.GetComponent<PreviewObject>();
        if(!preview.Buildable)
            return;

        var realview_obj = Instantiate(m_realview_object, m_preview_object.transform.position, Quaternion.identity);
        realview_obj.transform.SetPositionAndRotation(m_preview_object.transform.position, Quaternion.identity);

        var realview_transform = realview_obj.GetComponentInChildren<RealviewObject>().transform;
        realview_transform.rotation = m_preview_object.transform.rotation;

        var breakable_obj = realview_obj.GetComponent<BreakableBuilding>();
        breakable_obj.Inject(m_camera_shaker, m_item_object_converter);

        var unlock_trigger = realview_obj.GetComponentInChildren<CraftUnlockTrigger>();
        if(unlock_trigger != null)
        {
            unlock_trigger.Inject(m_craft_presenter);
        }

        SoundManager.Instance.PlaySFX("Build", true, m_preview_object.transform.position);
        ConsumeIngredients();
        m_user_service.UpdateLevel(m_module_receipe.EXP);

        if(!CanBuild())
        {
            Deactivate(true);
            return;
        }
    }

    private void ConsumeIngredients()
    {
        foreach(var ingredient in m_module_receipe.Ingredients)
        {
            m_inventory_service.RemoveItem(ingredient.Item.Code, ingredient.Count);
        }        
    }

    private bool CanBuild()
    {
        foreach(var ingredient in m_module_receipe.Ingredients)
        {
            if(m_inventory_service.GetItemCount(ingredient.Item.Code) < ingredient.Count)
            {
                return false;
            }
        }

        return true;
    }
}
