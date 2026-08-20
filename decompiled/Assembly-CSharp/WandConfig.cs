using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[Serializable]
public class WandConfig
{
	public class Initializer : DataMgr.ConfigInitializer<WandConfig>
	{
		public override void ApplyResult(List<WandConfig> result)
		{
			list = result;
			dic = new Dictionary<int, WandConfig>();
			foreach (WandConfig item in list)
			{
				dic.Add(item.id, item);
			}
		}
	}

	public static Dictionary<int, WandConfig> dic;

	public static List<WandConfig> list;

	public int id;

	public int priceCoin;

	public int priceHP;

	public string icon;

	public string iconH;

	public int dropStage;

	public float shootInterval;

	public float coolDown;

	public float angle;

	public int maxMP;

	public int mpRecovery;

	public int shootCount;

	public int costCorrection;

	public float criticalChance;

	public float damageCorrection;

	public bool[] normalSlotIsLock;

	public bool[] postSlotIsLock;

	public SlotData[] normalSlots;

	public SlotData[] postSlots;

	public WandAbility specialAbility;

	public float PostslotMoveChargeRatio;

	public float PostslotKillEnemyChargeRatio;

	public float PostslotSpellHitChargeRatio;

	public float PostslotStandChargeRatio;

	public float PostslotCastSpellChargeRatio;

	public float PostslotHighDamageChargeRatio;

	public float PostslotCriticalHitChargeRatio;

	public float PostslotTakeDamageChargeRatio;

	public float PostslotTimeChargeRatio;

	public bool[] transIntoPostslotLockData = Array.Empty<bool>();

	public int int1;

	public int int2;

	public int int3;

	public float float1;

	public float float2;

	public float float3;

	public WandPostSlotTriggerType postSlotTriggerType { get; set; }

	public float PostSlotTriggerChargeRatio { get; set; }

	public SlotData[] transIntoPostslotData { get; set; } = Array.Empty<SlotData>();


	public List<SlotData> AllfieldSharedSpellList { get; set; } = new List<SlotData>();


	public static WandConfig GetConfig(int id)
	{
		if (!dic.ContainsKey(id))
		{
			Debug.LogError("No ID:" + id);
		}
		return dic[id].Copy();
	}

	public WandConfig Copy()
	{
		WandConfig wandConfig = new WandConfig();
		wandConfig.id = id;
		wandConfig.priceCoin = priceCoin;
		wandConfig.priceHP = priceHP;
		wandConfig.icon = icon;
		wandConfig.iconH = iconH;
		wandConfig.dropStage = dropStage;
		wandConfig.shootInterval = shootInterval;
		wandConfig.coolDown = coolDown;
		wandConfig.angle = angle;
		wandConfig.maxMP = maxMP;
		wandConfig.mpRecovery = mpRecovery;
		wandConfig.shootCount = shootCount;
		wandConfig.costCorrection = costCorrection;
		wandConfig.criticalChance = criticalChance;
		wandConfig.damageCorrection = damageCorrection;
		wandConfig.specialAbility = specialAbility;
		wandConfig.PostslotMoveChargeRatio = PostslotMoveChargeRatio;
		wandConfig.PostslotKillEnemyChargeRatio = PostslotKillEnemyChargeRatio;
		wandConfig.PostslotSpellHitChargeRatio = PostslotSpellHitChargeRatio;
		wandConfig.PostslotStandChargeRatio = PostslotStandChargeRatio;
		wandConfig.PostslotCastSpellChargeRatio = PostslotCastSpellChargeRatio;
		wandConfig.PostslotHighDamageChargeRatio = PostslotHighDamageChargeRatio;
		wandConfig.PostslotCriticalHitChargeRatio = PostslotCriticalHitChargeRatio;
		wandConfig.PostslotTakeDamageChargeRatio = PostslotTakeDamageChargeRatio;
		wandConfig.PostslotTimeChargeRatio = PostslotTimeChargeRatio;
		wandConfig.normalSlotIsLock = normalSlotIsLock.Copy();
		wandConfig.postSlotIsLock = postSlotIsLock.Copy();
		wandConfig.normalSlots = new SlotData[normalSlots.Length];
		wandConfig.postSlots = new SlotData[postSlots.Length];
		for (int i = 0; i < normalSlots.Length; i++)
		{
			if (normalSlots[i] == null)
			{
				wandConfig.normalSlots[i] = null;
			}
			else
			{
				wandConfig.normalSlots[i] = normalSlots[i].Copy();
			}
		}
		for (int j = 0; j < postSlots.Length; j++)
		{
			if (postSlots[j] == null)
			{
				wandConfig.postSlots[j] = null;
			}
			else
			{
				wandConfig.postSlots[j] = postSlots[j].Copy();
			}
		}
		wandConfig.postSlotTriggerType = postSlotTriggerType;
		wandConfig.PostSlotTriggerChargeRatio = PostSlotTriggerChargeRatio;
		wandConfig.int1 = int1;
		wandConfig.int2 = int2;
		wandConfig.int3 = int3;
		wandConfig.float1 = float1;
		wandConfig.float2 = float2;
		wandConfig.float3 = float3;
		return wandConfig;
	}

	public void OnLoadedAfter()
	{
		ProcessSlotCost(normalSlots);
		ProcessSlotCost(postSlots);
		static void ProcessSlotCost(SlotData[] slots)
		{
			for (int i = 0; i < slots.Length; i++)
			{
				if (slots[i] != null)
				{
					int slotCost = slots[i].GetFinalConfig().slotCost;
					if (slotCost > 1)
					{
						for (int j = 0; j < slotCost - 1; j++)
						{
							slots[i + j + 1] = new SlotData
							{
								sealSlotOwner = slots[i]
							};
						}
						i += slotCost - 1;
					}
				}
			}
		}
	}

	public int GetPrice()
	{
		int num = priceCoin;
		for (int i = 0; i < normalSlots.Length; i++)
		{
			if (normalSlots[i] != null && !normalSlots[i].isSealSlot)
			{
				num = ((!normalSlotIsLock[i]) ? (num + SpellConfig.dic[normalSlots[i].id].priceCoin) : (num + Mathf.FloorToInt((float)SpellConfig.dic[normalSlots[i].id].priceCoin / 2f)));
			}
		}
		for (int j = 0; j < postSlots.Length; j++)
		{
			if (postSlots[j] != null && !postSlots[j].isSealSlot)
			{
				num = ((!postSlotIsLock[j]) ? (num + SpellConfig.dic[postSlots[j].id].priceCoin) : (num + Mathf.FloorToInt((float)SpellConfig.dic[postSlots[j].id].priceCoin / 2f)));
			}
		}
		return num;
	}

	public float GetWandScatter()
	{
		float num = angle;
		if (PlayerMgr.Inst.ItemCtrller.curseCfg_ScatterAdd != null)
		{
			num += (float)PlayerMgr.Inst.ItemCtrller.curseCfg_ScatterAdd.int1.result;
		}
		return num;
	}

	public int GetPriceHP()
	{
		return priceHP;
	}

	public string GetName()
	{
		return (id + 5000000).GetText();
	}

	public List<SlotData> GetSharedSpellFromTargetList(SlotData[] sourseData)
	{
		List<SlotData> list = new List<SlotData>();
		foreach (SlotData slotData in sourseData)
		{
			if (slotData == null || !slotData.isAllFieldSharedSpell)
			{
				break;
			}
			list.Add(slotData);
		}
		return list;
	}

	public List<SlotData> GetSharedSpellList()
	{
		List<SlotData> obj = new List<SlotData>();
		obj.AddRange(GetSharedSpellFromTargetList(normalSlots));
		obj.AddRange(GetSharedSpellFromTargetList(postSlots));
		return obj;
	}

	public bool IsAllfieldSharedSpellSame(List<SlotData> data)
	{
		AllfieldSharedSpellList = GetSharedSpellList();
		if (data.Count != AllfieldSharedSpellList.Count)
		{
			return false;
		}
		for (int i = 0; i < AllfieldSharedSpellList.Count; i++)
		{
			if (data[i].id != AllfieldSharedSpellList[i].id)
			{
				return false;
			}
			if (data[i].GetConfigIgnoreMimic().abilityType != SpellAbilityType.Mimic && data[i].mimicSpellID != AllfieldSharedSpellList[i].mimicSpellID)
			{
				return false;
			}
		}
		return true;
	}

	public float GetPostSlotFinalChargeRatio()
	{
		return PlayerMgr.Inst.GetPostSlotChargeEfficiency(this);
	}

	public string GetPostSlotDesc()
	{
		StringBuilder stringBuilder = new StringBuilder();
		DataTextColorType type = DataTextColorType.Default;
		if (GetPostSlotFinalChargeRatio() > 1f)
		{
			type = DataTextColorType.Green;
		}
		if (GetPostSlotFinalChargeRatio() < 1f)
		{
			type = DataTextColorType.Red;
		}
		if (PostslotKillEnemyChargeRatio > 0f)
		{
			string source = GeneralTool.FloatToRetainDecimals(PostslotKillEnemyChargeRatio * GetPostSlotFinalChargeRatio(), 1);
			stringBuilder.StartField().Append("◆\u00a0\u200a" + 1001001.GetText());
			stringBuilder = stringBuilder.Replace("PostSlotTriggerChargeRatio", TextProcesser.GetColorText(source, type));
		}
		if (PostslotMoveChargeRatio > 0f)
		{
			string source2 = GeneralTool.FloatToRetainDecimals(PostslotMoveChargeRatio * GetPostSlotFinalChargeRatio(), 1);
			stringBuilder.StartField().Append("◆\u00a0\u200a" + 1001002.GetText());
			stringBuilder = stringBuilder.Replace("PostSlotTriggerChargeRatio", TextProcesser.GetColorText(source2, type));
		}
		if (PostslotStandChargeRatio > 0f)
		{
			string source3 = GeneralTool.FloatToRetainDecimals(PostslotStandChargeRatio * GetPostSlotFinalChargeRatio(), 1);
			stringBuilder.StartField().Append("◆\u00a0\u200a" + 1001004.GetText());
			stringBuilder = stringBuilder.Replace("PostSlotTriggerChargeRatio", TextProcesser.GetColorText(source3, type));
		}
		if (PostslotHighDamageChargeRatio > 0f)
		{
			string source4 = GeneralTool.FloatToRetainDecimals(PostslotHighDamageChargeRatio * GetPostSlotFinalChargeRatio(), 1);
			stringBuilder.StartField().Append("◆\u00a0\u200a" + 1001006.GetText());
			stringBuilder = stringBuilder.Replace("DamageThreshold", 45.ToString());
			stringBuilder = stringBuilder.Replace("PostSlotTriggerChargeRatio", TextProcesser.GetColorText(source4, type));
		}
		if (PostslotTakeDamageChargeRatio > 0f)
		{
			string source5 = GeneralTool.FloatToRetainDecimals(PostslotTakeDamageChargeRatio * GetPostSlotFinalChargeRatio(), 1);
			stringBuilder.StartField().Append("◆\u00a0\u200a" + 1001008.GetText());
			stringBuilder = stringBuilder.Replace("PostSlotTriggerChargeRatio", TextProcesser.GetColorText(source5, type));
		}
		if (PostslotSpellHitChargeRatio > 0f)
		{
			string source6 = GeneralTool.FloatToRetainDecimals(PostslotSpellHitChargeRatio * GetPostSlotFinalChargeRatio(), 1);
			stringBuilder.StartField().Append("◆\u00a0\u200a" + 1001003.GetText());
			stringBuilder = stringBuilder.Replace("PostSlotTriggerChargeRatio", TextProcesser.GetColorText(source6, type));
		}
		if (PostslotCriticalHitChargeRatio > 0f)
		{
			string source7 = GeneralTool.FloatToRetainDecimals(PostslotCriticalHitChargeRatio * GetPostSlotFinalChargeRatio(), 1);
			stringBuilder.StartField().Append("◆\u00a0\u200a" + 1001007.GetText());
			stringBuilder = stringBuilder.Replace("PostSlotTriggerChargeRatio", TextProcesser.GetColorText(source7, type));
		}
		if (PostslotCastSpellChargeRatio > 0f)
		{
			string source8 = GeneralTool.FloatToRetainDecimals(PostslotCastSpellChargeRatio * GetPostSlotFinalChargeRatio(), 1);
			stringBuilder.StartField().Append("◆\u00a0\u200a" + 1001005.GetText());
			stringBuilder = stringBuilder.Replace("PostSlotTriggerChargeRatio", TextProcesser.GetColorText(source8, type));
		}
		if (PostslotTimeChargeRatio > 0f)
		{
			string source9 = GeneralTool.FloatToRetainDecimals(PostslotTimeChargeRatio * GetPostSlotFinalChargeRatio(), 1);
			stringBuilder.StartField().Append("◆\u00a0\u200a" + 1001009.GetText());
			stringBuilder = stringBuilder.Replace("PostSlotTriggerChargeRatio", TextProcesser.GetColorText(source9, type));
		}
		return stringBuilder.ToString();
	}

	public void ResetPostSlot()
	{
		PostslotMoveChargeRatio = 0f;
		PostslotKillEnemyChargeRatio = 0f;
		PostslotSpellHitChargeRatio = 0f;
		PostslotStandChargeRatio = 0f;
		PostslotCastSpellChargeRatio = 0f;
		PostslotHighDamageChargeRatio = 0f;
		PostslotCriticalHitChargeRatio = 0f;
		PostslotTakeDamageChargeRatio = 0f;
		PostslotTimeChargeRatio = 0f;
		WandConfig wandConfig = dic[id];
		switch (wandConfig.postSlotTriggerType)
		{
		case WandPostSlotTriggerType.KillEnemy:
			postSlotTriggerType = WandPostSlotTriggerType.KillEnemy;
			PostslotKillEnemyChargeRatio = wandConfig.PostSlotTriggerChargeRatio;
			break;
		case WandPostSlotTriggerType.MoveDistance:
			postSlotTriggerType = WandPostSlotTriggerType.MoveDistance;
			PostslotMoveChargeRatio = wandConfig.PostSlotTriggerChargeRatio;
			break;
		case WandPostSlotTriggerType.SpellHit:
			postSlotTriggerType = WandPostSlotTriggerType.SpellHit;
			PostslotSpellHitChargeRatio = wandConfig.PostSlotTriggerChargeRatio;
			break;
		case WandPostSlotTriggerType.Stand:
			postSlotTriggerType = WandPostSlotTriggerType.Stand;
			PostslotStandChargeRatio = wandConfig.PostSlotTriggerChargeRatio;
			break;
		case WandPostSlotTriggerType.CastSpell:
			postSlotTriggerType = WandPostSlotTriggerType.CastSpell;
			PostslotCastSpellChargeRatio = wandConfig.PostSlotTriggerChargeRatio;
			break;
		case WandPostSlotTriggerType.HighDamage:
			postSlotTriggerType = WandPostSlotTriggerType.HighDamage;
			PostslotHighDamageChargeRatio = wandConfig.PostSlotTriggerChargeRatio;
			break;
		case WandPostSlotTriggerType.CriticalHit:
			postSlotTriggerType = WandPostSlotTriggerType.CriticalHit;
			PostslotCriticalHitChargeRatio = wandConfig.PostSlotTriggerChargeRatio;
			break;
		case WandPostSlotTriggerType.TakeDamage:
			postSlotTriggerType = WandPostSlotTriggerType.TakeDamage;
			PostslotTakeDamageChargeRatio = wandConfig.PostSlotTriggerChargeRatio;
			break;
		case WandPostSlotTriggerType.Time:
			postSlotTriggerType = WandPostSlotTriggerType.Time;
			PostslotTimeChargeRatio = wandConfig.PostSlotTriggerChargeRatio;
			break;
		}
	}

	public void UpdatePostSlotType()
	{
		if (postSlots.Length != 0)
		{
			int num = 0;
			if (PostslotKillEnemyChargeRatio > 0f)
			{
				num++;
				postSlotTriggerType = WandPostSlotTriggerType.KillEnemy;
			}
			if (PostslotMoveChargeRatio > 0f)
			{
				num++;
				postSlotTriggerType = WandPostSlotTriggerType.MoveDistance;
			}
			if (PostslotStandChargeRatio > 0f)
			{
				num++;
				postSlotTriggerType = WandPostSlotTriggerType.Stand;
			}
			if (PostslotHighDamageChargeRatio > 0f)
			{
				num++;
				postSlotTriggerType = WandPostSlotTriggerType.HighDamage;
			}
			if (PostslotTakeDamageChargeRatio > 0f)
			{
				num++;
				postSlotTriggerType = WandPostSlotTriggerType.TakeDamage;
			}
			if (PostslotCriticalHitChargeRatio > 0f)
			{
				num++;
				postSlotTriggerType = WandPostSlotTriggerType.CriticalHit;
			}
			if (PostslotSpellHitChargeRatio > 0f)
			{
				num++;
				postSlotTriggerType = WandPostSlotTriggerType.SpellHit;
			}
			if (PostslotCastSpellChargeRatio > 0f)
			{
				num++;
				postSlotTriggerType = WandPostSlotTriggerType.CastSpell;
			}
			if (PostslotTimeChargeRatio > 0f)
			{
				num++;
				postSlotTriggerType = WandPostSlotTriggerType.Time;
			}
			if (num > 1)
			{
				postSlotTriggerType = WandPostSlotTriggerType.Mix;
			}
		}
	}

	public Sprite GetPostSlotIcon()
	{
		UpdatePostSlotType();
		switch (postSlotTriggerType)
		{
		case WandPostSlotTriggerType.KillEnemy:
			if (GameMgr.IsHarmony_Static)
			{
				return ABResources.LoadAsset<Sprite>("Textures/WandPostSlotIcons/DeathChargeH");
			}
			return ABResources.LoadAsset<Sprite>("Textures/WandPostSlotIcons/DeathCharge");
		case WandPostSlotTriggerType.MoveDistance:
			return ABResources.LoadAsset<Sprite>("Textures/WandPostSlotIcons/MoveCharge");
		case WandPostSlotTriggerType.SpellHit:
			return ABResources.LoadAsset<Sprite>("Textures/WandPostSlotIcons/SpellHitCharge");
		case WandPostSlotTriggerType.Stand:
			return ABResources.LoadAsset<Sprite>("Textures/WandPostSlotIcons/StandCharge");
		case WandPostSlotTriggerType.CastSpell:
			return ABResources.LoadAsset<Sprite>("Textures/WandPostSlotIcons/CastSpellCharge");
		case WandPostSlotTriggerType.HighDamage:
			return ABResources.LoadAsset<Sprite>("Textures/WandPostSlotIcons/HighDamage");
		case WandPostSlotTriggerType.CriticalHit:
			return ABResources.LoadAsset<Sprite>("Textures/WandPostSlotIcons/CriticalHit");
		case WandPostSlotTriggerType.TakeDamage:
			return ABResources.LoadAsset<Sprite>("Textures/WandPostSlotIcons/TakeDamage");
		case WandPostSlotTriggerType.Time:
			return ABResources.LoadAsset<Sprite>("Textures/WandPostSlotIcons/TimeCharge");
		case WandPostSlotTriggerType.Mix:
			return ABResources.LoadAsset<Sprite>("Textures/WandPostSlotIcons/MixCharge");
		default:
			return ABResources.LoadAsset<Sprite>("Textures/WandPostSlotIcons/MixCharge");
		}
	}

	public bool IsSlotLock(WandSlotType type, int index)
	{
		bool[] slotsLockState = GetSlotsLockState(type);
		if (index >= slotsLockState.Length)
		{
			return false;
		}
		return slotsLockState[index];
	}

	public SlotData[] GetValidSlotsData(bool normal, bool post)
	{
		List<SlotData> list = new List<SlotData>();
		if (normal)
		{
			list.AddRange(normalSlots.Where((SlotData e) => e != null && !e.isSealSlot));
		}
		if (post)
		{
			list.AddRange(postSlots.Where((SlotData e) => e != null && !e.isSealSlot));
		}
		return list.ToArray();
	}

	public SlotData[] GetSlotsData(WandSlotType type)
	{
		switch (type)
		{
		case WandSlotType.Normal:
			return normalSlots;
		case WandSlotType.Post:
			return postSlots;
		default:
			Debug.LogError("不应该返回前中后格子以外的类型来着 如果到这里就有错了 检查一下吧");
			return normalSlots;
		}
	}

	public bool[] GetSlotsLockState(WandSlotType type)
	{
		switch (type)
		{
		case WandSlotType.Normal:
			return normalSlotIsLock;
		case WandSlotType.Post:
			return postSlotIsLock;
		default:
			Debug.LogError("不应该返回前中后格子以外的类型来着 如果到这里就有错了 检查一下吧");
			return normalSlotIsLock;
		}
	}

	public void SetSlotsData(WandSlotType type, SlotData[] data)
	{
		switch (type)
		{
		case WandSlotType.Normal:
			normalSlots = data;
			break;
		case WandSlotType.Post:
			postSlots = data;
			break;
		}
	}

	public void SetSlotsLockState(WandSlotType type, bool[] data)
	{
		switch (type)
		{
		case WandSlotType.Normal:
			normalSlotIsLock = data;
			break;
		case WandSlotType.Post:
			postSlotIsLock = data;
			break;
		}
	}

	public float GetExtraMaxMP()
	{
		float num = 0f;
		for (int i = 0; i < normalSlots.Length; i++)
		{
			if (normalSlots[i] != null && !normalSlots[i].isSealSlot && SpellConfig.dic[normalSlots[i].id].abilityType == SpellAbilityType.EmptyContainer)
			{
				num += SpellConfig.dic[normalSlots[i].id].float1;
			}
		}
		for (int j = 0; j < postSlots.Length; j++)
		{
			if (postSlots[j] != null && !postSlots[j].isSealSlot && SpellConfig.dic[postSlots[j].id].abilityType == SpellAbilityType.EmptyContainer)
			{
				num += SpellConfig.dic[postSlots[j].id].float1;
			}
		}
		return num;
	}

	public float GetExtraMPRecovery()
	{
		float num = 0f;
		for (int i = 0; i < normalSlots.Length; i++)
		{
			if (normalSlots[i] != null && !normalSlots[i].isSealSlot && SpellConfig.dic[normalSlots[i].id].abilityType == SpellAbilityType.ManaEssence)
			{
				num += SpellConfig.dic[normalSlots[i].id].float1;
			}
		}
		for (int j = 0; j < postSlots.Length; j++)
		{
			if (postSlots[j] != null && !postSlots[j].isSealSlot && SpellConfig.dic[postSlots[j].id].abilityType == SpellAbilityType.ManaEssence)
			{
				num += SpellConfig.dic[postSlots[j].id].float1;
			}
		}
		return num;
	}

	public float GetExtraShootInterval()
	{
		float num = 0f;
		for (int i = 0; i < normalSlots.Length; i++)
		{
			if (normalSlots[i] != null && !normalSlots[i].isSealSlot)
			{
				num += SpellConfig.dic[normalSlots[i].id].shootIntervalAddSubRevise;
			}
		}
		for (int j = 0; j < postSlots.Length; j++)
		{
			if (postSlots[j] != null && !postSlots[j].isSealSlot)
			{
				num += SpellConfig.dic[postSlots[j].id].shootIntervalAddSubRevise;
			}
		}
		return num;
	}

	public float GetExtraCoolDown()
	{
		float num = 0f;
		for (int i = 0; i < normalSlots.Length; i++)
		{
			if (normalSlots[i] != null && !normalSlots[i].isSealSlot)
			{
				num += SpellConfig.dic[normalSlots[i].id].coolDownAddSubRevise;
			}
		}
		for (int j = 0; j < postSlots.Length; j++)
		{
			if (postSlots[j] != null && !postSlots[j].isSealSlot)
			{
				num += SpellConfig.dic[postSlots[j].id].coolDownAddSubRevise;
			}
		}
		return num;
	}

	public float GetCoolDownRatio()
	{
		float num = 1f;
		for (int i = 0; i < normalSlots.Length; i++)
		{
			if (normalSlots[i] != null && !normalSlots[i].isSealSlot)
			{
				num *= SpellConfig.dic[normalSlots[i].id].coolDownRatio;
			}
		}
		for (int j = 0; j < postSlots.Length; j++)
		{
			if (postSlots[j] != null && !postSlots[j].isSealSlot)
			{
				num *= SpellConfig.dic[postSlots[j].id].coolDownRatio;
			}
		}
		return num;
	}

	public string GetInfo()
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (GetWandScatter() > 0f)
		{
			stringBuilder.Append("◆\u00a0\u200a" + 14000206.GetText(forceApplyAlogia: true) + ": +" + GetWandScatter() + "°");
		}
		else if (GetWandScatter() < 0f)
		{
			stringBuilder.Append("◆\u00a0\u200a" + 14000206.GetText(forceApplyAlogia: true) + ": " + GetWandScatter() + "°");
		}
		if (shootCount > 1)
		{
			if (stringBuilder.Length != 0)
			{
				stringBuilder.Append("\n");
			}
			stringBuilder.Append("◆\u00a0\u200a" + 14000207.GetText(forceApplyAlogia: true) + ": " + shootCount);
		}
		if (costCorrection != 100)
		{
			if (stringBuilder.Length != 0)
			{
				stringBuilder.Append("\n");
			}
			stringBuilder.Append("◆\u00a0\u200a" + 14000305.GetText(forceApplyAlogia: true) + ": ×" + costCorrection + "%");
		}
		if (criticalChance != 0f)
		{
			if (stringBuilder.Length != 0)
			{
				stringBuilder.Append("\n");
			}
			if (criticalChance > 0f)
			{
				stringBuilder.Append("◆\u00a0\u200a" + 14000307.GetText(forceApplyAlogia: true) + ": +" + criticalChance + "%");
			}
			else if (criticalChance < 0f)
			{
				stringBuilder.Append("◆\u00a0\u200a" + 14000307.GetText(forceApplyAlogia: true) + ": " + criticalChance + "%");
			}
		}
		if (damageCorrection != 100f)
		{
			if (stringBuilder.Length != 0)
			{
				stringBuilder.Append("\n");
			}
			stringBuilder.Append("◆\u00a0\u200a" + 14000308.GetText(forceApplyAlogia: true) + ": ×" + damageCorrection + "%");
		}
		string text = (id + 5100000).GetText();
		if (text != "")
		{
			if (stringBuilder.Length != 0)
			{
				stringBuilder.Append("\n");
			}
			stringBuilder.Append("◆\u00a0\u200a" + text);
		}
		float num = float2;
		if (specialAbility == WandAbility.HealNearByTeammate)
		{
			num *= 1f + PlayerMgr.Inst.ExtraRadiusOfInfluence(isSpell: false);
		}
		stringBuilder = stringBuilder.Replace("\\", "\n◆\u00a0\u200a");
		stringBuilder = stringBuilder.Replace("int1", int1.ToString());
		stringBuilder = stringBuilder.Replace("int2", int2.ToString());
		stringBuilder = stringBuilder.Replace("int3", int2.ToString());
		stringBuilder = stringBuilder.Replace("float1", float1.ToString());
		stringBuilder = stringBuilder.Replace("float2", num.ToString());
		stringBuilder = stringBuilder.Replace("float3", float3.ToString());
		if (postSlots.Length != 0)
		{
			if (stringBuilder.Length != 0)
			{
				stringBuilder.Append("\n");
			}
			CheckPostSlotChargeData();
			stringBuilder.Append(GetPostSlotDesc());
		}
		return stringBuilder.ToString();
	}

	public void CheckPostSlotChargeData()
	{
		if (postSlots.Length != 0 && PostslotKillEnemyChargeRatio <= 0f && PostslotStandChargeRatio <= 0f && PostslotMoveChargeRatio <= 0f && PostslotSpellHitChargeRatio <= 0f && PostslotCriticalHitChargeRatio <= 0f && PostslotTakeDamageChargeRatio <= 0f && PostslotTimeChargeRatio <= 0f && PostslotCastSpellChargeRatio <= 0f)
		{
			ResetPostSlot();
		}
	}

	public string GetIconPath()
	{
		if (GameMgr.IsHarmony_Static && iconH != null && iconH != "")
		{
			return "Textures/WandIcons/" + iconH;
		}
		return "Textures/WandIcons/" + icon;
	}

	public static List<int> GetCanDropWandIDs()
	{
		List<int> list = new List<int>();
		for (int i = 0; i < WandConfig.list.Count; i++)
		{
			if (0 < WandConfig.list[i].dropStage && WandConfig.list[i].dropStage < 10)
			{
				list.Add(WandConfig.list[i].id);
			}
		}
		return list;
	}
}
