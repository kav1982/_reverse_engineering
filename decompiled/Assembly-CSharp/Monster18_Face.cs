using System;
using UnityEngine;

public class Monster18_Face : UnitBase
{
	private enum MonsterState
	{
		BornFly,
		Charge,
		ChargeIdle,
		Attack
	}

	public float attackDistance;

	[Header("Spell")]
	public float spellHeight;

	public float spellSpeed;

	public float spellDuration;

	[Header("安全模式")]
	public SpriteRenderer SR_OriginHead;

	public SpriteRenderer SR_SafeHead;

	private MonsterState state;

	private SpellSpawnParams ssp;

	private void OnEnable()
	{
		EventMgr.SafeModeStateChange = (Action)Delegate.Combine(EventMgr.SafeModeStateChange, new Action(SetSafeMode));
		SetSafeMode();
	}

	private void OnDisable()
	{
		EventMgr.SafeModeStateChange = (Action)Delegate.Remove(EventMgr.SafeModeStateChange, new Action(SetSafeMode));
	}

	public void SetSafeMode()
	{
		if (DataMgr.settingData.SafeMode || GameMgr.IsHarmony_Static)
		{
			SR_OriginHead.gameObject.SetActive(value: false);
			SR_SafeHead.gameObject.SetActive(value: true);
		}
		else
		{
			SR_OriginHead.gameObject.SetActive(value: true);
			SR_SafeHead.gameObject.SetActive(value: false);
		}
	}

	public override void SingleInitialCallback()
	{
		ssp = UnitDotsSyncSystem.GetSpellPrototype(10011);
		UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
		sSPModifier.Duration = spellDuration;
		sSPModifier.Speed = spellSpeed;
		sSPModifier.Shooter = myPpt.myEntity;
		sSPModifier.ApplyToSSP(ref ssp);
	}

	public override void EveryInitialCallback()
	{
		state = MonsterState.BornFly;
		base.Anima.SetTrigger("BornFly");
	}

	public override void Update()
	{
		base.Update();
		if (base.IsLocked)
		{
			return;
		}
		SetMove(Vector3.zero, isFlip: false);
		switch (state)
		{
		case MonsterState.ChargeIdle:
			checkTargetIntervalTimer += Time.deltaTime;
			if (checkTargetIntervalTimer >= 1f)
			{
				checkTargetIntervalTimer = 0f;
				GetNearestTarget(checkWall: true);
				if (base.HaveTarget && ToTargetDistanceSqr() < attackDistance * attackDistance)
				{
					state = MonsterState.Attack;
					base.Anima.SetTrigger("Attack");
				}
			}
			break;
		default:
			Debug.LogError(state);
			break;
		case MonsterState.BornFly:
		case MonsterState.Charge:
		case MonsterState.Attack:
			break;
		}
	}

	public override void AnimaAction(string animaName)
	{
		switch (animaName)
		{
		case "BornFlyFinish":
			state = MonsterState.Charge;
			base.Anima.SetTrigger("Charge");
			break;
		case "ChargeFinish":
			state = MonsterState.ChargeIdle;
			break;
		case "Shoot":
			if (base.HaveTarget)
			{
				UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
				sSPModifier.Direction = ToTargetDir();
				sSPModifier.SpawnPosition = base.transform.position + new Vector3(0f, 0f, 0f - spellHeight);
				sSPModifier.ApplyToSSP(ref ssp);
				ShootSpell(ssp);
			}
			break;
		case "AttackFinish":
			state = MonsterState.Charge;
			base.Anima.SetTrigger("Charge");
			break;
		default:
			Debug.LogError(animaName);
			break;
		}
	}
}
