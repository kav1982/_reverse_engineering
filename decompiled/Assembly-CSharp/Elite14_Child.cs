using System;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

public class Elite14_Child : UnitBase, IComparable<Elite14_Child>
{
	public enum MonsterState
	{
		Hide,
		BornIdle,
		RandomMove,
		Idle,
		Move,
		Aim,
		Attack,
		AttackAfter,
		DashPrepare,
		Dash,
		Cannon,
		CannonAfter
	}

	[Header("状态")]
	public MonsterState _state;

	public StateVariableMgr varMgr = new StateVariableMgr();

	private bool stateQuit;

	private bool changedState;

	private float stateExistTime;

	public AIPattern pattern;

	[Header("基础行动")]
	public VariableFloat IdleTime;

	public VariableFloat RandomMoveTime;

	public VariableFloat RandomMoveRadius;

	public float SideWalkChance;

	public VariableFloat SpeedRandom;

	[Header("表现")]
	public Transform tsf_Model;

	public Shadow thisShadow;

	[Header("近战杂兵缓慢加速")]
	public float accleration;

	public float speedLimit;

	[Header("远程攻击")]
	public ParticleSystem chargeParticle;

	public ParticleSystem attackParticle;

	public int bulletDamage;

	public float bulletLifeTime;

	public float bulletSpeed;

	public float bulletHeight;

	public float attackDistanceOffsetRange;

	public float bulletAttackDistance;

	public VariableFloat attackKeepDistance;

	public VariableFloat attackCD;

	public VariableFloat attackAngleRange;

	public float doubleAttackAngleOffset;

	private float attackCDTimer;

	private SpellInitialParameter sipBullet = new SpellInitialParameter();

	[Header("创人")]
	public float dashStartDistance;

	public VariableFloat dashCD;

	public float dashSpeedMultiplier;

	public float dashDuration;

	public float dashAimTime;

	private float dashCDTimer;

	private Vector3 dashAimDir;

	public float dashDamage;

	public float dashKnockBack;

	public ParticleSystem dashParticle;

	[Header("撞墙修正")]
	public UnityEngine.Collider wallTrigger;

	private List<Entity> dashedEntities = new List<Entity>();

	private List<float> dashedTimer = new List<float>();

	[Header("自爆")]
	public int explodeDirations;

	public float explodeSpeedAccleration;

	public float explodeMaxSpeed;

	[Header("五模式后摇")]
	public float attackAfterTime;

	[Header("大炮模式")]
	public float cannonPredictTime;

	public VariableFloat cannonSpreadRadius;

	public VariableFloat cannonCD;

	public VariableFloat cannonFlyTime;

	public float cannonStartHeight;

	[Header("狂暴")]
	public bool isBuffed;

	public float buffedSpeedRatio;

	public float buffedDefenceRatio;

	public SpriteRenderer buffedEffect;

	public ParticleSystem buffedParticle;

	public float nowSpeedRatio;

	[Header("玩家隐身处理")]
	public float noTargetSkillChargeRatio;

	[Header("和谐模式")]
	public ParticleSystem buffedParticle_H;

	public UnityEngine.Material mat_buffedEffect_H;

	private SpellSpawnParams ssp;

	public MonsterState state
	{
		get
		{
			return _state;
		}
		set
		{
			stateExistTime = 0f;
			stateQuit = true;
			_state = value;
			varMgr.Clear();
		}
	}

	public int CompareTo(Elite14_Child other)
	{
		Vector3 centerPoint = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint;
		float num = Tool2D.IgnoreZAngleWithSign(Vector3.up, base.transform.position - centerPoint);
		if (num < 0f)
		{
			num += 360f;
		}
		float num2 = Tool2D.IgnoreZAngleWithSign(Vector3.up, other.transform.position - centerPoint);
		if (num2 < 0f)
		{
			num2 += 360f;
		}
		if (num > num2)
		{
			return -1;
		}
		if (num < num2)
		{
			return 1;
		}
		return 0;
	}

	public override void SingleInitialCallback()
	{
		if (pattern == AIPattern.Pattern2)
		{
			ssp = UnitDotsSyncSystem.GetSpellPrototype(90271);
			UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
			sSPModifier.Shooter = myPpt.myEntity;
			sSPModifier.Speed = bulletSpeed;
			sSPModifier.Damage = bulletDamage;
			sSPModifier.Duration = bulletLifeTime;
			sSPModifier.ApplyToSSP(ref ssp);
		}
		myPpt.RemoveSRFromArray(buffedEffect);
		buffedEffect.enabled = false;
		nowSpeedRatio = 1f;
		buffedParticle.Stop();
		buffedParticle.Clear();
		if (GameMgr.IsHarmony_Static)
		{
			buffedParticle = buffedParticle_H;
			UnityEngine.Object.Destroy(buffedEffect.material);
			buffedEffect.material = mat_buffedEffect_H;
			buffedParticle.Stop();
			buffedParticle.Clear();
		}
	}

	public override void EveryInitialCallback()
	{
		LevelMgr.Inst.CurrentRoomCtrller.UnitUnregister(myPpt.myEntity);
		state = MonsterState.Hide;
		tsf_Model.gameObject.SetActive(value: false);
		base.CC_Self.enabled = false;
		SetDotsCCEnable(isOpen: false);
		UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
		componentData.CanBeTarget = false;
		componentData.CanTouch = false;
		componentData.showAffect = false;
		componentData.unitCfg.moveSpeed = SpeedRandom.RandomResult();
		SetComponentData(componentData);
		thisShadow.Hide();
		isBuffed = false;
		if (pattern == AIPattern.Pattern2 || pattern == AIPattern.Pattern6)
		{
			chargeParticle.Stop();
			chargeParticle.Clear();
		}
		if (pattern == AIPattern.Pattern3)
		{
			dashParticle.Stop();
			dashParticle.Clear();
		}
		buffedParticle.Stop();
		buffedParticle.Clear();
	}

	public void TryAttack()
	{
		if (pattern == AIPattern.Pattern2)
		{
			if (attackCD.result == 0f)
			{
				attackCD.RandomResult();
			}
			if (base.HaveTarget)
			{
				attackCDTimer += Time.deltaTime;
			}
			else
			{
				attackCDTimer += Time.deltaTime * noTargetSkillChargeRatio;
			}
			if (attackCDTimer > attackCD.result && (!base.HaveTarget || (ToTargetDistanceSqr() < Mathf.Pow(bulletAttackDistance, 2f) && state != MonsterState.Attack)))
			{
				attackCD.RandomResult();
				attackCDTimer = 0f;
				state = MonsterState.Attack;
			}
		}
	}

	public void TryDash()
	{
		if (pattern == AIPattern.Pattern3)
		{
			if (base.HaveTarget && dashCD.result == 0f)
			{
				dashCD.RandomResult();
			}
			if (base.HaveTarget)
			{
				dashCDTimer += Time.deltaTime;
			}
			else
			{
				dashCDTimer += Time.deltaTime * noTargetSkillChargeRatio;
			}
			if (dashCDTimer > dashCD.result && (!base.HaveTarget || ToTargetDistanceSqr() < Mathf.Pow(dashStartDistance, 2f)))
			{
				dashCD.RandomResult();
				dashCDTimer = 0f;
				state = MonsterState.DashPrepare;
			}
		}
	}

	public void TryCannon()
	{
		if (pattern == AIPattern.Pattern6)
		{
			if (cannonCD.result == 0f)
			{
				cannonCD.RandomResult();
			}
			if (base.HaveTarget)
			{
				attackCDTimer += Time.deltaTime;
			}
			else
			{
				attackCDTimer += Time.deltaTime * noTargetSkillChargeRatio;
			}
			if (attackCDTimer > cannonCD.result)
			{
				cannonCD.RandomResult();
				attackCDTimer = 0f;
				state = MonsterState.Cannon;
			}
		}
	}

	protected override void SetFlip(float motionX)
	{
		if (Mathf.Abs(motionX) > 0.01f)
		{
			tsf_Model.localScale = new Vector3(Mathf.Sign(motionX) * Mathf.Abs(tsf_Model.localScale.x), tsf_Model.localScale.y, tsf_Model.localScale.z);
		}
	}

	public override void Update()
	{
		if (Elite14.Inst.myPpt.AlreadyDead)
		{
			if (state == MonsterState.Hide)
			{
				TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(myPpt.myEntity);
				info.dontCreatebloodSplat = true;
				info.dontPlayDeadSE = true;
				info.dontCreateDeadEF = true;
				DotsAnnouncedDeath(info);
			}
			else
			{
				DotsAnnouncedDeath();
			}
			return;
		}
		for (int num = dashedTimer.Count - 1; num >= 0; num--)
		{
			dashedTimer[num] -= Time.deltaTime;
			if (dashedTimer[num] < 0f)
			{
				dashedTimer.RemoveAt(num);
				dashedEntities.RemoveAt(num);
			}
		}
		if (isBuffed != buffedEffect.enabled)
		{
			buffedEffect.enabled = isBuffed;
			if (isBuffed)
			{
				buffedParticle.Play();
			}
			nowSpeedRatio = (isBuffed ? buffedSpeedRatio : 1f);
		}
		base.Update();
		if (base.IsLocked)
		{
			return;
		}
		if (stateQuit)
		{
			stateQuit = false;
			changedState = true;
		}
		else
		{
			changedState = false;
		}
		stateExistTime += Time.deltaTime;
		if (pattern == AIPattern.Pattern1 && myPpt.unitCfg.moveSpeed < speedLimit)
		{
			myPpt.unitCfg.moveSpeed += Time.deltaTime * accleration;
			myPpt.unitCfg.moveSpeed = Mathf.Min(myPpt.unitCfg.moveSpeed, speedLimit);
			UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
			componentData.unitCfg.moveSpeed = myPpt.unitCfg.moveSpeed;
			SetComponentData(componentData);
		}
		switch (state)
		{
		case MonsterState.Hide:
			if (changedState)
			{
				tsf_Model.gameObject.SetActive(value: false);
				base.CC_Self.enabled = false;
				SetDotsCCEnable(isOpen: false);
				UnitProperty_Dots componentData2 = GetComponentData<UnitProperty_Dots>();
				componentData2.CanBeTarget = false;
				componentData2.CanTouch = false;
				componentData2.showAffect = false;
				SetComponentData(componentData2);
				myPpt.CanBeTarget = false;
				myPpt.CanTouch = false;
				thisShadow.Hide();
			}
			if (stateExistTime > Elite14.Inst.summonDelayTime)
			{
				state = MonsterState.BornIdle;
			}
			break;
		case MonsterState.BornIdle:
			if (changedState)
			{
				tsf_Model.gameObject.SetActive(value: true);
				if (pattern == AIPattern.Pattern3)
				{
					base.Anima.Play("Idle 1");
				}
				else
				{
					base.Anima.Play("Idle");
				}
				base.CC_Self.enabled = true;
				SetDotsCCEnable(isOpen: true);
				UnitProperty_Dots componentData5 = GetComponentData<UnitProperty_Dots>();
				componentData5.CanBeTarget = true;
				componentData5.CanTouch = true;
				componentData5.showAffect = true;
				SetComponentData(componentData5);
				thisShadow.Show();
				if (!myPpt.AlreadyDead)
				{
					LevelMgr.Inst.CurrentRoomCtrller.UnitRegister(myPpt.myEntity);
				}
			}
			if (stateExistTime > 0.5f)
			{
				state = MonsterState.Move;
			}
			break;
		case MonsterState.Move:
		{
			ref bool reference2 = ref varMgr.RegBool(0);
			if (changedState)
			{
				if (pattern == AIPattern.Pattern3)
				{
					base.Anima.Play("Move 1");
				}
				else
				{
					base.Anima.Play("Move");
				}
				GetNearestTarget();
				attackKeepDistance.RandomResult();
			}
			if (!base.HaveTarget)
			{
				GetNearestTarget();
			}
			if (!base.HaveTarget)
			{
				state = MonsterState.RandomMove;
				break;
			}
			switch (pattern)
			{
			case AIPattern.Pattern1:
				SetMove(ToTargetDir() * base.MoveSpeed * nowSpeedRatio);
				break;
			case AIPattern.Pattern2:
				TryAttack();
				if ((ToTargetDistanceSqr() > Mathf.Pow(attackKeepDistance.value2, 2f)) & reference2)
				{
					reference2 = false;
				}
				else if (ToTargetDistanceSqr() < Mathf.Pow(attackKeepDistance.value1, 2f) && !reference2)
				{
					reference2 = true;
				}
				SetMove(((!reference2) ? 1 : (-1)) * ToTargetDir() * base.MoveSpeed * nowSpeedRatio);
				if (Mathf.Abs(ToTargetDistance() - attackKeepDistance.result) < attackDistanceOffsetRange)
				{
					state = MonsterState.Aim;
				}
				break;
			case AIPattern.Pattern3:
				state = MonsterState.RandomMove;
				break;
			case AIPattern.Pattern6:
				state = MonsterState.RandomMove;
				break;
			case AIPattern.Pattern4:
			case AIPattern.Pattern5:
				break;
			}
			break;
		}
		case MonsterState.RandomMove:
		{
			ref bool reference = ref varMgr.RegBool(0);
			if (changedState)
			{
				if (pattern == AIPattern.Pattern3)
				{
					base.Anima.Play("Move 1");
				}
				else
				{
					base.Anima.Play("Move");
				}
				RandomMoveTime.RandomResult();
				reference = GeneralTool.ChanceResult(SideWalkChance);
				if (base.HaveTarget && !reference)
				{
					GetNavInfo(Tool2D.GetNavMeshPoint(base.transform.position, RandomMoveRadius, ToTargetDir(), 0f));
				}
				else
				{
					GetNavInfo(Tool2D.GetNavMeshPoint(base.transform.position, RandomMoveRadius));
				}
			}
			GetNearestTargetWithTimer();
			if (base.HaveTarget && pattern != AIPattern.Pattern3 && pattern != AIPattern.Pattern6)
			{
				state = MonsterState.Move;
				break;
			}
			if (stateExistTime > RandomMoveTime.result)
			{
				state = MonsterState.Idle;
				break;
			}
			CheckNavInfo();
			SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed * nowSpeedRatio);
			if (navInfo.allCornerArrived)
			{
				if (base.HaveTarget && !reference)
				{
					GetNavInfo(Tool2D.GetNavMeshPoint(base.transform.position, RandomMoveRadius, ToTargetDir(), 0f));
				}
				else
				{
					GetNavInfo(Tool2D.GetNavMeshPoint(base.transform.position, RandomMoveRadius));
				}
			}
			TryAttack();
			TryDash();
			TryCannon();
			break;
		}
		case MonsterState.Idle:
			if (changedState)
			{
				if (pattern == AIPattern.Pattern3)
				{
					base.Anima.Play("Idle 1");
				}
				else
				{
					base.Anima.Play("Idle");
				}
				IdleTime.RandomResult();
			}
			GetNearestTargetWithTimer();
			if (base.HaveTarget && pattern != AIPattern.Pattern6)
			{
				state = MonsterState.Move;
				break;
			}
			SetMove(Vector3.zero, isFlip: false);
			if (stateExistTime > IdleTime.result)
			{
				state = MonsterState.RandomMove;
				break;
			}
			TryAttack();
			TryDash();
			TryCannon();
			break;
		case MonsterState.Aim:
			if (changedState)
			{
				base.Anima.Play("Idle");
			}
			if (!base.HaveTarget)
			{
				GetNearestTarget();
			}
			if (!base.HaveTarget)
			{
				state = MonsterState.Move;
				break;
			}
			if (ToTargetDistanceSqr() > Mathf.Pow(attackKeepDistance.value2, 2f) || ToTargetDistanceSqr() < Mathf.Pow(attackKeepDistance.value1, 2f))
			{
				state = MonsterState.Move;
				break;
			}
			SetMove(Vector3.zero, isFlip: false);
			SetFlip(ToTargetDelta().x);
			TryAttack();
			break;
		case MonsterState.Attack:
			if (changedState)
			{
				base.Anima.Play("Attack");
				chargeParticle.Play();
			}
			if (base.HaveTarget)
			{
				SetFlip(ToTargetDelta().x);
			}
			SetMove(Vector3.zero, isFlip: false);
			TryAttack();
			break;
		case MonsterState.AttackAfter:
			if (changedState)
			{
				base.Anima.Play("Idle");
			}
			SetMove(Vector3.zero, isFlip: false);
			if (stateExistTime > attackAfterTime)
			{
				state = MonsterState.Move;
			}
			break;
		case MonsterState.DashPrepare:
			if (changedState)
			{
				base.Anima.Play("DashPrepare");
				dashAimDir = Tool2D.GetDir();
				GetNearestTargetPlayerFirst();
				if (base.HaveTarget)
				{
					dashAimDir = ToTargetDir();
				}
			}
			if (base.HaveTarget && stateExistTime < dashAimTime)
			{
				dashAimDir = ToTargetDir();
			}
			SetMove(Vector3.zero, isFlip: false);
			SetFlip(dashAimDir.x);
			break;
		case MonsterState.Dash:
			_ = ref varMgr.RegV3(0);
			if (changedState)
			{
				SEMgr.Inst.elite14ChildDash.PlaySE(SEPlayMode.Replay, 3, 0.2f).pitch = UnityEngine.Random.Range(0.8f, 1f);
				dashParticle.Play();
				wallTrigger.enabled = true;
				base.Anima.Play("Dash");
				base.gameObject.layer = LayerMask.NameToLayer("Monster_Ghost");
				PhysicsCollider pc = GetComponentData<PhysicsCollider>();
				DTool.SetCollider(in pc, 2048u, DTool.GetCollidesWith(8192u));
				SetComponentData(pc);
				UnitProperty_Dots componentData3 = GetComponentData<UnitProperty_Dots>();
				componentData3.CanTouch = false;
				componentData3.ImmuneKnockbackRegister();
				SetComponentData(componentData3);
			}
			SetMove(dashAimDir * base.MoveSpeed * dashSpeedMultiplier);
			if (stateExistTime > dashDuration)
			{
				wallTrigger.enabled = false;
				UnitProperty_Dots componentData4 = GetComponentData<UnitProperty_Dots>();
				componentData4.CanTouch = true;
				componentData4.ImmuneKnockbackUnregister();
				SetComponentData(componentData4);
				base.gameObject.layer = LayerMask.NameToLayer("Monster");
				PhysicsCollider pc2 = GetComponentData<PhysicsCollider>();
				DTool.SetCollider(in pc2, 2048u, DTool.GetCollidesWith(2048u));
				SetComponentData(pc2);
				state = MonsterState.Move;
				dashParticle.Stop();
			}
			break;
		case MonsterState.Cannon:
			if (changedState)
			{
				base.Anima.Play("Cannon");
				chargeParticle.Play();
			}
			SetMove(Vector3.zero, isFlip: false);
			if (base.HaveTarget)
			{
				SetFlip(ToTargetDelta().x);
			}
			break;
		}
	}

	public void Buff()
	{
		if (state != 0)
		{
			isBuffed = true;
		}
	}

	public void Trigger(Entity other)
	{
		if (state != MonsterState.Dash)
		{
			return;
		}
		uint layer = UnitDotsSyncSystem.GetLayer(other);
		Vector3 vector = UnitDotsSyncSystem.GetComponentData<LocalTransform>(other).Position;
		switch (layer)
		{
		case 256u:
		case 65536u:
		{
			UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
			componentData.CanTouch = true;
			componentData.ImmuneKnockbackUnregister();
			wallTrigger.enabled = false;
			base.gameObject.layer = LayerMask.NameToLayer("Monster");
			PhysicsCollider pc = GetComponentData<PhysicsCollider>();
			DTool.SetCollider(in pc, 2048u, DTool.GetCollidesWith(2048u));
			SetComponentData(pc);
			state = MonsterState.Move;
			dashParticle.Stop();
			break;
		}
		case 512u:
		case 32768u:
		case 131072u:
		case 2097152u:
			if (!dashedEntities.Contains(other))
			{
				dashedEntities.Add(other);
				dashedTimer.Add(0.5f);
				TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(myPpt.myEntity);
				info.damage = dashDamage;
				info.knockbackForce = ((vector - base.transform.position).normalized * 0.5f + base.CurrentMotion * 0.5f) * dashKnockBack;
				info.teammateTakeDamageRatio = 4f;
				if (layer == 131072)
				{
					info.damage = 999999f;
					info.ignoreFloatText = true;
				}
				if (layer != 32768)
				{
					ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_MonsterPunch_Large", vector + Tool2D.GetDir() * UnityEngine.Random.Range(0f, 0.2f) + new Vector3(0f, -1f, -0.5f), 1f);
					dashedEntities.Add(other);
					dashedTimer.Add(0.5f);
					SEMgr.Inst.monster37_KnockUnit.PlaySE();
				}
				UnitDotsSyncSystem.AddTakeDamageRequest(other, info);
			}
			break;
		}
	}

	public override void AfterDead(ref TakeDamageInfo_Dots info)
	{
		if (GeneralTool.ChanceResult(0.2f))
		{
			SEMgr.Inst.monster53_Shout.PlaySE(SEPlayMode.Unique, 3, 0.33f).volume = 0.8f * DataMgr.settingData.GetFinalSound();
		}
		Elite14.Inst.ChildDieDamage(myPpt.unitCfg.maxHP);
		base.AfterDead(ref info);
	}

	public override void AnimaAction(string animaName)
	{
		switch (animaName)
		{
		case "Attack":
		{
			chargeParticle.Stop();
			chargeParticle.Clear();
			attackParticle.Play();
			GetNearestTarget();
			Vector3 dir = Tool2D.GetDir();
			if (base.HaveTarget)
			{
				dir = Tool2D.GetDir(ToTargetDir(), attackAngleRange.RandomResult());
			}
			if (pattern == AIPattern.Pattern2)
			{
				UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
				sSPModifier.Direction = dir;
				sSPModifier.SpawnPosition = base.transform.position + new Vector3(0f, 0f, 0f - bulletHeight);
				sSPModifier.ApplyToSSP(ref ssp);
				ShootSpell(ssp);
			}
			else if (pattern == AIPattern.Pattern5)
			{
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Elite14_Bullet", base.transform.position).GetComponent<Boss6_Bullet>().InitializeSimple(Tool2D.GetDir(dir, 0f - doubleAttackAngleOffset), bulletSpeed, bulletLifeTime, useFakeHeight: false);
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Elite14_Bullet", base.transform.position).GetComponent<Boss6_Bullet>().InitializeSimple(Tool2D.GetDir(dir, doubleAttackAngleOffset), bulletSpeed, bulletLifeTime, useFakeHeight: false);
			}
			SetFlip(dir.x);
			break;
		}
		case "AttackFinish":
			if (pattern == AIPattern.Pattern5)
			{
				state = MonsterState.AttackAfter;
			}
			else
			{
				state = MonsterState.Move;
			}
			break;
		case "dashPrepareFinish":
			state = MonsterState.Dash;
			break;
		case "CannonShoot":
		{
			SEMgr.Inst.elite14ChildCannonShoot.PlaySE(SEPlayMode.Replay, 3, 0.2f);
			chargeParticle.Stop();
			chargeParticle.Clear();
			attackParticle.Play();
			GetNearestTarget();
			Vector3 vector = Elite14.Inst.GetRandomRoomPoint();
			if (base.HaveTarget)
			{
				vector = ((!(targetEntity == PlayerMgr.Inst.PlayerEtt)) ? base.TargetPoint : (base.TargetPoint + cannonPredictTime * PlayerMgr.Inst.PlayerCtrller.CurrentMotion));
				vector = Tool2D.GetNavMeshPointIngoreZ(vector, cannonSpreadRadius);
			}
			Elite14.MiniPool.GetGO("Prefabs/EF/EF_Elite14_Explosion", base.transform.position + Vector3.back * cannonStartHeight).GetComponent<Elite14_Cannon>().InitializeCannon(base.transform.position + Vector3.back * cannonStartHeight, vector, cannonFlyTime.RandomResult(), this);
			SetFlip((vector - base.transform.position).x);
			break;
		}
		case "CannonFinish":
			state = MonsterState.Idle;
			break;
		}
	}
}
