using UnityEngine;
using UnityEngine.EventSystems;

public class UIAged12 : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
{
	public void OnPointerClick(PointerEventData eventData)
	{
		UIMainMenuMgr.Inst.OpenCloesAgedInfo();
	}
}
