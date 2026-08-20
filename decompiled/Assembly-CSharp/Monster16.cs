using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class Monster16 : UnitBase
{
	private enum MonsterState
	{
		BornIdle,
		Idle,
		MoveRandom,
		Gravity
	}

	[Space(50f)]
	public AIPattern pattern;

	public VariableFloat idleTime;

	public VariableFloat moveRandomRadius;

	[Header("Rock")]
	public Transform[] tsf_Rocks;

	public float rockDistance;

	public float rockRotateSpeed;

	[Header("Pattern3,4")]
	public float gravityRange;

	public float gravityPush;

	public VariableFloat gravityTime;

	public float gravityTimer;

	private float rockAngle;

	private MonsterState state;

	private float idleTimer;

	public override void EveryInitialCallback()
	{
		state = MonsterState.BornIdle;
		idleTimer = 0f;
		idleTime.RandomResult();
		base.Anima.SetTrigger("Idle");
	}

	public override void Update()
	{
		base.Update();
		if (base.IsLocked)
		{
			return;
		}
		rockAngle += rockRotateSpeed * Time.deltaTime;
		for (int i = 0; i < tsf_Rocks.Length; i++)
		{
			Vector3 rootPoint = base.transform.position + Tool2D.GetDir((float)(360 / tsf_Rocks.Length * i) + rockAngle) * rockDistance;
			tsf_Rocks[i].position = Tool2D.GetLayerPoint(rootPoint);
		}
		switch (state)
		{
		case MonsterState.BornIdle:
			SetMove(Vector3.zero, isFlip: false);
			bornIdleTimer += Time.deltaTime;
			if (bornIdleTimer >= 0.5f)
			{
				state = MonsterState.Idle;
				gravityTime.RandomResult();
			}
			break;
		case MonsterState.Idle:
			SetMove(Vector3.zero, isFlip: false);
			idleTimer += Time.deltaTime;
			if (idleTimer >= idleTime.result)
			{
				idleTimer = 0f;
				idleTime.RandomResult();
				state = MonsterState.MoveRandom;
				base.Anima.SetTrigger("Move");
				GetNavInfo(Tool2D.GetNavMeshPoint(base.transform.position, moveRandomRadius));
			}
			break;
		case MonsterState.MoveRandom:
			if (navInfo.allCornerArrived)
			{
				state = MonsterState.Idle;
				base.Anima.SetTrigger("Idle");
			}
			else
			{
				SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed);
				CheckNavInfo();
			}
			break;
		case MonsterState.Gravity:
			SetMove(Vector3.zero);
			break;
		default:
			Debug.LogError("!");
			break;
		}
	}

	private void Gravity()
	{
		ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Monster16_Gravity", base.transform.position, 3f);
		List<Collider> collidersByTag = GeneralTool.GetCollidersByTag(base.transform.position, gravityRange, "Player", "Teammate");
		for (int i = 0; i < collidersByTag.Count; i++)
		{
			Vector3 vector = Tool2D.IgnoreZV2ToV1Normal(collidersByTag[i].transform.position, base.transform.position) * (0f - gravityPush);
			collidersByTag[i].GetComponent<UnitProperty>().TakeKnockback(vector * collidersByTag[i].GetComponent<UnitProperty>().unitCfg.knockbackRatio);
		}
	}

	public override void AnimaAction(string animaName)
	{
		if (!(animaName == "Gravity"))
		{
			if (animaName == "GravityDone")
			{
				base.Anima.SetTrigger("Idle");
				state = MonsterState.Idle;
			}
		}
		else
		{
			Gravity();
		}
	}

	public override void AfterDead(ref TakeDamageInfo_Dots info)
	{
		if (pattern == AIPattern.Pattern3)
		{
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Monster16_Explosion", base.transform.position, 5f).GetComponent<Monster16_Explosion>().Initialize();
		}
		if (pattern == AIPattern.Pattern4)
		{
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Monster16_ExplosionLarge", base.transform.position, 5f).GetComponent<Monster16_Explosion>().Initialize();
		}
		base.AfterDead(ref info);
	}

	public override void AfterTakeDamage_Dots(ref TakeDamageInfo_Dots info)
	{
		if (info.spell.Entity != Entity.Null)
		{
			idleTimer = 0f;
			state = MonsterState.Idle;
			base.Anima.SetTrigger("Idle");
			if ((pattern == AIPattern.Pattern2 || pattern == AIPattern.Pattern4) && EntityIsValid(info.attackerEntity))
			{
				UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>(info.attackerEntity);
				componentData.TakeKnockback(-info.knockbackForce * myPpt.unitCfg.knockbackRatio);
				SetComponentData(componentData, info.attackerEntity);
			}
		}
	}
}
