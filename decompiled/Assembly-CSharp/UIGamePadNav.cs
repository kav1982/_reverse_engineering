using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIGamePadNav : MonoBehaviour
{
	public UIGamePadNav Up;

	public UIGamePadNav Down;

	public UIGamePadNav Left;

	public UIGamePadNav Right;

	public Action<PointerEventData> OnSelectAction { get; set; }

	public Action<PointerEventData> OnDeselectAction { get; set; }

	public Action DoIfNavToSelf { get; set; }

	public void OnSelect(PointerEventData eventData)
	{
		OnSelectAction?.Invoke(eventData);
	}

	public void OnDeselect(PointerEventData eventData)
	{
		OnDeselectAction?.Invoke(eventData);
	}

	public void SetNav(UIGamePadNav up, UIGamePadNav down, UIGamePadNav left, UIGamePadNav right)
	{
		Up = up;
		Down = down;
		Left = left;
		Right = right;
	}

	public UIGamePadNav NavTo(Vector2 dir, PointerEventData exitData = null, PointerEventData enterData = null, Action ifNull = null)
	{
		UIGamePadNav uIGamePadNav = null;
		if (dir == Vector2.up)
		{
			uIGamePadNav = Up;
		}
		else if (dir == Vector2.down)
		{
			uIGamePadNav = Down;
		}
		else if (dir == Vector2.left)
		{
			uIGamePadNav = Left;
		}
		else if (dir == Vector2.right)
		{
			uIGamePadNav = Right;
		}
		if (uIGamePadNav != null)
		{
			if (uIGamePadNav == this)
			{
				DoIfNavToSelf?.Invoke();
				Debug.Log("NavToSelf");
				return this;
			}
			OnDeselect(exitData);
			uIGamePadNav.OnSelect(enterData);
			return uIGamePadNav;
		}
		ifNull?.Invoke();
		return this;
	}
}
