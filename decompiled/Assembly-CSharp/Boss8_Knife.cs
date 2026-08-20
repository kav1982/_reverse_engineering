using System.Collections;
using UnityEngine;

public class Boss8_Knife : UnitBase
{
	public enum MonsterState
	{
		BornIdle,
		Idle,
		Die
	}

	[Header("攻击")]
	public bool isAttacking;

	private SpellInitialParameter sipBullet = new SpellInitialParameter();

	public float spellHeight;

	public float spellSpeed;

	public float spellDuration;

	public int spellDamage;

	public VariableFloat attackCD;

	public float attackCDTimer;

	public int shootAmount;

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

	public override void SingleInitialCallback()
	{
		sipBullet.spelldataConfig = SpellConfig.GetConfigCopy(90281);
		sipBullet.spelldataConfig.speed = spellSpeed;
		sipBullet.spelldataConfig.duration = spellDuration;
		sipBullet.spelldataConfig.damage = spellDamage;
		sipBullet.ownerPpt = myPpt;
	}

	public override void EveryInitialCallback()
	{
		state = MonsterState.BornIdle;
		attackCD.RandomResult();
		attackCDTimer = 3f;
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
		attackCDTimer += Time.deltaTime;
		switch (state)
		{
		case MonsterState.BornIdle:
			if (changedState)
			{
				base.Anima.Play("Born");
			}
			if (stateExistTime > 0.5f)
			{
				state = MonsterState.Idle;
			}
			break;
		case MonsterState.Idle:
			if (attackCDTimer > attackCD.result)
			{
				attackCDTimer = 0f;
				attackCD.RandomResult();
				base.Anima.Play("Attack");
			}
			break;
		case MonsterState.Die:
			if (changedState)
			{
				base.Anima.Play("Die");
			}
			break;
		}
	}

	public void Attack()
	{
		SEMgr.Inst.monster12Land.PlaySE();
		StartCoroutine(Shoot());
		attackCDTimer = 0f;
	}

	private IEnumerator Shoot()
	{
		for (int i = 0; i < shootAmount; i++)
		{
			sipBullet.shootDirection = Tool2D.GetDir(Vector3.down, Random.Range((float)(i * 90 / shootAmount) * base.transform.localScale.x, (float)((i + 1) * 90 / shootAmount) * base.transform.localScale.x));
			ObjPoolMgr.Inst.GetGO("Prefabs/Spell/" + sipBullet.spelldataConfig.prefab, new Vector3(0f, 0f, 0f - spellHeight) + base.transform.position).GetComponent<SpellBase>().Initialize(sipBullet);
			yield return new WaitForSeconds(0.03f);
		}
	}

	public override void AnimaAction(string animaName)
	{
		if (!(animaName == "Attack"))
		{
			if (animaName == "Die")
			{
				myPpt.AnnouncedDeath();
			}
		}
		else
		{
			Attack();
		}
	}
}
