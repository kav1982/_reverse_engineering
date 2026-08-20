using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class UIMobileDrag : MonoBehaviour, IDragHandler, IEventSystemHandler, IBeginDragHandler, IEndDragHandler, IPointerClickHandler
{
	public bool activeOnPC;

	public GameUI gameui;

	private bool _hadBeenTriggered;

	private float _x;

	private float _y;

	public float sensitive;

	private float _sensitive;

	public bool triggerOncePerDrag;

	public UnityEvent dragleft;

	public UnityEvent dragright;

	public UnityEvent dragup;

	public UnityEvent dragdown;

	public UnityEvent clickLeftSide;

	public UnityEvent clickRightSide;

	private Coroutine _ieEndDrag;

	public bool clickTriggerDrag;

	public float cliskSideSensitive;

	public bool clickTriggerMultipleTime;

	public float triggerMultiSensitive;

	private bool _dragging;

	public void OnBeginDrag(PointerEventData eventData)
	{
		_dragging = true;
		_x = eventData.position.x;
		_y = eventData.position.y;
		_hadBeenTriggered = false;
	}

	public void OnDrag(PointerEventData eventData)
	{
		if (!_hadBeenTriggered || !triggerOncePerDrag)
		{
			if (gameui != null)
			{
				gameui.isDraging = true;
			}
			if (eventData.position.x > _x + _sensitive)
			{
				_x = eventData.position.x;
				_y = eventData.position.y;
				dragleft?.Invoke();
				_hadBeenTriggered = true;
			}
			else if (eventData.position.x < _x - _sensitive)
			{
				_x = eventData.position.x;
				_y = eventData.position.y;
				dragright?.Invoke();
				_hadBeenTriggered = true;
			}
			else if (eventData.position.y < _y - _sensitive)
			{
				_x = eventData.position.x;
				_y = eventData.position.y;
				dragup?.Invoke();
				_hadBeenTriggered = true;
			}
			else if (eventData.position.y > _y + _sensitive)
			{
				_x = eventData.position.x;
				_y = eventData.position.y;
				dragdown?.Invoke();
				_hadBeenTriggered = true;
			}
		}
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		_dragging = false;
		if (gameui != null)
		{
			if (_ieEndDrag != null)
			{
				StopCoroutine(_ieEndDrag);
			}
			_ieEndDrag = StartCoroutine(EndDrag());
		}
		_x = eventData.position.x;
		_y = eventData.position.y;
	}

	private IEnumerator EndDrag()
	{
		while (ControlMgr.Inst.isScreenTouching)
		{
			yield return new WaitForEndOfFrame();
		}
		gameui.isDraging = false;
	}

	private void OnEnable()
	{
		if (GameMgr.IsMobile_Static)
		{
			_sensitive = sensitive / 100f * (float)MobileMgr.inst.scalerwidth;
		}
		else if (activeOnPC)
		{
			_sensitive = sensitive / 100f * 1920f;
		}
		else
		{
			base.gameObject.SetActive(value: false);
		}
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (_dragging)
		{
			return;
		}
		Vector2 localPoint = default(Vector2);
		if (clickTriggerMultipleTime)
		{
			RectTransformUtility.ScreenPointToLocalPointInRectangle(base.transform as RectTransform, eventData.position, CamController.Inst.cam_UI, out localPoint);
		}
		float num = eventData.position.x - RectTransformUtility.WorldToScreenPoint(CamController.Inst.cam_UI, base.transform.position).x;
		if (num > cliskSideSensitive)
		{
			if (clickTriggerMultipleTime)
			{
				for (int i = 0; i < Mathf.CeilToInt((Mathf.Abs(localPoint.x) - triggerMultiSensitive / 2f) / triggerMultiSensitive); i++)
				{
					TriggerRight();
				}
			}
			else
			{
				TriggerRight();
			}
		}
		else
		{
			if (!(num < 0f - cliskSideSensitive))
			{
				return;
			}
			if (clickTriggerMultipleTime)
			{
				for (int j = 0; j < Mathf.CeilToInt((Mathf.Abs(localPoint.x) - triggerMultiSensitive / 2f) / triggerMultiSensitive); j++)
				{
					TriggerLeft();
				}
			}
			else
			{
				TriggerLeft();
			}
		}
	}

	public void TriggerRight()
	{
		if (clickTriggerDrag)
		{
			dragright?.Invoke();
		}
		else
		{
			clickRightSide?.Invoke();
		}
	}

	public void TriggerLeft()
	{
		if (clickTriggerDrag)
		{
			dragleft?.Invoke();
		}
		else
		{
			clickLeftSide?.Invoke();
		}
	}
}
