using System.Collections.Generic;
using UnityEngine;

public class Monster104 : UnitBase
{
	public enum MonsterState
	{
		BornIdle,
		Idle,
		RandomMove,
		Follow,
		Hurt,
		Bang
	}

	[Header("待机和随机移动")]
	public VariableFloat idleTime;

	public VariableFloat randomMoveTime;

	public VariableFloat randomMoveRadius;

	public float checkInterval;

	public float checkIntervalTimer;

	[Header("追击")]
	public float maxBangDistance;

	[Header("受伤")]
	public float hurtTime;

	public float hurtHealthPercent;

	public float hurtCounter;

	public float hurtRecoverSpeedPercent;

	[Header("尸体图片")]
	public SpriteRenderer bodyRender;

	public Sprite idleSprite;

	public Sprite bangSprite;

	public Sprite jumpSprite;

	[Header("自爆")]
	public ShockParam shockParam;

	public float knockback;

	public float boomRadius;

	public int boomDamage;

	public float playerDamageRatio;

	public bool readyToBang;

	[Header("状态机")]
	public MonsterState _state;

	public StateVariableMgr varMgr = new StateVariableMgr();

	private bool stateQuit;

	private bool changedState;

	private float stateExistTime;

	public float jumpMaxDistance;

	public float jumpUpSpeed;

	public float gravity;

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
	}

	public override void EveryInitialCallback()
	{
		readyToBang = false;
		base.gameObject.layer = LayerMask.NameToLayer("Monster");
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
		hurtCounter -= Time.deltaTime * hurtRecoverSpeedPercent * myPpt.unitCfg.maxHP;
		hurtCounter = Mathf.Max(hurtCounter, 0f);
		switch (state)
		{
		case MonsterState.BornIdle:
			if (changedState)
			{
				base.Anima.Play("Idle");
				bodyRender.sprite = idleSprite;
			}
			SetMove(Vector3.zero);
			if (stateExistTime > 0.5f)
			{
				state = MonsterState.Follow;
			}
			break;
		case MonsterState.Idle:
			if (changedState)
			{
				base.Anima.Play("Idle");
				idleTime.RandomResult();
			}
			SetMove(Vector3.zero);
			if (stateExistTime > idleTime.result)
			{
				state = MonsterState.RandomMove;
			}
			checkIntervalTimer += Time.deltaTime;
			if (checkIntervalTimer >= checkInterval)
			{
				GetNearestTarget();
				checkTargetIntervalTimer = 0f;
				if (base.HaveTarget)
				{
					state = MonsterState.Follow;
				}
			}
			break;
		case MonsterState.RandomMove:
		{
			ref Vector3 reference = ref varMgr.RegV3(0);
			if (changedState)
			{
				base.Anima.Play("Move");
				randomMoveTime.RandomResult();
				randomMoveRadius.RandomResult();
				reference = base.transform.position + Tool2D.GetDir() * randomMoveRadius.result;
				GetNavInfo(reference);
			}
			if (stateExistTime > randomMoveTime.result)
			{
				state = MonsterState.Idle;
				break;
			}
			checkIntervalTimer += Time.deltaTime;
			if (checkIntervalTimer >= checkInterval)
			{
				GetNearestTarget();
				checkTargetIntervalTimer = 0f;
				if (base.HaveTarget)
				{
					state = MonsterState.Follow;
				}
			}
			CheckNavInfo();
			if (navInfo.allCornerArrived)
			{
				reference = base.transform.position + Tool2D.GetDir() * randomMoveRadius.result;
				GetNavInfo(reference);
				break;
			}
			SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed, isFlip: false);
			if (base.CurrentMotion.x < 0f)
			{
				base.transform.localScale = new Vector3(-1f, 1f, 1f);
			}
			else
			{
				base.transform.localScale = new Vector3(1f, 1f, 1f);
			}
			break;
		}
		case MonsterState.Follow:
			if (changedState)
			{
				base.Anima.Play("Move");
				GetNearestTarget();
			}
			if (!base.HaveTarget)
			{
				GetNearestTarget();
			}
			if (!base.HaveTarget)
			{
				state = MonsterState.Idle;
				break;
			}
			if (Tool2D.IgnoreZDistanceSqr(base.transform.position, PlayerMgr.Inst.GetNearestPpt(base.transform.position).transform.position) < maxBangDistance * maxBangDistance)
			{
				state = MonsterState.Bang;
			}
			GetNavInfo(base.TargetPoint);
			SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed);
			break;
		case MonsterState.Hurt:
			if (changedState)
			{
				base.Anima.Play("Hurt", 0, 0f);
				hurtCounter = 0f;
			}
			SetMove(Vector3.zero, isFlip: false);
			if (stateExistTime > hurtTime)
			{
				state = MonsterState.Follow;
			}
			break;
		case MonsterState.Bang:
			if (changedState)
			{
				base.Anima.Play("Bang", 0, 0f);
			}
			SetMove(Vector3.zero, isFlip: false);
			if (base.transform.position.z > 0f)
			{
				base.transform.position = Tool2D.IgnoreZPoint(base.transform);
				JumpStop_Dots();
				myPpt.FlyUnregister();
				myPpt.AnnouncedDeath();
			}
			break;
		}
	}

	public override void AfterTakeDamage(TakeDamageInfo info)
	{
		if (state == MonsterState.Bang)
		{
			return;
		}
		hurtCounter += info.damage;
		if (hurtCounter > myPpt.unitCfg.maxHP * hurtHealthPercent)
		{
			if (state == MonsterState.Hurt)
			{
				stateExistTime -= hurtTime;
				stateExistTime = Mathf.Max(stateExistTime, 0f);
			}
			else
			{
				state = MonsterState.Hurt;
			}
		}
	}

	public override void AfterDead(ref TakeDamageInfo_Dots info)
	{
		base.AfterDead(ref info);
		SEMgr.Inst.monster12Land.PlaySE();
		ExplodeOnce(base.transform.position);
	}

	public override void AnimaAction(string animaName)
	{
		switch (animaName)
		{
		case "JumpStart":
			bodyRender.sprite = idleSprite;
			break;
		case "Ready":
			NormalJump(Mathf.Min(Vector3.Distance(base.transform.position, base.TargetPoint), jumpMaxDistance) * ToTargetDir(), jumpUpSpeed, gravity);
			base.gameObject.layer = LayerMask.NameToLayer("Monster_Fly");
			bodyRender.sprite = jumpSprite;
			break;
		case "ChangeBangSprite":
			bodyRender.sprite = bangSprite;
			break;
		}
	}

	private void ExplodeOnce(Vector3 explodePoint)
	{
		ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Monster104_ExplosionSingle", explodePoint, 6f);
		ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Monster34_Trace", explodePoint, 10f);
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

	public void NormalJump(Vector3 forwardDelta, float upForce, float gravity)
	{
		myPpt.FlyRegister();
		GetNearestTarget();
		Vector3 vector = ((!base.HaveTarget) ? Tool2D.GetNavMeshPointIngoreZ(base.transform.position + forwardDelta.normalized) : Tool2D.GetNavMeshPointIngoreZ(base.transform.position + forwardDelta));
		float num = GeneralTool.CannonSpeed(upForce, 0f, gravity, Vector3.Distance(base.transform.position, vector));
		base.Rigid.linearVelocity = ToPointDir(vector) * num;
		JumpStart_Dots(upForce, gravity);
	}

	protected override void SetFlip(float motionX)
	{
		if (base.HaveTarget)
		{
			if (base.TargetPoint.x < base.transform.position.x)
			{
				base.transform.localScale = new Vector3(-1f, 1f, 1f);
			}
			else
			{
				base.transform.localScale = new Vector3(1f, 1f, 1f);
			}
		}
	}
}
