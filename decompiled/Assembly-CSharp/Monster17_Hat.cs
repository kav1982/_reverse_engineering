using System.Collections.Generic;
using Unity.Collections;
using Unity.Transforms;
using UnityEngine;

public class Monster17_Hat : UnitBase
{
	private float startBounceTimer;

	private bool canBounce;

	private List<SpellAbilityType> allowRebounceType = new List<SpellAbilityType>
	{
		SpellAbilityType.Bullet,
		SpellAbilityType.Rollball,
		SpellAbilityType.Butterfly,
		SpellAbilityType.Laser,
		SpellAbilityType.PreFirework,
		SpellAbilityType.HoverTorch,
		SpellAbilityType.BackMP,
		SpellAbilityType.SnakeWalk,
		SpellAbilityType.Rainbow,
		SpellAbilityType.ArcaneNova,
		SpellAbilityType.Dash,
		SpellAbilityType.ManaCoin,
		SpellAbilityType.Boomerang,
		SpellAbilityType.ShiningStar,
		SpellAbilityType.MrBingArrow,
		SpellAbilityType.DimensionTraveller,
		SpellAbilityType.ShotGun,
		SpellAbilityType.BulletParabola
	};

	private void Start()
	{
		base.Anima.SetTrigger("Jump");
		base.transform.position = Tool2D.GetNavMeshPointIngoreZ(base.transform.position);
	}

	public override void Update()
	{
		base.Update();
		if ((double)startBounceTimer < 0.7)
		{
			startBounceTimer += Time.deltaTime;
		}
		else
		{
			canBounce = true;
		}
	}

	public override void BeforeTakeDamage_Dots(ref TakeDamageInfo_Dots info)
	{
		SpellAbilityType abilityType = info.spell.Config.AbilityType;
		if ((abilityType == SpellAbilityType.Meteor || abilityType == SpellAbilityType.DeathAdder || abilityType == SpellAbilityType.FireBall) && canBounce)
		{
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Monster17_Hat_Dead", Tool2D.IgnoreZPoint(base.transform.position), 2f);
			DotsAnnouncedDeath();
			return;
		}
		info.immuneDamage = true;
		if (!UnitDotsSyncSystem.EntityIsValid(info.spell.Entity) || info.spell.Movement.IsFallSpell || (info.spell.Config.ShooterType != UnitType.Teammate && info.spell.Config.ShooterType != 0) || !allowRebounceType.Contains(info.spell.Config.AbilityType) || !canBounce)
		{
			return;
		}
		UnitDotsSyncSystem.entityMgr.SetComponentEnabled<SpellDestroyTag>(info.spell.Entity, value: true);
		SpellSpawnParams ssp = CreateRebounceSpell(info.spell, GetComponentData<LocalTransform>(info.spell.Entity).Position, this);
		ShootSpell(ssp);
		ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Rebound", Tool2D.GetLayerPoint(ssp.SpawnPosition), Vector3.one * 0.2f, 1f);
		if (ssp.ConfigComponentData.AbilityType != SpellAbilityType.Dash)
		{
			UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
			componentData.unitCfg.currentHP -= ssp.ConfigComponentData.Damage.Base;
			SetComponentData(componentData);
			if (componentData.unitCfg.currentHP <= 0f)
			{
				ObjPoolMgr inst = ObjPoolMgr.Inst;
				FixedString128Bytes deadEF = myPpt.unitCfg.deadEF;
				inst.GetGO("Prefabs/EF/" + deadEF.ToString(), Tool2D.IgnoreZPoint(base.transform.position), 2f);
				DotsAnnouncedDeath();
			}
		}
		else
		{
			ObjPoolMgr inst2 = ObjPoolMgr.Inst;
			FixedString128Bytes deadEF = myPpt.unitCfg.deadEF;
			inst2.GetGO("Prefabs/EF/" + deadEF.ToString(), Tool2D.IgnoreZPoint(base.transform.position), 2f);
			UnitProperty_Dots componentData2 = GetComponentData<UnitProperty_Dots>();
			componentData2.unitCfg.currentHP = 0f;
			SetComponentData(componentData2);
			DotsAnnouncedDeath();
		}
	}

	public static SpellSpawnParams CreateRebounceSpell(TakeDamageInfo_Dots.SpellData sourceSpell, Vector3 position, UnitBase rebounceUnit)
	{
		Vector3 direction = -sourceSpell.Movement.Direction;
		if (sourceSpell.Config.AbilityType == SpellAbilityType.Laser)
		{
			direction = Tool2D.GetDir();
		}
		SpellSpawnParams ssp = UnitDotsSyncSystem.GetSpellPrototype(sourceSpell.Config.Id);
		UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
		if (sourceSpell.Config.AbilityType != SpellAbilityType.SnakeWalk)
		{
			sSPModifier.Speed /= 2f;
		}
		if (sourceSpell.Config.AbilityType == SpellAbilityType.Dash)
		{
			sSPModifier.Penetrate.Base = 99999;
			direction *= -1f;
		}
		sSPModifier.Direction = direction;
		Vector3 spawnPosition = position;
		if (sourceSpell.Config.AbilityType == SpellAbilityType.Laser)
		{
			spawnPosition = rebounceUnit.transform.position + Vector3.back * 0.3f;
		}
		sSPModifier.SpawnPosition = spawnPosition;
		sSPModifier.Damage = Mathf.Clamp(sourceSpell.Config.Damage.Base / 2f, 2f, 20f);
		if (sourceSpell.Config.AbilityType == SpellAbilityType.Dash)
		{
			sSPModifier.Damage = Mathf.Min(sSPModifier.Damage, 12f);
		}
		if (sourceSpell.Config.AbilityType == SpellAbilityType.BulletParabola)
		{
			sSPModifier.Gravity = 13f;
			sSPModifier.CurrentFallSpeed = -4f;
		}
		sSPModifier.CriticalChance = 0f;
		sSPModifier.Shooter = rebounceUnit.myPpt.myEntity;
		sSPModifier.ApplyToSSP(ref ssp);
		ssp.DisableShootSound = true;
		return ssp;
	}
}
