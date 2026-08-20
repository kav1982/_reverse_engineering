using System.Collections.Generic;
using UnityEngine;

public class Monster112 : UnitBase
{
	public enum MonsterState
	{
		BornIdle,
		Idle,
		Move,
		Scream,
		AttackPrepare,
		Attack
	}

	public Vector3 moveOffset;

	public float offsetRadius;

	public UnitProperty targetProperty;

	public float attackCdTime;

	public float attackCdTimer;

	public Animator circleAnima;

	[Header("自爆")]
	public ShockParam shockParam;

	public float knockback;

	public float boomRadius;

	public int boomDamage;

	public float playerDamageRatio;

	[Header("状态机")]
	public MonsterState _state;

	public StateVariableMgr varMgr = new StateVariableMgr();

	private bool stateQuit;

	private bool changedState;

	private float stateExistTime;

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
		moveOffset = Random.insideUnitSphere * offsetRadius;
		state = MonsterState.BornIdle;
	}

	public override void Update()
	{
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
		switch (state)
		{
		case MonsterState.BornIdle:
			if (changedState)
			{
				base.Anima.Play("Idle");
			}
			if (stateExistTime > 1f)
			{
				state = MonsterState.Move;
			}
			SetMove(Vector3.zero);
			break;
		case MonsterState.Idle:
			if (changedState)
			{
				base.Anima.Play("Idle");
			}
			SetMove(Vector3.zero);
			break;
		case MonsterState.Move:
			if (changedState)
			{
				base.Anima.Play("Move");
				foreach (UnitProperty targetablePpt in LevelMgr.Inst.CurrentRoomCtrller.TargetablePpts)
				{
					if (targetablePpt != myPpt)
					{
						targetProperty = targetablePpt;
						break;
					}
				}
			}
			GetNavInfo(targetProperty.transform.position + moveOffset);
			SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed);
			attackCdTimer += Time.deltaTime;
			if (attackCdTimer > attackCdTime)
			{
				attackCdTimer = 0f;
				state = MonsterState.Scream;
			}
			if (LevelMgr.Inst.CurrentRoomCtrller.TargetablePpts.Count == 1 && state != MonsterState.AttackPrepare)
			{
				state = MonsterState.AttackPrepare;
			}
			break;
		case MonsterState.Scream:
			if (changedState)
			{
				base.Anima.Play("Scream");
				circleAnima.Play("CircleScream");
			}
			SetMove(Vector3.zero);
			break;
		case MonsterState.AttackPrepare:
			if (changedState)
			{
				base.Anima.Play("AttackPrepare");
			}
			SetMove(Vector3.zero);
			break;
		case MonsterState.Attack:
		{
			ref Vector3 reference = ref varMgr.RegV3(0);
			if (changedState)
			{
				base.Anima.Play("Attack");
				GetNearestTarget();
				if (base.HaveTarget)
				{
					reference = Tool2D.IgnoreZV2ToV1Normal(base.TargetPoint, base.transform.position);
				}
				else
				{
					reference = Tool2D.GetDir();
				}
				base.transform.eulerAngles = Tool2D.GetEulerAngleByDir(reference);
			}
			base.CurrentMotion = reference * base.MoveSpeed * 5f;
			break;
		}
		}
	}

	public override void AnimaAction(string animaName)
	{
		if (!(animaName == "ScreamEnd"))
		{
			if (animaName == "AttackPrepareEnd")
			{
				state = MonsterState.Attack;
			}
		}
		else
		{
			state = MonsterState.Move;
		}
	}

	public override void AfterDead(ref TakeDamageInfo_Dots info)
	{
		base.AfterDead(ref info);
		ExplodeOnce(base.transform.position);
	}

	private void OnCollisionEnter(Collision collision)
	{
		if ((collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Abyss")) && state == MonsterState.Attack)
		{
			myPpt.AnnouncedDeath();
		}
	}

	private void ExplodeOnce(Vector3 explodePoint)
	{
		ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Monster104_ExplosionSingle", explodePoint, 6f);
		ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Monster34_Trace", explodePoint, 300f);
		CamController.Inst.SetShock(shockParam);
		SEMgr.Inst.monster34Explosion.PlaySE();
		List<Collider> collidersByTag = GeneralTool.GetCollidersByTag(explodePoint, boomRadius, "Monster", "Destructible", "SolidObj", "Spell", "RollBall", "Butterfly", "Brittleness", "Player", "Teammate");
		for (int i = 0; i < collidersByTag.Count; i++)
		{
			if (collidersByTag[i].tag == "Spell" || collidersByTag[i].tag == "RollBall" || collidersByTag[i].tag == "Butterfly")
			{
				if (!collidersByTag[i].gameObject.activeInHierarchy)
				{
					continue;
				}
				SpellBase componentInParent = collidersByTag[i].GetComponentInParent<SpellBase>();
				if (componentInParent.spellCfg.abilityType != SpellAbilityType.FireBall)
				{
					if (componentInParent.spellCfg.abilityType == SpellAbilityType.Rollball)
					{
						((Spell1002RollBall)componentInParent).TakeDamage(boomDamage);
					}
					else if (componentInParent.spellCfg.abilityType == SpellAbilityType.Butterfly)
					{
						((Spell1003Butterfly)componentInParent).HitEFAndRecycle();
					}
				}
			}
			else if (!(collidersByTag[i] == base.CC_Self))
			{
				TakeDamageInfo takeDamageInfo = new TakeDamageInfo();
				takeDamageInfo.knockbackForce = Tool2D.IgnoreZV2ToV1Normal(collidersByTag[i].transform.position, explodePoint) * knockback;
				takeDamageInfo.playerTakeDamageRatio = playerDamageRatio;
				collidersByTag[i].GetComponent<UnitProperty>().TakeDamage(boomDamage, null, takeDamageInfo);
			}
		}
	}
}
