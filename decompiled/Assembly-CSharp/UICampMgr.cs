using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UICampMgr : MonoBehaviour
{
	[Serializable]
	public class SetSize
	{
		public GameObject obj;

		public float Size;
	}

	private struct UICloseRule
	{
		public readonly Func<bool> CheckIsOpen;

		public readonly Action DoClose;

		public readonly Func<bool> CustomRule;

		public UICloseRule(Func<bool> isOpen, Action close, Func<bool> custom = null)
		{
			CheckIsOpen = isOpen;
			DoClose = close;
			CustomRule = custom;
		}
	}

	public Canvas canvas;

	public RectTransform rtsfCanvas;

	private InputActions inputActions;

	[HideInInspector]
	public GameObject TourHint;

	[Header("SteamDeck")]
	public List<SetSize> setSizesSteamDeck = new List<SetSize>();

	[Header("手游处理")]
	public List<SetSize> setSizes = new List<SetSize>();

	public List<GameObject> gameobjectMobile = new List<GameObject>();

	public List<GameObject> gameobjectPc = new List<GameObject>();

	public CanvasScaler canvas5Scaler_Mobile;

	private Stopwatch stopwatch;

	private List<UICloseRule> _uiCloseRules;

	public UIMenu uiMenu => UIMgr.Inst.UIMenu;

	public UIGallery uiGallery { get; set; }

	public GameObject uiFinishHardStory { get; set; }

	public static UICampMgr Inst { get; private set; }

	private void InitUICloseRules()
	{
		_uiCloseRules = new List<UICloseRule>
		{
			new UICloseRule(() => GameUISingletonMono<UISuit>.StaticIsOpen, delegate
			{
				GameUISingletonMono<UISuit>.Inst.Hide();
			}),
			new UICloseRule(() => GameUISingletonMono<UIQuickPanel>.StaticIsOpen, delegate
			{
				GameUISingletonMono<UIQuickPanel>.Inst.Hide();
			}),
			new UICloseRule(() => GameUISingletonMono<UIFullGame>.StaticIsOpen, delegate
			{
				GameUISingletonMono<UIFullGame>.Inst.Hide();
			}),
			new UICloseRule(() => GameUISingletonMono<UITalent>.StaticIsOpen, delegate
			{
				GameUISingletonMono<UITalent>.Inst.Hide();
			}),
			new UICloseRule(() => GameUISingletonMono<UIResearch>.StaticIsOpen, delegate
			{
				GameUISingletonMono<UIResearch>.Inst.Hide();
			}),
			new UICloseRule(() => GameUISingletonMono<UISet>.StaticIsOpen, delegate
			{
				GameUISingletonMono<UISet>.Inst.Hide();
			}),
			new UICloseRule(() => uiGallery != null && uiGallery.IsOpen, delegate
			{
				uiGallery.Hide();
			}),
			new UICloseRule(() => GameUISingletonMono<UITraining>.StaticIsOpen, delegate
			{
				GameUISingletonMono<UITraining>.Inst.Hide();
			}),
			new UICloseRule(() => GameUISingletonMono<UICampMirror>.StaticIsOpen, delegate
			{
				GameUISingletonMono<UICampMirror>.Inst.Hide();
			}),
			new UICloseRule(() => GameUISingletonMono<UICampSkinChanger>.StaticIsOpen, delegate
			{
				GameUISingletonMono<UICampSkinChanger>.Inst.Hide();
			}),
			new UICloseRule(() => GameUISingletonMono<UIHandbookHint>.StaticIsOpen, delegate
			{
				GameUISingletonMono<UIHandbookHint>.Inst.Hide();
			}),
			new UICloseRule(() => GameUISingletonMono<UIUnlockSystem>.StaticIsOpen, delegate
			{
				GameUISingletonMono<UIUnlockSystem>.Inst.Hide();
			}),
			new UICloseRule(() => GameUISingletonMono<UIEndlessTalent>.StaticIsOpen, delegate
			{
				GameUISingletonMono<UIEndlessTalent>.Inst.Hide();
			}),
			new UICloseRule(() => GameUISingletonMono<UIEndlessGallery>.StaticIsOpen, delegate
			{
				GameUISingletonMono<UIEndlessGallery>.Inst.Hide();
			}),
			new UICloseRule(() => GameUISingletonMono<UI_RankingList>.StaticIsOpen, null, delegate
			{
				if (GameUISingletonMono<UI_RankingList>.Inst.finishbuildshow.IsOpen)
				{
					GameUISingletonMono<UI_RankingList>.Inst.HideInfo();
				}
				else
				{
					GameUISingletonMono<UI_RankingList>.Inst.Hide();
				}
				return true;
			}),
			new UICloseRule(() => GameUISingletonMono<UIEndlessRankingList>.StaticIsOpen, null, delegate
			{
				if (GameUISingletonMono<UIEndlessRankingList>.Inst.finishbuildshow.IsOpen || GameUISingletonMono<UIEndlessRankingList>.Inst.endlessFinishPanel.IsOpen)
				{
					GameUISingletonMono<UIEndlessRankingList>.Inst.HideInfo();
				}
				else
				{
					GameUISingletonMono<UIEndlessRankingList>.Inst.Hide();
				}
				return true;
			}),
			new UICloseRule(() => GameUISingletonMono<UIActivateGirl>.StaticIsOpen, null, delegate
			{
				if (GameUISingletonMono<UIActivateGirl>.Inst.rtsf_InfoRoot.gameObject.activeSelf)
				{
					GameUISingletonMono<UIActivateGirl>.Inst._HideInfo();
				}
				else
				{
					GameUISingletonMono<UIActivateGirl>.Inst.Hide();
				}
				return true;
			}),
			new UICloseRule(() => GameUISingletonMono<UIResourceChanger>.StaticIsOpen, delegate
			{
				GameUISingletonMono<UIResourceChanger>.Inst.Hide();
			}),
			new UICloseRule(() => GameUISingletonMono<UIChoseGiftSet>.StaticIsOpen, delegate
			{
				GameUISingletonMono<UIChoseGiftSet>.Inst.Hide();
			})
		};
	}

	private bool TryCloseOpenedUI()
	{
		if (UIMgr.Inst.TryCloseGlobalTopUI())
		{
			return true;
		}
		if (uiFinishHardStory != null)
		{
			return true;
		}
		if (_uiCloseRules == null)
		{
			InitUICloseRules();
		}
		foreach (UICloseRule uiCloseRule in _uiCloseRules)
		{
			if (uiCloseRule.CheckIsOpen())
			{
				if (uiCloseRule.CustomRule == null)
				{
					uiCloseRule.DoClose?.Invoke();
					return true;
				}
				if (uiCloseRule.CustomRule())
				{
					return true;
				}
			}
		}
		return false;
	}

	private void Awake()
	{
		stopwatch = new Stopwatch();
		stopwatch.Start();
		PlatformSet();
		Inst = this;
		LoadAndRefCampObj();
		stopwatch.Stop();
		stopwatch = new Stopwatch();
		stopwatch.Start();
		InitUICloseRules();
	}

	public void LoadAndRefCampObj()
	{
		GameMgr.LoadSceneObjSOCamp.LoadObjsWithoutUI();
		if (UIMgr.Inst.UIMenu != null)
		{
			UnityEngine.Object.DestroyImmediate(UIMgr.Inst.UIMenu.gameObject);
		}
		UIMgr.Inst.UIMenu = canvas.GetComponentInChildren<UIMenu>();
	}

	public void PlatformSet()
	{
		if (GameMgr.IsMobile_Static)
		{
			if (canvas5Scaler_Mobile != null)
			{
				canvas5Scaler_Mobile.referenceResolution = new Vector2(MobileMgr.inst.scalerwidth, MobileMgr.inst.scalerhight);
			}
			foreach (GameObject item in gameobjectMobile)
			{
				item.SetActive(value: true);
			}
			foreach (GameObject item2 in gameobjectPc)
			{
				item2.SetActive(value: false);
			}
			{
				foreach (SetSize setSize in setSizes)
				{
					setSize.obj.transform.localScale = new Vector3(setSize.Size, setSize.Size, setSize.obj.transform.localScale.z);
				}
				return;
			}
		}
		if (GameMgr.IsSteamDeck_Static)
		{
			if (canvas5Scaler_Mobile != null)
			{
				canvas5Scaler_Mobile.referenceResolution = new Vector2(MobileMgr.inst.scalerwidth, MobileMgr.inst.scalerhight);
			}
			foreach (GameObject item3 in gameobjectMobile)
			{
				item3.SetActive(value: false);
			}
			foreach (GameObject item4 in gameobjectPc)
			{
				item4.SetActive(value: true);
			}
			{
				foreach (SetSize item5 in setSizesSteamDeck)
				{
					item5.obj.transform.localScale = new Vector3(item5.Size, item5.Size, item5.obj.transform.localScale.z);
				}
				return;
			}
		}
		foreach (GameObject item6 in gameobjectMobile)
		{
			item6.SetActive(value: false);
		}
		foreach (GameObject item7 in gameobjectPc)
		{
			item7.SetActive(value: true);
		}
	}

	private void OnEnable()
	{
		inputActions = ControlMgr.Inst.inputActions;
		inputActions.Player.Pause.performed += PausePerformed;
		inputActions.Player.GamepadEast.performed += GamepadEastPerformed;
	}

	private void OnDisable()
	{
		inputActions.Player.Pause.performed -= PausePerformed;
		inputActions.Player.GamepadEast.performed -= GamepadEastPerformed;
	}

	public void PausePerformed(InputAction.CallbackContext context)
	{
		if (ControlMgr.Inst.InputActionRecovering || (!GameMgr.IsMobile_Static && !GameMgr.IsSteamDeck_Static && !ControlMgr.Inst.CursorVisible && !uiMenu.IsOpen && UIMgr.Inst.InputType == PlayerInputType.Keyboard) || GameUISingletonMono<UIDialogueMgr>.StaticIsOpen || Inst.TourHint != null || UIMgr.Inst.uiFade.IsOpen || TryCloseOpenedUI())
		{
			return;
		}
		if (GameUISingletonMono<UIChapterThrough>.StaticIsOpen)
		{
			if (GameUISingletonMono<UIChapterThrough>.Inst.CanExit)
			{
				GameUISingletonMono<UIChapterThrough>.Inst.Hide();
			}
		}
		else
		{
			uiMenu.ShowUIMenu();
		}
	}

	private void GamepadEastPerformed(InputAction.CallbackContext context)
	{
		if (!ControlMgr.Inst.InputActionRecovering && !ControlMgr.Inst.rebinding && !UIMgr.Inst.uiFade.IsOpen)
		{
			TryCloseOpenedUI();
		}
	}

	private void Start()
	{
		stopwatch.Stop();
		canvas.worldCamera = CamController.Inst.cam_UI;
		TimeScaleMgr.Inst.ForceRecover();
		if (UIMgr.Inst.showbattleui)
		{
			UIPlayerDataMgr.Inst.ShowDirect();
		}
		else
		{
			UIPlayerDataMgr.Inst.HideDirect();
		}
	}
}
