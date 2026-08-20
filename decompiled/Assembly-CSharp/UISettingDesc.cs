using UnityEngine;
using UnityEngine.EventSystems;

public class UISettingDesc : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	public RectTransform rtsf_Text;

	public UISetting uiSetting;

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (!uiSetting.pointDown)
		{
			uiSetting.OtherTextEnter(this);
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (!GameMgr.IsMobile_Static)
		{
			uiSetting.OtherTextExit();
		}
	}
}
