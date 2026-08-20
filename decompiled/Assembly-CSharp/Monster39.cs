using System.Collections.Generic;
using Unity.Physics;
using UnityEngine;

public class Monster39 : UnitBase
{
	public enum MonsterState
	{
		BornIdle,
		Idle,
		Attack,
		RandomMove,
		Chase,
		PrepareSpin,
		Spin,
		StopSpin
	}

	private StateVariableMgr varMgr = new StateVariableMgr();

	public MonsterState _state;

	private bool stateQuit;

	private bool changedState;

	private float stateExistTime;

	public float idleTime;

	[Header("行动")]
	public VariableFloat randomMoveRadius;

	public float chaseDistance;

	public float loseTraceDistance;

	public float attackDistance;

	public float minChaseTime;

	[Header("子弹")]
	public float spellSpeed;

	public float spellDuration;

	public int spellDamage;

	public float spellAmplitude;

	public float spellFrequency;

	public float spellHeight;

	public float spellCount;

	[Header("剑")]
	public bool fakeFlipped;

	public Monster39_Sword sword;

	public Transform tsf_sprite;

	public bool showSword;

	public Transform tsf_ChestPoint;

	public Transform tsf_ChestPoint1;

	public float dashSpeed;

	public PhysicsMaterial PM_dash;

	public PhysicsMaterial PM_Common;

	[Header("影子")]
	public Transform tsf_body;

	public Transform tsf_Shadow;

	[Header("旋转！")]
	public VariableFloat SpinCDTime;

	private float SpinCDTimer;

	public float SpinTime;

	public float spinSpeedFix;

	private SpellInitialParameter sipBullet = new SpellInitialParameter();

	public Vector3 spinDir;

	[Header("方向替换试作")]
	public List<SpriteRenderer> directionalSR;

	public List<Sprite> directionalSpriteFront;

	public List<Sprite> directionalSpriteBack;

	private bool canSlashDirational;

	public SpriteRenderer thigh;

	public SpriteRenderer thigh1;

	public SpriteRenderer leg;

	public SpriteRenderer leg1;

	private Vector3 dashDir;

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

	public bool swordLocked => base.IsLocked;

	public override void SingleInitialCallback()
	{
		sword = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Monster39_Sword", base.transform.position, base.transform).GetComponent<Monster39_Sword>();
	}

	public unsafe override void EveryInitialCallback()
	{
		state = MonsterState.BornIdle;
		sword.gameObject.SetActive(value: true);
		sword.Initialize(this);
		sword.Update();
		showSword = false;
		base.CC_Self.material = PM_Common;
		PhysicsCollider componentData = GetComponentData<PhysicsCollider>();
		componentData.ColliderPtr->SetRestitution(PM_dash.bounciness);
		SetComponentData(componentData);
		SpinCDTime.RandomResult();
		SpinCDTimer = Random.Range(0f, SpinCDTime.RandomResult() / 2f);
		SetDirationalSprite(Vector3.down);
	}

	public void SetFakeFlip(Vector3 dir)
	{
		if (dir.x >= 0f)
		{
			tsf_sprite.localScale = new Vector3(1f, 1f, 1f);
			tsf_sprite.localPosition = new Vector3(0f - Mathf.Abs(tsf_sprite.localPosition.x), 0f, 0f);
			fakeFlipped = false;
		}
		else
		{
			tsf_sprite.localScale = new Vector3(-1f, 1f, 1f);
			fakeFlipped = true;
			tsf_sprite.localPosition = new Vector3(Mathf.Abs(tsf_sprite.localPosition.x), 0f, 0f);
		}
	}

	public void SetDirationalSprite(Vector3 dir, bool isSlash = false)
	{
		if (isSlash)
		{
			if (sword.state != Monster39_Sword.SwordState.Slash && sword.state != Monster39_Sword.SwordState.BeforeSlash)
			{
				return;
			}
			if (dir.y < 0f && dir.x * tsf_sprite.localScale.x <= 0f && sword.nowAngle / sword.slashAngle < 0.5f)
			{
				for (int i = 0; i < directionalSR.Count; i++)
				{
					directionalSR[i].sprite = directionalSpriteBack[i];
				}
				thigh.sortingOrder = -1;
				leg.sortingOrder = -2;
				thigh1.sortingOrder = 1;
				leg1.sortingOrder = 0;
				return;
			}
			if (dir.y > 0f && dir.x * tsf_sprite.localScale.x <= 0f && sword.nowAngle / sword.slashAngle > 0.5f)
			{
				for (int j = 0; j < directionalSR.Count; j++)
				{
					directionalSR[j].sprite = directionalSpriteFront[j];
				}
				thigh.sortingOrder = 0;
				leg.sortingOrder = 1;
				thigh1.sortingOrder = -2;
				leg1.sortingOrder = -1;
				return;
			}
		}
		if (dir.y > 0f)
		{
			for (int k = 0; k < directionalSR.Count; k++)
			{
				directionalSR[k].sprite = directionalSpriteBack[k];
			}
			thigh.sortingOrder = -1;
			leg.sortingOrder = -2;
			thigh1.sortingOrder = 1;
			leg1.sortingOrder = 0;
		}
		else
		{
			for (int l = 0; l < directionalSR.Count; l++)
			{
				directionalSR[l].sprite = directionalSpriteFront[l];
			}
			thigh.sortingOrder = 0;
			leg.sortingOrder = 1;
			thigh1.sortingOrder = -2;
			leg1.sortingOrder = -1;
		}
	}

	public unsafe override void Update()
	{
		tsf_Shadow.position = Tool2D.GetLayerPoint(new Vector3((tsf_body.position.x - base.transform.position.x) / 2f + base.transform.position.x, base.transform.position.y, 0f), LayerCorrectType.Shadow);
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
		SpinCDTimer += Time.deltaTime;
		switch (state)
		{
		case MonsterState.BornIdle:
			if (changedState)
			{
				base.Anima.Play("Idle", 0, 0f);
				bornIdleTimer = 0f;
			}
			SetMove(Vector3.zero, isFlip: false);
			bornIdleTimer += Time.deltaTime;
			if (bornIdleTimer > 0.5f)
			{
				state = MonsterState.RandomMove;
			}
			break;
		case MonsterState.RandomMove:
			if (changedState)
			{
				base.Anima.Play("Move");
				randomMoveRadius.RandomResult();
				GetNavInfo(Tool2D.GetNavMeshPoint(base.transform.position, randomMoveRadius.result));
			}
			if (navInfo.allCornerArrived)
			{
				state = MonsterState.Idle;
				break;
			}
			SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed, isFlip: false);
			SetFakeFlip(ToPointDir(navInfo.ToGoPoint));
			CheckNavInfo();
			checkTargetIntervalTimer += Time.deltaTime;
			if (checkTargetIntervalTimer >= 1f)
			{
				checkTargetIntervalTimer = 0f;
				GetNearestTarget(checkWall: true);
			}
			if (base.HaveTarget && ToTargetDistanceSqr() < chaseDistance * chaseDistance)
			{
				state = MonsterState.Chase;
			}
			break;
		case MonsterState.Idle:
		{
			ref float reference = ref varMgr.RegFloat(0);
			if (changedState)
			{
				base.Anima.Play("Idle");
			}
			reference += Time.deltaTime;
			if (reference > idleTime)
			{
				state = MonsterState.RandomMove;
				break;
			}
			SetDirationalSprite(Vector3.down);
			SetMove(Vector3.zero, isFlip: false);
			checkTargetIntervalTimer += Time.deltaTime;
			if (checkTargetIntervalTimer >= 1f)
			{
				checkTargetIntervalTimer = 0f;
				GetNearestTarget(checkWall: true);
			}
			if (base.HaveTarget && ToTargetDistanceSqr() < chaseDistance * chaseDistance)
			{
				state = MonsterState.Chase;
			}
			break;
		}
		case MonsterState.Chase:
			if (changedState)
			{
				base.Anima.Play("Move");
				if (!base.HaveTarget || ToTargetDistanceSqr() > loseTraceDistance * loseTraceDistance)
				{
					GetNearestTarget(checkWall: true);
				}
				if (!base.HaveTarget || ToTargetDistanceSqr() > loseTraceDistance * loseTraceDistance)
				{
					state = MonsterState.Idle;
					break;
				}
				GetNavInfo(base.TargetPoint);
			}
			if (!base.HaveTarget || ToTargetDistanceSqr() > loseTraceDistance * loseTraceDistance)
			{
				GetNearestTarget(checkWall: true);
			}
			if (!base.HaveTarget || ToTargetDistanceSqr() > loseTraceDistance * loseTraceDistance)
			{
				state = MonsterState.Idle;
				break;
			}
			GetNavInfo(base.TargetPoint);
			SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed);
			SetFakeFlip(ToPointDir(navInfo.ToGoPoint));
			if (SpinCDTimer > SpinCDTime.result)
			{
				SpinCDTimer = 0f;
				SpinCDTime.RandomResult();
				state = MonsterState.PrepareSpin;
			}
			else if (ToTargetDistanceSqr() < attackDistance * attackDistance && stateExistTime > minChaseTime)
			{
				state = MonsterState.Attack;
			}
			break;
		case MonsterState.Attack:
			if (changedState)
			{
				base.CC_Self.material = PM_dash;
				PhysicsCollider componentData4 = GetComponentData<PhysicsCollider>();
				componentData4.ColliderPtr->SetRestitution(PM_dash.bounciness);
				SetComponentData(componentData4);
				canSlashDirational = false;
				showSword = true;
				base.Anima.Play("Attack");
				if (base.HaveTarget)
				{
					SetFakeFlip(ToTargetDir());
				}
			}
			if (canSlashDirational)
			{
				SetDirationalSprite(sword.towards, isSlash: true);
			}
			SetMove(Vector3.zero, isFlip: false);
			break;
		case MonsterState.PrepareSpin:
			if (changedState)
			{
				base.Anima.Play("PrepareSpin");
				Vector3 vector = Tool2D.GetDir();
				GetNearestTarget();
				if (base.HaveTarget)
				{
					vector = ToTargetDir();
				}
				SetFakeFlip(vector);
				sword.SpinPrepare(vector);
				showSword = true;
			}
			SetDirationalSprite(sword.towards);
			SetMove(Vector3.zero, isFlip: false);
			break;
		case MonsterState.Spin:
		{
			if (changedState)
			{
				base.Anima.Play("Spin");
				sword.SpinStart();
				UnitProperty_Dots componentData2 = GetComponentData<UnitProperty_Dots>();
				componentData2.IsVelocityDeclice = false;
				componentData2.ImmuneKnockbackRegister();
				SetComponentData(componentData2);
				GetNearestTarget();
				if (base.HaveTarget)
				{
					base.Rigid.linearVelocity = ToTargetDir() * base.MoveSpeed * spinSpeedFix;
				}
				else
				{
					base.Rigid.linearVelocity = Tool2D.GetDir() * base.MoveSpeed * spinSpeedFix;
				}
			}
			if (base.Rigid.linearVelocity.sqrMagnitude < base.MoveSpeed * spinSpeedFix * (base.MoveSpeed * spinSpeedFix) * 0.9f)
			{
				base.Rigid.linearVelocity = base.Rigid.linearVelocity.normalized * base.MoveSpeed * spinSpeedFix;
			}
			PhysicsVelocity componentData3 = GetComponentData<PhysicsVelocity>();
			componentData3.Linear = base.Rigid.linearVelocity;
			SetComponentData(componentData3);
			SetFakeFlip(Tool2D.GetDir(sword.towards, 30f * (float)((!sword.isClockWise) ? 1 : (-1))));
			SetDirationalSprite(Tool2D.GetDir(sword.towards, 30f * (float)((!sword.isClockWise) ? 1 : (-1))));
			if (stateExistTime > SpinTime)
			{
				state = MonsterState.StopSpin;
			}
			break;
		}
		case MonsterState.StopSpin:
			if (changedState)
			{
				UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
				componentData.IsVelocityDeclice = true;
				componentData.ImmuneKnockbackUnregister();
				SetComponentData(componentData);
				base.Anima.Play("StopSpin");
				sword.SpinStop();
			}
			SetMove(Vector3.zero, isFlip: false);
			SetDirationalSprite(sword.towards);
			break;
		}
	}

	public void ShootBullet(Vector3 dir, Vector3 position)
	{
		sipBullet.spelldataConfig.speed = 7f;
		sipBullet.shootDirection = dir;
		ObjPoolMgr.Inst.GetGO("Prefabs/Spell/" + sipBullet.spelldataConfig.prefab, position + new Vector3(0f, 0f, 0f - spellHeight)).GetComponent<SpellBase>().Initialize(sipBullet);
	}

	public override void AfterDead(ref TakeDamageInfo_Dots info)
	{
		base.AfterDead(ref info);
	}

	public unsafe override void AnimaAction(string animaName)
	{
		switch (animaName)
		{
		case "Attack":
			if (base.HaveTarget)
			{
				dashDir = ToTargetDir();
				SetFakeFlip(dashDir);
				sword.SlashAt(dashDir);
				canSlashDirational = true;
			}
			break;
		case "AttackDash":
		{
			UnitProperty_Dots componentData2 = GetComponentData<UnitProperty_Dots>();
			componentData2.TakeKnockback(dashDir * dashSpeed);
			SetComponentData(componentData2);
			break;
		}
		case "PrepareSpinFinish":
			state = MonsterState.Spin;
			break;
		case "AttackSwordBack":
			sword.SwordRecycle();
			break;
		case "AttackFinish":
		{
			state = MonsterState.Chase;
			base.CC_Self.material = PM_Common;
			PhysicsCollider componentData = GetComponentData<PhysicsCollider>();
			componentData.ColliderPtr->SetRestitution(PM_Common.bounciness);
			SetComponentData(componentData);
			break;
		}
		}
	}
}
