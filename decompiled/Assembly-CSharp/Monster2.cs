using System;
using Unity.Transforms;
using UnityEngine;

public class Monster2 : UnitBase
{
	private enum UnitState
	{
		Idle,
		Hatching
	}

	[Space(50f)]
	public int summonID;

	public float checkInterval;

	public float hatchDistance;

	public float hatchTime;

	public float hatchAnimaSpeed;

	private UnitState state;

	private float checkIntervalTimer;

	private float hatchTimer;

	[Header("安全模式")]
	public SpriteRenderer originSR;

	public SpriteRenderer safeModeSR;

	[Header("和谐")]
	public Sprite sprite_H;

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

	public override void EveryInitialCallback()
	{
		state = UnitState.Idle;
		checkIntervalTimer = 0f;
		hatchTimer = 0f;
		base.Anima.SetFloat("Speed", 1f);
		if (GameMgr.IsHarmony_Static && sprite_H != null)
		{
			originSR.sprite = sprite_H;
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
		case UnitState.Idle:
			checkIntervalTimer += Time.deltaTime;
			if (checkIntervalTimer >= checkInterval)
			{
				checkIntervalTimer = 0f;
				GetNearestTarget();
				if (base.HaveTarget && (base.transform.position - (Vector3)GetComponentData<LocalTransform>(targetEntity).Position).sqrMagnitude < hatchDistance * hatchDistance)
				{
					state = UnitState.Hatching;
					base.Anima.SetFloat("Speed", hatchAnimaSpeed);
				}
			}
			break;
		case UnitState.Hatching:
			hatchTimer += Time.deltaTime;
			if (hatchTimer >= hatchTime)
			{
				ObjPoolMgr.Inst.GetGO("Prefabs/Units/" + summonID, base.transform.position);
				DotsAnnouncedDeath();
			}
			break;
		default:
			Debug.LogError(state);
			break;
		}
	}

	public override void AfterTakeDamage_Dots(ref TakeDamageInfo_Dots info)
	{
		if (state != UnitState.Hatching)
		{
			state = UnitState.Hatching;
			base.Anima.SetFloat("Speed", hatchAnimaSpeed);
		}
	}
}
