using System.Collections.Generic;
using UnityEngine;

public class Monster50 : UnitBase
{
	public enum MonsterState
	{
		BornIdle,
		Swim,
		Attack
	}

	[Space(50f)]
	public float sideFlySpeedRatio;

	public StateVariableMgr varMgr = new StateVariableMgr();

	public MonsterState _state;

	private bool stateQuit;

	private bool changedState;

	private float stateExistTime;

	[Header("触手")]
	public List<Monster50_Tentacle> tentacles = new List<Monster50_Tentacle>();

	[Header("行动")]
	public float swimForce;

	public VariableInt swimTimesBeforeAttack;

	private int swimTimesCounter;

	[Header("子弹")]
	public int bulletCount;

	private SpellInitialParameter sipBullet = new SpellInitialParameter();

	public float spellSpeed;

	public float spellVerticalSpeed;

	public float spellDuration;

	public int spellDamage;

	public float spellHeight;

	public bool rotateRight;

	[Header("二模式")]
	public AIPattern pattern;

	[Header("和谐")]
	public Sprite sprite_H;

	public MeshRenderer mr;

	private SpellSpawnParams ssp;

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
		sipBullet.ownerPpt = myPpt;
		if (GameMgr.IsHarmony_Static)
		{
			mr.material.SetTexture(GameConstManaged.shaderTextureIndex, sprite_H.texture);
		}
		if (GameMgr.IsMobile_Static && pattern == AIPattern.Pattern2)
		{
			bulletCount -= 2;
			spellSpeed *= 0.8f;
			spellVerticalSpeed *= 0.8f;
		}
		ssp = UnitDotsSyncSystem.GetSpellPrototype(90451);
		UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
		sSPModifier.Speed = Mathf.Sqrt(spellSpeed * spellSpeed + spellVerticalSpeed * spellVerticalSpeed);
		sSPModifier.Shooter = myPpt.myEntity;
		sSPModifier.Duration = spellDuration;
		sSPModifier.Damage = spellDamage;
		sSPModifier.ApplyToSSP(ref ssp);
	}

	public override void EveryInitialCallback()
	{
		for (int i = 0; i < tentacles.Count; i++)
		{
			tentacles[i].Initialize(this);
		}
		swimTimesCounter = Random.Range(0, swimTimesBeforeAttack.RandomResult() / 2);
		rotateRight = GeneralTool.ChanceResult(0.5f);
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
			bornIdleTimer += Time.deltaTime;
			if (bornIdleTimer > 0.5f)
			{
				state = MonsterState.Swim;
			}
			break;
		case MonsterState.Swim:
			if (changedState)
			{
				base.Anima.Play("Swim", 0, 0f);
				swimTimesCounter++;
			}
			SetMove(Vector3.zero);
			break;
		case MonsterState.Attack:
			if (changedState)
			{
				if (pattern == AIPattern.Pattern2)
				{
					base.Anima.Play("SuperAttack");
				}
				else
				{
					base.Anima.Play("Attack");
				}
			}
			SetMove(Vector3.zero);
			break;
		}
	}

	public override void Theme6Reposition(Vector3 changeValue)
	{
		base.Theme6Reposition(changeValue);
		for (int i = 0; i < tentacles.Count; i++)
		{
			tentacles[i].Theme6Reposition(changeValue);
		}
	}

	public override void AnimaAction(string animaName)
	{
		switch (animaName)
		{
		case "playSwimSE":
			SEMgr.Inst.monster50_Swim.PlaySE();
			break;
		case "SwimAddForce":
			base.CurrentMotion = Tool2D.GetDir() * swimForce;
			break;
		case "SwimFinish":
			if (swimTimesCounter >= swimTimesBeforeAttack.result)
			{
				swimTimesCounter = 0;
				swimTimesBeforeAttack.RandomResult();
				state = MonsterState.Attack;
			}
			else
			{
				state = MonsterState.Swim;
			}
			break;
		case "AttackFinish":
			state = MonsterState.Swim;
			break;
		case "Attack":
		{
			SEMgr.Inst.monstrer50_Attack.PlaySE();
			Vector3 dir = Tool2D.GetDir();
			UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
			sSPModifier.SpawnPosition = base.transform.position + new Vector3(0f, 0f, 0f - spellHeight);
			for (int i = 0; i < bulletCount; i++)
			{
				sSPModifier.Direction = Tool2D.GetDir(dir, 360f / (float)bulletCount * (float)i);
				sSPModifier.Float1 = spellVerticalSpeed / sSPModifier.Speed * (float)(rotateRight ? 1 : (-1));
				sSPModifier.Float2 = spellSpeed / sSPModifier.Speed;
				sSPModifier.ApplyToSSP(ref ssp);
				ShootSpell(ssp);
			}
			break;
		}
		}
	}
}
