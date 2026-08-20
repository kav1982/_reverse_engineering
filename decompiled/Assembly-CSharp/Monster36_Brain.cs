using System.Collections.Generic;
using UnityEngine;

public class Monster36_Brain : UnitBase
{
	private enum MonsterState
	{
		None,
		BornIdle,
		Idle,
		Move,
		Attack,
		Escape,
		EscapeIdle
	}

	private MonsterState state = MonsterState.BornIdle;

	private Vector3 roomCenterPoint;

	private float roomWidth;

	private float roomHeight;

	private MonsterState preState;

	private MonsterState tempState;

	private bool changedState;

	private bool invisibleStarted;

	public float bornIdleTime;

	private float bornWaitTimer;

	public float invisibleSpeed;

	private float invisibleAlpha = 1f;

	public List<ParticleSystem> signParticles = new List<ParticleSystem>();

	public float signTime;

	public float signExistTime;

	private float idleTimer;

	public VariableFloat idleTime;

	private Vector3 moveDir;

	public VariableFloat moveTime;

	private float moveTimer;

	public Vector3 knockBack;

	public Monster36 mouth;

	public VariableFloat waterDropFix;

	public float waterDropTime;

	private float waterDropTimer;

	public float waterRadius;

	public ParticleSystem waterDropParticle;

	private ParticleSystem.MainModule mainModule;

	private Shadow thisShadow;

	private void Start()
	{
		thisShadow = GetComponent<Shadow>();
		mainModule = waterDropParticle.main;
	}

	public override void Update()
	{
		changedState = false;
		preState = tempState;
		tempState = state;
		if (state == MonsterState.None)
		{
			state = MonsterState.BornIdle;
		}
		if (preState != state)
		{
			changedState = true;
		}
		if (myPpt.BaseColor.a != invisibleAlpha)
		{
			for (int i = 0; i < myPpt.SR_Models.Length; i++)
			{
				Color color = myPpt.SR_Models[i].color;
				myPpt.SR_Models[i].color = new Color(color.r, color.g, color.b, invisibleAlpha);
			}
			Color color2 = mainModule.startColor.color;
			mainModule.startColor = new Color(color2.r, color2.g, color2.b, invisibleAlpha / 2f);
		}
		waterDropTimer += Time.deltaTime;
		if (waterDropTimer > waterDropTime)
		{
			waterDropTimer = 0f;
			LevelMgr.Inst.CurrentRoomCtrller.waterCtrller.CreateWater(base.transform.position + Tool2D.GetDir() * waterDropFix.RandomResult(), waterRadius);
		}
		base.Update();
		if (base.IsLocked)
		{
			return;
		}
		if (invisibleStarted && invisibleAlpha > 0f)
		{
			invisibleAlpha -= Time.deltaTime * invisibleSpeed;
		}
		if (invisibleAlpha > 0f && !thisShadow.IsShow)
		{
			thisShadow.Show();
		}
		if (invisibleAlpha <= 0f && thisShadow.IsShow)
		{
			thisShadow.Hide();
		}
		switch (state)
		{
		case MonsterState.BornIdle:
			if (changedState)
			{
				bornWaitTimer = 0f;
				myPpt.CanTouch = false;
				base.CurrentMotion = Vector3.zero;
				myPpt.TakeKnockback(knockBack);
			}
			bornWaitTimer += Time.deltaTime;
			if (bornWaitTimer > bornIdleTime)
			{
				state = MonsterState.Move;
			}
			SetMove(Vector3.zero);
			break;
		case MonsterState.Idle:
			if (changedState)
			{
				invisibleStarted = true;
				idleTime.RandomResult();
				idleTimer = 0f;
			}
			idleTimer += Time.deltaTime;
			if (idleTimer > idleTime.result)
			{
				state = MonsterState.Move;
			}
			SetMove(Vector3.zero);
			break;
		case MonsterState.Move:
			if (changedState)
			{
				invisibleStarted = true;
				moveDir = Tool2D.GetDir();
				moveTime.RandomResult();
				moveTimer = 0f;
			}
			moveTimer += Time.deltaTime;
			if (moveTimer > moveTime.result)
			{
				state = MonsterState.Idle;
			}
			SetMove(moveDir * myPpt.unitCfg.moveSpeed);
			break;
		}
	}

	public override void BeforeTakeDamage(TakeDamageInfo info)
	{
		base.BeforeTakeDamage(info);
		invisibleAlpha = 1f;
		if (info.beHitShake)
		{
			if (info.spellBase != null)
			{
				mouth.myPpt.TakeBeHit(info.spellBase.Direction);
			}
			else
			{
				mouth.myPpt.TakeBeHit(Tool2D.GetDir());
			}
		}
		mouth.myPpt.SetBeHitColor();
	}

	public override void AfterDead(ref TakeDamageInfo_Dots info)
	{
		base.AfterDead(ref info);
		mouth.myPpt.AnnouncedDeath();
	}

	public override void AnimaAction(string animaName)
	{
	}
}
