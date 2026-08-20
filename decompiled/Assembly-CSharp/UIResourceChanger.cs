using System;
using System.Collections.Generic;
using PlayerLogger.Events;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[GameUISingletonPrefab("UIResourceChanger")]
public class UIResourceChanger : GameUISingletonMono<UIResourceChanger>
{
	public Text Title;

	public Animator anima;

	public List<UIResourceChangerSlot> slots = new List<UIResourceChangerSlot>();

	public static readonly Dictionary<UIResourceChangerSlotType, (int get, int cost)> ChangerDic = new Dictionary<UIResourceChangerSlotType, (int, int)>
	{
		{
			UIResourceChangerSlotType.CrystalBuyBlood,
			(1, 12)
		},
		{
			UIResourceChangerSlotType.CrystalBuyCore,
			(1, 120)
		},
		{
			UIResourceChangerSlotType.BloodBuyCrystal,
			(8, 1)
		},
		{
			UIResourceChangerSlotType.BloodBuyCore,
			(1, 12)
		},
		{
			UIResourceChangerSlotType.CoreBuyCrystal,
			(80, 1)
		},
		{
			UIResourceChangerSlotType.CoreBuyBlood,
			(8, 1)
		}
	};

	private int gamepadSelectIndex;

	private ResourceConvertLogger resourceConvertLogger;

	private int currentNum;

	public UIResourceChangerSlot interactingSlot { get; set; }

	protected override void RegistarWhenInit()
	{
		EventMgr.LanguageChange = (Action)Delegate.Combine(EventMgr.LanguageChange, new Action(LanguageChange));
		EventMgr.InputChange = (Action)Delegate.Combine(EventMgr.InputChange, new Action(InputChange));
	}

	protected override void RegistarOnlyWhenOpen()
	{
		base.inputActions.Player.GamepadDirect.performed += GamepadDirectPerformed;
		base.inputActions.Player.LeftStick.performed += GamepadDirectPerformed_Stick;
		base.inputActions.Player.Interact.performed += InteractPerformed;
		base.inputActions.Player.Interact.canceled += InteractCanceled;
	}

	protected override void UnRegistarOnlyWhenHide()
	{
		base.inputActions.Player.GamepadDirect.performed -= GamepadDirectPerformed;
		base.inputActions.Player.LeftStick.performed -= GamepadDirectPerformed_Stick;
		base.inputActions.Player.Interact.performed -= InteractPerformed;
		base.inputActions.Player.Interact.canceled -= InteractCanceled;
	}

	protected override void UnRegistarWhenDestroy()
	{
		EventMgr.LanguageChange = (Action)Delegate.Remove(EventMgr.LanguageChange, new Action(LanguageChange));
		EventMgr.InputChange = (Action)Delegate.Remove(EventMgr.InputChange, new Action(InputChange));
	}

	private void LanguageChange()
	{
		Title.text = 1002111.GetText();
	}

	private void GamepadDirectPerformed(InputAction.CallbackContext context)
	{
		if (UIMgr.Inst.InputType == PlayerInputType.Gamepad && base.IsOpen)
		{
			Vector2 direct = context.ReadValue<Vector2>();
			MoveDirect(direct);
		}
	}

	private void GamepadDirectPerformed_Stick(InputAction.CallbackContext context)
	{
		if (UIMgr.Inst.InputType == PlayerInputType.Gamepad && base.IsOpen)
		{
			Vector2 vector = context.ReadValue<Vector2>();
			vector = ControlMgr.Inst.RampVector2(vector, ControlMgr.rampType.UpDown);
			MoveDirect(vector);
		}
	}

	private void MoveDirect(Vector2 _direct)
	{
		if (interactingSlot != null)
		{
			return;
		}
		if (_direct == Vector2.up)
		{
			if (gamepadSelectIndex > 0)
			{
				GamepadMoveTo(gamepadSelectIndex - 1);
			}
		}
		else if (_direct == Vector2.down && gamepadSelectIndex < slots.Count - 1)
		{
			GamepadMoveTo(gamepadSelectIndex + 1);
		}
	}

	private void InputChange()
	{
		if (base.IsOpen)
		{
			switch (UIMgr.Inst.InputType)
			{
			case PlayerInputType.Keyboard:
				UnSelectSlot(gamepadSelectIndex);
				break;
			case PlayerInputType.Gamepad:
				gamepadSelectIndex = 0;
				SelectSlot(gamepadSelectIndex);
				break;
			default:
				Debug.LogError(UIMgr.Inst.InputType);
				break;
			}
		}
	}

	private void GamepadMoveTo(int i)
	{
		UnSelectSlot(gamepadSelectIndex);
		gamepadSelectIndex = i;
		SelectSlot(i);
	}

	private void UnSelectSlot(int i)
	{
		slots[i].go_Outline.SetActive(value: false);
	}

	private void SelectSlot(int i)
	{
		SEMgr.Inst.uiResearchHover.PlaySE();
		slots[i].go_Outline.SetActive(value: true);
	}

	private void InteractPerformed(InputAction.CallbackContext context)
	{
		if (UIMgr.Inst.InputType == PlayerInputType.Gamepad && base.IsOpen)
		{
			interactingSlot = slots[gamepadSelectIndex];
			interactingSlot.OnPointerClick(null);
			interactingSlot.OnPointerDown(null);
		}
	}

	private void InteractCanceled(InputAction.CallbackContext context)
	{
		if (UIMgr.Inst.InputType == PlayerInputType.Gamepad && base.IsOpen && interactingSlot != null)
		{
			interactingSlot.OnPointerUp(null);
			interactingSlot = null;
		}
	}

	protected override void OnShow(object obj = null)
	{
		resourceConvertLogger = new ResourceConvertLogger();
		resourceConvertLogger.AutoRecordBeforeResources();
		LanguageChange();
		anima.Play("Show");
		UIMgr.TryAdditionalMobileShow(base.transform);
		PlayerMgr.Inst.PlayerCtrller.StopMotion();
		UIPlayerDataMgr.Inst.ResourceUIPopUp(UIPlayerDataMgr.ResourceUIPop.Blood);
		UIPlayerDataMgr.Inst.ResourceUIPopUp(UIPlayerDataMgr.ResourceUIPop.Cores);
		UIPlayerDataMgr.Inst.ResourceUIPopUp(UIPlayerDataMgr.ResourceUIPop.Crystal);
		SEMgr.Inst.uiChangeLabel.PlaySE();
		slots.ForEach(delegate(UIResourceChangerSlot x)
		{
			x.UpdateResourses();
		});
	}

	protected override void OnHide()
	{
		anima.Play("Hide");
		UIMgr.TryAdditionalMobileHide(base.transform);
		PlayerMgr.Inst.PlayerCtrller.StartMotion();
		DataMgr.SaveSelectedWorldData();
		UIPlayerDataMgr.Inst.ResourceUISetToDefault(UIPlayerDataMgr.ResourceUIPop.Blood);
		UIPlayerDataMgr.Inst.ResourceUISetToDefault(UIPlayerDataMgr.ResourceUIPop.Cores);
		UIPlayerDataMgr.Inst.ResourceUISetToDefault(UIPlayerDataMgr.ResourceUIPop.Crystal);
		SEMgr.Inst.uiChangeLabelClose.PlaySE();
		resourceConvertLogger.AutoRecordAfterResourcesAndFlow();
		resourceConvertLogger.Report();
	}
}
