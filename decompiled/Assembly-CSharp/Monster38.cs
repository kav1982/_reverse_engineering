using System;
using System.Collections.Generic;
using UnityEngine;

public class Monster38 : UnitBase
{
	private enum MonsterState
	{
		Reset,
		BornIdle,
		Attack,
		RandomFly
	}

	public Sprite eyeOpenSprite;

	public Sprite eyeCloseSprite;

	public MeshRenderer mr;

	private MonsterState state = MonsterState.BornIdle;

	private Vector3 roomCenterPoint;

	private float roomWidth;

	private float roomHeight;

	public VariableFloat aimCenterFix;

	public float anglesBetweenRings;

	public float randomFlyRadius;

	public float attackCheckInterval;

	public bool ringAllRecycled;

	public List<Monster38_Ring> rings = new List<Monster38_Ring>();

	public float ringAmount;

	public float attackRange;

	public float attackRecoil;

	public AIPattern pattern;

	public Transform imageTransform;

	public Transform imageShadowTransform;

	private bool spinSpeeding;

	private float trueSpinSpeed;

	public float normalSpinSpeed;

	public float attackSpinSpeed;

	public float spinAcceleration;

	public float spinDeacceleration;

	public float groupAttackInterval;

	public GameObject ringPrefab;

	public AudioSource spinSound;

	public VariableFloat eyeBlinkTime;

	private float eyeBlinkTimer;

	private MonsterState preState;

	private MonsterState tempState;

	private bool changedState;

	private void OnEnable()
	{
		EventMgr.SoundVolumeChange = (Action)Delegate.Combine(EventMgr.SoundVolumeChange, new Action(SoundChange));
		SoundChange();
	}

	private void OnDisable()
	{
		EventMgr.SoundVolumeChange = (Action)Delegate.Remove(EventMgr.SoundVolumeChange, new Action(SoundChange));
	}

	private void SoundChange()
	{
		spinSound.volume = DataMgr.settingData.GetFinalSound();
	}

	public override void SingleInitialCallback()
	{
	}

	public override void EveryInitialCallback()
	{
		rings.Clear();
		for (int i = 0; (float)i < ringAmount; i++)
		{
			Monster38_Ring component = UnityEngine.Object.Instantiate(ringPrefab, base.transform.position, Quaternion.identity, LevelMgr.Inst.CurrentRoomT).GetComponent<Monster38_Ring>();
			rings.Add(component);
			component.master = this;
		}
		if (LevelMgr.Inst.CurrentRoomCtrller.roomCfg.themeType == RoomThemeType.Theme6_Chapter3 || LevelMgr.Inst.CurrentRoomCtrller.roomCfg.themeType == RoomThemeType.Theme22_Chapter3_Shortcut1)
		{
			roomCenterPoint = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint;
			roomWidth = LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme6Width;
			roomHeight = LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme6Height;
		}
		trueSpinSpeed = normalSpinSpeed;
		spinSpeeding = false;
		state = MonsterState.Reset;
	}

	public override void Update()
	{
		ringAllRecycled = true;
		for (int i = 0; i < rings.Count; i++)
		{
			if (!rings[i].recycled)
			{
				ringAllRecycled = false;
			}
		}
		base.Update();
		if (base.IsLocked)
		{
			return;
		}
		imageTransform.localEulerAngles += new Vector3(0f, 0f, trueSpinSpeed * Time.deltaTime);
		imageShadowTransform.localEulerAngles += new Vector3(0f, 0f, trueSpinSpeed * Time.deltaTime);
		if (spinSpeeding && trueSpinSpeed < attackSpinSpeed)
		{
			trueSpinSpeed += spinAcceleration * Time.deltaTime;
		}
		else if (!spinSpeeding && trueSpinSpeed > normalSpinSpeed)
		{
			trueSpinSpeed -= spinDeacceleration * Time.deltaTime;
		}
		changedState = false;
		preState = tempState;
		tempState = state;
		if (preState != state)
		{
			changedState = true;
		}
		eyeBlinkTimer += Time.deltaTime;
		if (eyeBlinkTimer > eyeBlinkTime.result)
		{
			eyeBlinkTime.RandomResult();
			eyeBlinkTimer = 0f;
			base.Anima.Play("Monster38_Blink", 1, 0f);
		}
		switch (state)
		{
		case MonsterState.Reset:
			if (changedState)
			{
				base.Anima.Play("Monster38_Idle");
			}
			state = MonsterState.BornIdle;
			break;
		case MonsterState.BornIdle:
			if (changedState)
			{
				base.Anima.Play("Monster38_Idle");
				bornIdleTimer = 0f;
			}
			state = MonsterState.RandomFly;
			bornIdleTimer += Time.deltaTime;
			if (bornIdleTimer > 0.5f)
			{
				state = MonsterState.RandomFly;
			}
			break;
		case MonsterState.Attack:
			if (changedState)
			{
				base.Anima.Play("Monster38_Attack");
			}
			break;
		case MonsterState.RandomFly:
			if (changedState)
			{
				GetNavInfo(Tool2D.GetNavMeshPoint(base.transform.position, randomFlyRadius));
				base.Anima.Play("Monster38_Move");
			}
			if (navInfo.allCornerArrived)
			{
				GetNavInfo(Tool2D.GetNavMeshPoint(base.transform.position, randomFlyRadius));
			}
			else
			{
				SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed);
				CheckNavInfo();
			}
			checkTargetIntervalTimer += Time.deltaTime;
			if (checkTargetIntervalTimer >= attackCheckInterval)
			{
				checkTargetIntervalTimer = 0f;
				GetNearestTarget();
			}
			if (base.HaveTarget && ringAllRecycled && Time.time % groupAttackInterval < 0.1f)
			{
				state = MonsterState.Attack;
			}
			break;
		}
	}

	public bool CheckMissedTarget(Monster38_Ring ring)
	{
		if (!base.HaveTarget)
		{
			return true;
		}
		if (Vector3.Dot(ring.moveDir, base.TargetPointIgnoreZ - ring.transform.position) < 0f)
		{
			return true;
		}
		return false;
	}

	public void SetRingDiration(Monster38_Ring ring)
	{
		if (!base.HaveTarget)
		{
			if (Mathf.Abs(ring.transform.position.y - roomCenterPoint.y - ring.circleOffset) >= roomHeight / 2f)
			{
				ring.moveDir = new Vector3(ring.moveDir.x, 0f - ring.moveDir.y, 0f);
			}
			else if (Mathf.Abs(base.transform.position.x - roomCenterPoint.x) < roomWidth / 2f)
			{
				ring.moveDir = new Vector3(ring.moveDir.x, 0f - ring.moveDir.y, 0f);
			}
			else
			{
				ring.moveDir = Tool2D.GetDir();
			}
		}
		else
		{
			ring.moveDir = Tool2D.IgnoreZPoint(base.TargetPoint - ring.transform.position).normalized;
		}
	}

	public override void Theme6Reposition(Vector3 changeValue)
	{
		for (int i = 0; i < rings.Count; i++)
		{
			rings[i].MasterRepositioned(base.transform.position);
		}
		base.Theme6Reposition(changeValue);
	}

	public override void AfterDead(ref TakeDamageInfo_Dots info)
	{
		base.AfterDead(ref info);
		for (int i = 0; i < rings.Count; i++)
		{
			rings[i].Mute();
		}
	}

	public override void AnimaAction(string animaName)
	{
		switch (animaName)
		{
		case "SpeedUp":
			spinSound.Play();
			spinSpeeding = true;
			break;
		case "SlowDown":
			spinSound.Stop();
			spinSpeeding = false;
			break;
		case "Attack":
		{
			Vector3 oldDir = Tool2D.GetDir();
			GetNearestTarget();
			SEMgr.Inst.monster38Shoot.PlaySE();
			if (base.HaveTarget)
			{
				oldDir = ToTargetDir();
			}
			oldDir = Tool2D.GetDir(oldDir, aimCenterFix.RandomResult());
			UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
			componentData.TakeKnockback(-oldDir * attackRecoil);
			SetComponentData(componentData);
			for (int i = 0; i < rings.Count; i++)
			{
				rings[i].transform.position = base.transform.position;
				rings[i].Active(Tool2D.GetDir(oldDir, (float)(-(rings.Count - 1)) * anglesBetweenRings / 2f + anglesBetweenRings * (float)i), pattern == AIPattern.Pattern2);
			}
			break;
		}
		case "AttackFinish":
			state = MonsterState.RandomFly;
			break;
		case "EyeOpen":
			mr.material.SetTexture(GameConstManaged.shaderTextureIndex, eyeOpenSprite.texture);
			break;
		case "EyeClose":
			mr.material.SetTexture(GameConstManaged.shaderTextureIndex, eyeCloseSprite.texture);
			break;
		}
	}
}
