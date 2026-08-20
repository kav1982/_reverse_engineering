using System.Collections.Generic;
using UnityEngine;

public class OutputMgr
{
	public static int RewardMaxHPValue
	{
		get
		{
			int num = 20 + DataMgr.selectedWorldData.GetTalentHPRoomValue();
			if (PlayerMgr.Inst.ItemCtrller.relicCfg_MoreMaxHPOutput != null)
			{
				num += Mathf.CeilToInt((float)num * ((float)PlayerMgr.Inst.ItemCtrller.relicCfg_MoreMaxHPOutput.int1.result / 100f));
			}
			return num;
		}
	}

	public static int GetSpecialRoomSpell()
	{
		float value = Random.value;
		if (BattleMgr.Inst == null)
		{
			return 10011;
		}
		switch (BattleMgr.Inst.CurrentStage)
		{
		case 1:
		case 2:
			if (value <= 0.5f)
			{
				return PlayerMgr.Inst.BaData.GetSpellFromPool(2, ItemDropType.Common);
			}
			return PlayerMgr.Inst.BaData.GetSpellFromPool(1, ItemDropType.Rare);
		case 3:
		case 4:
			if (value <= 0.5f)
			{
				return PlayerMgr.Inst.BaData.GetSpellFromPool(2, ItemDropType.Rare);
			}
			return PlayerMgr.Inst.BaData.GetSpellFromPool(2, ItemDropType.Common);
		case 5:
		case 6:
			if (value <= 0.75f)
			{
				return PlayerMgr.Inst.BaData.GetSpellFromPool(2, ItemDropType.Rare);
			}
			return PlayerMgr.Inst.BaData.GetSpellFromPool(1, ItemDropType.Epic);
		case 7:
		case 8:
			if (value <= 0.5f)
			{
				return PlayerMgr.Inst.BaData.GetSpellFromPool(3, ItemDropType.Common);
			}
			return PlayerMgr.Inst.BaData.GetSpellFromPool(1, ItemDropType.Epic);
		case 9:
		case 10:
			return PlayerMgr.Inst.BaData.GetSpellFromPool(1, ItemDropType.Epic);
		default:
			return PlayerMgr.Inst.BaData.GetSpellFromPool(2, ItemDropType.Common);
		}
	}

	public static int GetCrystalID()
	{
		switch (DataMgr.selectedWorldData.selectedDifficulty)
		{
		case DifficultyType.Easy:
			return 101;
		case DifficultyType.Normal:
			if (Random.value <= 0.4f)
			{
				return 102;
			}
			return 101;
		case DifficultyType.Hard:
			if (Random.value <= 0.8f)
			{
				return 102;
			}
			return 101;
		case DifficultyType.Nightmare1:
			if (Random.value <= 0.2f)
			{
				return 103;
			}
			return 102;
		case DifficultyType.Nightmare2:
			if (Random.value <= 0.6f)
			{
				return 103;
			}
			return 102;
		case DifficultyType.Nightmare3:
			return 103;
		default:
			Debug.LogError(DataMgr.selectedWorldData.selectedDifficulty);
			return 101;
		}
	}

	public static int GetBloodID()
	{
		switch (DataMgr.selectedWorldData.selectedDifficulty)
		{
		case DifficultyType.Easy:
			return 111;
		case DifficultyType.Normal:
			if (Random.value <= 0.4f)
			{
				return 112;
			}
			return 111;
		case DifficultyType.Hard:
			if (Random.value <= 0.8f)
			{
				return 112;
			}
			return 111;
		case DifficultyType.Nightmare1:
			if (Random.value <= 0.2f)
			{
				return 113;
			}
			return 112;
		case DifficultyType.Nightmare2:
			if (Random.value <= 0.6f)
			{
				return 113;
			}
			return 112;
		case DifficultyType.Nightmare3:
			return 113;
		default:
			Debug.LogError(DataMgr.selectedWorldData.selectedDifficulty);
			return 111;
		}
	}

	public static int GetCoreID()
	{
		return 121;
	}

	public static ItemInfo GetChapter1Floor0()
	{
		ItemInfo result = default(ItemInfo);
		result.type = ItemType.Spell;
		if (DataMgr.selectedWorldData.enterBattleTime == 2)
		{
			result.id = 10111;
		}
		else if (Random.value <= 0.12f)
		{
			result.id = PlayerMgr.Inst.BaData.GetSpellFromPool(1, ItemDropType.Rare);
		}
		else
		{
			result.id = PlayerMgr.Inst.BaData.GetSpellFromPool(1, ItemDropType.Common);
		}
		return result;
	}

	public static ItemInfo GetRewardD1_T0()
	{
		float num = (float)DataMgr.selectedWorldData.GetResearchValueConsiderActive(ResearchAbilityType.SafeBox) / 100f;
		float value = Random.value;
		if (num != 0f && value <= num)
		{
			value = GeneralTool.GetWeightRandom(1f, 1f, 1f);
			if (value != 0f)
			{
				if (value != 1f)
				{
					if (value == 2f)
					{
						return new ItemInfo(ItemType.Resource, 12);
					}
					Debug.LogError(value);
					return new ItemInfo(ItemType.Resource, 31);
				}
				return new ItemInfo(ItemType.Resource, 21);
			}
			return new ItemInfo(ItemType.Resource, 32);
		}
		if (value <= num + 0.15f)
		{
			return new ItemInfo(ItemType.Resource, 11);
		}
		return default(ItemInfo);
	}

	public static ItemInfo GetRewardD4_T3()
	{
		float num = (float)DataMgr.selectedWorldData.GetResearchValueConsiderActive(ResearchAbilityType.WildBerry) / 100f;
		if (num != 0f && Random.value <= num)
		{
			return new ItemInfo(ItemType.Resource, 31);
		}
		return default(ItemInfo);
	}

	public static ItemInfo GetRewardD1_T3()
	{
		if (Random.value <= 0.15f)
		{
			if (BattleMgr.Inst != null && BattleMgr.Inst.CurrentStage >= 9)
			{
				return new ItemInfo(ItemType.Resource, 12);
			}
			return new ItemInfo(ItemType.Resource, 11);
		}
		return default(ItemInfo);
	}

	public static ItemInfo GetRewardD1_T10()
	{
		ItemInfo result = default(ItemInfo);
		if (Random.value <= 0.2f)
		{
			result = new ItemInfo(ItemType.Resource, 11);
		}
		return result;
	}

	public static List<ItemInfo> GetSO102ItemInfos(SpecialObj102Type so102Type, int stage)
	{
		List<ItemInfo> list = new List<ItemInfo>();
		int num = 0;
		while (true)
		{
			num++;
			if (num >= 100)
			{
				Debug.LogError("!");
				list.Add(new ItemInfo(ItemType.Relic, 999));
			}
			int num2;
			switch (so102Type)
			{
			case SpecialObj102Type.CurseRelic:
				num2 = PlayerMgr.Inst.BaData.GetRelicFromPool(ItemDropType.Rare);
				break;
			case SpecialObj102Type.BloodRelic:
				num2 = PlayerMgr.Inst.BaData.GetRelicFromPool(ItemDropType.Epic);
				break;
			default:
				Debug.LogError(so102Type);
				num2 = 999;
				break;
			}
			bool flag = false;
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].id == num2)
				{
					flag = true;
					PlayerMgr.Inst.BaData.BackRelicToPool(num2, 1);
					break;
				}
			}
			if (!flag)
			{
				list.Add(new ItemInfo(ItemType.Relic, num2));
			}
			switch (so102Type)
			{
			case SpecialObj102Type.CurseRelic:
				if (stage < 7)
				{
					if (list.Count < 2)
					{
						continue;
					}
				}
				else if (list.Count < 3)
				{
					continue;
				}
				break;
			case SpecialObj102Type.BloodRelic:
				if (list.Count < 3)
				{
					continue;
				}
				break;
			default:
				Debug.LogError(so102Type);
				continue;
			}
			break;
		}
		return list;
	}

	public static List<ItemInfo> GetEliteOrBossItemInfos()
	{
		List<ItemInfo> list = new List<ItemInfo>();
		list.Add(new ItemInfo(ItemType.Resource, 32));
		int num = Random.Range(10, 21);
		int num2 = 0;
		int num3 = 0;
		switch (BattleMgr.Inst.CurrentStage)
		{
		case 1:
			num2 = 1;
			list.Add(new ItemInfo(ItemType.Resource, 32));
			break;
		case 2:
			num += 10;
			num2 = 2;
			list.Add(new ItemInfo(ItemType.Resource, 33));
			break;
		case 3:
			num += 5;
			list.Add(new ItemInfo(ItemType.Resource, 32));
			break;
		case 4:
			num += 15;
			num2 = 5;
			list.Add(new ItemInfo(ItemType.Resource, 33));
			break;
		case 5:
			num += 10;
			list.Add(new ItemInfo(ItemType.Resource, 32));
			break;
		case 6:
			num += 20;
			num2 = 7;
			list.Add(new ItemInfo(ItemType.Resource, 33));
			if (DataMgr.selectedWorldData.storyKillChapter3BossPickup)
			{
				num3 = 1;
			}
			break;
		case 7:
			num += 15;
			list.Add(new ItemInfo(ItemType.Resource, 32));
			break;
		case 8:
			num += 25;
			num2 = 9;
			list.Add(new ItemInfo(ItemType.Resource, 33));
			if (DataMgr.selectedWorldData.storyHardBossDropPickup)
			{
				num3 = 2;
			}
			break;
		case 9:
			num += 20;
			num /= 3;
			list.Add(new ItemInfo(ItemType.Resource, 33));
			break;
		case 10:
			if (LevelMgr.Inst.CurrentRewardType == LevelRewardType.Elite)
			{
				num += 30;
				num /= 3;
				list.Add(new ItemInfo(ItemType.Resource, 33));
				break;
			}
			num += 30;
			num2 = 11;
			list.Add(new ItemInfo(ItemType.Resource, 33));
			if (DataMgr.selectedWorldData.storyFinishHardDropPickup)
			{
				num3 = 4;
			}
			break;
		default:
			Debug.LogError(BattleMgr.Inst.CurrentStage);
			break;
		}
		if (DataMgr.selectedWorldData.enterBattleTime == 1)
		{
			list.Clear();
		}
		for (int i = 0; i < num; i++)
		{
			list.Add(new ItemInfo(ItemType.Resource, GetCrystalID()));
		}
		for (int j = 0; j < num2; j++)
		{
			list.Add(new ItemInfo(ItemType.Resource, GetBloodID()));
		}
		for (int k = 0; k < num3; k++)
		{
			list.Add(new ItemInfo(ItemType.Resource, GetCoreID()));
		}
		return list;
	}

	public static List<ItemInfo> GetStore()
	{
		List<ItemInfo> list = new List<ItemInfo>();
		int num = 1;
		switch (PlayerMgr.Inst.BaData.currentStage)
		{
		case 1:
		case 2:
			num = 1;
			break;
		case 3:
		case 4:
			num = 2;
			break;
		case 5:
		case 6:
			num = 4;
			break;
		case 7:
		case 8:
			num = 6;
			break;
		case 9:
		case 10:
			num = 8;
			break;
		default:
			num = 1;
			break;
		}
		list.Add(new ItemInfo(ItemType.Wand, PlayerMgr.Inst.BaData.GetWandFromPool(num)));
		List<int> list2 = new List<int>();
		list2.Add(PlayerMgr.Inst.BaData.GetSpellFromPool(1, ItemDropType.Common));
		list2.Add(PlayerMgr.Inst.BaData.GetSpellFromPool(1, ItemDropType.Rare));
		if (Random.Range(0, 3) == 0)
		{
			list2.Add(PlayerMgr.Inst.BaData.GetSpellFromPool(1, ItemDropType.Rare));
		}
		else
		{
			list2.Add(PlayerMgr.Inst.BaData.GetSpellFromPool(1, ItemDropType.Common));
		}
		list2.Upset();
		for (int i = 0; i < list2.Count; i++)
		{
			list.Add(new ItemInfo(ItemType.Spell, list2[i]));
		}
		List<int> list3 = new List<int>();
		list3.Add((Random.Range(0, 2) == 0) ? 33 : 43);
		list3.Add((Random.Range(0, 7) == 0) ? 22 : 21);
		int num2 = Random.Range(0, 4);
		switch (num2)
		{
		case 0:
			list3.Add(32);
			break;
		case 1:
			list3.Add(33);
			break;
		case 2:
			list3.Add(42);
			break;
		case 3:
			list3.Add(43);
			break;
		default:
			Debug.LogError(num2);
			break;
		}
		list3.Upset();
		if (PlayerMgr.Inst.ItemCtrller.uiRelic_RuneWizard != null)
		{
			for (int j = 0; j < list3.Count - 1; j++)
			{
				list.Add(new ItemInfo(ItemType.Resource, list3[j]));
			}
			list.Add(new ItemInfo(ItemType.RuneWizardRune, 51));
		}
		else
		{
			for (int k = 0; k < list3.Count; k++)
			{
				list.Add(new ItemInfo(ItemType.Resource, list3[k]));
			}
		}
		return list;
	}

	public static List<ItemInfo> GetEndlessStore()
	{
		List<ItemInfo> list = new List<ItemInfo>();
		int num = Mathf.Clamp((BattleMgr.Inst.EndlessCurrentLevel - 1) / 5, 0, 4) * 2;
		if (num == 0)
		{
			num = 1;
		}
		list.Add(new ItemInfo(ItemType.Wand, PlayerMgr.Inst.BaData.GetWandFromPool(num)));
		float num2 = (DataMgr.selectedWorldData.IsDave ? 0.3f : 0f);
		float num3 = ((PlayerMgr.Inst.ItemCtrller.uiRelic_RuneWizard != null) ? 0.3f : 0f);
		int weightRandomCompletion = GeneralTool.GetWeightRandomCompletion(0.03f, num2, num3);
		int num4 = 0;
		switch (weightRandomCompletion)
		{
		case 0:
			num4 = PlayerMgr.Inst.BaData.GetRelicFromPool(ItemDropType.Epic);
			list.Add(new ItemInfo(ItemType.Relic, num4));
			break;
		case 1:
			list.Add(new ItemInfo(ItemType.Relic, GeneralTool.GetRandomEnhancedHarpoons()));
			break;
		case 2:
			list.Add(new ItemInfo(ItemType.RuneWizardRune, 51));
			break;
		case 3:
			num4 = ((!GeneralTool.ChanceResult(0.2f)) ? PlayerMgr.Inst.BaData.GetRelicFromPool(ItemDropType.Common) : PlayerMgr.Inst.BaData.GetRelicFromPool(ItemDropType.Rare));
			list.Add(new ItemInfo(ItemType.Relic, num4));
			break;
		}
		switch (GeneralTool.GetWeightRandomCompletion(0.2f, 0.2f))
		{
		case 0:
			list.Add(new ItemInfo(ItemType.MaxHp, 61));
			break;
		case 1:
			list.Add(new ItemInfo(ItemType.Potion, PlayerMgr.Inst.BaData.GetPotionFromPool()));
			break;
		case 2:
			list.Add(new ItemInfo(ItemType.Resource, (Random.Range(0, 2) == 0) ? 33 : 43));
			break;
		}
		List<int> list2 = new List<int>();
		float num5 = 0.03f;
		float num6 = 1.2f;
		for (int i = 0; i < 4; i++)
		{
			switch (GeneralTool.GetWeightRandomCompletion(num5, num6))
			{
			case 0:
				list2.Add(PlayerMgr.Inst.BaData.GetSpellFromPool(1, ItemDropType.Epic));
				break;
			case 1:
				list2.Add(PlayerMgr.Inst.BaData.GetSpellFromPool(GetEndlessSpellLevel(), ItemDropType.Rare));
				break;
			case 2:
				list2.Add(PlayerMgr.Inst.BaData.GetSpellFromPool(GetEndlessSpellLevel(), ItemDropType.Common));
				break;
			}
			num5 -= 1f;
			num5 = Mathf.Clamp01(num5);
			num6 -= 1f;
			num6 = Mathf.Clamp01(num6);
		}
		list2.Upset();
		for (int j = 0; j < list2.Count; j++)
		{
			list.Add(new ItemInfo(ItemType.Spell, list2[j]));
		}
		ItemInfo item = new ItemInfo(ItemType.MaxHp, 61);
		switch (GeneralTool.GetWeightRandomCompletion(0.2f, 0.2f, 0.2f, 0.2f, 0.2f))
		{
		case 0:
			num4 = ((!GeneralTool.ChanceResult(0.2f)) ? PlayerMgr.Inst.BaData.GetRelicFromPool(ItemDropType.Common) : PlayerMgr.Inst.BaData.GetRelicFromPool(ItemDropType.Rare));
			item = new ItemInfo(ItemType.Relic, num4);
			break;
		case 1:
			item = new ItemInfo(ItemType.Potion, PlayerMgr.Inst.BaData.GetPotionFromPool());
			break;
		case 2:
			item = new ItemInfo(ItemType.Resource, (Random.Range(0, 2) == 0) ? 33 : 43);
			break;
		case 3:
			item = new ItemInfo(ItemType.MaxHp, 61);
			break;
		case 4:
		{
			int num7 = 0;
			num7 = ((!GeneralTool.ChanceResult(0.3f)) ? PlayerMgr.Inst.BaData.GetSpellFromPool(GetEndlessSpellLevel(), ItemDropType.Common) : PlayerMgr.Inst.BaData.GetSpellFromPool(GetEndlessSpellLevel(), ItemDropType.Rare));
			item = new ItemInfo(ItemType.Spell, num7);
			break;
		}
		}
		if (DataMgr.selectedWorldData.endless_LevelOfGoodsExtraCount == 2)
		{
			list.Insert(3, item);
		}
		else
		{
			list.Add(item);
		}
		return list;
	}

	private static int GetEndlessSpellLevel()
	{
		int num = 1;
		if (GeneralTool.ChanceResult(DataMgr.selectedWorldData.GetEndlessHighLevelSpellChange() / (3.2f + (float)DataMgr.selectedWorldData.endless_LevelOfGoodsExtraCount)))
		{
			num++;
		}
		return num;
	}

	public static ItemInfo GetEndlessChestDrone()
	{
		return GeneralTool.GetWeightRandomCompletion(0.4f, 0.4f, 0.2f) switch
		{
			0 => new ItemInfo(ItemType.Resource, 33), 
			1 => new ItemInfo(ItemType.Resource, 43), 
			2 => new ItemInfo(ItemType.Potion, PlayerMgr.Inst.BaData.GetPotionFromPool()), 
			_ => new ItemInfo(ItemType.Resource, 33), 
		};
	}

	public static int GetEndlessEliteDrop(int currentWave)
	{
		float value = Random.value;
		if (BattleMgr.Inst == null)
		{
			return 10011;
		}
		Mathf.Clamp(Mathf.FloorToInt((float)currentWave / 5f), 0, 4);
		switch (BattleMgr.Inst.CurrentStage)
		{
		case 0:
			if (value <= 0.5f)
			{
				return PlayerMgr.Inst.BaData.GetSpellFromPool(2, ItemDropType.Common);
			}
			return PlayerMgr.Inst.BaData.GetSpellFromPool(1, ItemDropType.Rare);
		case 1:
			if (value <= 0.5f)
			{
				return PlayerMgr.Inst.BaData.GetSpellFromPool(2, ItemDropType.Rare);
			}
			return PlayerMgr.Inst.BaData.GetSpellFromPool(2, ItemDropType.Common);
		case 2:
			if (value <= 0.75f)
			{
				return PlayerMgr.Inst.BaData.GetSpellFromPool(2, ItemDropType.Rare);
			}
			return PlayerMgr.Inst.BaData.GetSpellFromPool(1, ItemDropType.Epic);
		case 3:
			if (value <= 0.5f)
			{
				return PlayerMgr.Inst.BaData.GetSpellFromPool(3, ItemDropType.Common);
			}
			return PlayerMgr.Inst.BaData.GetSpellFromPool(1, ItemDropType.Epic);
		case 4:
			return PlayerMgr.Inst.BaData.GetSpellFromPool(1, ItemDropType.Epic);
		default:
			return PlayerMgr.Inst.BaData.GetSpellFromPool(2, ItemDropType.Common);
		}
	}

	public static List<ItemInfo> GetStorePotion()
	{
		List<ItemInfo> list = new List<ItemInfo>();
		int num = 0;
		List<int> list2 = new List<int>();
		do
		{
			num++;
			if (num >= 200)
			{
				Debug.LogError("!");
				break;
			}
			int potionFromPool = PlayerMgr.Inst.BaData.GetPotionFromPool();
			if (!list2.Contains(potionFromPool))
			{
				list2.Add(potionFromPool);
				list.Add(new ItemInfo(ItemType.Potion, potionFromPool));
			}
		}
		while (list2.Count < 6);
		return list;
	}

	public static RoomConfig GetChapterRoom0()
	{
		return RoomConfig.GetConfig(GetChapterRoom0ID());
	}

	public static int GetChapterRoom0ID()
	{
		switch (PlayerMgr.Inst.BaData.currentStage)
		{
		case 1:
			return 1011;
		case 3:
			return 1012;
		case 5:
			return 1013;
		case 7:
			return 1014;
		case 9:
			return 1015;
		case 300:
			return 1016;
		default:
			Debug.LogError(PlayerMgr.Inst.BaData.currentStage);
			return 1011;
		}
	}

	public static RoomConfig GetBossRoom0(bool dave = false)
	{
		int num = 0;
		switch (PlayerMgr.Inst.BaData.currentStage)
		{
		case 2:
			num = (dave ? 1026 : 1021);
			break;
		case 4:
			num = 1022;
			break;
		case 6:
			num = 1023;
			break;
		case 8:
			num = (dave ? 1027 : 1024);
			break;
		case 10:
			num = (dave ? 1028 : 1025);
			break;
		default:
			num = 1025;
			Debug.LogError(PlayerMgr.Inst.BaData.currentStage);
			break;
		}
		return RoomConfig.GetConfig(num);
	}

	public static RoomConfig GetHideBossRoom(bool dave = false)
	{
		RoomConfig config;
		switch (PlayerMgr.Inst.BaData.currentStage)
		{
		case 2:
			config = RoomConfig.GetConfig(dave ? 1006 : 1001);
			break;
		case 4:
			config = RoomConfig.GetConfig(1002);
			break;
		case 6:
			config = RoomConfig.GetConfig(1003);
			break;
		case 8:
			config = RoomConfig.GetConfig(dave ? 1007 : 1004);
			break;
		case 10:
			config = RoomConfig.GetConfig(dave ? 1008 : 1005);
			break;
		default:
			config = RoomConfig.GetConfig(1001);
			Debug.LogError(PlayerMgr.Inst.BaData.currentStage);
			break;
		}
		return config;
	}

	public static RoomConfig GetSpringRoom(int stage)
	{
		int num = 0;
		switch (stage)
		{
		case 1:
		case 2:
			num = 231;
			break;
		case 3:
		case 4:
			num = 232;
			break;
		case 5:
		case 6:
			num = 233;
			break;
		case 7:
		case 8:
			num = 234;
			break;
		case 9:
		case 10:
			num = 235;
			break;
		default:
			num = 231;
			Debug.LogError(stage);
			break;
		}
		return RoomConfig.GetConfig(num);
	}

	public static RoomConfig GetStoreRoomCfg(int stage)
	{
		int id = 201;
		if (DataMgr.selectedWorldData.IsDave)
		{
			id = 225;
		}
		else if (GameMgr.CampSkinType == CampSkinType.Summer)
		{
			id = 209;
		}
		else
		{
			switch (stage)
			{
			case 5:
			case 6:
				id = 203;
				break;
			case 7:
			case 8:
				id = 205;
				break;
			case 9:
			case 10:
				id = 207;
				break;
			}
		}
		return RoomConfig.GetConfig(id);
	}

	public static RoomConfig GetPotionRoomCfg(int stage)
	{
		int id = 202;
		if (DataMgr.selectedWorldData.IsDave)
		{
			id = 226;
		}
		else if (GameMgr.CampSkinType == CampSkinType.Summer)
		{
			id = 210;
		}
		else
		{
			switch (stage)
			{
			case 5:
			case 6:
				id = 204;
				break;
			case 7:
			case 8:
				id = 206;
				break;
			case 9:
			case 10:
				id = 208;
				break;
			}
		}
		return RoomConfig.GetConfig(id);
	}

	public static RoomConfig GetProcessRoomCfg(int stage)
	{
		int id = 211;
		if (DataMgr.selectedWorldData.IsDave)
		{
			id = 224;
		}
		else if (GameMgr.CampSkinType == CampSkinType.Summer)
		{
			id = 219;
		}
		else
		{
			switch (stage)
			{
			case 5:
			case 6:
				id = 213;
				break;
			case 7:
			case 8:
				id = 215;
				break;
			case 9:
			case 10:
				id = 217;
				break;
			}
		}
		return RoomConfig.GetConfig(id);
	}

	public static RoomConfig GetMoreInOneRoomCfg(int stage)
	{
		int id = 212;
		if (DataMgr.selectedWorldData.IsDave)
		{
			id = 227;
		}
		else if (GameMgr.CampSkinType == CampSkinType.Summer)
		{
			id = 220;
		}
		else
		{
			switch (stage)
			{
			case 5:
			case 6:
				id = 214;
				break;
			case 7:
			case 8:
				id = 216;
				break;
			case 9:
			case 10:
				id = 218;
				break;
			}
		}
		return RoomConfig.GetConfig(id);
	}

	public static List<LevelRewardType> GetDoors()
	{
		List<LevelRewardType> list = new List<LevelRewardType>();
		if (ScriptableObjMgr.Inst.testCtrller.BattleRuinedDoor)
		{
			list.Add(LevelRewardType.Spell);
			list.Add(LevelRewardType.Ruined);
			list.Upset();
			return list;
		}
		if (BattleMgr.Inst.CurrentStage >= 50)
		{
			list.Add(LevelRewardType.Spell);
			list.Add(LevelRewardType.Relic);
		}
		else if (BattleMgr.Inst.CurrentStage == 1 && BattleMgr.Inst.CurrentLevel <= 1)
		{
			list.Add((LevelRewardType)Random.Range(1, 5));
			int num = 0;
			while (true)
			{
				num++;
				if (num > 100)
				{
					Debug.LogError("!");
					list.Add((LevelRewardType)Random.Range(1, 5));
					break;
				}
				LevelRewardType levelRewardType = (LevelRewardType)Random.Range(1, 5);
				if (levelRewardType != list[0])
				{
					list.Add(levelRewardType);
					break;
				}
			}
		}
		else if (BattleMgr.Inst.CurrentStage == 9)
		{
			switch (BattleMgr.Inst.CurrentLevel)
			{
			case 0:
			case 4:
				list.Add(LevelRewardType.Store);
				list.Add(LevelRewardType.Process);
				if (DataMgr.selectedWorldData.GetResearchValueConsiderActive(ResearchAbilityType.Spring) > 0)
				{
					list.Add(LevelRewardType.Spring);
				}
				break;
			case 1:
			case 2:
			case 3:
				list.Add(LevelRewardType.Elite);
				break;
			default:
				Debug.LogError(BattleMgr.Inst.CurrentLevel);
				break;
			}
		}
		else if (BattleMgr.Inst.CurrentStage == 10)
		{
			switch (BattleMgr.Inst.CurrentLevel)
			{
			case 1:
			case 2:
			case 3:
				list.Add(LevelRewardType.Elite);
				break;
			case 4:
				list.Add(LevelRewardType.Store);
				list.Add(LevelRewardType.Process);
				if (DataMgr.selectedWorldData.GetResearchValueConsiderActive(ResearchAbilityType.Spring) > 0)
				{
					list.Add(LevelRewardType.Spring);
				}
				break;
			case 5:
				list.Add(LevelRewardType.Boss);
				break;
			default:
				Debug.LogError(BattleMgr.Inst.CurrentLevel);
				break;
			case 6:
				break;
			}
		}
		else if (BattleMgr.Inst.CurrentLevel == BattleMgr.Inst.stageLevelsCount[BattleMgr.Inst.CurrentStage - 1] - 1)
		{
			if (BattleMgr.Inst.CurrentStage % 2 == 1)
			{
				list.Add(LevelRewardType.Elite);
			}
			else
			{
				list.Add(LevelRewardType.Boss);
			}
		}
		else if (DataMgr.selectedWorldData.selectedDifficulty == DifficultyType.Easy && BattleMgr.Inst.CurrentStage == 6 && BattleMgr.Inst.CurrentLevel == BattleMgr.Inst.stageLevelsCount[5] - 2)
		{
			list.Add(LevelRewardType.Store);
			list.Add(LevelRewardType.Process);
		}
		else if (DataMgr.selectedWorldData.selectedDifficulty == DifficultyType.Normal && BattleMgr.Inst.CurrentStage == 8 && BattleMgr.Inst.CurrentLevel == BattleMgr.Inst.stageLevelsCount[7] - 2)
		{
			list.Add(LevelRewardType.Store);
			list.Add(LevelRewardType.Process);
		}
		else if (BattleMgr.Inst.CurrentLevel == BattleMgr.Inst.stageLevelsCount[BattleMgr.Inst.CurrentStage - 1] && BattleMgr.Inst.CurrentStage % 2 == 0)
		{
			list.Add(LevelRewardType.Chapter);
		}
		else if (PlayerMgr.Inst.BaData.continuousNonSpell >= 5)
		{
			list.Add(LevelRewardType.Spell);
			list.Add(LevelRewardType.Ruined);
		}
		else if (PlayerMgr.Inst.BaData.continuousNonRelic >= 5)
		{
			list.Add(LevelRewardType.Relic);
			list.Add(LevelRewardType.Ruined);
		}
		else if (PlayerMgr.Inst.BaData.continuousNonCoin >= 5)
		{
			list.Add(LevelRewardType.Coin);
			list.Add(LevelRewardType.Ruined);
		}
		else if (PlayerMgr.Inst.BaData.continuousNonProcess >= 11)
		{
			list.Add(LevelRewardType.Process);
			list.Add((LevelRewardType)Random.Range(1, 5));
		}
		else if (PlayerMgr.Inst.BaData.continuousNonStore >= 11)
		{
			list.Add(LevelRewardType.Store);
			list.Add((LevelRewardType)Random.Range(1, 5));
		}
		else
		{
			int num2 = 0;
			while (true)
			{
				num2++;
				if (num2 > 100)
				{
					Debug.LogError("循环100次得到的门都不符合条件");
					list.Add(GetRandomLevelRewardType());
					break;
				}
				LevelRewardType randomLevelRewardType = GetRandomLevelRewardType();
				if ((randomLevelRewardType != LevelRewardType.Store || PlayerMgr.Inst.BaData.continuousNonStore != 0) && (randomLevelRewardType != LevelRewardType.Process || PlayerMgr.Inst.BaData.continuousNonProcess != 0) && (randomLevelRewardType != LevelRewardType.Spring || PlayerMgr.Inst.BaData.continuousNonSpring != 0))
				{
					list.Add(randomLevelRewardType);
					break;
				}
			}
			num2 = 0;
			while (true)
			{
				num2++;
				if (num2 > 100)
				{
					Debug.LogError("!");
					list.Add(GetRandomLevelRewardType());
					break;
				}
				LevelRewardType randomLevelRewardType2 = GetRandomLevelRewardType();
				if ((randomLevelRewardType2 != LevelRewardType.Store || PlayerMgr.Inst.BaData.continuousNonStore != 0) && (randomLevelRewardType2 != LevelRewardType.Process || PlayerMgr.Inst.BaData.continuousNonProcess != 0) && (randomLevelRewardType2 != LevelRewardType.Spring || PlayerMgr.Inst.BaData.continuousNonSpring != 0) && ((list[0] != LevelRewardType.Store && list[0] != LevelRewardType.Process && list[0] != LevelRewardType.Spring) || (randomLevelRewardType2 != LevelRewardType.Store && randomLevelRewardType2 != LevelRewardType.Process && randomLevelRewardType2 != LevelRewardType.Spring)) && randomLevelRewardType2 != list[0])
				{
					list.Add(randomLevelRewardType2);
					break;
				}
			}
		}
		if (!list.Contains(LevelRewardType.Process))
		{
			PlayerMgr.Inst.BaData.continuousNonProcess++;
		}
		else
		{
			PlayerMgr.Inst.BaData.continuousNonProcess = 0;
		}
		if (!list.Contains(LevelRewardType.Store))
		{
			PlayerMgr.Inst.BaData.continuousNonStore++;
		}
		else
		{
			PlayerMgr.Inst.BaData.continuousNonStore = 0;
		}
		if (!list.Contains(LevelRewardType.Spring))
		{
			PlayerMgr.Inst.BaData.continuousNonSpring++;
		}
		else
		{
			PlayerMgr.Inst.BaData.continuousNonSpring = 0;
		}
		if (BattleMgr.Inst.CurrentStage < 50 && list.Count == 2 && (list[0] == LevelRewardType.Spell || list[0] == LevelRewardType.Relic || list[0] == LevelRewardType.MaxHP || list[0] == LevelRewardType.Coin) && (list[1] == LevelRewardType.Spell || list[1] == LevelRewardType.Relic || list[1] == LevelRewardType.MaxHP || list[1] == LevelRewardType.Coin) && BattleMgr.Inst.CurrentLevel < BattleMgr.Inst.stageLevelsCount[BattleMgr.Inst.CurrentStage - 1] - 4)
		{
			bool flag = true;
			switch (BattleMgr.Inst.CurrentStage)
			{
			case 1:
				if (DataMgr.selectedWorldData.battleData9.shortcutRoomAppearedStage1)
				{
					flag = false;
				}
				break;
			case 2:
				if (DataMgr.selectedWorldData.battleData9.shortcutRoomAppearedStage2)
				{
					flag = false;
				}
				break;
			case 3:
				if (DataMgr.selectedWorldData.battleData9.shortcutRoomAppearedStage3)
				{
					flag = false;
				}
				break;
			case 4:
				if (DataMgr.selectedWorldData.battleData9.shortcutRoomAppearedStage4)
				{
					flag = false;
				}
				break;
			case 5:
				if (DataMgr.selectedWorldData.battleData9.shortcutRoomAppearedStage5)
				{
					flag = false;
				}
				break;
			case 6:
				if (DataMgr.selectedWorldData.battleData9.shortcutRoomAppearedStage6)
				{
					flag = false;
				}
				break;
			default:
				flag = false;
				break;
			}
			if (flag)
			{
				switch (BattleMgr.Inst.CurrentStage)
				{
				case 1:
				case 2:
				{
					float num4 = (float)DataMgr.selectedWorldData.GetResearchValueConsiderActive(ResearchAbilityType.ShortcutChapter1) / 100f;
					if (Random.value >= num4)
					{
						flag = false;
					}
					break;
				}
				case 3:
				case 4:
				{
					float num5 = (float)DataMgr.selectedWorldData.GetResearchValueConsiderActive(ResearchAbilityType.ShortcutChapter2) / 100f;
					if (Random.value >= num5)
					{
						flag = false;
					}
					break;
				}
				case 5:
				case 6:
				{
					float num3 = (float)DataMgr.selectedWorldData.GetResearchValueConsiderActive(ResearchAbilityType.ShortcutChapter3) / 100f;
					if (Random.value >= num3)
					{
						flag = false;
					}
					break;
				}
				}
			}
			if (flag)
			{
				if (Random.Range(0, 2) == 0)
				{
					list[0] = LevelRewardType.Shortcut;
				}
				else
				{
					list[1] = LevelRewardType.Shortcut;
				}
				switch (BattleMgr.Inst.CurrentStage)
				{
				case 1:
					DataMgr.selectedWorldData.battleData9.shortcutRoomAppearedStage1 = true;
					break;
				case 2:
					DataMgr.selectedWorldData.battleData9.shortcutRoomAppearedStage2 = true;
					break;
				case 3:
					DataMgr.selectedWorldData.battleData9.shortcutRoomAppearedStage3 = true;
					break;
				case 4:
					DataMgr.selectedWorldData.battleData9.shortcutRoomAppearedStage4 = true;
					break;
				case 5:
					DataMgr.selectedWorldData.battleData9.shortcutRoomAppearedStage5 = true;
					break;
				case 6:
					DataMgr.selectedWorldData.battleData9.shortcutRoomAppearedStage6 = true;
					break;
				}
			}
		}
		list.Upset();
		return list;
	}

	public static LevelRewardType GetExtraDoor(List<LevelRewardType> normalDoors)
	{
		int num = 0;
		LevelRewardType randomLevelRewardType;
		do
		{
			num++;
			if (num > 100)
			{
				Debug.LogError("!");
				return GetRandomLevelRewardType();
			}
			randomLevelRewardType = GetRandomLevelRewardType();
		}
		while (normalDoors.Contains(randomLevelRewardType));
		return randomLevelRewardType;
	}

	public static List<bool> GetDoorIsShortcut(List<LevelRewardType> normalDoors)
	{
		List<bool> list = new List<bool>();
		for (int i = 0; i < normalDoors.Count; i++)
		{
			list.Add(item: false);
		}
		if (normalDoors.Count == 2 && (normalDoors[0] == LevelRewardType.Spell || normalDoors[0] == LevelRewardType.Relic || normalDoors[0] == LevelRewardType.MaxHP || normalDoors[0] == LevelRewardType.Coin) && (normalDoors[1] == LevelRewardType.Spell || normalDoors[1] == LevelRewardType.Relic || normalDoors[1] == LevelRewardType.MaxHP || normalDoors[1] == LevelRewardType.Coin))
		{
			bool flag = false;
			switch (BattleMgr.Inst.CurrentStage)
			{
			case 1:
			case 2:
			{
				float num3 = (float)DataMgr.selectedWorldData.GetResearchValueConsiderActive(ResearchAbilityType.ShortcutChapter1) / 100f;
				if (num3 != 0f && Random.value <= num3)
				{
					flag = true;
				}
				break;
			}
			case 3:
			case 4:
			{
				float num2 = (float)DataMgr.selectedWorldData.GetResearchValueConsiderActive(ResearchAbilityType.ShortcutChapter2) / 100f;
				if (num2 != 0f && Random.value <= num2)
				{
					flag = true;
				}
				break;
			}
			case 5:
			case 6:
			{
				float num = (float)DataMgr.selectedWorldData.GetResearchValueConsiderActive(ResearchAbilityType.ShortcutChapter1) / 100f;
				if (num != 0f && Random.value <= num)
				{
					flag = true;
				}
				break;
			}
			}
			if (flag)
			{
				if (Random.Range(0, 2) == 0)
				{
					list[0] = true;
				}
				else
				{
					list[1] = true;
				}
			}
		}
		return list;
	}

	public static LevelRewardType GetRandomLevelRewardType()
	{
		if (DataMgr.selectedWorldData.GetResearchValueConsiderActive(ResearchAbilityType.Spring) > 0)
		{
			return (LevelRewardType)(GeneralTool.GetWeightRandom(1f, 1f, 1f, 1f, 0.9f, 0.9f, 0.3f) + 1);
		}
		return (LevelRewardType)(GeneralTool.GetWeightRandom(1f, 1f, 1f, 1f, 0.9f, 0.9f) + 1);
	}

	public static int GetRRO()
	{
		int weightRandom = GeneralTool.GetWeightRandom(40f, 20f, 20f, 20f);
		switch (weightRandom)
		{
		case 0:
			return 401;
		case 1:
			return 402;
		case 2:
			return 403;
		case 3:
			return 1001;
		default:
			Debug.LogError(weightRandom);
			return 401;
		}
	}

	public static int GetSpellOptionCount()
	{
		int num = 2;
		if (DataMgr.selectedWorldData.ActivateGirlHave4Pick2())
		{
			num = 4;
		}
		else if (DataMgr.selectedWorldData.ActivateGirlHave3Pick2())
		{
			num = 3;
		}
		if (PlayerMgr.Inst.ItemCtrller.relicCfg_AddSpellOption != null)
		{
			num += PlayerMgr.Inst.ItemCtrller.relicCfg_AddSpellOption.int1.result;
		}
		if (PlayerMgr.Inst.ItemCtrller.curseCfg_LostSpellOption != null)
		{
			num -= PlayerMgr.Inst.ItemCtrller.curseCfg_LostSpellOption.int1.result;
		}
		return num;
	}

	public static int GetRelicOptionCount()
	{
		int num = 3;
		if (PlayerMgr.Inst.ItemCtrller.curseCfg_RelicReduce != null)
		{
			num -= PlayerMgr.Inst.ItemCtrller.curseCfg_RelicReduce.int1.result;
			if (num < 1)
			{
				Debug.LogError("为什么遗物数量小于1!");
				num = 1;
			}
		}
		if (PlayerMgr.Inst.ItemCtrller.relicCfg_AddRelicOption != null)
		{
			num += PlayerMgr.Inst.ItemCtrller.relicCfg_AddRelicOption.int1.result;
		}
		return num;
	}
}
