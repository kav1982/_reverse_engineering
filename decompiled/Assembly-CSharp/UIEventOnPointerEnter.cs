using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class UIEventOnPointerEnter : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler
{
	public UnityEvent unityEvent;

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (eventData.pointerEnter == base.gameObject)
		{
			unityEvent?.Invoke();
		}
	}
}
