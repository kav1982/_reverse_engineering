using UnityEngine;
using UnityEngine.UI;

public class Monster999 : UnitBase
{
	private enum UnitState
	{
		Idle,
		Stand,
		Attack,
		Win
	}

	public int dropBloodChangeState;

	public float attackInterval;

	public VariableFloat angle;

	public Text text;

	public int text_1Congratulation;

	public int text_2Oh;

	public int text_3TooLate;

	public int text_4Win;

	public int text_5YouhaveMyRespect;

	[Header("Spell")]
	public Vector3 spellOffset;

	public int spellCount;

	public float spellHeight;

	public float spellSpeed;

	public float spellDuration;

	private UnitState state;

	private float attackIntervalTimer;

	private bool isStand;

	private Monster999_Mother mother;

	public override void SingleInitialCallback()
	{
		spellCfg1 = SpellConfig.GetConfigCopy(10011);
		spellCfg1.speed = spellSpeed;
		spellCfg1.duration = spellDuration;
	}

	public override void EveryInitialCallback()
	{
		base.EveryInitialCallback();
		state = UnitState.Idle;
		text.text = text_1Congratulation.GetText();
		base.Anima.Play("Monster999_Idle");
		base.CC_Self.enabled = true;
		base.CC_Self.radius = 0.4f;
		base.Rigid.isKinematic = false;
		base.Anima.speed = 1f;
		myPpt.CanTouch = true;
		text.text = text_1Congratulation.GetText();
		myPpt.unitCfg.unitType = UnitType.Monster;
	}

	public override void Frame1InitialCallback()
	{
		Debug.Log("这个地方应该将额外掉落物清空，现在掉落物走的是Dots，所以这个怪物有了Dots在DOts里写");
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
		case UnitState.Attack:
			if (PlayerMgr.Inst.PlayerPpt.AlreadyDead)
			{
				state = UnitState.Win;
				base.Anima.SetTrigger("Win");
				text.text = text_4Win.GetText();
				break;
			}
			attackIntervalTimer += Time.deltaTime;
			if (attackIntervalTimer >= attackInterval)
			{
				attackIntervalTimer = 0f;
				Random.Range(0f, 360f);
				for (int i = 0; i < spellCount; i++)
				{
					ObjPoolMgr.Inst.GetGO("Prefabs/Spell/" + spellCfg1.prefab, base.transform.position + spellOffset + new Vector3(0f, 0f, 0f - spellHeight)).GetComponent<SpellBase>().Initialize(myPpt, ToPointDir(PlayerMgr.Inst.PlayerPoint, angle.RandomResult()), spellCfg1, null);
				}
			}
			break;
		default:
			Debug.LogError(state);
			break;
		case UnitState.Idle:
		case UnitState.Stand:
		case UnitState.Win:
			break;
		}
	}

	public override void AnimaAction(string animaName)
	{
		if (!(animaName == "Stand1"))
		{
			if (animaName == "StandFinish")
			{
				state = UnitState.Attack;
				base.Anima.SetTrigger("Attack");
			}
			else
			{
				Debug.LogError(animaName);
			}
		}
		else
		{
			text.text = text_3TooLate.GetText();
			base.CC_Self.radius = 1.5f;
		}
	}

	public override void AfterTakeDamage(TakeDamageInfo info)
	{
		base.AfterTakeDamage(info);
		Stand();
	}

	public override void AfterDead(ref TakeDamageInfo_Dots info)
	{
		base.AfterDead(ref info);
	}

	public void SetMother(Monster999_Mother mother)
	{
		this.mother = mother;
	}

	public void Stand(bool forceStand = false)
	{
		if (!isStand && state == UnitState.Idle && (myPpt.unitCfg.currentHP <= myPpt.unitCfg.maxHP - (float)dropBloodChangeState || forceStand))
		{
			isStand = true;
			state = UnitState.Stand;
			base.Anima.SetTrigger("Stand");
			text.text = text_2Oh.GetText();
			for (int num = LevelMgr.Inst.CurrentRoomCtrller.TargetablePpts.Count - 1; num >= 0; num--)
			{
				LevelMgr.Inst.CurrentRoomCtrller.TargetablePpts[num].GetComponent<Monster999>()?.Stand(forceStand: true);
			}
			mother.ShowHP();
		}
	}
}
