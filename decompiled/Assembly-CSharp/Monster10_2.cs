using UnityEngine;

public class Monster10_2 : UnitBase
{
	private enum MonsterState
	{
		BornIdle,
		Idle,
		Attacking
	}

	[Space(50f)]
	public VariableFloat attackInterval;

	public float attackDistance;

	public AIPattern pattern;

	[Header("SPell BulletPrabola")]
	public float spellLandRadius;

	public float spellHeight;

	public VariableFloat spellUpForce;

	public float spellGravity;

	public int spellDamage;

	public bool spellCanRebounce;

	private SpellSpawnParams ssp;

	private MonsterState state;

	private float attackIntervalTimer;

	public override void SingleInitialCallback()
	{
		ssp = UnitDotsSyncSystem.GetSpellPrototype(90191);
		UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
		sSPModifier.Damage = spellDamage;
		sSPModifier.Gravity = 0f - spellGravity;
		sSPModifier.Shooter = myPpt.myEntity;
		switch (pattern)
		{
		case AIPattern.Pattern1:
			sSPModifier.ColorType = SpellColorType.Monster;
			break;
		case AIPattern.Pattern2:
			sSPModifier.ColorType = SpellColorType.Mucus;
			break;
		case AIPattern.Pattern3:
			sSPModifier.ColorType = SpellColorType.Venom;
			break;
		default:
			Debug.LogError(pattern);
			break;
		}
		if (spellCanRebounce)
		{
			sSPModifier.ReboundCount = 1;
		}
		sSPModifier.ApplyToSSP(ref ssp);
	}

	public override void EveryInitialCallback()
	{
		state = MonsterState.BornIdle;
		attackIntervalTimer = 0f;
		attackInterval.RandomResult();
		base.Anima.SetTrigger("Idle");
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
			SetMove(Vector3.zero);
			bornIdleTimer += Time.deltaTime;
			if (bornIdleTimer >= 0.5f)
			{
				state = MonsterState.Idle;
			}
			break;
		case MonsterState.Idle:
			attackIntervalTimer += Time.deltaTime;
			if (attackIntervalTimer >= attackInterval.result)
			{
				attackIntervalTimer = 0f;
				attackInterval.RandomResult();
				GetNearestTarget();
				if (base.HaveTarget && ToTargetDistanceSqr() < attackDistance * attackDistance)
				{
					state = MonsterState.Attacking;
					base.Anima.SetTrigger("Attack");
				}
			}
			break;
		default:
			Debug.LogError(state);
			break;
		case MonsterState.Attacking:
			break;
		}
	}

	public override void AnimaAction(string animaName)
	{
		if (!(animaName == "Attack"))
		{
			if (animaName == "AttackFinish")
			{
				base.Anima.SetTrigger("Idle");
				state = MonsterState.Idle;
			}
			else
			{
				Debug.LogError(animaName);
			}
			return;
		}
		Vector3 vector = base.transform.position + Random.Range(0f, 1f) * attackDistance * Tool2D.GetDir() * Random.Range(0f, spellLandRadius);
		if (base.HaveTarget && ToTargetDistanceSqr() < attackDistance * attackDistance)
		{
			vector = Tool2D.IgnoreZPoint(base.TargetPoint + Tool2D.GetDir() * Random.Range(0f, spellLandRadius));
		}
		float num = spellUpForce.RandomResult();
		UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
		sSPModifier.CurrentFallSpeed = 0f - num;
		sSPModifier.SpawnPosition = base.transform.position + new Vector3(0f, 0f, 0f - spellHeight);
		sSPModifier.Speed = GeneralTool.CannonSpeed(num, spellHeight, spellGravity, Tool2D.IgnoreZDistance(base.transform.position, vector));
		sSPModifier.Direction = ToPointDir(vector);
		sSPModifier.ApplyToSSP(ref ssp);
		ShootSpell(ssp);
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
			VenomSystem.CreateVenom(Tool2D.IgnoreZPoint(base.transform), base.transform.localScale.x * base.CC_Self.radius * 2f, 6f);
			break;
		default:
			Debug.LogError(pattern);
			break;
		case AIPattern.Pattern1:
			break;
		}
	}
}
