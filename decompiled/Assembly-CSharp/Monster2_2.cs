using System;
using UnityEngine;

public class Monster2_2 : UnitBase
{
	private enum UnitState
	{
		BornIdle,
		Summoning
	}

	public int summonID;

	public int sumonMaxCount;

	public VariableFloat summonInterval;

	[Header("BornJump")]
	public float forwardForce;

	public float upForce;

	public float gravity;

	[Header("安全模式")]
	public SpriteRenderer originSR;

	public SpriteRenderer safeModeSR;

	[Header("和谐")]
	public Sprite sprite_H;

	private UnitState state;

	private float summonIntervalTimer = 999999f;

	private int summonCounter;

	public override void EveryInitialCallback()
	{
		state = UnitState.BornIdle;
		summonIntervalTimer = 999999f;
		summonCounter = 0;
		summonInterval.RandomResult();
		if (GameMgr.IsHarmony_Static && sprite_H != null)
		{
			originSR.sprite = sprite_H;
		}
	}

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
		if (DataMgr.settingData.SafeMode)
		{
			originSR.gameObject.SetActive(value: false);
			safeModeSR.gameObject.SetActive(value: true);
		}
		else
		{
			originSR.gameObject.SetActive(value: true);
			safeModeSR.gameObject.SetActive(value: false);
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
		case UnitState.BornIdle:
			bornIdleTimer += Time.deltaTime;
			if (bornIdleTimer >= 0.5f)
			{
				state = UnitState.Summoning;
			}
			break;
		case UnitState.Summoning:
			summonIntervalTimer += Time.deltaTime;
			if (summonIntervalTimer >= summonInterval.result)
			{
				summonIntervalTimer = 0f;
				summonInterval.RandomResult();
				base.Anima.SetTrigger("Summon");
			}
			break;
		default:
			Debug.LogError(state);
			break;
		}
	}

	public override void AnimaAction(string animaName)
	{
		if (animaName == "Summon")
		{
			ObjPoolMgr.Inst.GetGO("Prefabs/Units/" + summonID, base.transform.position).GetComponent<Monster1>().BornJump(Tool2D.GetDir() * forwardForce, upForce, gravity);
			summonCounter++;
			if (summonCounter >= sumonMaxCount)
			{
				DotsAnnouncedDeath();
			}
		}
		else
		{
			Debug.LogError(animaName);
		}
	}
}
