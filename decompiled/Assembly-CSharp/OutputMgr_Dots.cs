using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class OutputMgr_Dots
{
	public static float DiveSuitHarpponsDropChance = 1f;

	public static int GetCrystalID()
	{
		switch (DataMgr.selectedWorldData.selectedDifficulty)
		{
		case DifficultyType.Easy:
			return 101;
		case DifficultyType.Normal:
			if (UnityEngine.Random.value <= 0.4f)
			{
				return 102;
			}
			return 101;
		case DifficultyType.Hard:
			if (UnityEngine.Random.value <= 0.8f)
			{
				return 102;
			}
			return 101;
		case DifficultyType.Nightmare1:
			if (UnityEngine.Random.value <= 0.2f)
			{
				return 103;
			}
			return 102;
		case DifficultyType.Nightmare2:
			if (UnityEngine.Random.value <= 0.6f)
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
			if (UnityEngine.Random.value <= 0.4f)
			{
				return 112;
			}
			return 111;
		case DifficultyType.Hard:
			if (UnityEngine.Random.value <= 0.8f)
			{
				return 112;
			}
			return 111;
		case DifficultyType.Nightmare1:
			if (UnityEngine.Random.value <= 0.2f)
			{
				return 113;
			}
			return 112;
		case DifficultyType.Nightmare2:
			if (UnityEngine.Random.value <= 0.6f)
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

	public static ItemInfo GetRewardD1_T3(ref Unity.Mathematics.Random random)
	{
		if (DTool.RandomValue(ref random) <= 1f)
		{
			if (BattleMgr.Inst != null && BattleMgr.Inst.CurrentStage >= 9)
			{
				return new ItemInfo(ItemType.Resource, 12);
			}
			return new ItemInfo(ItemType.Resource, 11);
		}
		return new ItemInfo(ItemType.Resource, 0);
	}

	public static ItemInfo GetRewardD4_T3(ref Unity.Mathematics.Random random)
	{
		float num = (float)DataMgr.selectedWorldData.GetResearchValueConsiderActive(ResearchAbilityType.WildBerry) / 100f;
		if (num != 0f && DTool.RandomValue(ref random) <= num)
		{
			return new ItemInfo(ItemType.Resource, 31);
		}
		return new ItemInfo(ItemType.Resource, 0);
	}

	public static BlobAssetReference<BlobArray<ItemInfo>> GetSO4Chest(ChestType chestType)
	{
		List<ItemInfo> list = new List<ItemInfo>();
		if ((uint)chestType <= 3u)
		{
			int weightRandom = GeneralTool.GetWeightRandom(75f, 20f, 5f);
			switch (weightRandom)
			{
			case 0:
			{
				for (int j = 0; j < 2; j++)
				{
					weightRandom = UnityEngine.Random.Range(0, 11);
					switch (weightRandom)
					{
					case 0:
					case 1:
					{
						int num2 = UnityEngine.Random.Range(5, 12);
						for (int k = 0; k < num2; k++)
						{
							list.Add(new ItemInfo(ItemType.Resource, 11));
						}
						break;
					}
					case 2:
					case 3:
						list.Add(new ItemInfo(ItemType.Resource, 32));
						break;
					case 4:
					case 5:
						list.Add(new ItemInfo(ItemType.Resource, 42));
						break;
					case 6:
					case 7:
						list.Add(new ItemInfo(ItemType.Potion, PlayerMgr.Inst.BaData.GetPotionFromPool()));
						break;
					case 8:
					case 9:
						list.Add(new ItemInfo(ItemType.Resource, 21));
						break;
					case 10:
						if (UnityEngine.Random.value <= 0.12f)
						{
							list.Add(new ItemInfo(ItemType.Spell, PlayerMgr.Inst.BaData.GetSpellFromPool(1, ItemDropType.Rare)));
						}
						else
						{
							list.Add(new ItemInfo(ItemType.Spell, PlayerMgr.Inst.BaData.GetSpellFromPool(1, ItemDropType.Common)));
						}
						break;
					default:
						Debug.LogError(weightRandom);
						break;
					}
				}
				break;
			}
			case 1:
				if (UnityEngine.Random.value <= 0.12f)
				{
					list.Add(new ItemInfo(ItemType.Relic, PlayerMgr.Inst.BaData.GetRelicFromPool(ItemDropType.Rare)));
				}
				else
				{
					list.Add(new ItemInfo(ItemType.Relic, PlayerMgr.Inst.BaData.GetRelicFromPool(ItemDropType.Common)));
				}
				break;
			case 2:
			{
				int num = UnityEngine.Random.Range(8, 16);
				for (int i = 0; i < num; i++)
				{
					list.Add(new ItemInfo(ItemType.Resource, GetCrystalID()));
				}
				break;
			}
			default:
				Debug.LogError(weightRandom);
				break;
			}
			if (PlayerMgr.Inst.ItemCtrller.relic_DivingSuit != null)
			{
				int randomEnhancedHarpoons = GeneralTool.GetRandomEnhancedHarpoons();
				list.Add(new ItemInfo(ItemType.Relic, randomEnhancedHarpoons));
				if (PlayerMgr.Inst.ItemCtrller.relic_DivingSuit.level == 2 && UnityEngine.Random.Range(0f, 1f) <= 0.5f)
				{
					randomEnhancedHarpoons = GeneralTool.GetRandomEnhancedHarpoons();
					list.Add(new ItemInfo(ItemType.Relic, randomEnhancedHarpoons));
				}
				DiveSuitHarpponsDropChance = 0.4f;
			}
			else
			{
				DiveSuitHarpponsDropChance += 0.13f;
			}
		}
		else
		{
			Debug.LogError(chestType);
		}
		return DTool.ListToBlobArray(list);
	}

	public static List<ItemInfo> GetLevelReward(LevelRewardType type)
	{
		List<ItemInfo> list = new List<ItemInfo>();
		switch (type)
		{
		case LevelRewardType.Spell:
		{
			if (DataMgr.selectedWorldData.enterBattleTime == 1 && BattleMgr.Inst.CurrentStage == 1 && BattleMgr.Inst.CurrentLevel == 2)
			{
				list.Add(new ItemInfo(ItemType.Spell, 30131));
				list.Add(new ItemInfo(ItemType.Spell, 30071));
				break;
			}
			int spellOptionCount = GetSpellOptionCount();
			float num5 = 0.01f;
			if (DataMgr.selectedWorldData.enterBattleTime <= 1)
			{
				num5 = 0f;
			}
			if (DataMgr.selectedWorldData.selectedDifficulty == DifficultyType.Nightmare1)
			{
				num5 *= 1.5f;
			}
			else if (DataMgr.selectedWorldData.selectedDifficulty == DifficultyType.Nightmare2)
			{
				num5 *= 1.5f;
			}
			else if (DataMgr.selectedWorldData.selectedDifficulty == DifficultyType.Nightmare3)
			{
				num5 *= 2f;
			}
			float num6 = 0.12f;
			num6 *= (float)DataMgr.selectedWorldData.GetTalentSpellRoomValue() / 100f;
			for (int num7 = 0; num7 < spellOptionCount; num7++)
			{
				float num8 = UnityEngine.Random.value;
				if (DataMgr.selectedWorldData.ActivateGirlHave4Pick2())
				{
					num8 /= 0.5f;
				}
				else if (DataMgr.selectedWorldData.ActivateGirlHave3Pick2())
				{
					num8 /= 0.666667f;
				}
				int num9 = 0;
				num9 = ((!(num8 < num5)) ? ((!(num8 <= num5 + num6)) ? PlayerMgr.Inst.BaData.GetSpellFromPool(1, ItemDropType.Common) : PlayerMgr.Inst.BaData.GetSpellFromPool(1, ItemDropType.Rare)) : PlayerMgr.Inst.BaData.GetSpellFromPool(1, ItemDropType.Epic));
				list.Add(new ItemInfo(ItemType.Spell, num9));
			}
			break;
		}
		case LevelRewardType.RuneWizardRune:
		{
			for (int m = 0; m < 3; m++)
			{
				list.Add(new ItemInfo(ItemType.Spell, GameConstManaged.LostCastleRuneID[m]));
			}
			break;
		}
		case LevelRewardType.Relic:
		{
			int relicOptionCount = GetRelicOptionCount();
			float num13 = 0f;
			if (DataMgr.selectedWorldData.selectedDifficulty == DifficultyType.Nightmare2)
			{
				num13 = 0.01f;
			}
			else if (DataMgr.selectedWorldData.selectedDifficulty == DifficultyType.Nightmare3)
			{
				num13 = 0.016f;
			}
			float num14 = 0.06f;
			num14 *= (float)DataMgr.selectedWorldData.GetTalentRelicRoomValue() / 100f;
			int num15 = 0;
			do
			{
				num15++;
				if (num15 >= 100)
				{
					Debug.LogError("!");
					list.Add(new ItemInfo(ItemType.Relic, 999));
				}
				float value = UnityEngine.Random.value;
				int num16 = 0;
				num16 = ((value < num13) ? PlayerMgr.Inst.BaData.GetRelicFromPool(ItemDropType.Epic) : ((!(value <= num13 + num14)) ? PlayerMgr.Inst.BaData.GetRelicFromPool(ItemDropType.Common) : PlayerMgr.Inst.BaData.GetRelicFromPool(ItemDropType.Rare)));
				bool flag = false;
				for (int num17 = 0; num17 < list.Count; num17++)
				{
					if (list[num17].id == num16)
					{
						flag = true;
						PlayerMgr.Inst.BaData.BackRelicToPool(num16, 1);
						break;
					}
				}
				if (!flag)
				{
					list.Add(new ItemInfo(ItemType.Relic, num16));
				}
			}
			while (list.Count < relicOptionCount);
			break;
		}
		case LevelRewardType.Coin:
		{
			int num10 = 10;
			num10 += DataMgr.selectedWorldData.GetTalentCoinRoomValue() / 10;
			if (PlayerMgr.Inst.ItemCtrller.relicCfg_MoreCoinOutput != null)
			{
				num10 += Mathf.CeilToInt((float)(num10 * PlayerMgr.Inst.ItemCtrller.relicCfg_MoreCoinOutput.int1.result) / 100f);
			}
			for (int num11 = 0; num11 < num10; num11++)
			{
				int num12 = GeneralTool.GetWeightRandom(9f, 1f);
				switch (num12)
				{
				case 0:
					num12 = 11;
					break;
				case 1:
					num12 = 12;
					break;
				default:
					Debug.LogError(num12);
					break;
				}
				list.Add(new ItemInfo(ItemType.Resource, num12));
			}
			break;
		}
		case LevelRewardType.Elite:
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
			switch (BattleMgr.Inst.CurrentStage)
			{
			case 1:
				list.Add(new ItemInfo(ItemType.Wand, PlayerMgr.Inst.BaData.GetWandFromPool(1)));
				list.Add(new ItemInfo(ItemType.Wand, PlayerMgr.Inst.BaData.GetWandFromPool(1)));
				break;
			case 3:
			{
				list.Add(new ItemInfo(ItemType.Spell, PlayerMgr.Inst.BaData.GetSpellFromPool(2, ItemDropType.Common)));
				for (int num3 = 1; num3 < num; num3++)
				{
					list.Add(new ItemInfo(ItemType.Spell, PlayerMgr.Inst.BaData.GetSpellFromPool(1, ItemDropType.Rare)));
				}
				break;
			}
			case 5:
			{
				list.Add(new ItemInfo(ItemType.Spell, PlayerMgr.Inst.BaData.GetSpellFromPool(2, ItemDropType.Rare)));
				for (int num4 = 1; num4 < num; num4++)
				{
					list.Add(new ItemInfo(ItemType.Spell, PlayerMgr.Inst.BaData.GetSpellFromPool(2, ItemDropType.Common)));
				}
				break;
			}
			case 7:
			{
				for (int num2 = 0; num2 < num; num2++)
				{
					list.Add(new ItemInfo(ItemType.Spell, PlayerMgr.Inst.BaData.GetSpellFromPool(2, ItemDropType.Rare)));
				}
				break;
			}
			case 9:
			case 10:
			{
				for (int n = 0; n < num; n++)
				{
					list.Add(new ItemInfo(ItemType.Spell, PlayerMgr.Inst.BaData.GetSpellFromPool(2, ItemDropType.Rare)));
				}
				break;
			}
			default:
				list.Add(new ItemInfo(ItemType.Spell, PlayerMgr.Inst.BaData.GetSpellFromPool(1, ItemDropType.Common)));
				list.Add(new ItemInfo(ItemType.Spell, PlayerMgr.Inst.BaData.GetSpellFromPool(1, ItemDropType.Common)));
				Debug.LogError(BattleMgr.Inst.CurrentStage);
				break;
			}
			break;
		}
		case LevelRewardType.Boss:
			switch (BattleMgr.Inst.CurrentStage)
			{
			case 2:
				list.Add(new ItemInfo(ItemType.Wand, PlayerMgr.Inst.BaData.GetWandFromPool(2)));
				list.Add(new ItemInfo(ItemType.Wand, PlayerMgr.Inst.BaData.GetWandFromPool(2)));
				break;
			case 4:
				list.Add(new ItemInfo(ItemType.Wand, PlayerMgr.Inst.BaData.GetWandFromPool(4)));
				list.Add(new ItemInfo(ItemType.Wand, PlayerMgr.Inst.BaData.GetWandFromPool(4)));
				break;
			case 6:
				list.Add(new ItemInfo(ItemType.Wand, PlayerMgr.Inst.BaData.GetWandFromPool(6)));
				list.Add(new ItemInfo(ItemType.Wand, PlayerMgr.Inst.BaData.GetWandFromPool(6)));
				break;
			case 8:
				list.Add(new ItemInfo(ItemType.Wand, PlayerMgr.Inst.BaData.GetWandFromPool(8)));
				list.Add(new ItemInfo(ItemType.Wand, PlayerMgr.Inst.BaData.GetWandFromPool(8)));
				break;
			case 10:
				list.Add(new ItemInfo(ItemType.Wand, PlayerMgr.Inst.BaData.GetWandFromPool(10)));
				list.Add(new ItemInfo(ItemType.Wand, PlayerMgr.Inst.BaData.GetWandFromPool(10)));
				break;
			default:
				list.Add(new ItemInfo(ItemType.Wand, PlayerMgr.Inst.BaData.GetWandFromPool(10)));
				list.Add(new ItemInfo(ItemType.Wand, PlayerMgr.Inst.BaData.GetWandFromPool(10)));
				Debug.LogError(BattleMgr.Inst.CurrentStage);
				break;
			}
			break;
		case LevelRewardType.EndlessWand:
		{
			int num18 = 1;
			num18 = Mathf.Clamp(BattleMgr.Inst.CurrentLevel / 5, 0, 4) * 2;
			if (num18 == 0)
			{
				num18 = 1;
			}
			list.Add(new ItemInfo(ItemType.Wand, PlayerMgr.Inst.BaData.GetWandFromPool(num18)));
			list.Add(new ItemInfo(ItemType.Wand, PlayerMgr.Inst.BaData.GetWandFromPool(num18)));
			break;
		}
		case LevelRewardType.EndlessSpell:
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
			switch ((BattleMgr.Inst.CurrentLevel + 1 - 1) / 5)
			{
			case 0:
				list.Add(new ItemInfo(ItemType.Wand, PlayerMgr.Inst.BaData.GetWandFromPool(1)));
				list.Add(new ItemInfo(ItemType.Wand, PlayerMgr.Inst.BaData.GetWandFromPool(1)));
				break;
			case 1:
			{
				list.Add(new ItemInfo(ItemType.Spell, PlayerMgr.Inst.BaData.GetSpellFromPool(2, ItemDropType.Common)));
				for (int k = 1; k < num; k++)
				{
					list.Add(new ItemInfo(ItemType.Spell, PlayerMgr.Inst.BaData.GetSpellFromPool(1, ItemDropType.Rare)));
				}
				break;
			}
			case 2:
			{
				list.Add(new ItemInfo(ItemType.Spell, PlayerMgr.Inst.BaData.GetSpellFromPool(2, ItemDropType.Rare)));
				for (int l = 1; l < num; l++)
				{
					list.Add(new ItemInfo(ItemType.Spell, PlayerMgr.Inst.BaData.GetSpellFromPool(2, ItemDropType.Common)));
				}
				break;
			}
			case 3:
			case 4:
			{
				for (int j = 0; j < num; j++)
				{
					list.Add(new ItemInfo(ItemType.Spell, PlayerMgr.Inst.BaData.GetSpellFromPool(2, ItemDropType.Rare)));
				}
				break;
			}
			default:
			{
				for (int i = 0; i < num; i++)
				{
					list.Add(new ItemInfo(ItemType.Spell, PlayerMgr.Inst.BaData.GetSpellFromPool(2, ItemDropType.Rare)));
				}
				break;
			}
			}
			break;
		}
		default:
			Debug.LogError(type);
			break;
		case LevelRewardType.Wand:
		case LevelRewardType.MaxHP:
		case LevelRewardType.Store:
		case LevelRewardType.Process:
		case LevelRewardType.Spring:
		case LevelRewardType.None:
			break;
		}
		if (ScriptableObjMgr.Inst.testCtrller.Battle100PercentEpicItem && list.Count > 0)
		{
			switch (type)
			{
			case LevelRewardType.Spell:
				list[UnityEngine.Random.Range(0, list.Count)] = new ItemInfo(ItemType.Spell, PlayerMgr.Inst.BaData.GetSpellFromPool(1, ItemDropType.Epic));
				break;
			case LevelRewardType.Relic:
				list[UnityEngine.Random.Range(0, list.Count)] = new ItemInfo(ItemType.Relic, PlayerMgr.Inst.BaData.GetRelicFromPool(ItemDropType.Epic));
				break;
			}
		}
		return list;
	}

	public static BlobAssetReference<BlobArray<ItemInfo>> GetEliteOrBossItemInfos()
	{
		List<ItemInfo> list = new List<ItemInfo>();
		list.Add(new ItemInfo(ItemType.Resource, 32));
		int num = UnityEngine.Random.Range(10, 21);
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
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
		case 300:
			num = 0;
			num2 = 0;
			num3 = 0;
			num4 = SpecialObj301EndlessMonsterSpawner.Inst.currentStageSpawnInfo.dropCount;
			list.Add(new ItemInfo(ItemType.Resource, 32));
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
		for (int l = 0; l < num4 / 100; l++)
		{
			list.Add(new ItemInfo(ItemType.Resource, 133));
		}
		for (int m = 0; m < num4 % 100 / 10; m++)
		{
			list.Add(new ItemInfo(ItemType.Resource, 132));
		}
		for (int n = 0; n < num4 % 10; n++)
		{
			list.Add(new ItemInfo(ItemType.Resource, 131));
		}
		return DTool.ListToBlobArray(list);
	}

	public static List<ItemInfo> GetExtraDrop(LevelRewardType type)
	{
		List<ItemInfo> list = new List<ItemInfo>();
		switch (type)
		{
		case LevelRewardType.Spell:
		case LevelRewardType.Relic:
		case LevelRewardType.MaxHP:
		case LevelRewardType.Coin:
		case LevelRewardType.Shortcut:
		{
			if (UnityEngine.Random.value <= 0.3333f)
			{
				list.Add(new ItemInfo(ItemType.Resource, GetCrystalID()));
			}
			int num = UnityEngine.Random.Range(0, 3);
			for (int i = 0; i < num; i++)
			{
				list.Add(new ItemInfo(ItemType.Resource, 11));
			}
			int weightRandom = GeneralTool.GetWeightRandom(55f, 7f, 13f, 16f, 9f);
			switch (weightRandom)
			{
			case 1:
				list.Add(new ItemInfo(ItemType.Resource, 12));
				break;
			case 2:
				list.Add(new ItemInfo(ItemType.Resource, 21));
				break;
			case 3:
				list.Add(new ItemInfo(ItemType.Potion, PlayerMgr.Inst.BaData.GetPotionFromPool()));
				break;
			case 4:
				list.Add(new ItemInfo(ItemType.Resource, 32));
				break;
			default:
				Debug.LogError(weightRandom);
				break;
			case 0:
				break;
			}
			break;
		}
		default:
			Debug.LogError(type);
			break;
		case LevelRewardType.Wand:
		case LevelRewardType.Store:
		case LevelRewardType.Process:
		case LevelRewardType.Spring:
		case LevelRewardType.Elite:
		case LevelRewardType.Boss:
		case LevelRewardType.Chapter:
		case LevelRewardType.None:
		case LevelRewardType.Ruined:
		case LevelRewardType.EndlessChapter:
			break;
		}
		return list;
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
