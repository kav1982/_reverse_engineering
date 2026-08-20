using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class Monster45 : UnitBase
{
	public enum MonsterState
	{
		BornIdle,
		Walk,
		Attack,
		Egg,
		dying
	}

	private enum attackType
	{
		egg,
		fog
	}

	public float eggFlyTime;

	public Vector3 EggOffset;

	public Vector3 eyeLeftOffset;

	public Vector3 eyeRightOffset;

	private Monster45_2 eyeLeft;

	private Monster45_2 eyeRight;

	public Collider MonsterCollider;

	public Transform center;

	public int SummonIDEye = 104502;

	public int SummonIDEgg = 104603;

	public Animator MonsterAnimator;

	public Animator EyeAnimator;

	public MeshRenderer meshEyeLeft;

	public MeshRenderer meshEyeRight;

	public GameObject ParticleDead;

	public GameObject ParticleLeftEyeDead;

	public GameObject ParticleRightEyeDead;

	public float beforeDieTime;

	public VariableFloat attackInterval;

	public AIPattern pattern;

	private Vector3 noTargetPoint;

	public StateVariableMgr varMgr = new StateVariableMgr();

	public MonsterState _state;

	private bool stateQuit;

	private bool changedState;

	private float stateExistTime;

	private attackType thisAttack;

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

	public override void Update()
	{
		base.Update();
		if (!myPpt.AlreadyDead)
		{
			SyncChildPosition();
			CheckChildDeath();
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
				base.Anima.Play("Idle");
			}
			if (stateExistTime >= 0.5f)
			{
				state = MonsterState.Walk;
			}
			break;
		case MonsterState.Walk:
			if (changedState)
			{
				GetNearestTargetPlayerFirst();
				attackInterval.RandomResult();
				base.Anima.Play("Idle");
			}
			checkTargetIntervalTimer += Time.deltaTime;
			if (checkTargetIntervalTimer > 1f)
			{
				GetNearestTargetPlayerFirst();
			}
			if (base.HaveTarget)
			{
				GetNavInfo(base.TargetPoint);
				Vector3 dir3 = Tool2D.GetDir(ToPointDir(navInfo.ToGoPoint), 45f);
				SetMove(dir3 * base.MoveSpeed);
			}
			else if ((base.transform.position - noTargetPoint).sqrMagnitude < 1f)
			{
				Vector3 centerPoint2 = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint;
				Vector3 vector2 = LevelMgr.Inst.CurrentRoomCtrller.RoomScale * 0.25f;
				noTargetPoint = new Vector3((float)Random.Range(-1, 1) * vector2.x, (float)Random.Range(-1, 1) * vector2.y, 0f) + centerPoint2;
			}
			else
			{
				GetNavInfo(noTargetPoint);
				Vector3 dir4 = Tool2D.GetDir(ToPointDir(navInfo.ToGoPoint), 45f);
				SetMove(dir4 * base.MoveSpeed);
			}
			if (!(stateExistTime >= attackInterval.result))
			{
				break;
			}
			if (pattern == AIPattern.Pattern2)
			{
				if (Random.Range(0, 2) == 0)
				{
					state = MonsterState.Attack;
				}
				else
				{
					state = MonsterState.Egg;
				}
			}
			else
			{
				state = MonsterState.Attack;
			}
			break;
		case MonsterState.Attack:
			if (changedState)
			{
				MonsterAnimator.Play("Attack");
			}
			if (base.HaveTarget)
			{
				GetNavInfo(base.TargetPoint);
				Vector3 dir = Tool2D.GetDir(ToPointDir(navInfo.ToGoPoint), 45f);
				SetMove(dir * base.MoveSpeed);
			}
			else if ((base.transform.position - noTargetPoint).sqrMagnitude < 1f)
			{
				Vector3 centerPoint = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint;
				Vector3 vector = LevelMgr.Inst.CurrentRoomCtrller.RoomScale * 0.25f;
				noTargetPoint = new Vector3((float)Random.Range(-1, 1) * vector.x, (float)Random.Range(-1, 1) * vector.y, 0f) + centerPoint;
			}
			else
			{
				GetNavInfo(noTargetPoint);
				Vector3 dir2 = Tool2D.GetDir(ToPointDir(navInfo.ToGoPoint), 45f);
				SetMove(dir2 * base.MoveSpeed);
			}
			break;
		case MonsterState.Egg:
			if (changedState)
			{
				MonsterAnimator.Play("AttackEgg");
			}
			if (base.HaveTarget)
			{
				GetNavInfo(base.TargetPoint);
				Vector3 dir5 = Tool2D.GetDir(ToPointDir(navInfo.ToGoPoint), 45f);
				SetMove(dir5 * base.MoveSpeed);
			}
			else if ((base.transform.position - noTargetPoint).sqrMagnitude < 1f)
			{
				Vector3 centerPoint3 = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint;
				Vector3 vector3 = LevelMgr.Inst.CurrentRoomCtrller.RoomScale * 0.25f;
				noTargetPoint = new Vector3((float)Random.Range(-1, 1) * vector3.x, (float)Random.Range(-1, 1) * vector3.y, 0f) + centerPoint3;
			}
			else
			{
				GetNavInfo(noTargetPoint);
				Vector3 dir6 = Tool2D.GetDir(ToPointDir(navInfo.ToGoPoint), 45f);
				SetMove(dir6 * base.MoveSpeed);
			}
			break;
		case MonsterState.dying:
			if (changedState)
			{
				ParticleDead.SetActive(value: true);
				SEMgr.Inst.PlaySE("SE_Dead_Flesh5Long");
				MonsterAnimator.Play("Dying");
				myPpt.CanTouch = false;
				UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
				componentData.CanTouch = false;
				SetComponentData(componentData);
			}
			MonsterCollider.enabled = true;
			if (stateExistTime >= beforeDieTime)
			{
				DotsAnnouncedDeath();
			}
			break;
		default:
			Debug.LogError(state);
			break;
		}
		void CheckChildDeath()
		{
			if (!eyeLeft.gameObject.activeSelf && !eyeRight.gameObject.activeSelf && state != MonsterState.dying)
			{
				state = MonsterState.dying;
			}
		}
		void SyncChildPosition()
		{
			if (EntityIsValid(eyeLeft.myPpt.myEntity))
			{
				eyeLeft.transform.position = base.transform.position + eyeLeftOffset;
				LocalTransform componentData2 = eyeLeft.GetComponentData<LocalTransform>();
				componentData2.Position = eyeLeft.transform.position;
				eyeLeft.SetComponentData(componentData2);
			}
			if (EntityIsValid(eyeRight.myPpt.myEntity))
			{
				eyeRight.gameObject.transform.position = base.transform.position + eyeRightOffset;
				LocalTransform componentData3 = eyeRight.GetComponentData<LocalTransform>();
				componentData3.Position = eyeRight.transform.position;
				eyeRight.SetComponentData(componentData3);
			}
		}
	}

	public void EyeDead(bool left)
	{
		if (left)
		{
			ParticleRightEyeDead.SetActive(value: true);
			meshEyeLeft.enabled = false;
		}
		else
		{
			ParticleLeftEyeDead.SetActive(value: true);
			meshEyeRight.enabled = false;
		}
	}

	public void EyeBeenAttack(bool left)
	{
		if (left)
		{
			EyeAnimator.Play("Left", 0, 0f);
		}
		else
		{
			EyeAnimator.Play("Right", 0, 0f);
		}
	}

	public void Attack()
	{
		SEMgr.Inst.PlaySE("SE_Monster45_blast");
		ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Monster45_PoisonFog", base.transform.position);
	}

	public void AttackEgg()
	{
		SEMgr.Inst.PlaySE("SE_Monster15Land");
		GetNearestTarget();
		Monster45_Egg component = ObjPoolMgr.Inst.GetGO("Prefabs/Units/" + SummonIDEgg, base.transform.position + EggOffset).GetComponent<Monster45_Egg>();
		if (base.HaveTarget)
		{
			component.landPoint = Tool2D.GetNavMeshPointIngoreZ(Tool2D.IgnoreZPoint(base.TargetPoint));
			return;
		}
		Vector3 a = LevelMgr.Inst.CurrentRoomCtrller.RoomScale;
		Vector3 startPoint = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint + Vector3.Scale(a, new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0f));
		component.landPoint = Tool2D.IgnoreZPoint(Tool2D.GetNavMeshPoint(startPoint));
	}

	public override void EveryInitialCallback()
	{
		MonsterCollider.enabled = true;
		state = MonsterState.BornIdle;
		EveryInitialCallbackEye();
		Vector3 centerPoint = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint;
		Vector3 vector = LevelMgr.Inst.CurrentRoomCtrller.RoomScale * 0.25f;
		noTargetPoint = new Vector3((float)Random.Range(-1, 1) * vector.x, (float)Random.Range(-1, 1) * vector.y, 0f) + centerPoint;
	}

	private void EveryInitialCallbackEye()
	{
		meshEyeLeft.enabled = true;
		meshEyeRight.enabled = true;
		eyeLeft = ObjPoolMgr.Inst.GetGO("Prefabs/Units/" + SummonIDEye, base.transform.position).GetComponent<Monster45_2>();
		eyeLeft.transform.position = Tool2D.IgnoreZPoint(base.transform.position + eyeLeftOffset);
		eyeRight = ObjPoolMgr.Inst.GetGO("Prefabs/Units/" + SummonIDEye, base.transform.position).GetComponent<Monster45_2>();
		eyeRight.transform.position = Tool2D.IgnoreZPoint(base.transform.position + eyeRightOffset);
		eyeLeft.SetMother(this, left: true);
		eyeRight.SetMother(this, left: false);
	}

	public override void BeforeTakeDamage_Dots(ref TakeDamageInfo_Dots info)
	{
		info.immuneDamage = true;
		if (info.attackerEntity != Entity.Null && (info.attackerEntity == eyeLeft.myPpt.myEntity || info.attackerEntity == eyeRight.myPpt.myEntity))
		{
			info.immuneDamage = false;
		}
	}

	public override void AfterTakeDamage_Dots(ref TakeDamageInfo_Dots info)
	{
		myPpt.unitCfg.currentHP = 100000000f;
		UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
		componentData.unitCfg.currentHP = 100000000f;
		SetComponentData(componentData);
	}

	public override void AfterDead(ref TakeDamageInfo_Dots info)
	{
		ParticleLeftEyeDead.SetActive(value: false);
		ParticleRightEyeDead.SetActive(value: false);
		ParticleDead.SetActive(value: false);
		base.AfterDead(ref info);
		eyeRight.DotsAnnouncedDeath();
		eyeLeft.DotsAnnouncedDeath();
	}

	public override void AnimaAction(string animaName)
	{
		switch (animaName)
		{
		case "Egg":
			AttackEgg();
			break;
		case "Attack":
			Attack();
			break;
		case "AttackFinish":
			state = MonsterState.Walk;
			break;
		}
	}
}
