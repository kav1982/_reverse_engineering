using System;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class Monster38_Ring : MonoBehaviour
{
	public enum RingState
	{
		Out,
		Chase,
		BackOut,
		Back
	}

	public float damageInterval;

	private float damageTimer;

	public int damage;

	public float radius;

	public float flySpeed;

	public float flyTime;

	public float spinSpeed;

	public Transform spin;

	public Transform spinShadow;

	public Vector3 moveDir;

	public Monster38 master;

	public float recycleRange;

	public float circleOffset;

	public bool recycled;

	public GameObject ringImage;

	public ParticleSystem ringEffect;

	public ParticleSystem fadeEffect;

	private ParticleSystem.MainModule mainModule;

	public float speedAcceleration;

	public float trueSpeed;

	public float recycleKnockBack;

	public AudioSource spinSound;

	public float chaseSpeedAccleration;

	public float chaseTime;

	private float chaseTimer;

	public float minChaseStopSpeed;

	private bool chaseSlowDown;

	private bool stuckInBorder;

	[Header("和谐模式")]
	public SpriteRenderer thisRenderer;

	public Sprite ringSprite_H;

	public ParticleSystem ringEffect_H;

	public ParticleSystem fadeEffect_H;

	public RingState state;

	private RingState tempState;

	private RingState preState;

	private bool changedState;

	private Vector3 roomCenterPoint;

	private float roomWidth;

	private float roomHeight;

	private bool isPattern2;

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

	public void Active(Vector3 moveDir, bool isPattern2 = false)
	{
		this.moveDir = moveDir;
		damageTimer = 0f;
		recycled = false;
		ringImage.SetActive(value: true);
		ringEffect.Play();
		trueSpeed = flySpeed;
		spinSound.Play();
		state = RingState.Out;
		this.isPattern2 = isPattern2;
		chaseTimer = chaseTime;
	}

	public void Mute()
	{
		spinSound.Stop();
		if (!recycled)
		{
			fadeEffect.Play();
		}
		recycled = true;
		ringImage.SetActive(value: false);
		ringEffect.Stop();
		SEMgr.Inst.monster38Recycle.PlaySE();
		base.enabled = false;
	}

	private void Start()
	{
		if (LevelMgr.Inst.CurrentRoomCtrller.roomCfg.themeType == RoomThemeType.Theme6_Chapter3 || LevelMgr.Inst.CurrentRoomCtrller.roomCfg.themeType == RoomThemeType.Theme22_Chapter3_Shortcut1)
		{
			roomCenterPoint = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint;
			roomWidth = LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme6Width;
			roomHeight = LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme6Height;
		}
		recycled = true;
		if (GameMgr.IsChAge14_Static)
		{
			thisRenderer.sprite = ringSprite_H;
			ringEffect = ringEffect_H;
			fadeEffect = fadeEffect_H;
		}
		ringImage.SetActive(value: false);
		ringEffect.Stop();
		damageTimer = 0f;
		mainModule = ringEffect.main;
		if (GameMgr.IsMobile_Static)
		{
			flySpeed *= 0.8f;
			trueSpeed = flySpeed;
		}
	}

	public void MasterRepositioned(Vector3 beforeRepositionPos)
	{
		if (state == RingState.Back)
		{
			state = RingState.BackOut;
			moveDir = (beforeRepositionPos - base.transform.position).normalized;
		}
	}

	private void Update()
	{
		spinShadow.localEulerAngles += new Vector3(0f, 0f, spinSpeed * Time.deltaTime);
		spin.localEulerAngles += new Vector3(0f, 0f, spinSpeed * Time.deltaTime);
		mainModule.startRotation = spin.localEulerAngles.z;
		changedState = false;
		preState = tempState;
		tempState = state;
		if (preState != state)
		{
			changedState = true;
		}
		switch (state)
		{
		case RingState.Out:
			_ = changedState;
			if (trueSpeed > 0f)
			{
				trueSpeed -= Time.deltaTime * speedAcceleration;
			}
			else if (isPattern2)
			{
				state = RingState.Chase;
				chaseTimer -= 1f;
			}
			else
			{
				state = RingState.Back;
			}
			if ((LevelMgr.Inst.CurrentRoomCtrller.roomCfg.themeType != RoomThemeType.Theme6_Chapter3 && LevelMgr.Inst.CurrentRoomCtrller.roomCfg.themeType != RoomThemeType.Theme22_Chapter3_Shortcut1) || (Mathf.Abs(base.transform.position.y - roomCenterPoint.y - circleOffset) < roomHeight / 2f && Mathf.Abs(base.transform.position.x - roomCenterPoint.x) < roomWidth / 2f))
			{
				base.transform.position += Time.deltaTime * trueSpeed * moveDir;
			}
			break;
		case RingState.Chase:
			if (changedState)
			{
				master.SetRingDiration(this);
				chaseSlowDown = false;
				stuckInBorder = false;
				if ((LevelMgr.Inst.CurrentRoomCtrller.roomCfg.themeType == RoomThemeType.Theme6_Chapter3 || LevelMgr.Inst.CurrentRoomCtrller.roomCfg.themeType == RoomThemeType.Theme22_Chapter3_Shortcut1) && (!(Mathf.Abs(base.transform.position.y - roomCenterPoint.y - circleOffset) < roomHeight / 2f) || !(Mathf.Abs(base.transform.position.x - roomCenterPoint.x) < roomWidth / 2f)))
				{
					stuckInBorder = true;
				}
			}
			if ((LevelMgr.Inst.CurrentRoomCtrller.roomCfg.themeType != RoomThemeType.Theme6_Chapter3 && LevelMgr.Inst.CurrentRoomCtrller.roomCfg.themeType != RoomThemeType.Theme22_Chapter3_Shortcut1) || (Mathf.Abs(base.transform.position.y - roomCenterPoint.y - circleOffset) < roomHeight / 2f && Mathf.Abs(base.transform.position.x - roomCenterPoint.x) < roomWidth / 2f && stuckInBorder))
			{
				stuckInBorder = false;
			}
			if (!chaseSlowDown)
			{
				if (trueSpeed < flySpeed)
				{
					trueSpeed += Time.deltaTime * chaseSpeedAccleration;
				}
			}
			else if (trueSpeed > 0f)
			{
				trueSpeed -= Time.deltaTime * chaseSpeedAccleration;
			}
			if (trueSpeed >= minChaseStopSpeed)
			{
				chaseSlowDown = true;
			}
			if (chaseSlowDown && trueSpeed <= 0f)
			{
				if (chaseTimer <= 0f)
				{
					state = RingState.Back;
				}
				else
				{
					master.SetRingDiration(this);
					chaseSlowDown = false;
					stuckInBorder = false;
					if (!(Mathf.Abs(base.transform.position.y - roomCenterPoint.y - circleOffset) < roomHeight / 2f) || !(Mathf.Abs(base.transform.position.x - roomCenterPoint.x) < roomWidth / 2f))
					{
						stuckInBorder = true;
					}
					chaseTimer -= 1f;
				}
			}
			if ((LevelMgr.Inst.CurrentRoomCtrller.roomCfg.themeType != RoomThemeType.Theme6_Chapter3 && LevelMgr.Inst.CurrentRoomCtrller.roomCfg.themeType != RoomThemeType.Theme22_Chapter3_Shortcut1) || (Mathf.Abs(base.transform.position.y - roomCenterPoint.y - circleOffset) < roomHeight / 2f && Mathf.Abs(base.transform.position.x - roomCenterPoint.x) < roomWidth / 2f) || stuckInBorder)
			{
				base.transform.position += Time.deltaTime * trueSpeed * moveDir;
			}
			break;
		case RingState.BackOut:
			_ = changedState;
			if (trueSpeed > 0f)
			{
				trueSpeed -= Time.deltaTime * speedAcceleration;
			}
			else
			{
				state = RingState.Back;
			}
			if ((LevelMgr.Inst.CurrentRoomCtrller.roomCfg.themeType != RoomThemeType.Theme6_Chapter3 && LevelMgr.Inst.CurrentRoomCtrller.roomCfg.themeType != RoomThemeType.Theme22_Chapter3_Shortcut1) || (Mathf.Abs(base.transform.position.y - roomCenterPoint.y - circleOffset) < roomHeight / 2f && Mathf.Abs(base.transform.position.x - roomCenterPoint.x) < roomWidth / 2f))
			{
				base.transform.position += Time.deltaTime * trueSpeed * moveDir;
			}
			else
			{
				state = RingState.Back;
			}
			break;
		case RingState.Back:
			if (changedState)
			{
				trueSpeed = 0f;
				moveDir = (master.transform.position - base.transform.position).normalized;
			}
			if (trueSpeed < flySpeed)
			{
				trueSpeed += Time.deltaTime * speedAcceleration;
			}
			base.transform.position += Time.deltaTime * (master.transform.position - base.transform.position).normalized * trueSpeed;
			if ((master.transform.position - base.transform.position).sqrMagnitude < recycleRange * recycleRange && !recycled)
			{
				recycled = true;
				ringImage.SetActive(value: false);
				ringEffect.Stop();
				fadeEffect.Play();
				spinSound.Stop();
				if (UnitDotsSyncSystem.EntityIsValid(master.myPpt.myEntity))
				{
					UnitProperty_Dots componentData = master.GetComponentData<UnitProperty_Dots>();
					componentData.TakeKnockback((master.transform.position - base.transform.position).normalized * recycleKnockBack);
					master.SetComponentData(componentData);
				}
				SEMgr.Inst.monster38Recycle.PlaySE();
			}
			break;
		}
		if (recycled)
		{
			return;
		}
		damageTimer += Time.deltaTime;
		if (!(damageTimer >= damageInterval))
		{
			return;
		}
		damageTimer = 0f;
		List<UnitDotsSyncSystem.DistanceHitResult> list = new List<UnitDotsSyncSystem.DistanceHitResult>();
		UnitDotsSyncSystem.GetCollidersInRange(base.transform.position, radius, GameConst.Filter_MonsterAoe, list);
		for (int i = 0; i < list.Count; i++)
		{
			UnitDotsSyncSystem.DistanceHitResult distanceHitResult = list[i];
			Entity entity = distanceHitResult.entity;
			switch (UnitDotsSyncSystem.GetLayer(entity))
			{
			case 16777216u:
			{
				UnitDotsSyncSystem.ProcessHitSpell(entity, damage, out var hitRollBall);
				if (hitRollBall)
				{
					if (!GameMgr.IsHarmony_Static)
					{
						ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Monster38_Hit", distanceHitResult.point, 2f);
					}
					else
					{
						ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_MonsterPunch", distanceHitResult.point, 2f);
					}
					SEMgr.Inst.spell3007Hit.PlaySE();
				}
				break;
			}
			case 512u:
			case 32768u:
			case 131072u:
			case 2097152u:
			{
				TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(master.myPpt.myEntity);
				info.damage = damage;
				info.teammateTakeDamageRatio = 2f;
				if (!GameMgr.IsHarmony_Static)
				{
					ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Monster38_Hit", distanceHitResult.point, 2f);
				}
				else
				{
					ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_MonsterPunch", distanceHitResult.point, 2f);
				}
				SEMgr.Inst.spell3007Hit.PlaySE();
				UnitDotsSyncSystem.AddTakeDamageRequest(distanceHitResult.entity, info);
				break;
			}
			}
		}
	}
}
