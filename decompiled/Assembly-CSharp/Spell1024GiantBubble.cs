using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell1024GiantBubble : SpellBase
{
	public float normalExplodeDelayTime;

	public float detonatedExplodeDelayTime;

	public float bubbleBaseDetectSize;

	public float bubbleIgnoreDetectTime;

	public float spawnRainTimeThreshold;

	public ShockParam shockParam;

	public float shockGrowRatio;

	public float maxShockRatio;

	public float extraKnockBackForceRatio;

	public float teammateKnockBackRatio;

	public float fallBaseSpeed;

	private static readonly HashSet<string> detectTags_Player = new HashSet<string> { "Monster" };

	private static readonly HashSet<string> detectTags_Monster = new HashSet<string> { "Player", "Teammate" };

	private static readonly Collider[] detectTargetBuffer = new Collider[256];

	public float bonusFinalDamageRatio { get; private set; }

	public float bonusFinalSizeRatio { get; private set; }

	public float bubbleCollapseTime { get; private set; }

	public bool collapseEnd { get; private set; }

	public bool startCollapse { get; private set; }

	protected override float FallingReboundForce => Mathf.Clamp((0f - base.CurrentUpSpeed) * 0.5f, InFallingReboundingGravity / 2f, 9999f);

	protected override float InFallingReboundingGravity => base.InFallingReboundingGravity * 0.2f;

	public override void InitializeCallback()
	{
		base.InitializeCallback();
		collapseEnd = false;
		startCollapse = false;
		bonusFinalDamageRatio = 0f;
		bonusFinalSizeRatio = 0f;
		bubbleCollapseTime = 0f;
		ApplySpeedToVelocity();
		base.spellCfg.duration += base.SpellHoverTime;
		base.spellVolumeRatio = 1f;
		if (base.SIP.spellIsFall)
		{
			base.CurrentUpSpeed = Mathf.Max(-20f, base.CurrentUpSpeed);
		}
	}

	public override void Update()
	{
		base.Update();
		if (!collapseEnd)
		{
			DetectEnemyTargetNearBy();
			base.DurationTimer += Time.deltaTime;
			if (!base.isFlyFinish && base.DurationTimer >= base.spellCfg.duration && !base.SIP.spellIsFall)
			{
				BubbleNormalExplode();
			}
		}
		UpdateAbilityBonusState();
	}

	private float GetExtraFallRadius()
	{
		return base.SIP.fallExplosionRadius * base.radiusRatio * base.finalRadiusRatio;
	}

	private void UpdateAbilityBonusState()
	{
		bonusFinalSizeRatio += base.spellCfg.float2 / 100f * Time.deltaTime;
		bonusFinalDamageRatio += base.spellCfg.float3 / 100f * Time.deltaTime;
		SpellConfig spellConfig = SpellConfig.dic[base.spellCfg.id];
		if (base.SIP.spellIsFall)
		{
			base.spellCfg.radius = spellConfig.radius * (base.radiusRatio + bonusFinalSizeRatio) * base.finalRadiusRatio + GetExtraFallRadius();
		}
		else
		{
			base.spellCfg.radius = spellConfig.radius * (base.radiusRatio + bonusFinalSizeRatio) * base.finalRadiusRatio;
		}
		base.spellCfg.damage = Mathf.Ceil(spellConfig.damage * base.damageRatio * (base.finalDamageRatio + bonusFinalDamageRatio)) + base.InitialParameter.finalDamageExtra;
	}

	private void DetectEnemyTargetNearBy()
	{
		if (!(base.DurationTimer <= bubbleIgnoreDetectTime) && !base.SIP.spellIsFall)
		{
			float radius = bubbleBaseDetectSize * (base.radiusRatio + bonusFinalSizeRatio) * base.finalRadiusRatio;
			int mask = LayerMask.GetMask("Monster", "Monster_Fly", "Monster_Ghost");
			if ((IsSameCamp(UnitType.Player) ? GeneralTool.GetCollidersNonAlloc(base.transform.position, radius, detectTargetBuffer, detectTags_Player, mask) : GeneralTool.GetCollidersNonAlloc(base.transform.position, radius, detectTargetBuffer, detectTags_Monster)) > 0)
			{
				BubbleNormalExplode();
			}
		}
	}

	protected override void OnFallingGroundTryReboundOrRecycle()
	{
		if (!OnFallingGroundTryRebound())
		{
			ChangeCurrentSpeed(0f);
			BubbleNormalExplode();
		}
	}

	protected override void InitSpeedAndPosition()
	{
		if (base.SIP.spellIsFall)
		{
			base.spellCfg.speed = (fallBaseSpeed + base.bonusSpeed) * base.speedRatio * base.finalSpeedRatio;
		}
		base.InitSpeedAndPosition();
	}

	public override void PoolRecycle()
	{
		BubbleNormalExplode();
		base.PoolRecycle();
	}

	public void BubbleNormalExplode()
	{
		BubbleStartCollapse(normalExplodeDelayTime);
	}

	private void BubbleFastExplode()
	{
		if (!base.SIP.spellIsFall)
		{
			BubbleStartCollapse(detonatedExplodeDelayTime);
		}
	}

	private void BubbleStartCollapse(float delayTime)
	{
		if (!startCollapse && !(base.DurationTimer <= bubbleIgnoreDetectTime))
		{
			startCollapse = true;
			StartCoroutine(Explode(delayTime));
		}
	}

	private IEnumerator Explode(float delayTime)
	{
		base.enableAroundPlayer = false;
		enableFollowMouse = false;
		enableFollowTarget = false;
		rigid.linearVelocity = Vector3.zero;
		bubbleCollapseTime = delayTime;
		EffectBase.ManualCreateEffect("ChargeEnd");
		PlaySE("Collapse");
		collapseEnd = true;
		yield return new WaitForSeconds(delayTime);
		SpellEffectBase effectBase = EffectBase;
		Vector3? position = base.transform.position;
		effectBase.ManualCreateEffect("Explosion", null, position);
		SpellEffectBase effectBase2 = EffectBase;
		position = base.transform.position;
		effectBase2.ManualCreateEffect("ExplosionGround", null, position);
		if (base.DurationTimer >= spawnRainTimeThreshold)
		{
			EffectBase.ManualCreateEffect("EndRain");
			EffectBase.ManualCreateEffect("EndRainGround");
		}
		base.isFlyFinish = true;
		PlaySE("Explosion");
		ScreenShake();
		DealDamageInRange();
		if (base.ColorType == SpellColorType.Mucus)
		{
			LevelMgr.Inst.CurrentRoomCtrller.mucusCtrller.CreateMucus(Tool2D.IgnoreZPoint(base.transform), base.spellCfg.radius);
		}
		if (base.ColorType == SpellColorType.Venom)
		{
			LevelMgr.Inst.CurrentRoomCtrller.venomCtrller.CreateVenom(Tool2D.IgnoreZPoint(base.transform), base.spellCfg.radius, spellVenomTime);
		}
		SpellColorType colorType = base.ColorType;
		if (colorType == SpellColorType.Frozen || colorType == SpellColorType.Player || colorType == SpellColorType.Thunder)
		{
			LevelMgr.Inst.CurrentRoomCtrller.waterCtrller.CreateWater(Tool2D.IgnoreZPoint(base.transform), base.spellCfg.radius);
		}
		PoolRecycle();
	}

	private void ScreenShake()
	{
		ShockParam shockParam = this.shockParam;
		float num = Mathf.Min(maxShockRatio, base.DurationTimer * shockGrowRatio);
		shockParam.radius *= 1f + num;
		shockParam.speed *= 1f + num * 0.4f;
		shockParam.time *= 1f + num * 0.2f;
		CamController.Inst.SetShock(shockParam, new Vector3(0f, -1f, 0f));
	}

	private void DealDamageInRange()
	{
		List<Collider> collidersByTag = GeneralTool.GetCollidersByTag(base.transform.position, base.spellCfg.radius, "Monster", "Destructible", "SolidObj", "Spell", "RollBall", "Butterfly", "Brittleness", "Player", "Teammate");
		base.spellCfg.knockback = SpellConfig.dic[base.spellCfg.id].knockback * (base.knockbackRatio + extraKnockBackForceRatio * base.DurationTimer) * base.finalDamageRatio;
		foreach (Collider item in collidersByTag)
		{
			if (item.gameObject.CompareAnyTag("Spell", "RollBall", "Butterfly"))
			{
				SpellBase componentInParent = item.GetComponentInParent<SpellBase>();
				if (!(componentInParent is Spell1024GiantBubble spell1024GiantBubble))
				{
					if (!(componentInParent is Spell1002RollBall spell1002RollBall))
					{
						if (componentInParent is Spell1003Butterfly spell1003Butterfly)
						{
							spell1003Butterfly.HitEFAndRecycle();
						}
					}
					else
					{
						spell1002RollBall.TakeDamage(base.spellCfg.damage);
					}
				}
				else
				{
					spell1024GiantBubble.BubbleFastExplode();
				}
			}
			else if (item.gameObject.CompareAnyTag("Monster", "Player", "Teammate"))
			{
				UnitProperty component = item.gameObject.GetComponent<UnitProperty>();
				TakeDamageInfo info = new TakeDamageInfo
				{
					canRebound = false
				};
				if (!IsSameCamp(component.unitCfg.unitType))
				{
					OutputDamage(item.gameObject, info);
					Vector3 lookAt = Tool2D.IgnoreZPoint((item.transform.position - base.transform.position).normalized);
					Vector3 value = item.transform.position + new Vector3(0f, 0.3f, 0f);
					CreateHitEffectLookAt(lookAt, value);
				}
				if (IsSameCamp(UnitType.Player))
				{
					float currentMulRatio = SpellShootGroupExtend.GetRecoilRatio(base.shooterWand).CurrentMulRatio;
					component.TakeKnockback(base.spellCfg.knockback * teammateKnockBackRatio * currentMulRatio * Tool2D.IgnoreZV2ToV1Normal(item.transform, base.transform));
				}
				else
				{
					component.TakeKnockback(Tool2D.IgnoreZV2ToV1Normal(item.transform, base.transform) * base.spellCfg.knockback);
				}
			}
			else
			{
				TakeDamageInfo info2 = new TakeDamageInfo
				{
					canRebound = false,
					knockbackForce = Tool2D.IgnoreZV2ToV1Normal(item.transform, base.transform) * base.spellCfg.knockback
				};
				OutputDamage(item.gameObject, info2);
			}
		}
	}

	public override void TriggerIn(Collider other)
	{
		string text = other.tag;
		if ((text == "Wall" || text == "SolidObj") && !isThroughWall && base.currentSpellMovement != SpellSpecialMovementType.Rotation && base.rebounceTime <= 0)
		{
			BubbleNormalExplode();
		}
	}
}
