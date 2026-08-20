using System;
using DG.Tweening;
using UnityEngine;

public class Teammate1 : Teammate
{
	private enum bulletShootFrom
	{
		normal,
		left,
		right
	}

	public class StateBornIdle : TeammateState
	{
		private float _timer;

		public override void OnUpdate()
		{
			Self.SetMove(Vector3.zero);
			_timer += Time.deltaTime;
			if (_timer >= 0.5f && !((Teammate1)Self).TrySwitchState_RunToTarget())
			{
				Self.ChangeState(new StateIdle());
			}
			Self.myPpt.tsf_Layer.gameObject.SetActive(value: true);
		}
	}

	public class StateIdle : TeammateState
	{
		private float _timer;

		private static int IdleID = Animator.StringToHash("Idle");

		public override void OnEnter()
		{
			((Teammate1)Self).idleTime.RandomResult();
			((Teammate1)Self).Anima.SetTrigger(IdleID);
		}

		public override void OnUpdate()
		{
			if (Self.SummonerSpellBase.currentSpellMovement == SpellSpecialMovementType.Rotation)
			{
				Self.ChangeState(new StateIdleWalk());
				return;
			}
			Self.SetMove(Vector3.zero, isFlip: false);
			_timer += Time.deltaTime;
			if (_timer >= ((Teammate1)Self).idleTime.result)
			{
				_timer = 0f;
				if (!((Teammate1)Self).TrySwitchState_RunToTarget())
				{
					Self.ChangeState(new StateIdleWalk());
					Self.GetNavInfo(Tool2D.GetNavMeshPoint(Self.transform.position, ((Teammate1)Self).idleWalkRadius));
				}
			}
		}
	}

	public class StateIdleWalk : TeammateState
	{
		private float _timer;

		private static int IdleWalkID = Animator.StringToHash("IdleWalk");

		public override void OnEnter()
		{
			((Teammate1)Self).Anima.SetTrigger(IdleWalkID);
			((Teammate1)Self).idleWalkTime.RandomResult();
		}

		public override void OnUpdate()
		{
			if (Self.SummonerSpellBase.currentSpellMovement != SpellSpecialMovementType.Rotation)
			{
				if (((Teammate1)Self).navInfo.allCornerArrived)
				{
					Self.GetNavInfo(Tool2D.GetNavMeshPoint(Self.transform.position, ((Teammate1)Self).idleWalkRadius));
				}
				else
				{
					Self.SetMove(((Teammate1)Self).ToPointDir(((Teammate1)Self).navInfo.ToGoPoint) * Self.GetSummonUnitRealMoveSpeed() * ((Teammate1)Self).idleWalkSpeedRatio);
					Self.CheckNavInfo();
				}
			}
			_timer += Time.deltaTime;
			if (_timer >= ((Teammate1)Self).idleWalkTime.result)
			{
				_timer = 0f;
				if (!((Teammate1)Self).TrySwitchState_RunToTarget())
				{
					Self.ChangeState(new StateIdle());
					((Teammate1)Self).idleTime.RandomResult();
				}
			}
		}
	}

	public class StateRunToTarget : TeammateState
	{
		private float _timer;

		private static readonly int RunID = Animator.StringToHash("Run");

		public override void OnEnter()
		{
			((Teammate1)Self).Anima.SetTrigger(RunID);
		}

		public override void OnUpdate()
		{
			if (!((Teammate1)Self).HaveTarget)
			{
				Self.GetNearestTarget();
			}
			if (!((Teammate1)Self).HaveTarget)
			{
				Self.ChangeState(new StateIdle());
				return;
			}
			_timer += Time.deltaTime;
			if (_timer >= ((Teammate1)Self).recheckInterval)
			{
				_timer = 0f;
				Self.GetNearestTarget();
			}
			if (!((Teammate1)Self).HaveTarget)
			{
				return;
			}
			if (((Teammate1)Self).ToTargetDistanceSqr() < ((Teammate1)Self).finalAttackDistance * ((Teammate1)Self).finalAttackDistance && (PlayerMgr.Inst.ItemCtrller.relic_SpellThroughWall || !Tool2D.IsTargetPointBlockByWall(((Teammate1)Self).transform.position, ((Teammate1)Self).TargetPoint)))
			{
				if (_timer >= ((Teammate1)Self).attackInterval)
				{
					_timer = 0f;
					Self.ChangeState(new StateAttack());
				}
				else
				{
					Self.ChangeState(new StateRunToTargetWait());
					((Teammate1)Self).Anima.SetTrigger(IdleID);
				}
			}
			else if (Self.SummonerSpellBase.currentSpellMovement != SpellSpecialMovementType.Rotation)
			{
				Self.GetNavInfo(((Teammate1)Self).TargetPoint);
				Vector3 toGoPoint = ((Teammate1)Self).navInfo.ToGoPoint;
				Self.SetMove(((Teammate1)Self).ToPointDir(toGoPoint) * Self.GetSummonUnitRealMoveSpeed());
			}
		}
	}

	public class StateRunToTargetWait : TeammateState
	{
		private float _timer = 9999f;

		public override void OnUpdate()
		{
			if (!((Teammate1)Self).HaveTarget)
			{
				Self.GetNearestTarget();
			}
			if (!((Teammate1)Self).HaveTarget)
			{
				Self.ChangeState(new StateIdle());
				return;
			}
			Self.SetMove(Vector3.zero, isFlip: false);
			((Teammate1)Self).SetFlip(((Teammate1)Self).ToTargetDir().x);
			_timer += Time.deltaTime * Self.SummonerSpellBase.GetSummonValueRatio().attackSpeedRatio;
			if (_timer >= ((Teammate1)Self).attackInterval)
			{
				if (((Teammate1)Self).ToTargetDistanceSqr() < ((Teammate1)Self).finalAttackDistance * ((Teammate1)Self).finalAttackDistance)
				{
					_timer = 0f;
					Self.ChangeState(new StateAttack());
				}
				else
				{
					Self.ChangeState(new StateRunToTarget());
				}
			}
		}
	}

	public class StateAttack : TeammateState
	{
		private static int AttackID = Animator.StringToHash("Attack");

		private static int ChargeAttackID = Animator.StringToHash("ChargeAttack");

		public override void OnEnter()
		{
			int num = ((Self.SummonerSpellBase.SIP.summonAdvanceSkillType1Level > 0 && UnityEngine.Random.Range(0f, 1f) <= ((Teammate1)Self).bombBulletChance + (float)(((Teammate1)Self).SummonerSpellBase.SIP.summonAdvanceSkillType1Level - 1) * ((Teammate1)Self).bombChanceRatioUpPerLevel) ? 1 : 0);
			((Teammate1)Self).lastFrameTargetPoint = ((Teammate1)Self).TargetPoint;
			if (num != 0)
			{
				((Teammate1)Self).Anima.SetTrigger(ChargeAttackID);
			}
			else
			{
				((Teammate1)Self).Anima.SetTrigger(AttackID);
			}
		}

		public override void OnUpdate()
		{
			((Teammate1)Self).Anima.speed = Self.SummonerSpellBase.GetSummonValueRatio().attackSpeedRatio;
			Self.SetMove(Vector3.zero, isFlip: false);
			if (((Teammate1)Self).HaveTarget)
			{
				((Teammate1)Self).SetFlip(((Teammate1)Self).ToTargetDir().x);
			}
		}
	}

	public Shadow selfShadow;

	public VariableFloat idleTime;

	public VariableFloat idleWalkTime;

	public VariableFloat idleWalkRadius;

	public float idleWalkSpeedRatio;

	public float recheckInterval;

	[Header("Attack")]
	public float attackInterval;

	public float spellHeight;

	public float spellForwardSpeed;

	public float spellUpSpeed;

	public float spellGravity;

	public float spellKnockback;

	[Header("Color")]
	public SpriteRenderer sr;

	public GameObject fireEffect;

	public GameObject[] body;

	private float finalAttackDistance;

	private static readonly int IdleID = Animator.StringToHash("Idle");

	public float bodyCenterShift;

	private static readonly int UseGhostEffect = Shader.PropertyToID("_UseGhostEffect");

	private static readonly int UseFuseShineEffect = Shader.PropertyToID("_UseFuseShineEffect");

	private static readonly int FuseShineProcess = Shader.PropertyToID("_FuseShineProcess");

	[Header("精魄新技能")]
	public float bombEffectRange;

	public float bombChanceRatioUpPerLevel = 0.1f;

	public float bombBulletChance = 0.2f;

	public VariableFloat bombLandTime;

	public VariableFloat bombMaxHeightRange;

	public float bombCloseAttackRange;

	public float bombCloseAttackLandTime;

	public float bombLandDistanceSpeedRatio;

	private Transform ChargeEffectList;

	private Vector3 lastFrameTargetPoint;

	public override void EveryInitialCallback()
	{
		base.EveryInitialCallback();
		ChangeState(new StateBornIdle());
		base.Anima.SetTrigger(IdleID);
		myPpt.tsf_Layer.gameObject.SetActive(value: false);
		ChargeEffectList = null;
		selfShadow.ShadowGO.SetActive(value: true);
		lastFrameTargetPoint = base.transform.position;
	}

	public override void HideTeammate()
	{
		myPpt.tsf_Layer.gameObject.SetActive(value: false);
		selfShadow.ShadowGO.SetActive(value: false);
	}

	public override void ShowTeammate()
	{
		myPpt.tsf_Layer.gameObject.SetActive(value: true);
		selfShadow.ShadowGO.SetActive(value: true);
		ChangeState(new StateRunToTarget());
		base.Anima.SetTrigger(Animator.StringToHash("Run"));
	}

	public float GetBombExplosionRange()
	{
		return bombEffectRange * base.SummonerSpellBase.radiusRatio * base.SummonerSpellBase.finalRadiusRatio;
	}

	public override void Frame1InitialCallback()
	{
		base.SummonerSpellBase.GetAroundTargetBasePoint();
		if (base.SummonerSpellBase.SIP.summonAdvanceSkillType1Level > 0 && base.SummonerSpellBase.SIP.radiuDecreaseRatio < 1f)
		{
			float num = 1f + GeneralTool.GetSpellRadiusToDamageRatio(GetBombExplosionRange(), base.SummonerSpellBase.SIP.radiuDecreaseRatio, base.SummonerSpellBase.SIP.radiuDcreaseTransIntoDamageRatio);
			base.SummonerSpellBase.finalDamageRatio *= num;
			_ = base.SummonerSpellBase.spellCfg.damage;
			base.SummonerSpellBase.spellCfg.damage = Mathf.CeilToInt((base.SummonerSpellBase.spellCfg.damage - base.SummonerSpellBase.SIP.finalDamageExtra) * num + base.SummonerSpellBase.SIP.finalDamageExtra);
		}
		spellCfg1 = SpellConfig.GetConfigCopy(90011);
		spellCfg1.speed = spellForwardSpeed;
		spellCfg1.upSpeed = spellUpSpeed;
		spellCfg1.gravity = spellGravity;
		spellCfg1.knockback = spellKnockback;
		spellCfg1.damage = base.SummonerSpellBase.spellCfg.damage;
		fireEffect.SetActive(value: false);
		spellCfg2 = SpellConfig.GetConfigCopy(90012);
		spellCfg2.speed = spellForwardSpeed;
		spellCfg2.upSpeed = spellUpSpeed;
		spellCfg2.gravity = spellGravity;
		spellCfg2.knockback = spellKnockback;
		spellCfg2.damage = base.SummonerSpellBase.spellCfg.damage;
		spellCfg2.radius = bombEffectRange;
		GameObject[] array = body;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(value: false);
		}
		switch (base.SummonerSpellBase.ColorType)
		{
		case SpellColorType.Frozen:
			sr = body[0].GetComponent<SpriteRenderer>();
			body[0].SetActive(value: true);
			break;
		case SpellColorType.Mucus:
			sr = body[1].GetComponent<SpriteRenderer>();
			body[1].SetActive(value: true);
			break;
		case SpellColorType.Player:
			sr = body[2].GetComponent<SpriteRenderer>();
			body[2].SetActive(value: true);
			break;
		case SpellColorType.Thunder:
			sr = body[3].GetComponent<SpriteRenderer>();
			body[3].SetActive(value: true);
			break;
		case SpellColorType.Venom:
			sr = body[4].GetComponent<SpriteRenderer>();
			body[4].SetActive(value: true);
			break;
		case SpellColorType.Fire:
			fireEffect.SetActive(value: true);
			sr = body[5].GetComponent<SpriteRenderer>();
			body[5].SetActive(value: true);
			break;
		case SpellColorType.Void:
			sr = body[6].GetComponent<SpriteRenderer>();
			body[6].SetActive(value: true);
			break;
		default:
			Debug.LogError(base.SummonerSpellBase.ColorType);
			sr = body[0].GetComponent<SpriteRenderer>();
			break;
		}
		sr.material.SetInt(UseGhostEffect, 0);
		sr.material.SetInt(UseFuseShineEffect, 0);
		sr.material.SetFloat(FuseShineProcess, 0f);
		fireEffect.GetComponent<SpriteRenderer>().material.SetFloat(FuseShineProcess, 0f);
		GeneralTool.InitialSpriteMaterial(sr);
		if (base.SummonerSpellBase.currentSpellMovement == SpellSpecialMovementType.Rotation || base.SummonerSpellBase.SIP.spellIsFall)
		{
			finalAttackDistance = 20f;
		}
		else
		{
			finalAttackDistance = (spellForwardSpeed + base.SummonerSpellBase.bonusSpeed) * base.SummonerSpellBase.speedRatio * base.SummonerSpellBase.finalSpeedRatio * (spellUpSpeed * 2f / (0f - spellGravity));
		}
	}

	public override void OnEnterDelayDeathEvent()
	{
		ShowTeammate();
		base.OnEnterDelayDeathEvent();
		if (!(base.SummonerSpellBase.SIP.SpellSummonimmuteDeathTime <= 0f))
		{
			sr.material.SetInt(UseGhostEffect, 1);
			SummonGhostEffectToggle(state: true);
			ColliderToggle(state: false);
			FreeFromTeammate6();
		}
	}

	public override void OnEnterFuseStateEvent()
	{
		base.OnEnterFuseStateEvent();
		sr.material.SetInt(UseFuseShineEffect, 1);
		sr.material.DOFloat(1f, FuseShineProcess, 1.3f);
		if (base.SummonerSpellBase.ColorType == SpellColorType.Fire)
		{
			fireEffect.GetComponent<SpriteRenderer>().material.DOFloat(1f, FuseShineProcess, 1.3f);
		}
		selfShadow.ShadowGO.SetActive(value: false);
	}

	private void LateUpdate()
	{
		if (ChargeEffectList != null)
		{
			if (!ChargeEffectList.gameObject.activeInHierarchy)
			{
				ChargeEffectList = null;
			}
			else
			{
				ChargeEffectList.position = base.transform.position + new Vector3(0f, 0.15f * base.transform.localScale.x, 0f);
			}
		}
	}

	public override void Update()
	{
		SummonsTouchMonster();
		if (base.SummonerSpellBase.currentSpellMovement == SpellSpecialMovementType.Rotation && base.CanMove)
		{
			float num = 360f / (MathF.PI * 2f * base.SummonerSpellBase.spellAroundOwnerRadius / GetSummonUnitRealMoveSpeed()) * Time.deltaTime;
			base.SummonerSpellBase.spellAroundOwnerCurrentAngle += num;
			Vector3 v = base.SummonerSpellBase.GetAroundTargetBasePoint() + Tool2D.GetDir(base.SummonerSpellBase.spellAroundOwnerCurrentAngle) * base.SummonerSpellBase.spellAroundOwnerRadius;
			base.transform.position = Tool2D.IgnoreZPoint(v, base.transform.position.z);
			base.SummonerSpellBase.SpellAroundPlayerUpdateMoveTrigger(num);
		}
		base.Update();
		myPpt.bodyCenterPoint = base.transform.position + new Vector3(0f, bodyCenterShift, 0f);
	}

	public void ControldByTeammate6()
	{
		base.CanMove = false;
		ColliderToggle(state: false);
		HideTeammate();
		base.beingControlledByTeammate6 = true;
	}

	public void FreeFromTeammate6()
	{
		if (base.beingControlledByTeammate6)
		{
			base.CanMove = true;
			base.transform.eulerAngles = Vector3.zero;
		}
	}

	private Vector3 GetShootStartPosition(bulletShootFrom pos)
	{
		Vector3 result = Tool2D.IgnoreZPoint(base.transform.position, 0f - spellHeight);
		float num = 0.35f * myPpt.tsf_Layer.localScale.x;
		switch (pos)
		{
		case bulletShootFrom.left:
			result += new Vector3(0f - num, 0f, 0f);
			break;
		case bulletShootFrom.right:
			result += new Vector3(num, 0f, 0f);
			break;
		}
		return result;
	}

	private void SpawnNormalBullet(Vector3 spawnPos)
	{
		SpellConfig spellConfig = spellCfg1;
		SpellBase component = ObjPoolMgr.Inst.GetGO("Prefabs/Spell/" + spellConfig.prefab, spawnPos).GetComponent<SpellBase>();
		Vector3 vector = (base.HaveTarget ? base.TargetPoint : lastFrameTargetPoint);
		SpellInitialParameter spellInitialParameter = new SpellInitialParameter(myPpt, Tool2D.IgnoreZV2ToV1Normal(vector, spawnPos), spellConfig.id, spellConfig.Copy(), base.SummonerSpellBase.InitialParameter.shooterWand, base.SummonerSpellBase.InitialParameter.shootSpellPreSpells)
		{
			RefractionInfo = base.SummonerSpellBase.SIP.RefractionInfo,
			shooterWand = base.SummonerSpellBase.SIP.shooterWand,
			shooterWandCfg = base.SummonerSpellBase.SIP.shooterWandCfg,
			WandPostSlotChargeData = base.SummonerSpellBase.SIP.WandPostSlotChargeData,
			lightningChainDamage = base.SummonerSpellBase.SIP.lightningChainDamage,
			EnableThunderEffect = base.SummonerSpellBase.SIP.EnableThunderEffect,
			SpellVolumeRatio = base.SummonerSpellBase.SIP.SpellVolumeRatio,
			ColorType = base.SummonerSpellBase.SIP.ColorType,
			radiuDecreaseRatio = base.SummonerSpellBase.SIP.radiuDecreaseRatio,
			radiuDcreaseTransIntoDamageRatio = base.SummonerSpellBase.SIP.radiuDcreaseTransIntoDamageRatio
		};
		if (base.SummonerSpellBase.SIP.spellIsFall)
		{
			spellInitialParameter.finalShootSpatialInfo = ShootSpellSpatialInfo.ToPoint(base.transform.position, vector);
			spellInitialParameter.fallExplosionRadius = base.SummonerSpellBase.SIP.fallExplosionRadius;
		}
		component.Initialize(spellInitialParameter);
		component.ColorType = base.SummonerSpellBase.ColorType;
		component.voidExplosionInfo = base.SummonerSpellBase.voidExplosionInfo;
		component.spellVenomTime = base.SummonerSpellBase.spellVenomTime;
		component.spellVenomOnceCount = base.SummonerSpellBase.spellVenomOnceCount;
		component.spellMucusTime = base.SummonerSpellBase.spellMucusTime;
		component.spellMucusMoveSpeedRatio = base.SummonerSpellBase.spellMucusMoveSpeedRatio;
		component.spellMucusSpellSpeedRatio = base.SummonerSpellBase.spellMucusSpellSpeedRatio;
		component.spellFrozenTime = base.SummonerSpellBase.spellFrozenTime;
		component.spellBurnTime = base.SummonerSpellBase.spellBurnTime;
		component.burnHpRatioPerSeconds = base.SummonerSpellBase.burnHpRatioPerSeconds;
		component.endThunderHitPercent = base.SummonerSpellBase.endThunderHitPercent;
		component.endThunderHitRadiu = base.SummonerSpellBase.endThunderHitRadiu;
		component.endTHunderHitChance = base.SummonerSpellBase.endTHunderHitChance;
		component.overalCriticalChance = base.SummonerSpellBase.overalCriticalChance;
		component.spellCfg.damage = spellCfg1.damage;
		component.spellCfg.duration = (component.spellCfg.duration + base.SummonerSpellBase.bonusDuration) * base.SummonerSpellBase.finalDurationRatio;
		component.wandShootAngle = base.SummonerSpellBase.wandShootAngle;
		component._angle = base.SummonerSpellBase._angle;
		component.Direction = Tool2D.GetDir(component.Direction, UnityEngine.Random.Range((0f - component._angle) / 2f, component._angle / 2f));
		component.ApplySpeedToVelocity();
		if (component.currentSpellMovement == SpellSpecialMovementType.Rotation && component.spellAroundOwnerRadius > 0f)
		{
			component.spellAroundOwnerRadius = base.SummonerSpellBase.spellAroundOwnerRadius * base.SummonerSpellBase.radiusRatio * base.SummonerSpellBase.finalRadiusRatio;
		}
		component.OwnerTsf = base.transform;
		component.OwnerPoint = base.transform.position;
	}

	private void SpawnBoBoBomb(Vector3 spawnPos)
	{
		SpellBase component = ObjPoolMgr.Inst.GetGO("Prefabs/Spell/" + spellCfg2.prefab, spawnPos).GetComponent<SpellBase>();
		Vector3 vector = (base.HaveTarget ? base.TargetPoint : lastFrameTargetPoint);
		Vector3 vector2 = vector + Tool2D.IgnoreZPoint(UnityEngine.Random.insideUnitSphere) * base.SummonerSpellBase.wandShootAngle * 0.06f;
		float num = Vector3.Distance(vector2, base.transform.position);
		float num2 = ((num >= bombCloseAttackRange) ? (num / bombLandDistanceSpeedRatio * bombLandTime.RandomResult()) : bombCloseAttackLandTime);
		spellCfg2.gravity = spellGravity * bombMaxHeightRange.RandomResult();
		spellCfg2.upSpeed = (0f - spellCfg2.gravity) * num2 / 2f;
		SpellInitialParameter spellInitialParameter = new SpellInitialParameter(myPpt, Tool2D.IgnoreZV2ToV1Normal(vector2, spawnPos), spellCfg2.id, spellCfg2.Copy(), base.SummonerSpellBase.InitialParameter.shooterWand, base.SummonerSpellBase.InitialParameter.shootSpellPreSpells)
		{
			RefractionInfo = base.SummonerSpellBase.SIP.RefractionInfo,
			shooterWand = base.SummonerSpellBase.SIP.shooterWand,
			shooterWandCfg = base.SummonerSpellBase.SIP.shooterWandCfg,
			WandPostSlotChargeData = base.SummonerSpellBase.SIP.WandPostSlotChargeData,
			lightningChainDamage = base.SummonerSpellBase.SIP.lightningChainDamage,
			extraSizeRatio = base.SummonerSpellBase.SIP.extraSizeRatio,
			finalSizeRatio = base.SummonerSpellBase.SIP.finalSizeRatio,
			SpellVolumeRatio = base.SummonerSpellBase.SIP.SpellVolumeRatio
		};
		if (base.SummonerSpellBase.SIP.spellIsFall)
		{
			spellInitialParameter.finalShootSpatialInfo = ShootSpellSpatialInfo.ToPoint(base.transform.position, vector);
			spellInitialParameter.fallExplosionRadius = base.SummonerSpellBase.SIP.fallExplosionRadius;
		}
		component.Initialize(spellInitialParameter);
		component.ColorType = base.SummonerSpellBase.ColorType;
		component.spellCfg.speed = Vector3.Distance(vector2, base.transform.position) / num2;
		component.CurrentSpeed = component.spellCfg.speed;
		component.rigid.linearVelocity = component.Direction * component.CurrentSpeed;
		component.voidExplosionInfo = base.SummonerSpellBase.voidExplosionInfo;
		component.spellVenomTime = base.SummonerSpellBase.spellVenomTime;
		component.spellVenomOnceCount = base.SummonerSpellBase.spellVenomOnceCount;
		component.spellMucusTime = base.SummonerSpellBase.spellMucusTime;
		component.spellMucusMoveSpeedRatio = base.SummonerSpellBase.spellMucusMoveSpeedRatio;
		component.spellMucusSpellSpeedRatio = base.SummonerSpellBase.spellMucusSpellSpeedRatio;
		component.spellFrozenTime = base.SummonerSpellBase.spellFrozenTime;
		component.spellBurnTime = base.SummonerSpellBase.spellBurnTime;
		component.burnHpRatioPerSeconds = base.SummonerSpellBase.burnHpRatioPerSeconds;
		component.endThunderHitPercent = base.SummonerSpellBase.endThunderHitPercent;
		component.endThunderHitRadiu = base.SummonerSpellBase.endThunderHitRadiu;
		component.endTHunderHitChance = base.SummonerSpellBase.endTHunderHitChance;
		component.overalCriticalChance = base.SummonerSpellBase.overalCriticalChance;
		component.spellCfg.duration = (component.spellCfg.duration + base.SummonerSpellBase.bonusDuration) * base.SummonerSpellBase.finalDurationRatio;
		int num3 = Mathf.CeilToInt(SpellConfig.dic[base.SummonerSpellBase.spellCfg.id].damage * (5f + 15f * (float)base.SummonerSpellBase.SIP.summonAdvanceSkillType1Level));
		component.spellCfg.damage = Mathf.CeilToInt((float)num3 * base.SummonerSpellBase.damageRatio * base.SummonerSpellBase.finalDamageRatio + base.SummonerSpellBase.SIP.finalDamageExtra);
		component.damageRatio = base.SummonerSpellBase.damageRatio;
		component.finalDamageRatio = base.SummonerSpellBase.finalDamageRatio;
		if (component.currentSpellMovement == SpellSpecialMovementType.Rotation && component.spellAroundOwnerRadius > 0f)
		{
			component.spellAroundOwnerRadius = base.SummonerSpellBase.spellAroundOwnerRadius * base.SummonerSpellBase.radiusRatio * base.SummonerSpellBase.finalRadiusRatio;
		}
		component.OwnerTsf = base.transform;
		component.OwnerPoint = base.transform.position;
		((Spell90012BoBoBomb)component).HandleSpecialCase();
	}

	public override void AnimaAction(string animaName)
	{
		switch (animaName)
		{
		case "Shoot":
			SpawnNormalBullet(GetShootStartPosition(bulletShootFrom.normal));
			break;
		case "ChargeStart":
		{
			GameObject effect = base.SummonerSpellBase.GetEffect("Charge_" + base.SummonerSpellBase.ColorType, base.transform.position + new Vector3(0f, 0.15f * base.transform.localScale.x, 0f), 1.5f / base.SummonerSpellBase.GetSummonValueRatio().attackSpeedRatio);
			ChargeEffectList = effect.transform;
			effect.transform.localScale = base.transform.localScale;
			foreach (Transform item in effect.transform.Find("Tsf").transform)
			{
				ParticleSystem.MainModule main = item.GetComponent<ParticleSystem>().main;
				main.simulationSpeed = base.SummonerSpellBase.GetSummonValueRatio().attackSpeedRatio;
			}
			{
				foreach (Transform item2 in effect.transform.Find("Tsf2").transform)
				{
					ParticleSystem.MainModule main2 = item2.GetComponent<ParticleSystem>().main;
					main2.simulationSpeed = base.SummonerSpellBase.GetSummonValueRatio().attackSpeedRatio;
				}
				break;
			}
		}
		case "ChargeShoot":
			SpawnBoBoBomb(GetShootStartPosition(bulletShootFrom.normal));
			break;
		case "ChargeShootRight":
			SpawnBoBoBomb(GetShootStartPosition(bulletShootFrom.right));
			break;
		case "ChargeShootLeft":
			SpawnBoBoBomb(GetShootStartPosition(bulletShootFrom.left));
			break;
		case "AttackFinish":
			if (base.CanMove)
			{
				ChangeState(new StateRunToTarget());
			}
			base.Anima.speed = 1f;
			break;
		default:
			Debug.LogError(animaName);
			break;
		}
	}

	public override void SummonsThrough()
	{
		if (SummonMayThroughMap())
		{
			SummonFollowOwnerThroughMap();
			return;
		}
		base.SummonerSpellBase.SpellSummonAfterDeadSpawnWormCount = 0;
		base.SummonerSpellBase.SIP.SpellSummonimmuteDeathTime = 0f;
		myPpt.ClearVoidState();
		base.SummonsThrough();
		myPpt.AnnouncedDeath(new TakeDamageInfo
		{
			isPlayDeadSE = false,
			isCreateDeadEF = false,
			isTeammateThrough = true
		});
	}

	private bool TrySwitchState_RunToTarget()
	{
		GetNearestTarget();
		if (targetPpt != null)
		{
			ChangeState(new StateRunToTarget());
			return true;
		}
		return false;
	}
}
