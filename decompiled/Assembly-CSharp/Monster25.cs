using System;
using UnityEngine;

public class Monster25 : UnitBase
{
	private enum UnitState
	{
		BornIdle,
		Roam,
		Attacking
	}

	[Space(50f)]
	public float rotateSpeed;

	public VariableFloat turnInterval;

	public float turnHalfAngleOffsetNonT6;

	[Header("Tentacle")]
	public GameObject pfb_Tentacle;

	public Transform tsf_Motion;

	public Transform tsf_TentacleParent;

	public int tentacleCount;

	public float tentacleAngle;

	[Header("Warning")]
	public LayerMask laserCheckLayer;

	public LineRenderer[] lr_Warnings;

	public LineRenderer[] lr_WarningShadows;

	[Header("Attack")]
	public GameObject[] go_ChargeEFs;

	public GameObject[] go_MuzzleEFs;

	public Transform[] tsf_ShootPoints;

	public Transform tsf_ShootPointParent;

	public VariableFloat attackInterval;

	public float attackRotateSpeed;

	public float attackSpeedRatio;

	public float attackBeforeTime;

	public float attackKnockback;

	[Header("Spell")]
	public float spellHeight;

	public int spellDamage;

	public float spellSpeed;

	[Header("Pattern2")]
	public AIPattern pattern;

	public int deadID;

	public int deadCount;

	[Header("Audio")]
	public AudioSource as_Charge;

	[Header("切图和和谐模式")]
	public Sprite sprite_Idle;

	public Sprite sprite_Attack;

	public Sprite sprite_Idle_H;

	public Sprite sprite_Attack_H;

	public MeshRenderer mr;

	public GameObject[] go_ChargeEFs_H;

	public GameObject[] go_MuzzleEFs_H;

	public LineRenderer[] lr_Warning_H;

	private Monster25_Tentacle[] tentacles;

	private UnitState state;

	private Vector3 roamDir;

	private float turnIntervalTimer;

	private float attackIntervalTimer;

	private float attackBeforeTimer;

	private SpellSpawnParams ssp;

	public Vector3 CurrentDir { get; private set; }

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
		as_Charge.volume = DataMgr.settingData.GetFinalSound();
	}

	private void SetSprite(bool isIdle)
	{
		Sprite sprite = ((!isIdle) ? (GameMgr.IsHarmony_Static ? sprite_Attack_H : sprite_Attack) : (GameMgr.IsHarmony_Static ? sprite_Idle_H : sprite_Idle));
		mr.material.SetTexture(GameConstManaged.shaderTextureIndex, sprite.texture);
	}

	public override void SingleInitialCallback()
	{
		tentacles = new Monster25_Tentacle[tentacleCount];
		for (int i = 0; i < tentacleCount; i++)
		{
			tentacles[i] = UnityEngine.Object.Instantiate(pfb_Tentacle, tsf_TentacleParent).GetComponent<Monster25_Tentacle>();
			tentacles[i].SingleInitial(this, (0f - tentacleAngle) / 2f + tentacleAngle / (float)(tentacleCount - 1) * (float)i);
		}
		if (pattern == AIPattern.Pattern3 || pattern == AIPattern.Pattern4)
		{
			ssp = UnitDotsSyncSystem.GetSpellPrototype(90032);
		}
		else
		{
			ssp = UnitDotsSyncSystem.GetSpellPrototype(90031);
		}
		UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
		sSPModifier.Damage = spellDamage;
		sSPModifier.Speed = spellDamage;
		sSPModifier.Shooter = myPpt.myEntity;
		sSPModifier.ApplyToSSP(ref ssp);
		if (GameMgr.IsHarmony_Static)
		{
			Vector3 localScale = mr.transform.localScale;
			localScale.x *= 1.2f;
			localScale.y *= 1.2f;
			mr.transform.localScale = localScale;
		}
	}

	public override void EveryInitialCallback()
	{
		state = UnitState.BornIdle;
		turnIntervalTimer = 0f;
		if (pattern == AIPattern.Pattern1)
		{
			attackIntervalTimer = UnityEngine.Random.Range(0f, attackInterval.value2 / 2f);
		}
		else
		{
			attackIntervalTimer = UnityEngine.Random.Range(0f, attackInterval.value2);
		}
		attackBeforeTimer = 0f;
		for (int i = 0; i < tentacles.Length; i++)
		{
			tentacles[i].EveryInitial();
		}
		CurrentDir = Tool2D.GetDir();
		CorrectRotation();
		attackInterval.RandomResult();
		SetSprite(isIdle: true);
		if (GameMgr.IsChAge14_Static)
		{
			go_ChargeEFs = go_ChargeEFs_H;
			go_MuzzleEFs = go_MuzzleEFs_H;
			lr_Warnings = lr_Warning_H;
		}
		for (int j = 0; j < go_ChargeEFs.Length; j++)
		{
			go_ChargeEFs[j].SetActive(value: false);
			lr_Warnings[j].gameObject.SetActive(value: false);
			lr_WarningShadows[j].gameObject.SetActive(value: false);
			go_MuzzleEFs[j].SetActive(value: false);
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
			SetMove(Vector3.zero);
			bornIdleTimer += Time.deltaTime;
			if (bornIdleTimer >= 0.5f)
			{
				state = UnitState.Roam;
				GetRoamDir();
			}
			break;
		case UnitState.Roam:
			CurrentDir = Tool2D.DirMoveTowards(CurrentDir, roamDir, rotateSpeed * Time.deltaTime);
			CorrectRotation();
			SetMove(CurrentDir * base.MoveSpeed);
			turnIntervalTimer += Time.deltaTime;
			if (turnIntervalTimer >= turnInterval.result)
			{
				turnIntervalTimer = 0f;
				turnInterval.RandomResult();
				GetRoamDir();
			}
			attackIntervalTimer += Time.deltaTime;
			if (attackIntervalTimer >= attackInterval.result)
			{
				attackIntervalTimer = 0f;
				attackInterval.RandomResult();
				GetNearestTarget();
				state = UnitState.Attacking;
				SetSprite(isIdle: false);
				for (int k = 0; k < go_ChargeEFs.Length; k++)
				{
					go_ChargeEFs[k].SetActive(value: true);
				}
				for (int l = 0; l < lr_Warnings.Length; l++)
				{
					lr_Warnings[l].gameObject.SetActive(value: true);
				}
				for (int m = 0; m < lr_WarningShadows.Length; m++)
				{
					lr_WarningShadows[m].gameObject.SetActive(value: true);
				}
				as_Charge.Play();
				for (int n = 0; n < lr_Warnings.Length; n++)
				{
					Vector3 vector3 = tsf_ShootPoints[n].position - tsf_ShootPointParent.localPosition;
					vector3.z = 0f - spellHeight;
					RaycastHit hitInfo2;
					Vector3 vector4 = ((!Physics.Raycast(vector3, tsf_ShootPoints[n].up, out hitInfo2, 100f, laserCheckLayer)) ? (vector3 + tsf_ShootPoints[n].up * 100f) : hitInfo2.point);
					lr_Warnings[n].SetPosition(0, Tool2D.GetLayerPoint(vector3));
					lr_Warnings[n].SetPosition(1, Tool2D.GetLayerPoint(vector4));
					lr_WarningShadows[n].SetPosition(0, Tool2D.IgnoreZPoint(vector3, 1.05f));
					lr_WarningShadows[n].SetPosition(1, Tool2D.IgnoreZPoint(vector4, 1.05f));
				}
			}
			break;
		case UnitState.Attacking:
			if (base.HaveTarget)
			{
				CurrentDir = Tool2D.DirMoveTowards(CurrentDir, ToTargetDir(), attackRotateSpeed * Time.deltaTime);
			}
			else
			{
				GetNearestTarget();
			}
			CorrectRotation();
			SetMove(CurrentDir * base.MoveSpeed * attackSpeedRatio);
			attackBeforeTimer += Time.deltaTime;
			if (attackBeforeTimer >= attackBeforeTime)
			{
				attackBeforeTimer = 0f;
				state = UnitState.Roam;
				GetRoamDir();
				SetSprite(isIdle: true);
				myPpt.TakeKnockback(-tsf_Motion.up * attackKnockback);
				UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
				for (int i = 0; i < tsf_ShootPoints.Length; i++)
				{
					Vector3 spawnPosition = tsf_ShootPoints[i].position - tsf_ShootPointParent.localPosition;
					spawnPosition.z = 0f - spellHeight;
					sSPModifier.Direction = tsf_ShootPoints[i].up;
					sSPModifier.SpawnPosition = spawnPosition;
					sSPModifier.ApplyToSSP(ref ssp);
					ShootSpell(ssp);
					go_ChargeEFs[i].SetActive(value: false);
					lr_Warnings[i].gameObject.SetActive(value: false);
					lr_WarningShadows[i].gameObject.SetActive(value: false);
					go_MuzzleEFs[i].SetActive(value: false);
					go_MuzzleEFs[i].SetActive(value: true);
				}
			}
			else
			{
				for (int j = 0; j < lr_Warnings.Length; j++)
				{
					Vector3 vector = tsf_ShootPoints[j].position - tsf_ShootPointParent.localPosition;
					vector.z = 0f - spellHeight;
					RaycastHit hitInfo;
					Vector3 vector2 = ((!Physics.Raycast(vector, tsf_ShootPoints[j].up, out hitInfo, 100f, laserCheckLayer)) ? (vector + tsf_ShootPoints[j].up * 100f) : hitInfo.point);
					lr_Warnings[j].SetPosition(0, Tool2D.GetLayerPoint(vector));
					lr_Warnings[j].SetPosition(1, Tool2D.GetLayerPoint(vector2));
					lr_WarningShadows[j].SetPosition(0, Tool2D.IgnoreZPoint(vector, 1.05f));
					lr_WarningShadows[j].SetPosition(1, Tool2D.IgnoreZPoint(vector2, 1.05f));
				}
			}
			break;
		default:
			Debug.LogError(state);
			break;
		}
	}

	private void GetRoamDir()
	{
		if (LevelMgr.Inst.CurrentRoomCfg.themeType == RoomThemeType.Theme6_Chapter3 || LevelMgr.Inst.CurrentRoomCtrller.roomCfg.themeType == RoomThemeType.Theme22_Chapter3_Shortcut1)
		{
			roamDir = Tool2D.GetDir();
			return;
		}
		roamDir = ToPointDir(PlayerMgr.Inst.PlayerPoint);
		roamDir = Tool2D.GetDir(roamDir, UnityEngine.Random.Range(0f - turnHalfAngleOffsetNonT6, turnHalfAngleOffsetNonT6));
	}

	private void CorrectRotation()
	{
		float z = Tool2D.IgnoreZAngleWithSign(Vector3.up, CurrentDir);
		tsf_Motion.localEulerAngles = new Vector3(0f, 0f, z);
		tsf_TentacleParent.localEulerAngles = new Vector3(0f, 0f, z);
		tsf_ShootPointParent.localEulerAngles = new Vector3(0f, 0f, z);
	}

	public void Summon(Vector3 summonDir)
	{
		state = UnitState.Roam;
		roamDir = summonDir;
	}

	public override void AfterDead(ref TakeDamageInfo_Dots info)
	{
		base.AfterDead(ref info);
		if (pattern == AIPattern.Pattern2 || pattern == AIPattern.Pattern4)
		{
			for (int i = 0; i < deadCount; i++)
			{
				ObjPoolMgr.Inst.GetGO("Prefabs/Units/" + deadID, base.transform.position).GetComponent<UnitProperty>().GetComponent<Monster25>()
					.Summon(Tool2D.GetDir(360f / (float)deadCount * (float)i));
			}
		}
	}

	public override void Theme6Reposition(Vector3 changeValue)
	{
		base.Theme6Reposition(changeValue);
		for (int i = 0; i < tentacles.Length; i++)
		{
			tentacles[i].Theme6Reposition(changeValue);
		}
	}
}
