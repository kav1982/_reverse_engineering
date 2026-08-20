using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Unity.Mathematics;
using UnityEngine;

public class SpellShootData
{
	private enum OverSplitTriggerCountMode
	{
		Count,
		Sum
	}

	[NotNull]
	public readonly SlotData Spell;

	[NotNull]
	public readonly SpellShootGroup OwnerGroup;

	[NotNull]
	public SlotData[] EnhanceList = Array.Empty<SlotData>();

	[NotNull]
	public SlotData[] Triggers = Array.Empty<SlotData>();

	[CanBeNull]
	public SpellShootGroup SubGroup;

	public SpellShootData(SlotData spell, SpellShootGroup ownerGroup)
	{
		Spell = spell;
		OwnerGroup = ownerGroup;
	}

	public SpellShootData Copy([NotNull] SpellShootGroup ownerGroup)
	{
		if (ownerGroup == null)
		{
			throw new Exception("ShootData \ufffd\ufffd OwnerGroup \ufffd\ufffd\ufffd\ufffdΪ\ufffd\ufffd");
		}
		SpellShootData spellShootData = new SpellShootData(Spell.Copy(), ownerGroup)
		{
			EnhanceList = EnhanceList.Select((SlotData e) => e.Copy()).ToArray(),
			Triggers = Triggers.Select((SlotData e) => e.Copy()).ToArray()
		};
		spellShootData.SubGroup = SubGroup?.Copy(spellShootData);
		return spellShootData;
	}

	public int GetMaxMeantimeShootCount(int baseMeantimeShootCount)
	{
		int num = baseMeantimeShootCount;
		SlotData[] enhanceList = EnhanceList;
		for (int i = 0; i < enhanceList.Length; i++)
		{
			SpellConfig finalConfig = enhanceList[i].GetFinalConfig();
			int num2 = num;
			num = num2 + finalConfig.abilityType switch
			{
				SpellAbilityType.Volley => finalConfig.int1, 
				SpellAbilityType.TotalScattering => finalConfig.int1, 
				_ => 0, 
			};
		}
		return num;
	}

	public bool GetSpellIgnoreWall()
	{
		if (PlayerMgr.Inst.ItemCtrller.relic_SpellThroughWall)
		{
			return true;
		}
		return false;
	}

	public RatioValue GetSpellFlySpeed()
	{
		RatioValue ratioValue = new RatioValue(Spell.GetFinalConfig().speed);
		SlotData[] enhanceList = EnhanceList;
		for (int i = 0; i < enhanceList.Length; i++)
		{
			SpellConfig finalConfig = enhanceList[i].GetFinalConfig();
			switch (finalConfig.abilityType)
			{
			case SpellAbilityType.AroundMouse:
			case SpellAbilityType.EnhanceSpeedValue:
				ratioValue.AddBase(finalConfig.float1);
				break;
			case SpellAbilityType.SpeedToDuration:
				ratioValue.MulRatio(finalConfig.float1 / 100f);
				break;
			}
		}
		return ratioValue;
	}

	public (int count, float gap) GetSpellMultiShootData()
	{
		SpellConfig[] array = (from e in EnhanceList
			select e.GetFinalConfig() into e
			where e.abilityType == SpellAbilityType.Multishot
			select e).ToArray();
		int num = 1;
		float num2 = 0f;
		if (array.Length == 0)
		{
			return (num, num2);
		}
		num += array.Sum((SpellConfig e) => e.int1);
		num2 += array.Max((SpellConfig e) => e.float1);
		return (num, num2);
	}

	public RatioValue GetSpellScatter()
	{
		RatioValue ratioValue = new RatioValue(Spell.GetFinalConfig().angle);
		ratioValue.AddBase(OwnerGroup.GetGroupShootScatterFromVolley(this));
		foreach (SpellConfig item in EnhanceList.Select((SlotData e) => e.GetFinalConfig()))
		{
			ratioValue.AddBase(item.angle);
		}
		return ratioValue;
	}

	public RatioValue GetSpellDuration()
	{
		RatioValue ratioValue = new RatioValue(Spell.GetFinalConfig().duration);
		foreach (SpellConfig item in EnhanceList.Select((SlotData e) => e.GetFinalConfig()))
		{
			switch (item.abilityType)
			{
			case SpellAbilityType.EnhanceDurationValue:
				ratioValue.AddBase(item.float1);
				break;
			case SpellAbilityType.PowerSavingMode:
				ratioValue.MulRatio(item.float2 / 100f);
				break;
			case SpellAbilityType.RandomRotationRadiu:
				ratioValue.AddBase(item.float3);
				break;
			case SpellAbilityType.AroundMouse:
				ratioValue.AddBase(item.float2);
				break;
			}
		}
		return ratioValue;
	}

	public RatioValue GetSummonHp()
	{
		SpellConfig finalConfig = Spell.GetFinalConfig();
		if (finalConfig.summonID == 0)
		{
			return new RatioValue(0.0);
		}
		RatioValue ratioValue = new RatioValue(UnitConfig.map[finalConfig.summonID].maxHP);
		SlotData[] enhanceList = EnhanceList;
		for (int i = 0; i < enhanceList.Length; i++)
		{
			SpellConfig finalConfig2 = enhanceList[i].GetFinalConfig();
			switch (finalConfig2.abilityType)
			{
			case SpellAbilityType.EnhanceSummonHPRecover:
				ratioValue.AddRatio(finalConfig2.float2 / 100f);
				break;
			case SpellAbilityType.TeammateSacrifice:
				ratioValue.MulRatio(finalConfig2.float1 / 100f);
				break;
			}
		}
		return ratioValue;
	}

	public float GetSummonHpRecover()
	{
		return (from e in EnhanceList
			select e.GetFinalConfig() into e
			where e.abilityType == SpellAbilityType.EnhanceSummonHPRecover
			select e).Sum((SpellConfig e) => e.float1);
	}

	public int GetSummonSpiritEssenceLevel()
	{
		return (from e in EnhanceList
			select e.GetFinalConfig() into e
			where e.abilityType == SpellAbilityType.TeammateSprite
			select e).Sum((SpellConfig e) => e.int1);
	}

	public RatioValue GetSummonAttackSpeedRatio()
	{
		RatioValue ratioValue = new RatioValue(0.0);
		foreach (SpellConfig item in from e in EnhanceList
			select e.GetFinalConfig() into e
			where e.abilityType == SpellAbilityType.ParasiticWorm
			select e)
		{
			ratioValue.AddRatio(item.float2 / 100f);
		}
		return ratioValue;
	}

	public RatioValue GetSummonMoveSpeedRatio()
	{
		RatioValue ratioValue = new RatioValue(0.0);
		SlotData[] enhanceList = EnhanceList;
		for (int i = 0; i < enhanceList.Length; i++)
		{
			SpellConfig finalConfig = enhanceList[i].GetFinalConfig();
			if (finalConfig.abilityType == SpellAbilityType.ParasiticWorm)
			{
				ratioValue.AddRatio(finalConfig.float3 / 100f);
			}
		}
		return ratioValue;
	}

	public int GetSummonLimit()
	{
		return Spell.GetFinalConfig().summonLimit;
	}

	public int GetSplitCount()
	{
		return (from e in EnhanceList
			select e.GetFinalConfig() into e
			where e.abilityType == SpellAbilityType.SpellSplit
			select e).Sum((SpellConfig e) => e.int1);
	}

	public RatioValue GetSpellCriticalChance()
	{
		RatioValue ratioValue = new RatioValue(Spell.GetFinalConfig().criticalChance / 100f);
		foreach (SpellConfig item in EnhanceList.Select((SlotData e) => e.GetFinalConfig()))
		{
			ratioValue.AddBase(item.criticalChance / 100f);
		}
		return ratioValue;
	}

	public RatioValue GetSpellEffectRadius()
	{
		RatioValue ratioValue = new RatioValue(Spell.GetFinalConfig().radius);
		SlotData[] enhanceList = EnhanceList;
		for (int i = 0; i < enhanceList.Length; i++)
		{
			SpellConfig finalConfig = enhanceList[i].GetFinalConfig();
			switch (finalConfig.abilityType)
			{
			case SpellAbilityType.EnhanceRadiusRatio:
				ratioValue.AddRatio(finalConfig.float1 / 100f);
				break;
			case SpellAbilityType.RadiuRatioDown:
				ratioValue.MulRatio(finalConfig.float1 / 100f);
				break;
			}
		}
		return ratioValue;
	}

	public (float extraSpeed, float extraDuration, float rotationRadiu) GetSpellRotationInfo(bool forceRandomRadius)
	{
		float num = 0f;
		float num2 = 0f;
		float num3 = 0f;
		bool flag = forceRandomRadius;
		SlotData[] enhanceList = EnhanceList;
		for (int i = 0; i < enhanceList.Length; i++)
		{
			SpellConfig finalConfig = enhanceList[i].GetFinalConfig();
			switch (finalConfig.abilityType)
			{
			case SpellAbilityType.AroundOwner:
				num += finalConfig.float2;
				num2 += finalConfig.float1;
				num3 = finalConfig.float3;
				break;
			case SpellAbilityType.RandomRotationRadiu:
				num3 = finalConfig.float3;
				flag = true;
				break;
			}
		}
		if (flag)
		{
			if (num3 <= 0f)
			{
				num3 += 2f;
			}
			num3 = UnityEngine.Random.Range(num3 * 0.5f, num3 * 1.5f);
		}
		return (num, num2, num3);
	}

	public (int count, float radius) GetHalfLifeTeleportData()
	{
		float item = 0f;
		int num = 0;
		SlotData[] enhanceList = EnhanceList;
		for (int i = 0; i < enhanceList.Length; i++)
		{
			SpellConfig finalConfig = enhanceList[i].GetFinalConfig();
			if (finalConfig.abilityType == SpellAbilityType.RandomTeleport)
			{
				item = finalConfig.float1;
				num++;
			}
		}
		return (num, item);
	}

	public (float radiusRatio, float ratioToDamage) GetSpellDecreaseRadiuSpeedInfo()
	{
		float num = 1f;
		float num2 = 0f;
		SlotData[] enhanceList = EnhanceList;
		for (int i = 0; i < enhanceList.Length; i++)
		{
			SpellConfig finalConfig = enhanceList[i].GetFinalConfig();
			if (finalConfig.abilityType == SpellAbilityType.RadiuRatioDown)
			{
				num *= finalConfig.float1 / 100f;
				num2 += finalConfig.float2 / 100f;
			}
		}
		return (num, num2);
	}

	public (float speedRatio, float speedToDurationRatio) GetSpeedToDurationData()
	{
		SpellConfig[] array = EnhanceList.Select((SlotData e) => e.GetFinalConfig()).ToArray();
		float num = 1f;
		float item = 0f;
		SpellConfig[] array2 = array;
		foreach (SpellConfig spellConfig in array2)
		{
			if (spellConfig.abilityType == SpellAbilityType.SpeedToDuration)
			{
				num *= spellConfig.float1 / 100f;
				item = spellConfig.float2;
			}
		}
		return (num, item);
	}

	public float GetSpellVolumeRatio()
	{
		float num = 0f;
		SlotData[] enhanceList = EnhanceList;
		for (int i = 0; i < enhanceList.Length; i++)
		{
			SpellConfig finalConfig = enhanceList[i].GetFinalConfig();
			if (finalConfig.abilityType == SpellAbilityType.FatSpell)
			{
				num += finalConfig.float1 / 100f;
			}
		}
		return num;
	}

	public (float DamageRange, float PullForce, int PullCount) GetSpellGravitationalCrystalData()
	{
		float num = 0f;
		float num2 = 0f;
		int num3 = 0;
		SlotData[] enhanceList = EnhanceList;
		for (int i = 0; i < enhanceList.Length; i++)
		{
			SpellConfig finalConfig = enhanceList[i].GetFinalConfig();
			if (finalConfig.abilityType == SpellAbilityType.PullForceCrystal)
			{
				num = math.max(num, finalConfig.float1);
				num2 = math.max(num2, finalConfig.float2);
				num3 += finalConfig.int2;
			}
		}
		return (num, num2, num3);
	}

	public int GetSpellPenetrateCount()
	{
		int num = 0;
		SlotData[] enhanceList = EnhanceList;
		for (int i = 0; i < enhanceList.Length; i++)
		{
			SpellConfig finalConfig = enhanceList[i].GetFinalConfig();
			if (finalConfig.abilityType == SpellAbilityType.Penetrate)
			{
				num += finalConfig.int1;
			}
		}
		return num;
	}

	public float GetSpellHoverDuration()
	{
		float num = 0f;
		SlotData[] enhanceList = EnhanceList;
		for (int i = 0; i < enhanceList.Length; i++)
		{
			SpellConfig finalConfig = enhanceList[i].GetFinalConfig();
			if (finalConfig.abilityType == SpellAbilityType.SpellHover)
			{
				num += finalConfig.float1;
			}
		}
		return num;
	}

	public (float duration, float count) GetVenomElementData()
	{
		float num = 0f;
		float num2 = 0f;
		SlotData[] enhanceList = EnhanceList;
		for (int i = 0; i < enhanceList.Length; i++)
		{
			SpellConfig finalConfig = enhanceList[i].GetFinalConfig();
			if (finalConfig.abilityType == SpellAbilityType.VenomCrystal)
			{
				num = math.max(num, finalConfig.float1);
				num2 += (float)finalConfig.int2;
			}
		}
		return (num, num2);
	}

	public (float moveSpeedRatio, float spellSpeedRatio, float duration) GetMucusElementData()
	{
		float num = 0f;
		float num2 = 1f;
		float num3 = 1f;
		SlotData[] enhanceList = EnhanceList;
		for (int i = 0; i < enhanceList.Length; i++)
		{
			SpellConfig finalConfig = enhanceList[i].GetFinalConfig();
			if (finalConfig.abilityType == SpellAbilityType.MucusCrystal)
			{
				num = math.max(num, finalConfig.float1);
				num2 *= finalConfig.float2 / 100f;
				num3 *= finalConfig.float3 / 100f;
			}
		}
		return (num2, num3, num);
	}

	public (float burnPercent, float duration) GetFireElementData()
	{
		float num = 0f;
		float num2 = 0f;
		SlotData[] enhanceList = EnhanceList;
		for (int i = 0; i < enhanceList.Length; i++)
		{
			SpellConfig finalConfig = enhanceList[i].GetFinalConfig();
			if (finalConfig.abilityType == SpellAbilityType.FireCrystal)
			{
				num = math.max(num, finalConfig.float2);
				num2 += (float)finalConfig.int1 / 100f;
			}
		}
		return (num2, num);
	}

	public (float ThunderHitChance, float ThunderHitRadius, float ThunderHitDamageRatio) GetThunderElementData()
	{
		float num = 0f;
		float num2 = 0f;
		float num3 = 0f;
		SlotData[] enhanceList = EnhanceList;
		for (int i = 0; i < enhanceList.Length; i++)
		{
			SpellConfig finalConfig = enhanceList[i].GetFinalConfig();
			if (finalConfig.abilityType == SpellAbilityType.ThunderCrystal)
			{
				num = math.max(num, finalConfig.float3 / 100f);
				num2 = math.max(num2, finalConfig.float1);
				num3 += finalConfig.float2 / 100f;
			}
		}
		return (num, num2, num3);
	}

	public float GetFrozenElementData()
	{
		float num = 0f;
		SlotData[] enhanceList = EnhanceList;
		for (int i = 0; i < enhanceList.Length; i++)
		{
			SpellConfig finalConfig = enhanceList[i].GetFinalConfig();
			if (finalConfig.abilityType == SpellAbilityType.Frozen)
			{
				num = math.max(num, finalConfig.float1);
			}
		}
		return num;
	}

	public (float VoidInstantKillThreshold, float VoidExplosionHpDamageRatio, float VoidExplosionRange, float VoidDuration) GetVoidElementData()
	{
		float num = 0f;
		float num2 = 0f;
		float num3 = 0f;
		float item = 0f;
		SlotData[] enhanceList = EnhanceList;
		for (int i = 0; i < enhanceList.Length; i++)
		{
			SpellConfig finalConfig = enhanceList[i].GetFinalConfig();
			if (finalConfig.abilityType == SpellAbilityType.DeathInfect)
			{
				num = math.max(num, finalConfig.float3 / 100f);
				num2 += finalConfig.float2 / 100f;
				num3 = math.max(num3, finalConfig.float1);
			}
		}
		if (num > 0f)
		{
			item = 5f;
		}
		return (num, num2, num3, item);
	}

	public RatioValue GetSpellDamage()
	{
		RatioValue damage = CanShootSpellUtils.GetDamage(Spell.GetFinalConfig().damage, EnhanceList.Select((SlotData e) => e.GetFinalConfig()).ToArray(), Spell.GetFinalConfig());
		damage.Merge(OwnerGroup.GetGroupDamageRatio());
		return damage;
	}

	public (int count, float radius)? GetSpellRefractionInfo()
	{
		SpellConfig[] array = (from e in EnhanceList
			select e.GetFinalConfig() into e
			where e.abilityType == SpellAbilityType.Refraction
			select e).ToArray();
		if (array.Length == 0)
		{
			return null;
		}
		float item = array.Max((SpellConfig e) => e.float1);
		return (array.Sum((SpellConfig e) => e.int1), item);
	}

	public float GetSpellRecoil()
	{
		return Spell.GetFinalConfig().recoil;
	}

	public float GetChaseEnemyRotateSpeed()
	{
		return (from e in EnhanceList
			select e.GetFinalConfig() into e
			where e.abilityType == SpellAbilityType.FollowTarget
			select e).Sum((SpellConfig e) => e.float1);
	}

	public float GetChaseOwnerRotateSpeed()
	{
		return (from e in EnhanceList
			select e.GetFinalConfig() into e
			where e.abilityType == SpellAbilityType.FollowOwner
			select e).Sum((SpellConfig e) => e.float1);
	}

	public float GetChaseMouseLerpSpeed()
	{
		return (from e in EnhanceList
			select e.GetFinalConfig() into e
			where e.abilityType == SpellAbilityType.AroundMouse
			select e).Sum((SpellConfig e) => e.float3);
	}

	public bool GetSpellIsEndTeleport()
	{
		return EnhanceList.Any((SlotData e) => e.GetFinalConfig().abilityType == SpellAbilityType.SpellEndTeleport);
	}

	public (int count, float reboundAddTime) GetReboundTime_FinalPlayerValue()
	{
		int num = 0;
		float num2 = 0f;
		SlotData[] enhanceList = EnhanceList;
		for (int i = 0; i < enhanceList.Length; i++)
		{
			SpellConfig finalConfig = enhanceList[i].GetFinalConfig();
			if (finalConfig.abilityType == SpellAbilityType.Rebound)
			{
				num += finalConfig.int1;
				num2 = Mathf.Max(num2, finalConfig.float1);
			}
		}
		return (num, num2);
	}

	public float GetSpellFallExplosionRadius()
	{
		return (from e in EnhanceList
			select e.GetFinalConfig() into e
			where e.abilityType == SpellAbilityType.Fall
			select e).Sum((SpellConfig e) => e.float1);
	}

	public float GetSpellManaCostAndSubGroupManaCost(float summonTypeSpellManaCostRatio)
	{
		float num = GetSpellManaCost(summonTypeSpellManaCostRatio).Result;
		if (SubGroup != null && NeedCostSubGroupMana())
		{
			num += SubGroup.GetGroupManaCost(summonTypeSpellManaCostRatio);
		}
		return num;
	}

	public RatioValue GetSpellManaCost(float summonTypeSpellManaCostRatio)
	{
		SpellConfig finalConfig = Spell.GetFinalConfig();
		RatioValue ratioValue = new RatioValue(finalConfig.mpCost);
		float groupManaCostRatio = OwnerGroup.GetGroupManaCostRatio(this);
		ratioValue.MulRatio(groupManaCostRatio);
		if (finalConfig.useType == SpellType.Summon)
		{
			ratioValue.MulRatio(summonTypeSpellManaCostRatio);
		}
		if (OwnerGroup.OwnerShootData != null)
		{
			int numberOfSubGroupShoot = OwnerGroup.OwnerShootData.GetNumberOfSubGroupShoot(OverSplitTriggerCountMode.Count);
			ratioValue.MulRatio((numberOfSubGroupShoot == 0) ? OwnerGroup.OwnerShootData.Spell.GetFinalConfig().shootCount : numberOfSubGroupShoot);
		}
		SlotData[] enhanceList = EnhanceList;
		for (int i = 0; i < enhanceList.Length; i++)
		{
			SpellConfig finalConfig2 = enhanceList[i].GetFinalConfig();
			switch (finalConfig2.abilityType)
			{
			case SpellAbilityType.Multishot:
			case SpellAbilityType.SpellSplit:
			case SpellAbilityType.PowerSavingMode:
			case SpellAbilityType.OverDrive:
				ratioValue.MulRatio(finalConfig2.mpCostMulDivCorrection / 100f);
				break;
			}
		}
		return ratioValue;
	}

	public bool NeedCostSubGroupMana()
	{
		if (SubGroup == null)
		{
			return false;
		}
		return GetNumberOfSubGroupShoot(OverSplitTriggerCountMode.Count) > 0;
	}

	private int GetNumberOfSubGroupShoot(OverSplitTriggerCountMode splitMode)
	{
		int num = GetNumberOfSubGroupShootIgnoreParent(splitMode);
		if (OwnerGroup.OwnerShootData != null)
		{
			num *= OwnerGroup.OwnerShootData.GetNumberOfSubGroupShoot(OverSplitTriggerCountMode.Sum);
		}
		return num;
	}

	private int GetNumberOfSubGroupShootIgnoreParent(OverSplitTriggerCountMode splitMode)
	{
		int num = 0;
		if (Spell.GetFinalConfig().isParentTypeSpell)
		{
			num++;
		}
		num += Triggers.Count((SlotData e) => e.GetFinalConfig().abilityType == SpellAbilityType.OnOverTrigger);
		IEnumerable<SpellConfig> source = from e in Triggers
			select e.GetFinalConfig() into e
			where e.abilityType == SpellAbilityType.OnStartRotationTrigger
			select e;
		int num2 = num;
		num = num2 + splitMode switch
		{
			OverSplitTriggerCountMode.Count => source.Count(), 
			OverSplitTriggerCountMode.Sum => source.Sum((SpellConfig e) => e.int2), 
			_ => 0, 
		};
		IEnumerable<SpellConfig> source2 = from e in Triggers
			select e.GetFinalConfig() into e
			where e.abilityType == SpellAbilityType.OnOverSplitTrigger
			select e;
		int num3 = num;
		num = num3 + splitMode switch
		{
			OverSplitTriggerCountMode.Count => source2.Count(), 
			OverSplitTriggerCountMode.Sum => source2.Sum((SpellConfig e) => e.int2), 
			_ => 0, 
		};
		return num * Spell.GetFinalConfig().shootCount;
	}
}
