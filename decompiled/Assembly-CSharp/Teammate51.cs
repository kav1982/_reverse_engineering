using System.Collections.Generic;
using SpriteEffectSystem;
using Unity.Mathematics;
using UnityEngine;

public class Teammate51 : UnitBase
{
	private enum MonsterState
	{
		BornIdle,
		Idle,
		IdleWalk,
		RunToTarget,
		SuicideExlode
	}

	public VariableFloat idleTime;

	public VariableFloat idleWalkTime;

	public VariableFloat idleWalkRadius;

	public float idleWalkSpeedRatio;

	public float recheckInterval;

	public float lifetime;

	private float realLifeTime;

	public VariableFloat bornAddForce;

	[Header("Explode")]
	public int suicideExplodeDamage;

	private int damage;

	public float suicideExplodeDetectRadiu;

	private float damageDetectRadiu;

	[Header("Color")]
	public SpriteRenderer sr;

	public SpriteRenderer srFire;

	public Material mat_ECFrozen;

	public Material mat_ECMucus;

	public Material mat_ECPlayer;

	public Material mat_ECVenom;

	public Material mat_ECVoid;

	public SpriteEffectAnima ExplosionAnima;

	private float recheckPositionTimer;

	private MonsterState state;

	private float idleTimer;

	private float idleWalkTimer;

	private float recheckIntervalTimer;

	private float lifetimeTimer;

	private float effectrRadiuscale = 1f;

	private float effectFinalRadiuRatio = 1f;

	private float hoverTime;

	private float frozenTime;

	private float burnDamage;

	private float burnTime;

	private float mucusMoveSpeedEffect = 1f;

	private float mucusSpellSpeedEffect = 1f;

	private float mucusTime;

	private float venomTime;

	private float venomApplyCount;

	private float explodeCriticalChance;

	private WandPostSlotChargeData chargeData;

	private float knockBackRatio = 1f;

	private float finalknockBackRatio = 1f;

	private float criticalThunderDamageRatio;

	private float criticalThunderRange;

	private float criticalThunderTargetsCount;

	private float criticalThunderDragForce;

	private SpellColorType colorType;

	private Spell3129VoidExplosion.VoidExplosionData explosionData;

	public void CheckEnchanceEffect(SpellBase targetbase)
	{
		if (!(targetbase != null))
		{
			return;
		}
		damage = Mathf.CeilToInt(targetbase.damageRatio * (float)suicideExplodeDamage * targetbase.finalDamageRatio);
		damageDetectRadiu = suicideExplodeDetectRadiu * targetbase.radiusRatio;
		effectrRadiuscale = targetbase.radiusRatio;
		effectFinalRadiuRatio = targetbase.finalRadiusRatio;
		hoverTime = targetbase.SpellHoverTime;
		realLifeTime = lifetime;
		chargeData = targetbase.wandChargeData;
		explodeCriticalChance = targetbase.GetCriticalChance();
		state = MonsterState.BornIdle;
		knockBackRatio = targetbase.knockbackRatio;
		finalknockBackRatio = targetbase.finalKnockbackRatio;
		frozenTime += targetbase.spellFrozenTime;
		mucusMoveSpeedEffect = targetbase.spellMucusMoveSpeedRatio;
		mucusSpellSpeedEffect = targetbase.spellMucusSpellSpeedRatio;
		mucusTime += targetbase.spellMucusTime;
		venomTime += targetbase.spellVenomTime;
		venomApplyCount = targetbase.spellVenomOnceCount;
		burnDamage = targetbase.burnHpRatioPerSeconds;
		burnTime = targetbase.spellBurnTime;
		criticalThunderDamageRatio = targetbase.criticalDragDamagePercent;
		criticalThunderRange = targetbase.criticalDragEffectRadiu;
		criticalThunderTargetsCount = targetbase.criticalDragApllyToCount;
		criticalThunderDragForce = targetbase.criticalDragPullForce;
		colorType = targetbase.ColorType;
		base.transform.localScale = Vector3.one * targetbase.spellVolumeRatio;
		explosionData = targetbase.voidExplosionInfo;
		srFire.gameObject.SetActive(value: false);
		switch (targetbase.ColorType)
		{
		case SpellColorType.Frozen:
			if (sr.material != mat_ECFrozen)
			{
				sr.material = mat_ECFrozen;
			}
			break;
		case SpellColorType.Mucus:
			if (sr.material != mat_ECMucus)
			{
				sr.material = mat_ECMucus;
			}
			break;
		case SpellColorType.Fire:
			if (sr.material != mat_ECPlayer)
			{
				sr.material = mat_ECPlayer;
			}
			srFire.gameObject.SetActive(value: true);
			break;
		case SpellColorType.Player:
		case SpellColorType.Thunder:
			if (sr.material != mat_ECPlayer)
			{
				sr.material = mat_ECPlayer;
			}
			break;
		case SpellColorType.Venom:
			if (sr.material != mat_ECVenom)
			{
				sr.material = mat_ECVenom;
			}
			break;
		case SpellColorType.Void:
			if (sr.material != mat_ECVoid)
			{
				sr.material = mat_ECVoid;
			}
			break;
		default:
			Debug.LogError(base.SummonerSpellBase.ColorType);
			break;
		}
	}

	private void CreateExplosionEffect()
	{
		SpellSpriteEffectController.Inst.PlayEffect(ExplosionAnima, new EffectPlayParam
		{
			Position = base.transform.position,
			Scale = Vector3.one * effectrRadiuscale * effectFinalRadiuRatio,
			FilpX = (UnityEngine.Random.Range(0, 2) == 0)
		});
	}

	public override void EveryInitialCallback()
	{
		idleTimer = 0f;
		idleWalkTimer = 0f;
		recheckIntervalTimer = 0f;
		lifetimeTimer = 0f;
		hoverTime = 0f;
		state = MonsterState.BornIdle;
		frozenTime = 0f;
		mucusMoveSpeedEffect = 1f;
		mucusSpellSpeedEffect = 1f;
		mucusTime = 0f;
		venomTime = 0f;
		venomApplyCount = 0f;
		recheckPositionTimer = 0f;
		chargeData = null;
		explodeCriticalChance = 0f;
		burnDamage = 0f;
		burnTime = 0f;
		float value = UnityEngine.Random.Range(-100f, 100f);
		sr.material.SetFloat("_TimeOffset", value);
		srFire.material.SetFloat("_TimeOffset", value);
	}

	public override void Update()
	{
		lifetimeTimer += Time.deltaTime;
		if (lifetimeTimer >= realLifeTime && state != MonsterState.SuicideExlode)
		{
			if (hoverTime > 0f)
			{
				hoverTime -= Time.deltaTime;
				SetMove(Vector3.zero, isFlip: false);
				base.Anima.SetTrigger("Idle");
			}
			else
			{
				CreateExplosionEffect();
				myPpt.TakeDamage(9999f, myPpt, new TakeDamageInfo
				{
					isFloatText = false
				});
				ObjPoolMgr.Inst.RecycleGO(base.gameObject);
			}
		}
		base.Update();
		if (base.IsLocked)
		{
			return;
		}
		switch (state)
		{
		case MonsterState.BornIdle:
			myPpt.TakeKnockback(Tool2D.GetDir() * bornAddForce.RandomResult());
			state = MonsterState.Idle;
			GetNavInfo(base.transform.position);
			break;
		case MonsterState.Idle:
			if (lifetimeTimer >= realLifeTime)
			{
				state = MonsterState.RunToTarget;
				break;
			}
			SetMove(Vector3.zero, isFlip: false);
			idleTimer += Time.deltaTime;
			if (idleTimer >= idleTime.result)
			{
				idleTimer = 0f;
				GetNearestTarget();
				if (base.HaveTarget)
				{
					state = MonsterState.RunToTarget;
					base.Anima.SetTrigger("Run");
					break;
				}
				state = MonsterState.IdleWalk;
				base.Anima.SetTrigger("IdleWalk");
				GetNavInfo(Tool2D.GetNavMeshPoint(base.transform.position, idleWalkRadius));
				idleWalkTime.RandomResult();
			}
			break;
		case MonsterState.IdleWalk:
			if (lifetimeTimer >= realLifeTime)
			{
				state = MonsterState.RunToTarget;
				break;
			}
			if (navInfo.allCornerArrived)
			{
				GetNavInfo(Tool2D.GetNavMeshPoint(base.transform.position, idleWalkRadius));
			}
			else
			{
				SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed * idleWalkSpeedRatio);
				CheckNavInfo();
			}
			idleWalkTimer += Time.deltaTime;
			if (idleWalkTimer >= idleWalkTime.result)
			{
				idleWalkTimer = 0f;
				GetNearestTarget();
				if (base.HaveTarget)
				{
					state = MonsterState.RunToTarget;
					base.Anima.SetTrigger("Run");
				}
				else
				{
					state = MonsterState.Idle;
					base.Anima.SetTrigger("Idle");
					idleTime.RandomResult();
				}
			}
			break;
		case MonsterState.RunToTarget:
			if (!base.HaveTarget)
			{
				GetNearestTarget();
			}
			if (!base.HaveTarget)
			{
				state = MonsterState.Idle;
				base.Anima.SetTrigger("Idle");
				idleTime.RandomResult();
				break;
			}
			recheckIntervalTimer += Time.deltaTime;
			if (recheckIntervalTimer >= recheckInterval)
			{
				recheckIntervalTimer = 0f;
				GetNearestTarget();
			}
			if (ToTargetDistance() < targetPpt.UnitBas.GetBodyColliderRadius() + damageDetectRadiu)
			{
				state = MonsterState.SuicideExlode;
				if (targetPpt != null)
				{
					if (explosionData != null)
					{
						targetPpt.SetVoid(explosionData);
					}
					TakeDamageInfo takeDamageInfo = targetPpt.TakeDamage(damage, myPpt, new TakeDamageInfo
					{
						canRebound = false,
						criticalChance = explodeCriticalChance
					});
					SetElementEffect(targetPpt);
					if (takeDamageInfo.isCriticalDamage && criticalThunderDamageRatio > 0f)
					{
						ActivePullCrystalAttack(targetPpt);
					}
					if (chargeData != null && chargeData.chargeTargetWand != null && chargeData.chargeType == WandPostSlotTriggerType.SpellHit)
					{
						chargeData.chargeTargetWand.ChargePostSlots(chargeData.chargeRatioAmount);
					}
					else if (chargeData != null && chargeData.chargeTargetWand != null && chargeData.chargeType == WandPostSlotTriggerType.KillEnemy && takeDamageInfo.isTargetDead)
					{
						chargeData.chargeTargetWand.ChargePostSlots(chargeData.chargeRatioAmount);
					}
					else if (chargeData != null && chargeData.chargeTargetWand != null && chargeData.chargeType == WandPostSlotTriggerType.CriticalHit && takeDamageInfo.isCriticalDamage)
					{
						chargeData.chargeTargetWand.ChargePostSlots(chargeData.chargeRatioAmount);
					}
				}
				CreateExplosionEffect();
				myPpt.TakeDamage(9999f, myPpt, new TakeDamageInfo
				{
					isFloatText = false
				});
				ObjPoolMgr.Inst.RecycleGO(base.gameObject);
			}
			else
			{
				recheckPositionTimer += Time.deltaTime;
				if (recheckPositionTimer >= Time.deltaTime && lifetimeTimer < realLifeTime)
				{
					recheckPositionTimer = 0f;
					GetNavInfo(base.TargetPoint);
					SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed);
				}
			}
			break;
		default:
			Debug.LogError(state);
			break;
		case MonsterState.SuicideExlode:
			break;
		}
	}

	public override void SummonsThrough()
	{
		base.SummonerSpellBase.SpellSummonAfterDeadSpawnWormCount = 0;
		base.SummonsThrough();
		myPpt.AnnouncedDeath(new TakeDamageInfo
		{
			isPlayDeadSE = false,
			isCreateDeadEF = false,
			isTeammateThrough = true
		});
	}

	public void SetElementEffect(UnitProperty targetPpt)
	{
		if (mucusTime > 0f)
		{
			targetPpt.SetMucus(mucusTime, mucusMoveSpeedEffect, mucusSpellSpeedEffect);
		}
		if (venomTime > 0f)
		{
			targetPpt.SetVenom(venomTime, venomApplyCount);
		}
		if (frozenTime > 0f)
		{
			targetPpt.SetFrozen(frozenTime);
		}
		if (burnTime > 0f)
		{
			targetPpt.SetBurn(burnTime, burnDamage);
		}
		if (explosionData != null)
		{
			targetPpt.SetVoid(explosionData);
		}
	}

	public void ActivePullCrystalAttack(UnitProperty targetPpt)
	{
		float radius = criticalThunderRange * effectrRadiuscale * effectFinalRadiuRatio;
		Vector3 position = targetPpt.transform.position;
		int num = Mathf.CeilToInt(criticalThunderDamageRatio * (float)damage);
		List<Collider> collidersByTag = GeneralTool.GetCollidersByTag(position, radius, "Monster");
		collidersByTag = GeneralTool.ListShuffle(collidersByTag);
		bool flag = false;
		int num2 = (int)Mathf.Min(collidersByTag.Count, criticalThunderTargetsCount);
		for (int i = 0; i < collidersByTag.Count; i++)
		{
			UnitProperty component = collidersByTag[i].gameObject.GetComponent<UnitProperty>();
			if (collidersByTag[i] != null && collidersByTag[i].gameObject.activeInHierarchy && component != null && component != targetPpt)
			{
				TakeDamageInfo takeDamageInfo = new TakeDamageInfo
				{
					canRebound = false,
					damage = num,
					attackerPpt = PlayerMgr.Inst.PlayerPpt,
					criticalChance = explodeCriticalChance
				};
				takeDamageInfo.beHitPpt = component;
				if (mucusTime > 0f)
				{
					takeDamageInfo.beHitPpt.SetMucus(mucusTime, mucusMoveSpeedEffect, mucusSpellSpeedEffect);
				}
				if (venomTime > 0f)
				{
					takeDamageInfo.beHitPpt.SetVenom(venomTime, venomApplyCount);
				}
				if (frozenTime > 0f)
				{
					takeDamageInfo.beHitPpt.SetFrozen(frozenTime);
				}
				if (burnTime > 0f)
				{
					takeDamageInfo.beHitPpt.SetBurn(burnTime, burnDamage);
				}
				flag = true;
				Spell3101PullCrystal component2 = ObjPoolMgr.Inst.GetGO("Prefabs/Spell/" + 31121, base.transform.position, quaternion.identity, 0.5f).GetComponent<Spell3101PullCrystal>();
				if (component2.tsf_Layer != null)
				{
					component2.tsf_Layer.localScale = base.transform.localScale;
				}
				component2.SetColor(colorType);
				component2.SetChainTargetTransform(targetPpt.gameObject.transform, collidersByTag[i].transform);
				component.TakeKnockback((targetPpt.transform.position - collidersByTag[i].transform.position).normalized * criticalThunderDragForce * knockBackRatio * finalknockBackRatio);
				component.TakeDamage(damage, myPpt, takeDamageInfo);
				component2.CreateHitEffect(colorType, collidersByTag[i].transform.position + component2.chainBaseHeight);
				num2--;
				if (num2 <= 0)
				{
					break;
				}
			}
		}
		if (flag)
		{
			SEMgr.Inst.spell3121Energy.PlaySE().pitch = UnityEngine.Random.Range(0.5f, 1.5f);
		}
	}

	private void OnDisable()
	{
		base.Rigid.isKinematic = false;
		myPpt.Affect_InAbyss = false;
		ObjPoolMgr.Inst.RecycleGO(base.gameObject);
	}
}
