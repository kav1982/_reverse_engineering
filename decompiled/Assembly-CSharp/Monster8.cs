using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class Monster8 : UnitBase
{
	private enum MonsterState
	{
		BornIdle,
		Idle,
		IdleWalk,
		Amaze,
		RunToTarget
	}

	public VariableFloat idleTime;

	public VariableFloat idleWalkRadius;

	public VariableFloat idleWalkTime;

	public float followDistance;

	public float shoutRadius;

	public float amazeTime;

	public AIPattern pattern;

	private MonsterState state;

	private float idleTimer;

	private float idleWalkTimer;

	private float amazeTimer;

	private bool isNPC7Pause;

	private Vector3 pauseBeforeMotion;

	[Header("和谐")]
	public List<Sprite> normalSprites;

	public List<Sprite> harmonySprites;

	public MeshRenderer MR;

	private Sprite nowSprite;

	public static List<Monster8> mates = new List<Monster8>();

	public override void EveryInitialCallback()
	{
		base.Anima.SetTrigger("Idle");
		state = MonsterState.BornIdle;
		idleTimer = 0f;
		idleWalkTimer = 0f;
		amazeTimer = 0f;
		isNPC7Pause = false;
		mates.Add(this);
	}

	public override void Update()
	{
		base.Update();
		if (base.IsLocked || isNPC7Pause)
		{
			return;
		}
		switch (state)
		{
		case MonsterState.BornIdle:
			SetMove(Vector3.zero, isFlip: false);
			bornIdleTimer += Time.deltaTime;
			if (bornIdleTimer >= 0.5f)
			{
				state = MonsterState.Idle;
				idleTime.RandomResult();
			}
			break;
		case MonsterState.Idle:
			SetMove(Vector3.zero, isFlip: false);
			idleTimer += Time.deltaTime;
			if (idleTimer >= idleTime.result)
			{
				idleTimer = 0f;
				state = MonsterState.IdleWalk;
				if (pattern == AIPattern.Pattern1)
				{
					base.Anima.SetTrigger("Walk");
				}
				else
				{
					base.Anima.SetTrigger("Walk1");
				}
				GetNavInfo(Tool2D.GetNavMeshPoint(base.transform.position, idleWalkRadius));
				idleWalkTime.RandomResult();
			}
			CheckTarget();
			break;
		case MonsterState.IdleWalk:
			idleWalkTimer += Time.deltaTime;
			if (idleWalkTimer >= idleWalkTime.result)
			{
				idleWalkTimer = 0f;
				state = MonsterState.Idle;
				base.Anima.SetTrigger("Idle");
				idleTime.RandomResult();
			}
			else if (navInfo.allCornerArrived)
			{
				GetNavInfo(Tool2D.GetNavMeshPoint(base.transform.position, idleWalkRadius));
			}
			else
			{
				SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed);
				CheckNavInfo();
			}
			CheckTarget();
			break;
		case MonsterState.Amaze:
			SetMove(Vector3.zero, isFlip: false);
			if (base.HaveTarget)
			{
				SetFlip(ToTargetDir().x);
			}
			amazeTimer += Time.deltaTime;
			if (amazeTimer > amazeTime)
			{
				amazeTimer = 0f;
				state = MonsterState.RunToTarget;
				if (pattern == AIPattern.Pattern1)
				{
					base.Anima.SetTrigger("Walk");
				}
				else
				{
					base.Anima.SetTrigger("Walk1");
				}
				if (base.HaveTarget)
				{
					WarningOthers(targetEntity, base.transform.position, shoutRadius);
				}
			}
			break;
		case MonsterState.RunToTarget:
			checkTargetIntervalTimer += Time.deltaTime;
			if (checkTargetIntervalTimer >= 1f)
			{
				checkTargetIntervalTimer = 0f;
				GetNearestTarget();
			}
			if (!base.HaveTarget)
			{
				GetNearestTarget();
				if (!base.HaveTarget || ToTargetDistanceSqr() > followDistance * followDistance)
				{
					state = MonsterState.Idle;
					base.Anima.SetTrigger("Idle");
				}
			}
			else
			{
				GetNavInfo(base.TargetPoint);
				if (ToTargetDistanceSqr() > 0.040000003f)
				{
					SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed);
				}
				else
				{
					SetMove(Vector3.zero, isFlip: false);
				}
			}
			break;
		default:
			Debug.LogError(state);
			break;
		}
	}

	private void CheckTarget()
	{
		checkTargetIntervalTimer += Time.deltaTime;
		if (checkTargetIntervalTimer >= 1f)
		{
			checkTargetIntervalTimer = 0f;
			GetNearestTarget();
			if (base.HaveTarget && ToTargetDistanceSqr() < followDistance * followDistance)
			{
				state = MonsterState.Amaze;
				base.Anima.SetTrigger("Amaze");
			}
		}
	}

	public override void AnimaAction(string animaName)
	{
		switch (animaName)
		{
		case "0":
			MR.material.SetTexture(GameConstManaged.shaderTextureIndex, normalSprites[0].texture);
			break;
		case "1":
			MR.material.SetTexture(GameConstManaged.shaderTextureIndex, normalSprites[1].texture);
			break;
		case "2":
			MR.material.SetTexture(GameConstManaged.shaderTextureIndex, normalSprites[2].texture);
			break;
		case "3":
			if (normalSprites.Count <= 3)
			{
				MR.material.SetTexture(GameConstManaged.shaderTextureIndex, normalSprites[2].texture);
			}
			else
			{
				MR.material.SetTexture(GameConstManaged.shaderTextureIndex, normalSprites[3].texture);
			}
			break;
		}
	}

	public override void AfterDead(ref TakeDamageInfo_Dots info)
	{
		base.AfterDead(ref info);
		if (mates.Contains(this))
		{
			mates.Remove(this);
		}
		if (pattern == AIPattern.Pattern2)
		{
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Monster8_HealFog", base.transform.position, 4f).GetComponent<Monster8_HealFog>().owner = myPpt;
			SEMgr.Inst.monster8Explode.PlaySE();
		}
		if (pattern == AIPattern.Pattern3)
		{
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Monster8_HealFogLarge", base.transform.position, 4f).GetComponent<Monster8_HealFog>().owner = myPpt;
			SEMgr.Inst.monster8Explode.PlaySE();
		}
		if (pattern == AIPattern.Pattern4)
		{
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Monster8_HealFog2", base.transform.position, 6f).GetComponent<Monster8_HealFog>().owner = myPpt;
			SEMgr.Inst.monster8Explode.PlaySE();
		}
		if (pattern == AIPattern.Pattern5)
		{
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Monster8_HealFogLarge2", base.transform.position, 6f).GetComponent<Monster8_HealFog>().owner = myPpt;
			SEMgr.Inst.monster8Explode.PlaySE();
		}
	}

	public override void AfterTakeDamage_Dots(ref TakeDamageInfo_Dots info)
	{
		if (EntityIsValid(info.attackerEntity) && (state == MonsterState.Idle || state == MonsterState.IdleWalk))
		{
			UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>(info.attackerEntity);
			if (componentData.unitCfg.unitType == UnitType.Player || componentData.unitCfg.unitType == UnitType.Teammate || componentData.unitCfg.unitType == UnitType.TeammateNotAttack)
			{
				targetEntity = info.attackerEntity;
				state = MonsterState.Amaze;
				base.Anima.SetTrigger("Amaze");
			}
		}
	}

	public static void WarningOthers(Entity targetEntity, Vector3 position, float shoutRadius)
	{
		for (int num = mates.Count - 1; num >= 0; num--)
		{
			if (mates[num] == null || !mates[num].gameObject.activeInHierarchy)
			{
				mates.RemoveAt(num);
			}
			else if ((position - mates[num].transform.position).sqrMagnitude < shoutRadius * shoutRadius)
			{
				mates[num].Warned(targetEntity);
			}
		}
	}

	public void Warned(Entity targetEntity)
	{
		if (state == MonsterState.Idle || state == MonsterState.IdleWalk)
		{
			base.targetEntity = targetEntity;
			state = MonsterState.Amaze;
			base.Anima.SetTrigger("Amaze");
		}
	}

	public void NPC7Pause()
	{
		if (!isNPC7Pause)
		{
			isNPC7Pause = true;
			pauseBeforeMotion = base.CurrentMotion;
			base.CurrentMotion = Vector3.zero;
		}
	}

	public void NPC7PuaseRecovery()
	{
		if (isNPC7Pause)
		{
			isNPC7Pause = false;
			base.CurrentMotion = pauseBeforeMotion;
		}
	}
}
