using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public static class SpellSpawnParamsProcessor
{
	private delegate void Processor(ShootSpellUtils.SpellShootInfo shootInfo, SpellInitialParameter sip, ref SpellSpawnParams ssp);

	private const int MANY_REBOUND_COUNT = 15;

	private static readonly Dictionary<SpellAbilityType, Processor> _processors = new Dictionary<SpellAbilityType, Processor>();

	[RuntimeInitializeOnLoadMethod]
	private static void InitializeProcessors()
	{
		_processors.Clear();
		_processors.Add(SpellAbilityType.Butterfly, delegate(ShootSpellUtils.SpellShootInfo info, SpellInitialParameter sip, ref SpellSpawnParams ssp)
		{
			if (ssp.MovementComponentData.IsFallSpell)
			{
				ssp.MovementComponentData.CurrentFallSpeed *= 0.35f;
				ssp.MovementComponentData.Speed *= 0.35f;
				ssp.MovementComponentData.OriginalSpellHorizontalSpeed *= 0.35f;
			}
		});
		_processors.Add(SpellAbilityType.FireBall, delegate(ShootSpellUtils.SpellShootInfo info, SpellInitialParameter sip, ref SpellSpawnParams ssp)
		{
			if (!ssp.MovementComponentData.IsFallSpell)
			{
				ssp.MovementComponentData.ReboundCount += 15;
			}
		});
		_processors.Add(SpellAbilityType.BlackHole, delegate(ShootSpellUtils.SpellShootInfo info, SpellInitialParameter sip, ref SpellSpawnParams ssp)
		{
			if (ssp.MovementComponentData.IsFallSpell)
			{
				ssp.MovementComponentData.Speed *= 6f;
				ssp.MovementComponentData.CurrentFallSpeed *= 6f;
				ssp.MovementComponentData.OriginalSpellHorizontalSpeed *= 6f;
			}
			ssp.radiuDecreaseRatio = sip.radiuDecreaseRatio;
			ssp.radiuDcreaseTransIntoDamageRatio = sip.radiuDcreaseTransIntoDamageRatio;
		});
		_processors.Add(SpellAbilityType.ArcaneExplosion, delegate(ShootSpellUtils.SpellShootInfo info, SpellInitialParameter sip, ref SpellSpawnParams ssp)
		{
			ssp.MovementComponentData.Speed = 0f;
			ssp.ConfigComponentData.Duration = new AttributeValue(0.6f);
		});
		_processors.Add(SpellAbilityType.Rainbow, delegate(ShootSpellUtils.SpellShootInfo info, SpellInitialParameter sip, ref SpellSpawnParams ssp)
		{
			ssp.ConfigComponentData.Penetrate.Base++;
			float num2 = math.max(sip.extraScatter + 45f, 5f);
			float3 oldDir2 = sip.originShootDirection;
			float3 float2 = DTool.GetShiftedDir(in oldDir2, (0f - num2) / 2f + num2 / 7f * (float)ssp.InShootCountIndex);
			if (sip.reverseDirection)
			{
				float2 = -float2;
			}
			ssp.MovementComponentData.Direction = float2;
		});
		_processors.Add(SpellAbilityType.ShotGun, delegate(ShootSpellUtils.SpellShootInfo info, SpellInitialParameter sip, ref SpellSpawnParams ssp)
		{
			sip.equalScatter = true;
			if (ssp.MovementComponentData.Type != SpellSpecialMovementType.Rotation && !ssp.MovementComponentData.IsFallSpell && !info.IsCopyShoot)
			{
				float degree = math.max(sip.extraScatter + 30f, 5f) * ((float)ssp.InShootCountIndex / (1f + (float)ssp.ConfigComponentData.Level) - 0.5f);
				float3 oldDir = sip.originShootDirection;
				float3 @float = DTool.GetShiftedDir(in oldDir, degree);
				if (sip.reverseDirection)
				{
					@float = -@float;
				}
				ssp.MovementComponentData.Direction = @float;
			}
		});
		_processors.Add(SpellAbilityType.MagicBreaker, delegate(ShootSpellUtils.SpellShootInfo info, SpellInitialParameter sip, ref SpellSpawnParams ssp)
		{
			ssp.MovementComponentData.Direction = new float3(ssp.SourceShootDir.xy, 0f);
		});
		_processors.Add(SpellAbilityType.DimensionTraveller, delegate(ShootSpellUtils.SpellShootInfo info, SpellInitialParameter sip, ref SpellSpawnParams ssp)
		{
			ssp.MovementComponentData.Gravity = 0f;
			ssp.MovementComponentData.ChaseMouseLerpSpeed *= 0.25f;
			ssp.MovementComponentData.ChaseRotateSpeed *= 0.3f;
			ssp.ConfigComponentData.Penetrate.Base += 9999999;
		});
		_processors.Add(SpellAbilityType.RuneHammer, delegate(ShootSpellUtils.SpellShootInfo info, SpellInitialParameter sip, ref SpellSpawnParams ssp)
		{
			ssp.radiuDecreaseRatio = sip.radiuDecreaseRatio;
			ssp.radiuDcreaseTransIntoDamageRatio = sip.radiuDcreaseTransIntoDamageRatio;
		});
		_processors.Add(SpellAbilityType.DragonBreath, delegate(ShootSpellUtils.SpellShootInfo info, SpellInitialParameter sip, ref SpellSpawnParams ssp)
		{
			ssp.radiuDecreaseRatio = sip.radiuDecreaseRatio;
			ssp.radiuDcreaseTransIntoDamageRatio = sip.radiuDcreaseTransIntoDamageRatio;
			ssp.ConfigComponentData.Penetrate.Base += 9999999;
		});
		_processors.Add(SpellAbilityType.ArcaneNova, delegate(ShootSpellUtils.SpellShootInfo info, SpellInitialParameter sip, ref SpellSpawnParams ssp)
		{
			if (!ssp.MovementComponentData.IsFallSpell)
			{
				ssp.MovementComponentData.ReboundCount += 15;
			}
			ssp.ConfigComponentData.Penetrate.Base = 9999999;
		});
		_processors.Add(SpellAbilityType.Dash, delegate(ShootSpellUtils.SpellShootInfo info, SpellInitialParameter sip, ref SpellSpawnParams ssp)
		{
			if (!ssp.MovementComponentData.IsFallSpell)
			{
				ssp.MovementComponentData.ReboundCount += 15;
			}
			ssp.ConfigComponentData.Penetrate.Base = 9999999;
			ssp.MovementComponentData.ChaseMouseLerpSpeed = 0.8f;
		});
		_processors.Add(SpellAbilityType.HoverTorch, delegate(ShootSpellUtils.SpellShootInfo info, SpellInitialParameter sip, ref SpellSpawnParams ssp)
		{
			if (!ssp.MovementComponentData.IsFallSpell)
			{
				ssp.MovementComponentData.ReboundCount += 15;
			}
		});
		_processors.Add(SpellAbilityType.PreFirework, delegate(ShootSpellUtils.SpellShootInfo info, SpellInitialParameter sip, ref SpellSpawnParams ssp)
		{
			if (!ssp.MovementComponentData.IsFallSpell)
			{
				ssp.MovementComponentData.ReboundCount += 15;
			}
		});
		_processors.Add(SpellAbilityType.Boomerang, delegate(ShootSpellUtils.SpellShootInfo info, SpellInitialParameter sip, ref SpellSpawnParams ssp)
		{
			ssp.ConfigComponentData.Penetrate.Base += ssp.ConfigComponentData.Int1;
		});
		_processors.Add(SpellAbilityType.Summon1, delegate(ShootSpellUtils.SpellShootInfo info, SpellInitialParameter sip, ref SpellSpawnParams ssp)
		{
			ssp.ConfigComponentData.Duration.Base = 2.1474836E+09f;
		});
		_processors.Add(SpellAbilityType.Summon2, delegate(ShootSpellUtils.SpellShootInfo info, SpellInitialParameter sip, ref SpellSpawnParams ssp)
		{
			ssp.ConfigComponentData.Duration.Base = 2.1474836E+09f;
		});
		_processors.Add(SpellAbilityType.Summon3, delegate(ShootSpellUtils.SpellShootInfo info, SpellInitialParameter sip, ref SpellSpawnParams ssp)
		{
			ssp.ConfigComponentData.Duration.Base = 2.1474836E+09f;
		});
		_processors.Add(SpellAbilityType.Summon4, delegate(ShootSpellUtils.SpellShootInfo info, SpellInitialParameter sip, ref SpellSpawnParams ssp)
		{
			ssp.ConfigComponentData.Duration.Base = 2.1474836E+09f;
			ssp.MovementComponentData.ReboundCount += 15;
			ssp.ConfigComponentData.Penetrate.Base = 9999999;
		});
		_processors.Add(SpellAbilityType.GiantBubble, delegate(ShootSpellUtils.SpellShootInfo info, SpellInitialParameter sip, ref SpellSpawnParams ssp)
		{
			if (ssp.MovementComponentData.IsFallSpell)
			{
				float num = 2f;
				ssp.MovementComponentData.CurrentFallSpeed += num;
				ssp.MovementComponentData.Speed += math.abs(num / math.tan(1.3089969f));
			}
			ssp.radiuDecreaseRatio = sip.radiuDecreaseRatio;
			ssp.radiuDcreaseTransIntoDamageRatio = sip.radiuDcreaseTransIntoDamageRatio;
		});
		_processors.Add(SpellAbilityType.Summon5, delegate(ShootSpellUtils.SpellShootInfo info, SpellInitialParameter sip, ref SpellSpawnParams ssp)
		{
			ssp.ConfigComponentData.Duration.Base = 2.1474836E+09f;
		});
		_processors.Add(SpellAbilityType.Summon6, delegate(ShootSpellUtils.SpellShootInfo info, SpellInitialParameter sip, ref SpellSpawnParams ssp)
		{
			ssp.ConfigComponentData.Duration.Base = 2.1474836E+09f;
			ssp.ConfigComponentData.Int1 = info.ShootData.Spell.specialInt;
			ssp.radiuDecreaseRatio = sip.radiuDecreaseRatio;
			ssp.radiuDcreaseTransIntoDamageRatio = sip.radiuDcreaseTransIntoDamageRatio;
		});
		_processors.Add(SpellAbilityType.Summon7, delegate(ShootSpellUtils.SpellShootInfo info, SpellInitialParameter sip, ref SpellSpawnParams ssp)
		{
			ssp.ConfigComponentData.Duration.Base = 2.1474836E+09f;
		});
		_processors.Add(SpellAbilityType.WandSpirit, delegate(ShootSpellUtils.SpellShootInfo info, SpellInitialParameter sip, ref SpellSpawnParams ssp)
		{
			ssp.ConfigComponentData.Duration.Base = 2.1474836E+09f;
		});
		_processors.Add(SpellAbilityType.DaveHarpoons, delegate(ShootSpellUtils.SpellShootInfo info, SpellInitialParameter sip, ref SpellSpawnParams ssp)
		{
			List<int> list = new List<int>();
			if (PlayerMgr.Inst.ItemCtrller.relic_PowerfulHarpoonHead != null)
			{
				RelicConfig relic_PowerfulHarpoonHead = PlayerMgr.Inst.ItemCtrller.relic_PowerfulHarpoonHead;
				AttributeValue damage = ssp.ConfigComponentData.Damage;
				damage.AddRatio += (float)relic_PowerfulHarpoonHead.int1.result / 100f;
				ssp.ConfigComponentData.Damage = damage;
			}
			if (PlayerMgr.Inst.ItemCtrller.relic_FlameHarpoonHead != null)
			{
				RelicConfig relic_FlameHarpoonHead = PlayerMgr.Inst.ItemCtrller.relic_FlameHarpoonHead;
				AttributeValue damage2 = ssp.ConfigComponentData.Damage;
				damage2.AddRatio += (float)relic_FlameHarpoonHead.int1.result / 100f;
				ssp.ConfigComponentData.Damage = damage2;
				ssp.ElementComponentData.FireBurnDuration = math.max(ssp.ElementComponentData.FireBurnDuration, relic_FlameHarpoonHead.int2.result);
				ssp.ElementComponentData.FireHpBurnPercent += relic_FlameHarpoonHead.float1.result / 100f;
				list.Add(1);
			}
			if (PlayerMgr.Inst.ItemCtrller.relic_FrozenHarpoonHead != null)
			{
				RelicConfig relic_FrozenHarpoonHead = PlayerMgr.Inst.ItemCtrller.relic_FrozenHarpoonHead;
				ssp.ElementComponentData.FrozenDuration += relic_FrozenHarpoonHead.float1.result;
				list.Add(2);
			}
			if (PlayerMgr.Inst.ItemCtrller.relic_PoisonousHarpoonHead != null)
			{
				RelicConfig relic_PoisonousHarpoonHead = PlayerMgr.Inst.ItemCtrller.relic_PoisonousHarpoonHead;
				ssp.ElementComponentData.VenomApplyCount += relic_PoisonousHarpoonHead.int1.result;
				list.Add(3);
			}
			if (list.Count > 0)
			{
				if (ssp.ConfigComponentData.ColorType != 0)
				{
					list.Add(0);
				}
				int index = UnityEngine.Random.Range(0, list.Count);
				switch (list[index])
				{
				case 1:
					ssp.ConfigComponentData.ColorType = SpellColorType.Fire;
					break;
				case 2:
					ssp.ConfigComponentData.ColorType = SpellColorType.Frozen;
					break;
				case 3:
					ssp.ConfigComponentData.ColorType = SpellColorType.Venom;
					break;
				}
			}
		});
		_processors.Add(SpellAbilityType.LaserBeam, delegate(ShootSpellUtils.SpellShootInfo info, SpellInitialParameter sip, ref SpellSpawnParams ssp)
		{
			if (sip.ThunderElementData.ThunderHitChance > 0f && ssp.ConfigComponentData.ColorType != 0 && UnityEngine.Random.Range(0, 1) == 0)
			{
				ssp.ConfigComponentData.ColorType = SpellColorType.Thunder;
			}
		});
		_processors.Add(SpellAbilityType.RedRune, delegate(ShootSpellUtils.SpellShootInfo info, SpellInitialParameter sip, ref SpellSpawnParams ssp)
		{
			ssp.ConfigComponentData.Duration = new AttributeValue(0.35f);
			if (!sip.spellIsFall)
			{
				ssp.MovementComponentData.Speed = 0f;
			}
		});
		_processors.Add(SpellAbilityType.GreenRune, delegate(ShootSpellUtils.SpellShootInfo info, SpellInitialParameter sip, ref SpellSpawnParams ssp)
		{
			ssp.ConfigComponentData.Duration.Base = 2.1474836E+09f;
			if (!sip.spellIsFall)
			{
				ssp.MovementComponentData.Speed = 0f;
			}
		});
		_processors.Add(SpellAbilityType.BlueRune, delegate(ShootSpellUtils.SpellShootInfo info, SpellInitialParameter sip, ref SpellSpawnParams ssp)
		{
			ssp.ConfigComponentData.Duration.Base = math.max(ssp.ConfigComponentData.Duration.Base, 2.5f);
		});
	}

	public static void Process(ShootSpellUtils.SpellShootInfo info, SpellInitialParameter sip, ref SpellSpawnParams ssp)
	{
		if (_processors.TryGetValue(ssp.ConfigComponentData.AbilityType, out var value))
		{
			value(info, sip, ref ssp);
		}
	}
}
