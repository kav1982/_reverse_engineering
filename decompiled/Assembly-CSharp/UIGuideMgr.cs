using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIGuideMgr : MonoBehaviour
{
	public Canvas canvas;

	public RectTransform rtsfCanvas;

	[Header("LanguageChange_Guide2")]
	public Text text_Shoot;

	public Text text_UesPotion;

	public Text text_OpenBag;

	public Text text_OpenBag2;

	public Text text_DragTip;

	private InputActions inputActions;

	[Header("手游处理")]
	public CanvasScaler canvas5_Mobile;

	public List<GameObject> gameobjectMobile = new List<GameObject>();

	public List<GameObject> gameobjectPc = new List<GameObject>();

	[Header("PlatformLoad")]
	public List<PlatLoad> platLoad = new List<PlatLoad>();

	public UIMenu uiMenu => UIMgr.Inst.UIMenu;

	public static UIGuideMgr Inst { get; private set; }

	private void Awake()
	{
		PlatformSet();
		Inst = this;
		foreach (PlatLoad item in platLoad)
		{
			UIMgr.LoadUI(item);
		}
		if (UIMgr.Inst.UIMenu != null)
		{
			UnityEngine.Object.DestroyImmediate(UIMgr.Inst.UIMenu.gameObject);
		}
		UIMgr.Inst.UIMenu = canvas.GetComponentInChildren<UIMenu>();
	}

	public void PlatformSet()
	{
		if (GameMgr.IsSteamDeck_Static)
		{
			if (canvas5_Mobile != null)
			{
				canvas5_Mobile.referenceResolution = new Vector2(MobileMgr.inst.scalerwidth, MobileMgr.inst.scalerhight);
			}
			foreach (GameObject item in gameobjectMobile)
			{
				item.SetActive(value: false);
			}
			{
				foreach (GameObject item2 in gameobjectPc)
				{
					item2.SetActive(value: true);
				}
				return;
			}
		}
		if (GameMgr.IsMobile_Static)
		{
			if (canvas5_Mobile != null)
			{
				canvas5_Mobile.referenceResolution = new Vector2(MobileMgr.inst.scalerwidth, MobileMgr.inst.scalerhight);
			}
			foreach (GameObject item3 in gameobjectMobile)
			{
				item3.SetActive(value: true);
			}
			{
				foreach (GameObject item4 in gameobjectPc)
				{
					item4.SetActive(value: false);
				}
				return;
			}
		}
		foreach (GameObject item5 in gameobjectMobile)
		{
			item5.SetActive(value: false);
		}
		foreach (GameObject item6 in gameobjectPc)
		{
			item6.SetActive(value: true);
		}
	}

	private void OnEnable()
	{
		inputActions = ControlMgr.Inst.inputActions;
		inputActions.Player.Pause.performed += PausePerformed;
		inputActions.Player.GamepadEast.performed += GamepadEastPerformed;
		EventMgr.LanguageChange = (Action)Delegate.Combine(EventMgr.LanguageChange, new Action(LanguageChange));
	}

	private void OnDisable()
	{
		inputActions.Player.Pause.performed -= PausePerformed;
		inputActions.Player.GamepadEast.performed -= GamepadEastPerformed;
		EventMgr.LanguageChange = (Action)Delegate.Remove(EventMgr.LanguageChange, new Action(LanguageChange));
	}

	public void PausePerformed(InputAction.CallbackContext context)
	{
		if (!ControlMgr.Inst.InputActionRecovering && (GameMgr.IsMobile_Static || GameMgr.IsSteamDeck_Static || ControlMgr.Inst.CursorVisible || uiMenu.IsOpen || UIMgr.Inst.InputType != 0) && !GameUISingletonMono<UIDialogueMgr>.StaticIsOpen && !UIMgr.Inst.uiFade.IsOpen)
		{
			if (UIMgr.Inst.uiSetting.IsOpen)
			{
				UIMgr.Inst.uiSetting.Hide();
			}
			else if (GameUISingletonMono<UIHandbookHint>.StaticIsOpen)
			{
				GameUISingletonMono<UIHandbookHint>.Inst.Hide();
			}
			else if (uiMenu.Panel_Confirm.activeSelf)
			{
				uiMenu._MenuQuitNo();
			}
			else if (uiMenu.IsOpen)
			{
				uiMenu.Hide();
			}
			else if (!GameMgr.IsMobile_Static || !GuideMgr.Inst)
			{
				uiMenu.ShowUIMenu();
			}
		}
	}

	private void GamepadEastPerformed(InputAction.CallbackContext context)
	{
		if (!ControlMgr.Inst.InputActionRecovering && !ControlMgr.Inst.rebinding && !UIMgr.Inst.uiFade.IsOpen)
		{
			if (UIMgr.Inst.uiSetting.IsOpen)
			{
				UIMgr.Inst.uiSetting.Hide();
			}
			else if (uiMenu.Panel_Confirm.activeSelf)
			{
				uiMenu._MenuQuitNo();
			}
			else if (uiMenu.IsOpen)
			{
				uiMenu.Hide();
			}
		}
	}

	private void LanguageChange()
	{
		if (text_Shoot != null)
		{
			if (GameMgr.IsMobile_Static && !MobileMgr.inst.gamepadPlugged)
			{
				text_OpenBag.text = 1001806.GetText();
				text_OpenBag.fontSize += 10;
				text_OpenBag2.text = 1001806.GetText();
				text_OpenBag2.fontSize += 10;
				text_UesPotion.text = 1001807.GetText();
				text_UesPotion.fontSize += 10;
				GeneralTool.TextFormat(text_UesPotion);
				GeneralTool.TextFormat(text_OpenBag);
				text_Shoot.text = 1001808.GetText();
				text_Shoot.fontSize += 10;
			}
			else
			{
				text_Shoot.text = 1001801.GetText();
				text_UesPotion.text = 1001802.GetText();
				text_OpenBag.text = 1001803.GetText();
				text_OpenBag2.text = 1001803.GetText();
				text_DragTip.text = 1000701.GetText();
			}
		}
	}

	private void Start()
	{
		canvas.worldCamera = CamController.Inst.cam_UI;
		LanguageChange();
	}
}
