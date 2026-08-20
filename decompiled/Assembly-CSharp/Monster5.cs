using System;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class Monster5 : UnitBase
{
	public enum MonsterState
	{
		BornIdle,
		AroundSimilar,
		AroundObj,
		NoAround
	}

	public float aroundCheckInterval;

	public float aroundAdjustmentDistance;

	public float aroundPointDistance;

	public float noAroundRotateSpeed;

	[Header("Anima")]
	public MeshRenderer mr;

	public Sprite sprite_Attack;

	public Sprite sprite_Normal;

	[Header("Sprint")]
	public float sprintInterval;

	public float sprintTime;

	public float sprintSpeedRatio;

	public AIPattern pattern;

	[Header("Pattern2 Pattern3")]
	public VariableFloat attackInterval;

	public float attackDistance;

	[Header("Spell")]
	public float spellHeight;

	public float spellSpeed;

	public float spellDuration;

	private SpellSpawnParams ssp;

	[Header("AroundPlayer")]
	public bool isAroundPlayer;

	public float aroundPlayerRadius;

	public MonsterState state;

	private List<Monster5> monster5s = new List<Monster5>();

	private Entity aroundEntity;

	private Vector3 noAroundDir;

	private float aroundCheckIntervalTimer;

	private float attackIntervalTimer;

	private float sprintIntervalTimer;

	private float sprintTimer;

	private Vector3 sprintDir;

	private bool isSprint;

	public override void SingleInitialCallback()
	{
		ssp = UnitDotsSyncSystem.GetSpellPrototype(10011);
		UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
		sSPModifier.Speed = spellSpeed;
		sSPModifier.Duration = spellDuration;
		sSPModifier.Shooter = myPpt.myEntity;
		sSPModifier.ApplyToSSP(ref ssp);
		SetNavMeshArea(32);
	}

	public override void EveryInitialCallback()
	{
		attackInterval.RandomResult();
		base.Anima.SetTrigger("Idle");
		mr.material.SetTexture(GameConstManaged.shaderTextureIndex, sprite_Normal.texture);
		state = MonsterState.BornIdle;
		monster5s.Clear();
		aroundEntity = Entity.Null;
		aroundCheckIntervalTimer = 0f;
		attackIntervalTimer = 0f;
		sprintIntervalTimer = 0f;
		sprintTimer = 0f;
		isSprint = false;
	}

	public override void Update()
	{
		base.Update();
		if (base.IsLocked)
		{
			return;
		}
		aroundCheckIntervalTimer += Time.deltaTime;
		if (aroundCheckIntervalTimer >= aroundCheckInterval && EntityIsValid(aroundEntity))
		{
			aroundCheckIntervalTimer = 0f;
			CheckAround();
		}
		if (pattern == AIPattern.Pattern2 || pattern == AIPattern.Pattern3 || pattern == AIPattern.Pattern4 || pattern == AIPattern.Pattern5)
		{
			attackIntervalTimer += Time.deltaTime;
			if (attackIntervalTimer > attackInterval.result)
			{
				attackIntervalTimer = 0f;
				attackInterval.RandomResult();
				GetNearestTarget(checkWall: true);
				if (base.HaveTarget)
				{
					if (pattern == AIPattern.Pattern2 || pattern == AIPattern.Pattern3)
					{
						base.Anima.SetTrigger("Attack");
					}
					else
					{
						base.Anima.SetTrigger("Attack2");
					}
				}
			}
		}
		switch (state)
		{
		case MonsterState.BornIdle:
			bornIdleTimer += Time.deltaTime;
			if (bornIdleTimer >= 0.5f)
			{
				CheckAround();
			}
			break;
		case MonsterState.AroundSimilar:
		{
			if (isSprint)
			{
				SetMove(Tool2D.GetDir(sprintDir, UnityEngine.Random.Range(-60f, 60f)) * base.MoveSpeed * sprintSpeedRatio);
				sprintTimer += Time.deltaTime;
				if (!(sprintTimer > sprintTime))
				{
					break;
				}
				sprintTimer = 0f;
				for (int i = 0; i < monster5s.Count; i++)
				{
					if (monster5s[i].gameObject.activeSelf)
					{
						monster5s[i].SprintStop();
					}
				}
				break;
			}
			Vector3 zero = Vector3.zero;
			int num = 0;
			for (int j = 0; j < monster5s.Count; j++)
			{
				if (monster5s[j].gameObject.activeSelf)
				{
					num++;
					zero += monster5s[j].transform.position;
				}
			}
			zero /= (float)num;
			if (num == 1)
			{
				state = MonsterState.NoAround;
				noAroundDir = Tool2D.GetDir();
				break;
			}
			SetMove(GetMotion(zero));
			sprintIntervalTimer += Time.deltaTime;
			if (!(sprintIntervalTimer >= sprintInterval))
			{
				break;
			}
			sprintIntervalTimer = 0f;
			sprintDir = Tool2D.IgnoreZV2ToV1Normal(PlayerMgr.Inst.PlayerPoint, zero);
			for (int k = 0; k < monster5s.Count; k++)
			{
				if (monster5s[k].gameObject.activeSelf)
				{
					monster5s[k].SprintStart(sprintDir);
				}
			}
			break;
		}
		case MonsterState.AroundObj:
			if (!EntityIsValid(aroundEntity))
			{
				CheckAround();
			}
			else
			{
				SetMove(GetMotion(GetComponentData<LocalTransform>(aroundEntity).Position));
			}
			break;
		case MonsterState.NoAround:
			noAroundDir = Tool2D.GetDir(noAroundDir, (0f - noAroundRotateSpeed) * Time.deltaTime);
			Debug.DrawLine(base.transform.position, base.transform.position + noAroundDir, Color.red, 0.1f);
			SetMove(noAroundDir * base.MoveSpeed);
			break;
		default:
			Debug.LogError(state);
			break;
		}
	}

	private void CheckAround()
	{
		monster5s.Clear();
		aroundEntity = Entity.Null;
		if (isAroundPlayer)
		{
			if (PlayerMgr.Inst.PlayerCtrller.IsVisible)
			{
				aroundEntity = PlayerMgr.Inst.PlayerEtt;
			}
			else
			{
				aroundEntity = LevelMgr.Inst.CurrentRoomCtrller.GetNearestTargetableEntity(base.transform.position);
			}
			if (aroundEntity != Entity.Null)
			{
				state = MonsterState.AroundObj;
				return;
			}
			state = MonsterState.NoAround;
			noAroundDir = Tool2D.GetDir();
			return;
		}
		List<Entity> targetableEttList = LevelMgr.Inst.CurrentRoomCtrller.targetableEttList;
		if (targetableEttList.Count > 1)
		{
			for (int i = 0; i < targetableEttList.Count; i++)
			{
				UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>(targetableEttList[i]);
				if (componentData.id - 100500 > 0 && componentData.id - 100500 < 100)
				{
					monster5s.Add(GetComponentObject<UnitPptReference>(targetableEttList[i]).unitPpt.GetComponent<Monster5>());
				}
				else if (!EntityIsValid(aroundEntity))
				{
					aroundEntity = targetableEttList[i];
				}
				else if ((base.transform.position - (Vector3)GetComponentData<LocalTransform>(targetableEttList[i]).Position).sqrMagnitude < (base.transform.position - (Vector3)GetComponentData<LocalTransform>(aroundEntity).Position).sqrMagnitude)
				{
					aroundEntity = targetableEttList[i];
				}
			}
			if (EntityIsValid(aroundEntity))
			{
				state = MonsterState.AroundObj;
			}
			else
			{
				state = MonsterState.AroundSimilar;
			}
		}
		else if (state != MonsterState.NoAround)
		{
			state = MonsterState.NoAround;
			noAroundDir = Tool2D.GetDir();
		}
	}

	private Vector3 GetMotion(Vector3 targetPosition)
	{
		Vector3 vector = ToPointDir(targetPosition, 90f) * base.MoveSpeed;
		float num = Vector3.Distance(base.transform.position, targetPosition);
		bool flag = false;
		float num2 = (isAroundPlayer ? aroundPlayerRadius : aroundPointDistance);
		if (Mathf.Abs(num - num2) > aroundAdjustmentDistance)
		{
			flag = true;
			if (num < num2)
			{
				vector += -ToPointDir(targetPosition) * base.MoveSpeed;
			}
			else
			{
				vector += ToPointDir(targetPosition) * base.MoveSpeed;
			}
		}
		float num3 = 360f * base.MoveSpeed / (MathF.PI * 2f * num2);
		if (flag)
		{
			num3 *= 1.414f;
		}
		if (UnitDotsSyncSystem.Raycast(targetPosition, Tool2D.GetDir(base.transform.position - targetPosition, (0f - num3) * ((num < num2) ? 0.5f : 0.1f)), Mathf.Max(num2, num), GameConst.Filter_Border, out var result))
		{
			Debug.DrawLine(targetPosition, result.point);
			GetNavInfo(result.point);
			return ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed * (flag ? 1.414f : 1f);
		}
		return ToPointDir(Tool2D.GetNavMeshPoint(base.transform.position + vector * 0.1f, 8)) * base.MoveSpeed * (flag ? 1.414f : 1f);
	}

	public void SprintStart(Vector3 sprintDir)
	{
		sprintIntervalTimer = 0f;
		isSprint = true;
		this.sprintDir = sprintDir;
	}

	public void SprintStop()
	{
		sprintTimer = 0f;
		isSprint = false;
	}

	public override void AnimaAction(string animaName)
	{
		if (!(animaName == "Attack"))
		{
			if (animaName == "AttackFinish")
			{
				mr.material.SetTexture(GameConstManaged.shaderTextureIndex, sprite_Normal.texture);
				base.Anima.SetTrigger("Idle");
			}
			else
			{
				Debug.LogError(animaName);
			}
		}
		else if (base.HaveTarget)
		{
			mr.material.SetTexture(GameConstManaged.shaderTextureIndex, sprite_Attack.texture);
			UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
			if (pattern == AIPattern.Pattern2 || pattern == AIPattern.Pattern4)
			{
				sSPModifier.SpawnPosition = base.transform.position + new Vector3(0f, 0f, 0f - spellHeight);
				sSPModifier.Direction = ToTargetDir();
				sSPModifier.ApplyToSSP(ref ssp);
				ShootSpell(ssp);
			}
			else if (pattern == AIPattern.Pattern3 || pattern == AIPattern.Pattern5)
			{
				sSPModifier.SpawnPosition = base.transform.position + new Vector3(0f, 0f, 0f - spellHeight);
				sSPModifier.Direction = ToTargetDir(13f);
				sSPModifier.ApplyToSSP(ref ssp);
				ShootSpell(ssp);
				sSPModifier.Direction = ToTargetDir(-13f);
				sSPModifier.ApplyToSSP(ref ssp);
				ShootSpell(ssp);
			}
		}
	}
}
