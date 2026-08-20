using UnityEngine;

public class Monster10 : UnitBase
{
	private enum MonsterState
	{
		BornIdle,
		MoveRandom,
		Attack,
		MoveAroundTarget
	}

	[Space(50f)]
	public Monster10_Leg leg;

	public VariableFloat randomWalkRadius;

	public VariableFloat attackInterval;

	public float attackDistance = 6f;

	public AIPattern pattern;

	[Header("Pattern2 Pattern3 Pattern4")]
	public float liquidInteravl;

	public float liquidRadius;

	public float liquidTime;

	[Header("Bullet Prabola")]
	public VariableInt bulletCount;

	public float bulletLandRadius;

	public float spellHeight;

	public VariableFloat spellUpForce;

	public float spellGravity;

	public int spellDamage;

	public VariableFloat spellSpeed;

	public bool spellCanRebounce;

	private SpellSpawnParams ssp;

	private MonsterState state;

	private float attackIntervalTimer;

	private float liquidInteravlTimer;

	private Vector3 lastRecordPoint;

	public override void SingleInitialCallback()
	{
		ssp = UnitDotsSyncSystem.GetSpellPrototype(90191);
		UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
		sSPModifier.Damage = spellDamage;
		sSPModifier.Shooter = myPpt.myEntity;
		sSPModifier.Gravity = 0f - spellGravity;
		if (spellCanRebounce)
		{
			sSPModifier.ReboundCount = 1;
		}
		sSPModifier.ApplyToSSP(ref ssp);
		switch (pattern)
		{
		case AIPattern.Pattern1:
			ssp.ConfigComponentData.ColorType = SpellColorType.Monster;
			break;
		case AIPattern.Pattern2:
			ssp.ConfigComponentData.ColorType = SpellColorType.Mucus;
			ssp.ElementComponentData.MucusMoveSpeedRatio = 0.6f;
			ssp.ElementComponentData.MucusSpellSpeedRatio = 0.7f;
			ssp.ElementComponentData.MucusDuration = 3f;
			break;
		case AIPattern.Pattern3:
			ssp.ConfigComponentData.ColorType = SpellColorType.Venom;
			ssp.ElementComponentData.VenomApplyCount = 2f;
			ssp.ElementComponentData.VenomDuration = 3f;
			break;
		default:
			Debug.LogError(pattern);
			break;
		case AIPattern.Pattern4:
			break;
		}
	}

	public override void EveryInitialCallback()
	{
		state = MonsterState.BornIdle;
		attackIntervalTimer = 0f;
		liquidInteravlTimer = 0f;
		lastRecordPoint = Tool2D.IgnoreZPoint(base.transform);
		base.Anima.SetTrigger("Idle");
		attackInterval.RandomResult();
		switch (pattern)
		{
		case AIPattern.Pattern2:
			MucusSystem.CreateMucus(Tool2D.IgnoreZPoint(base.transform), liquidRadius);
			break;
		case AIPattern.Pattern3:
			VenomSystem.CreateVenom(Tool2D.IgnoreZPoint(base.transform), liquidRadius, liquidTime);
			break;
		case AIPattern.Pattern4:
			MucusSystem.CreateMucus(Tool2D.IgnoreZPoint(base.transform), liquidRadius);
			break;
		default:
			Debug.LogError(pattern);
			break;
		case AIPattern.Pattern1:
			break;
		}
	}

	public override void Update()
	{
		base.Update();
		if (base.IsLocked)
		{
			return;
		}
		switch (state)
		{
		case MonsterState.BornIdle:
			leg.SetDir(0f);
			SetMove(Vector3.zero);
			bornIdleTimer += Time.deltaTime;
			if (!(bornIdleTimer >= 0.5f))
			{
				break;
			}
			base.Anima.SetTrigger("Walk");
			if (pattern == AIPattern.Pattern4)
			{
				state = MonsterState.MoveAroundTarget;
				GetNearestTarget();
				if (base.HaveTarget)
				{
					GetNavInfo(Tool2D.GetNavMeshPoint(base.TargetPoint, randomWalkRadius));
				}
				else
				{
					GetNavInfo(Tool2D.GetNavMeshPoint(base.transform.position, randomWalkRadius));
				}
			}
			else
			{
				state = MonsterState.MoveRandom;
				GetNavInfo(Tool2D.GetNavMeshPoint(base.transform.position, randomWalkRadius));
			}
			break;
		case MonsterState.MoveRandom:
			leg.SetDir(base.CurrentMotion.x);
			if (navInfo.allCornerArrived)
			{
				GetNavInfo(Tool2D.GetNavMeshPoint(base.transform.position, randomWalkRadius));
			}
			else
			{
				SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed);
				CheckNavInfo();
			}
			attackIntervalTimer += Time.deltaTime;
			if (attackIntervalTimer >= attackInterval.result)
			{
				attackIntervalTimer = 0f;
				attackInterval.RandomResult();
				GetNearestTarget();
				if (base.HaveTarget && ToTargetDistanceSqr() < attackDistance * attackDistance)
				{
					state = MonsterState.Attack;
					base.Anima.SetTrigger("Attack");
				}
			}
			break;
		case MonsterState.Attack:
			leg.SetDir(0f);
			SetMove(Vector3.zero);
			break;
		case MonsterState.MoveAroundTarget:
			leg.SetDir(base.CurrentMotion.x);
			if (navInfo.allCornerArrived)
			{
				GetNearestTarget();
				if (base.HaveTarget)
				{
					GetNavInfo(Tool2D.GetNavMeshPoint(base.TargetPoint, randomWalkRadius));
				}
				else
				{
					GetNavInfo(Tool2D.GetNavMeshPoint(base.transform.position, randomWalkRadius));
				}
			}
			else
			{
				SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed);
				CheckNavInfo();
			}
			attackIntervalTimer += Time.deltaTime;
			if (attackIntervalTimer >= attackInterval.result && base.HaveTarget && ToTargetDistanceSqr() < attackDistance * attackDistance)
			{
				attackIntervalTimer = 0f;
				attackInterval.RandomResult();
				state = MonsterState.Attack;
				base.Anima.SetTrigger("Attack");
			}
			break;
		default:
			Debug.LogError(state);
			break;
		}
		liquidInteravlTimer += Time.deltaTime;
		if (!(liquidInteravlTimer > liquidInteravl))
		{
			return;
		}
		liquidInteravlTimer -= liquidInteravl;
		if (lastRecordPoint != base.transform.position)
		{
			switch (pattern)
			{
			case AIPattern.Pattern2:
			case AIPattern.Pattern4:
				MucusSystem.CreateMucus(lastRecordPoint, Tool2D.IgnoreZPoint(base.transform), liquidRadius);
				break;
			case AIPattern.Pattern3:
				VenomSystem.CreateVenom(lastRecordPoint, Tool2D.IgnoreZPoint(base.transform), liquidRadius, liquidTime);
				break;
			}
			lastRecordPoint = Tool2D.IgnoreZPoint(base.transform);
		}
	}

	public override void AnimaAction(string animaName)
	{
		if (!(animaName == "Attack"))
		{
			if (animaName == "AttackFinish")
			{
				base.Anima.SetTrigger("Walk");
				if (pattern == AIPattern.Pattern4)
				{
					state = MonsterState.MoveAroundTarget;
					return;
				}
				state = MonsterState.MoveRandom;
				GetNavInfo(Tool2D.GetNavMeshPoint(base.transform.position, randomWalkRadius));
			}
			else
			{
				Debug.LogError(animaName);
			}
			return;
		}
		SEMgr.Inst.spell1001Shoot.PlaySE();
		bulletCount.RandomResult();
		UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
		switch (pattern)
		{
		case AIPattern.Pattern1:
		case AIPattern.Pattern2:
		case AIPattern.Pattern3:
		{
			for (int j = 0; j < bulletCount.result; j++)
			{
				float num2 = spellUpForce.RandomResult();
				sSPModifier.CurrentFallSpeed = 0f - num2;
				sSPModifier.SpawnPosition = base.transform.position + new Vector3(0f, 0f, 0f - spellHeight);
				Vector3 vector = base.transform.position + Random.Range(0f, 1f) * attackDistance * Tool2D.GetDir() * Random.Range(0f, bulletLandRadius);
				if (base.HaveTarget && ToTargetDistanceSqr() < attackDistance * attackDistance)
				{
					vector = Tool2D.IgnoreZPoint(base.TargetPoint + Tool2D.GetDir() * Random.Range(0f, bulletLandRadius));
				}
				sSPModifier.Speed = GeneralTool.CannonSpeed(num2, spellHeight, spellGravity, Tool2D.IgnoreZDistance(base.transform.position, vector));
				sSPModifier.Direction = ToPointDir(vector);
				sSPModifier.ApplyToSSP(ref ssp);
				ShootSpell(ssp);
			}
			break;
		}
		case AIPattern.Pattern4:
		{
			for (int i = 0; i < bulletCount.result; i++)
			{
				sSPModifier.SpawnPosition = base.transform.position + new Vector3(0f, 0f, 0f - spellHeight);
				sSPModifier.Direction = Tool2D.GetDir();
				sSPModifier.CurrentFallSpeed = 0f - spellUpForce.RandomResult();
				sSPModifier.Speed = spellSpeed.RandomResult();
				int num = Random.Range(0, 3);
				sSPModifier.ApplyToSSP(ref ssp);
				switch (num)
				{
				case 0:
					ssp.ConfigComponentData.ColorType = SpellColorType.Monster;
					break;
				case 1:
					ssp.ConfigComponentData.ColorType = SpellColorType.Mucus;
					ssp.ElementComponentData.MucusMoveSpeedRatio = 0.6f;
					ssp.ElementComponentData.MucusSpellSpeedRatio = 0.7f;
					ssp.ElementComponentData.MucusDuration = 3f;
					break;
				case 2:
					ssp.ConfigComponentData.ColorType = SpellColorType.Venom;
					ssp.ElementComponentData.VenomApplyCount = 2f;
					ssp.ElementComponentData.VenomDuration = 3f;
					break;
				default:
					Debug.LogError(num);
					break;
				}
				ShootSpell(ssp);
			}
			break;
		}
		default:
			Debug.LogError(pattern);
			break;
		}
	}

	public override void AfterDead(ref TakeDamageInfo_Dots info)
	{
		base.AfterDead(ref info);
		switch (pattern)
		{
		case AIPattern.Pattern2:
			MucusSystem.CreateMucus(Tool2D.IgnoreZPoint(base.transform), base.transform.localScale.x * base.CC_Self.radius * 2f);
			break;
		case AIPattern.Pattern3:
			VenomSystem.CreateVenom(Tool2D.IgnoreZPoint(base.transform), base.transform.localScale.x * base.CC_Self.radius * 2f, liquidTime);
			break;
		case AIPattern.Pattern4:
			MucusSystem.CreateMucus(Tool2D.IgnoreZPoint(base.transform), base.transform.localScale.x * base.CC_Self.radius * 2f);
			VenomSystem.CreateVenom(Tool2D.IgnoreZPoint(base.transform), base.transform.localScale.x * base.CC_Self.radius * 2f, liquidTime);
			break;
		default:
			Debug.LogError(pattern);
			break;
		case AIPattern.Pattern1:
			break;
		}
	}
}
