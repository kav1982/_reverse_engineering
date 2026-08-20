using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using PlayerLogger;
using PlayerLogger.Events;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleMgr : MonoBehaviour
{
	[Serializable]
	public class overrideElite
	{
		public int stage;

		public int level;
	}

	public GameObject objToDestroy;

	public int[] stageLevelsCount;

	public List<overrideElite> overrideElitesLevelShow;

	[Range(0f, 1f)]
	public float[] rroChances;

	public NPC7 npc7;

	public Vector3 guideFlagPoint;

	[Header("Guide")]
	public GameObject go_Guide;

	public GameObject go_GuideKeyboard;

	public GameObject go_GuideKeyboardSwtichWand;

	public GameObject go_GuideKeyboardSwtichWandText;

	public GameObject go_GuideMobile;

	public GameObject go_GuideGamepad;

	public GameObject go_GuideGamepadSwtichWandText;

	public List<GameObject> GoGuideGamepadMains;

	public List<GameObject> GoGuideGamepadSwitchWands;

	public static bool EnterEndlessBattle;

	private bool isInitialized;

	private bool battleStart;

	private EntityManager ettMgr;

	public static BattleMgr Inst { get; private set; }

	public int CurrentStage
	{
		get
		{
			return DataMgr.selectedWorldData.battleData9.currentStage;
		}
		set
		{
			DataMgr.selectedWorldData.battleData9.currentStage = value;
		}
	}

	public int CurrentLevel
	{
		get
		{
			return DataMgr.selectedWorldData.battleData9.currentLevel;
		}
		set
		{
			DataMgr.selectedWorldData.battleData9.currentLevel = value;
		}
	}

	public int EndlessCurrentStage => Mathf.Max(0, Inst.CurrentLevel - 1 - 1) / 5 + 1;

	public int EndlessCurrentGear { get; set; }

	public int EndlessCurrentLevel => Inst.CurrentLevel;

	public int MaxEndlessLevel { get; set; }

	public LevelRewardType CurrentRewardType
	{
		get
		{
			return DataMgr.selectedWorldData.battleData9.currentRewardType;
		}
		set
		{
			DataMgr.selectedWorldData.battleData9.currentRewardType = value;
		}
	}

	public List<LevelRewardType> NextRewardTypes
	{
		get
		{
			return DataMgr.selectedWorldData.battleData9.nextRewardTypes;
		}
		set
		{
			DataMgr.selectedWorldData.battleData9.nextRewardTypes = value;
		}
	}

	public LevelRewardType NextExtraDoorRewardType
	{
		get
		{
			return DataMgr.selectedWorldData.battleData9.nextExtraDoorRewardType;
		}
		set
		{
			DataMgr.selectedWorldData.battleData9.nextExtraDoorRewardType = value;
		}
	}

	private void Awake()
	{
		Inst = this;
		UnityEngine.Object.Destroy(objToDestroy);
		ettMgr = World.DefaultGameObjectInjectionWorld.EntityManager;
		Debug.Log("Enter Battle");
	}

	private void OnEnable()
	{
		EventMgr.InputChange = (Action)Delegate.Combine(EventMgr.InputChange, new Action(InputChange));
	}

	private void OnDisable()
	{
		EventMgr.InputChange = (Action)Delegate.Remove(EventMgr.InputChange, new Action(InputChange));
	}

	private void InputChange()
	{
		if (!(go_Guide != null))
		{
			return;
		}
		go_GuideMobile.SetActive(value: false);
		go_GuideKeyboard.SetActive(value: false);
		go_GuideGamepad.SetActive(value: false);
		if (GameMgr.IsMobile_Static)
		{
			go_GuideMobile.SetActive(value: false);
			go_GuideGamepad.SetActive(MobileMgr.inst.gamepadPlugged);
			return;
		}
		if (GameMgr.IsSteamDeck_Static || UIMgr.Inst.InputType == PlayerInputType.Gamepad)
		{
			go_GuideGamepad.SetActive(value: true);
			foreach (GameObject goGuideGamepadMain in GoGuideGamepadMains)
			{
				goGuideGamepadMain.SetActive(value: false);
			}
			foreach (GameObject goGuideGamepadSwitchWand in GoGuideGamepadSwitchWands)
			{
				goGuideGamepadSwitchWand.SetActive(value: false);
			}
			GoGuideGamepadMains[(int)ControlMgr.Inst.GetControllerType()].SetActive(value: true);
			if (PlayerMgr.Inst.BaData != null)
			{
				if (PlayerMgr.Inst.BaData.wandMaxCount > 1)
				{
					GoGuideGamepadSwitchWands[(int)ControlMgr.Inst.GetControllerType()].SetActive(value: true);
					go_GuideGamepadSwtichWandText.SetActive(value: true);
				}
				else
				{
					go_GuideGamepadSwtichWandText.SetActive(value: false);
				}
			}
			return;
		}
		go_GuideKeyboard.SetActive(value: true);
		if (PlayerMgr.Inst.BaData != null)
		{
			if (PlayerMgr.Inst.BaData.wandMaxCount > 1)
			{
				go_GuideKeyboardSwtichWand.SetActive(value: true);
				go_GuideKeyboardSwtichWandText.SetActive(value: true);
			}
			else
			{
				go_GuideKeyboardSwtichWand.SetActive(value: false);
				go_GuideKeyboardSwtichWandText.SetActive(value: false);
			}
		}
	}

	private void Update()
	{
		ettMgr.CheckInitialize(ref isInitialized);
		if (isInitialized && !battleStart)
		{
			battleStart = true;
			StartHandle();
		}
	}

	private void StartHandle()
	{
		InputChange();
		UIPlayerDataMgr.Inst.ClearDragData();
		if (EnterEndlessBattle)
		{
			EnterEndlessBattle = false;
			Start_TestChapter(300, 0);
			UIMgr.Inst.uiFade.Hide(3f);
		}
		else if (ScriptableObjMgr.Inst.testCtrller.BattleChapter)
		{
			Start_TestChapter();
		}
		else
		{
			Start_Normal();
		}
		if (GameMgr.IsMobile_Static)
		{
			UIPlayerDataMgr.Inst.SetBuySuitBtnActive(isActive: false);
		}
		if (DataMgr.selectedWorldData.story3PlayerRoomEnter && !DataMgr.selectedWorldData.story3NPC4Rescued)
		{
			DataMgr.selectedWorldData.story3PlayerRoomEnter = false;
		}
		if (DataMgr.selectedWorldData.story4PlayerRoomEnter && !DataMgr.selectedWorldData.story4NPC5Rescued)
		{
			DataMgr.selectedWorldData.story4PlayerRoomEnter = false;
		}
		if (CurrentStage == 1 && CurrentLevel == 0)
		{
			if (npc7 != null)
			{
				npc7.Initialize();
			}
		}
		else
		{
			UnityEngine.Object.Destroy(go_Guide);
			if (npc7 != null)
			{
				npc7.EnterDoorDestroy();
			}
		}
		if (ScriptableObjMgr.Inst.testCtrller.BattleAllWand)
		{
			Dictionary<int, List<ItemInfo>> dictionary = new Dictionary<int, List<ItemInfo>>();
			for (int i = 0; i < WandConfig.list.Count; i++)
			{
				if (1 <= WandConfig.list[i].dropStage && WandConfig.list[i].dropStage <= 20)
				{
					if (!dictionary.ContainsKey(WandConfig.list[i].dropStage))
					{
						dictionary.Add(WandConfig.list[i].dropStage, new List<ItemInfo>
						{
							new ItemInfo(ItemType.Wand, WandConfig.list[i].id)
						});
					}
					else
					{
						dictionary[WandConfig.list[i].dropStage].Add(new ItemInfo(ItemType.Wand, WandConfig.list[i].id));
					}
				}
			}
			int num = 0;
			float num2 = 2f;
			foreach (KeyValuePair<int, List<ItemInfo>> item in dictionary)
			{
				Vector3[] circleDancePoints = Tool2D.GetCircleDancePoints(LevelMgr.Inst.CurrentRoomCtrller.CenterPoint + new Vector3((float)(-(dictionary.Count - 1)) / 2f * num2 + (float)num * num2, 0f, 0f), item.Value.Count, 0.25f);
				for (int j = 0; j < circleDancePoints.Length; j++)
				{
					QuickCreateSystem.Inst.CreateItem(LevelMgr.Inst.CurrentRoomMapPos, item.Value[j], circleDancePoints[j]);
				}
				num++;
			}
			if (ScriptableObjMgr.Inst.testCtrller.battleStageWandOnGround != 0)
			{
				List<ItemInfo> list = new List<ItemInfo>();
				for (int k = 0; k < WandConfig.list.Count; k++)
				{
					if (WandConfig.list[k].dropStage == ScriptableObjMgr.Inst.testCtrller.battleStageWandOnGround)
					{
						list.Add(new ItemInfo(ItemType.Wand, WandConfig.list[k].id));
					}
				}
				Vector3[] circleDancePoints2 = Tool2D.GetCircleDancePoints(LevelMgr.Inst.firstRoomPoint, list.Count, 0.25f);
				for (int l = 0; l < circleDancePoints2.Length; l++)
				{
					QuickCreateSystem.Inst.CreateItem(LevelMgr.Inst.CurrentRoomMapPos, list[l], circleDancePoints2[l]);
				}
			}
			if (ScriptableObjMgr.Inst.testCtrller.battleStageWandRandomOnHand != 0)
			{
				List<int> list2 = new List<int>();
				for (int m = 0; m < WandConfig.list.Count; m++)
				{
					if (WandConfig.list[m].dropStage == ScriptableObjMgr.Inst.testCtrller.battleStageWandRandomOnHand)
					{
						list2.Add(WandConfig.list[m].id);
					}
				}
				for (int n = 0; n < PlayerMgr.Inst.BaData.wandCfgs.Count; n++)
				{
					if (list2.Count == 0)
					{
						break;
					}
					int index = UnityEngine.Random.Range(0, list2.Count);
					PlayerMgr.Inst.WandPickUp(WandConfig.GetConfig(list2[index]));
					list2.RemoveAt(index);
				}
			}
		}
		if (ScriptableObjMgr.Inst.testCtrller.BattleAllSpell)
		{
			List<ItemInfo> list3 = new List<ItemInfo>();
			List<ItemInfo> list4 = new List<ItemInfo>();
			List<ItemInfo> list5 = new List<ItemInfo>();
			List<ItemInfo> list6 = new List<ItemInfo>();
			for (int num3 = 0; num3 < SpellConfig.list.Count; num3++)
			{
				if (SpellConfig.list[num3].level == 1 && SpellConfig.list[num3].dropType != 0)
				{
					switch (SpellConfig.list[num3].useType)
					{
					case SpellType.Missile:
						list3.Add(new ItemInfo(ItemType.Spell, SpellConfig.list[num3].id));
						break;
					case SpellType.Summon:
						list4.Add(new ItemInfo(ItemType.Spell, SpellConfig.list[num3].id));
						break;
					case SpellType.Enhance:
						list5.Add(new ItemInfo(ItemType.Spell, SpellConfig.list[num3].id));
						break;
					case SpellType.Passive:
						list6.Add(new ItemInfo(ItemType.Spell, SpellConfig.list[num3].id));
						break;
					}
				}
			}
			Vector3[] circleDancePoints3 = Tool2D.GetCircleDancePoints(LevelMgr.Inst.firstRoomPoint + new Vector3(-6f, 0f, 0f), list3.Count, 0.25f);
			for (int num4 = 0; num4 < circleDancePoints3.Length; num4++)
			{
				QuickCreateSystem.Inst.CreateItem(LevelMgr.Inst.CurrentRoomMapPos, list3[num4], circleDancePoints3[num4]);
			}
			circleDancePoints3 = Tool2D.GetCircleDancePoints(LevelMgr.Inst.firstRoomPoint + new Vector3(-2f, 0f, 0f), list4.Count, 0.25f);
			for (int num5 = 0; num5 < circleDancePoints3.Length; num5++)
			{
				QuickCreateSystem.Inst.CreateItem(LevelMgr.Inst.CurrentRoomMapPos, list4[num5], circleDancePoints3[num5]);
			}
			circleDancePoints3 = Tool2D.GetCircleDancePoints(LevelMgr.Inst.firstRoomPoint + new Vector3(2f, 0f, 0f), list5.Count, 0.25f);
			for (int num6 = 0; num6 < circleDancePoints3.Length; num6++)
			{
				QuickCreateSystem.Inst.CreateItem(LevelMgr.Inst.CurrentRoomMapPos, list5[num6], circleDancePoints3[num6]);
			}
			circleDancePoints3 = Tool2D.GetCircleDancePoints(LevelMgr.Inst.firstRoomPoint + new Vector3(6f, 0f, 0f), list6.Count, 0.25f);
			for (int num7 = 0; num7 < circleDancePoints3.Length; num7++)
			{
				QuickCreateSystem.Inst.CreateItem(LevelMgr.Inst.CurrentRoomMapPos, list6[num7], circleDancePoints3[num7]);
			}
		}
		if (ScriptableObjMgr.Inst.testCtrller.BattleAllRelic)
		{
			List<ItemInfo> list7 = new List<ItemInfo>();
			for (int num8 = 0; num8 < RelicConfig.list.Count; num8++)
			{
				list7.Add(new ItemInfo(ItemType.Relic, RelicConfig.list[num8].id));
			}
			int num9 = 10;
			int num10 = Mathf.CeilToInt((float)list7.Count / (float)num9);
			for (int num11 = 0; num11 < list7.Count; num11++)
			{
				float num12 = Mathf.FloorToInt((float)num11 / (float)num9);
				float num13 = num11 % num9;
				Vector3 vector = new Vector3(((float)(-num10) / 2f + num12) * 1.5f, (num13 - (float)num9 / 2f) * 0.4f);
				QuickCreateSystem.Inst.CreateItem(LevelMgr.Inst.CurrentRoomMapPos, list7[num11], LevelMgr.Inst.firstRoomPoint + vector);
			}
		}
		if (ScriptableObjMgr.Inst.testCtrller.BattleAllPotion)
		{
			List<ItemInfo> list8 = new List<ItemInfo>();
			for (int num14 = 0; num14 < PotionConfig.list.Count; num14++)
			{
				list8.Add(new ItemInfo(ItemType.Potion, PotionConfig.list[num14].id));
			}
			Vector3[] circleDancePoints4 = Tool2D.GetCircleDancePoints(LevelMgr.Inst.firstRoomPoint, list8.Count, 0.25f);
			for (int num15 = 0; num15 < circleDancePoints4.Length; num15++)
			{
				QuickCreateSystem.Inst.CreateItem(LevelMgr.Inst.CurrentRoomMapPos, list8[num15], circleDancePoints4[num15]);
			}
		}
		if (ScriptableObjMgr.Inst.testCtrller.BattleItem)
		{
			Vector3[] circleDancePoints5 = Tool2D.GetCircleDancePoints(LevelMgr.Inst.firstRoomPoint, ScriptableObjMgr.Inst.testCtrller.battleItemTypes.Length, 0.25f);
			for (int num16 = 0; num16 < circleDancePoints5.Length; num16++)
			{
				QuickCreateSystem.Inst.CreateItem(LevelMgr.Inst.CurrentRoomMapPos, new ItemInfo(ScriptableObjMgr.Inst.testCtrller.battleItemTypes[num16], ScriptableObjMgr.Inst.testCtrller.battleItemIDs[num16]), circleDancePoints5[num16]);
			}
		}
		if (ScriptableObjMgr.Inst.testCtrller.BattleCoin1000)
		{
			PlayerMgr.Inst.ChangeCoin(1000 - PlayerMgr.Inst.CoinCount);
		}
		if (ScriptableObjMgr.Inst.testCtrller.BattleKey100)
		{
			PlayerMgr.Inst.ChangeKey(100 - PlayerMgr.Inst.KeyCount);
		}
		if (ScriptableObjMgr.Inst.testCtrller.BattleCurse)
		{
			for (int num17 = 0; num17 < ScriptableObjMgr.Inst.testCtrller.battleCurseIDs.Length; num17++)
			{
				PlayerMgr.Inst.ItemCtrller.CurseAdd(ScriptableObjMgr.Inst.testCtrller.battleCurseIDs[num17], textFloat: false);
			}
		}
	}

	private void Start_Normal()
	{
		BattleData _archiveBattleData = DataMgr.selectedWorldData.battleData9;
		PlayerMgr.Inst.CreatePlayer();
		CamController.Inst.SetFollow(PlayerMgr.Inst.PlayerT);
		DataMgr.selectedWorldData.battleData9.RandomSpellWandCheck();
		DamageRecordeManager.ClearAllRecorde();
		Dictionary<Vector2Int, RoomConfig> dictionary = new Dictionary<Vector2Int, RoomConfig>();
		if (DataMgr.selectedWorldData.inBattle9)
		{
			DataMgr.selectedWorldData.battleData9 = _archiveBattleData;
			RoomConfig value = default(RoomConfig);
			if (!GameMgr.IsMobile_Static && ((CurrentLevel >= 1 && CurrentStage == 4) || CurrentStage > 5) && !ICJNOGPFMAM.MIFJADDOODN)
			{
				dictionary = GetDemoFinalRoom();
			}
			else
			{
				value = RoomConfig.GetConfig(DataMgr.selectedWorldData.battleData9.currentRoomID);
				value.isFinalRoom = true;
				value.LoadArchiveClear();
			}
			dictionary.Add(Vector2Int.zero, value);
			if (CurrentStage == 10 && CurrentLevel == stageLevelsCount[9])
			{
				RoomConfig value2 = ((DataMgr.selectedWorldData.battleData9.currentRoomID == 101002) ? RoomConfig.GetConfig(1008) : RoomConfig.GetConfig(1005));
				dictionary.Add(new Vector2Int(1, 0), value2);
			}
		}
		else
		{
			dictionary = GetRoomCfgs(CurrentRewardType);
			PlayerMgr.Inst.BaData.currentRoomID = dictionary[Vector2Int.zero].id;
			NextRewardTypes = OutputMgr.GetDoors();
			if (PlayerMgr.Inst.ItemCtrller.relic_ExtraDoor)
			{
				NextExtraDoorRewardType = OutputMgr.GetExtraDoor(NextRewardTypes);
			}
			else
			{
				NextExtraDoorRewardType = LevelRewardType.None;
			}
			if (DataMgr.selectedWorldData.firstEnterBattle && PlayerMgr.Inst.BaData.wandCfgs.Count == 1)
			{
				DataMgr.selectedWorldData.firstEnterBattle = false;
				CurrentLevel = 1;
				CurrentRewardType = LevelRewardType.Relic;
				dictionary = GetRoomCfgs(CurrentRewardType);
				RoomConfig config = RoomConfig.GetConfig(10201);
				config.isFinalRoom = true;
				dictionary[Vector2Int.zero] = config;
				NextRewardTypes.Clear();
				NextRewardTypes.Add(LevelRewardType.Spell);
				PlayerMgr.Inst.BaData.wandCfgs = new List<WandConfig> { WandConfig.GetConfig(1) };
				PlayerMgr.Inst.ChangeWandSpell(0, WandSlotType.Normal, 0, new SlotData(30051, 0));
				PlayerMgr.Inst.ChangeWandSpell(0, WandSlotType.Normal, 1, new SlotData(30121, 0));
			}
			DataMgr.selectedWorldData.enterBattleTime++;
		}
		Time.timeScale = 0f;
		LevelMgr.Inst.CreateLevel(dictionary, CurrentRewardType, NextRewardTypes, NextExtraDoorRewardType, fadeDisappear: false, delegate
		{
			CreateLevelFinish(!DataMgr.selectedWorldData.inBattle9);
			UIPlayerDataMgr.Inst.UpdateAllInfo();
			PlayerMgr.Inst.WandRecreate();
			PlayerMgr.Inst.WandSelect(0);
			Wand.wandHoldingFlyEffectApplying = false;
			PlayerMgr.Inst.AllWandFullMP();
			if (DataMgr.selectedWorldData.inBattle9)
			{
				UnitConfig playerCfg = _archiveBattleData.playerCfg;
				List<RelicConfig> list = new List<RelicConfig>();
				List<int> list2 = new List<int>();
				List<int> list3 = new List<int>();
				for (int i = 0; i < _archiveBattleData.relicCfgs.Count; i++)
				{
					list.Add(_archiveBattleData.relicCfgs[i]);
				}
				for (int j = 0; j < _archiveBattleData.curseIDs.Count; j++)
				{
					list2.Add(_archiveBattleData.curseIDs[j]);
				}
				for (int k = 0; k < _archiveBattleData.curseLevels.Count; k++)
				{
					list3.Add(_archiveBattleData.curseLevels[k]);
				}
				_archiveBattleData.relicCfgs.Clear();
				_archiveBattleData.curseIDs.Clear();
				_archiveBattleData.curseLevels.Clear();
				float bodySize = _archiveBattleData.bodySize;
				for (int l = 0; l < list.Count; l++)
				{
					RelicConfig relicConfig = list[l];
					int intTimer = relicConfig.intTimer;
					float floatTimer = relicConfig.floatTimer;
					if (RelicConfig.dic[relicConfig.id].abilityType == RelicAbilityType.WandAddSlot && relicConfig.level > 1)
					{
						PlayerMgr.Inst.ItemCtrller.relicCfg_WandAddSlot = RelicConfig.GetConfig(relicConfig.id);
						PlayerMgr.Inst.ItemCtrller.relicCfg_WandAddSlot.level = relicConfig.level - 1;
						PlayerMgr.Inst.BaData.relicCfgs.Add(PlayerMgr.Inst.ItemCtrller.relicCfg_WandAddSlot);
						PlayerMgr.Inst.ItemCtrller.RelicAdd(relicConfig.id, addGallery: false, fromLoadSave: true);
					}
					else
					{
						for (int m = 0; m < relicConfig.level; m++)
						{
							PlayerMgr.Inst.ItemCtrller.RelicAdd(relicConfig.id, addGallery: false, fromLoadSave: true);
						}
					}
					PlayerMgr.Inst.BaData.relicCfgs[l].intTimer = intTimer;
					PlayerMgr.Inst.BaData.relicCfgs[l].floatTimer = floatTimer;
				}
				for (int n = 0; n < list2.Count; n++)
				{
					for (int num = 0; num < list3[n]; num++)
					{
						if (CurseConfig.dic[list2[n]].abilityType == CurseAbilityType.BagReduce)
						{
							CurseConfig config2 = CurseConfig.GetConfig(list2[n]);
							config2.int1.RandomResult(1);
							PlayerMgr.Inst.BaData.bagCount += config2.int1.result;
							for (int num2 = 0; num2 < config2.int1.result; num2++)
							{
								PlayerMgr.Inst.BaData.bagSpellDatas.Add(null);
							}
						}
						PlayerMgr.Inst.ItemCtrller.CurseAdd(list2[n], textFloat: false);
					}
				}
				DataMgr.selectedWorldData.battleData9.playerCfg = playerCfg;
				PlayerMgr.Inst.TryGetPlayerPpt(out var playerPpt);
				playerPpt.unitCfg = DataMgr.selectedWorldData.battleData9.playerCfg;
				ettMgr.SetComponentData(PlayerMgr.Inst.PlayerEtt, playerPpt);
				DataMgr.selectedWorldData.battleData9.bodySize = bodySize;
				PlayerMgr.Inst.ChangeBodySize();
				PlayerMgr.Inst.PlayerCtrller.TextFloatQueueClear();
				UIPlayerDataMgr.Inst.UpdateAllInfo();
				if (CurrentStage == 10 && CurrentLevel == stageLevelsCount[9])
				{
					LevelMgr.Inst.CurrentRoomCtrller.HideBoundaryDisappear();
					QuickCreateSystem.Inst.CreateMixedEtt("BackCampPortal", Tool2D.GetNavMeshPointIngoreZ(LevelMgr.Inst.CurrentRoomCtrller.CenterPoint));
				}
			}
			else
			{
				PlayerMgr.Inst.BaData.RandomSpecialRoom();
			}
			DataMgr.selectedWorldData.inBattle9 = true;
			if (DataMgr.selectedWorldData.IsDave)
			{
				PlayerMgr.Inst.UpdateSkin();
			}
		});
	}

	private void Start_TestChapter(int stage = -1, int level = -1, LevelRewardType type = LevelRewardType.None)
	{
		PlayerMgr.Inst.CreatePlayer();
		CamController.Inst.SetFollow(PlayerMgr.Inst.PlayerT);
		DataMgr.selectedWorldData.battleData9.RandomSpellWandCheck();
		CurrentStage = ((stage >= 0) ? stage : ScriptableObjMgr.Inst.testCtrller.battleStage);
		CurrentLevel = ((level >= 0) ? level : ScriptableObjMgr.Inst.testCtrller.battleLevel);
		CurrentRewardType = ((type != LevelRewardType.None) ? type : ScriptableObjMgr.Inst.testCtrller.battleRewardType);
		Dictionary<Vector2Int, RoomConfig> dictionary = new Dictionary<Vector2Int, RoomConfig>();
		dictionary = GetRoomCfgs(CurrentRewardType);
		PlayerMgr.Inst.BaData.currentRoomID = dictionary[Vector2Int.zero].id;
		NextRewardTypes = OutputMgr.GetDoors();
		if (PlayerMgr.Inst.ItemCtrller.relic_ExtraDoor)
		{
			NextExtraDoorRewardType = OutputMgr.GetExtraDoor(NextRewardTypes);
		}
		else
		{
			NextExtraDoorRewardType = LevelRewardType.None;
		}
		if (PlayerMgr.Inst.BaData.currentStage <= stageLevelsCount.Length)
		{
			PlayerMgr.Inst.BaData.RandomSpecialRoom();
		}
		UIPlayerDataMgr.Inst.UpdateAllInfo();
		DataMgr.selectedWorldData.battleData9.endlessMonsterIDs.Clear();
		Time.timeScale = 0f;
		LevelMgr.Inst.CreateLevel(dictionary, CurrentRewardType, NextRewardTypes, NextExtraDoorRewardType, fadeDisappear: false, delegate
		{
			PlayerMgr.Inst.WandRecreate();
			PlayerMgr.Inst.WandSelect(0);
			PlayerMgr.Inst.AllWandFullMP();
			DataMgr.selectedWorldData.inBattle9 = true;
			if (DataMgr.selectedWorldData.IsDave)
			{
				PlayerMgr.Inst.UpdateSkin();
			}
			CreateLevelFinish(showPlaceName: false);
		});
	}

	private void CreateLevelFinish(bool showPlaceName)
	{
		StartCoroutine(CreateLevelFinishIE(showPlaceName));
	}

	private IEnumerator CreateLevelFinishIE(bool showPlaceName)
	{
		Time.timeScale = 1f;
		if (!DataMgr.selectedWorldData.isTriggerTutorialHpShow)
		{
			UIPlayerDataMgr.Inst.HideDirect();
		}
		UIMgr.Inst.uiFade.Hide();
		yield return new WaitForSeconds(UIMgr.Inst.uiFade.fadeTime);
		if (!DataMgr.selectedWorldData.isTriggerTutorialHpShow)
		{
			UIPlayerDataMgr.Inst.Show();
		}
		if (DataMgr.selectedWorldData.isTriggerTutorialHpShow)
		{
			TimeScaleMgr.Inst.Pause();
		}
		yield return new WaitForSeconds(1f);
		if (!ScriptableObjMgr.Inst.testCtrller.BattleSkipChapterThrough && showPlaceName)
		{
			PlaceNameType nameType = PlaceNameType.Camp;
			switch (CurrentStage)
			{
			case 1:
				nameType = PlaceNameType.Chapter1;
				break;
			case 3:
				nameType = PlaceNameType.Chapter2;
				break;
			case 5:
				nameType = PlaceNameType.Chapter3;
				break;
			case 7:
				nameType = PlaceNameType.Chapter4;
				break;
			case 9:
				nameType = PlaceNameType.Chapter5;
				break;
			case 300:
				nameType = PlaceNameType.Endless;
				break;
			default:
				Debug.LogError(CurrentStage);
				break;
			case 2:
			case 4:
			case 6:
			case 8:
			case 10:
				break;
			}
			UIPlaceNameMgr.Inst.Show(nameType);
		}
	}

	private void FixedUpdate()
	{
		if (ControlMgr.Inst.GetTimeCount())
		{
			if (DataMgr.selectedWorldData.timeuse < 1f)
			{
				DataMgr.selectedWorldData.timeuse += 3600f;
			}
			if (!UIBattleMgr.Inst.uiFinishBuildShow.IsOpen)
			{
				DataMgr.selectedWorldData.timeuse += Time.fixedDeltaTime;
			}
		}
	}

	private Dictionary<Vector2Int, RoomConfig> GetRoomCfgs(LevelRewardType rewardType)
	{
		Dictionary<Vector2Int, RoomConfig> dictionary = new Dictionary<Vector2Int, RoomConfig>();
		if (!GameMgr.IsMobile_Static && ((CurrentLevel >= 1 && CurrentStage == 4) || CurrentStage > 5) && !ICJNOGPFMAM.MIFJADDOODN)
		{
			return GetDemoFinalRoom();
		}
		if (CurrentStage == 300)
		{
			if (CurrentLevel == 0)
			{
				RoomConfig chapterRoom = OutputMgr.GetChapterRoom0();
				chapterRoom.isFinalRoom = true;
				dictionary.Add(new Vector2Int(0, 0), chapterRoom);
			}
			else
			{
				bool flag = CurrentLevel % 5 == 1 && CurrentLevel > 1;
				RoomConfig chapterRoom = PlayerMgr.Inst.BaData.GetEndlessRoomFromPool((!flag) ? RoomType.Monster : RoomType.Boss);
				chapterRoom.isFinalRoom = true;
				chapterRoom.generateRO = true;
				dictionary.Add(Vector2Int.zero, chapterRoom);
				RoomConfig endlessRoomFromPool = PlayerMgr.Inst.BaData.GetEndlessRoomFromPool(RoomType.Puzzle);
				int num = ((UnityEngine.Random.Range(0, 2) != 0) ? 1 : (-1));
				if (chapterRoom.accessLeft == Vector2Data.Up1000)
				{
					num = 1;
				}
				else if (chapterRoom.accessRight == Vector2Data.Up1000)
				{
					num = -1;
				}
				if (num == -1)
				{
					endlessRoomFromPool.ReverseX();
				}
				dictionary.Add(new Vector2Int(num, 0), endlessRoomFromPool);
			}
		}
		else if (CurrentStage >= 50)
		{
			bool flag2 = false;
			if (PlayerMgr.Inst.BaData.specialRoomLevels.Contains(CurrentLevel) || ScriptableObjMgr.Inst.testCtrller.BattleSideRoom)
			{
				flag2 = true;
			}
			RoomConfig chapterRoom = PlayerMgr.Inst.BaData.GetTestRoomFromPool();
			chapterRoom.isFinalRoom = true;
			if (UnityEngine.Random.Range(0, 2) == 0)
			{
				chapterRoom.ReverseX();
			}
			chapterRoom.generateRO = true;
			dictionary.Add(Vector2Int.zero, chapterRoom);
			if (flag2)
			{
				RoomConfig roomFromPool = PlayerMgr.Inst.BaData.GetRoomFromPool(RoomType.Puzzle, needLeftRightAccess: true, needRO: false);
				int num2 = ((UnityEngine.Random.Range(0, 2) != 0) ? 1 : (-1));
				if (chapterRoom.accessLeft == Vector2Data.Up1000)
				{
					num2 = 1;
				}
				else if (chapterRoom.accessRight == Vector2Data.Up1000)
				{
					num2 = -1;
				}
				if (num2 == -1)
				{
					roomFromPool.ReverseX();
				}
				dictionary.Add(new Vector2Int(num2, 0), roomFromPool);
			}
		}
		else if (CurrentLevel == 0 && CurrentStage % 2 == 1)
		{
			RoomConfig chapterRoom = OutputMgr.GetChapterRoom0();
			chapterRoom.isFinalRoom = true;
			dictionary.Add(new Vector2Int(0, 0), chapterRoom);
		}
		else
		{
			bool flag4;
			RoomConfig chapterRoom;
			int num4;
			RoomConfig hideBossRoom;
			switch (rewardType)
			{
			case LevelRewardType.Spell:
			case LevelRewardType.Relic:
			case LevelRewardType.MaxHP:
			case LevelRewardType.Coin:
			{
				bool flag5 = false;
				if (PlayerMgr.Inst.BaData.specialRoomLevels.Contains(CurrentLevel))
				{
					flag5 = true;
				}
				if (ScriptableObjMgr.Inst.testCtrller.BattleSideRoom)
				{
					flag5 = true;
				}
				float num5 = rroChances[CurrentStage - 1];
				if (PlayerMgr.Inst.ItemCtrller.relic_CertainlyHaveRRO)
				{
					num5 = 1f;
				}
				bool flag6 = ((UnityEngine.Random.value <= num5) ? true : false);
				if (ScriptableObjMgr.Inst.testCtrller.BattleRRO)
				{
					flag6 = true;
				}
				chapterRoom = PlayerMgr.Inst.BaData.GetRoomFromPool(RoomType.Monster, flag5, flag6);
				chapterRoom.isFinalRoom = true;
				if (UnityEngine.Random.Range(0, 2) == 0)
				{
					chapterRoom.ReverseX();
				}
				if (flag6)
				{
					chapterRoom.generateRO = true;
				}
				dictionary.Add(new Vector2Int(0, 0), chapterRoom);
				if (flag5)
				{
					RoomConfig value2 = ((DataMgr.selectedWorldData.story2Finish && !DataMgr.selectedWorldData.story3Finish && !DataMgr.selectedWorldData.story3PlayerRoomEnter && (CurrentStage == 3 || CurrentStage == 4)) ? RoomConfig.GetConfig(221) : ((!DataMgr.selectedWorldData.story3Finish || DataMgr.selectedWorldData.story4Finish || DataMgr.selectedWorldData.story4PlayerRoomEnter || (CurrentStage != 5 && CurrentStage != 6)) ? PlayerMgr.Inst.BaData.GetRoomFromPool(RoomType.Puzzle, needLeftRightAccess: true, needRO: false) : RoomConfig.GetConfig(222)));
					int num6 = ((UnityEngine.Random.Range(0, 2) != 0) ? 1 : (-1));
					if (chapterRoom.accessLeft == Vector2Data.Up1000)
					{
						num6 = 1;
					}
					else if (chapterRoom.accessRight == Vector2Data.Up1000)
					{
						num6 = -1;
					}
					if (num6 == -1)
					{
						value2.ReverseX();
					}
					dictionary.Add(new Vector2Int(num6, 0), value2);
				}
				break;
			}
			case LevelRewardType.Elite:
				chapterRoom = PlayerMgr.Inst.BaData.GetRoomFromPool(RoomType.Elite, needLeftRightAccess: false, needRO: false);
				chapterRoom.isFinalRoom = true;
				dictionary.Add(new Vector2Int(0, 0), chapterRoom);
				break;
			case LevelRewardType.Boss:
				flag4 = false;
				if (DataMgr.selectedWorldData.IsDave)
				{
					flag4 = true;
				}
				else if (ICJNOGPFMAM.GGPJCCLPBJL && UnityEngine.Random.Range(0f, 1f) >= 0.5f)
				{
					int currentStage = PlayerMgr.Inst.BaData.currentStage;
					if (currentStage != 2)
					{
						if (currentStage != 8)
						{
							if (currentStage == 10 && DataMgr.selectedWorldData.daveKilledBoss)
							{
								goto IL_051d;
							}
						}
						else if (DataMgr.selectedWorldData.daveKilledBoss4)
						{
							goto IL_051d;
						}
					}
					else if (DataMgr.selectedWorldData.daveKilledBoss1)
					{
						goto IL_051d;
					}
				}
				goto IL_0520;
			case LevelRewardType.Store:
				chapterRoom = OutputMgr.GetStoreRoomCfg(CurrentStage);
				chapterRoom.isFinalRoom = true;
				dictionary.Add(new Vector2Int(0, 0), chapterRoom);
				if (PlayerMgr.Inst.BaData.specialRoomLevels.Contains(CurrentLevel) || ScriptableObjMgr.Inst.testCtrller.BattleSideRoom || PlayerMgr.Inst.ItemCtrller.relic_SpecialStore)
				{
					Vector2Int key2 = new Vector2Int((UnityEngine.Random.Range(0, 2) != 0) ? 1 : (-1), 0);
					dictionary.Add(key2, OutputMgr.GetPotionRoomCfg(CurrentStage));
				}
				break;
			case LevelRewardType.Process:
				chapterRoom = OutputMgr.GetProcessRoomCfg(CurrentStage);
				chapterRoom.isFinalRoom = true;
				dictionary.Add(new Vector2Int(0, 0), chapterRoom);
				if (PlayerMgr.Inst.BaData.specialRoomLevels.Contains(CurrentLevel) || ScriptableObjMgr.Inst.testCtrller.BattleSideRoom || PlayerMgr.Inst.ItemCtrller.relic_SpecialStore)
				{
					Vector2Int key = new Vector2Int((UnityEngine.Random.Range(0, 2) != 0) ? 1 : (-1), 0);
					dictionary.Add(key, OutputMgr.GetMoreInOneRoomCfg(CurrentStage));
				}
				break;
			case LevelRewardType.Spring:
				chapterRoom = OutputMgr.GetSpringRoom(CurrentStage);
				chapterRoom.isFinalRoom = true;
				dictionary.Add(new Vector2Int(0, 0), chapterRoom);
				break;
			case LevelRewardType.Shortcut:
			{
				bool flag3 = false;
				for (int i = 0; i < 4; i++)
				{
					if (PlayerMgr.Inst.BaData.specialRoomLevels.Contains(CurrentLevel + i))
					{
						flag3 = true;
						break;
					}
				}
				if (ScriptableObjMgr.Inst.testCtrller.BattleSideRoom)
				{
					flag3 = true;
				}
				chapterRoom = PlayerMgr.Inst.BaData.GetRoomFromPool(RoomType.Shortcut, flag3, needRO: true);
				chapterRoom.isFinalRoom = true;
				chapterRoom.generateRO = true;
				if (UnityEngine.Random.Range(0, 2) == 0)
				{
					chapterRoom.ReverseX();
				}
				dictionary.Add(new Vector2Int(0, 0), chapterRoom);
				if (flag3)
				{
					RoomConfig value = ((DataMgr.selectedWorldData.story2Finish && !DataMgr.selectedWorldData.story3Finish && !DataMgr.selectedWorldData.story3PlayerRoomEnter && (CurrentStage == 3 || CurrentStage == 4) && ICJNOGPFMAM.MIFJADDOODN) ? RoomConfig.GetConfig(221) : ((!DataMgr.selectedWorldData.story3Finish || DataMgr.selectedWorldData.story4Finish || DataMgr.selectedWorldData.story4PlayerRoomEnter || (CurrentStage != 5 && CurrentStage != 6) || !ICJNOGPFMAM.MIFJADDOODN) ? PlayerMgr.Inst.BaData.GetRoomFromPool(RoomType.Puzzle, needLeftRightAccess: true, needRO: false) : RoomConfig.GetConfig(222)));
					int num3 = ((UnityEngine.Random.Range(0, 2) != 0) ? 1 : (-1));
					if (chapterRoom.accessLeft == Vector2Data.Up1000)
					{
						num3 = 1;
					}
					else if (chapterRoom.accessRight == Vector2Data.Up1000)
					{
						num3 = -1;
					}
					if (num3 == -1)
					{
						value.ReverseX();
					}
					dictionary.Add(new Vector2Int(num3, 0), value);
				}
				break;
			}
			default:
				Debug.LogError(rewardType);
				break;
			case LevelRewardType.Wand:
			case LevelRewardType.None:
				break;
				IL_0520:
				dictionary.Add(new Vector2Int(0, 0), OutputMgr.GetBossRoom0(flag4));
				chapterRoom = PlayerMgr.Inst.BaData.GetRoomFromPool(RoomType.Boss, needLeftRightAccess: false, needRO: false, flag4);
				chapterRoom.isFinalRoom = true;
				dictionary.Add(new Vector2Int(0, 1), chapterRoom);
				if ((CurrentStage == 6 && DataMgr.selectedWorldData.selectedDifficulty == DifficultyType.Easy) || (CurrentStage == 8 && DataMgr.selectedWorldData.selectedDifficulty == DifficultyType.Normal))
				{
					break;
				}
				if (CurrentStage == 10)
				{
					if (DataMgr.selectedWorldData.selectedDifficulty == DifficultyType.Nightmare3)
					{
						dictionary.Add(new Vector2Int(1, 1), OutputMgr.GetHideBossRoom(flag4));
					}
					break;
				}
				num4 = ((UnityEngine.Random.Range(0, 2) != 0) ? 1 : (-1));
				hideBossRoom = OutputMgr.GetHideBossRoom(flag4);
				if (hideBossRoom.id == 1006)
				{
					num4 = -1;
				}
				else if (num4 == -1)
				{
					hideBossRoom.ReverseX();
				}
				dictionary.Add(new Vector2Int(num4, 1), hideBossRoom);
				break;
				IL_051d:
				flag4 = true;
				goto IL_0520;
			}
		}
		return dictionary;
	}

	private Dictionary<Vector2Int, RoomConfig> GetBuyGameRoom()
	{
		Debug.Log("进入购买房间");
		Dictionary<Vector2Int, RoomConfig> dictionary = new Dictionary<Vector2Int, RoomConfig>();
		RoomConfig config = RoomConfig.GetConfig(110);
		dictionary[Vector2Int.zero] = config;
		return dictionary;
	}

	private Dictionary<Vector2Int, RoomConfig> GetDemoFinalRoom()
	{
		Debug.Log("进入Demo房间");
		Dictionary<Vector2Int, RoomConfig> dictionary = new Dictionary<Vector2Int, RoomConfig>();
		RoomConfig config = RoomConfig.GetConfig(110);
		dictionary[Vector2Int.zero] = config;
		return dictionary;
	}

	public void PlayerEnterDoor(DoorBase_Dots doorBase)
	{
		if (GameMgr.IsMobile_Static && GameMgr.IsUseBiliOneSDK && ((LevelMgr.Inst.CurrentRewardType == LevelRewardType.Elite && CurrentStage == 3) || CurrentStage > 4) && !ICJNOGPFMAM.MIFJADDOODN)
		{
			GameUISingletonMono<UIFullGame>.ShowInit();
			return;
		}
		if (CurrentStage > 1 || CurrentLevel > 0)
		{
			DataMgr.SaveSelectedWorldData();
		}
		if (CurrentStage == 1 && CurrentLevel == 0)
		{
			UISpellDisableHistory.SaveDisableHistory();
		}
		if (!ScriptableObjMgr.Inst.testCtrller.SkipAllStoryMixed && CurrentStage == 2 && CurrentLevel == 1 && DataMgr.selectedWorldData.storyHardBossDropPickup && DataMgr.selectedWorldData.storyHardFinishBackCamp && !DataMgr.selectedWorldData.storyHardFinishNPC7Appearance)
		{
			UIMgr.Inst.uiFade.Show(delegate
			{
				ObjPoolMgr.Inst.ClearAllPool();
				UIPlayerDataMgr.Inst.HideDirect();
				GameMgr.Inst.RecycleAllPool();
				using EntityQuery entityQuery2 = ettMgr.CreateEntityQuery(typeof(TeammateData));
				NativeArray<Entity> nativeArray2 = entityQuery2.ToEntityArray(Allocator.Temp);
				GameMgr.Inst.playerMgr.SummonsAllDead(instanceDeath: true, clearAllAutoWand: false);
				nativeArray2.Dispose();
				SceneManager.LoadScene("NPC7Appearance");
			});
			return;
		}
		if ((CurrentStage == 1 && CurrentLevel == 0) || (CurrentStage == 300 && CurrentLevel == 0))
		{
			if (PlayerMgr.Inst.BaData.poolOfRelic.ContainsKey(69))
			{
				BattleStartLogger battleStartLogger = new BattleStartLogger();
				battleStartLogger.suit = new Suit
				{
					id = DataMgr.selectedWorldData.selectedSetID,
					level = DataMgr.selectedWorldData.setUnlockedSets[DataMgr.selectedWorldData.selectedSetID]
				};
				battleStartLogger.ban_spells = (from e in GameUISingletonMono<UISpellDisable>.Inst.disableSlots
					where e.AlreadyDisable
					select e.Level1ID).ToList();
				battleStartLogger.talent = TalentStatus.CreateAuto();
				battleStartLogger.unlocked_activate = DataMgr.selectedWorldData.activateGirlActivatedIDs2;
				battleStartLogger.unlocked_research = DataMgr.selectedWorldData.researchedIDs;
				battleStartLogger.Report();
			}
		}
		else if (LevelMgr.Inst.RoomFinishLogger != null)
		{
			RoomFinishLogger roomFinishLogger = LevelMgr.Inst.RoomFinishLogger;
			roomFinishLogger.finish_equips = PlayerEquips.CreateAuto();
			roomFinishLogger.finish_resources = ResourcesStatus.CreateAuto();
			roomFinishLogger.flow_resources = ResourcesStatus.CreateFlow(roomFinishLogger.entry_resources, roomFinishLogger.finish_resources);
			roomFinishLogger.spend_seconds = Mathf.CeilToInt(DataMgr.selectedWorldData.timeuse - LevelMgr.Inst.BattleStartTime);
			roomFinishLogger.next_room_selected = doorBase.rewardType;
			using EntityQuery entityQuery = ettMgr.CreateEntityQuery(typeof(SpecialObj4_Dots));
			NativeArray<Entity> nativeArray = entityQuery.ToEntityArray(Allocator.Temp);
			for (int i = 0; i < nativeArray.Length; i++)
			{
				SpecialObj4_Dots componentData = ettMgr.GetComponentData<SpecialObj4_Dots>(nativeArray[i]);
				if (componentData.chestType == ChestType.Curse)
				{
					roomFinishLogger.cursed_chest.spawn_count++;
					roomFinishLogger.cursed_chest.spawn_curse.Add(componentData.curseID);
				}
				else if (componentData.chestType == ChestType.Lock)
				{
					roomFinishLogger.locked_chest.spawn_count++;
				}
			}
			for (int j = 0; j < LevelMgr.Inst.CurrentRoomCtrller.accessEttList.Count; j++)
			{
				AccessBase_Dots accessBase = ettMgr.GetComponentData<AccessBase_Dots>(LevelMgr.Inst.CurrentRoomCtrller.accessEttList[j]);
				if (accessBase.alreadyUseKey)
				{
					RoomFinishLogger.SideRoomInfo sideRoomInfo = roomFinishLogger.side_room.FirstOrDefault((RoomFinishLogger.SideRoomInfo room) => room.dir == accessBase.Dir);
					if (sideRoomInfo != null)
					{
						sideRoomInfo.unlocked = true;
					}
				}
			}
			if (GameMgr.IsMobile_Static)
			{
				string properties = JsonConvert.SerializeObject(roomFinishLogger);
				MobileMgr.inst.PluginActivity.UploadEvent("enter_room", properties);
			}
		}
		PlayerMgr.Inst.PlayerCtrller.StopMotion();
		ProcessPlayerEnterDoorEffects(doorBase);
		foreach (Wand wand in PlayerMgr.Inst.Wands)
		{
			wand.ReleaseCharge();
		}
		DamageRecordeManager.ClearCurrentRecorde();
		UIPlayerDataMgr.Inst.CancelDrag();
		StartCoroutine(PlayerEnterDoorDelay(doorBase));
	}

	private IEnumerator PlayerEnterDoorDelay(DoorBase_Dots doorBase)
	{
		yield return null;
		if (ettMgr.GetComponentData<UnitProperty_Dots>(PlayerMgr.Inst.PlayerEtt).unitCfg.currentHP <= 0f)
		{
			PlayerMgr.Inst.PlayerCtrller.StartMotion();
			yield break;
		}
		UIMgr.Inst.uiFade.Show(delegate
		{
			float currentMPRatio = 1f;
			if (PlayerMgr.Inst.ItemCtrller.curseCfg_EnterDoorNoMP != null)
			{
				currentMPRatio = (float)PlayerMgr.Inst.ItemCtrller.curseCfg_EnterDoorNoMP.int1.result / 100f;
			}
			PlayerMgr.Inst.AllWandFullMP(currentMPRatio);
			PlayerMgr.Inst.FlyRegisterWithAllMate();
			PlayerMgr.Inst.InvincibleRegister();
			foreach (Wand wand in PlayerMgr.Inst.Wands)
			{
				wand.ResetAndRecheck();
			}
			foreach (KeyValuePair<Vector2Int, RoomController> roomCtrller in LevelMgr.Inst.RoomCtrllers)
			{
				roomCtrller.Value.RoomRecyeleDelegateExecute();
			}
			if (go_Guide != null)
			{
				UnityEngine.Object.Destroy(go_Guide);
			}
			if (npc7 != null)
			{
				npc7.EnterDoorDestroy();
			}
			if (SpecialObj310EndlessEntrance.Inst != null)
			{
				UnityEngine.Object.Destroy(SpecialObj310EndlessEntrance.Inst.gameObject);
			}
			if (GameMgr.IsMobile_Static)
			{
				GameMgr.Inst.ClearAllPool();
			}
			else if (doorBase.rewardType == LevelRewardType.Chapter)
			{
				GameMgr.Inst.RecycleAllPool();
			}
			else
			{
				GameMgr.Inst.ClearAllPool();
			}
			if (doorBase.rewardType == LevelRewardType.Chapter)
			{
				UIMgr.Inst.uiFade.Hide(0f);
				foreach (Wand wand2 in PlayerMgr.Inst.Wands)
				{
					wand2.ClearAutoSpell(typeof(Spell4019BiAnBladeData));
				}
				int startChapter = CurrentStage / 2;
				GameUISingletonMono<UIChapterThrough>.Inst.DifficultyMoveImmediate();
				GameUISingletonMono<UIChapterThrough>.Inst.Show(startChapter, delegate
				{
					int currentStage = CurrentStage;
					CurrentStage = currentStage + 1;
					CurrentLevel = 0;
					PlayerMgr.Inst.BaData.RandomSpecialRoom();
					CurrentRewardType = LevelRewardType.None;
					NextRewardTypes = OutputMgr.GetDoors();
					if (PlayerMgr.Inst.ItemCtrller.relic_ExtraDoor)
					{
						NextExtraDoorRewardType = OutputMgr.GetExtraDoor(NextRewardTypes);
					}
					else
					{
						NextExtraDoorRewardType = LevelRewardType.None;
					}
					Dictionary<Vector2Int, RoomConfig> roomCfgs2 = GetRoomCfgs(LevelRewardType.None);
					PlayerMgr.Inst.BaData.currentRoomID = roomCfgs2[Vector2Int.zero].id;
					LevelMgr.Inst.CreateLevel(roomCfgs2, CurrentRewardType, NextRewardTypes, NextExtraDoorRewardType, fadeDisappear: true, PlayerEnterDoor2ShowChapterName);
				});
			}
			else
			{
				if (!doorBase.isExtraDoor)
				{
					switch (doorBase.rewardType)
					{
					case LevelRewardType.Spell:
						PlayerMgr.Inst.BaData.continuousNonRelic++;
						PlayerMgr.Inst.BaData.continuousNonSpell = 0;
						PlayerMgr.Inst.BaData.continuousNonCoin++;
						break;
					case LevelRewardType.Relic:
						PlayerMgr.Inst.BaData.continuousNonRelic = 0;
						PlayerMgr.Inst.BaData.continuousNonCoin++;
						PlayerMgr.Inst.BaData.continuousNonSpell++;
						break;
					case LevelRewardType.Coin:
						PlayerMgr.Inst.BaData.continuousNonRelic++;
						PlayerMgr.Inst.BaData.continuousNonCoin = 0;
						PlayerMgr.Inst.BaData.continuousNonSpell++;
						break;
					case LevelRewardType.Elite:
						PlayerMgr.Inst.BaData.continuousNonRelic++;
						PlayerMgr.Inst.BaData.continuousNonCoin++;
						PlayerMgr.Inst.BaData.continuousNonSpell++;
						break;
					case LevelRewardType.Boss:
						PlayerMgr.Inst.BaData.continuousNonRelic++;
						PlayerMgr.Inst.BaData.continuousNonCoin++;
						PlayerMgr.Inst.BaData.continuousNonSpell = 0;
						break;
					case LevelRewardType.MaxHP:
					case LevelRewardType.Store:
					case LevelRewardType.Process:
					case LevelRewardType.Spring:
						PlayerMgr.Inst.BaData.continuousNonSpell++;
						PlayerMgr.Inst.BaData.continuousNonRelic++;
						PlayerMgr.Inst.BaData.continuousNonCoin++;
						break;
					case LevelRewardType.Shortcut:
						PlayerMgr.Inst.BaData.continuousNonSpell = 0;
						PlayerMgr.Inst.BaData.continuousNonRelic = 0;
						PlayerMgr.Inst.BaData.continuousNonCoin = 0;
						break;
					default:
						Debug.LogError(doorBase.rewardType);
						break;
					case LevelRewardType.Chapter:
					case LevelRewardType.None:
					case LevelRewardType.EndlessChapter:
						break;
					}
				}
				if (doorBase.rewardType == LevelRewardType.Shortcut)
				{
					CurrentLevel += 4;
				}
				else if (doorBase.rewardType == LevelRewardType.EndlessChapter)
				{
					CurrentStage = 300;
					CurrentLevel = 0;
				}
				else if (CurrentStage == 300)
				{
					int currentLevel = CurrentLevel;
					CurrentLevel = currentLevel + 1;
				}
				else if (CurrentLevel == stageLevelsCount[CurrentStage - 1])
				{
					int currentLevel = CurrentStage;
					CurrentStage = currentLevel + 1;
					CurrentLevel = 1;
					PlayerMgr.Inst.BaData.RandomSpecialRoom();
				}
				else
				{
					int currentLevel = CurrentLevel;
					CurrentLevel = currentLevel + 1;
				}
				CurrentRewardType = doorBase.rewardType;
				NextRewardTypes = OutputMgr.GetDoors();
				if (PlayerMgr.Inst.ItemCtrller.relic_ExtraDoor)
				{
					NextExtraDoorRewardType = OutputMgr.GetExtraDoor(NextRewardTypes);
				}
				else
				{
					NextExtraDoorRewardType = LevelRewardType.None;
				}
				Dictionary<Vector2Int, RoomConfig> roomCfgs = GetRoomCfgs(doorBase.rewardType);
				if (doorBase.rewardType == LevelRewardType.Boss || roomCfgs[Vector2Int.zero].id == 1025)
				{
					PlayerMgr.Inst.BaData.currentRoomID = roomCfgs[Vector2Int.up].id;
				}
				else
				{
					PlayerMgr.Inst.BaData.currentRoomID = roomCfgs[Vector2Int.zero].id;
				}
				LevelMgr.Inst.CreateLevel(roomCfgs, CurrentRewardType, NextRewardTypes, NextExtraDoorRewardType, fadeDisappear: true, PlayerEnterDoor2);
			}
		});
		yield return null;
	}

	private void ProcessPlayerEnterDoorEffects(DoorBase_Dots door)
	{
		UnitProperty_Dots componentData = ettMgr.GetComponentData<UnitProperty_Dots>(PlayerMgr.Inst.PlayerEtt);
		if (PlayerMgr.Inst.ItemCtrller.relicGroupConfigs.TryGetValue(1, out var value))
		{
			float num = (float)value.int1.result / 100f;
			float value2 = componentData.unitCfg.shieldTemp * num;
			PlayerMgr.Inst.ChangeShield(value2);
		}
		if (componentData.unitCfg.shieldTemp > 0f)
		{
			PlayerMgr.Inst.ChangeShieldTemp(0f - componentData.unitCfg.shieldTemp);
		}
		if (PlayerMgr.Inst.ItemCtrller.uiPotionPsychedelic != null)
		{
			PlayerMgr.Inst.ItemCtrller.uiPotionPsychedelic.DestroySelf();
		}
		int num2 = ((door.rewardType != LevelRewardType.Shortcut) ? 1 : 4);
		for (int i = 0; i < num2; i++)
		{
			if (PlayerMgr.Inst.ItemCtrller.relicCfg_FloorShield != null)
			{
				PlayerMgr.Inst.ChangeShield(PlayerMgr.Inst.ItemCtrller.relicCfg_FloorShield.int1.result);
			}
			if (PlayerMgr.Inst.ItemCtrller.relic_FollowObj_FloorInterest != null)
			{
				int a = Mathf.CeilToInt((float)(PlayerMgr.Inst.BaData.coinCount * PlayerMgr.Inst.ItemCtrller.relic_FollowObj_FloorInterest.RelicCfg.int1.result) / 100f);
				a = Mathf.Min(a, PlayerMgr.Inst.ItemCtrller.relic_FollowObj_FloorInterest.RelicCfg.int2.result);
				if (a > 0)
				{
					PlayerMgr.Inst.ChangeCoin(a);
					ObjPoolMgr.Inst.GetUIGO("Prefabs/UI/UITextFloat").GetComponent<UITextFloat>().Initialize("+" + a, UITextFloatType.GetCoin, PlayerMgr.Inst.ItemCtrller.relic_FollowObj_FloorInterest.transform.position);
				}
			}
			float num3 = DataMgr.selectedWorldData.GetTalentEnterDoorRecoveryValue();
			if (PlayerMgr.Inst.ItemCtrller.relicCfg_FloorRecovery != null)
			{
				num3 += (float)PlayerMgr.Inst.ItemCtrller.relicCfg_FloorRecovery.int1.result;
			}
			if (num3 > 0f && componentData.unitCfg.maxHP != componentData.unitCfg.currentHP)
			{
				if (componentData.unitCfg.maxHP - componentData.unitCfg.currentHP < num3)
				{
					num3 = componentData.unitCfg.maxHP - componentData.unitCfg.currentHP;
				}
				UnitDotsSyncSystem.UnitRecoveryHP(PlayerMgr.Inst.PlayerEtt, num3, ettMgr);
			}
			if (PlayerMgr.Inst.ItemCtrller.relicCfg_TempShield != null)
			{
				PlayerMgr.Inst.ChangeShieldTemp(PlayerMgr.Inst.ItemCtrller.relicCfg_TempShield.int1.result);
			}
			if (PlayerMgr.Inst.ItemCtrller.relicCfg_EnterDoorRemoveCurse != null && PlayerMgr.Inst.BaData.curseIDs.Count > 0 && UnityEngine.Random.value <= (float)PlayerMgr.Inst.ItemCtrller.relicCfg_EnterDoorRemoveCurse.int1.result / 100f)
			{
				PlayerMgr.Inst.ItemCtrller.CurseRemoveByIndex(UnityEngine.Random.Range(0, PlayerMgr.Inst.BaData.curseIDs.Count));
				PlayerMgr.Inst.ItemCtrller.relicCfg_EnterDoorRemoveCurse.intTimer++;
			}
		}
		if (PlayerMgr.Inst.ItemCtrller.curseCfg_Bled != null)
		{
			TakeDamageInfo_Dots elem = TakeDamageInfo_Dots.NewInfo(AttackerType.NothingSpecial);
			elem.damage = PlayerMgr.Inst.ItemCtrller.curseCfg_Bled.int1.result;
			elem.ignorePlayerInvincibleFrame = true;
			elem.ignoreRelicDodge = true;
			elem.ignoreRelicOrCurseDamageRatioChange = true;
			elem.ignoreUmbrella = true;
			elem.ignorePostSlotSpellTakeDamageTrigger = true;
			ettMgr.GetBuffer<TakeDamageInfo_Dots>(PlayerMgr.Inst.PlayerEtt).Add(elem);
		}
		if (PlayerMgr.Inst.ItemCtrller.curseCfg_EnterDoorLoseCoin != null && PlayerMgr.Inst.CoinCount > 0)
		{
			int num4 = PlayerMgr.Inst.ItemCtrller.curseCfg_EnterDoorLoseCoin.int1.result;
			if (num4 > PlayerMgr.Inst.CoinCount)
			{
				num4 = PlayerMgr.Inst.CoinCount;
			}
			PlayerMgr.Inst.ChangeCoin(-num4);
			QuickCreateSystem.Inst.CreateTextFloatVFX(-num4, UITextFloatType.DropCoin, PlayerMgr.Inst.PlayerPoint + new Vector3(0f, 0.5f, 0f));
		}
	}

	private void ProcessPlayerEnterDoorAfterEffects()
	{
		if (PlayerMgr.Inst.ItemCtrller.potion_HoverEFGO != null)
		{
			UnityEngine.Object.Destroy(PlayerMgr.Inst.ItemCtrller.potion_HoverEFGO);
			PlayerMgr.Inst.FlyUnregister();
		}
		if ((bool)PlayerMgr.Inst.ItemCtrller.potion_Stomachache)
		{
			PlayerMgr.Inst.ItemCtrller.potion_Stomachache.DestroySelf();
		}
		if (PlayerMgr.Inst.ItemCtrller.relicCfg_PandorasBox != null)
		{
			for (int num = PlayerMgr.Inst.ItemCtrller.relicCfg_PandorasBox.int1.result - 1; num >= 0; num--)
			{
				if (num < PlayerMgr.Inst.BaData.bagSpellDatas.Count)
				{
					SlotData slotData = PlayerMgr.Inst.BaData.bagSpellDatas[num];
					if (slotData != null && slotData != null && !slotData.isSealSlot)
					{
						SpellConfig spellConfig = SpellConfig.dic[slotData.id];
						if (spellConfig.dropType != ItemDropType.Special)
						{
							int num2 = 0;
							num2 = ((spellConfig.abilityType != SpellAbilityType.DeathAdder) ? PlayerMgr.Inst.BaData.GetSpellFromPool(spellConfig.level, spellConfig.dropType, spellConfig.id) : PlayerMgr.Inst.BaData.GetSpellFromPool(1, spellConfig.dropType, 10171));
							SlotData slotData2 = new SlotData(num2);
							PlayerMgr.Inst.BagSpellChange(num, null);
							if (PlayerMgr.Inst.CanBagSpellChange(num, slotData2))
							{
								PlayerMgr.Inst.BagSpellChange(num, slotData2);
							}
							else
							{
								PlayerMgr.Inst.SpawnSpellToGround(slotData2);
							}
							if (UIPlayerDataMgr.Inst.IsBagOpen)
							{
								Vector3 position = UIPlayerDataMgr.Inst.rtsf_BagSpell.GetChild(num).transform.position;
								ObjPoolMgr.Inst.GetUIGO("Prefabs/Item/Potion_WhiteSmoke_UI", position, 2f);
							}
							DataMgr.selectedWorldData.GalleryUnlock(GalleryCategory.Spell, slotData2.id);
						}
					}
				}
			}
		}
		if (PlayerMgr.Inst.ItemCtrller.relicGroupConfigs.TryGetValue(4, out var value))
		{
			if (PlayerMgr.Inst.ItemCtrller.potion_Invincible == null)
			{
				PlayerMgr.Inst.ItemCtrller.potion_Invincible = UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Item/Potion_Invincible"), PlayerMgr.Inst.PlayerPoint, Quaternion.identity, PlayerMgr.Inst.PlayerT).GetComponent<Potion_Invincible>();
			}
			PlayerMgr.Inst.ItemCtrller.potion_Invincible.Initialize(value.int1.result);
		}
		if (PlayerMgr.Inst.ItemCtrller.uiRelic_LightArmor != null)
		{
			PlayerMgr.Inst.ItemCtrller.uiRelic_LightArmor.FullFill();
		}
		if (PlayerMgr.Inst.ItemCtrller.uiRelic_WarmSnow != null)
		{
			PlayerMgr.Inst.ItemCtrller.uiRelic_WarmSnow.FullFill();
		}
		if (PlayerMgr.Inst.ItemCtrller.relic_Huang != null)
		{
			PlayerMgr.Inst.ItemCtrller.relic_Huang.UIRelicHuang.FullFill();
		}
		if (PlayerMgr.Inst.ItemCtrller.relic_Reaper != null)
		{
			PlayerMgr.Inst.ItemCtrller.relic_Reaper.CompoundSpell();
		}
		if (PlayerMgr.Inst.ItemCtrller.relic_InvisibleWing != null)
		{
			PlayerMgr.Inst.ItemCtrller.relic_InvisibleWing.EnterDoor();
		}
		if (PlayerMgr.Inst.ItemCtrller.curse_RandomCurseCommon != null)
		{
			PlayerMgr.Inst.ItemCtrller.curse_RandomCurseCommon.RerollCurse();
		}
		if (PlayerMgr.Inst.ItemCtrller.curse_RandomCurseRare != null)
		{
			PlayerMgr.Inst.ItemCtrller.curse_RandomCurseRare.RerollCurse();
		}
		if (PlayerMgr.Inst.ItemCtrller.curse_CantShootEnterRoom != null)
		{
			PlayerMgr.Inst.ItemCtrller.curse_CantShootEnterRoom.EnterDoor();
		}
		if (PlayerMgr.Inst.ItemCtrller.curse_SnailHunt != null)
		{
			PlayerMgr.Inst.ItemCtrller.curse_SnailHunt.EnterDoor();
		}
		if (PlayerMgr.Inst.ItemCtrller.curse_Stealthy != null)
		{
			PlayerMgr.Inst.ItemCtrller.curse_Stealthy.EnterDoor();
		}
	}

	private void PlayerEnterDoor2()
	{
		StartCoroutine(PlayerEnterDoor2IE(showChapterName: false));
	}

	private void PlayerEnterDoor2ShowChapterName()
	{
		StartCoroutine(PlayerEnterDoor2IE(showChapterName: true));
	}

	private IEnumerator PlayerEnterDoor2IE(bool showChapterName)
	{
		ProcessPlayerEnterDoorAfterEffects();
		PlayerMgr.Inst.PlayerCtrller.StartMotion();
		PlayerMgr.Inst.FlyUnregisterWithAllMate();
		PlayerMgr.Inst.InvincibleUnregister();
		PlayerMgr.Inst.PlayerCtrller.ClearDashOverheat();
		GeneralTool.SyncTeammatesPosition(PlayerMgr.Inst.PlayerPointIgnoreZ);
		GeneralTool.UpdateThroughMapTeammatesData();
		if (showChapterName)
		{
			yield return new WaitForSeconds(0.5f);
			PlaceNameType nameType = PlaceNameType.Camp;
			switch (CurrentStage)
			{
			case 3:
				nameType = PlaceNameType.Chapter2;
				break;
			case 5:
				nameType = PlaceNameType.Chapter3;
				break;
			case 7:
				nameType = PlaceNameType.Chapter4;
				break;
			case 9:
				nameType = PlaceNameType.Chapter5;
				break;
			case 300:
				nameType = PlaceNameType.Endless;
				break;
			default:
				Debug.LogError(CurrentStage);
				break;
			}
			UIPlaceNameMgr.Inst.Show(nameType);
			if (DataMgr.selectedWorldData.IsDave)
			{
				PlayerMgr.Inst.UpdateSkin();
			}
		}
	}
}
