using UnityEngine;
using UnityEngine.EventSystems;

public interface IItemSlotView : IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerClickHandler
{
    void Inject(ItemSlotPresenter presenter);


    void ClearUI();
    void UpdateUI(Sprite item_image, bool stackable, int count);

    bool IsMask(ItemType type);			// 특정 아이템 타입만 걸러낼 때 사용한다.	
    void SetCursor(CursorMode mode);	// 커서의 스프라이트를 변경할 때 사용한다.
}