using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIMainMenuMgr : MonoBehaviour
{
	private enum UIState
	{
		closed,
		opening,
		opened,
		closing
	}

	public enum PointerFocus
	{
		None,
		StartGame,
		Setting,
		Credits,
		Quit,
		BtnDicord,
		BtnQQQun,
		BugReport,
		BiliLogOut
	}

	public Canvas canvas4;

	public Canvas canvas;

	public CanvasScaler canvas5_Scaler;

	public RectTransform rtsfcanvas;

	public GameObject mainmenuParticleObj;

	[Header("AgedInfo")]
	public GameObject AgedInfo;

	public CanvasGroup AgedInfoCanvasGroup;

	private UIState uiAged12State;

	[Header("手游处理")]
	private bool justChangeToPadThisFrame;

	private PointerFocus pointerFocus;

	private InputActions inputActions;

	public uiMainMenu uiMainMenu { get; private set; }

	public UIArchive uiArchive { get; private set; }

	public UICredites uicredit { get; private set; }

	public Animator anima_Menu => uiMainMenu.animator;

	public RectTransform rtsf_Pointer => uiMainMenu.rtsf_Pointer;

	public Button btn_StartGame => uiMainMenu.btn_StartGame;

	public Button btn_Setting => uiMainMenu.btn_Setting;

	public Button btn_Credits => uiMainMenu.btn_Credits;

	public Button btn_Quit => uiMainMenu.btn_Quit;

	public Button btn_Dicord => uiMainMenu.btn_Dicord;

	public Button btn_QQQun => uiMainMenu.btn_QQQun;

	public Button btn_bugreport => uiMainMenu.btn_bugreport;

	public Button btn_BiliLogOut => uiMainMenu.btn_BiliLogOut;

	public static UIMainMenuMgr Inst { get; private set; }

	public void ShowParticle()
	{
		if (mainmenuParticleObj != null)
		{
			mainmenuParticleObj.SetActive(value: true);
		}
	}

	public void HideParticle()
	{
		if (mainmenuParticleObj != null)
		{
			mainmenuParticleObj.SetActive(value: false);
		}
	}

	private void Awake()
	{
		Inst = this;
		PlatformSet();
		if (UIMgr.Inst.UIMenu != null)
		{
			UnityEngine.Object.Destroy(UIMgr.Inst.UIMenu.gameObject);
		}
		LoadObj();
	}

	private void LoadObj()
	{
		GameMgr.LoadSceneObjSOMainMenu.LoadObjsWithoutUI();
		uiMainMenu = canvas.GetComponentInChildren<uiMainMenu>();
		uicredit = canvas.GetComponentInChildren<UICredites>();
		uiArchive = canvas.GetComponentInChildren<UIArchive>();
	}

	private void PlatformSet()
	{
		if ((GameMgr.IsSteamDeck_Static || GameMgr.IsMobile_Static) && canvas5_Scaler != null)
		{
			if (MobileMgr.inst.screenType == MobileMgr.ScreenType.Normal)
			{
				canvas5_Scaler.referenceResolution = new Vector2(MobileMgr.inst.scalerwidth, MobileMgr.inst.scalerhight + 40);
			}
			else
			{
				canvas5_Scaler.referenceResolution = new Vector2(MobileMgr.inst.scalerwidth, MobileMgr.inst.scalerhight);
			}
		}
	}

	private void OnEnable()
	{
		inputActions = ControlMgr.Inst.inputActions;
		inputActions.Player.Enable();
		inputActions.Player.Pause.performed += PausePerformed;
		inputActions.Player.GamepadEast.performed += GamepadEastPerformed;
		inputActions.Player.GamepadDirect.performed += GamepadDirectPerformed;
		inputActions.Player.LeftStick.performed += GamepadDirectPerformed_Stick;
		inputActions.Player.Interact.performed += InteractPerformed;
		inputActions.Player.GamepadWest.performed += TryOpenAge14;
		EventMgr.LanguageChange = (Action)Delegate.Combine(EventMgr.LanguageChange, new Action(LanguageChange));
		EventMgr.InputChange = (Action)Delegate.Combine(EventMgr.InputChange, new Action(InputChange));
		EventMgr.InputChange = (Action)Delegate.Combine(EventMgr.InputChange, new Action(ControlChange));
	}

	private void TryOpenAge14(InputAction.CallbackContext obj)
	{
		if ((bool)uiMainMenu.Age12Button && uiMainMenu.Age12Button.activeInHierarchy)
		{
			uiMainMenu._Age12Click();
		}
	}

	private void OnDisable()
	{
		inputActions.Player.Pause.performed -= PausePerformed;
		inputActions.Player.GamepadEast.performed -= GamepadEastPerformed;
		inputActions.Player.GamepadDirect.performed -= GamepadDirectPerformed;
		inputActions.Player.LeftStick.performed -= GamepadDirectPerformed_Stick;
		inputActions.Player.GamepadWest.performed -= TryOpenAge14;
		inputActions.Player.Interact.performed -= InteractPerformed;
		EventMgr.LanguageChange = (Action)Delegate.Remove(EventMgr.LanguageChange, new Action(LanguageChange));
		EventMgr.InputChange = (Action)Delegate.Remove(EventMgr.InputChange, new Action(InputChange));
		EventMgr.InputChange = (Action)Delegate.Remove(EventMgr.InputChange, new Action(ControlChange));
	}

	private void PausePerformed(InputAction.CallbackContext context)
	{
		switch (uiAged12State)
		{
		case UIState.opened:
			OpenCloesAgedInfo();
			return;
		case UIState.opening:
		case UIState.closing:
			return;
		}
		if (UIMgr.Inst.uiSetting.IsOpen)
		{
			UIMgr.Inst.uiSetting._Close();
		}
		else if (uiArchive.panel_AreYouSure.activeSelf)
		{
			uiArchive._DeleteNo();
		}
		else if (uiArchive.Panel_Skip.activeSelf)
		{
			SEMgr.Inst.uiClick.PlaySE();
			uiArchive.Panel_Skip.SetActive(value: false);
		}
		else if (uiArchive.IsOpen)
		{
			uiArchive._Close();
		}
		else if (GameUISingletonMono<UIReleaseNote>.StaticIsOpen)
		{
			GameUISingletonMono<UIReleaseNote>.Inst.Hide();
		}
		else if (uicredit.IsOpen)
		{
			uicredit.Hide();
		}
		else if (GameMgr.IsMobile_Static)
		{
			MobileMgr.inst.PluginActivity.SDKQuitGame();
		}
		else
		{
			UIMgr.Inst.uiSetting.ShowFromMainMenue();
		}
	}

	private void GamepadEastPerformed(InputAction.CallbackContext context)
	{
		if (ControlMgr.Inst.InputActionRecovering || UIMgr.Inst.TryCloseGlobalTopUI())
		{
			return;
		}
		switch (uiAged12State)
		{
		case UIState.opened:
			OpenCloesAgedInfo();
			return;
		case UIState.opening:
		case UIState.closing:
			return;
		}
		if (uiArchive.Panel_Skip.activeSelf)
		{
			uiArchive.PanelSkipCLose();
		}
		else if (uiArchive.panel_AreYouSure.activeSelf)
		{
			uiArchive._DeleteNo();
		}
		else if (uiArchive.IsAreYouSureOpen)
		{
			uiArchive._DeleteNo();
		}
		else if (uiArchive.IsOpen)
		{
			uiArchive._Close();
		}
		else if (GameUISingletonMono<UIReleaseNote>.StaticIsOpen)
		{
			GameUISingletonMono<UIReleaseNote>.Inst.Hide();
		}
		else if (uicredit.IsOpen)
		{
			uicredit.Hide();
		}
	}

	public void ControlMoveDirection(Vector2 _direct)
	{
		if (_direct == Vector2.down)
		{
			switch (pointerFocus)
			{
			case PointerFocus.StartGame:
				SetPointer(PointerFocus.Setting);
				break;
			case PointerFocus.Setting:
				SetPointer(PointerFocus.Credits);
				break;
			case PointerFocus.Credits:
				SetPointer(PointerFocus.Quit);
				break;
			case PointerFocus.Quit:
				if (GameMgr.IsChinaSteam)
				{
					SetPointer(PointerFocus.BtnQQQun);
				}
				if (uiMainMenu.btn_Dicord.isActiveAndEnabled)
				{
					SetPointer(PointerFocus.BtnDicord);
				}
				else if ((bool)btn_BiliLogOut && btn_BiliLogOut.isActiveAndEnabled)
				{
					SetPointer(PointerFocus.BiliLogOut);
				}
				break;
			case PointerFocus.BtnDicord:
				SetPointer(PointerFocus.StartGame);
				break;
			case PointerFocus.BtnQQQun:
				SetPointer(PointerFocus.StartGame);
				break;
			default:
				Debug.LogError(pointerFocus);
				break;
			case PointerFocus.BugReport:
				break;
			}
		}
		else if (_direct == Vector2.left)
		{
			switch (pointerFocus)
			{
			case PointerFocus.BtnQQQun:
				if (uiMainMenu.btn_Dicord.isActiveAndEnabled)
				{
					SetPointer(PointerFocus.BtnDicord);
				}
				break;
			case PointerFocus.BugReport:
				if (uiMainMenu.btn_QQQun.isActiveAndEnabled)
				{
					SetPointer(PointerFocus.BtnQQQun);
				}
				break;
			default:
				Debug.LogError(pointerFocus);
				break;
			case PointerFocus.StartGame:
			case PointerFocus.Setting:
			case PointerFocus.Credits:
			case PointerFocus.Quit:
			case PointerFocus.BtnDicord:
				break;
			}
		}
		else if (_direct == Vector2.right)
		{
			switch (pointerFocus)
			{
			case PointerFocus.BtnDicord:
				SetPointer(PointerFocus.BtnQQQun);
				break;
			case PointerFocus.BtnQQQun:
				SetPointer(PointerFocus.BugReport);
				break;
			default:
				Debug.LogError(pointerFocus);
				break;
			case PointerFocus.StartGame:
			case PointerFocus.Setting:
			case PointerFocus.Credits:
			case PointerFocus.Quit:
				break;
			}
		}
		else
		{
			if (!(_direct == Vector2.up))
			{
				return;
			}
			switch (pointerFocus)
			{
			case PointerFocus.StartGame:
				if (uiMainMenu.btn_Dicord.isActiveAndEnabled)
				{
					SetPointer(PointerFocus.BtnDicord);
				}
				break;
			case PointerFocus.Setting:
				SetPointer(PointerFocus.StartGame);
				break;
			case PointerFocus.Credits:
				SetPointer(PointerFocus.Setting);
				break;
			case PointerFocus.Quit:
				SetPointer(PointerFocus.Credits);
				break;
			case PointerFocus.BtnDicord:
				SetPointer(PointerFocus.Quit);
				break;
			case PointerFocus.BtnQQQun:
				SetPointer(PointerFocus.Quit);
				break;
			case PointerFocus.BiliLogOut:
				SetPointer(PointerFocus.Quit);
				break;
			default:
				Debug.LogError(pointerFocus);
				break;
			case PointerFocus.BugReport:
				break;
			}
		}
	}

	private void GamepadDirectPerformed(InputAction.CallbackContext context)
	{
		if (justChangeToPadThisFrame)
		{
			justChangeToPadThisFrame = false;
		}
		else if (UIMgr.Inst.InputType == PlayerInputType.Gamepad && !UIMgr.Inst.uiSetting.IsOpen && !uiArchive.IsOpen && !uicredit.IsOpen && !GameUISingletonMono<UIReleaseNote>.StaticIsOpen && (!AgedInfoCanvasGroup.gameObject.activeInHierarchy || AgedInfoCanvasGroup.alpha == 0f))
		{
			Vector2 direct = context.ReadValue<Vector2>();
			ControlMoveDirection(direct);
		}
	}

	private void GamepadDirectPerformed_Stick(InputAction.CallbackContext context)
	{
		if (justChangeToPadThisFrame)
		{
			justChangeToPadThisFrame = false;
		}
		else if (UIMgr.Inst.InputType == PlayerInputType.Gamepad && !UIMgr.Inst.uiSetting.IsOpen && !uicredit.IsOpen && !uiArchive.IsOpen && !GameUISingletonMono<UIReleaseNote>.StaticIsOpen)
		{
			Vector2 vector = context.ReadValue<Vector2>();
			vector = ControlMgr.Inst.RampVector2(vector);
			ControlMoveDirection(vector);
		}
	}

	private void InteractPerformed(InputAction.CallbackContext context)
	{
		if (UIMgr.Inst.InputType != PlayerInputType.Gamepad || uiArchive.IsOpen || uicredit.IsOpen || GameUISingletonMono<UIReleaseNote>.StaticIsOpen || (AgedInfoCanvasGroup.gameObject.activeInHierarchy && AgedInfoCanvasGroup.alpha != 0f))
		{
			return;
		}
		if (GameMgr.IsMobile_Static)
		{
			uiMainMenu._StartGame();
			return;
		}
		switch (pointerFocus)
		{
		case PointerFocus.StartGame:
			uiMainMenu._StartGame();
			break;
		case PointerFocus.Setting:
			uiMainMenu._Setting();
			break;
		case PointerFocus.Credits:
			uiMainMenu._Credits();
			break;
		case PointerFocus.Quit:
			uiMainMenu._Quit();
			break;
		case PointerFocus.BtnDicord:
			uiMainMenu._Discord();
			break;
		case PointerFocus.BtnQQQun:
			uiMainMenu._QQQun();
			break;
		case PointerFocus.BugReport:
			uiMainMenu._BugReport();
			break;
		case PointerFocus.BiliLogOut:
			uiMainMenu._PCOneSDKLogout();
			break;
		default:
			Debug.LogError(pointerFocus);
			break;
		}
	}

	private void LanguageChange()
	{
		uiMainMenu.text_StartGame.text = 1000001.GetText();
		uiMainMenu.text_Setting.text = 1000002.GetText();
		uiMainMenu.text_Credits.text = 1000003.GetText();
		if (GameMgr.IsMobile_Static)
		{
			uiMainMenu.text_Quit.text = 1000021.GetText();
		}
		else
		{
			uiMainMenu.text_Quit.text = 1000004.GetText();
		}
		uiMainMenu.text_AddWishlist.text = 1000005.GetText();
		uiMainMenu.text_BugReport.text = 1000006.GetText();
		uiMainMenu.text_QQGroup.text = 1000028.GetText();
		for (int i = 0; i < uiMainMenu.go_Logos.Length; i++)
		{
			uiMainMenu.go_Logos[i].SetActive(value: false);
		}
		for (int j = 0; j < uiMainMenu.go_Logos.Length; j++)
		{
			if (j == (int)DataMgr.settingData.language)
			{
				uiMainMenu.go_Logos[j].SetActive(value: true);
				break;
			}
		}
	}

	private void InputChange()
	{
		switch (UIMgr.Inst.InputType)
		{
		case PlayerInputType.Keyboard:
			SetPointer(PointerFocus.None);
			break;
		case PlayerInputType.Gamepad:
			justChangeToPadThisFrame = true;
			SetPointer(PointerFocus.StartGame);
			break;
		default:
			Debug.LogError(UIMgr.Inst.InputType);
			break;
		}
	}

	private void ControlChange()
	{
		if (uiMainMenu.updatebuttonshows != null)
		{
			UpdatButtonShow[] updatebuttonshows = uiMainMenu.updatebuttonshows;
			for (int i = 0; i < updatebuttonshows.Length; i++)
			{
				updatebuttonshows[i].UpdateButton();
			}
		}
	}

	private void Start()
	{
		if (UIMgr.Inst.InputType == PlayerInputType.Keyboard)
		{
			ControlMgr.Inst.CursorVisibleSet(set: true);
		}
		canvas4.worldCamera = CamController.Inst.cam_UI;
		canvas.worldCamera = CamController.Inst.cam_UI;
		TimeScaleMgr.Inst.ForceRecover();
		LanguageChange();
		InputChange();
		GameInit.Pin("Enter MainMenu");
	}

	public void SetPointerMouse(PointerFocus pointerFocus)
	{
		Inst.rtsf_Pointer.gameObject.SetActive(value: false);
		switch (pointerFocus)
		{
		case PointerFocus.StartGame:
			Inst.rtsf_Pointer.gameObject.SetActive(value: true);
			rtsf_Pointer.position = btn_StartGame.transform.position;
			break;
		case PointerFocus.Setting:
			Inst.rtsf_Pointer.gameObject.SetActive(value: true);
			rtsf_Pointer.position = btn_Setting.transform.position;
			break;
		case PointerFocus.Credits:
			Inst.rtsf_Pointer.gameObject.SetActive(value: true);
			rtsf_Pointer.position = btn_Credits.transform.position;
			break;
		case PointerFocus.Quit:
			Inst.rtsf_Pointer.gameObject.SetActive(value: true);
			rtsf_Pointer.position = btn_Quit.transform.position;
			break;
		}
	}

	public void PointOutMouse()
	{
		Inst.rtsf_Pointer.gameObject.SetActive(value: false);
	}

	private void SetPointer(PointerFocus pointerFocus)
	{
		if (GameMgr.IsMobile_Static)
		{
			return;
		}
		rtsf_Pointer.gameObject.SetActive(value: false);
		this.pointerFocus = pointerFocus;
		btn_StartGame.animator.SetTrigger("Normal");
		btn_Setting.animator.SetTrigger("Normal");
		btn_Credits.animator.SetTrigger("Normal");
		btn_Quit.animator.SetTrigger("Normal");
		btn_Dicord.animator.SetTrigger("Normal");
		btn_QQQun.animator.SetTrigger("Normal");
		btn_bugreport.animator.SetTrigger("Normal");
		if ((bool)btn_BiliLogOut)
		{
			btn_BiliLogOut.animator.SetTrigger("Normal");
		}
		switch (pointerFocus)
		{
		case PointerFocus.StartGame:
			btn_StartGame.animator.SetTrigger("Highlighted");
			rtsf_Pointer.gameObject.SetActive(value: true);
			rtsf_Pointer.position = btn_StartGame.transform.position;
			break;
		case PointerFocus.Setting:
			btn_Setting.animator.SetTrigger("Highlighted");
			rtsf_Pointer.gameObject.SetActive(value: true);
			rtsf_Pointer.position = btn_Setting.transform.position;
			break;
		case PointerFocus.Credits:
			btn_Credits.animator.SetTrigger("Highlighted");
			rtsf_Pointer.gameObject.SetActive(value: true);
			rtsf_Pointer.position = btn_Credits.transform.position;
			break;
		case PointerFocus.Quit:
			btn_Quit.animator.SetTrigger("Highlighted");
			rtsf_Pointer.gameObject.SetActive(value: true);
			rtsf_Pointer.position = btn_Quit.transform.position;
			break;
		case PointerFocus.BtnDicord:
			btn_Dicord.animator.SetTrigger("Highlighted");
			break;
		case PointerFocus.BtnQQQun:
			btn_QQQun.animator.SetTrigger("Highlighted");
			break;
		case PointerFocus.BugReport:
			btn_bugreport.animator.SetTrigger("Highlighted");
			break;
		case PointerFocus.BiliLogOut:
			if ((bool)btn_BiliLogOut)
			{
				btn_BiliLogOut.animator.SetTrigger("Highlighted");
			}
			break;
		default:
			Debug.LogError(pointerFocus);
			break;
		case PointerFocus.None:
			break;
		}
	}

	public void OpenCloesAgedInfo()
	{
		if (UIMgr.Inst.uiSetting.IsOpen || uiArchive.IsOpen || uicredit.IsOpen || GameUISingletonMono<UIReleaseNote>.StaticIsOpen)
		{
			return;
		}
		if (uiAged12State == UIState.opened)
		{
			uiAged12State = UIState.closing;
			DOTween.Sequence().Append(AgedInfoCanvasGroup.DOFade(0f, 0.5f)).AppendCallback(delegate
			{
				AgedInfo.SetActive(value: false);
				uiAged12State = UIState.closed;
			});
		}
		else if (uiAged12State == UIState.closed)
		{
			uiAged12State = UIState.opening;
			AgedInfo.SetActive(value: true);
			DOTween.Sequence().Append(AgedInfoCanvasGroup.DOFade(1f, 0.5f)).AppendCallback(delegate
			{
				uiAged12State = UIState.opened;
			});
		}
	}

	public void TryShowReleaseNote()
	{
		if (!UIMgr.Inst.uiSetting.IsOpen && !uiArchive.IsOpen && !uicredit.IsOpen && (!AgedInfoCanvasGroup.gameObject.activeInHierarchy || AgedInfoCanvasGroup.alpha == 0f) && (bool)uiMainMenu.uiReleaseNoteSmall && uiMainMenu.uiReleaseNoteSmall.gameObject.activeInHierarchy)
		{
			GameUISingletonMono<UIReleaseNote>.ShowInit();
		}
	}
}
