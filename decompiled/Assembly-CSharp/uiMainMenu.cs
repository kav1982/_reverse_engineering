using System;
using UnityEngine;
using UnityEngine.UI;

public class uiMainMenu : MonoBehaviour
{
	public GameObject Age12Button;

	public GameObject BiliSDKLogOut;

	public Animator animator;

	public UIReleaseNoteSmall uiReleaseNoteSmall;

	public Text userID;

	[Header("LanguageChange")]
	public Text text_StartGame;

	public Text text_Setting;

	public Text text_Credits;

	public Text text_Quit;

	public Text text_AddWishlist;

	public Text text_BugReport;

	public Text text_QQGroup;

	public GameObject[] go_Logos;

	[Header("Gamepad")]
	public RectTransform rtsf_Pointer;

	public Button btn_StartGame;

	public Button btn_Setting;

	public Button btn_Credits;

	public Button btn_Quit;

	public Button btn_Dicord;

	public Button btn_QQQun;

	public Button btn_bugreport;

	public Button btn_BiliLogOut;

	public Button btn_buyFullGame;

	public UpdatButtonShow[] updatebuttonshows;

	[Header("Mobile")]
	public Text textMobileStartGame;

	public Button btnMobileStartGame;

	public UINeedUpdatePenel UINeedUpdate;

	public GameObject confirmQuitPanel;

	public void Awake()
	{
		if (BiliSDKLogOut != null && GameMgr.IsUseBiliOneSDK && !GameMgr.IsMobile_Static)
		{
			BiliSDKLogOut.SetActive(value: true);
		}
		if (GameMgr.IsChinaSteam)
		{
			btn_Dicord.gameObject.SetActive(value: false);
			btn_QQQun.gameObject.transform.position = btn_Dicord.gameObject.transform.position;
		}
		else if (ScriptableObjMgr.Inst.testCtrller.publishTesting)
		{
			Age12Button.SetActive(value: true);
			btn_bugreport.gameObject.SetActive(value: false);
			btn_Dicord.gameObject.SetActive(value: false);
			btn_QQQun.gameObject.SetActive(value: false);
			uiReleaseNoteSmall.gameObject.SetActive(value: false);
		}
	}

	private void OnEnable()
	{
		if (GameMgr.IsMobile_Static)
		{
			EventMgr.RoleItemChange = (Action)Delegate.Combine(EventMgr.RoleItemChange, new Action(OnRoleItemChange));
			OnRoleItemChange();
		}
	}

	private void OnDisable()
	{
		if (GameMgr.IsMobile_Static)
		{
			EventMgr.RoleItemChange = (Action)Delegate.Remove(EventMgr.RoleItemChange, new Action(OnRoleItemChange));
		}
	}

	private void Start()
	{
		MainMenuLog();
		userID.gameObject.SetActive(ScriptableObjMgr.Inst.testCtrller.UseServer && ScriptableObjMgr.Inst.testCtrller.UseBiliOneSDK);
	}

	private void Update()
	{
		if (ScriptableObjMgr.Inst.testCtrller.UseServer && ScriptableObjMgr.Inst.testCtrller.UseBiliOneSDK)
		{
			userID.text = "uid:" + ((PluginActivity.logUid == "") ? "???" : PluginActivity.logUid);
		}
	}

	public void PointInStart()
	{
		UIMainMenuMgr.Inst.SetPointerMouse(UIMainMenuMgr.PointerFocus.StartGame);
	}

	public void PointInSetting()
	{
		UIMainMenuMgr.Inst.SetPointerMouse(UIMainMenuMgr.PointerFocus.Setting);
	}

	public void PointInCredit()
	{
		UIMainMenuMgr.Inst.SetPointerMouse(UIMainMenuMgr.PointerFocus.Credits);
	}

	public void PointInQuit()
	{
		UIMainMenuMgr.Inst.SetPointerMouse(UIMainMenuMgr.PointerFocus.Quit);
	}

	public void PointOutMainPanel()
	{
		UIMainMenuMgr.Inst.PointOutMouse();
	}

	public void _StartGame()
	{
		SEMgr.Inst.uiClick.PlaySE();
		UIMainMenuMgr.Inst.uiArchive.Show();
	}

	public void _Setting()
	{
		SEMgr.Inst.uiClick.PlaySE();
		UIMgr.Inst.uiSetting.ShowFromMainMenue();
	}

	public void _Credits()
	{
		UIMainMenuMgr.Inst.uicredit.Show();
		SEMgr.Inst.uiClick.PlaySE();
	}

	public void _Quit()
	{
		SEMgr.Inst.uiClick.PlaySE();
		GameMgr.QuitGame();
	}

	public void _Discord()
	{
		Application.OpenURL("https://discord.gg/9TQTkH8pmj");
	}

	public void _QQQun()
	{
		Application.OpenURL("https://qm.qq.com/q/lwVecYjdja");
	}

	public void _AddWishlist()
	{
		Application.OpenURL("https://store.steampowered.com/app/2103140/_Magicraft/");
	}

	public void _BugReport()
	{
		if (DataMgr.settingData.language == LanguageType.ChineseS)
		{
			Application.OpenURL("https://docs.qq.com/sheet/DVmVic2N6VkRPZmpD?tab=BB08J2");
		}
		else
		{
			Application.OpenURL("https://discord.gg/9TQTkH8pmj");
		}
	}

	public void _Age12Click()
	{
		UIMainMenuMgr.Inst.OpenCloesAgedInfo();
	}

	public void _PCOneSDKLogout()
	{
		BiliOneSDKMgr.Logout();
	}

	public void OneSDKCloseAccount()
	{
		if (GameMgr.IsMobile_Static)
		{
			MobileMgr.inst.PluginActivity.CloseAccount();
		}
	}

	public static void SetStartButtonToStart()
	{
		UIMainMenuMgr.Inst.uiMainMenu.textMobileStartGame.text = "开始游戏";
		UIMainMenuMgr.Inst.uiMainMenu.btnMobileStartGame.onClick.RemoveAllListeners();
		UIMainMenuMgr.Inst.uiMainMenu.btnMobileStartGame.onClick.AddListener(UIMainMenuMgr.Inst.uiMainMenu._StartGame);
	}

	public static void SetStartButtonToLog()
	{
		UIMainMenuMgr.Inst.uiMainMenu.textMobileStartGame.text = "登录";
		UIMainMenuMgr.Inst.uiMainMenu.btnMobileStartGame.onClick.RemoveAllListeners();
		UIMainMenuMgr.Inst.uiMainMenu.btnMobileStartGame.onClick.AddListener(UIMainMenuMgr.Inst.uiMainMenu.MainMenuLog);
	}

	public void MainMenuLog()
	{
		if (GameMgr.IsMobile_Static)
		{
			if (!ScriptableObjMgr.Inst.testCtrller.UseBiliOneSDK || !ScriptableObjMgr.Inst.testCtrller.UseServer || PluginActivity.ServerLogged)
			{
				SetStartButtonToStart();
				return;
			}
			MobileMgr.inst.PluginActivity.SetClientSetting();
			CNHCHFKLMOH.StartLog();
		}
	}

	public void ConfirmQuit()
	{
		_Quit();
	}

	public void CancelQuit()
	{
		confirmQuitPanel.gameObject.SetActive(value: false);
	}

	public void OpenBuyFullGameUI()
	{
		GameUISingletonMono<UIFullGame>.Inst.Show();
	}

	public void OnRoleItemChange()
	{
		btn_buyFullGame.gameObject.SetActive(!ICJNOGPFMAM.MIFJADDOODN);
	}

	public void OpenNoticeUI()
	{
		CNHCHFKLMOH.ShowCurrentVersion();
	}
}
