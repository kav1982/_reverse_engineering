using UnityEngine;

[CreateAssetMenu(fileName = "TestCtrllerSO", menuName = "ScriptableObjects/TestCtrllerSO", order = 100)]
public class TestController : ScriptableObject
{
	public enum EndlessTestType
	{
		Normal,
		Boss,
		Elite
	}

	public TestItemForceAvailable OverrideForceChinaSteam;

	public TestItemForceAvailable OverrideHaveGame = TestItemForceAvailable.Have;

	public TestItemForceAvailable OverrideHaveHolloweenDLC = TestItemForceAvailable.Have;

	public TestItemForceAvailable OverrideHaveSpringDLC;

	public TestItemForceAvailable OverrideHaveChristmasDLC = TestItemForceAvailable.Have;

	public TestItemForceAvailable OverrideHaveSummerDLC;

	public TestItemForceAvailable OverrideHaveDaveDLC = TestItemForceAvailable.Have;

	public TestItemForceAvailable OverrideHaveEndlessDLC = TestItemForceAvailable.Have;

	public bool skipDaveDialogue;

	public bool UseServer;

	public bool CheckSteam;

	public bool StreamerPackege;

	public TestBWBattleMgrType StreamerPackegeType;

	public bool UseBiliOneSDK;

	public bool DisableSteam;

	public bool BLive;

	public CampSkinType campSkinType;

	public TestGameServerEnum gameServer;

	public TestHarmonyScale harmonyScale;

	public bool ShowFps;

	public bool MasterSwitch;

	public bool _dontUse_publishTesting;

	public bool _dontUse_androidTesting;

	public bool _dontUse_steamDeckTesting;

	public bool _dontUse_bLiveTestMode;

	public bool _dontUse_disableLowFrameDynamicOptimize;

	public bool _dontUse_commandLine;

	public string autoCommand = "";

	public bool _dontUse_shortcut;

	public bool _dontUse_directEnterScene;

	public TestDirectEnterScene enterScene;

	public bool _dontUse_unlockAllNPC;

	public bool _dontUse_unlockAllGallery;

	public bool _dontUse_skipAllStoryMixed;

	public bool _dontUse_showItemID;

	public bool _dontUse_showGift;

	public bool _dontUse_forceNotSupportVFX;

	public bool _dontUse_battleSkipChapterThrough;

	public bool _dontUse_battleSideRoom;

	public bool _dontUse_battleRRO;

	public bool _dontUse_battleAllSpell;

	public bool _dontUse_battleAllRelic;

	public bool _dontUse_battleAllPotion;

	public bool _dontUse_battleCoin1000;

	public bool _dontUse_battleKey100;

	public bool _dontUse_battleSkipBossShow;

	public bool _dontUse_battleRuinedDoor;

	public bool _dontUse_battleInfiniteReroll;

	public bool _dontUse_battle100PercentEpicItem;

	public float battleBossHpDivide = 1f;

	public bool _dontUse_chapter5Experience;

	public bool _dontUse_battleAllWand;

	public int battleStageWandOnGround;

	public int battleStageWandRandomOnHand;

	public bool _dontUse_battleChapter;

	public int battleStage = 1;

	public int battleLevel;

	public LevelRewardType battleRewardType;

	public bool _dontUse_useEndlessTestStage;

	public EndlessTestType _dontUse_endlessTestType;

	public int endlessTestBossID;

	public int endlessTestEliteID;

	public EndlessBattleSpawnInfo.StageSpawnInfo endlessWaveInfo;

	public bool _dontUse_battleItem;

	public ItemType[] battleItemTypes;

	public int[] battleItemIDs;

	public bool _dontUse_battleCurse;

	public int[] battleCurseIDs;

	public bool _dontUse_guideSkipStory1Computer;

	public bool _dontUse_guideSkipStory2Toilet;

	public bool _dontUse_guideSkipStory3Pee;

	public bool _dontUse_guideStandToWand;

	public bool _dontUse_guideSkipPickWand;

	public bool _dontUse_guide2SkipDialogue1;

	public bool _dontUse_guide2SkipDialogue2;

	public bool isBW
	{
		get
		{
			if (ScriptableObjMgr.Inst.testCtrller.StreamerPackege)
			{
				return ScriptableObjMgr.Inst.testCtrller.StreamerPackegeType == TestBWBattleMgrType.BattleChapter5;
			}
			return false;
		}
	}

	public bool publishTesting
	{
		get
		{
			if (MasterSwitch)
			{
				return _dontUse_publishTesting;
			}
			return false;
		}
		set
		{
			_dontUse_publishTesting = value;
		}
	}

	public bool AndroidTesting
	{
		get
		{
			if (MasterSwitch)
			{
				return _dontUse_androidTesting;
			}
			return false;
		}
		set
		{
			_dontUse_androidTesting = value;
		}
	}

	public bool SteamDeckTesting
	{
		get
		{
			if (MasterSwitch)
			{
				return _dontUse_steamDeckTesting;
			}
			return false;
		}
		set
		{
			_dontUse_steamDeckTesting = value;
		}
	}

	public bool BLiveTestMode
	{
		get
		{
			if (MasterSwitch && _dontUse_bLiveTestMode)
			{
				return true;
			}
			return false;
		}
		set
		{
			_dontUse_bLiveTestMode = value;
		}
	}

	public bool DisableLowFrameDynamicOptimize
	{
		get
		{
			if (MasterSwitch && _dontUse_disableLowFrameDynamicOptimize)
			{
				return true;
			}
			return false;
		}
		set
		{
			_dontUse_disableLowFrameDynamicOptimize = value;
		}
	}

	public bool CommandLine
	{
		get
		{
			if (MasterSwitch && _dontUse_commandLine)
			{
				return true;
			}
			return false;
		}
		set
		{
			_dontUse_commandLine = value;
		}
	}

	public bool Shortcut
	{
		get
		{
			if (MasterSwitch && _dontUse_shortcut)
			{
				return true;
			}
			return false;
		}
		set
		{
			_dontUse_shortcut = value;
		}
	}

	public bool DirectEnterScene
	{
		get
		{
			if (MasterSwitch && _dontUse_directEnterScene)
			{
				return true;
			}
			return false;
		}
		set
		{
			_dontUse_directEnterScene = value;
		}
	}

	public bool UnlockAllNPC
	{
		get
		{
			if (MasterSwitch && _dontUse_unlockAllNPC)
			{
				return true;
			}
			return false;
		}
		set
		{
			_dontUse_unlockAllNPC = value;
		}
	}

	public bool UnlockAllGallery
	{
		get
		{
			if (MasterSwitch && _dontUse_unlockAllGallery)
			{
				return true;
			}
			return false;
		}
		set
		{
			_dontUse_unlockAllGallery = value;
		}
	}

	public bool SkipAllStoryMixed
	{
		get
		{
			if (MasterSwitch && _dontUse_skipAllStoryMixed)
			{
				return true;
			}
			return false;
		}
		set
		{
			_dontUse_skipAllStoryMixed = value;
		}
	}

	public bool ShowItemID
	{
		get
		{
			if (MasterSwitch && _dontUse_showItemID)
			{
				return true;
			}
			return false;
		}
		set
		{
			_dontUse_showItemID = value;
		}
	}

	public bool ShowGift
	{
		get
		{
			if (MasterSwitch && _dontUse_showGift)
			{
				return true;
			}
			return false;
		}
		set
		{
			_dontUse_showGift = value;
		}
	}

	public bool forceNotSupportVFX
	{
		get
		{
			if (MasterSwitch && _dontUse_forceNotSupportVFX)
			{
				return true;
			}
			return false;
		}
		set
		{
			_dontUse_forceNotSupportVFX = value;
		}
	}

	public bool BattleSkipChapterThrough
	{
		get
		{
			if (MasterSwitch && _dontUse_battleSkipChapterThrough)
			{
				return true;
			}
			return false;
		}
		set
		{
			_dontUse_battleSkipChapterThrough = value;
		}
	}

	public bool BattleSideRoom
	{
		get
		{
			if (MasterSwitch && _dontUse_battleSideRoom)
			{
				return true;
			}
			return false;
		}
		set
		{
			_dontUse_battleSideRoom = value;
		}
	}

	public bool BattleRRO
	{
		get
		{
			if (MasterSwitch && _dontUse_battleRRO)
			{
				return true;
			}
			return false;
		}
		set
		{
			_dontUse_battleRRO = value;
		}
	}

	public bool BattleAllSpell
	{
		get
		{
			if (MasterSwitch && _dontUse_battleAllSpell)
			{
				return true;
			}
			return false;
		}
		set
		{
			_dontUse_battleAllSpell = value;
		}
	}

	public bool BattleAllRelic
	{
		get
		{
			if (MasterSwitch && _dontUse_battleAllRelic)
			{
				return true;
			}
			return false;
		}
		set
		{
			_dontUse_battleAllRelic = value;
		}
	}

	public bool BattleAllPotion
	{
		get
		{
			if (MasterSwitch && _dontUse_battleAllPotion)
			{
				return true;
			}
			return false;
		}
		set
		{
			_dontUse_battleAllPotion = value;
		}
	}

	public bool BattleCoin1000
	{
		get
		{
			if (MasterSwitch && _dontUse_battleCoin1000)
			{
				return true;
			}
			return false;
		}
		set
		{
			_dontUse_battleCoin1000 = value;
		}
	}

	public bool BattleKey100
	{
		get
		{
			if (MasterSwitch && _dontUse_battleKey100)
			{
				return true;
			}
			return false;
		}
		set
		{
			_dontUse_battleKey100 = value;
		}
	}

	public bool BattleSkipBossShow
	{
		get
		{
			if (MasterSwitch && _dontUse_battleSkipBossShow)
			{
				return true;
			}
			return false;
		}
		set
		{
			_dontUse_battleSkipBossShow = value;
		}
	}

	public bool BattleRuinedDoor
	{
		get
		{
			if (MasterSwitch && _dontUse_battleRuinedDoor)
			{
				return true;
			}
			return false;
		}
		set
		{
			_dontUse_battleRuinedDoor = value;
		}
	}

	public bool BattleInfiniteReroll
	{
		get
		{
			if (MasterSwitch && _dontUse_battleInfiniteReroll)
			{
				return true;
			}
			return false;
		}
		set
		{
			_dontUse_battleInfiniteReroll = value;
		}
	}

	public bool Battle100PercentEpicItem
	{
		get
		{
			if (MasterSwitch && _dontUse_battle100PercentEpicItem)
			{
				return true;
			}
			return false;
		}
		set
		{
			_dontUse_battle100PercentEpicItem = value;
		}
	}

	public bool Chapter5Experience
	{
		get
		{
			if (MasterSwitch && _dontUse_chapter5Experience)
			{
				return true;
			}
			return false;
		}
		set
		{
			_dontUse_chapter5Experience = value;
		}
	}

	public bool BattleAllWand
	{
		get
		{
			if (MasterSwitch && _dontUse_battleAllWand)
			{
				return true;
			}
			return false;
		}
		set
		{
			_dontUse_battleAllWand = value;
		}
	}

	public bool BattleChapter
	{
		get
		{
			if (MasterSwitch && _dontUse_battleChapter)
			{
				return true;
			}
			return false;
		}
		set
		{
			_dontUse_battleChapter = value;
		}
	}

	public bool UseEndlessTestStage
	{
		get
		{
			if (MasterSwitch && _dontUse_useEndlessTestStage)
			{
				return true;
			}
			return false;
		}
		set
		{
			_dontUse_useEndlessTestStage = value;
		}
	}

	public EndlessTestType endlessTestType
	{
		get
		{
			return _dontUse_endlessTestType;
		}
		set
		{
			_dontUse_endlessTestType = value;
		}
	}

	public bool BattleItem
	{
		get
		{
			if (MasterSwitch && _dontUse_battleItem)
			{
				return true;
			}
			return false;
		}
		set
		{
			_dontUse_battleItem = value;
		}
	}

	public bool BattleCurse
	{
		get
		{
			if (MasterSwitch && _dontUse_battleCurse)
			{
				return true;
			}
			return false;
		}
		set
		{
			_dontUse_battleCurse = value;
		}
	}

	public bool GuideSkipStory1Computer
	{
		get
		{
			if (MasterSwitch && _dontUse_guideSkipStory1Computer)
			{
				return true;
			}
			return false;
		}
		set
		{
			_dontUse_guideSkipStory1Computer = value;
		}
	}

	public bool GuideSkipStory2Toilet
	{
		get
		{
			if (MasterSwitch && _dontUse_guideSkipStory2Toilet)
			{
				return true;
			}
			return false;
		}
		set
		{
			_dontUse_guideSkipStory2Toilet = value;
		}
	}

	public bool GuideSkipStory3Pee
	{
		get
		{
			if (MasterSwitch && _dontUse_guideSkipStory3Pee)
			{
				return true;
			}
			return false;
		}
		set
		{
			_dontUse_guideSkipStory3Pee = value;
		}
	}

	public bool GuideStandToWand
	{
		get
		{
			if (MasterSwitch && _dontUse_guideStandToWand)
			{
				return true;
			}
			return false;
		}
		set
		{
			_dontUse_guideStandToWand = value;
		}
	}

	public bool GuideSkipPickWand
	{
		get
		{
			if (MasterSwitch && _dontUse_guideSkipPickWand)
			{
				return true;
			}
			return false;
		}
		set
		{
			_dontUse_guideSkipPickWand = value;
		}
	}

	public bool Guide2SkipDialogue1
	{
		get
		{
			if (MasterSwitch && _dontUse_guide2SkipDialogue1)
			{
				return true;
			}
			return false;
		}
		set
		{
			_dontUse_guide2SkipDialogue1 = value;
		}
	}

	public bool Guide2SkipDialogue2
	{
		get
		{
			if (MasterSwitch && _dontUse_guide2SkipDialogue2)
			{
				return true;
			}
			return false;
		}
		set
		{
			_dontUse_guide2SkipDialogue2 = value;
		}
	}
}
