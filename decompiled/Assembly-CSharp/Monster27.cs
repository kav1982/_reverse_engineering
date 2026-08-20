using System;
using System.Collections.Generic;
using UnityEngine;

public class Monster27 : UnitBase
{
	[Space(50f)]
	public Transform tsf_Motion;

	public float decelerationDistance;

	public float minSpeedRatio;

	public VariableFloat decelerationTimeToShoot;

	[Header("Attack")]
	public float attackMoveSpeedRatio;

	public float spellHeight;

	[Header("Wing")]
	public GameObject pfb_WingL;

	public GameObject pfb_WingR;

	[Header("身体跟随摆动")]
	public Transform bodyRoot;

	public float bodyAmplitude;

	public float bodySpeed;

	private float bodySeed;

	public float sinOffset;

	[Header("Pattern2")]
	public AIPattern pattern;

	public GameObject pfb_Wing2L;

	public GameObject pfb_Wing2R;

	[Header("Pattern3，4")]
	public float aroundSpeedFix;

	public float attackInterval;

	private float attackTimer;

	public float aroundRatioExtra;

	public float moveAngle;

	private float isClockwise;

	private Vector3 noTargetPoint;

	[Header("Audio")]
	public AudioSource as_Attack;

	private Monster27_Wing[] wings;

	private float decelerationTimer;

	private bool isAttacking;

	[Header("和谐模式")]
	public List<Sprite> sprites;

	public List<Sprite> sprites_H;

	public MeshRenderer MR;

	private SpellSpawnParams ssp;

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
		as_Attack.volume = DataMgr.settingData.GetFinalSound();
	}

	public override void SingleInitialCallback()
	{
		float timeSeed = UnityEngine.Random.Range(0f, MathF.PI * 2f);
		if (pattern == AIPattern.Pattern1 || pattern == AIPattern.Pattern3)
		{
			wings = new Monster27_Wing[2];
			wings[0] = UnityEngine.Object.Instantiate(pfb_WingL, base.transform).GetComponent<Monster27_Wing>();
			wings[1] = UnityEngine.Object.Instantiate(pfb_WingR, base.transform).GetComponent<Monster27_Wing>();
			wings[0].SingleInitial(this, Tool2D.GetDir(90f), isLeft: true, timeSeed);
			wings[1].SingleInitial(this, Tool2D.GetDir(-90f), isLeft: false, timeSeed);
			ssp = UnitDotsSyncSystem.GetSpellPrototype(90041);
		}
		else if (pattern == AIPattern.Pattern2 || pattern == AIPattern.Pattern4)
		{
			wings = new Monster27_Wing[6];
			wings[0] = UnityEngine.Object.Instantiate(pfb_WingL, base.transform).GetComponent<Monster27_Wing>();
			wings[1] = UnityEngine.Object.Instantiate(pfb_Wing2L, base.transform).GetComponent<Monster27_Wing>();
			wings[2] = UnityEngine.Object.Instantiate(pfb_Wing2L, base.transform).GetComponent<Monster27_Wing>();
			wings[3] = UnityEngine.Object.Instantiate(pfb_WingR, base.transform).GetComponent<Monster27_Wing>();
			wings[4] = UnityEngine.Object.Instantiate(pfb_Wing2R, base.transform).GetComponent<Monster27_Wing>();
			wings[5] = UnityEngine.Object.Instantiate(pfb_Wing2R, base.transform).GetComponent<Monster27_Wing>();
			wings[0].SingleInitial(this, Tool2D.GetDir(90f), isLeft: true, timeSeed);
			wings[1].SingleInitial(this, Tool2D.GetDir(45f), isLeft: true, timeSeed);
			wings[2].SingleInitial(this, Tool2D.GetDir(135f), isLeft: true, timeSeed);
			wings[3].SingleInitial(this, Tool2D.GetDir(-90f), isLeft: false, timeSeed);
			wings[4].SingleInitial(this, Tool2D.GetDir(-45f), isLeft: false, timeSeed);
			wings[5].SingleInitial(this, Tool2D.GetDir(-135f), isLeft: false, timeSeed);
			ssp = UnitDotsSyncSystem.GetSpellPrototype(90042);
		}
		else
		{
			Debug.LogError(pattern);
		}
		bodySeed = timeSeed;
		if (GameMgr.IsHarmony_Static)
		{
			sprites = sprites_H;
			MR.material.SetTexture(GameConstManaged.shaderTextureIndex, sprites[0].texture);
		}
	}

	public override void EveryInitialCallback()
	{
		isClockwise = ((!(UnityEngine.Random.Range(0f, 1f) < 0.5f)) ? 1 : (-1));
		decelerationTimer = 0f;
		isAttacking = false;
		decelerationTimeToShoot.RandomResult();
		GetNearestTarget();
		for (int i = 0; i < wings.Length; i++)
		{
			wings[i].EveryInitial();
		}
		noTargetPoint = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint + Tool2D.GetDir() * UnityEngine.Random.Range(0f, LevelMgr.Inst.CurrentRoomCtrller.RoomScale.y / 2f);
		MR.material.SetTexture(GameConstManaged.shaderTextureIndex, sprites[0].texture);
	}

	public override void Update()
	{
		base.Update();
		if (base.IsLocked)
		{
			return;
		}
		bodyRoot.transform.localPosition = new Vector3(0f, bodyAmplitude * Mathf.Sin(Time.timeSinceLevelLoad * bodySpeed + bodySeed + sinOffset * (MathF.PI / 180f)), 0f);
		if (base.HaveTarget)
		{
			float num = 1f;
			float num2 = ToTargetDistanceSqr();
			float num3 = decelerationDistance * decelerationDistance;
			if (num2 < num3)
			{
				decelerationTimer += Time.deltaTime;
				num = Mathf.Lerp(minSpeedRatio, 1f, num2 / num3);
				if (decelerationTimer >= decelerationTimeToShoot.result)
				{
					if (pattern == AIPattern.Pattern3 || (pattern == AIPattern.Pattern4 && attackTimer > attackInterval))
					{
						attackTimer = 0f;
						decelerationTimer = 0f;
						base.Anima.SetTrigger("Attack");
						isAttacking = true;
					}
					else if (pattern == AIPattern.Pattern1 || pattern == AIPattern.Pattern2)
					{
						decelerationTimer = 0f;
						base.Anima.SetTrigger("Attack");
						isAttacking = true;
					}
				}
			}
			else
			{
				decelerationTimer = 0f;
			}
			Vector3 motion;
			if (pattern == AIPattern.Pattern3 || pattern == AIPattern.Pattern4)
			{
				attackTimer += Time.deltaTime;
				motion = ((!(num < minSpeedRatio + aroundRatioExtra)) ? (Tool2D.GetDir(ToTargetDir(), moveAngle * isClockwise) * base.MoveSpeed * num) : (Tool2D.GetDir(ToTargetDir(), 90f * isClockwise) * base.MoveSpeed * num * aroundSpeedFix));
			}
			else
			{
				motion = ToTargetDir() * base.MoveSpeed * num;
			}
			if (isAttacking)
			{
				motion *= attackMoveSpeedRatio;
			}
			SetMove(motion);
			return;
		}
		if ((double)(base.transform.position - noTargetPoint).sqrMagnitude < 2.25)
		{
			noTargetPoint = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint + Tool2D.GetDir() * UnityEngine.Random.Range(0f, LevelMgr.Inst.CurrentRoomCtrller.RoomScale.y / 2f);
		}
		else
		{
			Vector3 oldDir = noTargetPoint - base.transform.position;
			float num4 = 1f;
			float sqrMagnitude = (base.transform.position - noTargetPoint).sqrMagnitude;
			float num5 = decelerationDistance * decelerationDistance;
			if (sqrMagnitude < num5)
			{
				num4 = Mathf.Lerp(minSpeedRatio, 1f, sqrMagnitude / num5);
			}
			Vector3 motion2 = ((pattern != AIPattern.Pattern3 && pattern != AIPattern.Pattern4) ? (oldDir.normalized * base.MoveSpeed * num4) : ((!(num4 < minSpeedRatio + aroundRatioExtra)) ? (Tool2D.GetDir(oldDir, moveAngle * isClockwise).normalized * base.MoveSpeed * num4) : (Tool2D.GetDir(oldDir, 90f * isClockwise).normalized * base.MoveSpeed * num4 * aroundSpeedFix)));
			SetMove(motion2);
		}
		checkTargetIntervalTimer += Time.deltaTime;
		if (checkTargetIntervalTimer >= 1f)
		{
			GetNearestTarget();
		}
	}

	public override void AnimaAction(string animaName)
	{
		switch (animaName)
		{
		case "0":
			MR.material.SetTexture(GameConstManaged.shaderTextureIndex, sprites[0].texture);
			break;
		case "1":
			MR.material.SetTexture(GameConstManaged.shaderTextureIndex, sprites[1].texture);
			break;
		case "2":
			MR.material.SetTexture(GameConstManaged.shaderTextureIndex, sprites[2].texture);
			break;
		case "AttackSE":
			as_Attack.Play();
			break;
		case "Shoot":
		{
			Vector3 zero = Vector3.zero;
			zero = ((!base.HaveTarget) ? Tool2D.GetDir() : ToTargetDir());
			UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
			sSPModifier.Direction = zero;
			sSPModifier.Shooter = myPpt.myEntity;
			sSPModifier.SpawnPosition = base.transform.position + new Vector3(0f, 0f, 0f - spellHeight);
			sSPModifier.ApplyToSSP(ref ssp);
			ShootSpell(ssp);
			break;
		}
		case "AttackFinish":
			isAttacking = false;
			break;
		default:
			Debug.LogError(animaName);
			break;
		}
	}
}
