using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

public class SpellShootGroup
{
	[NotNull]
	public SpellShootData[] Shoots = Array.Empty<SpellShootData>();

	[CanBeNull]
	public SpellShootData OwnerShootData;

	public SpellShootGroup Copy(SpellShootData overwriteOwnerShootData = null)
	{
		SpellShootGroup newGroup = new SpellShootGroup
		{
			OwnerShootData = overwriteOwnerShootData
		};
		newGroup.Shoots = Shoots.Select((SpellShootData s) => s.Copy(newGroup)).ToArray();
		return newGroup;
	}

	public float GetGroupHighestRecoil()
	{
		(float, float) groupRecoilData = GetGroupRecoilData();
		if (groupRecoilData.Item1 * groupRecoilData.Item2 >= 0f)
		{
			float result;
			if (Mathf.Abs(groupRecoilData.Item1) >= Mathf.Abs(groupRecoilData.Item2))
			{
				(result, _) = groupRecoilData;
			}
			else
			{
				result = groupRecoilData.Item2;
			}
			return result;
		}
		return groupRecoilData.Item1 + groupRecoilData.Item2;
	}

	public (float highestRecoil, float lowestRecoil) GetGroupRecoilData()
	{
		float[] array = (from e in Shoots
			where e.GetSpellFallExplosionRadius() <= 0f
			select e.GetSpellRecoil()).ToArray();
		if (array.Length != 0)
		{
			return (array.Max(), array.Min());
		}
		return (0f, 0f);
	}

	public RatioValue GetGroupDamageRatio()
	{
		RatioValue ratioValue = new RatioValue(0.0);
		if (OwnerShootData == null)
		{
			return ratioValue;
		}
		SpellConfig finalConfig = OwnerShootData.Spell.GetFinalConfig();
		if (finalConfig.abilityType == SpellAbilityType.ArcaneNova)
		{
			ratioValue.MulRatio((float)finalConfig.int2 / 100f);
		}
		ratioValue.Merge(OwnerShootData.OwnerGroup.GetGroupDamageRatio());
		return ratioValue;
	}

	public float GetGroupShootScatterFromVolley(SpellShootData shootData)
	{
		if (!Shoots.Contains(shootData))
		{
			throw new Exception("法术组解析错误，这个组中不存在想要获取散射的法术");
		}
		SpellConfig maxLevelVolley = shootData.GetMaxLevelVolley();
		if (maxLevelVolley == null)
		{
			return 0f;
		}
		int num = GetGroupTotalShootDataForVolley().ToArray().Length;
		return maxLevelVolley.int2 * (num - 1);
	}

	public float GetGroupManaCost(float summonTypeSpellManaCostRatio)
	{
		return Shoots.Sum((SpellShootData e) => e.GetSpellManaCostAndSubGroupManaCost(summonTypeSpellManaCostRatio));
	}

	public float GetGroupManaCostRatio(SpellShootData shootData)
	{
		return GetGroupManaCostRatioFromVolley(shootData) * GetGroupManaCostRatioFromMultiShoot() * GetGroupManaCostRatioFromParentSpell();
	}

	private float GetGroupManaCostRatioFromVolley(SpellShootData shootData)
	{
		SpellConfig maxLevelVolley = shootData.GetMaxLevelVolley();
		if (maxLevelVolley == null)
		{
			return 1f;
		}
		int num = GetGroupTotalShootDataForVolley().ToArray().Length;
		return Mathf.Pow(maxLevelVolley.float1 / 100f, num - 1);
	}

	private float GetGroupManaCostRatioFromMultiShoot()
	{
		if (OwnerShootData == null)
		{
			return 1f;
		}
		float groupManaCostRatioFromMultiShoot = OwnerShootData.OwnerGroup.GetGroupManaCostRatioFromMultiShoot();
		return (from e in OwnerShootData.EnhanceList
			select e.GetFinalConfig() into e
			where e.abilityType == SpellAbilityType.Multishot
			select e).Aggregate(groupManaCostRatioFromMultiShoot, (float f, SpellConfig cfg) => f * (cfg.mpCostMulDivCorrection / 100f));
	}

	private float GetGroupManaCostRatioFromParentSpell()
	{
		float num = 1f;
		if (OwnerShootData == null)
		{
			return num;
		}
		SpellConfig finalConfig = OwnerShootData.Spell.GetFinalConfig();
		SpellAbilityType abilityType = finalConfig.abilityType;
		if (abilityType == SpellAbilityType.ArcaneNova || abilityType == SpellAbilityType.PreFirework)
		{
			num *= finalConfig.float1 / 100f;
		}
		return num * OwnerShootData.OwnerGroup.GetGroupManaCostRatioFromParentSpell();
	}

	private IEnumerable<SpellShootData> GetGroupTotalShootDataForVolley()
	{
		if (OwnerShootData != null)
		{
			return OwnerShootData.OwnerGroup.GetGroupTotalShootDataForVolley();
		}
		List<SpellShootData> list = new List<SpellShootData>();
		Queue<SpellShootGroup> queue = new Queue<SpellShootGroup>();
		queue.Enqueue(this);
		SpellShootGroup result;
		while (queue.TryDequeue(out result))
		{
			list.AddRange(result.Shoots);
			foreach (SpellShootData item in result.Shoots.Where((SpellShootData e) => e.SubGroup != null))
			{
				queue.Enqueue(item.SubGroup);
			}
		}
		return list;
	}

	private IEnumerable<SlotData> GetAllSlotData(bool deepSearch)
	{
		SpellShootData[] shoots = Shoots;
		foreach (SpellShootData shoot in shoots)
		{
			yield return shoot.Spell;
			if (!deepSearch || shoot.SubGroup == null)
			{
				continue;
			}
			foreach (SlotData allSlotDatum in shoot.SubGroup.GetAllSlotData(deepSearch: true))
			{
				yield return allSlotDatum;
			}
		}
	}

	public bool HasShootableSpell(SpellAbilityType abilityType, bool deepSearch)
	{
		return GetAllSlotData(deepSearch).Any((SlotData e) => e.GetFinalConfig().abilityType == abilityType);
	}

	public bool HasSomeCastTypeSpell(SpellType spellType, bool deepSearch)
	{
		return GetAllSlotData(deepSearch).Any((SlotData e) => e.GetFinalConfig().useType == spellType);
	}
}
