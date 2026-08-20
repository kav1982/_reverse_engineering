using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Spell1018ThunderAura : SpellBase
{
	public Transform tsf_Ground;

	public ShockParam shockParam;

	public float longRangeDetectSizeRatio;

	private float initialDamage;

	private float attackIntervalTimer;

	private float? _interval;

	private int timeScale = 1;

	private Spell1018Effect effect;

	private float CurRandomDamageInterval
	{
		get
		{
			float? interval = _interval;
			if (!interval.HasValue)
			{
				float num = base.spellCfg.DPSDamageInterval * base.spellCfg.float2;
				_interval = UnityEngine.Random.Range(base.spellCfg.DPSDamageInterval - num, base.spellCfg.DPSDamageInterval + num);
			}
			return _interval.Value;
		}
	}

	private void ReRandomDamageInterval()
	{
		_interval = null;
	}

	public override void SingleInitialCallback()
	{
		base.SingleInitialCallback();
		effect = (Spell1018Effect)EffectBase;
	}

	public override void OnFirstFrame()
	{
		base.OnFirstFrame();
		if (base.SIP.spellIsFall)
		{
			PlaySE("Fall");
			ShootSpellSpatialInfo finalShootSpatialInfo = base.SIP.finalShootSpatialInfo;
			if (finalShootSpatialInfo == null || !finalShootSpatialInfo.Target.HasValue)
			{
				Debug.LogWarning("坠落落雷阵没有攻击目标？");
			}
			else if (base.currentSpellMovement == SpellSpecialMovementType.Rotation)
			{
				Vector3 v = Tool2D.GetDir(base.spellAroundOwnerCurrentAngle) * base.spellAroundOwnerRadius + GetAroundTargetBasePoint();
				v = Tool2D.IgnoreZPoint(v);
				effect.CreateFallLighting(new Vector3[2]
				{
					base.transform.position,
					v
				}, isRebounce: false);
			}
			else
			{
				effect.CreateFallLighting(new Vector3[2]
				{
					base.transform.position,
					base.SIP.finalShootSpatialInfo.Target.Value
				}, isRebounce: false);
			}
		}
		else
		{
			EffectBase.ManualCreateEffect("Spell");
			EffectBase.ManualCreateEffect("Rain");
		}
	}

	public override void InitializeCallback()
	{
		attackIntervalTimer = -0.05f;
		ApplySpeedToVelocity();
		base.transform.localScale = Vector3.one;
		tsf_Layer.localScale = Vector3.one * base.spellCfg.radius;
		tsf_Ground.localScale = tsf_Layer.localScale;
		initialDamage = base.spellCfg.damage;
		timeScale = 1;
		if (!base.SIP.spellIsFall)
		{
			base.transform.position = new Vector3(base.transform.position.x, base.transform.position.y, -0.3f);
			tsf_Ground.localPosition = new Vector3(tsf_Ground.localPosition.x, 0.3f, tsf_Ground.localPosition.z);
		}
	}

	public override void Update()
	{
		base.Update();
		timeScale = ((!GeneralTool.IsLowFpsOptimizeActive(30f)) ? 1 : Mathf.CeilToInt(10f * (1f - GameMgr.Inst.GetFps() / 30f)));
		if (base.SIP.spellIsFall)
		{
			return;
		}
		base.DurationTimer += Time.deltaTime * (float)timeScale;
		if (base.DurationTimer > base.spellCfg.duration)
		{
			if (base.SpellHoverTime > 0f && base.SpellHoverTimer < base.SpellHoverTime)
			{
				base.SpellHoverTimer += Time.deltaTime;
				base.enableAroundPlayer = false;
				enableFollowMouse = false;
				enableFollowTarget = false;
				rigid.linearVelocity = Vector3.zero;
			}
			else
			{
				base.isFlyFinish = true;
				base.spellCfg.damage = initialDamage;
				PoolRecycle();
			}
		}
		attackIntervalTimer += Time.deltaTime;
		if (attackIntervalTimer > CurRandomDamageInterval)
		{
			List<Collider> aroundTargetsAndLightningLineAttackTargets = GetAroundTargetsAndLightningLineAttackTargets();
			LightningAttack(aroundTargetsAndLightningLineAttackTargets);
			attackIntervalTimer -= CurRandomDamageInterval;
			ReRandomDamageInterval();
		}
	}

	private List<Collider> GetAroundTargets(Vector3 position, float radius)
	{
		List<Collider> source = ((!IsSameCamp(UnitType.Player)) ? GeneralTool.GetCollidersByTagAndCheckCamp(position, radius, UnitType.Player, true, "Player", "Teammate", "RollBall", "Butterfly") : GeneralTool.GetCollidersByTagAndCheckCamp(position, radius, UnitType.Player, false, "Monster", "RollBall", "Butterfly"));
		Monster17_Hat component;
		return source.Where((Collider e) => !e.TryGetComponent<Monster17_Hat>(out component)).ToList();
	}

	private List<Collider> GetAroundTargetsAndLightningLineAttackTargets()
	{
		List<Collider> aroundTargets = GetAroundTargets(base.transform.position, base.spellCfg.radius);
		return GetLightningLineAttackTargets(aroundTargets);
	}

	private List<Collider> GetLightningLineAttackTargets(List<Collider> startTargets)
	{
		if (startTargets.Count == 0)
		{
			return new List<Collider>(0);
		}
		List<Collider> result = startTargets.ToList();
		int num = 0;
		while (result.Count < base.spellCfg.int1)
		{
			num++;
			if (num >= 100)
			{
				break;
			}
			List<Collider> list = result;
			Collider[] array = (from t in GetAroundTargets(list[list.Count - 1].transform.position, base.spellCfg.radius * longRangeDetectSizeRatio)
				where !result.Contains(t)
				select t).ToArray();
			if (array.Length == 0)
			{
				break;
			}
			result.Add(array[UnityEngine.Random.Range(0, array.Length)]);
		}
		return result;
	}

	private void LightningAttack(List<Collider> targets)
	{
		if (targets.Count == 0)
		{
			return;
		}
		CamController.Inst.SetShock(shockParam);
		((Spell1018Effect)EffectBase).CreateChainEffect(targets.Select((Collider e) => e.transform.position).ToArray());
		PlaySE("Hit");
		base.spellCfg.damage = ((targets.Count == 1) ? Mathf.Floor(initialDamage * base.spellCfg.float1 / 100f) : initialDamage);
		bool isAddToIgnoreRefract = false;
		foreach (Collider target in targets)
		{
			MakeDamage(target, !isAddToIgnoreRefract, out isAddToIgnoreRefract);
			CreateHitEffect(target.transform.position);
		}
		TryRefract();
	}

	private void MakeDamage(Collider target, bool addToIgnoreRefract, out bool isAddToIgnoreRefract)
	{
		isAddToIgnoreRefract = false;
		if (target.gameObject.CompareAnyTag("Player", "Teammate", "Monster"))
		{
			UnitProperty component = target.GetComponent<UnitProperty>();
			if (component != null)
			{
				if (addToIgnoreRefract)
				{
					isAddToIgnoreRefract = true;
					refractedTargets.Add(component.gameObject);
				}
				float num = base.spellCfg.damage * (float)timeScale;
				if (!base.SIP.spellIsFall)
				{
					num = Mathf.Ceil(num * base.spellCfg.DPSDamageInterval);
				}
				OutputDamage(component, new TakeDamageInfo
				{
					canRebound = false,
					knockbackForce = Tool2D.GetDir() * base.spellCfg.knockback,
					damage = num
				});
			}
		}
		else if (target.gameObject.CompareAnyTag("RollBall", "Butterfly"))
		{
			SpellBase componentInParent = target.GetComponentInParent<SpellBase>();
			if (!componentInParent.IsSameCamp(this))
			{
				if (componentInParent.spellCfg.abilityType == SpellAbilityType.Rollball)
				{
					((Spell1002RollBall)componentInParent).TakeDamage(base.spellCfg.damage);
				}
				else if (componentInParent.spellCfg.abilityType == SpellAbilityType.Butterfly)
				{
					((Spell1003Butterfly)componentInParent).HitEFAndRecycle();
				}
				else
				{
					MonoBehaviour.print(componentInParent.spellCfg.abilityType);
				}
			}
		}
		else
		{
			float num2 = base.spellCfg.damage;
			if (!base.SIP.spellIsFall)
			{
				num2 = Mathf.Ceil(num2 * base.spellCfg.DPSDamageInterval);
			}
			OutputDamage(target.gameObject, new TakeDamageInfo
			{
				canRebound = false,
				damage = num2
			});
		}
	}

	public void OnFallingLightingOnGround(Vector3 damagePosition)
	{
		base.transform.position = damagePosition;
		effect.CreateFallingExplosion(damagePosition);
		foreach (Collider fallingGroundDamageTarget in GetFallingGroundDamageTargets(damagePosition))
		{
			List<Collider> list = new List<Collider>(new Collider[1] { fallingGroundDamageTarget });
			if (fallingGroundDamageTarget.CompareTag("Monster"))
			{
				list = GetLightningLineAttackTargets(list);
			}
			LightningAttack(list);
		}
		OnFallingGroundTryReboundOrRecycle();
	}

	protected override bool TryFallingRebound()
	{
		if (base.rebounceTime <= 0)
		{
			return false;
		}
		switch (base.currentSpellMovement)
		{
		case SpellSpecialMovementType.Normal:
			effect.CreateFallLighting(new Vector3[2]
			{
				base.transform.position,
				base.transform.position + base.Direction * base.spellCfg.radius
			}, isRebounce: true);
			break;
		case SpellSpecialMovementType.ChaseEnemy:
			spellFollowTargetPpt = GetMiniMalAngleTargetablePpt();
			if (base.SpellFollowHaveTarget)
			{
				ShootSpellSpatialInfo finalShootSpatialInfo = base.SIP.finalShootSpatialInfo;
				if (finalShootSpatialInfo != null && finalShootSpatialInfo.Target.HasValue)
				{
					Vector3 to2 = Tool2D.IgnoreZPoint(spellFollowTargetPpt.transform.position - base.transform.position);
					Vector3 direction2 = Tool2D.RotateTowardsAroundZAxis(base.Direction, to2, spellFollowTargetRotateSpeed * 3f);
					Vector3 vector3 = direction2.normalized * base.spellCfg.radius + base.transform.position;
					Vector3 vector4 = base.transform.position + base.Direction * base.spellCfg.radius;
					effect.CreateFallLighting(new Vector3[3]
					{
						base.transform.position,
						vector4,
						vector3
					}, isRebounce: true);
					base.Direction = direction2;
					break;
				}
			}
			effect.CreateFallLighting(new Vector3[2]
			{
				base.transform.position,
				base.transform.position + base.Direction * base.spellCfg.radius
			}, isRebounce: true);
			break;
		case SpellSpecialMovementType.ChaseMouse:
		{
			Vector3 vector8 = Tool2D.IgnoreZPoint(UnityEngine.Random.insideUnitSphere * 0.3f);
			base.Direction = Tool2D.IgnoreZPoint(PlayerMgr.Inst.GetMousePoint() - (base.transform.position + vector8)).normalized;
			effect.CreateFallLighting(new Vector3[2]
			{
				base.transform.position,
				base.transform.position + base.Direction * base.spellCfg.radius
			}, isRebounce: true);
			break;
		}
		case SpellSpecialMovementType.Rotation:
		{
			float num = 360f / (MathF.PI * 2f * base.spellAroundOwnerRadius / 20f) * 0.02f;
			Vector3 vector5 = GetAroundTargetBasePoint() + Tool2D.GetDir(base.spellAroundOwnerCurrentAngle) * base.spellAroundOwnerRadius;
			base.spellAroundOwnerCurrentAngle += num * 2f;
			Vector3 vector6 = GetAroundTargetBasePoint() + Tool2D.GetDir(base.spellAroundOwnerCurrentAngle) * base.spellAroundOwnerRadius;
			base.spellAroundOwnerCurrentAngle += num * 2f;
			Vector3 vector7 = GetAroundTargetBasePoint() + Tool2D.GetDir(base.spellAroundOwnerCurrentAngle) * base.spellAroundOwnerRadius;
			effect.CreateFallLighting(new Vector3[3] { vector5, vector6, vector7 }, isRebounce: true);
			base.Direction = Tool2D.GetDir(base.spellAroundOwnerCurrentAngle + 90f);
			break;
		}
		case SpellSpecialMovementType.ChaseOwner:
		{
			Vector3? spellFollowToOwnerPoint = GetSpellFollowToOwnerPoint();
			if (!spellFollowToOwnerPoint.HasValue)
			{
				effect.CreateFallLighting(new Vector3[2]
				{
					base.transform.position,
					base.transform.position + base.Direction * base.spellCfg.radius
				}, isRebounce: true);
				break;
			}
			Vector3 position = base.transform.position;
			Vector3 vector = position + base.Direction * 2f;
			Vector3 to = Tool2D.IgnoreZPoint(spellFollowToOwnerPoint.Value - position);
			Vector3 direction = Tool2D.RotateTowardsAroundZAxis(base.Direction, to, spellFollowTargetRotateSpeed * 3f);
			Vector3 vector2 = direction.normalized * base.spellCfg.radius + position;
			effect.CreateFallLighting(new Vector3[3]
			{
				base.transform.position,
				vector,
				vector2
			}, isRebounce: true);
			base.Direction = direction;
			break;
		}
		default:
			throw new ArgumentOutOfRangeException();
		}
		base.rebounceTime--;
		return true;
	}

	protected override bool TryFallingRefract()
	{
		if (base.remainRefractCount <= 0 || lastOutputDamageFrame != Time.frameCount)
		{
			return false;
		}
		base.remainRefractCount--;
		UnitProperty nextRefractionTargetablePpt = GetNextRefractionTargetablePpt();
		Vector3 vector;
		if ((bool)nextRefractionTargetablePpt)
		{
			vector = nextRefractionTargetablePpt.transform.position;
			refractedTargets.Add(nextRefractionTargetablePpt.gameObject);
		}
		else
		{
			vector = base.transform.position + base.Direction * 2f;
		}
		effect.CreateFallLighting(new Vector3[2]
		{
			base.transform.position,
			vector
		}, isRebounce: true);
		return true;
	}

	public override void CreateHitEffect(Vector3? position = null, Quaternion? rotation = null)
	{
		if (position.HasValue)
		{
			position += new Vector3(UnityEngine.Random.Range(-0.1f, 0.1f), 0.3f + UnityEngine.Random.Range(-0.1f, 0.1f), 0f);
		}
		base.CreateHitEffect(position, rotation);
	}

	public override void TriggerIn(Collider other)
	{
	}
}
