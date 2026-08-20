using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PlayerLogger;
using PlayerLogger.Events;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIBattleMgr : MonoBehaviour
{
	public class SetSize
	{
		public GameObject obj;

		public float Size;
	}

	public Canvas canvas;

	public RectTransform rtsfCanvas;

	public RectTransform rtsf_CanvasThings;

	public RectTransform rtsf_uiProcess;

	[Header("Language")]
	public Text text_GuideMove;

	public Text text_GuideOpenCloseBag;

	public Text text_GuideShoot;

	public Text text_GuideUsePotion;

	public Text text_GuideSwitchWand;

	public Text text_GuideMove_Gemepad;

	public Text text_GuideOpenCloseBag_Gemepad;

	public Text text_GuideShoot_Gemepad;

	public Text text_GuideUsePotion_Gemepad;

	public Text text_GuideSwitchWand_Gemepad;

	private InputActions inputActions;

	[Header("手游处理")]
	public List<SetSize> setSizes = new List<SetSize>();

	public static readonly float3 UIProcessPositionMobileLong = new float3(-6.5f, 0f, 0f);

	public static readonly float3 UIProcessPositionMobileWide = new float3(-4.5f, 0f, 0f);

	public static readonly float3 UIRerollRelicPositionMobileLong = new float3(-6.5f, -1f, 0f);

	public static readonly float3 UIRerollRelicPositionMobileWide = new float3(-5.5f, -1f, 0f);

	[Header("SteamDeck缩放")]
	public CanvasScaler canvas5_Mobile;

	private List<GameObject> gameobjectMobile = new List<GameObject>();

	private List<GameObject> gameobjectPc = new List<GameObject>();

	private List<SetSize> setSizesSteamDeck = new List<SetSize>();

	public UIMenu uiMenu => UIMgr.Inst.UIMenu;

	public UIFinishBuildShow uiFinishBuildShow { get; set; }

	public UIEndlessFinishPanel uiEndlessFinishPanel { get; set; }

	public static UIBattleMgr Inst { get; private set; }

	public float3 UIProcessPositionDefault
	{
		get
		{
			if (MobileMgr.inst.screenType != 0)
			{
				return UIProcessPositionMobileLong;
			}
			return UIProcessPositionMobileWide;
		}
	}

	public float3 UIRerollRelicPositionMobileDefault
	{
		get
		{
			if (MobileMgr.inst.screenType != 0)
			{
				return UIRerollRelicPositionMobileLong;
			}
			return UIRerollRelicPositionMobileWide;
		}
	}

	private void Awake()
	{
		PlatformSet();
		Inst = this;
		LoadAndRefObj();
	}

	private void LoadAndRefObj()
	{
		GameMgr.LoadSceneObjSOBattle.LoadObjsWithoutUI();
		uiFinishBuildShow = canvas.GetComponentInChildren<UIFinishBuildShow>();
		uiFinishBuildShow.gameObject.SetActive(value: false);
		uiEndlessFinishPanel = canvas.GetComponentInChildren<UIEndlessFinishPanel>();
		uiEndlessFinishPanel.gameObject.SetActive(value: false);
		if (UIMgr.Inst.UIMenu != null)
		{
			UnityEngine.Object.DestroyImmediate(UIMgr.Inst.UIMenu.gameObject);
		}
		UIMgr.Inst.UIMenu = canvas.GetComponentInChildren<UIMenu>();
		UIMgr.Inst.UIMenu.menuType = UIMenuType.Battle;
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
		if (!ControlMgr.Inst.InputActionRecovering && (GameMgr.IsMobile_Static || GameMgr.IsSteamDeck_Static || ControlMgr.Inst.CursorVisible || uiMenu.IsOpen || UIMgr.Inst.InputType != 0) && (!GameUISingletonMono<UIDialogueMgr>.Inited || (!GameUISingletonMono<UIDialogueMgr>.StaticIsOpen && !GameUISingletonMono<UIDialogueMgr>.Inst.IsOpen && !GameUISingletonMono<UIDialogueMgr>.Inst.IsOptionsOpen)) && !TryCloseOpenedUI() && !GameUISingletonMono<UIRerollRelic>.StaticIsOpen && !GameUISingletonMono<UIPlayerDead>.StaticIsOpen)
		{
			uiMenu.ShowUIMenu();
		}
	}

	private void GamepadEastPerformed(InputAction.CallbackContext context)
	{
		if (!ControlMgr.Inst.InputActionRecovering && !ControlMgr.Inst.rebinding)
		{
			TryCloseOpenedUI();
		}
	}

	private bool TryCloseOpenedUI()
	{
		if (UIMgr.Inst.TryCloseGlobalTopUI())
		{
			return true;
		}
		if (GameUISingletonMono<UIWhiteScreen>.StaticIsOpen && GameUISingletonMono<UIWhiteScreen>.Inst.canClose)
		{
			GameUISingletonMono<UIWhiteScreen>.Inst.Hide();
			return true;
		}
		UITraining inst = GameUISingletonMono<UITraining>.Inst;
		if ((object)inst != null && inst.IsOpen)
		{
			GameUISingletonMono<UITraining>.Inst.Hide();
			return true;
		}
		if (GameUISingletonMono<UIQuickPanel>.StaticIsOpen)
		{
			GameUISingletonMono<UIQuickPanel>.Inst.Hide();
			return true;
		}
		UISpellDisableHistoryApply inst2 = UISpellDisableHistoryApply.Inst;
		if ((object)inst2 != null && inst2.IsOpen)
		{
			UISpellDisableHistoryApply.Inst.Hide();
			return true;
		}
		if (GameUISingletonMono<UICommonHint>.Inited && GameUISingletonMono<UICommonHint>.Inst.IsOpen)
		{
			GameUISingletonMono<UICommonHint>.Inst.Hide();
			return true;
		}
		UISpellDisableHistory inst3 = UISpellDisableHistory.Inst;
		if ((object)inst3 != null && inst3.IsOpen)
		{
			UISpellDisableHistory.Inst.Hide();
			return true;
		}
		if (GameUISingletonMono<UISpellDisable>.StaticIsOpen)
		{
			if (GameUISingletonMono<UISpellDisable>.Inst.panel_Confirm.activeSelf)
			{
				GameUISingletonMono<UISpellDisable>.Inst._ConfirmClose();
			}
			else
			{
				GameUISingletonMono<UISpellDisable>.Inst.Hide();
			}
			return true;
		}
		if (GameUISingletonMono<UIProcessInOne_Controller>.StaticIsOpen)
		{
			GameUISingletonMono<UIProcessInOne_Controller>.Inst.Hide();
			return true;
		}
		if (GameUISingletonMono<UIBossShow>.StaticIsOpen)
		{
			return true;
		}
		if (GameUISingletonMono<UIChapterThrough>.StaticIsOpen)
		{
			return true;
		}
		if (uiFinishBuildShow.IsOpen)
		{
			uiFinishBuildShow._Close();
			return true;
		}
		if (UIMgr.Inst.uiFade.IsOpen)
		{
			return true;
		}
		if (GameUISingletonMono<UILevelReward>.StaticIsOpen)
		{
			GameUISingletonMono<UILevelReward>.Inst.Hide();
			return true;
		}
		if (GameUISingletonMono<UICompound>.StaticIsOpen)
		{
			GameUISingletonMono<UICompound>.Inst.Hide();
			return true;
		}
		if (GameUISingletonMono<UIReroll>.StaticIsOpen)
		{
			GameUISingletonMono<UIReroll>.Inst.Hide();
			return true;
		}
		if (GameUISingletonMono<UIMoreInOne>.StaticIsOpen)
		{
			GameUISingletonMono<UIMoreInOne>.Inst.BackSpellGamepadEast();
			return true;
		}
		if (GameUISingletonMono<UISell>.StaticIsOpen)
		{
			GameUISingletonMono<UISell>.Inst._Close();
			return true;
		}
		if (GameUISingletonMono<UIHandbookHint>.StaticIsOpen)
		{
			GameUISingletonMono<UIHandbookHint>.Inst.Hide();
			return true;
		}
		if (Inst.uiEndlessFinishPanel.IsOpen)
		{
			Inst.uiEndlessFinishPanel.Close();
			return true;
		}
		return false;
	}

	private void LanguageChange()
	{
		if (!(text_GuideMove != null))
		{
			return;
		}
		if (GameMgr.IsMobile_Static)
		{
			text_GuideOpenCloseBag_Gemepad.text = 1001806.GetText();
			text_GuideUsePotion_Gemepad.text = 1001807.GetText();
			GeneralTool.TextFormat(text_GuideUsePotion_Gemepad);
			GeneralTool.TextFormat(text_GuideOpenCloseBag_Gemepad);
			if (MobileMgr.inst.gamepadPlugged)
			{
				GamePadTextUpdate();
			}
		}
		else
		{
			text_GuideMove.text = 1001804.GetText();
			text_GuideOpenCloseBag.text = 1001803.GetText();
			text_GuideShoot.text = 1001801.GetText();
			text_GuideUsePotion.text = 1001802.GetText();
			text_GuideSwitchWand.text = 1001805.GetText();
			GamePadTextUpdate();
		}
	}

	public void GamePadTextUpdate()
	{
		text_GuideMove_Gemepad.text = 1001804.GetText();
		text_GuideOpenCloseBag_Gemepad.text = 1001803.GetText();
		text_GuideShoot_Gemepad.text = 1001801.GetText();
		text_GuideUsePotion_Gemepad.text = 1001802.GetText();
		text_GuideSwitchWand_Gemepad.text = 1001805.GetText();
	}

	public void PlatformSet()
	{
		if (GameMgr.IsSteamDeck_Static)
		{
			foreach (SetSize item in setSizesSteamDeck)
			{
				item.obj.transform.localScale = new Vector3(item.Size, item.Size, item.obj.transform.localScale.z);
			}
			foreach (GameObject item2 in gameobjectMobile)
			{
				item2.SetActive(value: false);
			}
			{
				foreach (GameObject item3 in gameobjectPc)
				{
					item3.SetActive(value: true);
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
			foreach (GameObject item4 in gameobjectMobile)
			{
				item4.SetActive(value: true);
			}
			foreach (GameObject item5 in gameobjectPc)
			{
				item5.SetActive(value: false);
			}
			{
				foreach (SetSize setSize in setSizes)
				{
					setSize.obj.transform.localScale = new Vector3(setSize.Size, setSize.Size, setSize.obj.transform.localScale.z);
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

	private void Start()
	{
		canvas.worldCamera = CamController.Inst.cam_UI;
		LanguageChange();
		if (UIMgr.Inst.showbattleui)
		{
			if (DataMgr.selectedWorldData.isTriggerTutorialHpShow)
			{
				UIPlayerDataMgr.Inst.playerinfoNormal.transform.localScale = Vector3.zero;
				Debug.Log("设置透明度逻辑执行");
				Debug.Log(UIPlayerDataMgr.Inst.playerinfoNormal.GetComponent<CanvasGroup>().alpha);
			}
			UIPlayerDataMgr.Inst.ShowDirect();
		}
		else
		{
			UIPlayerDataMgr.Inst.HideDirect();
		}
	}

	public void PopoutEndlessFinishBuild(Action OnClose = null)
	{
		if (!(BattleMgr.Inst == null))
		{
			StartCoroutine(IePopoutCurrentEndlessFinishBuild(OnClose));
		}
	}

	private IEnumerator IePopoutCurrentEndlessFinishBuild(Action OnClose = null)
	{
		Debug.Log("显示无尽模式通关数据");
		FinishEndlessGameBuild _build = DataMgr.WorldDataToEndlessBuildData(DataMgr.selectedWorldData);
		if (DataMgr.finishEndlessGameBuilds == null || DataMgr.finishEndlessGameBuilds.finishGameBuilds == null)
		{
			Debug.Log("无尽通关数据不存在");
		}
		else
		{
			Debug.Log("无尽通关数据存在");
			DataMgr.finishEndlessGameBuilds.finishGameBuilds.Add(_build);
			if (GameMgr.IsMobile_Static && DataMgr.finishEndlessGameBuilds.finishGameBuilds.Count > 50)
			{
				List<FinishGameBuild> finishGameBuilds = DataMgr.finishGameBuilds.finishGameBuilds;
				FinishGameBuild finishGameBuild = finishGameBuilds.OrderBy((FinishGameBuild b) => b.time).FirstOrDefault();
				if (finishGameBuild != null)
				{
					finishGameBuilds.Remove(finishGameBuild);
				}
			}
		}
		GameFinishLogger gameFinishLogger = new GameFinishLogger();
		gameFinishLogger.equips = PlayerEquips.CreateAuto();
		gameFinishLogger.resources = ResourcesStatus.CreateAuto();
		gameFinishLogger.Report();
		yield return null;
		PlayerMgr.Inst.PlayerCtrller.StopMotion();
		TimeScaleMgr.Inst.Pause();
		DataMgr.SaveEndlessBuildDatas();
		DataMgr.SaveEndlessBuildBackUp();
		yield return null;
		uiEndlessFinishPanel.Show(delegate
		{
			if (!uiFinishBuildShow.gameObject.activeSelf)
			{
				uiFinishBuildShow.transform.SetAsLastSibling();
				uiFinishBuildShow.Show();
				uiFinishBuildShow.UpdateBuildInfoFinishBattle(_build.finishGameBuild, UIFinishBuildShow.RecordUIFrom.FinishDrop);
			}
		}, _build, delegate
		{
			PlayerMgr.Inst.PlayerCtrller.StartMotion();
			TimeScaleMgr.Inst.Recovery();
			OnClose?.Invoke();
		});
	}

	public void PopoutCurrentFinishBuild(Action OnClose = null)
	{
		if (!(BattleMgr.Inst == null))
		{
			StartCoroutine(IePopoutCurrentFinishBuild(OnClose));
		}
	}

	private IEnumerator IePopoutCurrentFinishBuild(Action OnClose = null)
	{
		Debug.Log("显示无尽通关数据");
		FinishGameBuild finishGameBuild = DataMgr.WorlddataToBuildData(DataMgr.selectedWorldData);
		if (DataMgr.finishEndlessGameBuilds == null || DataMgr.finishEndlessGameBuilds.finishGameBuilds == null)
		{
			Debug.Log("无尽通关数据不存在");
		}
		else
		{
			Debug.Log("无尽通关数据存在");
			DataMgr.finishGameBuilds.finishGameBuilds.Add(finishGameBuild);
			if (GameMgr.IsMobile_Static && DataMgr.finishGameBuilds.finishGameBuilds.Count > 50)
			{
				List<FinishGameBuild> finishGameBuilds = DataMgr.finishGameBuilds.finishGameBuilds;
				FinishGameBuild finishGameBuild2 = finishGameBuilds.OrderBy((FinishGameBuild b) => b.time).FirstOrDefault();
				if (finishGameBuild2 != null)
				{
					finishGameBuilds.Remove(finishGameBuild2);
				}
			}
		}
		GameFinishLogger gameFinishLogger = new GameFinishLogger();
		gameFinishLogger.equips = PlayerEquips.CreateAuto();
		gameFinishLogger.resources = ResourcesStatus.CreateAuto();
		gameFinishLogger.Report();
		uiFinishBuildShow.Show();
		uiFinishBuildShow.UpdateBuildInfoFinishBattle(finishGameBuild, UIFinishBuildShow.RecordUIFrom.FinishDrop);
		PlayerMgr.Inst.PlayerCtrller.StopMotion();
		TimeScaleMgr.Inst.Pause();
		DataMgr.SaveBuildDatas();
		DataMgr.SaveBuildBackUp();
		if (SteamManager.Initialized)
		{
			Debug.Log("上传UGC");
			SteamLeadBoardManager.Inst.UploadUGCAndScore((int)DataMgr.selectedWorldData.timeuse, finishGameBuild, DataMgr.selectedWorldData.selectedDifficulty);
		}
		else
		{
			Debug.LogWarning("Steam未连接，无法上传UGC");
		}
		while (uiFinishBuildShow.IsOpen)
		{
			yield return new WaitForEndOfFrame();
		}
		PlayerMgr.Inst.PlayerCtrller.StartMotion();
		TimeScaleMgr.Inst.Recovery();
		OnClose?.Invoke();
	}
}
