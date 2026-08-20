using System;
using System.Collections.Generic;
using UnityEngine;

public class Monster39_Drone : UnitBase
{
	public enum MonsterState
	{
		Rest,
		Out,
		Aim,
		Attack,
		Back
	}

	private float rotateAngle;

	public float rotateSpeed;

	public float rotateDistance;

	public float recycleRange;

	private Vector3 targetDelta;

	public VariableFloat targetOffset;

	public Monster39 master;

	public bool prepared;

	[Header("Aim and Attack")]
	public float aimTime;

	public VariableFloat aimOffset;

	private Vector3 aimDiration;

	public VariableFloat aimBeforeTime;

	private float aimBeforeTimer;

	public VariableFloat aimDelay;

	private float aimDelayTimer;

	private bool aimed;

	private float aimTimer;

	public float attackRange;

	public float attackDelay;

	private float attackTimer;

	public LayerMask laserCheckLayer;

	public LineRenderer lr_aim;

	private Vector3 aimPoint;

	[Header("Spell")]
	public int spellDamage;

	public float spellSpeed;

	public float spellDuration;

	public float spellHeight;

	private Vector3 headDiration;

	public float headRotateSpeed;

	public Transform headTransform;

	public Transform tsf_Motion;

	public List<Monster39_Tentacle> tentacles = new List<Monster39_Tentacle>();

	public MonsterState state;

	private MonsterState preState;

	private MonsterState tempState;

	private bool changedState;

	private SpellInitialParameter sipBullet = new SpellInitialParameter();

	public VariableFloat aroundPointDistance;

	public float aroundAdjustmentDistance;

	private void Start()
	{
	}

	public override void SingleInitialCallback()
	{
		sipBullet.spelldataConfig = SpellConfig.GetConfigCopy(10041);
		sipBullet.spelldataConfig.speed = spellSpeed;
		sipBullet.spelldataConfig.duration = spellDuration;
		sipBullet.spelldataConfig.damage = spellDamage;
		sipBullet.ownerPpt = myPpt;
		aimOffset.RandomResult();
		for (int i = 0; i < tentacles.Count; i++)
		{
			tentacles[i].Initialize(this, ((float)i - ((float)tentacles.Count - 1f) / 2f) * 15f);
		}
	}

	public override void EveryInitialCallback()
	{
		lr_aim.SetPosition(0, base.transform.position);
		lr_aim.SetPosition(1, base.transform.position);
		rotateAngle = UnityEngine.Random.Range(0, 6);
		targetOffset.RandomResult();
		state = MonsterState.Rest;
		aroundPointDistance.RandomResult();
	}

	public override void AfterDead(ref TakeDamageInfo_Dots info)
	{
		base.AfterDead(ref info);
	}

	private Vector3 GetMotion(Vector3 targetPosition)
	{
		Vector3 result = ToPointDir(targetPosition, 90f) * base.MoveSpeed;
		float num = Vector3.Distance(base.transform.position, targetPosition);
		if (Mathf.Abs(num - aroundPointDistance.result) > aroundAdjustmentDistance)
		{
			if (num < aroundPointDistance.result)
			{
				result += -ToPointDir(targetPosition) * base.MoveSpeed;
			}
			else
			{
				result += ToPointDir(targetPosition) * base.MoveSpeed;
			}
		}
		return result;
	}

	public Vector3 GetRotatePoint()
	{
		return master.transform.position + Tool2D.GetDir(rotateAngle) * rotateDistance;
	}

	public override void Update()
	{
		base.Update();
		if (base.IsLocked)
		{
			return;
		}
		changedState = false;
		preState = tempState;
		tempState = state;
		if (preState != state)
		{
			changedState = true;
		}
		if (master == null && master.myPpt.AlreadyDead)
		{
			return;
		}
		rotateAngle += Time.deltaTime * rotateSpeed;
		if (state != MonsterState.Aim && state != MonsterState.Attack)
		{
			headDiration = base.CurrentMotion;
		}
		headTransform.up = Vector3.RotateTowards(headTransform.up, headDiration, headRotateSpeed * (MathF.PI / 180f) * Time.deltaTime, 0f);
		switch (state)
		{
		case MonsterState.Rest:
			if (changedState)
			{
				aroundPointDistance.RandomResult();
				aimBeforeTimer = 0f;
				aimBeforeTime.RandomResult();
			}
			GetNavInfo(GetRotatePoint());
			SetMove(GetMotion(master.transform.position));
			break;
		case MonsterState.Out:
			if (changedState)
			{
				GetNearestTarget();
				targetOffset.RandomResult();
				targetDelta = Tool2D.GetDir() * targetOffset.result;
			}
			if (targetPpt == null)
			{
				GetNearestTarget();
				if (targetPpt == null)
				{
					state = MonsterState.Back;
				}
				else
				{
					targetOffset.RandomResult();
					targetDelta = Tool2D.GetDir() * targetOffset.result;
				}
			}
			GetNavInfo(base.TargetPoint + targetDelta);
			SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed);
			if (ToTargetDistanceSqr() < attackRange * attackRange)
			{
				aimBeforeTimer += 1f;
				if (aimBeforeTimer > aimBeforeTime.result)
				{
					state = MonsterState.Aim;
				}
			}
			break;
		case MonsterState.Aim:
			if (changedState)
			{
				aimTimer = 0f;
				aimDelayTimer = 0f;
				aimDelay.RandomResult();
				aimed = false;
			}
			SetMove(Vector3.zero);
			if (!base.HaveTarget)
			{
				state = MonsterState.Out;
			}
			aimDelayTimer += Time.deltaTime;
			if (aimDelayTimer > aimDelay.result && !aimed)
			{
				aimed = true;
				Vector3 vector = base.transform.position + new Vector3(0f, 0f, 0f - spellHeight);
				RaycastHit hitInfo;
				Vector3 vector2 = (aimPoint = ((!Physics.Raycast(vector, base.TargetPoint - base.transform.position + Tool2D.GetDir() * aimOffset.RandomResult(), out hitInfo, 100f, laserCheckLayer)) ? (vector + ToTargetDir() * 100f) : hitInfo.point));
				aimDiration = Tool2D.IgnoreZPoint(vector2 - vector).normalized;
				lr_aim.SetPosition(1, Tool2D.GetLayerPoint(vector));
				lr_aim.SetPosition(0, Tool2D.GetLayerPoint(vector2));
			}
			headDiration = ToTargetDir();
			if (aimed)
			{
				headDiration = aimDiration;
				RaycastHit hitInfo2;
				Vector3 vector3 = ((!Physics.Raycast(base.transform.position, aimDiration, out hitInfo2, 100f, laserCheckLayer)) ? (base.transform.position + (aimPoint - base.transform.position) * 100f) : hitInfo2.point);
				lr_aim.SetPosition(1, Tool2D.GetLayerPoint(base.transform.position + new Vector3(0f, 0f, 0f - spellHeight)));
				lr_aim.SetPosition(0, Tool2D.GetLayerPoint(vector3 + new Vector3(0f, 0f, 0f - spellHeight)));
				aimTimer += Time.deltaTime;
				if (aimTimer > aimTime)
				{
					lr_aim.SetPosition(0, base.transform.position);
					lr_aim.SetPosition(1, base.transform.position);
					state = MonsterState.Attack;
				}
			}
			break;
		case MonsterState.Attack:
			if (changedState)
			{
				sipBullet.shootDirection = aimDiration;
				ObjPoolMgr.Inst.GetGO("Prefabs/Spell/" + sipBullet.spelldataConfig.prefab, base.transform.position + new Vector3(0f, 0f, 0f - spellHeight)).GetComponent<SpellBase>().Initialize(sipBullet);
				attackTimer = 0f;
			}
			SetMove(Vector3.zero);
			attackTimer += Time.deltaTime;
			if (attackTimer > attackDelay)
			{
				state = MonsterState.Back;
			}
			break;
		case MonsterState.Back:
			GetNavInfo(master.transform.position);
			SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed);
			if ((base.transform.position - master.transform.position).sqrMagnitude <= recycleRange * recycleRange)
			{
				state = MonsterState.Rest;
			}
			break;
		}
	}

	public override void AnimaAction(string animaName)
	{
	}
}
