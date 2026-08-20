using System;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityEngine.VFX;

public class Elite11_Laser : MonoBehaviour
{
	public enum LaserState
	{
		Stop,
		Wait,
		Attack
	}

	[Header("攻击模式")]
	public bool isBigLaser;

	[Header("伤害判定")]
	public int damage;

	public float checkInterval;

	public float laserWidth;

	public LayerMask attackLayer;

	private float checkIntervalTimer;

	public float damageInterval;

	public List<Entity> attackedEntities = new List<Entity>();

	private List<float> attackedEntitiesCd = new List<float>();

	[Header("预警")]
	public float laserWarningTime;

	public VariableFloat laserWarningAngle;

	public float warningMaxAlpha;

	public AnimationCurve warningRotateCurve;

	public AnimationCurve warningTransparencyCurve;

	private float nowAngle;

	private float deltaAngle;

	private float targetAngle;

	[Header("伤害")]
	public AnimationCurve laserWidthCurve;

	public AnimationCurve laserTransparencyCurve;

	public float laserExistTime;

	public float startDamageWidth;

	private float originLaserWidth;

	private float originShadowWidth;

	[Header("通用表现")]
	public float startDistance;

	public float endDistance;

	public float height;

	public ParticleSystem chargeParticle;

	public ParticleSystem laserParticle;

	public ParticleSystem chargeParticle_H;

	public ParticleSystem laserParticle_H;

	public float laserParticleStopPercent;

	public int lrPoints;

	public LineRenderer lr_Aim;

	public LineRenderer lr_Aim_H;

	public LineRenderer lr_AimShadow;

	public LineRenderer lr_Laser;

	public LineRenderer lr_Laser1;

	public LineRenderer lr_Laser_H;

	public LineRenderer lr_Laser1_H;

	public LineRenderer lr_LaserShadow;

	public float outLaserWidthRatio;

	public Transform tsf_Node1;

	public Transform tsf_Node1_H;

	public Transform tsf_Node2;

	public VisualEffect ve_Bubble;

	public int bubbleCountPerMeter;

	public ShockParam shock;

	[Header("循环音效")]
	public AudioSource as_laserLoop;

	public AudioSource as_LaserLoopBG;

	public LaserState _state;

	private bool stateQuit;

	private bool changedState;

	public StateVariableMgr varMgr = new StateVariableMgr();

	private float stateExistTime;

	public LaserState state
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

	private void Start()
	{
		ve_Bubble.transform.SetParent(LevelMgr.Inst.CurrentRoomT);
		ve_Bubble.transform.position = Vector3.zero;
		state = LaserState.Stop;
		originLaserWidth = lr_Laser.widthMultiplier;
		originShadowWidth = lr_LaserShadow.widthMultiplier;
	}

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
		if (isBigLaser)
		{
			as_laserLoop.volume = DataMgr.settingData.GetFinalSound();
			as_LaserLoopBG.volume = DataMgr.settingData.GetFinalSound();
		}
	}

	public void Initialize(Vector3 targetDir, float laserStartDistance)
	{
		startDistance = laserStartDistance;
		targetAngle = Tool2D.IgnoreZAngleWithSign(Vector3.up, targetDir);
		deltaAngle = laserWarningAngle.RandomResult();
		state = LaserState.Wait;
		if (GameMgr.IsChAge14_Static)
		{
			lr_Aim.enabled = false;
			lr_Laser.enabled = false;
			lr_Laser1.enabled = false;
			lr_Aim = lr_Aim_H;
			lr_Laser = lr_Laser_H;
			lr_Laser1 = lr_Laser1_H;
			chargeParticle = chargeParticle_H;
			laserParticle = laserParticle_H;
			tsf_Node1 = tsf_Node1_H;
		}
		lr_Aim.positionCount = lrPoints;
		lr_AimShadow.positionCount = lrPoints;
		lr_Laser.positionCount = lrPoints;
		lr_Laser1.positionCount = lrPoints;
		lr_LaserShadow.positionCount = lrPoints;
		attackedEntities.Clear();
		attackedEntitiesCd.Clear();
	}

	public void InitializeLarge(float angle, float waitTime, float attackTime, float laserStartDistance)
	{
		startDistance = laserStartDistance;
		laserWarningTime = waitTime;
		laserExistTime = attackTime;
		targetAngle = angle;
		nowAngle = angle;
		deltaAngle = 0f;
		state = LaserState.Wait;
		if (GameMgr.IsChAge14_Static)
		{
			lr_Aim.enabled = false;
			lr_Laser.enabled = false;
			lr_Laser1.enabled = false;
			lr_Aim = lr_Aim_H;
			lr_Laser = lr_Laser_H;
			lr_Laser1 = lr_Laser1_H;
			chargeParticle = chargeParticle_H;
			laserParticle = laserParticle_H;
			tsf_Node1 = tsf_Node1_H;
		}
		lr_Aim.positionCount = lrPoints;
		lr_AimShadow.positionCount = lrPoints;
		lr_Laser.positionCount = lrPoints;
		lr_Laser1.positionCount = lrPoints;
		lr_LaserShadow.positionCount = lrPoints;
		attackedEntities.Clear();
		attackedEntitiesCd.Clear();
	}

	private void Update()
	{
		for (int num = attackedEntitiesCd.Count - 1; num >= 0; num--)
		{
			attackedEntitiesCd[num] -= Time.deltaTime;
			if (attackedEntitiesCd[num] < 0f)
			{
				attackedEntitiesCd.RemoveAt(num);
				attackedEntities.RemoveAt(num);
			}
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
		case LaserState.Stop:
			if (changedState)
			{
				lr_Aim.enabled = false;
				lr_AimShadow.enabled = false;
				lr_Laser.enabled = false;
				lr_Laser1.enabled = false;
				lr_LaserShadow.enabled = false;
			}
			break;
		case LaserState.Wait:
		{
			ref float reference = ref varMgr.RegFloat(0);
			if (changedState)
			{
				if (isBigLaser)
				{
					chargeParticle.Play();
				}
				lr_Laser.enabled = false;
				lr_Laser1.enabled = false;
				lr_Aim.enabled = true;
				lr_AimShadow.enabled = true;
			}
			reference = Mathf.Lerp(deltaAngle, 0f, warningRotateCurve.Evaluate(stateExistTime / laserWarningTime));
			nowAngle = targetAngle + reference;
			lr_Aim.material.SetFloat("_Transparency", warningTransparencyCurve.Evaluate(stateExistTime / laserWarningTime));
			lr_AimShadow.material.SetFloat("_Transparency", warningTransparencyCurve.Evaluate(stateExistTime / laserWarningTime) * 0.4f);
			if (stateExistTime > laserWarningTime)
			{
				state = LaserState.Attack;
			}
			break;
		}
		case LaserState.Attack:
			if (changedState)
			{
				checkIntervalTimer = 0f;
				laserParticle.Play();
				lr_Laser.enabled = true;
				lr_Laser1.enabled = true;
				lr_Aim.enabled = false;
				lr_AimShadow.enabled = false;
				lr_LaserShadow.enabled = true;
				SEMgr.Inst.spell1011Shoot.PlaySE();
				if (isBigLaser)
				{
					as_laserLoop.Play();
					as_LaserLoopBG.Play();
					chargeParticle.Stop();
				}
				else
				{
					SEMgr.Inst.elite11LaserLoop.PlayLoopSE(laserExistTime);
				}
			}
			if (stateExistTime > laserExistTime)
			{
				if (isBigLaser)
				{
					as_laserLoop.Stop();
					as_LaserLoopBG.Stop();
					SEMgr.Inst.elite11BigLaserStop.PlaySE();
				}
				else
				{
					SEMgr.Inst.elite11LaserEnd.PlaySE();
				}
				state = LaserState.Stop;
				break;
			}
			lr_Laser.widthMultiplier = Mathf.Max(0f, laserWidthCurve.Evaluate(stateExistTime / laserExistTime)) * originLaserWidth;
			lr_Laser1.widthMultiplier = outLaserWidthRatio * Mathf.Max(0f, laserWidthCurve.Evaluate(stateExistTime / laserExistTime)) * originLaserWidth;
			lr_LaserShadow.widthMultiplier = Mathf.Max(0f, laserWidthCurve.Evaluate(stateExistTime / laserExistTime)) * originShadowWidth;
			if (laserWidthCurve.Evaluate(stateExistTime / laserExistTime) > startDamageWidth)
			{
				DealDamage();
			}
			if (laserParticle.isPlaying && laserParticleStopPercent < stateExistTime / laserExistTime)
			{
				laserParticle.Stop();
			}
			break;
		}
		Vector3 vector = Elite11.elite11Position + Tool2D.GetDir(Vector3.up, nowAngle) * startDistance;
		Vector3 vector2 = Elite11.elite11Position + Tool2D.GetDir(Vector3.up, nowAngle) * endDistance;
		for (int i = 0; i < lr_Aim.positionCount; i++)
		{
			lr_Aim.SetPosition(i, Tool2D.GetLayerPoint(Vector3.Lerp(vector, vector2, (float)i / (float)(lrPoints - 1)) + new Vector3(0f, 0f, 0f - height)));
			lr_AimShadow.SetPosition(i, Tool2D.GetLayerPoint(Vector3.Lerp(vector, vector2, (float)i / (float)(lrPoints - 1)), LayerCorrectType.Shadow));
			lr_Laser.SetPosition(i, Tool2D.GetLayerPoint(Vector3.Lerp(vector, vector2, (float)i / (float)(lrPoints - 1)) + new Vector3(0f, 0f, 0f - height)));
			lr_Laser1.SetPosition(i, Tool2D.GetLayerPoint(Vector3.Lerp(vector, vector2, (float)i / (float)(lrPoints - 1)) + new Vector3(0f, 0f, 0f - height)) + new Vector3(0f, 0f, 0.001f));
			lr_LaserShadow.SetPosition(i, Tool2D.GetLayerPoint(Vector3.Lerp(vector, vector2, (float)i / (float)(lrPoints - 1)), LayerCorrectType.Shadow));
		}
		tsf_Node1.position = Tool2D.GetLayerPoint(vector + new Vector3(0f, 0f, 0f - height));
		tsf_Node1.localEulerAngles = new Vector3(0f, 0f, nowAngle + 90f);
		tsf_Node2.position = Tool2D.GetLayerPoint(vector2);
	}

	public void SetAngle(float nowAngle)
	{
		targetAngle = nowAngle;
		this.nowAngle = nowAngle;
	}

	public void DealDamage()
	{
		checkIntervalTimer += Time.deltaTime;
		if (!(checkIntervalTimer >= checkInterval))
		{
			return;
		}
		checkIntervalTimer -= checkInterval;
		if (isBigLaser)
		{
			CamController.Inst.SetShock(shock);
		}
		UnitDotsSyncSystem.RayCastHitResult[] array = UnitDotsSyncSystem.SphereCastAll(Elite11.Inst.transform.position + Tool2D.GetDir(Vector3.up, nowAngle) * (startDistance - 1f), Tool2D.GetDir(Vector3.up, nowAngle), laserWidth, 30f, GameConst.Filter_Laser);
		for (int i = 0; i < array.Length; i++)
		{
			Entity entity = array[i].entity;
			if (!attackedEntities.Contains(entity))
			{
				attackedEntities.Add(entity);
				attackedEntitiesCd.Add(damageInterval);
				Elite11.MiniPool.GetGO("Prefabs/EF/EF_Elite11_LaserHit", array[i].point, 3f);
				TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(Elite11.Inst.myPpt.myEntity);
				if (isBigLaser)
				{
					info.ignorePlayerInvincibleFrame = true;
				}
				info.damage = damage;
				info.teammateTakeDamageRatio = 4f;
				UnitDotsSyncSystem.AddTakeDamageRequest(entity, info);
			}
		}
	}
}
