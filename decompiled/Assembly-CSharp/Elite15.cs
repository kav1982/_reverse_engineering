using System;
using System.Collections.Generic;
using Spine;
using UnityEngine;

public class Elite15 : UnitBase
{
	private enum UnitState
	{
		BornIdle,
		WakeUp,
		Blink,
		BlinkIdle,
		ActionIdle,
		Action1,
		Action2,
		Action2Loop,
		Action3,
		Action4,
		Stage2Change,
		Stage2Idle,
		Stage2Action1,
		Stage2Action2,
		Stage2Action3,
		Stage2Action3Idle,
		Stage2Action4,
		Stage2Action4Idle
	}

	private enum LastAction
	{
		None,
		Action1,
		Action2,
		Action3,
		Action4
	}

	private enum LastActionStage2
	{
		None,
		Action1,
		Action2,
		Action3,
		Action4
	}

	[Space(50f)]
	public Transform tsf_Model;

	public Shadow shadow;

	public VariableFloat actionIdleTime;

	[Header("Blink")]
	[Range(0f, 1f)]
	public float blinkChance;

	public float blinkRadiusFromCenter;

	public float blinkMinDistanceFromLast;

	[Header("Action1")]
	public Vector3 action1ShootOffset;

	public float action1ChargeFlySpeed;

	public float action1Knockback;

	[Header("Action2")]
	public Vector3 action2ShootOffset1;

	public Vector3 action2ShootOffset2;

	public float action2FullAngle;

	public float action2ShootInterval;

	public int action2ShootCount;

	public float bulletSpeed;

	[Header("Action3")]
	public int action3SummonID;

	public int action3SummonMaxCount;

	[Header("Action4")]
	public Vector3 action4ShootOffset;

	public float action4ChargeFlySpeed;

	public float action4Knockback;

	public float action4WarningRadius;

	[Header("Stage2Change")]
	[Range(0f, 1f)]
	public float stage2ChangeHPRatio;

	public float stage2ChangeMinTime;

	public float stage2ChangeDisThreshold;

	[Header("Stage2")]
	public float stage2BulletOffset;

	public float stage2BulletHeight;

	public float stage2BulletSpeed;

	[Header("Stage2Idle")]
	public VariableFloat stage2ActionIdleTime;

	[Header("Stage2Action1")]
	public float stage2Action1BulletInterval;

	public int stage2Action1BulletWave;

	public int stage2Action1BulletCount;

	[Header("Stage2Action2")]
	public float stage2Action2BulletInterval;

	public int stage2Action2BulletDirCount;

	public float stage2Action2RotateSpeed;

	public int stage2Action2RotateAccelerate;

	public float stage2Action2Duration;

	[Header("Stage2Action3")]
	public float stage2Action3BulletInterval;

	public int stage2Action3BulletWave;

	public int stage2Action3BulletCount;

	public float stage2Action3BulletSpeed;

	public VariableFloat stage2Action3BulletUpSpeed;

	public float stage2Action3BulletGravity;

	public float stage2Action3BulletBounceRatio;

	public float stage2Action3IdleTime;

	[Header("Stage2Action4")]
	public VariableFloat stage2Action4EggInterval;

	public int stage2Action4EggCount;

	public float stage2Action4IdleTime;

	public static Elite15 Inst;

	[Header("音效")]
	public AudioSource AS_SecondStageLoop;

	public VariableFloat shoutInterval;

	private float shoutTimer;

	private bool inSecondStage;

	private UnitState state;

	private LastAction lastAction;

	private LastActionStage2 lastActionStage2;

	private float actionIdleTimer;

	private Elite15_Action1Charge action1Charge;

	private Elite15_Action1Charge action4Charge;

	private int action2ShootCounter;

	private float action2ShootIntervalTimer;

	private float action2ShootAngleOffset;

	private bool action2ShootPoint1Clockwise;

	private bool action2ShootPoint2Clockwise;

	private List<Elite15_Child> childs = new List<Elite15_Child>();

	private bool isPlaySE;

	private WarningArea warningArea;

	private float stage2ChangeMinTimer;

	private float stage2ActionIdleTimer;

	private float stage2Action1BulletIntervalTimer;

	private int stage2Action1BulletWaveTimer;

	private float stage2Action2DirOffset;

	private float stage2Action2BulletIntervalTimer;

	private float stage2Action2DurationTimer;

	private float stage2Action2CurrentRotateSpeed;

	private bool stage2Action2Clockwise;

	private bool stage2Action2Reversed;

	private float stage2Action3BulletIntervalTimer;

	private int stage2Action3BulletWaveTimer;

	private float stage2Action3IdleTimer;

	private float stage2Action4EggIntervalTimer;

	private int stage2Action4EggCounter;

	private float stage2Action4IdleTimer;

	private MiniObjPool miniPool;

	private void OnEnable()
	{
		EventMgr.SoundVolumeChange = (Action)Delegate.Combine(EventMgr.SoundVolumeChange, new Action(SoundVolumeChange));
		SoundVolumeChange();
	}

	private void OnDisable()
	{
		EventMgr.SoundVolumeChange = (Action)Delegate.Remove(EventMgr.SoundVolumeChange, new Action(SoundVolumeChange));
	}

	private void SoundVolumeChange()
	{
		AS_SecondStageLoop.volume = DataMgr.settingData.GetFinalSound();
	}

	public override void SingleInitialCallback()
	{
		actionIdleTime.RandomResult();
		stage2ActionIdleTime.RandomResult();
		if (GameMgr.IsHarmony_Static)
		{
			base.SAnima.initialSkinName = base.SAnima.initialSkinName + "_HX";
			base.SAnima.Initialize(overwrite: true);
		}
		base.SAnima.AnimationState.Event += SAnimaEvent;
		if (GameMgr.IsMobile_Static)
		{
			stage2Action1BulletCount -= 3;
			stage2Action2RotateSpeed *= 0.8f;
			stage2Action3BulletInterval *= 1.15f;
			stage2Action3BulletWave = Mathf.CeilToInt((float)stage2Action3BulletWave * 0.85f);
			stage2Action4EggInterval.value1 *= 1.15f;
			stage2Action4EggInterval.value2 *= 1.15f;
			stage2Action4EggCount = Mathf.CeilToInt((float)stage2Action4EggCounter * 0.85f);
		}
	}

	public override void EveryInitialCallback()
	{
		Inst = this;
		action2ShootCounter = 0;
		action2ShootIntervalTimer = 0f;
		stage2ChangeMinTimer = 0f;
		stage2ActionIdleTimer = 0f;
		stage2Action1BulletIntervalTimer = 0f;
		stage2Action1BulletWaveTimer = 0;
		stage2Action2BulletIntervalTimer = 0f;
		stage2Action2DurationTimer = 0f;
		stage2Action2CurrentRotateSpeed = 0f;
		stage2Action2Clockwise = false;
		stage2Action2Reversed = false;
		stage2Action3BulletIntervalTimer = 0f;
		stage2Action3BulletWaveTimer = 0;
		stage2Action3IdleTimer = 0f;
		stage2Action4EggIntervalTimer = 0f;
		stage2Action4EggCounter = 0;
		stage2Action4IdleTimer = 0f;
		base.Rigid.isKinematic = false;
		SyncDotsRigidKindmatic();
		base.CC_Self.enabled = true;
		SetDotsCCEnable(isOpen: true);
		UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
		componentData.CanTouch = true;
		componentData.CanBeTarget = true;
		componentData.ChangeColor(myPpt.Color_NormalBody);
		SetComponentData(componentData);
		if (base.Anima != null)
		{
			base.Anima.speed = 1f;
		}
		if (base.SAnima != null)
		{
			base.SAnima.timeScale = 1f;
		}
		lastAction = LastAction.None;
		lastActionStage2 = LastActionStage2.None;
		miniPool = UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Mixed/MiniObjPool"), LevelMgr.Inst.CurrentRoomT).GetComponent<MiniObjPool>();
		actionIdleTime.RandomResult();
		stage2ActionIdleTime.RandomResult();
		action2ShootCounter = 0;
		action2ShootIntervalTimer = 0f;
		state = UnitState.BornIdle;
		base.SAnima.AnimationState.SetAnimation(0, "BornIdle", loop: false);
		inSecondStage = false;
	}

	private void SAnimaEvent(TrackEntry trackEntry, Spine.Event e)
	{
		if (base.deadStayed)
		{
			return;
		}
		switch (e.String)
		{
		case "WakeUpFinish":
			state = UnitState.Blink;
			base.SAnima.AnimationState.SetAnimation(0, "Blink", loop: false);
			break;
		case "ColliderClose":
		{
			base.CC_Self.enabled = false;
			SetDotsCCEnable(isOpen: false);
			UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
			componentData.CanTouch = false;
			SetComponentData(componentData);
			SEMgr.Inst.elite15BlinkUp.PlaySE();
			break;
		}
		case "BlinkChangePoint":
		{
			for (int k = 0; k < 10; k++)
			{
				Vector3 vector2 = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint + Tool2D.GetDir() * UnityEngine.Random.Range(0f, blinkRadiusFromCenter);
				if (Vector3.SqrMagnitude(vector2 - base.transform.position) > blinkMinDistanceFromLast * blinkMinDistanceFromLast)
				{
					base.transform.position = vector2;
					SyncDotsPosition();
					return;
				}
			}
			base.transform.position = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint + Tool2D.GetDir() * UnityEngine.Random.Range(0f, blinkRadiusFromCenter);
			SyncDotsPosition();
			break;
		}
		case "ColliderOpen":
		{
			base.CC_Self.enabled = true;
			SetDotsCCEnable(isOpen: true);
			UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
			componentData.CanTouch = true;
			SetComponentData(componentData);
			SEMgr.Inst.elite15BlinkDown.PlaySE();
			break;
		}
		case "BlinkFinish":
			state = UnitState.BlinkIdle;
			base.SAnima.AnimationState.SetAnimation(0, "Idle", loop: true);
			break;
		case "Action1Charge":
			action1Charge = miniPool.GetGO("Prefabs/EF/EF_Elite15_Action1Charge" + (GameMgr.IsHarmony_Static ? " H" : ""), GetAction1ShootRealPoint()).GetComponent<Elite15_Action1Charge>();
			action1Charge.Initialize(this, Elite15ActionType.Action1, miniPool);
			break;
		case "Action1ChargeShoot":
		{
			action1Charge.Shoot(action1ChargeFlySpeed);
			UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
			componentData.TakeKnockback(new Vector3(action1Knockback * (float)((tsf_Model.localScale.x != 1f) ? 1 : (-1)), 0f, 0f));
			SetComponentData(componentData);
			break;
		}
		case "Action1Finish":
			state = UnitState.ActionIdle;
			base.SAnima.AnimationState.SetAnimation(0, "Idle", loop: true);
			break;
		case "Action2Finish":
			state = UnitState.Action2Loop;
			base.SAnima.AnimationState.SetAnimation(0, "Action2Loop", loop: true);
			action2ShootAngleOffset = UnityEngine.Random.Range(0, 360);
			action2ShootPoint1Clockwise = UnityEngine.Random.Range(0, 2) == 0;
			action2ShootPoint2Clockwise = !action2ShootPoint1Clockwise;
			break;
		case "Action2EndFinish":
			base.SAnima.AnimationState.SetAnimation(0, "Idle", loop: true);
			break;
		case "Action3Summon":
		{
			Vector3 vector = Vector3.zero;
			for (int i = 0; i < 15; i++)
			{
				Vector2Data vector2Data = LevelMgr.Inst.CurrentRoomCfg.allTileList[0][UnityEngine.Random.Range(0, LevelMgr.Inst.CurrentRoomCfg.allTileList[0].Count)];
				vector = LevelMgr.Inst.CurrentRoomCtrller.transform.position + vector2Data.GetVector3();
				bool flag = true;
				for (int j = 0; j < childs.Count; j++)
				{
					if (vector == childs[j].transform.position)
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					break;
				}
			}
			Elite15_Child component = ObjPoolMgr.Inst.GetGO("Prefabs/Units/" + action3SummonID, vector).GetComponent<Elite15_Child>();
			component.SetMother(this);
			childs.Add(component);
			if (!isPlaySE)
			{
				isPlaySE = true;
				SEMgr.Inst.elite15Action3.PlaySE();
			}
			break;
		}
		case "Action3Finish":
			state = UnitState.ActionIdle;
			base.SAnima.AnimationState.SetAnimation(0, "Idle", loop: true);
			break;
		case "Action4Charge":
			action4Charge = miniPool.GetGO("Prefabs/EF/EF_Elite15_Action1Charge" + (GameMgr.IsHarmony_Static ? " H" : ""), GetAction1ShootRealPoint()).GetComponent<Elite15_Action1Charge>();
			action4Charge.Initialize(this, Elite15ActionType.Action4, miniPool);
			GetNearestTarget();
			if (base.HaveTarget)
			{
				warningArea = ObjPoolMgr.Inst.GetGO("Prefabs/Mixed/WarningArea_Circle" + (GameMgr.IsHarmony_Static ? " Purple" : ""), base.TargetPointIgnoreZ).GetComponent<WarningArea>();
				warningArea.Initialize(action4WarningRadius, 1f, zoomDirect: false);
			}
			break;
		case "Action4ChargeShoot":
		{
			Vector3 zero = Vector3.zero;
			zero = ((!base.HaveTarget && !(warningArea != null)) ? Tool2D.GetNavMeshPointIngoreZ(PlayerMgr.Inst.PlayerPoint + Tool2D.GetDir() * UnityEngine.Random.Range(0.5f, 2f)) : warningArea.transform.position);
			action4Charge.Shoot(action4ChargeFlySpeed, zero);
			if (base.transform.position.x < zero.x)
			{
				tsf_Model.localScale = Vector3.one;
			}
			else
			{
				tsf_Model.localScale = new Vector3(-1f, 1f, 1f);
			}
			UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
			componentData.TakeKnockback(new Vector3(action4Knockback * (float)((tsf_Model.localScale.x != 1f) ? 1 : (-1)), 0f, 0f));
			SetComponentData(componentData);
			float duration = (GetAction4ShootRealPoint() - zero).magnitude / action4ChargeFlySpeed;
			if (warningArea == null)
			{
				ObjPoolMgr.Inst.GetGO("Prefabs/Mixed/WarningArea_Circle" + (GameMgr.IsHarmony_Static ? " Purple" : ""), zero).GetComponent<WarningArea>().Initialize(action4WarningRadius, duration);
				break;
			}
			warningArea.BeginZoom(duration);
			warningArea = null;
			break;
		}
		case "Action4Finish":
			state = UnitState.ActionIdle;
			base.SAnima.AnimationState.SetAnimation(0, "Idle", loop: true);
			break;
		default:
			Debug.LogError(e.String);
			break;
		}
	}

	public override void Update()
	{
		base.Update();
		if (base.IsLocked)
		{
			return;
		}
		if (inSecondStage && !base.deadStayed && state != UnitState.ActionIdle)
		{
			shoutTimer += Time.deltaTime;
			if (shoutTimer > shoutInterval.result)
			{
				shoutTimer = 0f;
				shoutInterval.RandomResult();
				SEMgr.Inst.elite15Shout.PlaySE().pitch = UnityEngine.Random.Range(0.8f, 1f);
			}
		}
		switch (state)
		{
		case UnitState.BornIdle:
		case UnitState.WakeUp:
			SetMove(Vector3.zero);
			break;
		case UnitState.Blink:
			SetMove(Vector3.zero);
			break;
		case UnitState.BlinkIdle:
			SetMove(Vector3.zero);
			actionIdleTimer += Time.deltaTime;
			if (actionIdleTimer >= actionIdleTime.result)
			{
				actionIdleTimer = 0f;
				actionIdleTime.RandomResult();
				CheckAction(considerBlink: false);
			}
			break;
		case UnitState.ActionIdle:
			SetMove(Vector3.zero);
			actionIdleTimer += Time.deltaTime;
			if (actionIdleTimer >= actionIdleTime.result)
			{
				actionIdleTimer = 0f;
				actionIdleTime.RandomResult();
				CheckAction(considerBlink: true);
			}
			break;
		case UnitState.Action1:
			SetMove(Vector3.zero);
			break;
		case UnitState.Action2:
			SetMove(Vector3.zero);
			break;
		case UnitState.Action2Loop:
			SetMove(Vector3.zero);
			action2ShootIntervalTimer += Time.deltaTime;
			if (action2ShootIntervalTimer >= action2ShootInterval)
			{
				action2ShootIntervalTimer = 0f;
				Vector3 initialForce = Tool2D.GetDir(action2ShootAngleOffset + (float)(action2ShootPoint1Clockwise ? 1 : (-1)) * action2FullAngle / (float)action2ShootCount * (float)action2ShootCounter) * bulletSpeed;
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Elite15_Bullet" + (GameMgr.IsHarmony_Static ? " H" : ""), GetAction2ShootRealPoint1()).GetComponent<Elite15_Bullet>().Initialize(initialForce);
				initialForce = Tool2D.GetDir(action2ShootAngleOffset + (float)(action2ShootPoint2Clockwise ? 1 : (-1)) * action2FullAngle / (float)action2ShootCount * (float)action2ShootCounter) * bulletSpeed;
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Elite15_Bullet" + (GameMgr.IsHarmony_Static ? " H" : ""), GetAction2ShootRealPoint2()).GetComponent<Elite15_Bullet>().Initialize(initialForce);
				action2ShootCounter++;
				if (action2ShootCounter >= action2ShootCount)
				{
					action2ShootCounter = 0;
					state = UnitState.ActionIdle;
					base.SAnima.AnimationState.SetAnimation(0, "Action2End", loop: false);
				}
				SEMgr.Inst.elite15Action2SHoot.PlaySE(SEPlayMode.Replay, 3, 0.2f);
			}
			break;
		case UnitState.Action3:
			SetMove(Vector3.zero);
			break;
		case UnitState.Action4:
			SetMove(Vector3.zero);
			if (base.HaveTarget && warningArea != null)
			{
				warningArea.transform.position = base.TargetPointIgnoreZ;
			}
			break;
		case UnitState.Stage2Change:
			SetMove(ToPointDir(LevelMgr.Inst.CurrentRoomCtrller.CenterPoint) * base.MoveSpeed);
			stage2ChangeMinTimer += Time.deltaTime;
			if (stage2ChangeMinTimer > 2f && (ToPointDistanceSqr(LevelMgr.Inst.CurrentRoomCtrller.CenterPoint) < stage2ChangeDisThreshold * stage2ChangeDisThreshold || stage2ChangeMinTimer > stage2ChangeMinTime))
			{
				state = UnitState.Stage2Idle;
				base.SAnima.AnimationState.SetAnimation(0, "Stage2ChangeEnd", loop: true);
			}
			break;
		case UnitState.Stage2Idle:
			SetMove(Vector3.zero);
			stage2ActionIdleTimer += Time.deltaTime;
			if (stage2ActionIdleTimer >= stage2ActionIdleTime.result)
			{
				stage2ActionIdleTimer = 0f;
				stage2ActionIdleTime.RandomResult();
				CheckActionStage2();
			}
			break;
		case UnitState.Stage2Action1:
			SetMove(Vector3.zero);
			stage2Action1BulletIntervalTimer += Time.deltaTime;
			if (stage2Action1BulletIntervalTimer >= stage2Action1BulletInterval)
			{
				stage2Action1BulletIntervalTimer = 0f;
				float num = UnityEngine.Random.Range(0f, 360f);
				for (int m = 0; m < stage2Action1BulletCount; m++)
				{
					Vector3 dir3 = Tool2D.GetDir(num + 360f / (float)stage2Action1BulletCount * (float)m);
					ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Elite15_Bullet" + (GameMgr.IsHarmony_Static ? " H" : ""), base.transform.position + new Vector3(0f, 0f, 0f - stage2BulletHeight) + dir3 * stage2BulletOffset).GetComponent<Elite15_Bullet>().Initialize(dir3 * stage2BulletSpeed);
				}
				stage2Action1BulletWaveTimer++;
				if (stage2Action1BulletWaveTimer >= stage2Action1BulletWave)
				{
					stage2Action1BulletWaveTimer = 0;
					state = UnitState.Stage2Idle;
				}
			}
			break;
		case UnitState.Stage2Action2:
			SetMove(Vector3.zero);
			stage2Action2CurrentRotateSpeed = Mathf.MoveTowards(stage2Action2CurrentRotateSpeed, stage2Action2RotateSpeed, (float)stage2Action2RotateAccelerate * Time.deltaTime);
			if (!stage2Action2Reversed && stage2Action2DurationTimer >= stage2Action2Duration / 2f)
			{
				stage2Action2Reversed = true;
			}
			if (stage2Action2Clockwise)
			{
				if (stage2Action2Reversed)
				{
					stage2Action2DirOffset += stage2Action2CurrentRotateSpeed * Time.deltaTime;
				}
				else
				{
					stage2Action2DirOffset -= stage2Action2CurrentRotateSpeed * Time.deltaTime;
				}
			}
			else if (stage2Action2Reversed)
			{
				stage2Action2DirOffset -= stage2Action2CurrentRotateSpeed * Time.deltaTime;
			}
			else
			{
				stage2Action2DirOffset += stage2Action2CurrentRotateSpeed * Time.deltaTime;
			}
			stage2Action2BulletIntervalTimer += Time.deltaTime;
			if (stage2Action2BulletIntervalTimer >= stage2Action2BulletInterval)
			{
				stage2Action2BulletIntervalTimer = 0f;
				for (int l = 0; l < stage2Action2BulletDirCount; l++)
				{
					Vector3 dir2 = Tool2D.GetDir(360f / (float)stage2Action2BulletDirCount * (float)l + stage2Action2DirOffset);
					ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Elite15_Bullet" + (GameMgr.IsHarmony_Static ? " H" : ""), base.transform.position + new Vector3(0f, 0f, 0f - stage2BulletHeight) + dir2 * stage2BulletOffset).GetComponent<Elite15_Bullet>().Initialize(dir2 * stage2BulletSpeed);
				}
			}
			stage2Action2DurationTimer += Time.deltaTime;
			if (stage2Action2DurationTimer >= stage2Action2Duration)
			{
				stage2Action2DurationTimer = 0f;
				state = UnitState.Stage2Idle;
			}
			break;
		case UnitState.Stage2Action3:
			SetMove(Vector3.zero);
			stage2Action3BulletIntervalTimer += Time.deltaTime;
			if (stage2Action3BulletIntervalTimer >= stage2Action3BulletInterval)
			{
				stage2Action3BulletIntervalTimer = 0f;
				stage2Action3BulletUpSpeed.RandomResult();
				for (int k = 0; k < stage2Action3BulletCount; k++)
				{
					Vector3 dir = Tool2D.GetDir(360f / (float)stage2Action3BulletCount * (float)k);
					ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Elite15_Bullet" + (GameMgr.IsHarmony_Static ? " H" : ""), base.transform.position + new Vector3(0f, 0f, 0f - stage2BulletHeight) + dir * stage2BulletOffset).GetComponent<Elite15_Bullet>().Initialize(dir * stage2Action3BulletSpeed, stage2Action3BulletUpSpeed.result, stage2Action3BulletGravity, stage2Action3BulletBounceRatio);
				}
				stage2Action3BulletWaveTimer++;
				if (stage2Action3BulletWaveTimer >= stage2Action3BulletWave)
				{
					stage2Action3BulletWaveTimer = 0;
					state = UnitState.Stage2Action3Idle;
				}
			}
			break;
		case UnitState.Stage2Action3Idle:
			SetMove(Vector3.zero);
			stage2Action3IdleTimer += Time.deltaTime;
			if (stage2Action3IdleTimer >= stage2Action3IdleTime)
			{
				stage2Action3IdleTimer = 0f;
				state = UnitState.Stage2Idle;
			}
			break;
		case UnitState.Stage2Action4:
		{
			stage2Action4EggIntervalTimer += Time.deltaTime;
			if (!(stage2Action4EggIntervalTimer >= stage2Action4EggInterval.result))
			{
				break;
			}
			stage2Action4EggIntervalTimer = 0f;
			stage2Action4EggInterval.RandomResult();
			Vector3 vector = Vector3.zero;
			for (int i = 0; i < 15; i++)
			{
				Vector2Data vector2Data = LevelMgr.Inst.CurrentRoomCfg.allTileList[0][UnityEngine.Random.Range(0, LevelMgr.Inst.CurrentRoomCfg.allTileList[0].Count)];
				vector = LevelMgr.Inst.CurrentRoomCtrller.transform.position + vector2Data.GetVector3();
				bool flag = true;
				for (int j = 0; j < childs.Count; j++)
				{
					if (vector == childs[j].transform.position)
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					break;
				}
			}
			Elite15_Child component = ObjPoolMgr.Inst.GetGO("Prefabs/Units/" + action3SummonID, vector).GetComponent<Elite15_Child>();
			component.SetMother(this);
			component.SetExlosion();
			childs.Add(component);
			stage2Action4EggCounter++;
			if (stage2Action4EggCounter >= stage2Action4EggCount)
			{
				stage2Action4EggCounter = 0;
				state = UnitState.Stage2Action4Idle;
			}
			break;
		}
		case UnitState.Stage2Action4Idle:
			SetMove(Vector3.zero);
			stage2Action4IdleTimer += Time.deltaTime;
			if (stage2Action4IdleTimer >= stage2Action4IdleTime)
			{
				stage2Action4IdleTimer = 0f;
				state = UnitState.Stage2Idle;
			}
			break;
		default:
			Debug.LogError(state);
			break;
		}
	}

	private void CheckAction(bool considerBlink)
	{
		if (base.CurrentHPRatio <= stage2ChangeHPRatio)
		{
			inSecondStage = true;
			SEMgr.Inst.elite15BigShout.PlaySE();
			AS_SecondStageLoop.Play();
			state = UnitState.Stage2Change;
			base.SAnima.AnimationState.SetAnimation(0, "Stage2Change", loop: true);
			UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
			componentData.unitCfg.knockbackRatio = 0f;
			SetComponentData(componentData);
			return;
		}
		if (considerBlink)
		{
			if (UnityEngine.Random.value <= blinkChance)
			{
				state = UnitState.Blink;
				base.SAnima.AnimationState.SetAnimation(0, "Blink", loop: false);
				return;
			}
			if (Vector3.SqrMagnitude(LevelMgr.Inst.CurrentRoomCtrller.CenterPoint - base.transform.position) > blinkRadiusFromCenter * blinkRadiusFromCenter)
			{
				state = UnitState.Blink;
				base.SAnima.AnimationState.SetAnimation(0, "Blink", loop: false);
				return;
			}
		}
		List<LastAction> list = new List<LastAction>
		{
			LastAction.Action1,
			LastAction.Action2,
			LastAction.Action3,
			LastAction.Action4
		};
		if (list.Contains(LastAction.Action3) && childs.Count >= action3SummonMaxCount)
		{
			list.Remove(LastAction.Action3);
		}
		if (lastAction != 0)
		{
			list.Remove(lastAction);
		}
		lastAction = list[UnityEngine.Random.Range(0, list.Count)];
		switch (lastAction)
		{
		case LastAction.Action1:
			state = UnitState.Action1;
			if (base.transform.position.x < LevelMgr.Inst.CurrentRoomCtrller.CenterPoint.x)
			{
				tsf_Model.localScale = Vector3.one;
			}
			else
			{
				tsf_Model.localScale = new Vector3(-1f, 1f, 1f);
			}
			base.SAnima.AnimationState.SetAnimation(0, "Action1", loop: false);
			break;
		case LastAction.Action2:
			state = UnitState.Action2;
			base.SAnima.AnimationState.SetAnimation(0, "Action2", loop: false);
			break;
		case LastAction.Action3:
			state = UnitState.Action3;
			base.SAnima.AnimationState.SetAnimation(0, "Action3", loop: false);
			isPlaySE = false;
			break;
		case LastAction.Action4:
			state = UnitState.Action4;
			base.SAnima.AnimationState.SetAnimation(0, "Action4", loop: false);
			GetNearestTargetPlayerFirst();
			if (base.HaveTarget)
			{
				if (base.transform.position.x < base.TargetPoint.x)
				{
					tsf_Model.localScale = Vector3.one;
				}
				else
				{
					tsf_Model.localScale = new Vector3(-1f, 1f, 1f);
				}
			}
			break;
		default:
			Debug.LogError(lastAction);
			break;
		}
	}

	private void CheckActionStage2()
	{
		List<LastActionStage2> list = new List<LastActionStage2>
		{
			LastActionStage2.Action1,
			LastActionStage2.Action2,
			LastActionStage2.Action3,
			LastActionStage2.Action4
		};
		if (lastActionStage2 == LastActionStage2.None)
		{
			list.Remove(LastActionStage2.Action3);
			list.Remove(LastActionStage2.Action4);
		}
		else
		{
			list.Remove(lastActionStage2);
		}
		lastActionStage2 = list[UnityEngine.Random.Range(0, list.Count)];
		switch (lastActionStage2)
		{
		case LastActionStage2.Action1:
			state = UnitState.Stage2Action1;
			break;
		case LastActionStage2.Action2:
			state = UnitState.Stage2Action2;
			stage2Action2DirOffset = UnityEngine.Random.Range(0f, 360f);
			stage2Action2Clockwise = UnityEngine.Random.Range(0, 2) == 0;
			stage2Action2Reversed = false;
			stage2Action2CurrentRotateSpeed = 0f;
			break;
		case LastActionStage2.Action3:
			state = UnitState.Stage2Action3;
			break;
		case LastActionStage2.Action4:
			state = UnitState.Stage2Action4;
			stage2Action4EggInterval.RandomResult();
			break;
		default:
			Debug.LogError(lastActionStage2);
			break;
		}
	}

	public override void AnimaAction(string animaName)
	{
	}

	public override void AfterTakeDamage_Dots(ref TakeDamageInfo_Dots info)
	{
		if (state == UnitState.BornIdle)
		{
			state = UnitState.WakeUp;
			base.SAnima.AnimationState.SetAnimation(0, "WakeUp", loop: false);
		}
	}

	protected override void BossDeadStay()
	{
		SEMgr.Inst.elite15BigShout.PlaySE();
		base.Anima.Play("Die");
		base.SAnima.timeScale = 0f;
		base.enabled = false;
		base.Rigid.isKinematic = true;
		SyncDotsRigidKindmatic();
		base.CC_Self.enabled = false;
		SetDotsCCEnable(isOpen: false);
		myPpt.enabled = false;
		UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
		componentData.BossDeadStay();
		SetComponentData(componentData);
		GetComponent<BossDeadCreateEffect>().CreateEffect();
		if (action1Charge != null && action1Charge.gameObject.activeSelf)
		{
			action1Charge.Mute();
		}
		if (action4Charge != null && action4Charge.gameObject.activeSelf)
		{
			action4Charge.Mute();
		}
	}

	public override void AfterDead(ref TakeDamageInfo_Dots info)
	{
		UnityEngine.Object.Destroy(miniPool.gameObject);
		base.AfterDead(ref info);
		for (int num = childs.Count - 1; num >= 0; num--)
		{
			childs[num].DeadNow();
		}
	}

	public Vector3 GetAction1ShootRealPoint()
	{
		if (tsf_Model.transform.localScale.x > 0f)
		{
			return base.transform.position + action1ShootOffset;
		}
		return base.transform.position + new Vector3(0f - action1ShootOffset.x, action1ShootOffset.y, action1ShootOffset.z);
	}

	public Vector3 GetAction2ShootRealPoint1()
	{
		if (tsf_Model.transform.localScale.x > 0f)
		{
			return base.transform.position + action2ShootOffset1;
		}
		return base.transform.position + new Vector3(0f - action2ShootOffset1.x, action2ShootOffset1.y, action2ShootOffset1.z);
	}

	public Vector3 GetAction2ShootRealPoint2()
	{
		if (tsf_Model.transform.localScale.x > 0f)
		{
			return base.transform.position + action2ShootOffset2;
		}
		return base.transform.position + new Vector3(0f - action2ShootOffset2.x, action2ShootOffset2.y, action2ShootOffset2.z);
	}

	public Vector3 GetAction4ShootRealPoint()
	{
		if (tsf_Model.transform.localScale.x > 0f)
		{
			return base.transform.position + action4ShootOffset;
		}
		return base.transform.position + new Vector3(0f - action4ShootOffset.x, action4ShootOffset.y, action4ShootOffset.z);
	}

	public void ChildUnregister(Elite15_Child child)
	{
		childs.Remove(child);
	}
}
