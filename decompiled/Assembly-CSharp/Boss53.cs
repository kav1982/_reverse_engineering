using System;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Physics.Stateful;
using UnityEngine;

public class Boss53 : UnitBase, IDotsCollisionReceiver, IDotsPhysicsReciever
{
	public enum MonsterState
	{
		Idle,
		MoveToTarget,
		NormalAttack,
		DropHead
	}

	[Serializable]
	public class Cooldown
	{
		public float CooldownTime;

		[HideInInspector]
		public float Timer;

		public bool Ready => Timer >= CooldownTime;

		public Cooldown(float cooldownTime)
		{
			CooldownTime = cooldownTime;
		}

		public bool Update()
		{
			Timer += Time.deltaTime;
			if (Ready)
			{
				Timer = 0f;
				return true;
			}
			return false;
		}

		public void Reset()
		{
			Timer = 0f;
		}
	}

	public static Boss53 Inst;

	public Transform flipTransform;

	public Transform headPoint;

	public Boss53NormalAttackLineController normalAttackLineController;

	public Cooldown attackCooldown = new Cooldown(2f);

	[Header("普通攻击（小闪电攻击）")]
	public float NormalAttackWeight = 1f;

	public float NormalAttackEndlag = 0.5f;

	public float NormalAttackDamage = 25f;

	public float NormalAttackRange = 2.5f;

	[Header("丢头")]
	public float DropHeadWeight = 1f;

	public StateVariableMgr varMgr = new StateVariableMgr();

	public MonsterState _state;

	private bool stateQuit;

	private bool changedState;

	private float stateExistTime;

	private float idleDelay;

	private Boss53_Head head;

	public Entity thisEntity { get; set; }

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

	public override void EveryInitialCallback()
	{
		Inst = this;
		state = MonsterState.Idle;
		head = ObjPoolMgr.Inst.GetGO("Prefabs/Units/505301_Head").GetComponent<Boss53_Head>();
		head.Boss = this;
	}

	public override void SingleInitialCallback()
	{
		base.SingleInitialCallback();
		Boss53NormalAttackLineController boss53NormalAttackLineController = normalAttackLineController;
		boss53NormalAttackLineController.OnAttack = (Action<Vector3>)Delegate.Combine(boss53NormalAttackLineController.OnAttack, new Action<Vector3>(OnNormalAttackLineAttack));
		Boss53NormalAttackLineController boss53NormalAttackLineController2 = normalAttackLineController;
		boss53NormalAttackLineController2.OnFinish = (Action)Delegate.Combine(boss53NormalAttackLineController2.OnFinish, new Action(OnNormalAttackLineFinish));
	}

	private void OnDisable()
	{
		ObjPoolMgr.Inst.RecycleGO(head.gameObject);
	}

	public override void Update()
	{
		base.Update();
		myPpt.Anima.SetBool("move", base.CurrentMotion.sqrMagnitude > 0.1f);
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
		case MonsterState.Idle:
			SetMove(Vector3.zero);
			idleDelay -= Time.deltaTime;
			if (!(idleDelay > 0f))
			{
				if (!base.HaveTarget)
				{
					GetNearestTargetPlayerFirst();
				}
				if (base.HaveTarget)
				{
					state = MonsterState.MoveToTarget;
				}
			}
			break;
		case MonsterState.MoveToTarget:
			if (changedState)
			{
				attackCooldown.Reset();
			}
			GetNavInfo(base.TargetPoint);
			if (!navInfo.allCornerArrived)
			{
				SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed);
			}
			else
			{
				SetMove(Vector3.zero);
			}
			CheckNavInfo();
			LookTarget();
			if (navInfo.allCornerArrived)
			{
				state = MonsterState.Idle;
			}
			if (attackCooldown.Update())
			{
				state = MonsterState.NormalAttack;
			}
			break;
		case MonsterState.NormalAttack:
			SetMove(Vector3.zero);
			LookTarget();
			if (base.HaveTarget)
			{
				if (changedState)
				{
					normalAttackLineController.StartLookTarget(targetEntity);
				}
			}
			else
			{
				state = MonsterState.Idle;
			}
			break;
		case MonsterState.DropHead:
			SetMove(Vector3.zero);
			if (changedState)
			{
				if (head.State == Boss53_Head.HeadState.Droped)
				{
					head.Back();
					state = MonsterState.Idle;
				}
				else if (head.State == Boss53_Head.HeadState.Normal)
				{
					base.Anima.SetTrigger("dropHead");
				}
			}
			break;
		default:
			throw new ArgumentOutOfRangeException();
		}
	}

	private void OnNormalAttackLineAttack(Vector3 position)
	{
		EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
		List<UnitDotsSyncSystem.DistanceHitResult> list = new List<UnitDotsSyncSystem.DistanceHitResult>();
		UnitDotsSyncSystem.GetCollidersInRange(position, NormalAttackRange, GameConst.Filter_MonsterAoe, list);
		foreach (UnitDotsSyncSystem.DistanceHitResult item in list)
		{
			if (entityManager.HasComponent<TakeDamageInfo_Dots>(item.entity))
			{
				TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(myPpt.myEntity);
				info.damage = NormalAttackDamage;
				UnitDotsSyncSystem.AddTakeDamageRequest(item.entity, info);
			}
		}
	}

	private void LookTarget()
	{
		if (base.HaveTarget)
		{
			float num = base.TargetPoint.x - base.transform.position.x;
			Vector3 localScale = flipTransform.localScale;
			if (num > 0.01f)
			{
				localScale.x = 1f;
			}
			else if (num < -0.01f)
			{
				localScale.x = -1f;
			}
			flipTransform.localScale = localScale;
		}
	}

	private void OnNormalAttackLineFinish()
	{
		state = MonsterState.Idle;
		idleDelay = NormalAttackEndlag;
	}

	public override void AnimaAction(string animaName)
	{
		if (animaName == "DropHead")
		{
			head.Drop();
			state = MonsterState.Idle;
		}
	}

	public void OnCollisionEnter_Dots(StatefulCollisionEvent collision)
	{
	}

	public void OnCollisionStay_Dots(StatefulCollisionEvent collision)
	{
	}

	public void OnCollisionExit_Dots(StatefulCollisionEvent collision)
	{
	}
}
