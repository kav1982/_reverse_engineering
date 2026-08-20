using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UITourHint : MonoBehaviour
{
	public Text textHint;

	public Text textConfirmButton;

	public CanvasGroup CanvasGroup;

	private InputActions inputActions;

	public GameObject gameobjectPadSelectConfirmButton;

	private void Awake()
	{
		inputActions = ControlMgr.Inst.inputActions;
		inputActions.Player.GamepadDirect.performed += DirectPerformed;
		inputActions.Player.Interact.performed += InteractPerformed;
		EventMgr.InputChange = (Action)Delegate.Combine(EventMgr.InputChange, new Action(InputChange));
	}

	private void InputChange()
	{
		PlayerInputType inputType = ControlMgr.Inst.InputType;
		if ((uint)inputType <= 1u)
		{
			gameobjectPadSelectConfirmButton.SetActive(value: false);
		}
	}

	private void Start()
	{
		textHint.text = 1006201.GetText();
		textConfirmButton.text = 1000105.GetText();
		CanvasGroup.alpha = 0f;
		CanvasGroup.DOFade(1f, 0.3f);
	}

	private void DirectPerformed(InputAction.CallbackContext context)
	{
		if (context.ReadValue<Vector2>() == Vector2.down)
		{
			gameobjectPadSelectConfirmButton.SetActive(value: true);
		}
	}

	private void InteractPerformed(InputAction.CallbackContext context)
	{
		if (gameobjectPadSelectConfirmButton.activeSelf)
		{
			ConfirmOnClick();
		}
	}

	private void OnDestroy()
	{
		EventMgr.InputChange = (Action)Delegate.Remove(EventMgr.InputChange, new Action(InputChange));
		inputActions.Player.GamepadDirect.performed -= DirectPerformed;
		inputActions.Player.Interact.performed -= InteractPerformed;
	}

	public void ConfirmOnClick()
	{
		PlayerMgr.Inst.PlayerCtrller.StartMotion();
		SEMgr.Inst.uiButtonHover_Button.PlaySE();
		UICampMgr.Inst.TourHint = null;
		UnityEngine.Object.Destroy(base.gameObject);
	}
}
