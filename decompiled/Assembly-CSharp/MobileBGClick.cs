using UnityEngine;
using UnityEngine.EventSystems;

public class MobileBGClick : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
{
	public bool ignoreOpen;

	public GameUI gameui;

	public void OnPointerClick(PointerEventData eventData)
	{
		if ((ignoreOpen || gameui.IsOpen) && GameMgr.IsMobile_Static && !gameui.isDraging)
		{
			gameui._Close();
		}
	}
}
