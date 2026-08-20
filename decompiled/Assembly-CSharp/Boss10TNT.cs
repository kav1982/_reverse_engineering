using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class Boss10TNT : UnitBase
{
	public SpriteRenderer spriteRenderer;

	public Sprite[] sprites;

	public SpriteRenderer warningRenderer;

	public Color warningColor;

	[Header("自爆")]
	public ShockParam shockParam;

	public float knockback;

	public float boomRadius;

	public int boomDamage;

	public float boomPercent;

	public bool isDead;

	[Header("子弹")]
	public float spellHeight;

	public float spellDuration;

	public int spellDamage;

	public int spellCount;

	public float spellGravity;

	public VariableFloat spellUpspeed;

	public VariableFloat spellVerticalSpeed;

	public bool rotateRight;

	private SpellSpawnParams ssp;

	private bool isBornFinish;

	public bool disableDeadBoom;

	private List<UnitDotsSyncSystem.DistanceHitResult> distanceHits = new List<UnitDotsSyncSystem.DistanceHitResult>();

	public override void EveryInitialCallback()
	{
		isDead = false;
		spriteRenderer.sprite = sprites[Random.Range(0, 3)];
		base.Anima.Play("Born");
		disableDeadBoom = false;
		UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
		componentData.InvincibleRegister();
		SetComponentData(componentData);
		isBornFinish = false;
		warningRenderer.transform.position = Tool2D.GetLayerPoint(base.transform.position, LayerCorrectType.GroundEffect);
		warningRenderer.transform.localScale = Vector3.one * boomRadius;
	}

	public override void SingleInitialCallback()
	{
		ssp = UnitDotsSyncSystem.GetSpellPrototype(90431);
		UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
		sSPModifier.Duration = spellDuration;
		sSPModifier.Damage = spellDamage;
		sSPModifier.Shooter = myPpt.myEntity;
		sSPModifier.Gravity = 0f - spellGravity;
		sSPModifier.ApplyToSSP(ref ssp);
		myPpt.RemoveSRFromArray(warningRenderer);
	}

	public override void BeforeAnnouncedDeath_Dots(ref TakeDamageInfo_Dots info)
	{
		if (!isDead)
		{
			isDead = true;
			base.Anima.Play("Boom");
			base.BeforeAnnouncedDeath_Dots(ref info);
			info.stopAnnouncedDeath = true;
			UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
			componentData.InvincibleRegister();
			SetComponentData(componentData);
		}
	}

	public override void BeforeTakeDamage_Dots(ref TakeDamageInfo_Dots info)
	{
		if (isBornFinish)
		{
			if (info.attackerEntity == Boss10.Inst.myPpt.myEntity)
			{
				info.damage *= 50f;
				info.ignoreFloatText = true;
			}
			if (!isDead)
			{
				base.Anima.Play("Hurt");
			}
		}
	}

	public override void AfterDead(ref TakeDamageInfo_Dots info)
	{
		base.AfterDead(ref info);
		if (!disableDeadBoom)
		{
			ExplodeOnce(base.transform.position);
		}
		Boss10.Inst.tntList.Remove(this);
		UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
		for (int i = 0; i < spellCount; i++)
		{
			sSPModifier.CurrentFallSpeed = 0f - spellUpspeed.RandomResult();
			sSPModifier.Speed = spellVerticalSpeed.RandomResult();
			sSPModifier.Direction = Tool2D.GetDir();
			sSPModifier.SpawnPosition = base.transform.position + Vector3.down * spellHeight;
			sSPModifier.ApplyToSSP(ref ssp);
			ShootSpell(ssp);
		}
	}

	private void ExplodeOnce(Vector3 explodePoint)
	{
		ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss10Explosion", explodePoint, 6f).transform.localScale = new Vector3(boomRadius / 2f, boomRadius / 2f, 1f);
		ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Monster34_Trace", explodePoint, 10f);
		CamController.Inst.SetShock(shockParam);
		SEMgr.Inst.boss10_Explosion.PlaySE();
		UnitDotsSyncSystem.GetCollidersInRange(explodePoint, boomRadius, GameConst.Filter_MonsterAoeUndiffer, distanceHits);
		for (int i = 0; i < distanceHits.Count; i++)
		{
			Entity entity = distanceHits[i].entity;
			uint layer = UnitDotsSyncSystem.GetLayer(entity);
			switch (layer)
			{
			case 16777216u:
			{
				UnitDotsSyncSystem.ProcessHitSpell(entity, boomDamage, out var _);
				break;
			}
			case 512u:
			case 2097152u:
			{
				TakeDamageInfo_Dots info3 = TakeDamageInfo_Dots.NewInfo(Boss10.Inst.myPpt.myEntity);
				info3.knockbackForce = Tool2D.IgnoreZV2ToV1Normal(distanceHits[i].point, explodePoint) * knockback;
				info3.damage = boomDamage;
				info3.isUndifferDamage = true;
				if (layer == 131072)
				{
					info3.ignoreFloatText = true;
				}
				UnitDotsSyncSystem.AddTakeDamageRequest(entity, info3);
				break;
			}
			case 32768u:
			case 131072u:
			{
				TakeDamageInfo_Dots info2 = TakeDamageInfo_Dots.NewInfo(Boss10.Inst.myPpt.myEntity);
				info2.knockbackForce = Tool2D.IgnoreZV2ToV1Normal(distanceHits[i].point, explodePoint) * knockback;
				info2.damage = boomDamage * 5;
				info2.isUndifferDamage = true;
				if (layer == 131072)
				{
					info2.ignoreFloatText = true;
				}
				UnitDotsSyncSystem.AddTakeDamageRequest(entity, info2);
				break;
			}
			case 2048u:
			case 4096u:
			case 8192u:
			{
				TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(Boss10.Inst.myPpt.myEntity);
				info.knockbackForce = Tool2D.IgnoreZV2ToV1Normal(distanceHits[i].point, explodePoint) * knockback;
				info.damage = Boss10.Inst.myPpt.unitCfg.maxHP * boomPercent;
				info.isUndifferDamage = true;
				UnitDotsSyncSystem.AddTakeDamageRequest(entity, info);
				break;
			}
			}
		}
	}

	public override void AnimaAction(string animaName)
	{
		if (!(animaName == "ShowFinish"))
		{
			if (animaName == "Boom")
			{
				DotsAnnouncedDeath();
			}
		}
		else
		{
			UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
			componentData.InvincibleUnregister();
			SetComponentData(componentData);
			isBornFinish = true;
		}
	}
}
