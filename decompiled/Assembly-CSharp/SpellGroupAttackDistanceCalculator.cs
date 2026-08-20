using Unity.Mathematics;
using UnityEngine;

public static class SpellGroupAttackDistanceCalculator
{
	public static float GetGroupChargeDuration(SpellShootGroup targetGroup)
	{
		if (targetGroup == null)
		{
			return 0f;
		}
		float num = 100f;
		bool flag = false;
		SpellShootData[] shoots = targetGroup.Shoots;
		foreach (SpellShootData spellShootData in shoots)
		{
			SpellConfig finalConfig = spellShootData.Spell.GetFinalConfig();
			float x = 0f;
			bool flag2 = false;
			switch (finalConfig.abilityType)
			{
			case SpellAbilityType.ShiningStar:
				flag2 = true;
				x = Mathf.Max(0f, (finalConfig.float3 / 100f - spellShootData.GetSpellCriticalChance().Result) / (finalConfig.float1 / 100f));
				break;
			case SpellAbilityType.SuperNova:
				flag2 = true;
				x = 6f;
				break;
			}
			if (flag2)
			{
				flag = true;
				num = math.min(x, num);
			}
		}
		if (!flag)
		{
			return 0f;
		}
		return num;
	}

	public static float SpellGroupAttackDuration(SpellShootGroup targetGroup, Wand targetWand)
	{
		if (targetGroup == null)
		{
			return 0f;
		}
		float num = 0f;
		SpellShootData[] shoots = targetGroup.Shoots;
		foreach (SpellShootData spellShootData in shoots)
		{
			float num2 = 0f;
			SpellSpecialMovementType spellSpecialMovementType = SpellSpecialMovementType.Normal;
			bool flag = false;
			SpellConfig finalConfig = spellShootData.Spell.GetFinalConfig();
			float num3 = 0f;
			for (int j = 0; j < spellShootData.EnhanceList.Length; j++)
			{
				SpellConfig finalConfig2 = spellShootData.EnhanceList[j].GetFinalConfig();
				switch (finalConfig2.abilityType)
				{
				case SpellAbilityType.AroundOwner:
					spellSpecialMovementType = SpellSpecialMovementType.Rotation;
					num3 += finalConfig2.float1;
					break;
				case SpellAbilityType.AroundMouse:
					spellSpecialMovementType = SpellSpecialMovementType.ChaseMouse;
					break;
				case SpellAbilityType.FollowTarget:
					spellSpecialMovementType = SpellSpecialMovementType.ChaseEnemy;
					break;
				case SpellAbilityType.Fall:
					flag = true;
					break;
				}
			}
			if (flag)
			{
				return 0f;
			}
			switch (finalConfig.abilityType)
			{
			case SpellAbilityType.DisintegrationRay:
			case SpellAbilityType.Dash:
			case SpellAbilityType.HighPressureWasher:
			case SpellAbilityType.DragonBreath:
				num2 += spellShootData.GetSpellDuration_FinalPlayerValue(targetWand).Result;
				if (spellSpecialMovementType == SpellSpecialMovementType.Rotation)
				{
					num2 += num3;
				}
				break;
			}
			num = math.max(num2, num);
		}
		return num;
	}

	public static bool SpellGroupWillIgnoreWall(SpellShootGroup targetGroup, Wand targetWand)
	{
		if (targetGroup == null)
		{
			return false;
		}
		if (PlayerMgr.Inst.ItemCtrller.relic_SpellThroughWall)
		{
			return true;
		}
		SpellShootData[] shoots = targetGroup.Shoots;
		int num = 0;
		if (num < shoots.Length)
		{
			SpellShootData spellShootData = shoots[num];
			SpellSpecialMovementType spellSpecialMovementType = SpellSpecialMovementType.Normal;
			bool flag = false;
			SpellConfig finalConfig = spellShootData.Spell.GetFinalConfig();
			for (int i = 0; i < spellShootData.EnhanceList.Length; i++)
			{
				switch (spellShootData.EnhanceList[i].GetFinalConfig().abilityType)
				{
				case SpellAbilityType.AroundOwner:
					spellSpecialMovementType = SpellSpecialMovementType.Rotation;
					break;
				case SpellAbilityType.FollowTarget:
				case SpellAbilityType.FollowOwner:
					spellSpecialMovementType = SpellSpecialMovementType.ChaseEnemy;
					break;
				case SpellAbilityType.AroundMouse:
					spellSpecialMovementType = SpellSpecialMovementType.ChaseMouse;
					break;
				case SpellAbilityType.Fall:
					flag = true;
					break;
				}
			}
			if (flag || spellSpecialMovementType == SpellSpecialMovementType.Rotation)
			{
				return true;
			}
			switch (finalConfig.abilityType)
			{
			case SpellAbilityType.Bullet:
			case SpellAbilityType.Rollball:
			case SpellAbilityType.Butterfly:
			case SpellAbilityType.Laser:
			case SpellAbilityType.PreFirework:
			case SpellAbilityType.HoverTorch:
			case SpellAbilityType.BackMP:
			case SpellAbilityType.DisintegrationRay:
			case SpellAbilityType.FireBall:
			case SpellAbilityType.Rainbow:
			case SpellAbilityType.ArcaneNova:
			case SpellAbilityType.Dash:
			case SpellAbilityType.HighPressureWasher:
			case SpellAbilityType.ManaCoin:
			case SpellAbilityType.JudgementBlade:
			case SpellAbilityType.GiantBubble:
			case SpellAbilityType.ShiningStar:
			case SpellAbilityType.MrBingArrow:
			case SpellAbilityType.ShotGun:
				return false;
			default:
				return true;
			}
		}
		return false;
	}

	public static float GetMinAttackDistance(SpellShootGroup targetGroup, Wand targetWand, bool getMaxDistance = false)
	{
		if (targetGroup == null)
		{
			Debug.LogError("\ufffd\ufffd\ufffd\ufffd\ufffd\u02ffյķ\ufffd\ufffd\ufffd\ufffd\ufffd");
			return (!getMaxDistance) ? 100 : 0;
		}
		float num = (getMaxDistance ? 0.5f : 100f);
		SpellShootData[] shoots = targetGroup.Shoots;
		foreach (SpellShootData spellShootData in shoots)
		{
			RatioValue spellDuration_FinalPlayerValue = spellShootData.GetSpellDuration_FinalPlayerValue(targetWand);
			RatioValue spellMoveSpeed_FinalPlayerValue = spellShootData.GetSpellMoveSpeed_FinalPlayerValue(targetWand);
			RatioValue spellEffectRadius_FinalPlayerValue = spellShootData.GetSpellEffectRadius_FinalPlayerValue(targetWand);
			RatioValue spellEffectRadius_FinalPlayerValue2 = spellShootData.GetSpellEffectRadius_FinalPlayerValue(targetWand);
			float num2 = 0f;
			float num3 = 0f;
			float num4 = 0f;
			SpellSpecialMovementType spellSpecialMovementType = SpellSpecialMovementType.Normal;
			bool flag = false;
			bool flag2 = false;
			SpellConfig finalConfig = spellShootData.Spell.GetFinalConfig();
			for (int j = 0; j < spellShootData.EnhanceList.Length; j++)
			{
				SpellConfig finalConfig2 = spellShootData.EnhanceList[j].GetFinalConfig();
				switch (finalConfig2.abilityType)
				{
				case SpellAbilityType.AroundOwner:
					num4 = Mathf.Max(num2, finalConfig2.float3);
					spellSpecialMovementType = SpellSpecialMovementType.Rotation;
					break;
				case SpellAbilityType.AroundMouse:
					spellSpecialMovementType = SpellSpecialMovementType.ChaseMouse;
					break;
				case SpellAbilityType.FollowTarget:
					spellSpecialMovementType = SpellSpecialMovementType.ChaseEnemy;
					break;
				case SpellAbilityType.Fall:
					flag2 = true;
					break;
				}
			}
			num4 *= spellEffectRadius_FinalPlayerValue2.CurrentFinalRatio;
			if (spellSpecialMovementType == SpellSpecialMovementType.Rotation)
			{
				flag = true;
				bool flag3 = false;
				if (finalConfig.abilityType == SpellAbilityType.JudgementBlade)
				{
					num2 = finalConfig.float1 * spellEffectRadius_FinalPlayerValue2.CurrentFinalRatio;
					flag3 = true;
				}
				if (!flag3)
				{
					num2 = num4;
				}
			}
			else if (flag2)
			{
				num2 = 20f;
				flag = true;
			}
			else if (spellSpecialMovementType == SpellSpecialMovementType.ChaseMouse && finalConfig.abilityType == SpellAbilityType.JudgementBlade)
			{
				num2 = 20f;
				flag = true;
			}
			if (!flag)
			{
				switch (finalConfig.abilityType)
				{
				case SpellAbilityType.Bullet:
				case SpellAbilityType.Rollball:
				case SpellAbilityType.PreFirework:
				case SpellAbilityType.BlackHole:
				case SpellAbilityType.BackMP:
				case SpellAbilityType.ManaCoin:
				case SpellAbilityType.ShiningStar:
					num2 = spellMoveSpeed_FinalPlayerValue.Result * spellDuration_FinalPlayerValue.Result;
					break;
				case SpellAbilityType.Meteor:
				case SpellAbilityType.DeathAdder:
					num2 = 20f;
					break;
				case SpellAbilityType.Laser:
					num2 = 5f + spellMoveSpeed_FinalPlayerValue.Result;
					break;
				case SpellAbilityType.Butterfly:
					num2 = spellMoveSpeed_FinalPlayerValue.Result * spellDuration_FinalPlayerValue.Result * finalConfig.float2;
					break;
				case SpellAbilityType.HoverTorch:
					num2 = spellMoveSpeed_FinalPlayerValue.Result * spellDuration_FinalPlayerValue.Result * 0.5f;
					break;
				case SpellAbilityType.ArcaneExplosion:
					num2 = spellEffectRadius_FinalPlayerValue.Result;
					break;
				case SpellAbilityType.SnakeWalk:
					num2 = spellMoveSpeed_FinalPlayerValue.Result * spellDuration_FinalPlayerValue.Result * 0.8f;
					break;
				case SpellAbilityType.DisintegrationRay:
					num2 = spellMoveSpeed_FinalPlayerValue.Result * 0.75f;
					break;
				case SpellAbilityType.Rainbow:
					num2 = spellMoveSpeed_FinalPlayerValue.Result / finalConfig.float1 * 0.8f;
					break;
				case SpellAbilityType.ArcaneNova:
					num3 += spellMoveSpeed_FinalPlayerValue.Result * spellDuration_FinalPlayerValue.Result * 0.5f;
					num2 = Mathf.Max(3f, spellMoveSpeed_FinalPlayerValue.Result * spellDuration_FinalPlayerValue.Result * 1.5f - num3);
					break;
				case SpellAbilityType.FireBall:
					num2 = spellMoveSpeed_FinalPlayerValue.Result * 0.5f;
					break;
				case SpellAbilityType.ThunderAura:
					num2 = Mathf.Max(spellEffectRadius_FinalPlayerValue.Result, spellMoveSpeed_FinalPlayerValue.Result * spellDuration_FinalPlayerValue.Result);
					break;
				case SpellAbilityType.HighPressureWasher:
					num2 = 8f;
					break;
				case SpellAbilityType.MagicBreaker:
					num2 = (float)(spellEffectRadius_FinalPlayerValue.BaseValue * (double)Mathf.Max(0.75f, spellEffectRadius_FinalPlayerValue.CurrentFinalRatio)) * Mathf.Pow(spellShootData.GetSpellDamage_FinalPlayerValue(targetWand).Result / finalConfig.damage, 0.3333f);
					break;
				case SpellAbilityType.Boomerang:
					num2 = 8f;
					break;
				case SpellAbilityType.JudgementBlade:
					num2 = finalConfig.float1 * spellEffectRadius_FinalPlayerValue.CurrentFinalRatio;
					break;
				case SpellAbilityType.GiantBubble:
					num2 = ((!(spellMoveSpeed_FinalPlayerValue.Result <= 0f)) ? Mathf.Min(10f, spellMoveSpeed_FinalPlayerValue.Result * spellDuration_FinalPlayerValue.Result) : spellEffectRadius_FinalPlayerValue2.Result);
					break;
				case SpellAbilityType.DragonBreath:
					num2 = finalConfig.float1 + 0.2f * spellMoveSpeed_FinalPlayerValue.Result;
					break;
				case SpellAbilityType.Harpoons:
					num2 = 6f + spellMoveSpeed_FinalPlayerValue.CurrentAddBase + spellDuration_FinalPlayerValue.CurrentAddBase;
					break;
				case SpellAbilityType.ShotGun:
					num2 = (3.5f + (float)finalConfig.level) * spellMoveSpeed_FinalPlayerValue.CurrentMulRatio + spellMoveSpeed_FinalPlayerValue.CurrentAddBase * 1f + spellDuration_FinalPlayerValue.Result * 1.5f;
					if (spellSpecialMovementType == SpellSpecialMovementType.ChaseMouse || spellSpecialMovementType == SpellSpecialMovementType.ChaseEnemy)
					{
						num2 *= 1.25f;
					}
					break;
				default:
					num2 = 20f;
					break;
				}
			}
			num = (getMaxDistance ? Mathf.Max(num, num2 + num3) : Mathf.Min(num, num2 + num3));
		}
		return num;
	}

	public static SpellSpecialMovementType GetShootGroupMovementType(SpellShootGroup targetGroup, Wand targetWand)
	{
		if (targetGroup == null)
		{
			Debug.LogError("\ufffd\ufffd\ufffd\ufffd\ufffd\u02ffյķ\ufffd\ufffd\ufffd\ufffd\ufffd");
			return SpellSpecialMovementType.Normal;
		}
		SpellSpecialMovementType result = SpellSpecialMovementType.Normal;
		SpellShootData[] shoots = targetGroup.Shoots;
		foreach (SpellShootData spellShootData in shoots)
		{
			for (int j = 0; j < spellShootData.EnhanceList.Length; j++)
			{
				switch (spellShootData.EnhanceList[j].GetFinalConfig().abilityType)
				{
				case SpellAbilityType.AroundOwner:
					result = SpellSpecialMovementType.Rotation;
					break;
				case SpellAbilityType.AroundMouse:
					result = SpellSpecialMovementType.ChaseMouse;
					break;
				case SpellAbilityType.FollowTarget:
					result = SpellSpecialMovementType.ChaseEnemy;
					break;
				}
			}
		}
		return result;
	}
}
