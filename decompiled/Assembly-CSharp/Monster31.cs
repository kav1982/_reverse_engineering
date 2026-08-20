using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class Monster31 : UnitBase
{
	public enum Monster31State
	{
		HideIdle,
		Show,
		ShowIdle,
		Hide
	}

	[Space(50f)]
	public SpriteRenderer sr;

	public Monster31_Tentacle pfb_Tentacle;

	public int tentacleCount;

	public Vector3 eyeColliderSize;

	public VariableFloat hideIdleTime;

	public float hideTime;

	public float showTime;

	public VariableFloat showIdleTime;

	[Header("Pattern")]
	public AIPattern pattern;

	public Monster31_Eye pfb_Eye;

	public Monster31_TentacleLong pfb_TentacleLong;

	public static List<Monster31> teammates;

	[Header("状态机")]
	public Monster31State _state;

	public StateVariableMgr varMgr = new StateVariableMgr();

	private bool stateQuit;

	private bool changedState;

	private float stateExistTime;

	private Monster31_Tentacle[] tentacles;

	private Monster31_Eye eye;

	private Monster31_EyeUnit eyeUnit;

	private Monster31_TentacleLong tentacleLong;

	public bool Monster31IsFrozen => base.IsLocked;

	public bool Monster31HaveTarget => base.HaveTarget;

	public Entity Monster31TargetEntity => targetEntity;

	public Vector3 AbyssPoint { get; private set; }

	public Monster31State state
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
		tentacles = new Monster31_Tentacle[tentacleCount];
		for (int i = 0; i < tentacleCount; i++)
		{
			tentacles[i] = Object.Instantiate(pfb_Tentacle, base.transform);
			tentacles[i].SingleInitial(this);
		}
		if (pattern == AIPattern.Pattern1)
		{
			eye = Object.Instantiate(pfb_Eye, base.transform);
			eye.SingleInitial(this);
		}
		else if (pattern == AIPattern.Pattern2)
		{
			tentacleLong = Object.Instantiate(pfb_TentacleLong, base.transform);
			tentacleLong.SingleInitial(this);
		}
		else
		{
			Debug.LogError(pattern);
		}
		sr.sprite = null;
	}

	public override void EveryInitialCallback()
	{
		state = Monster31State.HideIdle;
		GetRandomAbyssPoint();
		base.transform.position = AbyssPoint;
		SyncDotsPosition();
		for (int i = 0; i < tentacleCount; i++)
		{
			tentacles[i].EveryInitial();
		}
		if (pattern == AIPattern.Pattern1)
		{
			eyeUnit = ObjPoolMgr.Inst.GetGO("Prefabs/Units/103121", base.transform.position).GetComponent<Monster31_EyeUnit>();
			eyeUnit.Initialize(this);
			eye.EveryInitial();
		}
		else if (pattern == AIPattern.Pattern2)
		{
			tentacleLong.EveryInitial();
		}
		else
		{
			Debug.LogError(pattern);
		}
		teammates.Add(this);
	}

	public override void Update()
	{
		if (pattern == AIPattern.Pattern1)
		{
			if (state == Monster31State.HideIdle || state == Monster31State.Hide)
			{
				eyeUnit.closeImmume = true;
			}
			else
			{
				eyeUnit.closeImmume = false;
			}
			eyeUnit.transform.position = Tool2D.IgnoreZPoint(eye.EyePoint);
			eyeUnit.SyncDotsPositionSafe();
		}
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
		case Monster31State.HideIdle:
			if (changedState)
			{
				UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
				componentData.showAffect = false;
				componentData.CanBeTarget = false;
				componentData.CanTouch = false;
				SetComponentData(componentData);
				base.CC_Self.enabled = false;
				SetDotsCCEnable(isOpen: false);
				hideIdleTime.RandomResult();
			}
			if (stateExistTime >= hideIdleTime.result)
			{
				state = Monster31State.Show;
			}
			break;
		case Monster31State.Show:
			if (changedState)
			{
				UnitProperty_Dots componentData2 = GetComponentData<UnitProperty_Dots>();
				componentData2.showAffect = true;
				componentData2.CanBeTarget = true;
				SetComponentData(componentData2);
				myPpt.CC_Self.enabled = true;
				SetDotsCCEnable(isOpen: true);
				GetNearestTarget();
				GetRandomAbyssPoint();
				base.transform.position = AbyssPoint;
				SyncDotsPosition();
				for (int j = 0; j < tentacles.Length; j++)
				{
					tentacles[j].Show();
				}
				if (pattern == AIPattern.Pattern1)
				{
					eye.Show();
				}
				else if (pattern == AIPattern.Pattern2)
				{
					tentacleLong.Show();
				}
			}
			if (stateExistTime >= showTime)
			{
				if (base.HaveTarget)
				{
					state = Monster31State.ShowIdle;
				}
				else
				{
					state = Monster31State.Hide;
				}
			}
			break;
		case Monster31State.ShowIdle:
			if (changedState)
			{
				showIdleTime.RandomResult();
			}
			if (!base.HaveTarget)
			{
				GetNearestTarget();
			}
			if (!base.HaveTarget)
			{
				state = Monster31State.Hide;
			}
			else if (stateExistTime >= showIdleTime.result)
			{
				state = Monster31State.Hide;
			}
			break;
		case Monster31State.Hide:
			if (changedState)
			{
				for (int i = 0; i < tentacles.Length; i++)
				{
					tentacles[i].Hide();
				}
				if (pattern == AIPattern.Pattern1)
				{
					eye.Hide();
				}
				else if (pattern == AIPattern.Pattern2)
				{
					tentacleLong.Hide();
				}
			}
			if (stateExistTime >= hideTime)
			{
				state = Monster31State.HideIdle;
			}
			break;
		default:
			Debug.LogError(state);
			break;
		}
	}

	private void GetRandomAbyssPoint()
	{
		if (LevelMgr.Inst.CurrentRoomCtrller.abyssPoints_Dots.Count == 0)
		{
			AbyssPoint = Vector3.zero;
			return;
		}
		if (teammates == null)
		{
			teammates = new List<Monster31>();
		}
		for (int num = teammates.Count - 1; num >= 0; num--)
		{
			if (teammates[num] == null || !teammates[num].enabled)
			{
				teammates.RemoveAt(num);
			}
		}
		for (int i = 0; i < 20; i++)
		{
			int index = Random.Range(0, LevelMgr.Inst.CurrentRoomCtrller.abyssPoints_Dots.Count);
			AbyssPoint = LevelMgr.Inst.CurrentRoomCtrller.abyssPoints_Dots[index];
			bool flag = false;
			for (int j = 0; j < teammates.Count; j++)
			{
				if (teammates[j] != myPpt && teammates[j].AbyssPoint == AbyssPoint)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				break;
			}
		}
	}

	public void ShareDamage(TakeDamageInfo_Dots info)
	{
		info.knockbackForce = Vector3.zero;
		UnitDotsSyncSystem.AddTakeDamageRequest(myPpt.myEntity, info);
	}

	public override void BeforeAnnouncedDeath_Dots(ref TakeDamageInfo_Dots info)
	{
		if (pattern == AIPattern.Pattern1)
		{
			info.knockbackForce = Vector3.zero;
		}
		base.BeforeAnnouncedDeath_Dots(ref info);
	}

	public override void AfterDead(ref TakeDamageInfo_Dots info)
	{
		base.AfterDead(ref info);
		if (pattern == AIPattern.Pattern1)
		{
			eyeUnit.DotsAnnouncedDeath();
		}
	}
}
