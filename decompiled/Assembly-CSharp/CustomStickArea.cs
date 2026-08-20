using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class CustomStickArea : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler
{
	public List<OnScreenStickCustom> CustomSticks;

	private int? activePointerId;

	private Vector2 currentDragPosition;

	public void OnDrag(PointerEventData eventData)
	{
		if (activePointerId == eventData.pointerId)
		{
			CustomSticks.FirstOrDefault((OnScreenStickCustom x) => x.gameObject.activeInHierarchy)?.OnDrag(eventData);
			currentDragPosition = eventData.position;
		}
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (activePointerId.HasValue)
		{
			return;
		}
		activePointerId = eventData.pointerId;
		currentDragPosition = eventData.position;
		OnScreenStickCustom onScreenStickCustom = CustomSticks.FirstOrDefault((OnScreenStickCustom x) => x.gameObject.activeInHierarchy);
		if (!(onScreenStickCustom == null))
		{
			if (onScreenStickCustom.behaviour == OnScreenStickCustom.Behaviour.RelativePositionWithStaticOrigin)
			{
				onScreenStickCustom.OnDrag(eventData);
			}
			else
			{
				onScreenStickCustom.OnPointerDown(eventData);
			}
		}
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		if (activePointerId == eventData.pointerId && activePointerId.HasValue)
		{
			OnScreenStickCustom onScreenStickCustom = CustomSticks.FirstOrDefault((OnScreenStickCustom x) => x.gameObject.activeInHierarchy);
			if (onScreenStickCustom != null)
			{
				onScreenStickCustom.OnPointerUp((onScreenStickCustom.behaviour == OnScreenStickCustom.Behaviour.RelativePositionWithStaticOrigin) ? null : eventData);
			}
			activePointerId = null;
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
	}

	public void OnPointerExit(PointerEventData eventData)
	{
	}

	public void ForcePointerUp()
	{
		if (activePointerId.HasValue)
		{
			OnScreenStickCustom onScreenStickCustom = CustomSticks.FirstOrDefault((OnScreenStickCustom x) => x.gameObject.activeInHierarchy);
			if (onScreenStickCustom != null)
			{
				onScreenStickCustom.OnPointerUp(null);
			}
			activePointerId = null;
		}
	}
}
