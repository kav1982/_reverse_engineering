using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;

public class ControlMgr : MonoBehaviour
{
	public enum controllertype
	{
		Xbox,
		PS,
		SteamDeck,
		SteamDeckKeyBoard
	}

	public enum rampType
	{
		FourDirection,
		FourDirectionNotPrecise,
		LeftRight,
		UpDown
	}

	public enum controltype
	{
		key,
		pad
	}

	public float _timecountdown;

	private controllertype _controllertype;

	[Header("\ufffd\ufffd\ufffd\u0330\ufffd\ufffd\ufffd\ufffd\ufffdʾ")]
	public float defaultKeyHeight = 40f;

	public float minWidth = 35.7f;

	public float widthOffset = 21f;

	private const float ThrashHoldUseAxisTarget = 0.7f;

	private const float ThrashHoldUnUseAxisDiscard = 0.8f;

	private const float ThrashHoldUseAxisDiscardPrecise = 0.4f;

	private const float ThrashHoldUseAxisTargetPrecise = 0.95f;

	private const float ThrashHoldUseAxisDiscardNotPrecise = 0.4f;

	private const float ThrashHoldUseAxisTargetNotPrecise = 0.5f;

	private Dictionary<UISetting.btnEnum, int> dictionary_keybindingindex = new Dictionary<UISetting.btnEnum, int>();

	private Dictionary<UISetting.controlEnum, int> dictionary_controllerbindingindex = new Dictionary<UISetting.controlEnum, int>();

	public bool isScreenTouching;

	public bool usingTouchScreen;

	public static ControlMgr Inst;

	public InputActions inputActions;

	public InputAction.CallbackContext Lastinput;

	public bool CursorVisible = true;

	private bool CursorOverObject;

	public bool rebinding;

	public bool usingpad;

	public bool RampStickAvailable;

	private bool _RampStickAvailable;

	private char[] charsplit = new char[2] { '<', '>' };

	public float stick_active_delay = 0.2f;

	public bool InputActionRecovering;

	private Vector2 _input_WASD;

	private Vector2 _input_WASD_SimulatedButton;

	private bool ActiveSkillKeyUp;

	private const float GamepadEastThreshold = 0.5f;

	private bool _sprintControlerPressed;

	private bool timecount = true;

	public PlayerInputType InputType { get; private set; }

	private bool QuickPanelButtonPressed { get; set; }

	public bool QuickPanelButtonPressedTriggerOnce
	{
		get
		{
			if (QuickPanelButtonPressed)
			{
				QuickPanelButtonPressed = false;
				return true;
			}
			return false;
		}
	}

	private bool KillSummonPressed { get; set; }

	public bool KillSummonPressedTriggerOnce
	{
		get
		{
			PlayerInputType inputType = InputType;
			if ((uint)inputType <= 1u)
			{
				if (KillSummonPressed)
				{
					KillSummonPressed = false;
					return true;
				}
				return false;
			}
			return false;
		}
	}

	private bool GamepadEastPressed { get; set; }

	public void Start()
	{
		StartCoroutine(DelayChangeControl());
	}

	public bool isSprintPressed()
	{
		if (_sprintControlerPressed)
		{
			_sprintControlerPressed = false;
			return true;
		}
		return false;
	}

	public void Update()
	{
		if (_timecountdown > 0f)
		{
			_timecountdown -= Time.fixedDeltaTime;
		}
		if (!EventSystem.current.IsPointerOverGameObject())
		{
			CursorOverObject = false;
		}
		else
		{
			CursorOverObject = true;
		}
		if (!DataMgr.settingData.hardwareCursor)
		{
			float num = UIMgr.Inst.canvasScalerCanvas11.referenceResolution.y / (float)Display.main.renderingHeight;
			MobileMgr.inst.topui.MouseCursor.GetComponent<RectTransform>().localPosition = new Vector2(Input.mousePosition.x * num, Input.mousePosition.y * num);
		}
	}

	public bool IsPressingActiveSkillKey()
	{
		return _sprintControlerPressed;
	}

	public bool IsActiveSkillKeyUp()
	{
		return ActiveSkillKeyUp;
	}

	private void LateUpdate()
	{
		ActiveSkillKeyUp = false;
	}

	public void OnEnable()
	{
		InputActionRecovering = false;
		inputActions.Player.AnyMouseKeyboardInput.performed += AnyMouseKeyboardInputPerformed;
		inputActions.Player.AnyGamepadInput.performed += AnyGamepadInputPerformed;
		inputActions.Player.AnyTouchScreen.performed += AnyTouchScreen_performed;
		inputActions.Player.AnyTouchScreen.canceled += AnyTouchScreen_cancled;
		inputActions.Player.GamepadDpad.performed += UPdateInputWASD;
		inputActions.Player.GamepadDpad.canceled += UpdateInputWASD_cancled;
		inputActions.Player.WASD.canceled += UpdateInputWASD_cancled;
		inputActions.Player.LeftStick.canceled += UpdateInputWASD_cancled;
		inputActions.Player.WASD.performed += UPdateInputWASD;
		inputActions.Player.LeftStick.performed += UPdateInputWASD;
		inputActions.Player.Sprint.performed += SprintPerformed;
		inputActions.Player.Sprint.canceled += SprintCanceld;
		inputActions.Player.QuickPanel.performed += QuickPanelPerformed;
		inputActions.Player.QuickPanel.canceled += QuickPanelCanceld;
		inputActions.Player.KillSummon.performed += KillSummonPerformed;
		inputActions.Player.KillSummon.canceled += KillSummonCanceld;
	}

	public void OnDisable()
	{
		inputActions.Player.AnyMouseKeyboardInput.performed -= AnyMouseKeyboardInputPerformed;
		inputActions.Player.AnyGamepadInput.performed -= AnyGamepadInputPerformed;
		inputActions.Player.AnyTouchScreen.performed -= AnyTouchScreen_performed;
		inputActions.Player.AnyTouchScreen.canceled -= AnyTouchScreen_cancled;
		inputActions.Player.GamepadDpad.performed -= UPdateInputWASD;
		inputActions.Player.GamepadDpad.canceled -= UpdateInputWASD_cancled;
		inputActions.Player.WASD.canceled -= UpdateInputWASD_cancled;
		inputActions.Player.LeftStick.canceled -= UpdateInputWASD_cancled;
		inputActions.Player.WASD.performed -= UPdateInputWASD;
		inputActions.Player.LeftStick.performed -= UPdateInputWASD;
		inputActions.Player.Sprint.performed -= SprintPerformed;
		inputActions.Player.Sprint.canceled -= SprintCanceld;
		inputActions.Player.QuickPanel.performed -= QuickPanelPerformed;
		inputActions.Player.QuickPanel.canceled -= QuickPanelCanceld;
		inputActions.Player.KillSummon.performed -= KillSummonPerformed;
		inputActions.Player.KillSummon.canceled -= KillSummonCanceld;
	}

	public void Initialize()
	{
		Inst = this;
		if (GameMgr.IsSteamDeck_Static)
		{
			SetControllerType(controllertype.SteamDeck);
		}
		if (inputActions == null)
		{
			inputActions = new InputActions();
			dictionary_keybindingindex.Add(UISetting.btnEnum.w, 1);
			dictionary_keybindingindex.Add(UISetting.btnEnum.s, 2);
			dictionary_keybindingindex.Add(UISetting.btnEnum.a, 3);
			dictionary_keybindingindex.Add(UISetting.btnEnum.d, 4);
			dictionary_keybindingindex.Add(UISetting.btnEnum.shoot, 0);
			dictionary_keybindingindex.Add(UISetting.btnEnum.e, 0);
			dictionary_keybindingindex.Add(UISetting.btnEnum.bag, 0);
			dictionary_keybindingindex.Add(UISetting.btnEnum.switchwandup, 0);
			dictionary_keybindingindex.Add(UISetting.btnEnum.switchwanddown, 0);
			dictionary_keybindingindex.Add(UISetting.btnEnum.wand1, 0);
			dictionary_keybindingindex.Add(UISetting.btnEnum.wand2, 0);
			dictionary_keybindingindex.Add(UISetting.btnEnum.wand3, 0);
			dictionary_keybindingindex.Add(UISetting.btnEnum.wand4, 0);
			dictionary_keybindingindex.Add(UISetting.btnEnum.wand5, 0);
			dictionary_keybindingindex.Add(UISetting.btnEnum.wand6, 0);
			dictionary_keybindingindex.Add(UISetting.btnEnum.usepotion, 0);
			dictionary_keybindingindex.Add(UISetting.btnEnum.switchpotionup, 0);
			dictionary_keybindingindex.Add(UISetting.btnEnum.switchpotiondown, 0);
			dictionary_keybindingindex.Add(UISetting.btnEnum.quickremove, 0);
			dictionary_keybindingindex.Add(UISetting.btnEnum.Sprint, 0);
			dictionary_keybindingindex.Add(UISetting.btnEnum.QuickPanel, 0);
			dictionary_keybindingindex.Add(UISetting.btnEnum.KillSummon, 0);
			dictionary_controllerbindingindex.Add(UISetting.controlEnum.wasd, 0);
			dictionary_controllerbindingindex.Add(UISetting.controlEnum.aim, 0);
			dictionary_controllerbindingindex.Add(UISetting.controlEnum.e, 1);
			dictionary_controllerbindingindex.Add(UISetting.controlEnum.shoot, 1);
			dictionary_controllerbindingindex.Add(UISetting.controlEnum.bag, 1);
			dictionary_controllerbindingindex.Add(UISetting.controlEnum.switchwandup, 1);
			dictionary_controllerbindingindex.Add(UISetting.controlEnum.switchwanddown, 1);
			dictionary_controllerbindingindex.Add(UISetting.controlEnum.menue, 1);
			dictionary_controllerbindingindex.Add(UISetting.controlEnum.usepotion, 1);
			dictionary_controllerbindingindex.Add(UISetting.controlEnum.switchpotion, 0);
			dictionary_controllerbindingindex.Add(UISetting.controlEnum.throwaway, 0);
			dictionary_controllerbindingindex.Add(UISetting.controlEnum.moveobj, 0);
			dictionary_controllerbindingindex.Add(UISetting.controlEnum.back, 0);
			dictionary_controllerbindingindex.Add(UISetting.controlEnum.Sprint, 1);
			dictionary_controllerbindingindex.Add(UISetting.controlEnum.QuickPanel, 1);
			dictionary_controllerbindingindex.Add(UISetting.controlEnum.KillSummon, 1);
		}
		if (DataMgr.settingData.controldata.initalized)
		{
			loadcontrol();
			EventMgr.ControlChange?.Invoke();
		}
		else
		{
			Debug.LogError("initializekeycontrol");
			initializekeycontrol();
		}
		DataMgr.SaveSettingData();
		inputActions.Player.LeftStick.canceled += DirectionOncancle;
	}

	public bool GetTimeCount()
	{
		return timecount;
	}

	public void CursorVisibleSet(bool set)
	{
		if (GameMgr.IsMobile_Static)
		{
			MobileMgr.inst.topui.MouseCursor.SetActive(value: false);
			return;
		}
		CursorVisible = set;
		if (DataMgr.settingData.hardwareCursor)
		{
			if (set)
			{
				UIMgr.Inst.uiSetting.UpdateCursor();
				Cursor.visible = true;
			}
			else if (!set)
			{
				UISetting.SetCursorNull();
				Cursor.visible = false;
			}
		}
		else if (set)
		{
			MobileMgr.inst.topui.MouseCursor.SetActive(value: true);
		}
		else if (!set)
		{
			MobileMgr.inst.topui.MouseCursor.SetActive(value: false);
		}
	}

	public void CursorLockstateSet(CursorLockMode lockmode)
	{
		Cursor.lockState = lockmode;
	}

	private void AnyTouchScreen_cancled(InputAction.CallbackContext obj)
	{
		isScreenTouching = false;
	}

	private void AnyTouchScreen_performed(InputAction.CallbackContext obj)
	{
		isScreenTouching = true;
		InputType = PlayerInputType.Keyboard;
		if (!usingTouchScreen)
		{
			usingTouchScreen = true;
			EventMgr.InputChange();
		}
	}

	public bool Getcursorover()
	{
		return CursorOverObject;
	}

	private void AnyMouseKeyboardInputPerformed(InputAction.CallbackContext context)
	{
		if (_timecountdown > 0f || InputType == PlayerInputType.Keyboard || Inst.InputActionRecovering)
		{
			return;
		}
		InputControl control = context.control;
		if (control is ButtonControl buttonControl)
		{
			Debug.Log("Button control: " + buttonControl.name);
		}
		else if (control is AxisControl axisControl)
		{
			Debug.Log("Axis control: " + axisControl.name);
		}
		if (CanChangeControllerType())
		{
			if (GameMgr.IsSteamDeck_Static && _controllertype == controllertype.SteamDeck)
			{
				_controllertype = controllertype.SteamDeckKeyBoard;
				Inst.CursorVisibleSet(set: true);
				Inst.CursorLockstateSet(CursorLockMode.None);
			}
			else if (!GameMgr.IsMobile_Static)
			{
				InputType = PlayerInputType.Keyboard;
				Inst.CursorVisibleSet(set: true);
				Inst.CursorLockstateSet(CursorLockMode.None);
				EventMgr.InputChange?.Invoke();
			}
		}
	}

	private static string GetDeviceLayoutFromContext(InputAction.CallbackContext context)
	{
		if (context.control == null || context.control.device == null)
		{
			return "";
		}
		return context.control.device.layout;
	}

	private void AnyGamepadInputPerformed(InputAction.CallbackContext context)
	{
		if (GameMgr.IsMobile_Static)
		{
			usingTouchScreen = false;
		}
		if (Inst.InputActionRecovering || !CanChangeControllerType())
		{
			return;
		}
		if (GameMgr.IsSteamDeck_Static && _controllertype == controllertype.SteamDeckKeyBoard)
		{
			_controllertype = controllertype.SteamDeck;
			Inst.CursorVisibleSet(set: false);
			Inst.CursorLockstateSet(CursorLockMode.Locked);
		}
		else if (InputType == PlayerInputType.Keyboard)
		{
			InputType = PlayerInputType.Gamepad;
			if (PlayerMgr.Inst != null && PlayerMgr.Inst.PlayerCtrller != null)
			{
				PlayerMgr.Inst.PlayerCtrller.InitPadDirection();
			}
			SetControllerType(ChangeEvent: false);
			EventMgr.InputChange();
			_timecountdown = 0.2f;
			Inst.CursorVisibleSet(set: false);
			Inst.CursorLockstateSet(CursorLockMode.Locked);
		}
		else
		{
			SetControllerType(ChangeEvent: true);
		}
		void SetControllerType(bool ChangeEvent)
		{
			if (!GameMgr.IsSteamDeck_Static)
			{
				if (context.control.path.Split('/')[1] == "XInputControllerWindows")
				{
					if (_controllertype != 0)
					{
						_controllertype = controllertype.Xbox;
						if (ChangeEvent)
						{
							EventMgr.InputChange();
						}
					}
				}
				else if (context.control.path.Split('/')[1] == "DualShock4GamepadHID" || context.control.path.Split('/')[1] == "DualShockGamepadHID" || context.control.path.Split('/')[1] == "DualShock5GamepadHID")
				{
					if (_controllertype != controllertype.PS)
					{
						_controllertype = controllertype.PS;
						if (ChangeEvent)
						{
							EventMgr.InputChange();
						}
					}
				}
				else if (_controllertype != 0)
				{
					_controllertype = controllertype.Xbox;
					if (ChangeEvent)
					{
						EventMgr.InputChange();
					}
				}
			}
		}
	}

	private bool CanChangeControllerType()
	{
		if ((bool)UIPlayerDataMgr.Inst)
		{
			return !UIPlayerDataMgr.Inst.IsDraging;
		}
		return true;
	}

	public controllertype GetControllerType()
	{
		return _controllertype;
	}

	private void SetControllerType(controllertype controllertype)
	{
		Debug.Log(controllertype);
		if (GameMgr.IsSteamDeck_Static)
		{
			InputType = PlayerInputType.Gamepad;
			if ((bool)PlayerMgr.Inst.PlayerCtrller)
			{
				PlayerMgr.Inst.PlayerCtrller.InitPadDirection();
			}
			_controllertype = controllertype;
		}
	}

	public void ForceSetInputType(PlayerInputType inputType)
	{
		InputType = inputType;
	}

	public Vector2 RampVector2(Vector2 _vector2, rampType rampType = rampType.FourDirection)
	{
		if (Inst.RampStickAvailable)
		{
			Inst.RampStickAvailable = false;
			return RampForceRamp(_vector2, rampType);
		}
		if ((double)_vector2.y > -0.4 && (double)_vector2.y < 0.4 && (double)_vector2.x > -0.4 && (double)_vector2.x < 0.4)
		{
			Inst.RampStickAvailable = true;
			return Vector2.zero;
		}
		return Vector2.zero;
	}

	public Vector2 RampForceRamp(Vector2 _vector2, rampType rampType)
	{
		return rampType switch
		{
			rampType.FourDirection => Ramp4(_vector2), 
			rampType.FourDirectionNotPrecise => Ramp4(_vector2, 0.5f), 
			rampType.LeftRight => RampLeftRight(_vector2), 
			rampType.UpDown => RampUpDown(_vector2), 
			_ => Ramp4(_vector2), 
		};
		static Vector2 Ramp4(Vector2 _vector2, float thrashhold1 = 0.95f, float thrashhold2 = 0.4f)
		{
			if (_vector2.x >= thrashhold1 && _vector2.y > 0f - thrashhold2 && _vector2.y < thrashhold2)
			{
				return Vector2.right;
			}
			if (_vector2.x <= 0f - thrashhold1 && _vector2.y > 0f - thrashhold2 && _vector2.y < thrashhold2)
			{
				return Vector2.left;
			}
			if (_vector2.y >= thrashhold1 && _vector2.x > 0f - thrashhold2 && _vector2.x < thrashhold2)
			{
				return Vector2.up;
			}
			if (_vector2.y <= 0f - thrashhold1 && _vector2.x > 0f - thrashhold2 && _vector2.x < thrashhold2)
			{
				return Vector2.down;
			}
			Inst.RampStickAvailable = true;
			return Vector2.zero;
		}
		static Vector2 RampLeftRight(Vector2 _vector2)
		{
			if (_vector2.x >= 0.7f && _vector2.y > -0.8f && _vector2.y < 0.8f)
			{
				return Vector2.right;
			}
			if (_vector2.x <= -0.7f && _vector2.y > -0.8f && _vector2.y < 0.8f)
			{
				return Vector2.left;
			}
			Inst.RampStickAvailable = true;
			return Vector2.zero;
		}
		static Vector2 RampUpDown(Vector2 _vector2)
		{
			if (_vector2.y >= 0.7f && _vector2.x > -0.8f && _vector2.x < 0.8f)
			{
				return Vector2.up;
			}
			if (_vector2.y <= -0.7f && _vector2.x > -0.8f && _vector2.x < 0.8f)
			{
				return Vector2.down;
			}
			Inst.RampStickAvailable = true;
			return Vector2.zero;
		}
	}

	private Vector2 _RampVector2(Vector2 _vector2)
	{
		if (Inst._RampStickAvailable)
		{
			Inst._RampStickAvailable = false;
			if (_vector2.x >= 0.95f && _vector2.y > -0.4f && _vector2.y < 0.4f)
			{
				return Vector2.right;
			}
			if (_vector2.x <= -0.95f && _vector2.y > -0.4f && _vector2.y < 0.4f)
			{
				return Vector2.left;
			}
			if (_vector2.y >= 0.95f && _vector2.x > -0.4f && _vector2.x < 0.4f)
			{
				return Vector2.up;
			}
			if (_vector2.y <= -0.95f && _vector2.x > -0.4f && _vector2.x < 0.4f)
			{
				return Vector2.down;
			}
			Inst._RampStickAvailable = true;
			return Vector2.zero;
		}
		if (_vector2.y > -0.4f && _vector2.y < 0.4f && _vector2.x > -0.4f && _vector2.x < 0.4f)
		{
			Inst._RampStickAvailable = true;
			return Vector2.zero;
		}
		return Vector2.zero;
	}

	public Vector2 GetInputWASD()
	{
		return _input_WASD;
	}

	private void UPdateInputWASD(InputAction.CallbackContext context)
	{
		_input_WASD = context.ReadValue<Vector2>();
		if (UIMgr.Inst.InputType == PlayerInputType.Keyboard)
		{
			_input_WASD_SimulatedButton = Vector2.zero;
		}
		else
		{
			_input_WASD_SimulatedButton = Inst._RampVector2(_input_WASD);
		}
	}

	private void UpdateInputWASD_cancled(InputAction.CallbackContext context)
	{
		_input_WASD = Vector2.zero;
		_input_WASD_SimulatedButton = Vector2.zero;
		_RampStickAvailable = true;
	}

	private void DirectionOncancle(InputAction.CallbackContext context)
	{
		Inst.RampStickAvailable = true;
	}

	private void SprintPerformed(InputAction.CallbackContext context)
	{
		_sprintControlerPressed = true;
	}

	private void SprintCanceld(InputAction.CallbackContext context)
	{
		_sprintControlerPressed = false;
		ActiveSkillKeyUp = true;
	}

	public void SprintPerformed()
	{
		_sprintControlerPressed = true;
	}

	public void SprintCanceld()
	{
		_sprintControlerPressed = false;
		ActiveSkillKeyUp = true;
	}

	private void QuickPanelPerformed(InputAction.CallbackContext context)
	{
		Debug.Log("QuickPanelPerformed");
		QuickPanelButtonPressed = true;
	}

	private void QuickPanelCanceld(InputAction.CallbackContext context)
	{
		QuickPanelButtonPressed = false;
	}

	private void KillSummonPerformed(InputAction.CallbackContext context)
	{
		KillSummonPressed = true;
	}

	private void KillSummonCanceld(InputAction.CallbackContext context)
	{
		KillSummonPressed = false;
	}

	public void ReSetControl()
	{
		StartCoroutine(resetcontrol());
	}

	public IEnumerator resetcontrol()
	{
		InputActionRecovering = true;
		Debug.Log("\ufffd\ufffd\ufffd谴\ufffd\ufffd");
		InputActions inputActions = new InputActions();
		DataMgr.settingData.controldata.Key_w = inputActions.Player.WASD.bindings[dictionary_keybindingindex[UISetting.btnEnum.w]].path;
		DataMgr.settingData.controldata.Key_s = inputActions.Player.WASD.bindings[dictionary_keybindingindex[UISetting.btnEnum.s]].path;
		DataMgr.settingData.controldata.Key_a = inputActions.Player.WASD.bindings[dictionary_keybindingindex[UISetting.btnEnum.a]].path;
		DataMgr.settingData.controldata.Key_d = inputActions.Player.WASD.bindings[dictionary_keybindingindex[UISetting.btnEnum.d]].path;
		DataMgr.settingData.controldata.Key_shoot = inputActions.Player.Shoot.bindings[dictionary_keybindingindex[UISetting.btnEnum.shoot]].path;
		DataMgr.settingData.controldata.Key_e = inputActions.Player.Interact.bindings[dictionary_keybindingindex[UISetting.btnEnum.e]].path;
		DataMgr.settingData.controldata.Key_bag = inputActions.Player.Bag.bindings[dictionary_keybindingindex[UISetting.btnEnum.bag]].path;
		DataMgr.settingData.controldata.Key_wand1 = inputActions.Player.Alpha1.bindings[dictionary_keybindingindex[UISetting.btnEnum.wand1]].path;
		DataMgr.settingData.controldata.Key_wand2 = inputActions.Player.Alpha2.bindings[dictionary_keybindingindex[UISetting.btnEnum.wand2]].path;
		DataMgr.settingData.controldata.Key_wand3 = inputActions.Player.Alpha3.bindings[dictionary_keybindingindex[UISetting.btnEnum.wand3]].path;
		DataMgr.settingData.controldata.Key_wand4 = inputActions.Player.Alpha4.bindings[dictionary_keybindingindex[UISetting.btnEnum.wand4]].path;
		DataMgr.settingData.controldata.Key_wand5 = inputActions.Player.Alpha5.bindings[dictionary_keybindingindex[UISetting.btnEnum.wand5]].path;
		DataMgr.settingData.controldata.Key_wand6 = inputActions.Player.Alpha6.bindings[dictionary_keybindingindex[UISetting.btnEnum.wand6]].path;
		DataMgr.settingData.controldata.Key_usepotion = inputActions.Player.Drink.bindings[dictionary_keybindingindex[UISetting.btnEnum.usepotion]].path;
		DataMgr.settingData.controldata.Key_potionup = inputActions.Player.PotionUp.bindings[dictionary_keybindingindex[UISetting.btnEnum.switchpotiondown]].path;
		DataMgr.settingData.controldata.Key_potiondown = inputActions.Player.PotionDown.bindings[dictionary_keybindingindex[UISetting.btnEnum.switchpotiondown]].path;
		DataMgr.settingData.controldata.Key_quickremove = inputActions.Player.QuickRemove.bindings[dictionary_keybindingindex[UISetting.btnEnum.quickremove]].path;
		DataMgr.settingData.controldata.Key_Sprint = inputActions.Player.Sprint.bindings[dictionary_keybindingindex[UISetting.btnEnum.Sprint]].path;
		DataMgr.settingData.controldata.Key_QuickPanel = inputActions.Player.QuickPanel.bindings[dictionary_keybindingindex[UISetting.btnEnum.QuickPanel]].path;
		DataMgr.settingData.controldata.Key_KillSummon = inputActions.Player.KillSummon.bindings[dictionary_keybindingindex[UISetting.btnEnum.KillSummon]].path;
		DataMgr.settingData.controldata.Key_wandup = inputActions.Player.WandUp.bindings[dictionary_keybindingindex[UISetting.btnEnum.switchwandup]].path;
		DataMgr.settingData.controldata.Key_wanddown = inputActions.Player.WandDown.bindings[dictionary_keybindingindex[UISetting.btnEnum.switchwanddown]].path;
		DataMgr.settingData.controldata.Controler_move = inputActions.Player.LeftStick.bindings[dictionary_controllerbindingindex[UISetting.controlEnum.wasd]].path;
		DataMgr.settingData.controldata.Controler_aim = inputActions.Player.RightStick.bindings[dictionary_controllerbindingindex[UISetting.controlEnum.aim]].path;
		DataMgr.settingData.controldata.Controler_interact = inputActions.Player.Interact.bindings[dictionary_controllerbindingindex[UISetting.controlEnum.e]].path;
		DataMgr.settingData.controldata.Controler_shoot = inputActions.Player.Shoot.bindings[dictionary_controllerbindingindex[UISetting.controlEnum.shoot]].path;
		DataMgr.settingData.controldata.Controler_bag = inputActions.Player.Bag.bindings[dictionary_controllerbindingindex[UISetting.controlEnum.bag]].path;
		DataMgr.settingData.controldata.Controler_wandup = inputActions.Player.WandUp.bindings[dictionary_controllerbindingindex[UISetting.controlEnum.switchwandup]].path;
		DataMgr.settingData.controldata.Controler_wanddown = inputActions.Player.WandDown.bindings[dictionary_controllerbindingindex[UISetting.controlEnum.switchwanddown]].path;
		DataMgr.settingData.controldata.Controler_usepotion = inputActions.Player.Drink.bindings[dictionary_controllerbindingindex[UISetting.controlEnum.usepotion]].path;
		DataMgr.settingData.controldata.Controler_moveObj = inputActions.Player.GamepadWest.bindings[dictionary_controllerbindingindex[UISetting.controlEnum.moveobj]].path;
		DataMgr.settingData.controldata.Controler_back = inputActions.Player.GamepadEast.bindings[dictionary_controllerbindingindex[UISetting.controlEnum.back]].path;
		DataMgr.settingData.controldata.Controler_menue = inputActions.Player.Pause.bindings[dictionary_controllerbindingindex[UISetting.controlEnum.menue]].path;
		DataMgr.settingData.controldata.Controler_Sprint = inputActions.Player.Sprint.bindings[dictionary_controllerbindingindex[UISetting.controlEnum.Sprint]].path;
		DataMgr.settingData.controldata.Controler_QuickPanel = inputActions.Player.QuickPanel.bindings[dictionary_controllerbindingindex[UISetting.controlEnum.QuickPanel]].path;
		DataMgr.settingData.controldata.Controler_KillSummon = inputActions.Player.KillSummon.bindings[dictionary_controllerbindingindex[UISetting.controlEnum.KillSummon]].path;
		DataMgr.settingData.controldata.initalized = true;
		DataMgr.SaveSettingData();
		yield return new WaitForSecondsRealtime(0.2f);
		InputActionRecovering = false;
	}

	public void loadcontrol()
	{
		inputActions.Player.WASD.ApplyBindingOverride(dictionary_keybindingindex[UISetting.btnEnum.w], DataMgr.settingData.controldata.Key_w);
		inputActions.Player.WASD.ApplyBindingOverride(dictionary_keybindingindex[UISetting.btnEnum.s], DataMgr.settingData.controldata.Key_s);
		inputActions.Player.WASD.ApplyBindingOverride(dictionary_keybindingindex[UISetting.btnEnum.a], DataMgr.settingData.controldata.Key_a);
		inputActions.Player.WASD.ApplyBindingOverride(dictionary_keybindingindex[UISetting.btnEnum.d], DataMgr.settingData.controldata.Key_d);
		inputActions.Player.Shoot.ApplyBindingOverride(dictionary_keybindingindex[UISetting.btnEnum.shoot], DataMgr.settingData.controldata.Key_shoot);
		inputActions.Player.Interact.ApplyBindingOverride(dictionary_keybindingindex[UISetting.btnEnum.e], DataMgr.settingData.controldata.Key_e);
		inputActions.Player.Bag.ApplyBindingOverride(dictionary_keybindingindex[UISetting.btnEnum.bag], DataMgr.settingData.controldata.Key_bag);
		inputActions.Player.WandUp.ApplyBindingOverride(dictionary_keybindingindex[UISetting.btnEnum.switchwandup], DataMgr.settingData.controldata.Key_wandup);
		inputActions.Player.WandDown.ApplyBindingOverride(dictionary_keybindingindex[UISetting.btnEnum.switchwanddown], DataMgr.settingData.controldata.Key_wanddown);
		inputActions.Player.Alpha1.ApplyBindingOverride(dictionary_keybindingindex[UISetting.btnEnum.wand1], DataMgr.settingData.controldata.Key_wand1);
		inputActions.Player.Alpha2.ApplyBindingOverride(dictionary_keybindingindex[UISetting.btnEnum.wand2], DataMgr.settingData.controldata.Key_wand2);
		inputActions.Player.Alpha3.ApplyBindingOverride(dictionary_keybindingindex[UISetting.btnEnum.wand3], DataMgr.settingData.controldata.Key_wand3);
		inputActions.Player.Alpha4.ApplyBindingOverride(dictionary_keybindingindex[UISetting.btnEnum.wand4], DataMgr.settingData.controldata.Key_wand4);
		inputActions.Player.Alpha5.ApplyBindingOverride(dictionary_keybindingindex[UISetting.btnEnum.wand5], DataMgr.settingData.controldata.Key_wand5);
		inputActions.Player.Alpha6.ApplyBindingOverride(dictionary_keybindingindex[UISetting.btnEnum.wand6], DataMgr.settingData.controldata.Key_wand6);
		inputActions.Player.Drink.ApplyBindingOverride(dictionary_keybindingindex[UISetting.btnEnum.usepotion], DataMgr.settingData.controldata.Key_usepotion);
		inputActions.Player.PotionUp.ApplyBindingOverride(dictionary_keybindingindex[UISetting.btnEnum.switchpotionup], DataMgr.settingData.controldata.Key_potionup);
		inputActions.Player.PotionDown.ApplyBindingOverride(dictionary_keybindingindex[UISetting.btnEnum.switchpotiondown], DataMgr.settingData.controldata.Key_potiondown);
		inputActions.Player.QuickRemove.ApplyBindingOverride(dictionary_keybindingindex[UISetting.btnEnum.quickremove], DataMgr.settingData.controldata.Key_quickremove);
		if (string.IsNullOrEmpty(DataMgr.settingData.controldata.Key_Sprint))
		{
			DataMgr.settingData.controldata.Key_Sprint = inputActions.Player.Sprint.bindings[dictionary_keybindingindex[UISetting.btnEnum.Sprint]].path;
		}
		else
		{
			inputActions.Player.Sprint.ApplyBindingOverride(dictionary_keybindingindex[UISetting.btnEnum.Sprint], DataMgr.settingData.controldata.Key_Sprint);
		}
		if (string.IsNullOrEmpty(DataMgr.settingData.controldata.Key_QuickPanel))
		{
			DataMgr.settingData.controldata.Key_QuickPanel = inputActions.Player.QuickPanel.bindings[dictionary_keybindingindex[UISetting.btnEnum.QuickPanel]].path;
		}
		else
		{
			inputActions.Player.QuickPanel.ApplyBindingOverride(dictionary_keybindingindex[UISetting.btnEnum.QuickPanel], DataMgr.settingData.controldata.Key_QuickPanel);
		}
		if (string.IsNullOrEmpty(DataMgr.settingData.controldata.Key_KillSummon))
		{
			DataMgr.settingData.controldata.Key_KillSummon = inputActions.Player.KillSummon.bindings[dictionary_keybindingindex[UISetting.btnEnum.KillSummon]].path;
		}
		else
		{
			inputActions.Player.KillSummon.ApplyBindingOverride(dictionary_keybindingindex[UISetting.btnEnum.KillSummon], DataMgr.settingData.controldata.Key_KillSummon);
		}
		inputActions.Player.LeftStick.ApplyBindingOverride(dictionary_controllerbindingindex[UISetting.controlEnum.wasd], DataMgr.settingData.controldata.Controler_move);
		inputActions.Player.RightStick.ApplyBindingOverride(dictionary_controllerbindingindex[UISetting.controlEnum.aim], DataMgr.settingData.controldata.Controler_aim);
		inputActions.Player.Interact.ApplyBindingOverride(dictionary_controllerbindingindex[UISetting.controlEnum.e], DataMgr.settingData.controldata.Controler_interact);
		inputActions.Player.Shoot.ApplyBindingOverride(dictionary_controllerbindingindex[UISetting.controlEnum.shoot], DataMgr.settingData.controldata.Controler_shoot);
		inputActions.Player.Bag.ApplyBindingOverride(dictionary_controllerbindingindex[UISetting.controlEnum.bag], DataMgr.settingData.controldata.Controler_bag);
		inputActions.Player.WandUp.ApplyBindingOverride(dictionary_controllerbindingindex[UISetting.controlEnum.switchwandup], DataMgr.settingData.controldata.Controler_wandup);
		inputActions.Player.WandDown.ApplyBindingOverride(dictionary_controllerbindingindex[UISetting.controlEnum.switchwanddown], DataMgr.settingData.controldata.Controler_wanddown);
		inputActions.Player.Pause.ApplyBindingOverride(dictionary_controllerbindingindex[UISetting.controlEnum.menue], DataMgr.settingData.controldata.Controler_menue);
		inputActions.Player.Drink.ApplyBindingOverride(dictionary_controllerbindingindex[UISetting.controlEnum.usepotion], DataMgr.settingData.controldata.Controler_usepotion);
		inputActions.Player.GamepadEast.ApplyBindingOverride(dictionary_controllerbindingindex[UISetting.controlEnum.back], DataMgr.settingData.controldata.Controler_back);
		inputActions.Player.GamepadWest.ApplyBindingOverride(dictionary_controllerbindingindex[UISetting.controlEnum.moveobj], DataMgr.settingData.controldata.Controler_moveObj);
		if (string.IsNullOrEmpty(DataMgr.settingData.controldata.Controler_Sprint))
		{
			DataMgr.settingData.controldata.Controler_Sprint = inputActions.Player.Sprint.bindings[dictionary_controllerbindingindex[UISetting.controlEnum.Sprint]].path;
		}
		else
		{
			inputActions.Player.Sprint.ApplyBindingOverride(dictionary_controllerbindingindex[UISetting.controlEnum.Sprint], DataMgr.settingData.controldata.Controler_Sprint);
		}
		if (string.IsNullOrEmpty(DataMgr.settingData.controldata.Controler_QuickPanel))
		{
			DataMgr.settingData.controldata.Controler_QuickPanel = inputActions.Player.QuickPanel.bindings[dictionary_controllerbindingindex[UISetting.controlEnum.QuickPanel]].path;
		}
		else
		{
			inputActions.Player.QuickPanel.ApplyBindingOverride(dictionary_controllerbindingindex[UISetting.controlEnum.QuickPanel], DataMgr.settingData.controldata.Controler_QuickPanel);
		}
		if (string.IsNullOrEmpty(DataMgr.settingData.controldata.Controler_KillSummon))
		{
			DataMgr.settingData.controldata.Controler_KillSummon = inputActions.Player.KillSummon.bindings[dictionary_controllerbindingindex[UISetting.controlEnum.KillSummon]].path;
		}
		else
		{
			inputActions.Player.KillSummon.ApplyBindingOverride(dictionary_controllerbindingindex[UISetting.controlEnum.KillSummon], DataMgr.settingData.controldata.Controler_KillSummon);
		}
	}

	public void Changekey(InputAction action, controltype controltype, int keyindex, bool DeleteFirstCandidate = false)
	{
		if (InputActionRecovering)
		{
			return;
		}
		int bindingindex = ((controltype == controltype.key) ? dictionary_keybindingindex[(UISetting.btnEnum)keyindex] : dictionary_controllerbindingindex[(UISetting.controlEnum)keyindex]);
		Debug.Log("\ufffd\ufffd\ufffdÿ\ufffd\ufffd\ufffd");
		inputActions.Player.AnyGamepadInput.Disable();
		inputActions.Player.AnyKey.Disable();
		inputActions.Player.Pause.Disable();
		inputActions.Player.AnyMouseKeyboardInput.Disable();
		inputActions.Player.Disable();
		action.Disable();
		Debug.Log(bindingindex);
		InputActionRebindingExtensions.RebindingOperation operation2 = action.PerformInteractiveRebinding(bindingindex);
		operation2.OnPotentialMatch(delegate(InputActionRebindingExtensions.RebindingOperation operation)
		{
			foreach (InputControl candidate in operation.candidates)
			{
				Debug.Log("\ufffd\ufffdѡ\ufffd\ufffd\ufffd\ufffd" + candidate.path);
			}
			if (operation.candidates.Count > 0)
			{
				if (operation.candidates[0].path == "/Mouse/scroll/y")
				{
					operation.RemoveCandidate(operation.candidates[0]);
				}
				if (operation.candidates[0].path == "/Keyboard/escape")
				{
					operation.RemoveCandidate(operation.candidates[0]);
					operation.Cancel();
					return;
				}
				if (UIMgr.Inst.InputType == PlayerInputType.Gamepad || GameMgr.IsSteamDeck_Static)
				{
					switch (operation.candidates[0].name)
					{
					case "start":
					case "select":
					case "option":
					case "touchpadButton":
					case "systemButton":
					case "leftStickPress":
					case "rightStickPress":
						operation.Cancel();
						return;
					}
					string text = operation.candidates[0].path.Split('/')[1];
					if (text == "Keyboard" || text == "Mouse")
					{
						operation.Cancel();
						return;
					}
				}
				else if (UIMgr.Inst.InputType == PlayerInputType.Keyboard)
				{
					if (action == inputActions.Player.WASD && operation.candidates[0].path.Split('/')[1] == "Mouse")
					{
						operation.Cancel();
						return;
					}
					if (operation.candidates[0].path.Split('/')[1] == "XInputControllerWindows")
					{
						operation.Cancel();
						return;
					}
					if (operation.candidates[0].path.Split('/')[1] == "DualShock3GamepadHID" || operation.candidates[0].path.Split('/')[1] == "DualShock4GamepadHID")
					{
						operation.Cancel();
						return;
					}
				}
			}
			if (controltype == controltype.key)
			{
				(InputAction, int) tuple = CompareActionKeyControl(keyindex, operation.candidates[0].path);
				if (tuple.Item1 != null)
				{
					Debug.Log(action.name + "\ufffd\ufffd\u02f0\ufffd\ufffd\ufffd\ufffdظ\ufffd\ufffdˣ\ufffd\ufffd\ufffd\ufffd\ufffd:" + tuple.Item1.name);
					tuple.Item1.ApplyBindingOverride(tuple.Item2, action.bindings[bindingindex].effectivePath);
				}
			}
			else if (operation.action == inputActions.Player.LeftStick && PathToName(operation.candidates[0].path) != "leftStick" && PathToName(operation.candidates[0].path) != "rightStick")
			{
				operation.Cancel();
			}
			else if (operation.action == inputActions.Player.RightStick && PathToName(operation.candidates[0].path) != "leftStick" && PathToName(operation.candidates[0].path) != "rightStick")
			{
				operation.Cancel();
			}
			else if (operation.action != inputActions.Player.RightStick && operation.action != inputActions.Player.LeftStick && (PathToName(operation.candidates[0].path, 1) == "leftStick" || PathToName(operation.candidates[0].path, 1) == "rightStick" || PathToName(operation.candidates[0].path, 1) == "dpad"))
			{
				operation.Cancel();
			}
			else
			{
				(InputAction, int) tuple2 = CompareActionControllerControl(keyindex, operation.candidates[0].path);
				if (tuple2.Item1 != null)
				{
					Debug.Log(action.name + "\ufffd\ufffd\u02f0\ufffd\ufffd\ufffd\ufffdظ\ufffd\ufffdˣ\ufffd\ufffd\ufffd\ufffd\ufffd:" + tuple2.Item1.name + "bindingindex" + dictionary_controllerbindingindex[(UISetting.controlEnum)keyindex]);
					tuple2.Item1.ApplyBindingOverride(tuple2.Item2, action.bindings[bindingindex].effectivePath);
				}
			}
		}).OnComplete(delegate(InputActionRebindingExtensions.RebindingOperation operation)
		{
			ChangeKeyOnComplete(action, operation, controltype);
		}).OnCancel(delegate
		{
			ChangeKeyOnCancle();
		})
			.Start();
		rebinding = true;
		UIMgr.Inst.uiSetting.ShowWaitPress();
		void ChangeKeyOnCancle()
		{
			Debug.Log("ȡ\ufffd\ufffd\ufffd\ufffd");
			action.Enable();
			UIMgr.Inst.uiSetting.HideWaitPress();
			StartCoroutine(delayenablecontrol());
			rebinding = false;
			operation2.Dispose();
		}
		void ChangeKeyOnComplete(InputAction action, InputActionRebindingExtensions.RebindingOperation operation, controltype type)
		{
			Debug.Log("\ufffdļ\ufffd\ufffdɹ\ufffd:" + action.name + ">" + bindingindex + "/" + operation.candidates[0]);
			rebinding = false;
			SaveAllControl();
			EventMgr.ControlChange();
			StartCoroutine(delayenablecontrol());
			UIMgr.Inst.uiSetting.UpdateControlShow();
			UIMgr.Inst.uiSetting.SetAllKeyBackground();
			UIMgr.Inst.uiSetting.HideWaitPress();
		}
		void SaveAllControl()
		{
			DataMgr.settingData.controldata.Key_w = inputActions.Player.WASD.bindings[dictionary_keybindingindex[UISetting.btnEnum.w]].effectivePath;
			DataMgr.settingData.controldata.Key_s = inputActions.Player.WASD.bindings[dictionary_keybindingindex[UISetting.btnEnum.s]].effectivePath;
			DataMgr.settingData.controldata.Key_a = inputActions.Player.WASD.bindings[dictionary_keybindingindex[UISetting.btnEnum.a]].effectivePath;
			DataMgr.settingData.controldata.Key_d = inputActions.Player.WASD.bindings[dictionary_keybindingindex[UISetting.btnEnum.d]].effectivePath;
			DataMgr.settingData.controldata.Key_shoot = inputActions.Player.Shoot.bindings[dictionary_keybindingindex[UISetting.btnEnum.shoot]].effectivePath;
			DataMgr.settingData.controldata.Key_e = inputActions.Player.Interact.bindings[dictionary_keybindingindex[UISetting.btnEnum.e]].effectivePath;
			DataMgr.settingData.controldata.Key_bag = inputActions.Player.Bag.bindings[dictionary_keybindingindex[UISetting.btnEnum.bag]].effectivePath;
			DataMgr.settingData.controldata.Key_wandup = inputActions.Player.WandUp.bindings[dictionary_keybindingindex[UISetting.btnEnum.switchwandup]].effectivePath;
			DataMgr.settingData.controldata.Key_wanddown = inputActions.Player.WandDown.bindings[dictionary_keybindingindex[UISetting.btnEnum.switchwanddown]].effectivePath;
			DataMgr.settingData.controldata.Key_wand1 = inputActions.Player.Alpha1.bindings[dictionary_keybindingindex[UISetting.btnEnum.wand1]].effectivePath;
			DataMgr.settingData.controldata.Key_wand2 = inputActions.Player.Alpha2.bindings[dictionary_keybindingindex[UISetting.btnEnum.wand2]].effectivePath;
			DataMgr.settingData.controldata.Key_wand3 = inputActions.Player.Alpha3.bindings[dictionary_keybindingindex[UISetting.btnEnum.wand3]].effectivePath;
			DataMgr.settingData.controldata.Key_wand4 = inputActions.Player.Alpha4.bindings[dictionary_keybindingindex[UISetting.btnEnum.wand4]].effectivePath;
			DataMgr.settingData.controldata.Key_wand5 = inputActions.Player.Alpha5.bindings[dictionary_keybindingindex[UISetting.btnEnum.wand5]].effectivePath;
			DataMgr.settingData.controldata.Key_wand6 = inputActions.Player.Alpha6.bindings[dictionary_keybindingindex[UISetting.btnEnum.wand6]].effectivePath;
			DataMgr.settingData.controldata.Key_usepotion = inputActions.Player.Drink.bindings[dictionary_keybindingindex[UISetting.btnEnum.usepotion]].effectivePath;
			DataMgr.settingData.controldata.Key_potionup = inputActions.Player.PotionUp.bindings[dictionary_keybindingindex[UISetting.btnEnum.switchpotiondown]].effectivePath;
			DataMgr.settingData.controldata.Key_potiondown = inputActions.Player.PotionDown.bindings[dictionary_keybindingindex[UISetting.btnEnum.switchpotiondown]].effectivePath;
			DataMgr.settingData.controldata.Key_quickremove = inputActions.Player.QuickRemove.bindings[dictionary_keybindingindex[UISetting.btnEnum.quickremove]].effectivePath;
			DataMgr.settingData.controldata.Key_Sprint = inputActions.Player.Sprint.bindings[dictionary_keybindingindex[UISetting.btnEnum.Sprint]].effectivePath;
			DataMgr.settingData.controldata.Key_QuickPanel = inputActions.Player.QuickPanel.bindings[dictionary_keybindingindex[UISetting.btnEnum.QuickPanel]].effectivePath;
			DataMgr.settingData.controldata.Key_KillSummon = inputActions.Player.KillSummon.bindings[dictionary_keybindingindex[UISetting.btnEnum.KillSummon]].effectivePath;
			DataMgr.settingData.controldata.Controler_move = inputActions.Player.LeftStick.bindings[dictionary_controllerbindingindex[UISetting.controlEnum.wasd]].effectivePath;
			DataMgr.settingData.controldata.Controler_aim = inputActions.Player.RightStick.bindings[dictionary_controllerbindingindex[UISetting.controlEnum.aim]].effectivePath;
			DataMgr.settingData.controldata.Controler_interact = inputActions.Player.Interact.bindings[dictionary_controllerbindingindex[UISetting.controlEnum.e]].effectivePath;
			DataMgr.settingData.controldata.Controler_shoot = inputActions.Player.Shoot.bindings[dictionary_controllerbindingindex[UISetting.controlEnum.shoot]].effectivePath;
			DataMgr.settingData.controldata.Controler_bag = inputActions.Player.Bag.bindings[dictionary_controllerbindingindex[UISetting.controlEnum.bag]].effectivePath;
			DataMgr.settingData.controldata.Controler_wandup = inputActions.Player.WandUp.bindings[dictionary_controllerbindingindex[UISetting.controlEnum.switchwandup]].effectivePath;
			DataMgr.settingData.controldata.Controler_wanddown = inputActions.Player.WandDown.bindings[dictionary_controllerbindingindex[UISetting.controlEnum.switchwanddown]].effectivePath;
			DataMgr.settingData.controldata.Controler_usepotion = inputActions.Player.Drink.bindings[dictionary_controllerbindingindex[UISetting.controlEnum.usepotion]].effectivePath;
			DataMgr.settingData.controldata.Controler_moveObj = inputActions.Player.GamepadWest.bindings[dictionary_controllerbindingindex[UISetting.controlEnum.moveobj]].effectivePath;
			DataMgr.settingData.controldata.Controler_back = inputActions.Player.GamepadEast.bindings[dictionary_controllerbindingindex[UISetting.controlEnum.back]].effectivePath;
			DataMgr.settingData.controldata.Controler_menue = inputActions.Player.Pause.bindings[dictionary_controllerbindingindex[UISetting.controlEnum.menue]].effectivePath;
			DataMgr.settingData.controldata.Controler_Sprint = inputActions.Player.Sprint.bindings[dictionary_controllerbindingindex[UISetting.controlEnum.Sprint]].effectivePath;
			DataMgr.settingData.controldata.Controler_QuickPanel = inputActions.Player.QuickPanel.bindings[dictionary_controllerbindingindex[UISetting.controlEnum.QuickPanel]].effectivePath;
			DataMgr.settingData.controldata.Controler_KillSummon = inputActions.Player.KillSummon.bindings[dictionary_controllerbindingindex[UISetting.controlEnum.KillSummon]].effectivePath;
			DataMgr.settingData.controldata.initalized = true;
			DataMgr.SaveSettingData();
		}
		IEnumerator delayenablecontrol()
		{
			inputActions.Player.AnyGamepadInput.Enable();
			inputActions.Player.AnyKey.Enable();
			inputActions.Player.Pause.Enable();
			inputActions.Player.AnyMouseKeyboardInput.Enable();
			InputActionRecovering = true;
			action.Enable();
			inputActions.Player.Enable();
			yield return new WaitForSecondsRealtime(0.2f);
			InputActionRecovering = false;
			Debug.Log("\ufffd\u05b8\ufffd\ufffd\ufffd\ufffd\ufffd");
			operation2.Dispose();
		}
	}

	private void initializekeycontrol()
	{
		DataMgr.settingData.controldata.Key_w = inputActions.Player.WASD.bindings[dictionary_keybindingindex[UISetting.btnEnum.w]].path;
		DataMgr.settingData.controldata.Key_s = inputActions.Player.WASD.bindings[dictionary_keybindingindex[UISetting.btnEnum.s]].path;
		DataMgr.settingData.controldata.Key_a = inputActions.Player.WASD.bindings[dictionary_keybindingindex[UISetting.btnEnum.a]].path;
		DataMgr.settingData.controldata.Key_d = inputActions.Player.WASD.bindings[dictionary_keybindingindex[UISetting.btnEnum.d]].path;
		DataMgr.settingData.controldata.Key_shoot = inputActions.Player.Shoot.bindings[dictionary_keybindingindex[UISetting.btnEnum.shoot]].path;
		DataMgr.settingData.controldata.Key_e = inputActions.Player.Interact.bindings[dictionary_keybindingindex[UISetting.btnEnum.e]].path;
		DataMgr.settingData.controldata.Key_bag = inputActions.Player.Bag.bindings[dictionary_keybindingindex[UISetting.btnEnum.bag]].path;
		DataMgr.settingData.controldata.Key_wandup = inputActions.Player.WandUp.bindings[dictionary_keybindingindex[UISetting.btnEnum.switchwandup]].path;
		DataMgr.settingData.controldata.Key_wanddown = inputActions.Player.WandDown.bindings[dictionary_keybindingindex[UISetting.btnEnum.switchwanddown]].path;
		DataMgr.settingData.controldata.Key_wand1 = inputActions.Player.Alpha1.bindings[dictionary_keybindingindex[UISetting.btnEnum.wand1]].path;
		DataMgr.settingData.controldata.Key_wand2 = inputActions.Player.Alpha2.bindings[dictionary_keybindingindex[UISetting.btnEnum.wand2]].path;
		DataMgr.settingData.controldata.Key_wand3 = inputActions.Player.Alpha3.bindings[dictionary_keybindingindex[UISetting.btnEnum.wand3]].path;
		DataMgr.settingData.controldata.Key_wand4 = inputActions.Player.Alpha4.bindings[dictionary_keybindingindex[UISetting.btnEnum.wand4]].path;
		DataMgr.settingData.controldata.Key_wand5 = inputActions.Player.Alpha5.bindings[dictionary_keybindingindex[UISetting.btnEnum.wand5]].path;
		DataMgr.settingData.controldata.Key_wand6 = inputActions.Player.Alpha6.bindings[dictionary_keybindingindex[UISetting.btnEnum.wand6]].path;
		DataMgr.settingData.controldata.Key_usepotion = inputActions.Player.Drink.bindings[dictionary_keybindingindex[UISetting.btnEnum.usepotion]].path;
		DataMgr.settingData.controldata.Key_potionup = inputActions.Player.PotionUp.bindings[dictionary_keybindingindex[UISetting.btnEnum.switchpotiondown]].path;
		DataMgr.settingData.controldata.Key_potiondown = inputActions.Player.PotionDown.bindings[dictionary_keybindingindex[UISetting.btnEnum.switchpotiondown]].path;
		DataMgr.settingData.controldata.Key_quickremove = inputActions.Player.QuickRemove.bindings[dictionary_keybindingindex[UISetting.btnEnum.quickremove]].path;
		DataMgr.settingData.controldata.Key_Sprint = inputActions.Player.Sprint.bindings[dictionary_keybindingindex[UISetting.btnEnum.Sprint]].path;
		DataMgr.settingData.controldata.Key_QuickPanel = inputActions.Player.QuickPanel.bindings[dictionary_keybindingindex[UISetting.btnEnum.QuickPanel]].path;
		DataMgr.settingData.controldata.Key_KillSummon = inputActions.Player.KillSummon.bindings[dictionary_keybindingindex[UISetting.btnEnum.KillSummon]].path;
		DataMgr.settingData.controldata.Controler_move = inputActions.Player.LeftStick.bindings[dictionary_controllerbindingindex[UISetting.controlEnum.wasd]].path;
		DataMgr.settingData.controldata.Controler_aim = inputActions.Player.RightStick.bindings[dictionary_controllerbindingindex[UISetting.controlEnum.aim]].path;
		DataMgr.settingData.controldata.Controler_interact = inputActions.Player.Interact.bindings[dictionary_controllerbindingindex[UISetting.controlEnum.e]].path;
		DataMgr.settingData.controldata.Controler_shoot = inputActions.Player.Shoot.bindings[dictionary_controllerbindingindex[UISetting.controlEnum.shoot]].path;
		DataMgr.settingData.controldata.Controler_bag = inputActions.Player.Bag.bindings[dictionary_controllerbindingindex[UISetting.controlEnum.bag]].path;
		DataMgr.settingData.controldata.Controler_wandup = inputActions.Player.WandUp.bindings[dictionary_controllerbindingindex[UISetting.controlEnum.switchwandup]].path;
		DataMgr.settingData.controldata.Controler_wanddown = inputActions.Player.WandDown.bindings[dictionary_controllerbindingindex[UISetting.controlEnum.switchwanddown]].path;
		DataMgr.settingData.controldata.Controler_usepotion = inputActions.Player.Drink.bindings[dictionary_controllerbindingindex[UISetting.controlEnum.usepotion]].path;
		DataMgr.settingData.controldata.Controler_moveObj = inputActions.Player.GamepadWest.bindings[dictionary_controllerbindingindex[UISetting.controlEnum.moveobj]].path;
		DataMgr.settingData.controldata.Controler_back = inputActions.Player.GamepadEast.bindings[dictionary_controllerbindingindex[UISetting.controlEnum.back]].path;
		DataMgr.settingData.controldata.Controler_menue = inputActions.Player.Pause.bindings[dictionary_controllerbindingindex[UISetting.controlEnum.menue]].path;
		DataMgr.settingData.controldata.Controler_Sprint = inputActions.Player.Sprint.bindings[dictionary_controllerbindingindex[UISetting.controlEnum.Sprint]].path;
		DataMgr.settingData.controldata.Controler_QuickPanel = inputActions.Player.QuickPanel.bindings[dictionary_controllerbindingindex[UISetting.controlEnum.QuickPanel]].path;
		DataMgr.settingData.controldata.Controler_KillSummon = inputActions.Player.KillSummon.bindings[dictionary_controllerbindingindex[UISetting.controlEnum.KillSummon]].path;
		DataMgr.settingData.controldata.initalized = true;
	}

	private string SetbindingString(string path)
	{
		string[] array = path.Split(charsplit);
		string text = "";
		if (array.Length == 3)
		{
			return "/" + array[1] + array[2];
		}
		if (array.Length == 1)
		{
			return path;
		}
		return null;
	}

	private IEnumerator DelayChangeControl()
	{
		yield return new WaitForSeconds(0.1f);
		EventMgr.ControlChange?.Invoke();
	}

	private (InputAction inputAction, int id) CompareActionKeyControl(int keyindex, string path)
	{
		if (SetbindingString(inputActions.Player.WASD.bindings[1].effectivePath) == path && keyindex != 0)
		{
			return (inputActions.Player.WASD, dictionary_keybindingindex[UISetting.btnEnum.w]);
		}
		if (SetbindingString(inputActions.Player.WASD.bindings[2].effectivePath) == path && keyindex != 1)
		{
			return (inputActions.Player.WASD, dictionary_keybindingindex[UISetting.btnEnum.s]);
		}
		if (SetbindingString(inputActions.Player.WASD.bindings[3].effectivePath) == path && keyindex != 2)
		{
			return (inputActions.Player.WASD, dictionary_keybindingindex[UISetting.btnEnum.a]);
		}
		if (SetbindingString(inputActions.Player.WASD.bindings[4].effectivePath) == path && keyindex != 3)
		{
			return (inputActions.Player.WASD, dictionary_keybindingindex[UISetting.btnEnum.d]);
		}
		if (SetbindingString(inputActions.Player.Shoot.bindings[0].effectivePath) == path && keyindex != 4)
		{
			return (inputActions.Player.Shoot, dictionary_keybindingindex[UISetting.btnEnum.shoot]);
		}
		if (SetbindingString(inputActions.Player.Interact.bindings[0].effectivePath) == path && keyindex != 5)
		{
			return (inputActions.Player.Interact, dictionary_keybindingindex[UISetting.btnEnum.e]);
		}
		if (SetbindingString(inputActions.Player.Bag.bindings[0].effectivePath) == path && keyindex != 6)
		{
			return (inputActions.Player.Bag, dictionary_keybindingindex[UISetting.btnEnum.bag]);
		}
		if (SetbindingString(inputActions.Player.WandUp.bindings[0].effectivePath) == path && keyindex != 7)
		{
			return (inputActions.Player.WandUp, dictionary_keybindingindex[UISetting.btnEnum.switchwandup]);
		}
		if (SetbindingString(inputActions.Player.WandDown.bindings[0].effectivePath) == path && keyindex != 8)
		{
			return (inputActions.Player.WandDown, dictionary_keybindingindex[UISetting.btnEnum.switchwanddown]);
		}
		if (SetbindingString(inputActions.Player.Alpha1.bindings[0].effectivePath) == path && keyindex != 9)
		{
			return (inputActions.Player.Alpha1, dictionary_keybindingindex[UISetting.btnEnum.wand1]);
		}
		if (SetbindingString(inputActions.Player.Alpha2.bindings[0].effectivePath) == path && keyindex != 10)
		{
			return (inputActions.Player.Alpha2, dictionary_keybindingindex[UISetting.btnEnum.wand2]);
		}
		if (SetbindingString(inputActions.Player.Alpha3.bindings[0].effectivePath) == path && keyindex != 11)
		{
			return (inputActions.Player.Alpha3, dictionary_keybindingindex[UISetting.btnEnum.wand3]);
		}
		if (SetbindingString(inputActions.Player.Alpha4.bindings[0].effectivePath) == path && keyindex != 12)
		{
			return (inputActions.Player.Alpha4, dictionary_keybindingindex[UISetting.btnEnum.wand4]);
		}
		if (SetbindingString(inputActions.Player.Alpha5.bindings[0].effectivePath) == path && keyindex != 13)
		{
			return (inputActions.Player.Alpha5, dictionary_keybindingindex[UISetting.btnEnum.wand5]);
		}
		if (SetbindingString(inputActions.Player.Alpha6.bindings[0].effectivePath) == path && keyindex != 14)
		{
			return (inputActions.Player.Alpha6, dictionary_keybindingindex[UISetting.btnEnum.wand6]);
		}
		if (SetbindingString(inputActions.Player.Drink.bindings[0].effectivePath) == path && keyindex != 15)
		{
			return (inputActions.Player.Drink, dictionary_keybindingindex[UISetting.btnEnum.usepotion]);
		}
		if (SetbindingString(inputActions.Player.PotionUp.bindings[0].effectivePath) == path && keyindex != 16)
		{
			return (inputActions.Player.PotionUp, dictionary_keybindingindex[UISetting.btnEnum.switchpotionup]);
		}
		if (SetbindingString(inputActions.Player.PotionDown.bindings[0].effectivePath) == path && keyindex != 17)
		{
			return (inputActions.Player.PotionDown, dictionary_keybindingindex[UISetting.btnEnum.switchpotiondown]);
		}
		if (SetbindingString(inputActions.Player.Sprint.bindings[0].effectivePath) == path && keyindex != 19)
		{
			return (inputActions.Player.Sprint, dictionary_keybindingindex[UISetting.btnEnum.Sprint]);
		}
		if (SetbindingString(inputActions.Player.QuickPanel.bindings[0].effectivePath) == path && keyindex != 24)
		{
			return (inputActions.Player.QuickPanel, dictionary_keybindingindex[UISetting.btnEnum.QuickPanel]);
		}
		if (SetbindingString(inputActions.Player.KillSummon.bindings[0].effectivePath) == path && keyindex != 25)
		{
			return (inputActions.Player.KillSummon, dictionary_keybindingindex[UISetting.btnEnum.KillSummon]);
		}
		return (null, 0);
	}

	private (InputAction inputAction, int id) CompareActionControllerControl(int keyindex, string path)
	{
		new InputAction();
		if (PathToName(inputActions.Player.LeftStick.bindings[0].effectivePath) == PathToName(path) && keyindex != 0)
		{
			return (inputActions.Player.LeftStick, dictionary_controllerbindingindex[UISetting.controlEnum.wasd]);
		}
		if (PathToName(inputActions.Player.RightStick.bindings[0].effectivePath) == PathToName(path) && keyindex != 1)
		{
			return (inputActions.Player.RightStick, dictionary_controllerbindingindex[UISetting.controlEnum.aim]);
		}
		if (PathToName(inputActions.Player.Interact.bindings[1].effectivePath) == PathToName(path) && keyindex != 2)
		{
			return (inputActions.Player.Interact, dictionary_controllerbindingindex[UISetting.controlEnum.e]);
		}
		if (PathToName(inputActions.Player.Shoot.bindings[1].effectivePath) == PathToName(path) && keyindex != 3)
		{
			return (inputActions.Player.Shoot, dictionary_controllerbindingindex[UISetting.controlEnum.shoot]);
		}
		if (PathToName(inputActions.Player.Bag.bindings[1].effectivePath) == PathToName(path) && keyindex != 4)
		{
			return (inputActions.Player.Bag, dictionary_controllerbindingindex[UISetting.controlEnum.bag]);
		}
		if (PathToName(inputActions.Player.WandUp.bindings[1].effectivePath) == PathToName(path) && keyindex != 5)
		{
			return (inputActions.Player.WandUp, dictionary_controllerbindingindex[UISetting.controlEnum.switchwandup]);
		}
		if (PathToName(inputActions.Player.WandDown.bindings[1].effectivePath) == PathToName(path) && keyindex != 6)
		{
			return (inputActions.Player.WandDown, dictionary_controllerbindingindex[UISetting.controlEnum.switchwanddown]);
		}
		if (PathToName(inputActions.Player.Pause.bindings[1].effectivePath) == PathToName(path) && keyindex != 7)
		{
			return (inputActions.Player.Pause, dictionary_controllerbindingindex[UISetting.controlEnum.menue]);
		}
		if (PathToName(inputActions.Player.Drink.bindings[1].effectivePath) == PathToName(path) && keyindex != 8)
		{
			return (inputActions.Player.Drink, dictionary_controllerbindingindex[UISetting.controlEnum.usepotion]);
		}
		if (PathToName(inputActions.Player.GamepadWest.bindings[0].effectivePath) == PathToName(path) && keyindex != 11)
		{
			return (inputActions.Player.GamepadWest, dictionary_controllerbindingindex[UISetting.controlEnum.moveobj]);
		}
		if (PathToName(inputActions.Player.GamepadEast.bindings[0].effectivePath) == PathToName(path) && keyindex != 12)
		{
			return (inputActions.Player.GamepadEast, dictionary_controllerbindingindex[UISetting.controlEnum.back]);
		}
		return (null, 0);
	}

	private string PathToName(string path, int i = 0)
	{
		string[] array = path.Split('/');
		if (array.Length <= i + 1)
		{
			return array[^1];
		}
		return array[array.Length - 1 - i];
	}

	public static string GetShowNameByEnum(UISetting.btnEnum btnEnum, UISetting.controlEnum controlEnum)
	{
		if (UIMgr.Inst.InputType == PlayerInputType.Gamepad)
		{
			return controlEnum switch
			{
				UISetting.controlEnum.wasd => GetKeyDisplayName(Inst.inputActions.Player.LeftStick, 0), 
				UISetting.controlEnum.aim => GetKeyDisplayName(Inst.inputActions.Player.RightStick, 0), 
				UISetting.controlEnum.e => GetKeyDisplayName(Inst.inputActions.Player.Interact, 1), 
				UISetting.controlEnum.shoot => GetKeyDisplayName(Inst.inputActions.Player.Shoot, 1), 
				UISetting.controlEnum.bag => GetKeyDisplayName(Inst.inputActions.Player.Bag, 1), 
				UISetting.controlEnum.switchwandup => GetKeyDisplayName(Inst.inputActions.Player.WandUp, 1), 
				UISetting.controlEnum.switchwanddown => GetKeyDisplayName(Inst.inputActions.Player.WandDown, 1), 
				UISetting.controlEnum.menue => GetKeyDisplayName(Inst.inputActions.Player.Pause, 1), 
				UISetting.controlEnum.usepotion => GetKeyDisplayName(Inst.inputActions.Player.Drink, 1), 
				UISetting.controlEnum.switchpotion => GetKeyDisplayName(Inst.inputActions.Player.PotionDown, 1), 
				UISetting.controlEnum.throwaway => GetKeyDisplayName(Inst.inputActions.Player.GamepadWest, 0), 
				UISetting.controlEnum.moveobj => GetKeyDisplayName(Inst.inputActions.Player.GamepadWest, 0), 
				UISetting.controlEnum.back => GetKeyDisplayName(Inst.inputActions.Player.GamepadEast, 0), 
				UISetting.controlEnum.KillSummon => GetKeyDisplayName(Inst.inputActions.Player.KillSummon, 0), 
				UISetting.controlEnum.QuickPanel => GetKeyDisplayName(Inst.inputActions.Player.QuickPanel, 0), 
				_ => null, 
			};
		}
		return btnEnum switch
		{
			UISetting.btnEnum.w => GetKeyDisplayName(Inst.inputActions.Player.WASD, 1), 
			UISetting.btnEnum.s => GetKeyDisplayName(Inst.inputActions.Player.WASD, 2), 
			UISetting.btnEnum.a => GetKeyDisplayName(Inst.inputActions.Player.WASD, 3), 
			UISetting.btnEnum.d => GetKeyDisplayName(Inst.inputActions.Player.WASD, 4), 
			UISetting.btnEnum.shoot => GetKeyDisplayName(Inst.inputActions.Player.Shoot, 0), 
			UISetting.btnEnum.e => GetKeyDisplayName(Inst.inputActions.Player.Interact, 0), 
			UISetting.btnEnum.bag => GetKeyDisplayName(Inst.inputActions.Player.Bag, 0), 
			UISetting.btnEnum.switchwandup => GetKeyDisplayName(Inst.inputActions.Player.WandUp, 0), 
			UISetting.btnEnum.switchwanddown => GetKeyDisplayName(Inst.inputActions.Player.WandDown, 0), 
			UISetting.btnEnum.wand1 => GetKeyDisplayName(Inst.inputActions.Player.Alpha1, 0), 
			UISetting.btnEnum.wand2 => GetKeyDisplayName(Inst.inputActions.Player.Alpha2, 0), 
			UISetting.btnEnum.wand3 => GetKeyDisplayName(Inst.inputActions.Player.Alpha3, 0), 
			UISetting.btnEnum.wand4 => GetKeyDisplayName(Inst.inputActions.Player.Alpha4, 0), 
			UISetting.btnEnum.wand5 => GetKeyDisplayName(Inst.inputActions.Player.Alpha5, 0), 
			UISetting.btnEnum.wand6 => GetKeyDisplayName(Inst.inputActions.Player.Alpha6, 0), 
			UISetting.btnEnum.usepotion => GetKeyDisplayName(Inst.inputActions.Player.Drink, 0), 
			UISetting.btnEnum.switchpotionup => GetKeyDisplayName(Inst.inputActions.Player.PotionUp, 0), 
			UISetting.btnEnum.switchpotiondown => GetKeyDisplayName(Inst.inputActions.Player.PotionDown, 0), 
			UISetting.btnEnum.quickremove => GetKeyDisplayName(Inst.inputActions.Player.QuickRemove, 0), 
			UISetting.btnEnum.Sprint => GetKeyDisplayName(Inst.inputActions.Player.Sprint, 0), 
			UISetting.btnEnum.KillSummon => GetKeyDisplayName(Inst.inputActions.Player.KillSummon, 0), 
			UISetting.btnEnum.QuickPanel => GetKeyDisplayName(Inst.inputActions.Player.QuickPanel, 0), 
			UISetting.btnEnum.QcantChange => "Q", 
			UISetting.btnEnum.EcantChange => "E", 
			UISetting.btnEnum.AcantChange => "A", 
			UISetting.btnEnum.DcantChange => "D", 
			_ => throw new ArgumentOutOfRangeException("btnEnum", btnEnum, null), 
		};
	}

	public static string GetKeyDisplayName(InputAction inputAction, int index)
	{
		return GetKeyDisplayName(inputAction.GetBindingDisplayString(index));
	}

	public static string GetKeyDisplayName(string keyName)
	{
		switch (keyName)
		{
		case "Space":
			keyName = 1000165.GetText();
			break;
		case "LMB":
			keyName = 1000166.GetText();
			break;
		case "RMB":
			keyName = 1000167.GetText();
			break;
		case "Shift":
			keyName = "Left Shift";
			break;
		}
		return keyName;
	}

	public static string GetKeyBindingRootByEnum(UISetting.btnEnum btnEnum, UISetting.controlEnum controlEnum)
	{
		if (UIMgr.Inst.InputType == PlayerInputType.Gamepad || GameMgr.IsSteamDeck_Static)
		{
			return controlEnum switch
			{
				UISetting.controlEnum.wasd => DataMgr.settingData.controldata.Controler_move, 
				UISetting.controlEnum.aim => DataMgr.settingData.controldata.Controler_aim, 
				UISetting.controlEnum.e => DataMgr.settingData.controldata.Controler_interact, 
				UISetting.controlEnum.shoot => DataMgr.settingData.controldata.Controler_shoot, 
				UISetting.controlEnum.bag => DataMgr.settingData.controldata.Controler_bag, 
				UISetting.controlEnum.switchwandup => DataMgr.settingData.controldata.Controler_wandup, 
				UISetting.controlEnum.switchwanddown => DataMgr.settingData.controldata.Controler_wanddown, 
				UISetting.controlEnum.menue => DataMgr.settingData.controldata.Controler_menue, 
				UISetting.controlEnum.usepotion => DataMgr.settingData.controldata.Controler_usepotion, 
				UISetting.controlEnum.throwaway => DataMgr.settingData.controldata.Controler_moveObj, 
				UISetting.controlEnum.switchpotion => DataMgr.settingData.controldata.Controler_usepotion, 
				UISetting.controlEnum.moveobj => DataMgr.settingData.controldata.Controler_moveObj, 
				UISetting.controlEnum.back => DataMgr.settingData.controldata.Controler_back, 
				UISetting.controlEnum.KillSummon => DataMgr.settingData.controldata.Controler_KillSummon, 
				UISetting.controlEnum.QuickPanel => DataMgr.settingData.controldata.Controler_QuickPanel, 
				UISetting.controlEnum.LB => "<Gamepad>/leftShoulder", 
				UISetting.controlEnum.RB => "<Gamepad>/rightShoulder", 
				UISetting.controlEnum.LT => "<Gamepad>/leftTrigger", 
				UISetting.controlEnum.RT => "<Gamepad>/rightTrigger", 
				_ => null, 
			};
		}
		return btnEnum switch
		{
			UISetting.btnEnum.w => DataMgr.settingData.controldata.Key_w, 
			UISetting.btnEnum.a => DataMgr.settingData.controldata.Key_a, 
			UISetting.btnEnum.s => DataMgr.settingData.controldata.Key_s, 
			UISetting.btnEnum.d => DataMgr.settingData.controldata.Key_d, 
			UISetting.btnEnum.shoot => DataMgr.settingData.controldata.Key_shoot, 
			UISetting.btnEnum.e => DataMgr.settingData.controldata.Key_e, 
			UISetting.btnEnum.bag => DataMgr.settingData.controldata.Key_bag, 
			UISetting.btnEnum.switchwandup => DataMgr.settingData.controldata.Key_wandup, 
			UISetting.btnEnum.switchwanddown => DataMgr.settingData.controldata.Key_wanddown, 
			UISetting.btnEnum.wand1 => DataMgr.settingData.controldata.Key_wand1, 
			UISetting.btnEnum.wand2 => DataMgr.settingData.controldata.Key_wand2, 
			UISetting.btnEnum.wand3 => DataMgr.settingData.controldata.Key_wand3, 
			UISetting.btnEnum.wand4 => DataMgr.settingData.controldata.Key_wand4, 
			UISetting.btnEnum.wand5 => DataMgr.settingData.controldata.Key_wand5, 
			UISetting.btnEnum.wand6 => DataMgr.settingData.controldata.Key_wand6, 
			UISetting.btnEnum.usepotion => DataMgr.settingData.controldata.Key_usepotion, 
			UISetting.btnEnum.switchpotionup => DataMgr.settingData.controldata.Key_potionup, 
			UISetting.btnEnum.switchpotiondown => DataMgr.settingData.controldata.Key_potiondown, 
			UISetting.btnEnum.quickremove => DataMgr.settingData.controldata.Key_quickremove, 
			UISetting.btnEnum.Sprint => DataMgr.settingData.controldata.Key_Sprint, 
			UISetting.btnEnum.KillSummon => DataMgr.settingData.controldata.Key_KillSummon, 
			UISetting.btnEnum.QuickPanel => DataMgr.settingData.controldata.Key_QuickPanel, 
			UISetting.btnEnum.QcantChange => "<Keyboard>/q", 
			UISetting.btnEnum.EcantChange => "<Keyboard>/e", 
			UISetting.btnEnum.AcantChange => "<Keyboard>/a", 
			UISetting.btnEnum.DcantChange => "<Keyboard>/d", 
			_ => throw new ArgumentOutOfRangeException("btnEnum", btnEnum, null), 
		};
	}

	public static void UpdateButtonShow(RectTransform rect, Image buttonimage, Text buttontext, string bindingRoot)
	{
		Sprite sprite2 = (buttonimage.sprite = ((UIMgr.Inst.InputType != PlayerInputType.Gamepad && !GameMgr.IsSteamDeck_Static) ? UIMgr.Inst.uiSetting.getkeyimage(bindingRoot) : UIMgr.Inst.uiSetting.getcontrolimage(bindingRoot)));
		if (sprite2 == UIMgr.Inst.uiSetting.controlSprite_Default)
		{
			buttonimage.type = Image.Type.Sliced;
			buttonimage.pixelsPerUnitMultiplier = 1f;
			if (!buttontext)
			{
				return;
			}
			buttontext.enabled = true;
			buttontext.text = GetKeyDisplayName(buttontext.text);
			if ((bool)rect)
			{
				float num = buttontext.preferredWidth + Inst.widthOffset;
				if (num < Inst.minWidth)
				{
					num = Inst.minWidth;
				}
				rect.sizeDelta = new Vector2(num, Inst.defaultKeyHeight);
			}
		}
		else
		{
			buttonimage.type = Image.Type.Simple;
			if ((bool)buttontext)
			{
				buttontext.enabled = false;
			}
			if ((bool)rect)
			{
				rect.sizeDelta = new Vector2((float)(buttonimage.sprite.texture.width / buttonimage.sprite.texture.height) * rect.sizeDelta.y, rect.sizeDelta.y);
			}
		}
	}

	public static void GetAndShowControlKey(RectTransform rect, Image buttonimage, Text buttontext, UISetting.btnEnum btnEnum, UISetting.controlEnum controlEnum, float height_offset, float widthoffset)
	{
		string keyBindingRootByEnum = GetKeyBindingRootByEnum(btnEnum, controlEnum);
		if (buttontext != null)
		{
			buttontext.text = GetShowNameByEnum(btnEnum, controlEnum);
		}
		UpdateButtonShow(rect, buttonimage, buttontext, keyBindingRootByEnum);
	}
}
