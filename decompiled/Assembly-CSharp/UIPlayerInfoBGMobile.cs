using UnityEngine;
using UnityEngine.EventSystems;

public class UIPlayerInfoBGMobile : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	private bool triggered;

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (Input.GetMouseButtonDown(0) && UIPlayerDataMgr.Inst.uiPlayerInfoBG.alpha != 0f)
		{
			UIPlayerDataMgr.Inst.BagClose();
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (GameMgr.IsMobile_Static && triggered && !UIPlayerDataMgr.Inst.IsDraging && UIPlayerDataMgr.Inst.uiPlayerInfoBG.alpha != 0f)
		{
			triggered = false;
			UIPlayerDataMgr.Inst.BagClose();
		}
	}
}
