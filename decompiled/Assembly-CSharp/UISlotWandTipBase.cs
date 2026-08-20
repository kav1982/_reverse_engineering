using UnityEngine;
using UnityEngine.EventSystems;

public class UISlotWandTipBase : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	public enum UISlotWandInfoType
	{
		MimicError,
		LackMana,
		Unused
	}

	public UISlotWandInfoType infoType;

	public virtual void Show()
	{
		switch (infoType)
		{
		case UISlotWandInfoType.MimicError:
			UIPlayerDataMgr.Inst.uiSlotWandTips.image_MimicError.SetActive(value: true);
			break;
		case UISlotWandInfoType.LackMana:
			UIPlayerDataMgr.Inst.uiSlotWandTips.image_UnableToCastSlotSpellAlert.SetActive(value: true);
			break;
		case UISlotWandInfoType.Unused:
			UIPlayerDataMgr.Inst.uiSlotWandTips.image_Unused.SetActive(value: true);
			break;
		}
	}

	public virtual void Hide()
	{
		switch (infoType)
		{
		case UISlotWandInfoType.MimicError:
			UIPlayerDataMgr.Inst.uiSlotWandTips.image_MimicError.SetActive(value: false);
			break;
		case UISlotWandInfoType.LackMana:
			UIPlayerDataMgr.Inst.uiSlotWandTips.image_UnableToCastSlotSpellAlert.SetActive(value: false);
			break;
		case UISlotWandInfoType.Unused:
			UIPlayerDataMgr.Inst.uiSlotWandTips.image_Unused.SetActive(value: false);
			break;
		}
	}

	private void OnDisable()
	{
		Hide();
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		Show();
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		Hide();
	}
}
