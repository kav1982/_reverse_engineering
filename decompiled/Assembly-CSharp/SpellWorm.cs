using System.Collections.Generic;
using SpriteEffectSystem;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

public class SpellWorm : MonoBehaviour
{
	private enum MonsterState
	{
		BornIdle,
		Idle,
		IdleWalk,
		RunToTarget,
		Suicide
	}

	public SpriteEffectAnima idleAnima;

	public SpriteEffectAnima runAnima;

	public LocalSpriteEffectPlayer spritePlayer;

	public SpriteRenderer sprite;

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

	[HideInInspector]
	public float damage;

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

	private float recheckPositionTimer;

	public SpriteEffectAnima ExplosionAnima;

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

	private float criticalDamageRatio;

	private float criticalRange;

	private float criticalTargetsCount;

	private float criticalDragForce;

	private SpellColorType colorType;

	private Spell3129VoidExplosion.VoidExplosionData explosionData;

	private UnitProperty targetPpt;

	public float moveSpeed = 4f;

	private float lastRefindPathTime;

	private NavMeshPath navPath;

	private int currentNavPoint;

	private void Start()
	{
		spritePlayer.Play(idleAnima);
	}

	private void OnEnable()
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
		navPath = null;
		currentNavPoint = 0;
	}

	private void Update()
	{
		lifetimeTimer += Time.deltaTime;
		if (lifetimeTimer >= realLifeTime && state != MonsterState.Suicide)
		{
			if (hoverTime > 0f)
			{
				hoverTime -= Time.deltaTime;
				spritePlayer.Play(idleAnima);
			}
			else
			{
				CreateExplosionEffect();
				ObjPoolMgr.Inst.RecycleGO(base.gameObject);
			}
		}
		if (navPath != null)
		{
			MoveByNavPath();
		}
		switch (state)
		{
		case MonsterState.BornIdle:
			state = MonsterState.Idle;
			break;
		case MonsterState.Idle:
			if (lifetimeTimer >= realLifeTime)
			{
				state = MonsterState.RunToTarget;
				break;
			}
			idleTimer += Time.deltaTime;
			if (idleTimer >= idleTime.result)
			{
				idleTimer = 0f;
				RefindTarget();
				if ((bool)targetPpt)
				{
					state = MonsterState.RunToTarget;
					spritePlayer.Play(runAnima);
				}
				else
				{
					state = MonsterState.IdleWalk;
					spritePlayer.Play(runAnima);
					idleWalkTime.RandomResult();
				}
			}
			break;
		case MonsterState.IdleWalk:
			if (lifetimeTimer >= realLifeTime)
			{
				state = MonsterState.RunToTarget;
				break;
			}
			if (navPath == null)
			{
				RefindPath(Tool2D.GetNavMeshPoint(base.transform.position + UnityEngine.Random.insideUnitSphere.IgnoreZ() * 2f));
			}
			idleWalkTimer += Time.deltaTime;
			if (idleWalkTimer >= idleWalkTime.result)
			{
				idleWalkTimer = 0f;
				RefindTarget();
				if (targetPpt != null)
				{
					state = MonsterState.RunToTarget;
					spritePlayer.Play(runAnima);
					break;
				}
				state = MonsterState.Idle;
				navPath = null;
				spritePlayer.Play(idleAnima);
				idleTime.RandomResult();
			}
			break;
		case MonsterState.RunToTarget:
		{
			if (targetPpt == null)
			{
				RefindTarget();
			}
			if (targetPpt == null)
			{
				state = MonsterState.Idle;
				spritePlayer.Play(idleAnima);
				idleTime.RandomResult();
				break;
			}
			if (navPath == null)
			{
				RefindPath(targetPpt.transform.position);
			}
			recheckIntervalTimer += Time.deltaTime;
			if (recheckIntervalTimer >= recheckInterval)
			{
				recheckIntervalTimer = 0f;
				RefindPath(targetPpt.transform.position.IgnoreZ());
			}
			float num = targetPpt.UnitBas.GetBodyColliderRadius() + damageDetectRadiu;
			if (Tool2D.IgnoreZDistanceSqr(targetPpt.transform.position, base.transform.position) < num * num)
			{
				state = MonsterState.Suicide;
				if (targetPpt != null)
				{
					if (explosionData != null)
					{
						targetPpt.SetVoid(explosionData);
					}
					TakeDamageInfo takeDamageInfo = targetPpt.TakeDamage(damage, AttackerType.NothingSpecial, new TakeDamageInfo
					{
						canRebound = false,
						criticalChance = explodeCriticalChance
					});
					if (!takeDamageInfo.immuneDamage)
					{
						ApplyElementEffectToTarget(targetPpt);
						if (takeDamageInfo.isCriticalDamage && criticalDamageRatio > 0f)
						{
							ApplyPullEffect(targetPpt);
						}
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
				ObjPoolMgr.Inst.RecycleGO(base.gameObject);
			}
			else
			{
				recheckPositionTimer += Time.deltaTime;
				if (recheckPositionTimer >= Time.deltaTime && lifetimeTimer < realLifeTime)
				{
					recheckPositionTimer = 0f;
					RefindPath(targetPpt.transform.position);
				}
			}
			break;
		}
		case MonsterState.Suicide:
			break;
		}
	}

	private void RefindTarget()
	{
		targetPpt = LevelMgr.Inst.CurrentRoomCtrller.GetNearestTargetablePpt(base.transform.position);
	}

	private void RefindPath(Vector3 target)
	{
		navPath = Tool2D.GetNavMeshPath(base.transform.position, target);
		currentNavPoint = 0;
	}

	private void MoveByNavPath()
	{
		if (navPath == null)
		{
			return;
		}
		Vector3[] corners = navPath.corners;
		if (currentNavPoint + 1 >= corners.Length)
		{
			navPath = null;
			return;
		}
		Vector3 vector = navPath.corners[currentNavPoint + 1];
		float num = Vector3.SqrMagnitude(vector - base.transform.position);
		float num2 = moveSpeed * Time.deltaTime;
		if (num <= num2 * num2)
		{
			currentNavPoint++;
			base.transform.position = vector;
		}
		else
		{
			Vector3 normalized = (vector - base.transform.position).IgnoreZ().normalized;
			base.transform.Translate(normalized * moveSpeed * Time.deltaTime);
			sprite.flipX = normalized.x < 0f;
		}
	}

	public void ApplySpellEffect(SpellBase spell)
	{
		if (!(spell != null))
		{
			return;
		}
		damage = spell.damageRatio * (float)suicideExplodeDamage * spell.finalDamageRatio;
		damageDetectRadiu = suicideExplodeDetectRadiu * spell.radiusRatio;
		effectrRadiuscale = spell.radiusRatio;
		effectFinalRadiuRatio = spell.finalRadiusRatio;
		hoverTime = spell.SpellHoverTime;
		realLifeTime = lifetime;
		chargeData = spell.wandChargeData;
		explodeCriticalChance = spell.GetCriticalChance();
		state = MonsterState.BornIdle;
		knockBackRatio = spell.knockbackRatio;
		finalknockBackRatio = spell.finalKnockbackRatio;
		frozenTime += spell.spellFrozenTime;
		mucusMoveSpeedEffect = spell.spellMucusMoveSpeedRatio;
		mucusSpellSpeedEffect = spell.spellMucusSpellSpeedRatio;
		mucusTime += spell.spellMucusTime;
		venomTime += spell.spellVenomTime;
		venomApplyCount = spell.spellVenomOnceCount;
		burnDamage = spell.burnHpRatioPerSeconds;
		burnTime = spell.spellBurnTime;
		criticalDamageRatio = spell.criticalDragDamagePercent;
		criticalRange = spell.criticalDragEffectRadiu;
		criticalTargetsCount = spell.criticalDragApllyToCount;
		criticalDragForce = spell.criticalDragPullForce;
		colorType = spell.ColorType;
		base.transform.localScale = Vector3.one * spell.spellVolumeRatio;
		explosionData = spell.voidExplosionInfo;
		srFire.gameObject.SetActive(value: false);
		switch (spell.ColorType)
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
			Debug.LogError(spell.ColorType);
			break;
		}
	}

	private void ApplyPullEffect(UnitProperty targetPpt)
	{
		float radius = criticalRange * effectrRadiuscale * effectFinalRadiuRatio;
		Vector3 position = targetPpt.transform.position;
		float num = criticalDamageRatio * damage;
		List<Collider> collidersByTag = GeneralTool.GetCollidersByTag(position, radius, "Monster");
		collidersByTag = GeneralTool.ListShuffle(collidersByTag);
		int num2 = (int)Mathf.Min(collidersByTag.Count, criticalTargetsCount);
		bool flag = false;
		for (int i = 0; i < collidersByTag.Count && i < num2; i++)
		{
			Collider collider = collidersByTag[i];
			UnitProperty component = collider.gameObject.GetComponent<UnitProperty>();
			if (!(component == null) && !(component == targetPpt))
			{
				flag = true;
				TakeDamageInfo takeDamageInfo = new TakeDamageInfo
				{
					canRebound = false,
					damage = num,
					attackerPpt = PlayerMgr.Inst.PlayerPpt,
					criticalChance = explodeCriticalChance,
					beHitPpt = component
				};
				Spell3101PullCrystal component2 = ObjPoolMgr.Inst.GetGO("Prefabs/Spell/" + 31121, base.transform.position, quaternion.identity, 0.5f).GetComponent<Spell3101PullCrystal>();
				if (component2.tsf_Layer != null)
				{
					component2.tsf_Layer.localScale = base.transform.localScale;
				}
				component2.SetColor(colorType);
				component2.SetChainTargetTransform(targetPpt.gameObject.transform, collider.transform);
				component.TakeKnockback((targetPpt.transform.position - collider.transform.position).normalized * criticalDragForce * knockBackRatio * finalknockBackRatio);
				component.TakeDamage(damage, AttackerType.NothingSpecial, takeDamageInfo);
				component2.CreateHitEffect(colorType, collider.transform.position + component2.chainBaseHeight);
				ApplyElementEffectToTarget(takeDamageInfo.beHitPpt);
			}
		}
		if (flag)
		{
			SEMgr.Inst.spell3121Energy.PlaySE().pitch = UnityEngine.Random.Range(0.5f, 1.5f);
		}
	}

	private void CreateExplosionEffect()
	{
		SpellSpriteEffectController.Inst.PlayEffect(ExplosionAnima, new EffectPlayParam
		{
			Position = base.transform.position,
			Scale = Vector3.one * effectrRadiuscale * effectFinalRadiuRatio,
			FilpX = (UnityEngine.Random.Range(0, 2) == 0),
			Color = new Color(1f, 1f, 1f, DataMgr.settingData.FinalSpellTransparent)
		});
	}

	private void ApplyElementEffectToTarget(UnitProperty targetPpt)
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
}
