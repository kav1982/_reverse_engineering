using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Unity.Entities;
using UnityEngine;

public class WorldData
{
	private struct Dialog
	{
		public int id;

		public NPCPlot npc;

		[CanBeNull]
		public Func<bool> Condition;
	}

	public DifficultyType selectedDifficulty;

	public List<DifficultyType> finishedDifficulty = new List<DifficultyType>();

	public BattleData battleData9;

	public bool inBattle9;

	public CampSkinType campSkinType;

	public PlayerLook playerLook;

	public long timeStampOnStartUsing;

	public bool haveUsed;

	public int deadCount;

	public bool showTourHint;

	public float timeuse;

	public bool firstEnterBattle = true;

	public bool firstPickSpell = true;

	public int magicCrystalCount;

	public int ancientBloodCount;

	public int chaosCoreCount;

	public int GearCount;

	public float playTime;

	public int enterBattleTime;

	public bool hadBlood;

	public bool hadCore;

	public bool isReachChatper2;

	public bool isReachChatper3;

	public bool isReachChatper4;

	public bool isReachChatper5;

	public bool isScarecrowOpen = true;

	public bool isPlayerDeadBackCamp;

	public bool isTriggerTutorialHpShow;

	public bool toAppleStoreTirggered;

	public int BestEndlessLevel;

	public int levelOfWandLimit;

	public int levelOfBagLimit;

	public int levelOfEnterDoorRecovery;

	public bool isTalentUnlock1;

	public int levelOfMaxHP;

	public int levelOfHPRoom;

	public bool isTalentUnlock2;

	public int levelOfInitialCoin;

	public int levelOfCoinRoom;

	public bool isTalentUnlock3;

	public int levelOfSpellRoom;

	public int levelOfRelicRoom;

	public bool isTalentUnlock4;

	public int levelOfMaxMP;

	public int levelOfMPRecover;

	public bool isCoverTo0_80Version;

	public bool isCoverTo0_82Version;

	public bool isCoverTo1_0Version;

	public bool isCoverTo1_0_29Version;

	public bool isCoverTo1_0_32Version;

	public bool isCoverTo1_2_19Version;

	public int levelOfWandLimit_2;

	public int levelOfBagLimit_2;

	public int levelOfMaxHP_2;

	public bool isTalentUnlock1_2;

	public int levelOfPotionLimit_2;

	public int levelOfHPRoom_2;

	public bool isTalentUnlock2_2;

	public int levelOfInitialCoin_2;

	public int levelOfCoinRoom_2;

	public bool isTalentUnlock3_2;

	public int levelOfRelicRoom_2;

	public int levelOfSpellRoom_2;

	public bool isTalentUnlock4_2;

	public int levelOfMaxMP_2;

	public int levelOfMPRecover_2;

	public int endless_LevelOfSupplyBox;

	public int endless_LevelOfGoodsExtraCount;

	public int endless_LevelOfGallery;

	public int endless_LevelOfFinishCoin;

	public int endless_LevelOfLcokMachine;

	public int endless_LevelOfHightLevelSpell;

	public int endless_LevelOfProcessSpell;

	public int endless_LevelOfMaxHP;

	public int endless_LevelOfExtraDamage;

	public List<int> researchedIDs = new List<int>();

	public List<int> researchHoveredIDs = new List<int>();

	public List<int> researchDisactive = new List<int>();

	public Dictionary<int, int> setUnlockedSets = new Dictionary<int, int>
	{
		{ 1, 1 },
		{ 2, 1 }
	};

	public bool FindSet3;

	public bool FindSet4;

	public bool FindSet5;

	public bool FindSet6;

	public bool FindSet7;

	public bool FindSet8;

	public bool FindSet9;

	public bool FindSet10;

	public bool FindSet12;

	public int selectedSetID = 1;

	public int set6KillCounter;

	public bool canSetUpgrade;

	public bool oldPlayerGift;

	public bool useGift;

	public bool OpenHandbookOnce;

	public bool MenuGalleryDot;

	public List<int> galleryUnlockedMonsters = new List<int>();

	public List<int> galleryUnlockedBosses = new List<int>();

	public List<int> galleryUnlockedWands = new List<int>();

	public List<int> galleryUnlockedSpells = new List<int>();

	public List<int> galleryUnlockedRelics = new List<int>();

	public List<int> galleryUnlockedPotions = new List<int>();

	public List<int> galleryUnlockedCurses = new List<int>();

	public Dictionary<int, int> galleryKilledMonsterCounts = new Dictionary<int, int>();

	public Dictionary<int, int> galleryKilledBossCounts = new Dictionary<int, int>();

	public Dictionary<int, int> galleryRelicGetTimes = new Dictionary<int, int>();

	public Dictionary<int, int> galleryPotionUseTimes = new Dictionary<int, int>();

	public Dictionary<int, int> galleryCurseGetTimes = new Dictionary<int, int>();

	public List<int> activateGirlActivatedIDs2 = new List<int>();

	public int spellDisableCost_Crystal2;

	public int spellDisableCost_Blood2;

	public List<int> spellDisableFreeIDs3 = new List<int>();

	public List<List<int>> spellDisableHistory2 = new List<List<int>>();

	public bool story1Finish;

	public bool story2Open;

	public bool story2Finish;

	public bool story3PlayerRoomEnter;

	public bool story3NPC4Rescued;

	public bool story3Finish;

	public bool story3NPC4GiveCloth;

	public bool story4PlayerRoomEnter;

	public bool story4NPC5Rescued;

	public bool story4Finish;

	public bool story4NPC5ForceShow;

	public bool storyKillChapter3BossPickup;

	public bool storyNormalFinishBackCamp;

	public bool storyKillChapter3BossBackCamp;

	public bool storyHardBossDropPickup;

	public bool storyHardFinishBackCamp;

	public bool storyHardFinishNPC7Appearance;

	public bool storyHardFinishNPC7OpenFunction;

	public bool storyFinishHardDropPickup;

	public bool storyFinishHardBackCamp;

	public bool storyFinishNightmare1;

	public bool storyFinishNightmare1BackCamp;

	public bool storyFinishNightmare2;

	public bool storyFinishNightmare2BackCamp;

	public bool storyFinishNightmare3;

	public bool storyFinishNightmare3BackCamp;

	public bool haveSeeLishujian;

	public bool storyMixedFirstFinishLevel;

	public bool storyMixedFirstEncounterElite;

	public bool storyMixedFirstEnterChapter2;

	public bool storyMixedFirstEnterChapter3;

	public bool storyMixedFirstEnterChapter4;

	public bool storyMixedFirstEnterChapter5;

	public bool storyMixedFirstEnterBloodRoom;

	public bool storyMixedFirstPickPostSlotWand;

	public bool storyMixedSecondEnterBattle;

	public bool daveMirrorTalk;

	public bool daveSpringTalk;

	public bool daveFirstMeetBoss9 = true;

	public bool daveFirstMeetBoss10 = true;

	public bool daveFirstMeetBoss13 = true;

	public bool daveKilledBoss;

	public bool daveKilledBoss1;

	public bool daveKilledBoss4;

	public readonly List<int> finishedRandomDialogs = new List<int>();

	public NPCPlot npc1VivianImportantPlot = new NPCPlot(9);

	public NPCPlot npc2NimueImportantPlot = new NPCPlot(10);

	public NPCPlot npc3ImportantPlot = new NPCPlot(14);

	public NPCPlot npc4ImportantPlot = new NPCPlot(0);

	public NPCPlot npc5ImportantPlot = new NPCPlot(0);

	public NPCPlot npc6ImportantPlot = new NPCPlot(0);

	public NPCPlot npc7ImportantPlot = new NPCPlot(0);

	public NPCPlot npc9ImportantPlot = new NPCPlot(0);

	public NPCPlot npc1VivianSchedulePlot = new NPCPlot(0);

	public NPCPlot npc2NimueSchedulePlot = new NPCPlot(0);

	public NPCPlot npc3SchedulePlot = new NPCPlot(0);

	public NPCPlot npc4SchedulePlot = new NPCPlot(0);

	public NPCPlot npc5SchedulePlot = new NPCPlot(0);

	public NPCPlot npc6SchedulePlot = new NPCPlot(0);

	public NPCPlot npc7SchedulePlot = new NPCPlot(0);

	public NPCPlot npc9SchedulePlot = new NPCPlot(0);

	public NPCPlot npc1VivianCasualPlot = new NPCPlot(0);

	public NPCPlot npc2NimueCasualPlot = new NPCPlot(0);

	public NPCPlot npc3CasualPlot = new NPCPlot(0);

	public NPCPlot npc4CasualPlot = new NPCPlot(0);

	public NPCPlot npc5CasualPlot = new NPCPlot(0);

	public NPCPlot npc6CasualPlot = new NPCPlot(0);

	public NPCPlot npc7CasualPlot = new NPCPlot(0);

	public NPCPlot npc9CasualPlot = new NPCPlot(0);

	public NPCPlot npc1VivianRandomPlotV2 = new NPCPlot(0);

	public NPCPlot npc2NimueRandomPlotV2 = new NPCPlot(0);

	public NPCPlot npc3RandomPlotV2 = new NPCPlot(0);

	public NPCPlot npc4RandomPlotV2 = new NPCPlot(0);

	public NPCPlot npc5RandomPlotV2 = new NPCPlot(0);

	public NPCPlot npc6RandomPlotV2 = new NPCPlot(0);

	public NPCPlot npc7RandomPlotV2 = new NPCPlot(0);

	public NPCPlot npc9RandomPlotV2 = new NPCPlot(0);

	[JsonIgnore]
	public bool directEnterCampByLoadSave;

	private Dictionary<ResearchAbilityType, int> researchValues = new Dictionary<ResearchAbilityType, int>();

	private bool activeGirlHave3Pick2;

	private bool activaGrilHaveSpellLock;

	private bool activeGirlHave4Pick2;

	private float lastRecordPlayTime;

	public bool mobilePotionDragTutorialShown;

	public bool mobileWandDragTutorialShown;

	[JsonIgnore]
	private Dialog[] _deadRandomDialogs;

	[JsonIgnore]
	private Dialog[] _reEnterCampDialogs;

	[JsonIgnore]
	public bool IsDave
	{
		get
		{
			if (ICJNOGPFMAM.GGPJCCLPBJL && DataMgr.selectedWorldData != null)
			{
				return DataMgr.selectedWorldData.selectedSetID == 11;
			}
			return false;
		}
	}

	[JsonIgnore]
	public bool InDaveRoom
	{
		get
		{
			if ((bool)BattleMgr.Inst && DataMgr.selectedWorldData.battleData9 != null)
			{
				return IDMgr.DaveRoom.Contains(DataMgr.selectedWorldData.battleData9.currentRoomID);
			}
			return false;
		}
	}

	[JsonIgnore]
	public bool InBuyGameRoom
	{
		get
		{
			if ((bool)BattleMgr.Inst && DataMgr.selectedWorldData.inBattle9)
			{
				return LevelMgr.Inst.CurrentRoomCfg.id == 110;
			}
			return false;
		}
	}

	[JsonIgnore]
	private Dialog[] DeadRandomDialogs
	{
		get
		{
			Dialog[] array = _deadRandomDialogs;
			if (array == null)
			{
				SpellConfig value;
				Dialog[] obj = new Dialog[10]
				{
					new Dialog
					{
						id = 3111,
						npc = npc1VivianRandomPlotV2
					},
					new Dialog
					{
						id = 3112,
						npc = npc1VivianRandomPlotV2
					},
					new Dialog
					{
						id = 3113,
						npc = npc1VivianRandomPlotV2
					},
					new Dialog
					{
						id = 3121,
						npc = npc2NimueRandomPlotV2
					},
					new Dialog
					{
						id = 3131,
						npc = npc3RandomPlotV2
					},
					new Dialog
					{
						id = 3132,
						npc = npc3RandomPlotV2,
						Condition = () => galleryUnlockedSpells.Any((int e) => SpellConfig.dic.TryGetValue(e, out value) && value.abilityType != SpellAbilityType.ManaCoin && value.dropType == ItemDropType.Special)
					},
					new Dialog
					{
						id = 3141,
						npc = npc4RandomPlotV2,
						Condition = () => selectedSetID != 1
					},
					new Dialog
					{
						id = 3151,
						npc = npc5RandomPlotV2
					},
					new Dialog
					{
						id = 3161,
						npc = npc6RandomPlotV2
					},
					new Dialog
					{
						id = 3171,
						npc = npc7RandomPlotV2
					}
				};
				Dialog[] array2 = obj;
				_deadRandomDialogs = obj;
				array = array2;
			}
			return array;
		}
	}

	[JsonIgnore]
	private Dialog[] ReEnterCampDialogs
	{
		get
		{
			Dialog[] array = _reEnterCampDialogs;
			if (array == null)
			{
				Dialog[] obj = new Dialog[5]
				{
					new Dialog
					{
						id = 3211,
						npc = npc1VivianRandomPlotV2
					},
					new Dialog
					{
						id = 3221,
						npc = npc2NimueRandomPlotV2
					},
					new Dialog
					{
						id = 3231,
						npc = npc3RandomPlotV2
					},
					new Dialog
					{
						id = 3241,
						npc = npc4RandomPlotV2,
						Condition = delegate
						{
							int num = selectedSetID;
							if (num != 9 && num != 8 && num != 7 && num != 6)
							{
								PlayerLook playerLook = this.playerLook;
								return playerLook == PlayerLook.Default || playerLook == PlayerLook.Jojo;
							}
							return false;
						}
					},
					new Dialog
					{
						id = 3261,
						npc = npc6RandomPlotV2
					}
				};
				Dialog[] array2 = obj;
				_reEnterCampDialogs = obj;
				array = array2;
			}
			return array;
		}
	}

	public void AddRandomFinishedDialogueID(int id)
	{
		if (!finishedRandomDialogs.Contains(id))
		{
			finishedRandomDialogs.Add(id);
		}
	}

	private bool RandomDialogueIsFinished(int id)
	{
		return finishedRandomDialogs.Contains(id);
	}

	public int GetTalentEnterDoorRecoveryValue()
	{
		if (levelOfEnterDoorRecovery == 0)
		{
			return 0;
		}
		return ScriptableObjMgr.Inst.talentUpgrade2.enterDoorRecovery[levelOfEnterDoorRecovery - 1].value;
	}

	public int GetTalentHPRoomValue()
	{
		if (levelOfHPRoom == 0)
		{
			return 0;
		}
		return ScriptableObjMgr.Inst.talentUpgrade2.hpRoom[levelOfHPRoom - 1].value;
	}

	public int GetTalentCoinRoomValue()
	{
		if (levelOfCoinRoom == 0)
		{
			return 0;
		}
		return ScriptableObjMgr.Inst.talentUpgrade2.coinRoom[levelOfCoinRoom - 1].value;
	}

	public int GetTalentRelicRoomValue()
	{
		if (levelOfRelicRoom == 0)
		{
			return 100;
		}
		return 100 + ScriptableObjMgr.Inst.talentUpgrade2.relicRoom[levelOfRelicRoom - 1].value;
	}

	public int GetTalentSpellRoomValue()
	{
		if (levelOfSpellRoom == 0)
		{
			return 100;
		}
		return 100 + ScriptableObjMgr.Inst.talentUpgrade2.spellRoom[levelOfSpellRoom - 1].value;
	}

	public (ActiveSkillType, bool dirControl) HaveSkillRelic()
	{
		using (List<RelicConfig>.Enumerator enumerator = battleData9.relicCfgs.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				switch (enumerator.Current.abilityType)
				{
				case RelicAbilityType.Hunag:
					return (ActiveSkillType.Huang, false);
				case RelicAbilityType.LightArmor:
					return (ActiveSkillType.LightArmor, true);
				case RelicAbilityType.WarmSnow:
					return (ActiveSkillType.WarmSnow, false);
				case RelicAbilityType.DivingSuit:
					return (ActiveSkillType.DivingSuit, true);
				case RelicAbilityType.RuneWizard:
					return (ActiveSkillType.RuneWizard, true);
				}
			}
		}
		return (ActiveSkillType.None, false);
	}

	public float GetEndlessHighLevelSpellChange()
	{
		if (endless_LevelOfHightLevelSpell == 0)
		{
			return 0f;
		}
		return (float)ScriptableObjMgr.Inst.EndlessTalentUpgrade.hightLevelSpell[endless_LevelOfHightLevelSpell - 1].value / 100f;
	}

	public int GetEndlessFinishCoinCount()
	{
		if (endless_LevelOfFinishCoin == 0)
		{
			return 0;
		}
		return ScriptableObjMgr.Inst.EndlessTalentUpgrade.finishCoin[endless_LevelOfFinishCoin - 1].value;
	}

	public int GetResearchValueConsiderActive(ResearchAbilityType type)
	{
		(bool, bool) tuple = CheckResearchUnlockActive(type);
		if (researchValues.ContainsKey(type) && tuple.Item1 && tuple.Item2)
		{
			return researchValues[type];
		}
		return 0;
	}

	public void AddResearchHoveredID(int id)
	{
		if (!researchHoveredIDs.Contains(id))
		{
			researchHoveredIDs.Add(id);
		}
	}

	public SetConfig GetSelectedSetCfg()
	{
		return SetConfig.dic[selectedSetID];
	}

	public int GetCurrentSetWandID()
	{
		if (setUnlockedSets.ContainsKey(selectedSetID))
		{
			return GetSelectedSetCfg().WandIDs[setUnlockedSets[selectedSetID] - 1];
		}
		Debug.LogWarning("旧存档预处理后存档数据还是有问题？？");
		selectedSetID = 1;
		_ = UICampMgr.Inst != null;
		return GetSelectedSetCfg().WandIDs[0];
	}

	public int GetCurrentSetRelicID()
	{
		if (setUnlockedSets.ContainsKey(selectedSetID))
		{
			return GetSelectedSetCfg().relicID;
		}
		Debug.LogWarning("旧存档预处理后存档数据还是有问题？？");
		selectedSetID = 1;
		return GetSelectedSetCfg().relicID;
	}

	public int GetCurrentSetRelicLevel()
	{
		if (setUnlockedSets.ContainsKey(selectedSetID))
		{
			return setUnlockedSets[selectedSetID];
		}
		Debug.LogWarning("旧存档预处理后存档数据还是有问题？？");
		selectedSetID = 1;
		return setUnlockedSets[selectedSetID];
	}

	public int GetSetUnlockCount()
	{
		int num = 1;
		if (FindSet3)
		{
			num++;
		}
		if (FindSet4)
		{
			num++;
		}
		if (FindSet5)
		{
			num++;
		}
		if (FindSet6)
		{
			num++;
		}
		if (FindSet7)
		{
			num++;
		}
		if (FindSet8)
		{
			num++;
		}
		if (FindSet9)
		{
			num++;
		}
		if (FindSet10)
		{
			num++;
		}
		if (FindSet12)
		{
			num++;
		}
		if (ICJNOGPFMAM.GGPJCCLPBJL)
		{
			num++;
		}
		return num;
	}

	public bool IsSetUnlocked(int id)
	{
		switch (id)
		{
		case 1:
			return true;
		case 2:
			return true;
		case 3:
			return DataMgr.selectedWorldData.FindSet3;
		case 4:
			return DataMgr.selectedWorldData.FindSet4;
		case 5:
			return DataMgr.selectedWorldData.FindSet5;
		case 6:
			return DataMgr.selectedWorldData.FindSet6;
		case 7:
			return DataMgr.selectedWorldData.FindSet7;
		case 8:
			return DataMgr.selectedWorldData.FindSet8;
		case 9:
			return DataMgr.selectedWorldData.FindSet9;
		case 10:
			return DataMgr.selectedWorldData.FindSet10;
		case 12:
			return DataMgr.selectedWorldData.FindSet12;
		case 11:
			return ICJNOGPFMAM.GGPJCCLPBJL;
		default:
			Debug.LogWarning("这是什么套装");
			return false;
		}
	}

	public void SetFindSet3()
	{
		if (!FindSet3)
		{
			FindSet3 = true;
			setUnlockedSets.Add(3, 1);
			GalleryUnlock(GalleryCategory.Relic, 936);
			UIMgr.Inst.ShowFindSetUI(3);
			Debug.Log("find set3");
		}
		else
		{
			Debug.Log("set3 already finded");
		}
	}

	public void SetFindSet4()
	{
		if (!FindSet4)
		{
			FindSet4 = true;
			setUnlockedSets.Add(4, 1);
			UIMgr.Inst.ShowFindSetUI(4);
			Debug.Log("find set4");
		}
		else
		{
			Debug.Log("set4 already finded");
		}
	}

	public void SetFindSet5()
	{
		if (!FindSet5)
		{
			FindSet5 = true;
			setUnlockedSets.Add(5, 1);
			GalleryUnlock(GalleryCategory.Relic, 931);
			UIMgr.Inst.ShowFindSetUI(5);
			Debug.Log("find set5");
		}
		else
		{
			Debug.Log("set5 already finded");
		}
	}

	public void SetFindSet6(int killCount)
	{
		if (!FindSet6)
		{
			if (killCount >= SetConfig.dic[6].unlockInt1)
			{
				FindSet6 = true;
				setUnlockedSets.Add(6, 1);
				GalleryUnlock(GalleryCategory.Relic, 932);
				UIMgr.Inst.ShowFindSetUI(6);
				Debug.Log("find set6");
			}
		}
		else
		{
			Debug.Log("set6 already finded");
		}
	}

	public void SetFindSet7()
	{
		if (!FindSet7)
		{
			FindSet7 = true;
			setUnlockedSets.Add(7, 1);
			GalleryUnlock(GalleryCategory.Relic, 933);
			UIMgr.Inst.ShowFindSetUI(7);
			Debug.Log("find set7");
		}
		else
		{
			Debug.Log("set7 already finded");
		}
	}

	public void SetFindSet8()
	{
		if (!FindSet8)
		{
			FindSet8 = true;
			setUnlockedSets.Add(8, 1);
			GalleryUnlock(GalleryCategory.Relic, 934);
			UIMgr.Inst.ShowFindSetUI(8);
			Debug.Log("find set8");
		}
		else
		{
			Debug.Log("set8 already finded");
		}
	}

	public void SetFindSet9()
	{
		if (!FindSet9 && GetSetUnlockCount() >= SetConfig.dic[9].unlockInt1)
		{
			ForceSetFindSet9();
		}
	}

	public void SetFindSet12()
	{
		if (!FindSet12 && GetSetUnlockCount() >= SetConfig.dic[12].unlockInt1)
		{
			ForceSetFindSe12();
		}
	}

	public void ForceSetFindSet9()
	{
		FindSet9 = true;
		setUnlockedSets.Add(9, 1);
		GalleryUnlock(GalleryCategory.Relic, 935);
		UIMgr.Inst.ShowFindSetUI(9);
		Debug.Log("find set9");
	}

	public void ForceSetFindSe12()
	{
		FindSet12 = true;
		setUnlockedSets.Add(12, 1);
		GalleryUnlock(GalleryCategory.Relic, 939);
		UIMgr.Inst.ShowFindSetUI(12);
		Debug.Log("find set12");
	}

	public void SetFindSet10()
	{
		if (!FindSet10)
		{
			int num = 0;
			{
				foreach (KeyValuePair<int, int> galleryPotionUseTime in galleryPotionUseTimes)
				{
					num += galleryPotionUseTime.Value;
					if (num >= 200)
					{
						FindSet10 = true;
						setUnlockedSets.Add(10, 1);
						GalleryUnlock(GalleryCategory.Relic, 937);
						UIMgr.Inst.ShowFindSetUI(10);
						Debug.Log("find set10");
						break;
					}
				}
				return;
			}
		}
		Debug.Log("set10 already finded");
	}

	public void SetFindSet120()
	{
		if (!FindSet12)
		{
			FindSet12 = true;
			setUnlockedSets.Add(12, 1);
			GalleryUnlock(GalleryCategory.Relic, 939);
			UIMgr.Inst.ShowFindSetUI(12);
			Debug.Log("find set12");
		}
		else
		{
			Debug.Log("set12 already finded");
		}
	}

	public void GalleryUnlock(GalleryCategory category, int id)
	{
		bool flag = false;
		switch (category)
		{
		case GalleryCategory.Monster:
			if (!galleryUnlockedMonsters.Contains(id))
			{
				flag = true;
				galleryUnlockedMonsters.Add(id);
			}
			break;
		case GalleryCategory.Boss:
			if (!galleryUnlockedBosses.Contains(id))
			{
				if (id == 509901)
				{
					UIMgr.Inst.UIMenu.uiGallery.needReload = true;
				}
				galleryUnlockedBosses.Add(id);
				flag = true;
			}
			break;
		case GalleryCategory.Wand:
			if (!galleryUnlockedWands.Contains(id))
			{
				flag = true;
				galleryUnlockedWands.Add(id);
			}
			break;
		case GalleryCategory.Spell:
			if (!galleryUnlockedSpells.Contains(id))
			{
				flag = true;
				galleryUnlockedSpells.Add(id);
			}
			break;
		case GalleryCategory.Relic:
			if (!galleryUnlockedRelics.Contains(id))
			{
				flag = true;
				galleryUnlockedRelics.Add(id);
			}
			break;
		case GalleryCategory.Potion:
			if (!galleryUnlockedPotions.Contains(id))
			{
				flag = true;
				galleryUnlockedPotions.Add(id);
			}
			galleryUnlockedPotions.Add(id);
			break;
		case GalleryCategory.Curse:
			if (!galleryUnlockedCurses.Contains(id))
			{
				flag = true;
				galleryUnlockedCurses.Add(id);
			}
			break;
		default:
			Debug.LogError(category);
			break;
		}
		if (flag && (bool)UIMgr.Inst.UIMenu.uiGallery && UIMgr.Inst.UIMenu.uiGallery.init)
		{
			UIMgr.Inst.UIMenu.uiGallery.slotInited[(int)category] = false;
			DataMgr.selectedWorldData.MenuGalleryDot = true;
		}
	}

	public void GalleryUnitsDead(TakeDamageInfo_Dots info, UnitProperty_Dots ppt_Dots)
	{
		if (!ppt_Dots.unitCfg.inGallery)
		{
			return;
		}
		UnitType unitType = ppt_Dots.unitCfg.unitType;
		if ((uint)(unitType - 4) <= 1u)
		{
			if (galleryKilledBossCounts.ContainsKey(ppt_Dots.unitCfg.id))
			{
				galleryKilledBossCounts[ppt_Dots.unitCfg.id]++;
			}
			else
			{
				galleryKilledBossCounts.Add(ppt_Dots.unitCfg.id, 1);
			}
			if (!FindSet6 && info.spell.Entity != Entity.Null && info.spell.Config.AbilityType == SpellAbilityType.MagicBreaker)
			{
				set6KillCounter++;
				SetFindSet6(set6KillCounter);
			}
		}
		else
		{
			if (galleryKilledMonsterCounts.ContainsKey(ppt_Dots.unitCfg.id))
			{
				galleryKilledMonsterCounts[ppt_Dots.unitCfg.id]++;
			}
			else
			{
				galleryKilledMonsterCounts.Add(ppt_Dots.unitCfg.id, 1);
			}
			if (!FindSet6 && info.spell.Entity != Entity.Null && info.spell.Config.AbilityType == SpellAbilityType.MagicBreaker)
			{
				set6KillCounter++;
				SetFindSet6(set6KillCounter);
			}
		}
	}

	public void GalleryRelicGet(int id)
	{
		if (galleryRelicGetTimes.ContainsKey(id))
		{
			galleryRelicGetTimes[id]++;
		}
		else
		{
			galleryRelicGetTimes.Add(id, 1);
		}
	}

	public void GalleryPotionUse(int id)
	{
		if (galleryPotionUseTimes.ContainsKey(id))
		{
			galleryPotionUseTimes[id]++;
		}
		else
		{
			galleryPotionUseTimes.Add(id, 1);
		}
	}

	public void GalleryCurseGet(int id)
	{
		if (galleryCurseGetTimes.ContainsKey(id))
		{
			galleryCurseGetTimes[id]++;
		}
		else
		{
			galleryCurseGetTimes.Add(id, 1);
		}
	}

	public bool ActivateGirlHave3Pick2()
	{
		return activeGirlHave3Pick2;
	}

	public bool ActivateGirlHaveSpellLock()
	{
		return activaGrilHaveSpellLock;
	}

	public bool ActivateGirlHave4Pick2()
	{
		return activeGirlHave4Pick2;
	}

	public int ActivateGirl_ExtraFreeDisableCount()
	{
		int num = 0;
		for (int i = 1; i < ScriptableObjMgr.Inst.activateGirlLayerNeed.ints.Length; i++)
		{
			if (activateGirlActivatedIDs2.Count >= ScriptableObjMgr.Inst.activateGirlLayerNeed.ints[i])
			{
				num += 3;
			}
		}
		return num;
	}

	public int ActivateGirl_ExtraMaxDisableCount()
	{
		int num = 0;
		for (int i = 1; i < ScriptableObjMgr.Inst.activateGirlLayerNeed.ints.Length; i++)
		{
			if (activateGirlActivatedIDs2.Count >= ScriptableObjMgr.Inst.activateGirlLayerNeed.ints[i])
			{
				num += 8;
			}
		}
		return num;
	}

	public void PlayTimeRecord()
	{
		lastRecordPlayTime = Time.unscaledTime;
	}

	public void PlayTimeSettle()
	{
		playTime += Time.unscaledTime - lastRecordPlayTime;
		lastRecordPlayTime = Time.unscaledTime;
	}

	public int GetTalentSpentCrystal()
	{
		int num = 0;
		for (int i = 0; i < levelOfWandLimit; i++)
		{
			num += ScriptableObjMgr.Inst.talentUpgrade2.wandLimit[i].cost;
		}
		for (int j = 0; j < levelOfBagLimit; j++)
		{
			num += ScriptableObjMgr.Inst.talentUpgrade2.bagLimit[j].cost;
		}
		for (int k = 0; k < levelOfEnterDoorRecovery; k++)
		{
			num += ScriptableObjMgr.Inst.talentUpgrade2.enterDoorRecovery[k].cost;
		}
		for (int l = 0; l < levelOfMaxHP; l++)
		{
			num += ScriptableObjMgr.Inst.talentUpgrade2.maxHP[l].cost;
		}
		for (int m = 0; m < levelOfHPRoom; m++)
		{
			num += ScriptableObjMgr.Inst.talentUpgrade2.hpRoom[m].cost;
		}
		for (int n = 0; n < levelOfInitialCoin; n++)
		{
			num += ScriptableObjMgr.Inst.talentUpgrade2.initialCoin[n].cost;
		}
		for (int num2 = 0; num2 < levelOfCoinRoom; num2++)
		{
			num += ScriptableObjMgr.Inst.talentUpgrade2.coinRoom[num2].cost;
		}
		for (int num3 = 0; num3 < levelOfSpellRoom; num3++)
		{
			num += ScriptableObjMgr.Inst.talentUpgrade2.spellRoom[num3].cost;
		}
		for (int num4 = 0; num4 < levelOfRelicRoom; num4++)
		{
			num += ScriptableObjMgr.Inst.talentUpgrade2.relicRoom[num4].cost;
		}
		for (int num5 = 0; num5 < levelOfMaxMP; num5++)
		{
			num += ScriptableObjMgr.Inst.talentUpgrade2.maxMP[num5].cost;
		}
		for (int num6 = 0; num6 < levelOfMPRecover; num6++)
		{
			num += ScriptableObjMgr.Inst.talentUpgrade2.mpRecover[num6].cost;
		}
		return num;
	}

	public int GetHistoryGetCrystal()
	{
		return GetTalentSpentCrystal() + spellDisableCost_Crystal2 + magicCrystalCount;
	}

	public int GetHistoryGetBlood()
	{
		int num = ancientBloodCount;
		if (isTalentUnlock1)
		{
			num += ScriptableObjMgr.Inst.talentUpgrade2.unlock1Cost;
		}
		if (isTalentUnlock2)
		{
			num += ScriptableObjMgr.Inst.talentUpgrade2.unlock2Cost;
		}
		if (isTalentUnlock3)
		{
			num += ScriptableObjMgr.Inst.talentUpgrade2.unlock3Cost;
		}
		if (isTalentUnlock4)
		{
			num += ScriptableObjMgr.Inst.talentUpgrade2.unlock4Cost;
		}
		for (int i = 0; i < researchedIDs.Count; i++)
		{
			if (ResearchConfig.dic.ContainsKey(researchedIDs[i]))
			{
				num += ResearchConfig.dic[researchedIDs[i]].cost;
			}
		}
		foreach (KeyValuePair<int, int> setUnlockedSet in setUnlockedSets)
		{
			if (SetConfig.dic.ContainsKey(setUnlockedSet.Key) && setUnlockedSet.Value > 1)
			{
				for (int j = 1; j < setUnlockedSet.Value; j++)
				{
					num += SetConfig.dic[setUnlockedSet.Key].upgradeCosts[j - 1];
				}
			}
		}
		return num + spellDisableCost_Blood2;
	}

	public int GetHistoryGetCore()
	{
		int num = chaosCoreCount;
		for (int i = 0; i < activateGirlActivatedIDs2.Count; i++)
		{
			if (ActivateGirlConfig.dic.ContainsKey(activateGirlActivatedIDs2[i]))
			{
				num += ActivateGirlConfig.dic[activateGirlActivatedIDs2[i]].cost;
			}
			else
			{
				Debug.LogWarning("版本原因计算核心数量出错");
			}
		}
		return num;
	}

	public void CalculateAddingPoints()
	{
		foreach (int researchedID in researchedIDs)
		{
			if (researchValues.ContainsKey(ResearchConfig.dic[researchedID].abilityType))
			{
				if (researchValues[ResearchConfig.dic[researchedID].abilityType] < ResearchConfig.dic[researchedID].int1)
				{
					researchValues[ResearchConfig.dic[researchedID].abilityType] = ResearchConfig.dic[researchedID].int1;
				}
			}
			else
			{
				researchValues.Add(ResearchConfig.dic[researchedID].abilityType, ResearchConfig.dic[researchedID].int1);
			}
		}
		activeGirlHave3Pick2 = activateGirlActivatedIDs2.Contains(101);
		activaGrilHaveSpellLock = activateGirlActivatedIDs2.Contains(103);
		activeGirlHave4Pick2 = activateGirlActivatedIDs2.Contains(104);
	}

	public (bool unlock, bool isActive) CheckResearchUnlockActive(ResearchAbilityType type)
	{
		bool flag = false;
		for (int i = 0; i < researchedIDs.Count; i++)
		{
			if (ResearchConfig.dic[researchedIDs[i]].abilityType != type)
			{
				continue;
			}
			flag = true;
			if (!ResearchConfig.dic[researchedIDs[i]].canDisactive)
			{
				return (flag, true);
			}
			for (int j = 0; j < researchDisactive.Count; j++)
			{
				if (ResearchConfig.dic[researchDisactive[j]].abilityType == type)
				{
					return (flag, false);
				}
			}
			return (flag, true);
		}
		return (false, false);
	}

	public void BackCampCheckPlot()
	{
		if (GameMgr.IsMobile_Static)
		{
			bool haveGame = ICJNOGPFMAM.MIFJADDOODN;
			bool haveAllCampSkin = ICJNOGPFMAM.IMFNIOLONJP;
			UIPlayerDataMgr.Inst.buySuitBtn.SetActive(haveGame && !haveAllCampSkin);
		}
		if (storyKillChapter3BossPickup && !storyNormalFinishBackCamp)
		{
			if (!finishedDifficulty.Contains(DifficultyType.Easy))
			{
				finishedDifficulty.Add(DifficultyType.Easy);
			}
			story1Finish = true;
			story2Open = true;
			story2Finish = true;
			npc1VivianImportantPlot.SetNewState(41);
			npc2NimueImportantPlot.SetNewState(42);
			npc3ImportantPlot.SetNewState(43);
			return;
		}
		if (storyHardBossDropPickup && !storyHardFinishBackCamp)
		{
			if (!finishedDifficulty.Contains(DifficultyType.Normal))
			{
				finishedDifficulty.Add(DifficultyType.Normal);
			}
			npc1VivianImportantPlot.SetNewState(54);
			npc2NimueImportantPlot.SetNewState(55);
			npc3ImportantPlot.SetNewState(56);
			npc6ImportantPlot.SetNewState(58);
			npc7ImportantPlot.SetNewState(59);
			if (story3Finish && npc4SchedulePlot.hdID < 1402)
			{
				npc4SchedulePlot.SetNewState(1402);
				canSetUpgrade = true;
			}
			return;
		}
		if (storyFinishHardDropPickup && !storyFinishHardBackCamp)
		{
			if (!finishedDifficulty.Contains(DifficultyType.Hard))
			{
				DataMgr.selectedWorldData.finishedDifficulty.Add(DifficultyType.Hard);
			}
			npc1VivianImportantPlot.SetNewState(69);
			npc2NimueImportantPlot.SetNewState(70);
			npc3ImportantPlot.SetNewState(71);
			npc4ImportantPlot.SetNewState(72);
			npc5ImportantPlot.SetNewState(73);
			npc6ImportantPlot.SetNewState(74);
			npc7ImportantPlot.SetNewState(75);
			return;
		}
		if (storyFinishNightmare1 && !storyFinishNightmare1BackCamp)
		{
			if (!finishedDifficulty.Contains(DifficultyType.Nightmare1))
			{
				finishedDifficulty.Add(DifficultyType.Nightmare1);
			}
			npc1VivianImportantPlot.SetNewState(76);
			return;
		}
		if (storyFinishNightmare2 && !storyFinishNightmare2BackCamp)
		{
			if (!finishedDifficulty.Contains(DifficultyType.Nightmare2))
			{
				finishedDifficulty.Add(DifficultyType.Nightmare2);
			}
			npc2NimueImportantPlot.SetNewState(79);
			return;
		}
		if (storyFinishNightmare3 && !storyFinishNightmare3BackCamp)
		{
			if (!finishedDifficulty.Contains(DifficultyType.Nightmare3))
			{
				finishedDifficulty.Add(DifficultyType.Nightmare3);
			}
			npc1VivianImportantPlot.SetNewState(81);
			npc3ImportantPlot.SetNewState(83);
			npc6ImportantPlot.SetNewState(82);
			return;
		}
		if (LevelMgr.Inst.CurrentRoomCfg.id == 1005)
		{
			if (LevelMgr.Inst.CurrentRoomCtrller.IsFinish)
			{
				if (haveSeeLishujian)
				{
					npc1VivianImportantPlot.SetNewState(204);
				}
				else
				{
					haveSeeLishujian = true;
					npc1VivianImportantPlot.SetNewState(202);
				}
			}
			else if (haveSeeLishujian)
			{
				npc1VivianImportantPlot.SetNewState(203);
			}
			else
			{
				haveSeeLishujian = true;
				npc1VivianImportantPlot.SetNewState(201);
			}
		}
		if (!story1Finish)
		{
			return;
		}
		if ((battleData9.currentStage > 1 || (battleData9.currentStage == 1 && battleData9.currentLevel >= 3)) && !story2Open)
		{
			story2Open = true;
			npc1VivianImportantPlot.SetNewState(12);
			npc3ImportantPlot.SetNewState(14);
			return;
		}
		if (story3NPC4Rescued && !story3Finish)
		{
			npc4ImportantPlot.SetNewState(20);
		}
		if (story4NPC5Rescued && !story4Finish)
		{
			npc5ImportantPlot.SetNewState(27);
		}
		if (battleData9.currentStage >= 9)
		{
			_ = npc1VivianSchedulePlot.hdID;
			_ = 1104;
		}
		if (isReachChatper4)
		{
			_ = npc1VivianSchedulePlot.hdID;
			_ = 1103;
			if (story3Finish && npc4SchedulePlot.hdID < 1402)
			{
				npc4SchedulePlot.SetNewState(1402);
				canSetUpgrade = true;
			}
		}
		if (isReachChatper3 && npc1VivianSchedulePlot.hdID < 1102)
		{
			npc1VivianSchedulePlot.SetNewState(1102);
			npc2NimueSchedulePlot.SetNewState(1202);
			npc3SchedulePlot.SetNewState(1302);
			return;
		}
		if (isReachChatper2 && npc1VivianSchedulePlot.hdID < 1101)
		{
			npc1VivianSchedulePlot.SetNewState(1101);
			npc2NimueSchedulePlot.SetNewState(1201);
			npc3SchedulePlot.SetNewState(1301);
			return;
		}
		if (!story4NPC5Rescued && enterBattleTime >= 6 && story3NPC4Rescued && story3Finish)
		{
			story4NPC5Rescued = true;
			story4NPC5ForceShow = true;
			npc5ImportantPlot.SetNewState(28);
		}
		if (battleData9.currentStage > 1 || (battleData9.currentStage == 1 && battleData9.currentLevel >= BattleMgr.Inst.stageLevelsCount[0]))
		{
			if (npc1VivianCasualPlot.hdID == 0)
			{
				npc1VivianCasualPlot.SetNewState(2101);
			}
			if (npc2NimueCasualPlot.hdID == 0)
			{
				npc2NimueCasualPlot.SetNewState(2201);
			}
		}
		if ((battleData9.currentStage > 1 || battleData9.currentLevel == BattleMgr.Inst.stageLevelsCount[0]) && isPlayerDeadBackCamp && UnityEngine.Random.Range(0f, 1f) > 0.5f)
		{
			TryActiveDeadRandomDialog();
		}
	}

	public bool TryActiveDeadRandomDialog()
	{
		Dialog[] array = DeadRandomDialogs.Where((Dialog e) => !RandomDialogueIsFinished(e.id)).ToArray();
		if (array.Length != 0)
		{
			return TryActiveRandomDialog(array);
		}
		return false;
	}

	public bool TryActiveReEnterGameRandomDialog()
	{
		Dialog[] array = ReEnterCampDialogs.Where((Dialog e) => !RandomDialogueIsFinished(e.id)).ToArray();
		if (array.Length != 0)
		{
			return TryActiveRandomDialog(array);
		}
		return false;
	}

	private bool TryActiveRandomDialog(Dialog[] dialogs)
	{
		for (int i = 0; i < 10; i++)
		{
			Dialog dialog = dialogs[UnityEngine.Random.Range(0, dialogs.Length)];
			if (CheckNPCPlotEnable(dialog.npc) && (dialog.Condition == null || dialog.Condition()) && (dialog.npc.isInteract || dialog.npc.hdID == 0))
			{
				dialog.npc.SetNewState(dialog.id);
				DataMgr.SaveWorldData(DataMgr.selectedWorldData);
				return true;
			}
		}
		return false;
	}

	public bool CheckNPCPlotEnable(NPCPlot plot)
	{
		if (new NPCPlot[4] { npc1VivianImportantPlot, npc1VivianSchedulePlot, npc1VivianCasualPlot, npc1VivianRandomPlotV2 }.Contains(plot))
		{
			return true;
		}
		if (new NPCPlot[4] { npc2NimueImportantPlot, npc2NimueSchedulePlot, npc2NimueCasualPlot, npc2NimueRandomPlotV2 }.Contains(plot))
		{
			return true;
		}
		if (new NPCPlot[4] { npc3ImportantPlot, npc3SchedulePlot, npc3CasualPlot, npc3RandomPlotV2 }.Contains(plot))
		{
			return story2Open;
		}
		if (new NPCPlot[4] { npc4ImportantPlot, npc4SchedulePlot, npc4CasualPlot, npc4RandomPlotV2 }.Contains(plot))
		{
			return story3NPC4Rescued;
		}
		if (new NPCPlot[4] { npc5ImportantPlot, npc5SchedulePlot, npc5CasualPlot, npc5RandomPlotV2 }.Contains(plot))
		{
			return story4NPC5Rescued;
		}
		if (new NPCPlot[4] { npc6ImportantPlot, npc6SchedulePlot, npc6CasualPlot, npc6RandomPlotV2 }.Contains(plot))
		{
			return storyKillChapter3BossPickup;
		}
		if (new NPCPlot[4] { npc7ImportantPlot, npc7SchedulePlot, npc7CasualPlot, npc7RandomPlotV2 }.Contains(plot))
		{
			return storyHardFinishNPC7OpenFunction;
		}
		return false;
	}

	public void CorrectData()
	{
		if (levelOfWandLimit > ScriptableObjMgr.Inst.talentUpgrade2.wandLimit.Length)
		{
			levelOfWandLimit = ScriptableObjMgr.Inst.talentUpgrade2.wandLimit.Length;
			Debug.LogWarning("天赋长度超过最大长度");
		}
		if (levelOfBagLimit > ScriptableObjMgr.Inst.talentUpgrade2.bagLimit.Length)
		{
			levelOfBagLimit = ScriptableObjMgr.Inst.talentUpgrade2.bagLimit.Length;
			Debug.LogWarning("天赋长度超过最大长度");
		}
		if (levelOfEnterDoorRecovery > ScriptableObjMgr.Inst.talentUpgrade2.enterDoorRecovery.Length)
		{
			levelOfEnterDoorRecovery = ScriptableObjMgr.Inst.talentUpgrade2.enterDoorRecovery.Length;
			Debug.LogWarning("天赋长度超过最大长度");
		}
		if (levelOfMaxHP > ScriptableObjMgr.Inst.talentUpgrade2.maxHP.Length)
		{
			levelOfMaxHP = ScriptableObjMgr.Inst.talentUpgrade2.maxHP.Length;
			Debug.LogWarning("天赋长度超过最大长度");
		}
		if (levelOfHPRoom > ScriptableObjMgr.Inst.talentUpgrade2.hpRoom.Length)
		{
			levelOfHPRoom = ScriptableObjMgr.Inst.talentUpgrade2.hpRoom.Length;
			Debug.LogWarning("天赋长度超过最大长度");
		}
		if (levelOfInitialCoin > ScriptableObjMgr.Inst.talentUpgrade2.initialCoin.Length)
		{
			levelOfInitialCoin = ScriptableObjMgr.Inst.talentUpgrade2.initialCoin.Length;
			Debug.LogWarning("天赋长度超过最大长度");
		}
		if (levelOfCoinRoom > ScriptableObjMgr.Inst.talentUpgrade2.coinRoom.Length)
		{
			levelOfCoinRoom = ScriptableObjMgr.Inst.talentUpgrade2.coinRoom.Length;
			Debug.LogWarning("天赋长度超过最大长度");
		}
		if (levelOfSpellRoom > ScriptableObjMgr.Inst.talentUpgrade2.spellRoom.Length)
		{
			levelOfSpellRoom = ScriptableObjMgr.Inst.talentUpgrade2.spellRoom.Length;
			Debug.LogWarning("天赋长度超过最大长度");
		}
		if (levelOfRelicRoom > ScriptableObjMgr.Inst.talentUpgrade2.relicRoom.Length)
		{
			levelOfRelicRoom = ScriptableObjMgr.Inst.talentUpgrade2.relicRoom.Length;
			Debug.LogWarning("天赋长度超过最大长度");
		}
		if (levelOfMaxMP > ScriptableObjMgr.Inst.talentUpgrade2.maxMP.Length)
		{
			levelOfMaxMP = ScriptableObjMgr.Inst.talentUpgrade2.maxMP.Length;
			Debug.LogWarning("天赋长度超过最大长度");
		}
		if (levelOfMPRecover > ScriptableObjMgr.Inst.talentUpgrade2.mpRecover.Length)
		{
			levelOfMPRecover = ScriptableObjMgr.Inst.talentUpgrade2.mpRecover.Length;
			Debug.LogWarning("天赋长度超过最大长度");
		}
		if (levelOfWandLimit_2 > 0)
		{
			levelOfWandLimit = levelOfWandLimit_2;
			levelOfWandLimit_2 = 0;
		}
		if (levelOfBagLimit_2 > 0)
		{
			levelOfBagLimit = levelOfBagLimit_2;
			levelOfBagLimit_2 = 0;
		}
		if (levelOfPotionLimit_2 > 0)
		{
			levelOfEnterDoorRecovery = levelOfPotionLimit_2;
			levelOfPotionLimit_2 = 0;
		}
		if (levelOfMaxHP_2 > 0)
		{
			levelOfMaxHP = levelOfMaxHP_2;
			levelOfMaxHP_2 = 0;
		}
		if (levelOfHPRoom_2 > 0)
		{
			levelOfHPRoom = levelOfHPRoom_2;
			levelOfHPRoom_2 = 0;
		}
		if (levelOfInitialCoin_2 > 0)
		{
			levelOfInitialCoin = levelOfInitialCoin_2;
			levelOfInitialCoin_2 = 0;
		}
		if (levelOfCoinRoom_2 > 0)
		{
			levelOfCoinRoom = levelOfCoinRoom_2;
			levelOfCoinRoom_2 = 0;
		}
		if (levelOfSpellRoom_2 > 0)
		{
			levelOfSpellRoom = levelOfSpellRoom_2;
			levelOfSpellRoom_2 = 0;
		}
		if (levelOfRelicRoom_2 > 0)
		{
			levelOfRelicRoom = levelOfRelicRoom_2;
			levelOfRelicRoom_2 = 0;
		}
		if (levelOfMaxMP_2 > 0)
		{
			levelOfMaxMP = levelOfMaxMP_2;
			levelOfMaxMP_2 = 0;
		}
		if (levelOfMPRecover_2 > 0)
		{
			levelOfMPRecover = levelOfMPRecover_2;
			levelOfMPRecover_2 = 0;
		}
		if (isTalentUnlock1_2)
		{
			isTalentUnlock1 = isTalentUnlock1_2;
			isTalentUnlock1_2 = false;
		}
		if (isTalentUnlock2_2)
		{
			isTalentUnlock2 = isTalentUnlock2_2;
			isTalentUnlock2_2 = false;
		}
		if (isTalentUnlock3_2)
		{
			isTalentUnlock3 = isTalentUnlock3_2;
			isTalentUnlock3_2 = false;
		}
		if (isTalentUnlock4_2)
		{
			isTalentUnlock4 = isTalentUnlock4_2;
			isTalentUnlock4_2 = false;
		}
		if (!setUnlockedSets.ContainsKey(selectedSetID))
		{
			Debug.LogWarning("选中的套装ID不存在，选中ID " + selectedSetID);
			selectedSetID = 1;
		}
		if (campSkinType < CampSkinType.Default)
		{
			campSkinType = CampSkinType.Default;
		}
		if (ICJNOGPFMAM.GGPJCCLPBJL && !setUnlockedSets.ContainsKey(11))
		{
			setUnlockedSets.TryAdd(11, 1);
		}
		else if (!ICJNOGPFMAM.GGPJCCLPBJL && selectedSetID == 11)
		{
			selectedSetID = 1;
		}
		if (daveMirrorTalk && !galleryUnlockedRelics.Contains(938))
		{
			galleryUnlockedRelics.Add(938);
		}
		if (!isCoverTo0_80Version)
		{
			isCoverTo0_80Version = true;
			if (isTalentUnlock3)
			{
				isTalentUnlock4 = true;
			}
		}
		if (!isCoverTo0_82Version)
		{
			isCoverTo0_82Version = true;
			if (finishedDifficulty.Contains(DifficultyType.Normal))
			{
				canSetUpgrade = true;
				npc4SchedulePlot.SetNewState(1402);
				npc4SchedulePlot.isInteract = true;
			}
		}
		if (!isCoverTo1_0Version)
		{
			isCoverTo1_0Version = true;
			if (finishedDifficulty.Contains(DifficultyType.Easy))
			{
				isReachChatper2 = true;
				isReachChatper3 = true;
			}
			if (finishedDifficulty.Contains(DifficultyType.Normal))
			{
				isReachChatper4 = true;
			}
			if (researchedIDs.Contains(121) && !researchHoveredIDs.Contains(121))
			{
				researchHoveredIDs.Add(121);
			}
		}
		if (!isCoverTo1_0_29Version)
		{
			isCoverTo1_0_29Version = true;
			if (playTime >= 7200f)
			{
				oldPlayerGift = true;
			}
		}
		if (!isCoverTo1_2_19Version)
		{
			isCoverTo1_2_19Version = true;
			if (setUnlockedSets.ContainsKey(3))
			{
				int num = setUnlockedSets[3];
				switch (num)
				{
				case 5:
					ancientBloodCount += 350;
					break;
				case 4:
					ancientBloodCount += 50;
					break;
				}
				if (num > 3)
				{
					Debug.Log($"召唤师套装等级修正{num}到3");
					setUnlockedSets[3] = 3;
				}
			}
		}
		if (FindSet3 && !galleryUnlockedRelics.Contains(936))
		{
			galleryUnlockedRelics.Add(936);
		}
		if (FindSet5 && !galleryUnlockedRelics.Contains(931))
		{
			galleryUnlockedRelics.Add(931);
		}
		if (FindSet10 && !galleryUnlockedRelics.Contains(937))
		{
			galleryUnlockedRelics.Add(937);
		}
		if (FindSet12 && !galleryUnlockedRelics.Contains(939))
		{
			galleryUnlockedRelics.Add(939);
		}
		if (storyKillChapter3BossBackCamp)
		{
			storyKillChapter3BossBackCamp = false;
			if (!activateGirlActivatedIDs2.Contains(1))
			{
				activateGirlActivatedIDs2.Add(1);
			}
			if (!activateGirlActivatedIDs2.Contains(2))
			{
				activateGirlActivatedIDs2.Add(2);
			}
		}
		if (setUnlockedSets.ContainsKey(8) && setUnlockedSets[8] == 3)
		{
			setUnlockedSets[8] = 2;
		}
		if (setUnlockedSets.ContainsKey(9) && setUnlockedSets[9] == 3)
		{
			setUnlockedSets[9] = 2;
		}
		if (!isCoverTo1_0_32Version)
		{
			isCoverTo1_0_32Version = true;
			if (researchedIDs.Contains(141))
			{
				researchedIDs.Remove(141);
			}
			if (researchHoveredIDs.Contains(141))
			{
				researchHoveredIDs.Remove(141);
			}
		}
		if (FindSet3 && !setUnlockedSets.ContainsKey(3))
		{
			setUnlockedSets.Add(3, 1);
		}
		if (FindSet4 && !setUnlockedSets.ContainsKey(4))
		{
			setUnlockedSets.Add(4, 1);
		}
		if (FindSet5 && !setUnlockedSets.ContainsKey(5))
		{
			setUnlockedSets.Add(5, 1);
		}
		if (finishedDifficulty.Contains(DifficultyType.Easy))
		{
			storyKillChapter3BossPickup = true;
		}
		if (finishedDifficulty.Contains(DifficultyType.Normal))
		{
			storyHardBossDropPickup = true;
		}
		if (finishedDifficulty.Contains(DifficultyType.Hard))
		{
			storyFinishHardDropPickup = true;
		}
	}

	public void FinishUnfinishedButRescuedNpcStory()
	{
		if (GameMgr.IsMobile_Static)
		{
			if (DataMgr.selectedWorldData.story2Open && !DataMgr.selectedWorldData.story2Finish)
			{
				DataMgr.selectedWorldData.story2Finish = true;
			}
			if (DataMgr.selectedWorldData.story3NPC4Rescued && !DataMgr.selectedWorldData.story3Finish)
			{
				DataMgr.selectedWorldData.story3Finish = true;
			}
			if (DataMgr.selectedWorldData.story4NPC5Rescued && !DataMgr.selectedWorldData.story4Finish)
			{
				DataMgr.selectedWorldData.story4Finish = true;
			}
		}
	}
}
