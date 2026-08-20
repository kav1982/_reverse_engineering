using System.Collections.Generic;
using UnityEngine;

public class Monster108 : UnitBase
{
	public enum MonsterState
	{
		BornIdle,
		Idle,
		RandomMove,
		Follow,
		Attack
	}

	[Header("待机和随机移动")]
	public VariableFloat idleTime;

	public VariableFloat randomMoveTime;

	public VariableFloat randomMoveRadius;

	public float checkInterval;

	public float checkIntervalTimer;

	[Header("攻击")]
	public bool isAttacking;

	private SpellInitialParameter sipBullet = new SpellInitialParameter();

	public float spellHeight;

	public float spellSpeed;

	public float spellDuration;

	public int spellDamage;

	public VariableFloat bulletOffset;

	public float attackStartTime;

	public VariableFloat attackCD;

	public float attackCDTimer;

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

	private SpellSpawnParams SipBulletSsp;

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
		SipBulletSsp = UnitDotsSyncSystem.GetSpellPrototype(90281);
		UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in SipBulletSsp);
		sSPModifier.Speed = spellSpeed;
		sSPModifier.Duration = spellDuration;
		sSPModifier.Damage = spellDamage;
		sSPModifier.Shooter = myPpt.myEntity;
		sSPModifier.ApplyToSSP(ref SipBulletSsp);
		attackCDTimer = 4f;
	}

	public override void EveryInitialCallback()
	{
		state = MonsterState.BornIdle;
		attackCDTimer = 4f;
		attackCD.RandomResult();
		myPpt.InvincibleRegister();
		isAttacking = false;
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
				SEMgr.Inst.monster12Land.PlaySE();
			}
			SetMove(Vector3.zero);
			if (stateExistTime > idleTime.result)
			{
				state = MonsterState.RandomMove;
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
			if (checkIntervalTimer >= checkInterval && PlayerMgr.Inst.PlayerCtrller.IsVisible)
			{
				state = MonsterState.Follow;
			}
			CheckNavInfo();
			if (navInfo.allCornerArrived)
			{
				reference = base.transform.position + Tool2D.GetDir() * randomMoveRadius.result;
				GetNavInfo(reference);
				break;
			}
			SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed);
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
			}
			if (!PlayerMgr.Inst.PlayerCtrller.IsVisible)
			{
				state = MonsterState.Idle;
				break;
			}
			GetNavInfo(PlayerMgr.Inst.PlayerPoint);
			SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed);
			CheckAttack();
			break;
		case MonsterState.Attack:
			if (changedState)
			{
				base.Anima.Play("Attack");
				isAttacking = true;
				attackStartTime = Time.time;
			}
			SetMove(Vector3.zero);
			break;
		}
	}

	public void Attack()
	{
		float num = 30f;
		float num2 = bulletOffset.RandomResult();
		for (int i = 0; i < 12; i++)
		{
			UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in SipBulletSsp);
			sSPModifier.Direction = Tool2D.GetDir(Vector3.up, (float)i * num + 5f + num2);
			sSPModifier.SpawnPosition = new Vector3(0f, -0.2f, 0f - spellHeight) + base.transform.position;
			sSPModifier.ApplyToSSP(ref SipBulletSsp);
			ShootSpell(SipBulletSsp);
		}
		attackCDTimer = 0f;
		attackCD.RandomResult();
		isAttacking = false;
	}

	public void CheckAttack()
	{
		attackCDTimer += Time.deltaTime;
		if (attackCDTimer > attackCD.result && !isAttacking)
		{
			state = MonsterState.Attack;
		}
	}

	public override void AfterDead(ref TakeDamageInfo_Dots info)
	{
		base.AfterDead(ref info);
		SEMgr.Inst.monster12Land.PlaySE();
		ExplodeOnce(base.transform.position);
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
			else
			{
				TakeDamageInfo takeDamageInfo = new TakeDamageInfo();
				takeDamageInfo.knockbackForce = Tool2D.IgnoreZV2ToV1Normal(collidersByTag[i].transform.position, explodePoint) * knockback;
				takeDamageInfo.playerTakeDamageRatio = playerDamageRatio;
				collidersByTag[i].GetComponent<UnitProperty>().TakeDamage(boomDamage, null, takeDamageInfo);
			}
		}
	}

	public override void AnimaAction(string animaName)
	{
		switch (animaName)
		{
		case "Attack":
			Attack();
			break;
		case "UnSetInvincible":
			myPpt.InvincibleUnregister();
			break;
		case "EndAttack":
			state = MonsterState.Follow;
			myPpt.InvincibleRegister();
			break;
		case "EndDead":
			myPpt.AnnouncedDeath();
			break;
		}
	}
}
