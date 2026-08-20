using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class UIButtonEvent : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	public UnityEvent unityEventPointIn;

	public UnityEvent unityEventPointOut;

	private bool skipOnceSE;

	public void SKipOnceSE()
	{
		skipOnceSE = true;
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		unityEventPointIn?.Invoke();
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		unityEventPointOut?.Invoke();
	}

	private void _PlayUIHoverSE()
	{
		if (skipOnceSE)
		{
			skipOnceSE = false;
		}
		else
		{
			SEMgr.Inst.uiButtonHover_Button.PlaySE();
		}
	}
}
