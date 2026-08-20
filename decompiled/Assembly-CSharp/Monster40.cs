using System;
using System.Collections.Generic;
using UnityEngine;

public class Monster40 : UnitBase
{
	public enum MonsterState
	{
		BornIdle,
		Idle,
		GetIntoRange,
		FarFormTarget,
		AroundMove,
		AroundMoveFake,
		Attack,
		RandomMove
	}

	public MonsterState _state;

	public StateVariableMgr varMgr = new StateVariableMgr();

	private bool stateQuit;

	private bool changedState;

	private float stateExistTime;

	private Vector3 roomCenterPoint;

	private float roomWidth;

	private float roomHeight;

	[Header("action")]
	public AIPattern pattern;

	public VariableFloat keepDistance;

	public VariableFloat moveDirRotation;

	public float rotateRight;

	private float moveTimer;

	public VariableFloat MoveTime;

	private float IdleTimer;

	public float IdleTime;

	private float attackTimer;

	public VariableFloat attackTime;

	public float rotateChance;

	private Vector3 randomMovePoint;

	[Header("body")]
	public Vector3 faceDir;

	public Vector3 faceDirFixed;

	public float faceRotateSpeed;

	public Transform tsf_Motion;

	public Transform tsf_TailRoot;

	public Transform tsf_TailRotator;

	public Transform bulletLauncher;

	public bool IsMove;

	public GameObject pfb_Leg;

	public GameObject pfb_Tail;

	public GameObject head;

	public GameObject body;

	private Vector3 bodyOriginPos;

	public GameObject tailRoot;

	public float legOffset;

	public float legAngle;

	public float bodyShakeOffset;

	public float bodyShakeRange;

	public float bodyShakeSpeed;

	private float bodyShakeTimer;

	public List<Monster40_Leg> legs = new List<Monster40_Leg>();

	private Monster40_Tail tail;

	public VariableFloat rotateRandomFixer;

	[Header("attack")]
	public VariableFloat spellSpeed;

	public float spellDuration;

	public VariableInt spellDamage;

	public float spellHeight;

	public VariableFloat spellAngle;

	public int bulletCount;

	public float attackKnockBack;

	public float attackSpeedFixer;

	public float attackChangeDirInterval;

	private float attackChangeDirTimer;

	private int attackCount;

	public int maxAttackCount;

	[Header("和谐模式")]
	public SpriteRenderer sr_body;

	public Sprite sprite_H;

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

	public override void SingleInitialCallback()
	{
		ssp = UnitDotsSyncSystem.GetSpellPrototype(90171);
		UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
		sSPModifier.Duration = spellDuration;
		sSPModifier.Shooter = myPpt.myEntity;
		sSPModifier.ApplyToSSP(ref ssp);
		tail = UnityEngine.Object.Instantiate(pfb_Tail, base.transform).GetComponent<Monster40_Tail>();
		tail.Initialize(this, 0f);
		for (int i = 0; i < 3; i++)
		{
			Monster40_Leg component = UnityEngine.Object.Instantiate(pfb_Leg, base.transform).GetComponent<Monster40_Leg>();
			component.Initialize(this, 90f - legAngle + legAngle * (float)i);
			legs.Add(component);
		}
		for (int j = 0; j < 3; j++)
		{
			Monster40_Leg component2 = UnityEngine.Object.Instantiate(pfb_Leg, base.transform).GetComponent<Monster40_Leg>();
			component2.Initialize(this, 270f - legAngle + legAngle * (float)j);
			legs.Add(component2);
		}
		if (GameMgr.IsHarmony_Static)
		{
			sr_body.sprite = sprite_H;
		}
		if (GameMgr.IsMobile_Static)
		{
			maxAttackCount = Mathf.CeilToInt((float)maxAttackCount * 0.66f);
		}
	}

	public override void EveryInitialCallback()
	{
		if (LevelMgr.Inst.CurrentRoomCtrller.roomCfg.themeType == RoomThemeType.Theme6_Chapter3 || LevelMgr.Inst.CurrentRoomCtrller.roomCfg.themeType == RoomThemeType.Theme22_Chapter3_Shortcut1)
		{
			roomCenterPoint = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint;
			roomWidth = LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme6Width;
			roomHeight = LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme6Height;
		}
		if (pattern == AIPattern.Pattern1)
		{
			base.Anima.Play("Monster40_Idle");
		}
		else
		{
			base.Anima.Play("Monster40_Idle2");
		}
		for (int i = 0; i < legs.Count; i++)
		{
			legs[i].EveryInitial();
		}
		tail.EveryInitial();
		state = MonsterState.BornIdle;
		bornIdleTimer = 0f;
		faceDir = Vector3.down;
		attackTime.RandomResult();
		attackTimer = UnityEngine.Random.Range(0f, attackTime.result);
	}

	public bool IsNearBorder()
	{
		if (!(roomHeight / 2f - Mathf.Abs(base.transform.position.y - roomCenterPoint.y) < 0.5f))
		{
			return roomWidth / 2f - Mathf.Abs(base.transform.position.x - roomCenterPoint.x) < 0.5f;
		}
		return true;
	}

	public void AttackCheck()
	{
		attackTimer += Time.deltaTime;
		if (attackTimer > attackTime.result)
		{
			attackTime.RandomResult();
			attackTimer = 0f;
			GetNearestTargetPlayerFirst();
			if (base.HaveTarget)
			{
				state = MonsterState.Attack;
			}
		}
	}

	public override void Update()
	{
		base.Update();
		if (base.IsLocked)
		{
			return;
		}
		if (base.HaveTarget)
		{
			faceDir = Tool2D.IgnoreZPoint(Vector3.RotateTowards(faceDir, ToTargetDir(), MathF.PI / 180f * faceRotateSpeed * Time.deltaTime, 0f)).normalized;
			faceDirFixed = Tool2D.GetDir(Tool2D.GetDegree(-faceDir) + bodyShakeOffset);
			body.transform.up = Tool2D.GetDir(Tool2D.GetDegree(-faceDir) + bodyShakeOffset);
			tailRoot.transform.localEulerAngles = new Vector3(0f, 0f, bodyShakeOffset);
			tsf_TailRotator.localEulerAngles = new Vector3(0f, 0f, bodyShakeOffset);
		}
		else
		{
			faceDir = Tool2D.IgnoreZPoint(Vector3.RotateTowards(faceDir, ToPointDir(randomMovePoint), MathF.PI / 180f * faceRotateSpeed * Time.deltaTime, 0f)).normalized;
			faceDirFixed = Tool2D.GetDir(Tool2D.GetDegree(-faceDir) + bodyShakeOffset);
			body.transform.up = Tool2D.GetDir(Tool2D.GetDegree(-faceDir) + bodyShakeOffset);
			tailRoot.transform.localEulerAngles = new Vector3(0f, 0f, bodyShakeOffset);
			tsf_TailRotator.localEulerAngles = new Vector3(0f, 0f, bodyShakeOffset);
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
				bodyShakeOffset = 0f;
				if (pattern == AIPattern.Pattern1)
				{
					base.Anima.Play("Monster40_Idle");
				}
				else
				{
					base.Anima.Play("Monster40_Idle2");
				}
				IsMove = false;
				bornIdleTimer = 0f;
			}
			bornIdleTimer += Time.deltaTime;
			if (bornIdleTimer > 0.5f)
			{
				state = MonsterState.GetIntoRange;
			}
			break;
		case MonsterState.RandomMove:
			if (changedState)
			{
				randomMovePoint = Tool2D.GetNavMeshPoint(roomCenterPoint + new Vector3(UnityEngine.Random.Range(-0.5f, 0.5f) * roomWidth, UnityEngine.Random.Range(-0.5f, 0.5f) * roomHeight, 0f));
				GetNavInfo(randomMovePoint);
			}
			CheckNavInfo();
			if (navInfo.allCornerArrived)
			{
				state = MonsterState.Idle;
			}
			else
			{
				SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed);
			}
			break;
		case MonsterState.GetIntoRange:
			if (changedState)
			{
				bodyShakeOffset = 0f;
				if (pattern == AIPattern.Pattern1)
				{
					base.Anima.Play("Monster40_Idle");
				}
				else
				{
					base.Anima.Play("Monster40_Idle2");
				}
				IsMove = true;
				keepDistance.RandomResult();
				rotateRight = ((!(UnityEngine.Random.Range(0f, 1f) > 0.5f)) ? 1 : (-1));
				moveDirRotation.RandomResult();
				moveTimer = 0f;
				MoveTime.RandomResult();
			}
			bodyShakeTimer += Time.deltaTime * bodyShakeSpeed;
			bodyShakeOffset = bodyShakeRange * Mathf.Sin(bodyShakeTimer * MathF.PI * 2f);
			moveTimer += Time.deltaTime;
			if (moveTimer > MoveTime.result)
			{
				if (rotateChance < UnityEngine.Random.Range(0f, 1f))
				{
					moveTimer = 0f;
					MoveTime.RandomResult();
					rotateRight = 0f - rotateRight;
				}
				else
				{
					state = MonsterState.Idle;
				}
			}
			if (!base.HaveTarget)
			{
				GetNearestTargetPlayerFirst();
			}
			if (!base.HaveTarget)
			{
				state = MonsterState.RandomMove;
				break;
			}
			if ((base.transform.position - base.TargetPointIgnoreZ).sqrMagnitude < keepDistance.result * keepDistance.result)
			{
				state = MonsterState.AroundMove;
			}
			if (!IsNearBorder())
			{
				Vector3 navMeshPoint = Tool2D.GetNavMeshPoint(base.transform.position + faceDir);
				SetMove(ToPointDir(navMeshPoint) * base.MoveSpeed);
			}
			else
			{
				Vector3 navMeshPoint2 = Tool2D.GetNavMeshPoint(base.transform.position - faceDir);
				SetMove(ToPointDir(navMeshPoint2) * base.MoveSpeed);
			}
			AttackCheck();
			break;
		case MonsterState.FarFormTarget:
			if (changedState)
			{
				bodyShakeTimer = 0f;
				bodyShakeOffset = 0f;
				if (pattern == AIPattern.Pattern1)
				{
					base.Anima.Play("Monster40_Idle");
				}
				else
				{
					base.Anima.Play("Monster40_Idle2");
				}
				IsMove = true;
				keepDistance.RandomResult();
				rotateRight = ((!(UnityEngine.Random.Range(0f, 1f) > 0.5f)) ? 1 : (-1));
				moveDirRotation.RandomResult();
			}
			bodyShakeTimer += Time.deltaTime * bodyShakeSpeed;
			bodyShakeOffset = bodyShakeRange * Mathf.Sin(bodyShakeTimer * MathF.PI * 2f);
			if (!base.HaveTarget)
			{
				GetNearestTargetPlayerFirst();
			}
			if (!base.HaveTarget)
			{
				state = MonsterState.RandomMove;
				break;
			}
			if ((base.transform.position - base.TargetPointIgnoreZ).sqrMagnitude > keepDistance.result * keepDistance.result)
			{
				state = MonsterState.AroundMove;
			}
			if (!IsNearBorder())
			{
				Vector3 navMeshPoint4 = Tool2D.GetNavMeshPoint(base.transform.position - faceDir);
				SetMove(ToPointDir(navMeshPoint4) * base.MoveSpeed);
			}
			else
			{
				Vector3 navMeshPoint5 = Tool2D.GetNavMeshPoint(base.transform.position + faceDir);
				SetMove(ToPointDir(navMeshPoint5) * base.MoveSpeed);
			}
			AttackCheck();
			break;
		case MonsterState.AroundMove:
		{
			if (changedState)
			{
				bodyShakeOffset = 0f;
				if (pattern == AIPattern.Pattern1)
				{
					base.Anima.Play("Monster40_Idle");
				}
				else
				{
					base.Anima.Play("Monster40_Idle2");
				}
				rotateRandomFixer.RandomResult();
				IsMove = true;
				rotateRight = ((!(UnityEngine.Random.Range(0f, 1f) > 0.5f)) ? 1 : (-1));
				MoveTime.RandomResult();
				moveTimer = 0f;
			}
			moveTimer += Time.deltaTime;
			if (moveTimer > MoveTime.result)
			{
				if (rotateChance > UnityEngine.Random.Range(0f, 1f))
				{
					moveTimer = 0f;
					MoveTime.RandomResult();
					rotateRandomFixer.RandomResult();
					rotateRight = 0f - rotateRight;
				}
				else
				{
					state = MonsterState.Idle;
				}
				break;
			}
			if (!base.HaveTarget)
			{
				GetNearestTargetPlayerFirst();
			}
			if (!base.HaveTarget)
			{
				state = MonsterState.RandomMove;
				break;
			}
			Vector3 navMeshPoint3 = Tool2D.GetNavMeshPoint(base.transform.position + Tool2D.GetDir(ToTargetDir(), 90f * rotateRight + rotateRandomFixer.result));
			SetMove(ToPointDir(navMeshPoint3) * base.MoveSpeed);
			if ((base.transform.position - base.TargetPointIgnoreZ).sqrMagnitude < keepDistance.value1 * keepDistance.value1)
			{
				state = MonsterState.FarFormTarget;
			}
			else if ((base.transform.position - base.TargetPointIgnoreZ).sqrMagnitude > keepDistance.value2 * keepDistance.value2)
			{
				state = MonsterState.GetIntoRange;
			}
			else
			{
				AttackCheck();
			}
			break;
		}
		case MonsterState.Idle:
			if (changedState)
			{
				bodyShakeOffset = 0f;
				if (pattern == AIPattern.Pattern1)
				{
					base.Anima.Play("Monster40_Idle");
				}
				else
				{
					base.Anima.Play("Monster40_Idle2");
				}
				IsMove = false;
				IdleTimer = 0f;
			}
			IdleTimer += Time.deltaTime;
			if (IdleTimer > IdleTime)
			{
				state = MonsterState.AroundMove;
			}
			SetMove(Vector3.zero);
			AttackCheck();
			break;
		case MonsterState.Attack:
			if (changedState)
			{
				IsMove = true;
				attackCount = 0;
				if (pattern == AIPattern.Pattern1)
				{
					base.Anima.Play("Monster40_Attack");
				}
				else
				{
					base.Anima.Play("Monster40_Attack2");
				}
				attackChangeDirTimer = 0f;
			}
			attackChangeDirTimer += Time.deltaTime;
			if (attackChangeDirTimer > attackChangeDirInterval)
			{
				attackChangeDirTimer = 0f;
				if ((double)UnityEngine.Random.Range(0f, 1f) < 0.5)
				{
					rotateRight = 0f - rotateRight;
				}
			}
			if (base.HaveTarget)
			{
				SetMove(ToPointDir(GetMotion(base.TargetPoint)) * base.MoveSpeed * attackSpeedFixer);
				break;
			}
			GetNearestTargetPlayerFirst();
			SetMove(Vector3.zero);
			break;
		case MonsterState.AroundMoveFake:
			break;
		}
	}

	private Vector3 GetMotion(Vector3 targetPosition)
	{
		Vector3 vector = ToPointDir(targetPosition, 90f * rotateRight) * base.MoveSpeed * attackSpeedFixer;
		float num = Vector3.Distance(base.transform.position, targetPosition);
		if (Mathf.Abs(num - keepDistance.result) > 2f)
		{
			vector = ((!(num < keepDistance.result)) ? ToPointDir(targetPosition, 45f * rotateRight) : ToPointDir(targetPosition, 135f * rotateRight));
		}
		return Tool2D.GetNavMeshPoint(base.transform.position + vector);
	}

	public override void Theme6Reposition(Vector3 changeValue)
	{
		base.Theme6Reposition(changeValue);
		for (int i = 0; i < legs.Count; i++)
		{
			legs[i].Theme6Reposition(changeValue);
		}
		tail.Theme6Reposition(changeValue);
	}

	public override void AnimaAction(string animaName)
	{
		switch (animaName)
		{
		case "Shoot":
		{
			attackCount++;
			UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
			for (int i = 0; i < bulletCount; i++)
			{
				sSPModifier.Speed = spellSpeed.RandomResult();
				sSPModifier.Direction = Tool2D.GetDir(Tool2D.IgnoreZPoint(faceDir), spellAngle.RandomResult());
				sSPModifier.Damage = spellSpeed.RandomResult();
				sSPModifier.SpawnPosition = bulletLauncher.position - new Vector3(0f, tsf_Motion.localPosition.y, 0f) + new Vector3(0f, 0f, 0f - spellHeight);
				sSPModifier.ApplyToSSP(ref ssp);
				ShootSpell(ssp);
			}
			myPpt.TakeKnockback(-faceDir * attackKnockBack);
			break;
		}
		case "AttackFinish":
			if (attackCount >= maxAttackCount)
			{
				state = MonsterState.AroundMove;
			}
			break;
		case "PrepareDone":
			base.Anima.Play("Monster40_Attack");
			break;
		}
	}
}
