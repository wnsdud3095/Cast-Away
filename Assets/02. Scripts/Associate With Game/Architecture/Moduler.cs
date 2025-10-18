using InventoryService;
using UnityEngine;

public class Moduler : MonoBehaviour
{
    private IInventoryService m_inventory_service;
    private IModuleDataBase m_module_db;
    private ModulerTutorialPresenter m_moduler_tutorial_presenter;
    private ModuleReceipe m_module_receipe;

    private bool m_is_active;

    private GameObject m_preview_object;
    private GameObject m_realview_object;

    [Header("지형 레이어")]
    [SerializeField] LayerMask m_layer_mask;

    [Header("모듈러 레이의 길이")]
    [SerializeField] private float m_ray_length;

    private void Update()
    {
        if(!m_is_active)
        {
            return;
        }

        if(!CanBuild())
        {
            Deactivate();
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
            Deactivate();
        }
    }

    public void Inject(IModuleDataBase module_db,
                       IInventoryService inventory_service,
                       ModulerTutorialPresenter moduler_tutorial_presenter)
    {
        m_module_db = module_db;
        m_inventory_service = inventory_service;
        m_moduler_tutorial_presenter = moduler_tutorial_presenter;
    }

    public void Activate(ModuleReceipe module_receipe)
    {
        var center = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
        var ray = Camera.main.ScreenPointToRay(center);

        var module = m_module_db.GetModule(module_receipe.Code);
        m_module_receipe = module_receipe;

        m_preview_object = Instantiate(module.PreviewPrefab, ray.GetPoint(m_ray_length), Quaternion.identity);
        m_realview_object = module.RealviewPrefab;

        m_is_active = true;
    }

    public void Deactivate()
    {
        if(m_is_active)
        {
            Destroy(m_preview_object);
        }

        m_preview_object = null;
        m_realview_object = null;

        m_is_active = false;
        m_moduler_tutorial_presenter.CloseUI();
        GameEventBus.Publish(GameEventType.INPLAY);
    }

    private void Translation()
    {
        var preview_obj = m_preview_object.GetComponent<PreviewObject>();

        if (preview_obj != null && preview_obj.IsSnapped)
        {
            preview_obj.TryUnsnap();

            if (preview_obj.IsSnapped)
            {
                m_preview_object.transform.position = preview_obj.SnapPosition;
                return;
            }
        }

        var center = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
        var ray = Camera.main.ScreenPointToRay(center);

        m_preview_object.transform.position = ray.GetPoint(m_ray_length);
    }

    private void Build()
    {
        if(GameManager.Instance.GameType != GameEventType.CRAFTING)
        {
            return;
        }

        if(!m_is_active)
        {
            return;
        }

        var preview = m_preview_object.GetComponent<PreviewObject>();
        if(!preview.Buildable)
        {
            return;
        }

        var center = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
        var ray = Camera.main.ScreenPointToRay(center);

        var realview_obj = Instantiate(m_realview_object, m_preview_object.transform.position, Quaternion.identity);
        var realview_transform = realview_obj.GetComponentInChildren<RealviewObject>().transform;
        realview_transform.rotation = m_preview_object.transform.rotation;

        ConsumeIngredients();
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
