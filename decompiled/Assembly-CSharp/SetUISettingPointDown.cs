using UnityEngine;
using UnityEngine.EventSystems;

public class SetUISettingPointDown : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler
{
	public UISetting uiSetting;

	public void OnPointerDown(PointerEventData eventData)
	{
		uiSetting.pointDown = true;
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		uiSetting.pointDown = false;
	}
}
