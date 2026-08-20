using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class UnityEventTriggerEnterAndExit : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IBeginDragHandler, IEndDragHandler
{
	public UnityEvent PointEnter;

	public UnityEvent PointExit;

	public UnityEvent DragStart;

	public UnityEvent DragEnd;

	public void OnBeginDrag(PointerEventData eventData)
	{
		DragStart?.Invoke();
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		DragEnd?.Invoke();
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		PointEnter?.Invoke();
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		PointExit?.Invoke();
	}
}
