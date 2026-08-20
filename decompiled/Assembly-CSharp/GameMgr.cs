using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;
using Newtonsoft.Json;
using PlayerLogger.Events;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMgr : MonoBehaviour
{
	[Serializable]
	public class SetSize
	{
		public List<GameObject> obj = new List<GameObject>();

		public float Size;
	}

	public static string InitDataFolderName = "GameInitData";

	public GameObject[] dontDestroyGOs;

	public SEMgr soundMgr;

	public MusicMgr musicMgr;

	public PlayerMgr playerMgr;

	public ObjPoolMgr objPoolMgr;

	public CamController cameCtrller;

	public TimeScaleMgr itemScaleMgr;

	public MobileMgr mobileMgr;

	public UIMgr uiMgr;

	public UIInteractiveObjMgr uiInteractiveObjMgr;

	public int fpsLimit;

	public ControlMgr controlMgr;

	public PlayerSkinMgr playerSkinMgr;

	private static bool isSupportVFX = true;

	public static bool IsAndroidSimulator = false;

	private static bool isLightErrorDevice = false;

	[Header("计算Fps")]
	private const float fpsUpdateInterval = 0.1f;

	private float fpsUpdateIntervalTimer = 0.1f;

	private List<float> fpsList = new List<float>();

	private float m_FPS;

	private float m_FPS5;

	private static List<LoadSceneObjSO> _loadSceneObjSOs;

	private static LoadSceneObjSO _loadSceneObjSOEntry;

	private static LoadSceneObjSO _loadSceneObjSOCamp;

	private static LoadSceneObjSO _loadSceneObjSOMainMenu;

	private static LoadSceneObjSO _loadSceneObjSOBattle;

	private bool testCampDecorateNpc2;

	private int testCampDecorateNpc3;

	private int testCampDecorateNpc4;

	private bool testCampDecorateNpc5;

	private int testCampDecorateNpc6 = -1;

	public Coroutine IElogCheck;

	public static Dictionary<string, LanguageType> steamLanguageToGameLanguage = new Dictionary<string, LanguageType>
	{
		{
			"schinese",
			LanguageType.ChineseS
		},
		{
			"tchinese",
			LanguageType.ChineseT
		},
		{
			"english",
			LanguageType.English
		},
		{
			"japanese",
			LanguageType.Japanese
		},
		{
			"german",
			LanguageType.German
		},
		{
			"koreana",
			LanguageType.Korean
		},
		{
			"russian",
			LanguageType.Russian
		},
		{
			"spanish",
			LanguageType.Spanish_spain
		},
		{
			"french",
			LanguageType.French
		},
		{
			"portuguese_brazil",
			LanguageType.Portuguese_brazil
		},
		{
			"swedish",
			LanguageType.Swedish
		},
		{
			"polish",
			LanguageType.Polish
		},
		{
			"turkish",
			LanguageType.Turkish
		},
		{
			"italian",
			LanguageType.Italian
		},
		{
			"czech",
			LanguageType.Czech
		}
	};

	private EntityManager ettMgr;

	private static bool _haveDaveDlc;

	public static GameMgr Inst { get; private set; }

	public UIPlaceNameMgr uiPlaceNameMgr { get; set; }

	public UIPlayerDataMgr UIPlayerDataMgr { get; set; }

	public CommandLineMgr commandLineMgr { get; set; }

	public static bool InEndlessMode
	{
		get
		{
			if (BattleMgr.Inst == null)
			{
				return false;
			}
			if (DataMgr.selectedWorldData == null)
			{
				return false;
			}
			if (DataMgr.selectedWorldData.battleData9 == null)
			{
				return false;
			}
			return BattleMgr.Inst.CurrentStage == 300;
		}
	}

	public static bool IsChinaSteam => ScriptableObjMgr.staticTestCtrller.OverrideForceChinaSteam switch
	{
		TestItemForceAvailable.Disabled => false, 
		TestItemForceAvailable.Have => true, 
		TestItemForceAvailable.DontHave => false, 
		_ => false, 
	};

	public static bool IsSupportVFX
	{
		get
		{
			if (ScriptableObjMgr.Inst.testCtrller.forceNotSupportVFX)
			{
				return false;
			}
			return isSupportVFX;
		}
		set
		{
			isSupportVFX = value;
		}
	}

	public static bool IsSupportVideo { get; private set; } = true;


	public static bool IsLightErrorDevice
	{
		get
		{
			return isLightErrorDevice;
		}
		set
		{
			isLightErrorDevice = value;
		}
	}

	public static bool IsMobile_Static
	{
		get
		{
			if (!Application.isMobilePlatform)
			{
				return ScriptableObjMgr.staticTestCtrller.AndroidTesting;
			}
			return true;
		}
	}

	public static bool IsSteamDeck_Static
	{
		get
		{
			if (!SteamManager.CheckIfSteamDeck())
			{
				return ScriptableObjMgr.staticTestCtrller.SteamDeckTesting;
			}
			return true;
		}
	}

	public static bool IsUseBiliOneSDK => ScriptableObjMgr.staticTestCtrller.UseBiliOneSDK;

	public static bool IsMobileCamp
	{
		get
		{
			if (IsMobile_Static)
			{
				return MobileMgr.inst.screenRatio >= 1.7777778f;
			}
			return false;
		}
	}

	public static bool IsHarmony_Static
	{
		get
		{
			TestHarmonyScale harmonyScale = ScriptableObjMgr.staticTestCtrller.harmonyScale;
			return harmonyScale == TestHarmonyScale.HarmonyForPlayer14OrOlder || harmonyScale == TestHarmonyScale.HarmonyForPlayer16OrOlder;
		}
	}

	public static bool IsChAge14_Static => ScriptableObjMgr.staticTestCtrller.harmonyScale == TestHarmonyScale.HarmonyForPlayer14OrOlder;

	public static bool IsChAge16_Static => ScriptableObjMgr.staticTestCtrller.harmonyScale == TestHarmonyScale.HarmonyForPlayer16OrOlder;

	public static CampSkinType CampSkinType
	{
		get
		{
			if (DataMgr.selectedWorldData == null || ScriptableObjMgr.staticTestCtrller.campSkinType != 0)
			{
				return ScriptableObjMgr.staticTestCtrller.campSkinType;
			}
			return DataMgr.selectedWorldData.campSkinType;
		}
	}

	public static LoadSceneObjSO LoadSceneObjSOEntry => LoadSO(ref _loadSceneObjSOEntry, InitDataFolderName + "/LoadEntrySceneObjSO");

	public static LoadSceneObjSO LoadSceneObjSOCamp => LoadSO(ref _loadSceneObjSOCamp, InitDataFolderName + "/LoadCampSceneObjSO");

	public static LoadSceneObjSO LoadSceneObjSOMainMenu => LoadSO(ref _loadSceneObjSOMainMenu, InitDataFolderName + "/LoadMainMenuSceneObjSO");

	public static LoadSceneObjSO LoadSceneObjSOBattle => LoadSO(ref _loadSceneObjSOBattle, InitDataFolderName + "/LoadBattleSceneObjSO");

	public static List<LoadSceneObjSO> LoadSceneObjSOs
	{
		get
		{
			if (_loadSceneObjSOs == null)
			{
				_loadSceneObjSOs = new List<LoadSceneObjSO> { LoadSceneObjSOEntry, LoadSceneObjSOCamp, LoadSceneObjSOMainMenu, LoadSceneObjSOBattle };
			}
			return _loadSceneObjSOs;
		}
	}

	private static T LoadSO<T>(ref T cache, string path) where T : ScriptableObject
	{
		if ((UnityEngine.Object)cache == (UnityEngine.Object)null)
		{
			cache = Resources.Load<T>(path);
		}
		return cache;
	}

	private void Awake()
	{
		GameInit.Pin("Gamemgr Awake");
		Inst = this;
		ettMgr = World.DefaultGameObjectInjectionWorld.EntityManager;
		new GameLaunchLogger().Report();
		UnityEngine.Debug.Log("=== 系统信息 ===");
		UnityEngine.Debug.Log("显卡: " + SystemInfo.graphicsDeviceName);
		UnityEngine.Debug.Log("显卡厂商: " + SystemInfo.graphicsDeviceVendor);
		UnityEngine.Debug.Log("驱动版本: " + SystemInfo.graphicsDeviceVersion);
		UnityEngine.Debug.Log("显存: " + SystemInfo.graphicsMemorySize + " MB");
		UnityEngine.Debug.Log("操作系统: " + SystemInfo.operatingSystem);
		UnityEngine.Debug.Log("处理器: " + SystemInfo.processorType);
		UnityEngine.Debug.Log("核心数: " + SystemInfo.processorCount);
		UnityEngine.Debug.Log("内存: " + SystemInfo.systemMemorySize + " MB");
		UnityEngine.Debug.Log("设备型号: " + SystemInfo.deviceModel);
		UnityEngine.Debug.Log("设备名称: " + SystemInfo.deviceName);
		UnityEngine.Debug.Log("设备类型: " + SystemInfo.deviceType);
		UnityEngine.Debug.Log("maxComputeBufferInputsVertex: " + SystemInfo.maxComputeBufferInputsVertex);
		UnityEngine.Debug.Log("支持计算着色器: " + SystemInfo.supportsComputeShaders);
		UnityEngine.Debug.Log("支持几何着色器: " + SystemInfo.supportsGeometryShaders);
		UnityEngine.Debug.Log("支持曲面细分着色器: " + SystemInfo.supportsTessellationShaders);
		UnityEngine.Debug.Log("图形API级别: " + SystemInfo.graphicsDeviceVersion);
		UpdateVFXSuport();
		UpdateVideoSupport();
		CheckIsLightErrorDivice();
		Text versionText = UIVersion.Inst.versionText;
		string text = (((object)versionText == null) ? "[GET VERSION ERROR]" : versionText.text);
		UnityEngine.Debug.Log("游戏版本号:" + text);
		PrintGameProgramInfo();
		_ = IsMobile_Static;
		GameInit.Pin("开始一系列Init");
		TextConfig.RegetAllText();
		soundMgr.Initialize();
		musicMgr.Initialize();
		playerMgr.Initialize();
		objPoolMgr.Initialize();
		cameCtrller.Initialize();
		mobileMgr.Initialize();
		controlMgr.Initialize();
		if (ScriptableObjMgr.Inst.testCtrller.BLive || Environment.GetCommandLineArgs().Contains("--BLive"))
		{
			UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/BLiveMgr"), base.transform);
		}
		GameInit.Pin("开始加载预制体");
		LoadEntryObj();
		RefEntryObj();
		uiInteractiveObjMgr.Initialize();
		GameInit.Pin("加载完预制体");
		itemScaleMgr.Initialize();
		uiPlaceNameMgr.Initialize();
		commandLineMgr.Initialize();
		GameInit.Pin("开始UImgr Init");
		uiMgr.Initialize();
		GameInit.Pin("GameMgr awake end");
		SteamAchievementMgr.UnlockAndUpload(SteamAchievementType.FirstLaunchGame);
		CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
		CultureInfo.CurrentUICulture = CultureInfo.InvariantCulture;
		CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
		CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;
	}

	private void LoadEntryObj()
	{
		LoadSceneObjSOEntry.LoadObjsWithoutUI();
		mobileMgr.topui = uiMgr.canvas11.GetComponentInChildren<TopUI>();
		if (IsMobile_Static)
		{
			MobileMgr.inst.topui.topUIMobile.SetActive(value: true);
		}
	}

	private void RefEntryObj()
	{
		UIPlayerDataMgr = uiMgr.canvas_1.GetComponentInChildren<UIPlayerDataMgr>();
		commandLineMgr = uiMgr.rtsf_Canvas10.GetComponentInChildren<CommandLineMgr>(includeInactive: true);
		uiPlaceNameMgr = uiMgr.rtsf_Canvas10.GetComponentInChildren<UIPlaceNameMgr>();
	}

	private IEnumerator Start()
	{
		GameInit.Pin("Awake->Start:");
		playerSkinMgr.Initialize();
		for (int i = 0; i < dontDestroyGOs.Length; i++)
		{
			UnityEngine.Object.DontDestroyOnLoad(dontDestroyGOs[i]);
		}
		if (!IsMobile_Static)
		{
			if (DataMgr.settingData.Vsync)
			{
				QualitySettings.vSyncCount = 1;
			}
			else
			{
				QualitySettings.vSyncCount = 0;
				Application.targetFrameRate = fpsLimit;
			}
		}
		SetWindowsMode();
		if (IsMobile_Static && (ScriptableObjMgr.Inst.testCtrller.UseServer || ScriptableObjMgr.Inst.testCtrller.UseBiliOneSDK))
		{
			ScriptableObjMgr.Inst.testCtrller.Shortcut = false;
			ScriptableObjMgr.Inst.testCtrller.CommandLine = false;
		}
		if (ScriptableObjMgr.Inst.testCtrller.DirectEnterScene)
		{
			DataMgr.SetSelectedWorldData(0);
			DataMgr.selectedWorldData.haveUsed = true;
			DataMgr.selectedWorldData.PlayTimeRecord();
			switch (ScriptableObjMgr.Inst.testCtrller.enterScene)
			{
			case TestDirectEnterScene.Camp:
			case TestDirectEnterScene.CampTraining:
			case TestDirectEnterScene.EndlessCamp:
				SceneManager.LoadScene("Camp");
				break;
			case TestDirectEnterScene.Guide:
				SceneManager.LoadScene("Guide");
				break;
			case TestDirectEnterScene.Guide2:
				SceneManager.LoadScene("Guide2");
				break;
			case TestDirectEnterScene.Battle:
				SceneManager.LoadScene("Battle");
				break;
			case TestDirectEnterScene.NormalFinishBackHome:
				SceneManager.LoadScene("EasyFinishBackHome");
				break;
			case TestDirectEnterScene.NPC7Appearance:
				SceneManager.LoadScene("NPC7Appearance");
				break;
			default:
				UnityEngine.Debug.LogError(ScriptableObjMgr.Inst.testCtrller.enterScene);
				break;
			}
		}
		else if (IsMobile_Static)
		{
			uiMgr.uiFade.Hide(0f);
			GameUISingletonMono<UIEntryLanguage>.ShowInit();
		}
		else if (DataMgr.settingData.firstCreate || ScriptableObjMgr.Inst.testCtrller.publishTesting)
		{
			uiMgr.uiFade.Hide(0f);
			GameUISingletonMono<UIEntryLanguage>.ShowInit();
		}
		else
		{
			SceneManager.LoadScene("MainMenu");
		}
		yield return new WaitForSeconds(0.5f);
		if (!ScriptableObjMgr.Inst.testCtrller.UseBiliOneSDK)
		{
			Application.runInBackground = false;
		}
	}

	public void SetWindowsMode()
	{
		Vector2Int resolution = DataMgr.settingData.GetResolution();
		switch (DataMgr.settingData.windowsMode)
		{
		case SettingData.WindowsMode.FullScreen:
			Screen.SetResolution(resolution.x, resolution.y, FullScreenMode.ExclusiveFullScreen);
			break;
		case SettingData.WindowsMode.Windows:
			Screen.SetResolution(resolution.x, resolution.y, fullscreen: false);
			break;
		case SettingData.WindowsMode.BoardlessWindows:
			Screen.SetResolution(resolution.x, resolution.y, FullScreenMode.FullScreenWindow);
			break;
		}
	}

	public static void OpenQuickPanel()
	{
		if (!TimeScaleMgr.Inst.GamePaused)
		{
			if (GameUISingletonMono<UIQuickPanel>.Inited)
			{
				GameUISingletonMono<UIQuickPanel>.Inst.Switch();
			}
			else
			{
				GameUISingletonMono<UIQuickPanel>.ShowInit();
			}
			if ((bool)UIQuickPanelButton.Inst)
			{
				UIQuickPanelButton.Inst.PlayButtonPressedEffect();
			}
		}
	}

	private void Update()
	{
		if (DataMgr.selectedWorldData.GetResearchValueConsiderActive(ResearchAbilityType.DamageRecordBoard) > 0 && (bool)PlayerMgr.Inst.PlayerCtrller && ControlMgr.Inst.QuickPanelButtonPressedTriggerOnce)
		{
			OpenQuickPanel();
		}
		if (DataMgr.selectedWorldData.GetResearchValueConsiderActive(ResearchAbilityType.CancelSummon) != 0 && ControlMgr.Inst.KillSummonPressedTriggerOnce && !TimeScaleMgr.Inst.GamePaused)
		{
			UIKillSummonButton.StaticKillSummon();
		}
		if (ScriptableObjMgr.Inst.testCtrller.Shortcut && !commandLineMgr.debugCmdInputField.isFocused)
		{
			if (Input.GetKeyDown(KeyCode.R))
			{
				TestButtonReloadLevel();
			}
			if (Input.GetKeyDown(KeyCode.T))
			{
				LevelMgr.Inst.CurrentRoomCtrller.KillAllMonster();
			}
			if (Input.GetKeyDown(KeyCode.Y))
			{
				LevelMgr.Inst.CurrentRoomCtrller.KillAllMonster2();
			}
			if (Input.GetKeyDown(KeyCode.J))
			{
				ScreenCapture.CaptureScreenshot("Assets/ScreenCapture.png");
			}
			if (Input.GetKeyDown(KeyCode.U))
			{
				SwitchBattleUI();
			}
			if (Input.GetKeyDown(KeyCode.L))
			{
				uiMgr.uiSetting._LanguageChangeLeftRight(1);
			}
			if (Input.GetKeyDown(KeyCode.F1))
			{
				ChangeTimeScale(-1f);
			}
			if (Input.GetKeyDown(KeyCode.F2))
			{
				ChangeTimeScale(-0.1f);
			}
			if (Input.GetKeyDown(KeyCode.F3))
			{
				ChangeTimeScale(0.1f);
			}
			if (Input.GetKeyDown(KeyCode.F4))
			{
				ChangeTimeScale(1f);
			}
			if (Input.GetKeyDown(KeyCode.F5))
			{
				Time.timeScale = 1f;
			}
			if (Input.GetKeyDown(KeyCode.F8))
			{
				GameUISingletonMono<UITraining>.ShowInit();
			}
			if (Input.GetKeyDown(KeyCode.X))
			{
				for (int i = 0; i < PlayerMgr.Inst.Wands.Count; i++)
				{
					PlayerMgr.Inst.Wands[i].UpdateHandDisplay();
				}
			}
			if (Input.GetKeyDown(KeyCode.M))
			{
				TimeScaleMgr.Inst.ClearAllTimeScaleModifyRequest();
				SceneManager.LoadScene("MainMenu");
				if (GameUISingletonMono<UIDialogueMgr>.Inited)
				{
					GameUISingletonMono<UIDialogueMgr>.DestroyUI();
				}
				uiMgr.uiFilmBlackEdge.Hide(0f);
			}
			if (Input.GetKeyDown(KeyCode.F6))
			{
				string value = JsonConvert.SerializeObject(PlayerMgr.Inst.BaData.wandCfgs);
				PlayerPrefs.SetString("TEST:WAND", value);
				string value2 = JsonConvert.SerializeObject(playerMgr.BaData.bagSpellDatas.Where((SlotData e) => e != null && e.id > 0).ToArray());
				PlayerPrefs.SetString("TEST:BAG", value2);
				PlayerPrefs.SetString("TEST:RELIC", JsonConvert.SerializeObject(PlayerMgr.Inst.BaData.relicCfgs));
				UnityEngine.Debug.Log("保存测试法杖、背包法术、遗物");
				string text = SaveDataCompressor.CompressSaveData(new StringSaveData
				{
					w = PlayerMgr.Inst.BaData.wandCfgs,
					b = playerMgr.BaData.bagSpellDatas.Where((SlotData e) => e != null && e.id > 0).ToArray(),
					r = PlayerMgr.Inst.BaData.relicCfgs
				});
				UnityEngine.Debug.Log("配置代码已生成: \n" + text + "\n");
			}
			if (Input.GetKeyDown(KeyCode.F9))
			{
				if (PlayerPrefs.HasKey("TEST:WAND"))
				{
					List<WandConfig> list = JsonConvert.DeserializeObject<List<WandConfig>>(PlayerPrefs.GetString("TEST:WAND"));
					int index = ((playerMgr.SelectedWandIndex >= 0) ? playerMgr.SelectedWandIndex : 0);
					for (int j = 0; j < list.Count; j++)
					{
						playerMgr.SetWand(j, list[j]);
					}
					playerMgr.WandSelect(index);
					playerMgr.AllWandFullMP();
				}
				if (PlayerPrefs.HasKey("TEST:BAG"))
				{
					SlotData[] array = JsonConvert.DeserializeObject<SlotData[]>(PlayerPrefs.GetString("TEST:BAG"));
					foreach (SlotData spellData in array)
					{
						playerMgr.SpellPick(spellData);
					}
				}
				if (PlayerPrefs.HasKey("TEST:RELIC"))
				{
					List<RelicConfig> list2 = JsonConvert.DeserializeObject<List<RelicConfig>>(PlayerPrefs.GetString("TEST:RELIC"));
					PlayerMgr.Inst.BaData.relicCfgs.Clear();
					for (int l = 0; l < list2.Count; l++)
					{
						int intTimer = list2[l].intTimer;
						float floatTimer = list2[l].floatTimer;
						if (RelicConfig.dic[list2[l].id].abilityType == RelicAbilityType.WandAddSlot && list2[l].level > 1)
						{
							PlayerMgr.Inst.ItemCtrller.relicCfg_WandAddSlot = RelicConfig.GetConfig(list2[l].id);
							PlayerMgr.Inst.ItemCtrller.relicCfg_WandAddSlot.level = list2[l].level - 1;
							PlayerMgr.Inst.BaData.relicCfgs.Add(PlayerMgr.Inst.ItemCtrller.relicCfg_WandAddSlot);
							PlayerMgr.Inst.ItemCtrller.RelicAdd(list2[l].id, addGallery: false, fromLoadSave: true);
						}
						else
						{
							for (int m = 0; m < list2[l].level; m++)
							{
								PlayerMgr.Inst.ItemCtrller.RelicAdd(list2[l].id, addGallery: false, fromLoadSave: true);
							}
						}
						RelicConfig relicConfig = PlayerMgr.Inst.BaData.relicCfgs[l];
						relicConfig.intTimer = intTimer;
						relicConfig.floatTimer = floatTimer;
						PlayerMgr.Inst.BaData.relicCfgs[l] = relicConfig;
					}
				}
				UnityEngine.Debug.Log("加载测试法杖、背包法术、遗物");
			}
			if (Input.GetKey(KeyCode.C) && Input.GetKeyDown(KeyCode.Alpha1))
			{
				ResetDecoration();
			}
			if (Input.GetKey(KeyCode.C) && Input.GetKeyDown(KeyCode.Alpha2))
			{
				testCampDecorateNpc2 = !testCampDecorateNpc2;
				if (CampMgr.Inst != null)
				{
					CampMgr.Inst.NPC2DecorateStage(testCampDecorateNpc2);
				}
			}
			if (Input.GetKey(KeyCode.C) && Input.GetKeyDown(KeyCode.Alpha3))
			{
				testCampDecorateNpc3++;
				if (testCampDecorateNpc3 > 3)
				{
					ResetDecoration();
				}
				if (CampMgr.Inst != null)
				{
					CampMgr.Inst.NPC3DecorateStage(testCampDecorateNpc3);
				}
			}
			if (Input.GetKey(KeyCode.C) && Input.GetKeyDown(KeyCode.Alpha4))
			{
				testCampDecorateNpc4++;
				if (testCampDecorateNpc4 > 3)
				{
					ResetDecoration();
				}
				else if (CampMgr.Inst != null)
				{
					CampMgr.Inst.NPC4DecorateStage(testCampDecorateNpc4);
				}
			}
			if (Input.GetKey(KeyCode.C) && Input.GetKeyDown(KeyCode.Alpha5))
			{
				testCampDecorateNpc5 = !testCampDecorateNpc5;
				if (CampMgr.Inst != null)
				{
					CampMgr.Inst.NPC5DecorateShow(testCampDecorateNpc5);
				}
			}
			if (Input.GetKey(KeyCode.C) && Input.GetKeyDown(KeyCode.Alpha6))
			{
				testCampDecorateNpc6++;
				if (testCampDecorateNpc6 > 3)
				{
					ResetDecoration();
				}
				else if (CampMgr.Inst != null)
				{
					CampMgr.Inst.NPC6DecorateStage(testCampDecorateNpc6);
				}
			}
		}
		CalculateFps();
		void ResetDecoration()
		{
			testCampDecorateNpc2 = false;
			testCampDecorateNpc3 = 0;
			testCampDecorateNpc4 = 0;
			testCampDecorateNpc5 = false;
			testCampDecorateNpc6 = 0;
			if (CampMgr.Inst != null)
			{
				CampMgr.Inst.CloseAllDecorate();
			}
		}
	}

	private void ChangeTimeScale(float scale)
	{
		Time.timeScale = Mathf.Clamp(Time.timeScale, 0.01f, Time.timeScale + scale);
		MonoBehaviour.print("时间速率：" + Time.timeScale);
	}

	public void SwitchBattleUI()
	{
		if (UICampMgr.Inst != null || UIBattleMgr.Inst != null)
		{
			if (!UIPlayerDataMgr.Inst.anima_PlayerInfo.GetCurrentAnimatorStateInfo(0).IsName("HideDirect"))
			{
				UIMgr.Inst.showbattleui = false;
				UIPlayerDataMgr.Inst.HideDirect();
				GameUISingletonMono<UIBossHP>.Inst.gameObject.SetActive(value: false);
			}
			else
			{
				UIMgr.Inst.showbattleui = true;
				UIPlayerDataMgr.Inst.ShowDirect();
				GameUISingletonMono<UIBossHP>.Inst.gameObject.SetActive(value: true);
			}
		}
	}

	private void CalculateFps()
	{
		bool flag = false;
		fpsUpdateIntervalTimer += Time.deltaTime;
		while (fpsUpdateIntervalTimer >= 0.1f)
		{
			fpsUpdateIntervalTimer -= 0.1f;
			float item = 1f / Time.deltaTime;
			fpsList.Add(item);
			flag = true;
		}
		while ((float)fpsList.Count > 50f)
		{
			fpsList.RemoveAt(0);
		}
		if (flag)
		{
			m_FPS = ((fpsList.Count >= 3) ? fpsList.Skip(fpsList.Count - 3).Average() : fpsList.Average());
			m_FPS5 = fpsList.Average();
		}
	}

	public float GetFps()
	{
		return m_FPS;
	}

	public float GetFps5()
	{
		return m_FPS5;
	}

	public void TestButtonReloadLevel()
	{
		if (UIMgr.Inst.InputType == PlayerInputType.Keyboard)
		{
			ControlMgr.Inst.CursorVisibleSet(set: true);
			ControlMgr.Inst.CursorLockstateSet(CursorLockMode.None);
		}
		if (playerMgr.ItemCtrller.relic_BlockSpellMono != null)
		{
			playerMgr.ItemCtrller.relic_BlockSpellMono.DestroySelf();
		}
		if (playerMgr.ItemCtrller.relic_GluttonousSnake != null)
		{
			playerMgr.ItemCtrller.relic_GluttonousSnake.DestroySelf();
		}
		ClearAllPool();
		DataMgr.Initialize();
		while (!DataMgr.InitializeIsDone())
		{
			Thread.Sleep(10);
		}
		controlMgr.Initialize();
		commandLineMgr.Initialize();
		TextConfig.RegetAllText();
		AllFunctionReset();
		if (ScriptableObjMgr.Inst.testCtrller.DirectEnterScene)
		{
			DataMgr.SetSelectedWorldData(0);
			DataMgr.selectedWorldData.haveUsed = true;
			DataMgr.selectedWorldData.PlayTimeRecord();
			switch (ScriptableObjMgr.Inst.testCtrller.enterScene)
			{
			case TestDirectEnterScene.Camp:
			case TestDirectEnterScene.CampTraining:
			case TestDirectEnterScene.EndlessCamp:
				SceneManager.LoadScene("Camp");
				break;
			case TestDirectEnterScene.Guide:
				SceneManager.LoadScene("Guide");
				break;
			case TestDirectEnterScene.Guide2:
				SceneManager.LoadScene("Guide2");
				break;
			case TestDirectEnterScene.Battle:
				SceneManager.LoadScene("Battle");
				break;
			case TestDirectEnterScene.NormalFinishBackHome:
				SceneManager.LoadScene("EasyFinishBackHome");
				break;
			case TestDirectEnterScene.NPC7Appearance:
				SceneManager.LoadScene("NPC7Appearance");
				break;
			default:
				UnityEngine.Debug.LogError(ScriptableObjMgr.Inst.testCtrller.enterScene);
				break;
			}
		}
		else
		{
			SceneManager.LoadScene("MainMenu");
		}
	}

	public GameObject LoadSceneObj(LoadUI loadUI)
	{
		GameObject gameObject = null;
		Transform transform = null;
		Transform transform2 = null;
		if (IsMobile_Static && loadUI.skipLoadMobile)
		{
			return null;
		}
		switch (loadUI.mainRoot)
		{
		case mainRoot.none:
			transform = null;
			break;
		case mainRoot.canvas1:
			transform = Inst.uiMgr.canvas_1.transform;
			break;
		case mainRoot.canvas3:
			transform = Inst.uiMgr.rtsf_Canvas3.transform;
			break;
		case mainRoot.canvas5_entry:
			transform = Inst.uiMgr.canvas5_Entry.transform;
			break;
		case mainRoot.canvas5:
			if ((bool)UICampMgr.Inst)
			{
				transform = UICampMgr.Inst.rtsfCanvas;
			}
			if ((bool)UIBattleMgr.Inst)
			{
				transform = UIBattleMgr.Inst.rtsfCanvas;
			}
			if ((bool)UIGuideMgr.Inst)
			{
				transform = UIGuideMgr.Inst.rtsfCanvas;
			}
			if ((bool)UIMainMenuMgr.Inst)
			{
				transform = UIMainMenuMgr.Inst.rtsfcanvas;
			}
			break;
		case mainRoot.canvas10:
			transform = Inst.uiMgr.rtsf_Canvas10.transform;
			break;
		case mainRoot.canvas11:
			transform = Inst.uiMgr.canvas11.transform;
			break;
		default:
			transform = null;
			break;
		}
		string text;
		if (IsMobile_Static)
		{
			gameObject = (string.IsNullOrEmpty(loadUI.resourcePathMobile.path) ? (Resources.Load(loadUI.resourcePathPC.path) as GameObject) : (Resources.Load(loadUI.resourcePathMobile.path) as GameObject));
			text = (string.IsNullOrEmpty(loadUI.mobileRoot) ? loadUI.pcRoot : loadUI.mobileRoot);
		}
		else if (IsSteamDeck_Static)
		{
			gameObject = (string.IsNullOrEmpty(loadUI.resourcePathSteamdeck.path) ? (Resources.Load(loadUI.resourcePathPC.path) as GameObject) : (Resources.Load(loadUI.resourcePathSteamdeck.path) as GameObject));
			text = (string.IsNullOrEmpty(loadUI.steamDeckRoot) ? loadUI.pcRoot : loadUI.steamDeckRoot);
		}
		else
		{
			gameObject = Resources.Load(loadUI.resourcePathPC.path) as GameObject;
			text = loadUI.pcRoot;
		}
		transform2 = transform;
		string[] array = text.Split('/');
		foreach (string text2 in array)
		{
			if (transform2 == null)
			{
				UnityEngine.Debug.LogWarning(text2);
				transform2 = GameObject.Find(text2).transform;
			}
			else
			{
				transform2 = transform2.Find(text2);
			}
		}
		if (transform2 != null)
		{
			transform = transform2;
		}
		try
		{
			if (gameObject != null)
			{
				GameObject gameObject2 = UnityEngine.Object.Instantiate(gameObject, transform, worldPositionStays: false);
				gameObject2.name = gameObject.name;
				if (IsMobile_Static)
				{
					if (loadUI.ovverideMobileScale && loadUI.mobileScaler != 1f)
					{
						gameObject2.transform.localScale = new Vector3(loadUI.mobileScaler, loadUI.mobileScaler, loadUI.mobileScaler);
					}
					if (loadUI.mobilePositionOffset != Vector3.zero)
					{
						gameObject2.transform.position += loadUI.mobilePositionOffset;
					}
				}
				if (IsSteamDeck_Static && loadUI.ovverideSteamDeckScale && loadUI.steamDeckScaler != 1f)
				{
					gameObject2.transform.localScale = new Vector3(loadUI.steamDeckScaler, loadUI.steamDeckScaler, loadUI.steamDeckScaler);
				}
				if (loadUI.overrideSiblings && loadUI.overrideSiblingindex != -1)
				{
					gameObject2.transform.SetSiblingIndex(loadUI.overrideSiblingindex);
				}
				return gameObject2;
			}
			UnityEngine.Debug.LogError("没找到这个预制体:" + loadUI.resourcePathPC.path);
			return null;
		}
		catch (Exception exception)
		{
			UnityEngine.Debug.LogError(gameObject.name);
			UnityEngine.Debug.LogException(exception);
			return null;
		}
	}

	public void DestroyAllTeammate()
	{
		using EntityQuery entityQuery = ettMgr.CreateEntityQuery(typeof(TeammateData));
		NativeArray<Entity> nativeArray = entityQuery.ToEntityArray(Allocator.Temp);
		foreach (Entity item in nativeArray)
		{
			ettMgr.DestroyEntity(item);
		}
		nativeArray.Dispose();
		if ((bool)LevelMgr.Inst && (bool)LevelMgr.Inst.CurrentRoomCtrller)
		{
			LevelMgr.Inst.CurrentRoomCtrller.CheckAllEttRecords();
		}
		EventMgr.DestroyAllTeammate?.Invoke();
	}

	public void RecycleAllPool()
	{
		SEMgr.Inst.StopAllLoopSE();
		ObjPoolMgr.Inst.RecycleAll();
		CheckIsTeammatesEnableAccessMap();
		using (EntityQuery entityQuery = ettMgr.CreateEntityQuery(typeof(Spell2005GrimoireData)))
		{
			NativeArray<Entity> nativeArray = entityQuery.ToEntityArray(Allocator.Temp);
			foreach (Entity item in nativeArray)
			{
				CheckGrimoireChildTeammateFollowOwnerChanceSate(item);
			}
			nativeArray.Dispose();
			using EntityQuery entityQuery2 = ettMgr.CreateEntityQuery(typeof(EnterDoorDestroy));
			NativeArray<Entity> nativeArray2 = entityQuery2.ToEntityArray(Allocator.Temp);
			foreach (Entity item2 in nativeArray2)
			{
				ettMgr.DestroyEntity(item2);
			}
			nativeArray2.Dispose();
			using EntityQuery entityQuery3 = ettMgr.CreateEntityQuery(typeof(SpellSpawnParams));
			entityQuery3.GetSingletonBuffer<SpellSpawnParams>().Clear();
			if ((bool)LevelMgr.Inst && (bool)LevelMgr.Inst.CurrentRoomCtrller)
			{
				LevelMgr.Inst.CurrentRoomCtrller.CheckAllEttRecords();
			}
			using EntityQuery entityQuery4 = ettMgr.CreateEntityQuery(typeof(SpecialObj4_Dots));
			NativeArray<SpecialObj4_Dots> nativeArray3 = entityQuery4.ToComponentDataArray<SpecialObj4_Dots>(Allocator.Temp);
			foreach (SpecialObj4_Dots item3 in nativeArray3)
			{
				if (item3.curseID != 0)
				{
					playerMgr.BaData.BackCurseToPool(item3.curseID, 1);
				}
			}
			nativeArray3.Dispose();
		}
		void CheckGrimoireChildTeammateFollowOwnerChanceSate(Entity bookEntity)
		{
			DynamicBuffer<TeammateOwnerInfoBuffer> buffer = ettMgr.GetBuffer<TeammateOwnerInfoBuffer>(bookEntity);
			bool flag = ettMgr.IsComponentEnabled<EnterDoorDestroy>(bookEntity);
			for (int i = 0; i < buffer.Length; i++)
			{
				if (flag)
				{
					if (ettMgr.HasComponent<EnterDoorDestroy>(buffer[i].TeammateEntity))
					{
						ettMgr.SetComponentEnabled<EnterDoorDestroy>(buffer[i].TeammateEntity, value: true);
					}
					if (ettMgr.HasComponent<Spell2002Data>(buffer[i].TeammateEntity))
					{
						Spell2002SyncSystem.DestroyMono(buffer[i].TeammateEntity);
					}
					if (ettMgr.HasComponent<Spell2005GrimoireData>(buffer[i].TeammateEntity))
					{
						CheckGrimoireChildTeammateFollowOwnerChanceSate(buffer[i].TeammateEntity);
					}
				}
			}
		}
	}

	public void CheckIsTeammatesEnableAccessMap()
	{
		using EntityQuery entityQuery = ettMgr.CreateEntityQuery(typeof(TeammateData));
		foreach (Entity item in entityQuery.ToEntityArray(Allocator.Temp))
		{
			TeammateData componentData = ettMgr.GetComponentData<TeammateData>(item);
			switch (componentData.TeammateType)
			{
			case TeammateType.teammate2:
			case TeammateType.teammate6:
				ettMgr.SetComponentEnabled<EnterDoorDestroy>(item, value: false);
				break;
			case TeammateType.teammate1:
			case TeammateType.teammate3:
			case TeammateType.teammate4:
			case TeammateType.teammate5:
			case TeammateType.teammate7:
				ettMgr.SetComponentEnabled<EnterDoorDestroy>(item, componentData.SummonFollowOwnerThroughMapChance < UnityEngine.Random.Range(0f, 1f));
				break;
			}
		}
	}

	public void ClearAllPool()
	{
		playerMgr.WandRemoveSpellTypePassive();
		SEMgr.Inst.ClearAllPool();
		ObjPoolMgr.Inst.ClearAllPool();
		CheckIsTeammatesEnableAccessMap();
		using EntityQuery entityQuery = ettMgr.CreateEntityQuery(typeof(EnterDoorDestroy));
		NativeArray<Entity> nativeArray = entityQuery.ToEntityArray(Allocator.Temp);
		foreach (Entity item in nativeArray)
		{
			ettMgr.DestroyEntity(item);
		}
		nativeArray.Dispose();
		if ((bool)LevelMgr.Inst && (bool)LevelMgr.Inst.CurrentRoomCtrller)
		{
			LevelMgr.Inst.CurrentRoomCtrller.CheckAllEttRecords();
		}
		if (CampMgr.Inst == null)
		{
			Resources.UnloadUnusedAssets().completed += delegate
			{
				UnityEngine.Debug.Log("Unload assets done");
			};
			GC.Collect();
		}
	}

	public void AllFunctionReset()
	{
		TimeScaleMgr.Inst.AddNewTimeScaleModifyRequest(1f, 0f, 10f);
		CamController.Inst.MouseOffsetContinue();
		CamController.Inst.FocusRecover(0f);
		UIPlayerDataMgr.Inst.HideDirect();
		UIPlayerDataMgr.Inst.ResetAllResourceLayer();
		UIPlayerDataMgr.Inst.BagBGToDefault();
		UIMgr.Inst.uiFilmBlackEdge.Hide(0f);
		uiInteractiveObjMgr.Reset();
		if (GameUISingletonMono<UIChapterThrough>.Inited)
		{
			GameUISingletonMono<UIChapterThrough>.Inst.HideDirect();
		}
		if (GameUISingletonMono<UIDialogueMgr>.Inited)
		{
			GameUISingletonMono<UIDialogueMgr>.Inst.HDHideDirect();
		}
		EventMgr.DotsSystemReset?.Invoke();
	}

	public GameObject LoadUIObj(string ObjName)
	{
		foreach (LoadSceneObjSO loadSceneObjSO in LoadSceneObjSOs)
		{
			foreach (LoadUIs allObjLoad in loadSceneObjSO.AllObjLoads)
			{
				foreach (LoadUI item in allObjLoad)
				{
					if (item.initWhenOpenForUI && item.resourcePathPC.path.Split("/").Last() == ObjName)
					{
						return Inst.LoadSceneObj(item);
					}
				}
			}
		}
		return null;
	}

	private void PrintGameProgramInfo()
	{
		if (!IsMobile_Static)
		{
			UnityEngine.Debug.Log(ProgramInfo.MakeProgramInfo());
		}
	}

	public IEnumerator WaitAndInvokeAction(int frameWait, Action action)
	{
		for (int i = 0; i < frameWait; i++)
		{
			yield return new WaitForEndOfFrame();
		}
		action?.Invoke();
	}

	public void RunTimed(Action action)
	{
		Stopwatch stopwatch = new Stopwatch();
		stopwatch.Start();
		action();
		stopwatch.Stop();
		UnityEngine.Debug.LogError(stopwatch.ElapsedMilliseconds);
	}

	public static void QuitGame()
	{
		if (IsUseBiliOneSDK && !IsMobile_Static)
		{
			BiliOneSDKMgr.OneSDKUnInit();
		}
		Application.Quit();
		if (IsMobile_Static)
		{
			PluginActivity.Inst.ForceKillAndroid();
		}
	}

	public void UpdateVFXSuport()
	{
		string text = SystemInfo.graphicsDeviceName?.ToLower() ?? "";
		string text2 = SystemInfo.graphicsDeviceType.ToString();
		string item = SystemInfo.deviceModel ?? "";
		bool num = new HashSet<string> { "iPad13,1", "iPad13,2" }.Contains(item);
		bool flag = text.Contains("mali") || text.Contains("Maleoon");
		bool flag2 = text2.Contains("OpenGL");
		isSupportVFX = !num && !(flag && flag2);
	}

	public void UpdateVideoSupport()
	{
		IsSupportVideo = true;
	}

	public void CheckIsLightErrorDivice()
	{
		isLightErrorDevice = (SystemInfo.graphicsDeviceName?.ToLower() ?? "").Contains("g720");
	}

	public void SaveDataToString()
	{
		string text = SaveDataCompressor.CompressSaveData(new StringSaveData
		{
			w = PlayerMgr.Inst.BaData.wandCfgs,
			b = playerMgr.BaData.bagSpellDatas.Where((SlotData e) => e != null && e.id > 0).ToArray(),
			r = PlayerMgr.Inst.BaData.relicCfgs
		});
		UnityEngine.Debug.Log("配置代码已复制到剪贴板: \n" + text + "\n");
		GUIUtility.systemCopyBuffer = text;
	}

	public void LoadDataFromStringSaveData(StringSaveData data)
	{
		List<WandConfig> w = data.w;
		int index = ((playerMgr.SelectedWandIndex >= 0) ? playerMgr.SelectedWandIndex : 0);
		for (int i = 0; i < w.Count; i++)
		{
			playerMgr.SetWand(i, w[i]);
		}
		playerMgr.WandSelect(index);
		playerMgr.AllWandFullMP();
		SlotData[] b = data.b;
		foreach (SlotData spellData in b)
		{
			playerMgr.SpellPick(spellData);
		}
		List<RelicConfig> r = data.r;
		PlayerMgr.Inst.BaData.relicCfgs.Clear();
		for (int k = 0; k < r.Count; k++)
		{
			int intTimer = r[k].intTimer;
			float floatTimer = r[k].floatTimer;
			if (RelicConfig.dic[r[k].id].abilityType == RelicAbilityType.WandAddSlot && r[k].level > 1)
			{
				PlayerMgr.Inst.ItemCtrller.relicCfg_WandAddSlot = RelicConfig.GetConfig(r[k].id);
				PlayerMgr.Inst.ItemCtrller.relicCfg_WandAddSlot.level = r[k].level - 1;
				PlayerMgr.Inst.BaData.relicCfgs.Add(PlayerMgr.Inst.ItemCtrller.relicCfg_WandAddSlot);
				PlayerMgr.Inst.ItemCtrller.RelicAdd(r[k].id, addGallery: false, fromLoadSave: true);
			}
			else
			{
				for (int l = 0; l < r[k].level; l++)
				{
					PlayerMgr.Inst.ItemCtrller.RelicAdd(r[k].id, addGallery: false, fromLoadSave: true);
				}
			}
			RelicConfig relicConfig = PlayerMgr.Inst.BaData.relicCfgs[k];
			relicConfig.intTimer = intTimer;
			relicConfig.floatTimer = floatTimer;
			PlayerMgr.Inst.BaData.relicCfgs[k] = relicConfig;
		}
	}
}
