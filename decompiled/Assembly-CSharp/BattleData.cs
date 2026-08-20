using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleData
{
	public UnitConfig playerCfg;

	public float bodySize = 1f;

	public int coinCount;

	public int keyCount;

	public int wandMaxCount = 1;

	public int bagCount = 10;

	public int potionMaxCount = 1;

	public int mpMax;

	public float mpRecovery;

	public float extraMoveSpeedRatio;

	public float extraDamageRatio;

	public float extraCriticalChance;

	public List<int> spellDisableCurrentBattle = new List<int>();

	public int currentLevel;

	public int currentStage = 1;

	public int currentRoomID;

	public LevelRewardType currentRewardType = LevelRewardType.None;

	public List<LevelRewardType> nextRewardTypes;

	public LevelRewardType nextExtraDoorRewardType = LevelRewardType.None;

	public int continuousNonSpell = 1;

	public int continuousNonRelic;

	public int continuousNonCoin;

	public int continuousNonProcess;

	public int continuousNonStore;

	public int continuousNonSpring;

	public List<int> specialRoomLevels = new List<int>();

	public bool shortcutRoomAppearedStage1;

	public bool shortcutRoomAppearedStage2;

	public bool shortcutRoomAppearedStage3;

	public bool shortcutRoomAppearedStage4;

	public bool shortcutRoomAppearedStage5;

	public bool shortcutRoomAppearedStage6;

	public bool isDestructible10Comfirm;

	public bool isMeetProducer;

	public List<int> endlessBossIDPool = new List<int>();

	public List<int> endlessEliteAppearLevel = new List<int>();

	public List<int> endlessMonsterIDs = new List<int>();

	public List<int> endlessExtraProcessorLevel = new List<int>();

	public List<WandConfig> wandCfgs = new List<WandConfig>();

	public List<SlotData> bagSpellDatas = new List<SlotData>();

	public List<RelicConfig> relicCfgs = new List<RelicConfig>();

	public List<int> potionIDs = new List<int>();

	public List<int> curseIDs = new List<int>();

	public List<int> curseLevels = new List<int>();

	public int relicBlockSpellCount;

	public Dictionary<int, int> lockSpellIDs = new Dictionary<int, int>();

	public List<int> poolOfRoomID = new List<int>();

	public List<int> poolOfWandID = new List<int>();

	public List<int> poolOfSpellID = new List<int>();

	public List<int> poolOfPotionID = new List<int>();

	public Dictionary<int, int> poolOfRelic = new Dictionary<int, int>();

	public Dictionary<int, int> poolOfCurse = new Dictionary<int, int>();

	public bool isSpellPoolHaveCommon = true;

	public bool isSpellPoolHaveRare = true;

	public bool isSpellPoolHaveEpic = true;

	public bool isSpellPoolHaveSpecial = true;

	public DamageRecorder damageRecorderV2 = new DamageRecorder();

	public void Initialize()
	{
		playerCfg = UnitConfig.map[800001];
		if (DataMgr.selectedWorldData.levelOfWandLimit > 0)
		{
			wandMaxCount += ScriptableObjMgr.Inst.talentUpgrade2.wandLimit[DataMgr.selectedWorldData.levelOfWandLimit - 1].value;
		}
		if (DataMgr.selectedWorldData.levelOfBagLimit > 0)
		{
			bagCount += ScriptableObjMgr.Inst.talentUpgrade2.bagLimit[DataMgr.selectedWorldData.levelOfBagLimit - 1].value;
		}
		if (DataMgr.selectedWorldData.levelOfMaxHP > 0)
		{
			playerCfg.maxHP += ScriptableObjMgr.Inst.talentUpgrade2.maxHP[DataMgr.selectedWorldData.levelOfMaxHP - 1].value;
		}
		playerCfg.currentHP = playerCfg.maxHP;
		if (DataMgr.selectedWorldData.levelOfInitialCoin > 0)
		{
			coinCount += ScriptableObjMgr.Inst.talentUpgrade2.initialCoin[DataMgr.selectedWorldData.levelOfInitialCoin - 1].value;
		}
		if (DataMgr.selectedWorldData.levelOfMaxMP > 0)
		{
			mpMax += ScriptableObjMgr.Inst.talentUpgrade2.maxMP[DataMgr.selectedWorldData.levelOfMaxMP - 1].value;
		}
		if (DataMgr.selectedWorldData.levelOfMPRecover > 0)
		{
			mpRecovery += ScriptableObjMgr.Inst.talentUpgrade2.mpRecover[DataMgr.selectedWorldData.levelOfMPRecover - 1].value;
		}
		DataMgr.selectedWorldData.CalculateAddingPoints();
		potionMaxCount += DataMgr.selectedWorldData.GetResearchValueConsiderActive(ResearchAbilityType.PotionLimit);
		keyCount += DataMgr.selectedWorldData.GetResearchValueConsiderActive(ResearchAbilityType.KeyChain);
		for (int i = 0; i < wandMaxCount; i++)
		{
			wandCfgs.Add(null);
		}
		for (int j = 0; j < bagCount; j++)
		{
			bagSpellDatas.Add(null);
		}
		for (int k = 0; k < potionMaxCount; k++)
		{
			potionIDs.Add(0);
		}
		InitialPools();
		if (DataMgr.selectedWorldData.GetSelectedSetCfg().WandIDs.Length != 0)
		{
			wandCfgs[0] = WandConfig.GetConfig(DataMgr.selectedWorldData.GetCurrentSetWandID());
		}
		int currentSetRelicID = DataMgr.selectedWorldData.GetCurrentSetRelicID();
		int currentSetRelicLevel = DataMgr.selectedWorldData.GetCurrentSetRelicLevel();
		if (currentSetRelicID == 0 || !poolOfRelic.ContainsKey(currentSetRelicID))
		{
			return;
		}
		for (int l = 0; l < currentSetRelicLevel; l++)
		{
			poolOfRelic[currentSetRelicID]--;
			if (poolOfRelic[currentSetRelicID] == 0)
			{
				poolOfRelic.Remove(currentSetRelicID);
				break;
			}
		}
	}

	public void InitialPools()
	{
		poolOfRoomID.Clear();
		poolOfWandID.Clear();
		poolOfSpellID.Clear();
		poolOfRelic.Clear();
		poolOfPotionID.Clear();
		poolOfCurse.Clear();
		for (int i = 0; i < RoomConfig.list.Count; i++)
		{
			poolOfRoomID.Add(RoomConfig.list[i].id);
		}
		for (int j = 0; j < PotionConfig.list.Count; j++)
		{
			if (PotionConfig.list[j].id != 3)
			{
				poolOfPotionID.Add(PotionConfig.list[j].id);
			}
		}
		for (int k = 0; k < CurseConfig.list.Count; k++)
		{
			if (CurseConfig.list[k].abilityType != CurseAbilityType.LostSpellOption || DataMgr.selectedWorldData.ActivateGirlHave4Pick2())
			{
				poolOfCurse.Add(CurseConfig.list[k].id, CurseConfig.list[k].count);
			}
		}
		for (int l = 0; l < WandConfig.list.Count; l++)
		{
			if ((DataMgr.selectedWorldData.enterBattleTime > 1 || WandConfig.list[l].postSlots.Length == 0) && (DataMgr.selectedWorldData.levelOfWandLimit != 0 || DataMgr.selectedWorldData.selectedSetID == 5 || (WandConfig.list[l].specialAbility != WandAbility.HoldWandDifferentManaGenRatio && WandConfig.list[l].specialAbility != WandAbility.AllWandMpCostRatio && WandConfig.list[l].specialAbility != WandAbility.AllWandMaxMpRatio && WandConfig.list[l].specialAbility != WandAbility.AllWandEffectRadiuRatio && WandConfig.list[l].specialAbility != WandAbility.AllWandMpGainAmountFix && WandConfig.list[l].specialAbility != WandAbility.Battery && WandConfig.list[l].id != 1009)))
			{
				poolOfWandID.Add(WandConfig.list[l].id);
			}
		}
		List<int> list = new List<int>();
		List<int> list2 = new List<int>();
		for (int m = 0; m < DataMgr.selectedWorldData.activateGirlActivatedIDs2.Count; m++)
		{
			if (!ActivateGirlConfig.dic.ContainsKey(DataMgr.selectedWorldData.activateGirlActivatedIDs2[m]))
			{
				continue;
			}
			ActivateGirlConfig activateGirlConfig = ActivateGirlConfig.dic[DataMgr.selectedWorldData.activateGirlActivatedIDs2[m]];
			if (activateGirlConfig.specialType == ActivateGirlSpecialType.None)
			{
				for (int n = 0; n < activateGirlConfig.spellIDs.Length; n++)
				{
					list.Add(activateGirlConfig.spellIDs[n]);
				}
			}
			else if (activateGirlConfig.specialType == ActivateGirlSpecialType.Relic)
			{
				for (int num = 0; num < activateGirlConfig.spellIDs.Length; num++)
				{
					list2.Add(activateGirlConfig.spellIDs[num]);
				}
			}
		}
		for (int num2 = 0; num2 < SpellConfig.list.Count; num2++)
		{
			if (SpellConfig.list[num2].dropType == ItemDropType.None)
			{
				continue;
			}
			if (SpellConfig.list[num2].needActivate)
			{
				int item = SpellConfig.list[num2].id / 10 * 10 + 1;
				if (list.Contains(item))
				{
					poolOfSpellID.Add(SpellConfig.list[num2].id);
				}
			}
			else if (wandMaxCount != 1 || DataMgr.selectedWorldData.selectedSetID == 5 || SpellConfig.list[num2].abilityType != SpellAbilityType.EchoRune)
			{
				poolOfSpellID.Add(SpellConfig.list[num2].id);
			}
		}
		if (DataMgr.selectedWorldData.selectedSetID == 9)
		{
			poolOfSpellID.Add(10281);
			poolOfSpellID.Add(10282);
			poolOfSpellID.Add(10283);
		}
		else if (DataMgr.selectedWorldData.selectedSetID == 11)
		{
			poolOfSpellID.Add(10311);
			poolOfSpellID.Add(10312);
			poolOfSpellID.Add(10313);
		}
		for (int num3 = 0; num3 < RelicConfig.list.Count; num3++)
		{
			if (RelicConfig.list[num3].needActivate)
			{
				if (!GameConstManaged.SpecialRelicList.Contains(RelicConfig.list[num3].id) && list2.Contains(RelicConfig.list[num3].id))
				{
					poolOfRelic.Add(RelicConfig.list[num3].id, RelicConfig.list[num3].maxCount);
				}
			}
			else if ((RelicConfig.list[num3].abilityType != RelicAbilityType.PostSlotMoreEfficiency || DataMgr.selectedWorldData.enterBattleTime > 1) && !GameConstManaged.SpecialRelicList.Contains(RelicConfig.list[num3].id))
			{
				poolOfRelic.Add(RelicConfig.list[num3].id, RelicConfig.list[num3].maxCount);
			}
		}
		if ((ICJNOGPFMAM.MIFJADDOODN && DataMgr.selectedWorldData.selectedSetID == 6) || DataMgr.selectedWorldData.selectedSetID == 7 || DataMgr.selectedWorldData.selectedSetID == 9)
		{
			poolOfRelic.Add(76, RelicConfig.dic[76].maxCount);
		}
		if (ICJNOGPFMAM.MIFJADDOODN && DataMgr.selectedWorldData.activateGirlActivatedIDs2.Contains(1))
		{
			poolOfRelic.Add(72, RelicConfig.dic[72].maxCount);
		}
		if (ICJNOGPFMAM.MIFJADDOODN && DataMgr.selectedWorldData.enterBattleTime <= 1 && poolOfRelic.ContainsKey(69))
		{
			poolOfRelic.Remove(69);
		}
	}

	public void RandomSpecialRoom()
	{
		specialRoomLevels.Clear();
		if (currentStage <= 1)
		{
			return;
		}
		specialRoomLevels.Add(UnityEngine.Random.Range(1, BattleMgr.Inst.stageLevelsCount[currentStage - 1]));
		if (!(UnityEngine.Random.value <= 0.3333f))
		{
			return;
		}
		int num = 0;
		int item;
		do
		{
			num++;
			if (num > 100)
			{
				Debug.LogError("!");
				return;
			}
			item = UnityEngine.Random.Range(1, BattleMgr.Inst.stageLevelsCount[currentStage - 1]);
		}
		while (specialRoomLevels.Contains(item));
		specialRoomLevels.Add(item);
	}

	public RoomConfig GetEndlessRoomFromPool(RoomType type)
	{
		List<int> list = new List<int>();
		float num = BattleMgr.Inst.EndlessCurrentStage;
		while (list.Count == 0)
		{
			if (num < 0f)
			{
				Debug.LogError("没有测试关卡");
				return RoomConfig.dic[111];
			}
			for (int i = 0; i < poolOfRoomID.Count; i++)
			{
				if ((float)RoomConfig.dic[poolOfRoomID[i]].belongStage == 300f + num && RoomConfig.dic[poolOfRoomID[i]].type == type)
				{
					list.Add(poolOfRoomID[i]);
				}
			}
			num -= 1f;
		}
		return RoomConfig.GetConfig(list[UnityEngine.Random.Range(0, list.Count)]);
	}

	public RoomConfig GetTestRoomFromPool()
	{
		List<int> list = new List<int>();
		for (int i = 0; i < poolOfRoomID.Count; i++)
		{
			if (RoomConfig.dic[poolOfRoomID[i]].belongStage == BattleMgr.Inst.CurrentStage)
			{
				list.Add(poolOfRoomID[i]);
			}
		}
		if (list.Count == 0)
		{
			Debug.LogError("没有测试关卡");
			return RoomConfig.dic[111];
		}
		return RoomConfig.GetConfig(list[UnityEngine.Random.Range(0, list.Count)]);
	}

	public RoomConfig GetRoomFromPool(RoomType type, bool needLeftRightAccess, bool needRO, bool useDave = false)
	{
		if (type == RoomType.Boss && useDave)
		{
			switch (PlayerMgr.Inst.BaData.currentStage)
			{
			case 2:
				return RoomConfig.GetConfig(21003);
			case 8:
				return RoomConfig.GetConfig(81002);
			case 10:
				return RoomConfig.GetConfig(101002);
			}
		}
		List<int> list = new List<int>();
		int num = BattleMgr.Inst.CurrentStage;
		for (int i = 0; i < poolOfRoomID.Count; i++)
		{
			RoomConfig roomConfig = RoomConfig.dic[poolOfRoomID[i]];
			if ((needLeftRightAccess && roomConfig.accessLeft == Vector2Data.Up1000 && roomConfig.accessRight == Vector2Data.Up1000) || (needRO && !roomConfig.haveRO) || (PlayerMgr.Inst.ItemCtrller.relic_ExtraDoor && type == RoomType.Monster && roomConfig.extraDoor == Vector2Data.Up1000))
			{
				continue;
			}
			if (roomConfig.belongStage == num)
			{
				if (roomConfig.type == type)
				{
					list.Add(poolOfRoomID[i]);
				}
			}
			else if (roomConfig.belongStage + 1 == num && type == RoomType.Puzzle && roomConfig.type == type)
			{
				list.Add(poolOfRoomID[i]);
			}
		}
		if (list.Count == 0)
		{
			Debug.LogError("关卡不足!  stage:" + num + "\t是否需要左右通道:" + needLeftRightAccess + "\t指定的关卡类型：" + type.ToString() + "\t是否需要LRO：" + needRO);
			return RoomConfig.dic[111];
		}
		int num2 = list[UnityEngine.Random.Range(0, list.Count)];
		while (num2 == 1026 && !ICJNOGPFMAM.GGPJCCLPBJL)
		{
			num2 = list[UnityEngine.Random.Range(0, list.Count)];
		}
		poolOfRoomID.Remove(num2);
		return RoomConfig.GetConfig(num2);
	}

	public int GetSpellFromPool(int level, ItemDropType dropType, int exceptID = 0)
	{
		List<int> list = new List<int>();
		for (int i = 0; i < poolOfSpellID.Count; i++)
		{
			SpellConfig spellConfig = SpellConfig.dic[poolOfSpellID[i]];
			if (spellConfig.level == level && spellConfig.dropType == dropType && spellConfig.id != exceptID)
			{
				list.Add(spellConfig.id);
			}
		}
		if (list.Count == 0)
		{
			switch (dropType)
			{
			case ItemDropType.Common:
				if (isSpellPoolHaveRare)
				{
					return GetSpellFromPool(level, ItemDropType.Rare);
				}
				if (isSpellPoolHaveEpic)
				{
					return GetSpellFromPool(1, ItemDropType.Epic);
				}
				return 10011;
			case ItemDropType.Rare:
				if (isSpellPoolHaveCommon)
				{
					return GetSpellFromPool(level, ItemDropType.Common);
				}
				if (isSpellPoolHaveEpic)
				{
					return GetSpellFromPool(1, ItemDropType.Epic);
				}
				return 10011;
			case ItemDropType.Epic:
				if (isSpellPoolHaveRare)
				{
					return GetSpellFromPool(level, ItemDropType.Rare);
				}
				if (isSpellPoolHaveCommon)
				{
					return GetSpellFromPool(1, ItemDropType.Common);
				}
				return 10011;
			case ItemDropType.Special:
				if (isSpellPoolHaveCommon)
				{
					return GetSpellFromPool(1, ItemDropType.Common);
				}
				return 10011;
			default:
				Debug.LogError("奇怪的掉落类型 " + dropType);
				return 10011;
			}
		}
		return list[UnityEngine.Random.Range(0, list.Count)];
	}

	public void RemoveSpellAllLevelFromPool(int id)
	{
		for (int num = poolOfSpellID.Count - 1; num >= 0; num--)
		{
			if (SpellConfig.dic[poolOfSpellID[num]].abilityType == SpellConfig.dic[id].abilityType)
			{
				poolOfSpellID.RemoveAt(num);
			}
		}
	}

	public void AddSpellAllLevelToPool(int id)
	{
		int num = id / 10 * 10 + 1;
		for (int i = num; i <= num + 8; i++)
		{
			if (SpellConfig.dic.ContainsKey(i))
			{
				poolOfSpellID.Add(i);
			}
		}
	}

	public void CheckSpellPoolHaveRarity()
	{
		isSpellPoolHaveCommon = false;
		isSpellPoolHaveRare = false;
		isSpellPoolHaveEpic = false;
		isSpellPoolHaveSpecial = false;
		for (int i = 0; i < poolOfSpellID.Count; i++)
		{
			if (SpellConfig.dic[poolOfSpellID[i]].dropType == ItemDropType.Common)
			{
				isSpellPoolHaveCommon = true;
			}
			else if (SpellConfig.dic[poolOfSpellID[i]].dropType == ItemDropType.Rare)
			{
				isSpellPoolHaveRare = true;
			}
			else if (SpellConfig.dic[poolOfSpellID[i]].dropType == ItemDropType.Epic)
			{
				isSpellPoolHaveEpic = true;
			}
			else if (SpellConfig.dic[poolOfSpellID[i]].dropType == ItemDropType.Special)
			{
				isSpellPoolHaveSpecial = true;
			}
			if (isSpellPoolHaveCommon && isSpellPoolHaveRare && isSpellPoolHaveEpic && isSpellPoolHaveSpecial)
			{
				break;
			}
		}
	}

	public int GetWandFromPool(int dropStage, params int[] exceptID)
	{
		List<int> list = new List<int>();
		for (int i = 0; i < poolOfWandID.Count; i++)
		{
			if (WandConfig.dic[poolOfWandID[i]].dropStage == dropStage && !exceptID.Contains(WandConfig.dic[poolOfWandID[i]].id))
			{
				list.Add(poolOfWandID[i]);
			}
		}
		if (list.Count == 0)
		{
			Debug.LogError("not enough Wand, drop stage:" + dropStage);
			return 1;
		}
		int num = list[UnityEngine.Random.Range(0, list.Count)];
		poolOfWandID.Remove(num);
		return num;
	}

	public void RemoveWandFromPool(int id)
	{
		poolOfWandID.Remove(id);
	}

	public void BackWandToPool(int id)
	{
		poolOfWandID.Add(id);
	}

	public void RandomSpellWandCheck()
	{
		for (int i = 0; i < WandConfig.list.Count; i++)
		{
			if (WandConfig.list[i].specialAbility == WandAbility.RandomCommonSpells)
			{
				SlotData[] normalSlots = WandConfig.list[i].normalSlots;
				Array.Fill(normalSlots, null);
				bool[] normalSlotIsLock = WandConfig.list[i].normalSlotIsLock;
				Array.Fill(normalSlotIsLock, value: false);
				for (int j = 0; j < normalSlots.Length; j++)
				{
					if (j >= WandConfig.list[i].int1 || (normalSlots[j] != null && normalSlots[j].isSealSlot) || normalSlotIsLock[j])
					{
						continue;
					}
					int num = 0;
					while (true)
					{
						if (num++ > 200)
						{
							Debug.LogError("死循环？!!!!");
							break;
						}
						SlotData data = new SlotData(GetSpellFromPool(1, ItemDropType.Common));
						if (normalSlots.Bag_CanSetSpell(normalSlotIsLock, data, j))
						{
							normalSlots.Bag_SetSlot(normalSlotIsLock, data, j);
							break;
						}
					}
				}
			}
			else
			{
				if (WandConfig.list[i].specialAbility != WandAbility.RandomRareSpells)
				{
					continue;
				}
				SlotData[] normalSlots2 = WandConfig.list[i].normalSlots;
				Array.Fill(normalSlots2, null);
				bool[] normalSlotIsLock2 = WandConfig.list[i].normalSlotIsLock;
				Array.Fill(normalSlotIsLock2, value: false);
				for (int k = 0; k < normalSlots2.Length; k++)
				{
					if (k >= WandConfig.list[i].int1 || (normalSlots2[k] != null && normalSlots2[k].isSealSlot) || normalSlotIsLock2[k])
					{
						continue;
					}
					int num2 = 0;
					while (true)
					{
						if (num2++ > 200)
						{
							Debug.LogError("死循环？!!!!");
							break;
						}
						SlotData data2 = new SlotData(GetSpellFromPool(1, ItemDropType.Rare));
						if (normalSlots2.Bag_CanSetSpell(normalSlotIsLock2, data2, k))
						{
							normalSlots2.Bag_SetSlot(normalSlotIsLock2, data2, k);
							break;
						}
					}
				}
			}
		}
	}

	public int GetRelicFromPool(ItemDropType dropType, int[] exceptIDs = null)
	{
		List<int> list = new List<int>();
		foreach (KeyValuePair<int, int> item in poolOfRelic)
		{
			if (RelicConfig.dic[item.Key].dropType == dropType && (exceptIDs == null || !exceptIDs.Contains(item.Key)) && (!GameMgr.InEndlessMode || RelicConfig.dic[item.Key].InEndlessMode))
			{
				list.Add(item.Key);
			}
		}
		if (list.Count == 0)
		{
			Debug.LogError("not enough Relic,DropType:" + dropType);
			return 999;
		}
		int num = list[UnityEngine.Random.Range(0, list.Count)];
		if (poolOfRelic[num] > 1)
		{
			poolOfRelic[num]--;
		}
		else if (poolOfRelic[num] == 1)
		{
			poolOfRelic.Remove(num);
		}
		else
		{
			Debug.LogError("!");
		}
		return num;
	}

	public void RemoveRelicFromPool(int id)
	{
		if (!CampMgr.Inst)
		{
			if (!poolOfRelic.ContainsKey(id))
			{
				Debug.LogError("没有指定的遗物ID：" + id);
			}
			else if (poolOfRelic[id] > 1)
			{
				poolOfRelic[id]--;
			}
			else if (poolOfRelic[id] == 1)
			{
				poolOfRelic.Remove(id);
			}
		}
	}

	public void BackRelicToPool(int id, int level)
	{
		if (poolOfRelic.ContainsKey(id))
		{
			poolOfRelic[id] += level;
		}
		else
		{
			poolOfRelic.Add(id, level);
		}
	}

	public int GetPotionFromPool(int exceptID = 0)
	{
		if (GameMgr.InEndlessMode)
		{
			List<int> list = new List<int>();
			for (int i = 0; i < poolOfPotionID.Count; i++)
			{
				if (PotionConfig.dic[poolOfPotionID[i]].InEndlessMode)
				{
					list.Add(poolOfPotionID[i]);
				}
			}
			return list[UnityEngine.Random.Range(0, list.Count)];
		}
		if (exceptID == 3)
		{
			return poolOfPotionID[UnityEngine.Random.Range(0, poolOfPotionID.Count)];
		}
		if (UnityEngine.Random.value <= 0.05f)
		{
			return 3;
		}
		List<int> list2 = new List<int>();
		for (int j = 0; j < poolOfPotionID.Count; j++)
		{
			if (poolOfPotionID[j] != exceptID)
			{
				list2.Add(poolOfPotionID[j]);
			}
		}
		return list2[UnityEngine.Random.Range(0, list2.Count)];
	}

	public int GetCurseFromPool(ItemDropType dropType)
	{
		List<int> list = new List<int>();
		foreach (KeyValuePair<int, int> item in poolOfCurse)
		{
			if (CurseConfig.dic[item.Key].dropType == dropType)
			{
				list.Add(item.Key);
			}
		}
		if (list.Count == 0)
		{
			Debug.LogError("Not enough curse, drop type:" + dropType);
			return 999;
		}
		int num = list[UnityEngine.Random.Range(0, list.Count)];
		if (poolOfCurse[num] > 1)
		{
			poolOfCurse[num]--;
		}
		else if (poolOfCurse[num] == 1)
		{
			poolOfCurse.Remove(num);
		}
		else
		{
			Debug.LogError(poolOfCurse[num]);
		}
		return num;
	}

	public void BackCurseToPool(int id, int level)
	{
		if (poolOfCurse.ContainsKey(id))
		{
			poolOfCurse[id] += level;
		}
		else
		{
			poolOfCurse.Add(id, level);
		}
	}
}
