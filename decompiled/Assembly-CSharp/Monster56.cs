using Unity.Entities;
using Unity.Physics;
using UnityEngine;

public class Monster56 : UnitBase
{
	public enum MonsterState
	{
		BornIdle,
		Idle,
		RandomMove,
		Move,
		MoveBreak,
		Attack,
		FakeDie,
		Reborn,
		RebornIdle,
		KeepDistance
	}

	[Header("状态机")]
	public MonsterState _state;

	public StateVariableMgr varMgr = new StateVariableMgr();

	private bool stateQuit;

	private bool changedState;

	private float stateExistTime;

	[Header("基础行动")]
	public VariableFloat IdleTime;

	public VariableFloat RandomMoveTime;

	public VariableFloat RandomMoveRadius;

	public VariableFloat moveTime;

	public VariableFloat moveBreakTime;

	public float SideWalkChance;

	[Header("翻转")]
	public Transform tsf_Model;

	[Header("复活")]
	public PhysicsMaterial PM_Fly;

	public PhysicsMaterial PM_Common;

	public float fakeDieHpRatio;

	public float fakeDieTime;

	public float rebornTime;

	public int maxRebornCount;

	private int rebornCounter;

	public Shadow thisShadow;

	public float normalShadowScale;

	public float fakeDieShadowScale;

	public ParticleSystem rebornParticle;

	[Header("远程")]
	public AIPattern pattern;

	public float keepDistance;

	private SpellSpawnParams ssp;

	public float spellHeight;

	public float spellSpeed;

	public float spellDuration;

	public int spellDamage;

	public int bulletCount;

	public float bulletSpreadRange;

	public VariableFloat spellAngleRange;

	public VariableFloat attackCD;

	private float attackCDTimer;

	public ParticleSystem attackParticle;

	public ParticleSystem attackParticle_H;

	[Header("分裂模式")]
	public float minKnockBack;

	public float maxKnockBack;

	public bool isSpliter;

	public VariableInt splitHeadCount;

	public VariableFloat splitKnockBackRatio;

	[Header("和谐")]
	public SpriteRenderer eyeGlowRenderer;

	public SpriteRenderer eyeGlowRenderer1;

	public SpriteRenderer headRenderer;

	public SpriteRenderer headRenderer_1;

	public SpriteRenderer bodyRenderer;

	public Sprite head_H;

	public Sprite body_H;

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

	public override void SingleInitialCallback()
	{
		ssp = UnitDotsSyncSystem.GetSpellPrototype(90101);
		UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
		sSPModifier.Damage = spellDamage;
		sSPModifier.Duration = spellDuration;
		sSPModifier.Speed = spellSpeed;
		sSPModifier.ReboundCount = 1;
		sSPModifier.Shooter = myPpt.myEntity;
		sSPModifier.ApplyToSSP(ref ssp);
	}

	public void InitializeHead(Vector3 knockback)
	{
		if (knockback == Vector3.zero)
		{
			knockback = Tool2D.GetDir();
		}
		knockback = knockback.normalized * Mathf.Clamp(knockback.magnitude, minKnockBack, maxKnockBack);
		myPpt.SetFlip(0f - myPpt.Rigid.linearVelocity.x > 0f);
		myPpt.Rigid.linearVelocity = knockback;
		SyncDotsVelocity();
		state = MonsterState.FakeDie;
		UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
		componentData.unitCfg.currentHP = myPpt.unitCfg.maxHP * fakeDieHpRatio;
		SetComponentData(componentData);
		myPpt.unitCfg.currentHP = myPpt.unitCfg.maxHP * fakeDieHpRatio;
	}

	public unsafe override void EveryInitialCallback()
	{
		state = MonsterState.BornIdle;
		base.CC_Self.enabled = true;
		SetDotsCCEnable(isOpen: true);
		base.CC_Self.material = PM_Common;
		PhysicsCollider componentData = GetComponentData<PhysicsCollider>();
		componentData.ColliderPtr->SetRestitution(PM_Common.bounciness);
		SetComponentData(componentData);
		if (GameMgr.IsHarmony_Static)
		{
			if (headRenderer != null)
			{
				headRenderer.sprite = head_H;
			}
			if (headRenderer_1 != null)
			{
				headRenderer_1.sprite = head_H;
			}
			bodyRenderer.sprite = body_H;
			if (eyeGlowRenderer != null)
			{
				eyeGlowRenderer.enabled = false;
			}
			if (eyeGlowRenderer1 != null)
			{
				eyeGlowRenderer1.enabled = false;
			}
			if (attackParticle != null)
			{
				attackParticle = attackParticle_H;
			}
		}
		attackCD.RandomResult();
		rebornCounter = 0;
		attackCDTimer = Random.Range(0f, attackCD.result / 2f);
		rebornParticle.Stop();
		rebornParticle.Clear();
	}

	protected override void SetFlip(float motionX)
	{
		Vector3 localScale = tsf_Model.localScale;
		Vector3 localPosition = tsf_Model.localPosition;
		localScale.x = Mathf.Abs(localScale.x) * Mathf.Sign(motionX);
		localPosition.x = Mathf.Abs(localPosition.x) * Mathf.Sign(motionX);
		tsf_Model.localScale = localScale;
		tsf_Model.localPosition = localPosition;
	}

	public unsafe override void Update()
	{
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
		switch (state)
		{
		case MonsterState.BornIdle:
			if (changedState)
			{
				thisShadow.SetScale(normalShadowScale);
				base.gameObject.layer = LayerMask.NameToLayer("Monster");
				PhysicsCollider pc3 = GetComponentData<PhysicsCollider>();
				DTool.SetCollider(in pc3, 2048u, DTool.GetCollidesWith(2048u));
				if (!isSpliter)
				{
					if (pattern == AIPattern.Pattern1)
					{
						base.Anima.Play("Idle");
					}
					else
					{
						base.Anima.Play("Idle1");
					}
				}
				else if (pattern == AIPattern.Pattern1)
				{
					base.Anima.Play("Idle2");
				}
				else
				{
					base.Anima.Play("Idle3");
				}
			}
			if (stateExistTime > 0.5f)
			{
				state = MonsterState.Move;
			}
			break;
		case MonsterState.RebornIdle:
			if (changedState)
			{
				thisShadow.SetScale(normalShadowScale);
				base.gameObject.layer = LayerMask.NameToLayer("Monster");
				PhysicsCollider pc2 = GetComponentData<PhysicsCollider>();
				DTool.SetCollider(in pc2, 2048u, DTool.GetCollidesWith(2048u));
				base.Anima.Play("RebornIdle");
			}
			if (stateExistTime > 0.5f)
			{
				state = MonsterState.Move;
			}
			break;
		case MonsterState.Move:
		{
			ref bool reference = ref varMgr.RegBool(0);
			if (changedState)
			{
				if (!isSpliter)
				{
					if (pattern == AIPattern.Pattern1)
					{
						base.Anima.Play("Move");
					}
					else
					{
						base.Anima.Play("Move1");
					}
				}
				else if (pattern == AIPattern.Pattern1)
				{
					base.Anima.Play("Move2");
				}
				else
				{
					base.Anima.Play("Move3");
				}
				checkTargetIntervalTimer = 0f;
				moveTime.RandomResult();
				if (GeneralTool.ChanceResult(SideWalkChance))
				{
					GetNavInfo(Tool2D.GetNavMeshPoint(base.transform.position, RandomMoveRadius));
					reference = true;
				}
			}
			if (!base.HaveTarget)
			{
				GetNearestTarget();
			}
			if (!base.HaveTarget)
			{
				state = MonsterState.Idle;
				break;
			}
			if (!reference)
			{
				GetNavInfoWithTimer(base.TargetPoint);
				CheckNavInfo();
				SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed);
			}
			else
			{
				CheckNavInfo();
				SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed);
				if (navInfo.allCornerArrived)
				{
					GetNavInfo(Tool2D.GetNavMeshPoint(base.transform.position, RandomMoveRadius));
				}
			}
			if (base.HaveTarget && pattern == AIPattern.Pattern2 && ToTargetDistanceSqr() < keepDistance * keepDistance)
			{
				state = MonsterState.Idle;
			}
			else if (stateExistTime > moveTime.result)
			{
				state = MonsterState.MoveBreak;
			}
			else
			{
				CheckAttack();
			}
			break;
		}
		case MonsterState.MoveBreak:
			if (changedState)
			{
				if (!isSpliter)
				{
					if (pattern == AIPattern.Pattern1)
					{
						base.Anima.Play("Idle");
					}
					else
					{
						base.Anima.Play("Idle1");
					}
				}
				else if (pattern == AIPattern.Pattern1)
				{
					base.Anima.Play("Idle2");
				}
				else
				{
					base.Anima.Play("Idle3");
				}
				moveBreakTime.RandomResult();
			}
			SetMove(Vector3.zero, isFlip: false);
			if (stateExistTime > moveBreakTime.result)
			{
				state = MonsterState.Move;
			}
			else
			{
				CheckAttack();
			}
			break;
		case MonsterState.Idle:
			if (changedState)
			{
				if (!isSpliter)
				{
					if (pattern == AIPattern.Pattern1)
					{
						base.Anima.Play("Idle");
					}
					else
					{
						base.Anima.Play("Idle1");
					}
				}
				else if (pattern == AIPattern.Pattern1)
				{
					base.Anima.Play("Idle2");
				}
				else
				{
					base.Anima.Play("Idle3");
				}
				IdleTime.RandomResult();
			}
			GetNearestTargetWithTimer();
			if (base.HaveTarget && pattern == AIPattern.Pattern1)
			{
				state = MonsterState.Move;
				break;
			}
			if (base.HaveTarget && pattern == AIPattern.Pattern2 && ToTargetDistanceSqr() > keepDistance * keepDistance)
			{
				state = MonsterState.Move;
				break;
			}
			if (stateExistTime > IdleTime.result)
			{
				state = MonsterState.RandomMove;
				break;
			}
			SetMove(Vector3.zero, isFlip: false);
			CheckAttack();
			break;
		case MonsterState.RandomMove:
			if (!isSpliter)
			{
				if (pattern == AIPattern.Pattern1)
				{
					base.Anima.Play("Move");
				}
				else
				{
					base.Anima.Play("Move1");
				}
			}
			else if (pattern == AIPattern.Pattern1)
			{
				base.Anima.Play("Move2");
			}
			else
			{
				base.Anima.Play("Move3");
			}
			GetNearestTargetWithTimer();
			if (base.HaveTarget && pattern == AIPattern.Pattern1)
			{
				state = MonsterState.Move;
				break;
			}
			if (base.HaveTarget && pattern == AIPattern.Pattern2 && ToTargetDistanceSqr() > keepDistance * keepDistance)
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
			SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed);
			if (navInfo.allCornerArrived)
			{
				GetNavInfo(Tool2D.GetNavMeshPoint(base.transform.position, RandomMoveRadius));
			}
			CheckAttack();
			break;
		case MonsterState.FakeDie:
			if (changedState)
			{
				base.gameObject.layer = LayerMask.NameToLayer("Monster_Fly");
				thisShadow.SetScale(fakeDieShadowScale);
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Monster56_FakeDie", base.transform.position + new Vector3(0f, 0f, -0.5f));
				base.Anima.Play("FakeDie");
				base.CurrentMotion = Vector3.zero;
				SEMgr.Inst.monster56_FakeDie.PlaySE();
				SEMgr.Inst.dead_Bone.PlaySE();
				base.CC_Self.material = PM_Fly;
				PhysicsCollider pc = GetComponentData<PhysicsCollider>();
				DTool.SetCollider(in pc, 2048u, DTool.GetCollidesWith(4096u));
				pc.ColliderPtr->SetRestitution(PM_Fly.bounciness);
				SetComponentData(pc);
				UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
				componentData.JumpStartSetting();
				componentData.CanTouch = false;
				SetComponentData(componentData);
			}
			SetMove(Vector3.zero, isFlip: false);
			if (stateExistTime > fakeDieTime)
			{
				state = MonsterState.Reborn;
			}
			break;
		case MonsterState.Reborn:
			if (changedState)
			{
				base.Anima.Play("Reborn");
				UnitProperty_Dots componentData2 = GetComponentData<UnitProperty_Dots>();
				componentData2.CanTouch = true;
				SetComponentData(componentData2);
				myPpt.CC_Self.enabled = true;
				SetDotsCCEnable(isOpen: true);
				base.CurrentMotion = Vector3.zero;
				SEMgr.Inst.monster56_Reborn.PlaySE();
				rebornParticle.Play();
			}
			SetMove(Vector3.zero, isFlip: false);
			if (stateExistTime > rebornTime)
			{
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Monster56_FakeDie", base.transform.position + new Vector3(0f, 0f, -0.5f));
				UnitProperty_Dots componentData3 = GetComponentData<UnitProperty_Dots>();
				componentData3.ImmuneKnockbackUnregister();
				SetComponentData(componentData3);
				UnitDotsSyncSystem.UnitRecoveryHP(myPpt.myEntity, myPpt.unitCfg.maxHP, World.DefaultGameObjectInjectionWorld.EntityManager, needTextFloat: false);
				SEMgr.Inst.monster56_RebornFinish.PlaySE();
				state = MonsterState.RebornIdle;
				rebornParticle.Stop();
			}
			break;
		case MonsterState.Attack:
			if (changedState)
			{
				if (!isSpliter)
				{
					base.Anima.Play("Attack");
				}
				else
				{
					base.Anima.Play("Attack1");
				}
				GetNearestTarget();
			}
			if (base.HaveTarget)
			{
				SetFlip(ToTargetDelta().x);
			}
			SetMove(Vector3.zero, isFlip: false);
			break;
		}
	}

	private void CheckAttack()
	{
		if (pattern != 0)
		{
			attackCDTimer += Time.deltaTime;
			if (attackCDTimer > attackCD.result)
			{
				attackCDTimer = 0f;
				attackCD.RandomResult();
				state = MonsterState.Attack;
			}
		}
	}

	public override void BeforeAnnouncedDeath_Dots(ref TakeDamageInfo_Dots info)
	{
		if (isSpliter)
		{
			return;
		}
		if (state != MonsterState.FakeDie && state != MonsterState.Reborn && maxRebornCount > rebornCounter)
		{
			info.stopAnnouncedDeath = true;
			rebornCounter++;
			state = MonsterState.FakeDie;
			if (base.Rigid.linearVelocity == Vector3.zero)
			{
				base.Rigid.linearVelocity = Tool2D.GetDir();
			}
			base.Rigid.linearVelocity = myPpt.Rigid.linearVelocity.normalized * Mathf.Clamp(splitKnockBackRatio.RandomResult(), minKnockBack, maxKnockBack);
			PhysicsVelocity componentData = GetComponentData<PhysicsVelocity>();
			componentData.Linear = base.Rigid.linearVelocity;
			SetComponentData(componentData);
			myPpt.unitCfg.currentHP = myPpt.unitCfg.maxHP * fakeDieHpRatio;
			UnitProperty_Dots componentData2 = GetComponentData<UnitProperty_Dots>();
			componentData2.unitCfg.currentHP = myPpt.unitCfg.currentHP;
			SetComponentData(componentData2);
		}
		else if (state != MonsterState.FakeDie && state != MonsterState.Reborn)
		{
			UnitProperty_Dots componentData3 = GetComponentData<UnitProperty_Dots>();
			componentData3.unitCfg.corpseCount = 8;
			SetComponentData(componentData3);
		}
		else
		{
			UnitProperty_Dots componentData4 = GetComponentData<UnitProperty_Dots>();
			componentData4.unitCfg.corpseCount = 3;
			SetComponentData(componentData4);
		}
	}

	public override void AfterDead(ref TakeDamageInfo_Dots info)
	{
		if (isSpliter)
		{
			myPpt.ClearVoidState();
			for (int i = 0; i < splitHeadCount.RandomResult(); i++)
			{
				ObjPoolMgr.Inst.GetGO("Prefabs/Units/" + (myPpt.unitCfg.id - 20), base.transform.position).GetComponent<Monster56>().InitializeHead(splitKnockBackRatio.RandomResult() * Tool2D.GetDir(base.Rigid.linearVelocity / myPpt.unitCfg.knockbackRatio, Random.Range(-60, 60)));
			}
		}
		base.AfterDead(ref info);
	}

	public unsafe override void AnimaAction(string animaName)
	{
		switch (animaName)
		{
		case "HeadFallBig":
		{
			base.gameObject.layer = LayerMask.NameToLayer("Monster");
			base.CC_Self.material = PM_Common;
			PhysicsCollider pc = GetComponentData<PhysicsCollider>();
			DTool.SetCollider(in pc, 2048u, DTool.GetCollidesWith(2048u));
			pc.ColliderPtr->SetRestitution(PM_Common.bounciness);
			SetComponentData(pc);
			SEMgr.Inst.dead_Bone.PlaySE();
			UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
			componentData.FlyUnregister();
			componentData.IsVelocityDeclice = true;
			if (Tool2D.IgnoreZDistance(Tool2D.GetNavMeshPointIngoreZ(base.transform.position, 8), base.transform.position) > 0.2f)
			{
				componentData.FallinAbyss(Tool2D.IgnoreZPoint(base.transform.position));
			}
			SetComponentData(componentData);
			break;
		}
		case "HeadFall":
			SEMgr.Inst.spell9002Land.PlaySE();
			break;
		case "AttackFinish":
			state = MonsterState.Idle;
			break;
		case "Attack":
		{
			attackParticle.Play();
			Vector3 dir = Tool2D.GetDir();
			GetNearestTarget();
			if (base.HaveTarget)
			{
				dir = Tool2D.GetDir(ToTargetDir(), spellAngleRange.RandomResult());
			}
			UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
			for (int i = 0; i < bulletCount; i++)
			{
				sSPModifier.Direction = Tool2D.GetDir(dir, bulletSpreadRange * (1f / (float)bulletCount * (float)i - 0.5f));
				sSPModifier.SpawnPosition = base.transform.position + new Vector3(Mathf.Sign(tsf_Model.localScale.x) * Mathf.Abs((attackParticle.transform.position - base.transform.position).x), 0f, 0f) + new Vector3(0f, 0f, 0f - spellHeight);
				sSPModifier.ApplyToSSP(ref ssp);
				ShootSpell(ssp);
			}
			SEMgr.Inst.spell1001Shoot.PlaySE();
			break;
		}
		}
	}
}
