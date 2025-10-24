using UnityEngine;
using InventoryService;


[RequireComponent(typeof(CanvasGroup))]
public class InventoryView : MonoBehaviour, IInventoryView
{
    [Header("UI 관련 컴포넌트")]
    [Header("팝업 UI 매니저")]
    [SerializeField] private PopupUIManager m_ui_manager;

    private CanvasGroup m_canvas_group;
    private InventoryPresenter m_presenter;

    private void Awake()
    {
        m_canvas_group = GetComponent<CanvasGroup>();
    }

    private void OnDestroy()
    {
        m_presenter.Dispose();
    }

    public void Inject(InventoryPresenter inventory_presenter)
    {
        m_presenter = inventory_presenter;
    }

    public void OpenUI()
    {
        m_canvas_group.alpha = 1f;          
        m_canvas_group.interactable = true; 
        m_canvas_group.blocksRaycasts = true;

        ServiceLocator.Get<IInventoryService>().AddItem(ItemCode.HAND_AXE, 1);
        ServiceLocator.Get<IInventoryService>().AddItem(ItemCode.STONE_AXE, 1);
        ServiceLocator.Get<IInventoryService>().AddItem(ItemCode.STONE_PICKAXE, 1);
        ServiceLocator.Get<IInventoryService>().AddItem(ItemCode.STONE_SPEAR, 1);
        ServiceLocator.Get<IInventoryService>().AddItem(ItemCode.FISHING_ROD, 1);
        ServiceLocator.Get<IInventoryService>().AddItem(ItemCode.WOOD, 90);
        ServiceLocator.Get<IInventoryService>().AddItem(ItemCode.STONE, 90);
        ServiceLocator.Get<IInventoryService>().AddItem(ItemCode.WOOL, 90);
        ServiceLocator.Get<IInventoryService>().AddItem(ItemCode.TIMBER, 90);
        ServiceLocator.Get<IInventoryService>().AddItem(ItemCode.ROPE, 90);

        SoundManager.Instance.PlaySFX("UI Open", false, Vector3.zero);
    }

    public void CloseUI()
    {
        m_canvas_group.alpha = 0f;           
        m_canvas_group.interactable = false; 
        m_canvas_group.blocksRaycasts = false; 

        SoundManager.Instance.PlaySFX("UI Close", false, Vector3.zero);
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