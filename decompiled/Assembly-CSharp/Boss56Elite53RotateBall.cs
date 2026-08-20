using System;
using Unity.Entities;
using UnityEngine;

public class Boss56Elite53RotateBall : MonoBehaviour
{
	public enum MonsterState
	{
		BornIdle,
		RandomMove,
		Charge,
		Attack
	}

	private StateVariableMgr varMgr = new StateVariableMgr();

	public MonsterState _state;

	private bool stateQuit;

	private bool changedState;

	private float stateExistTime;

	private Entity ShooterEntity;

	[Header("表现")]
	public Transform tsf_Model;

	public Transform tsf_Ball;

	public float ballScaleTime;

	public float floatFrequency;

	public float floatAmplitude;

	public ParticleSystem chargeParticle;

	public ParticleSystem attackParticle;

	[Header("攻击")]
	public float attackCDTime;

	public float attackChargeTime;

	public float attackAfterTime;

	[Header("旋转攻击")]
	public int rotateBulletCount;

	private bool rotateAttackClockwise;

	[Header("音效")]
	public AudioSource as_Charge;

	private Vector3 modelOriginLocalPos;

	private Vector3 ballOriginLocalScale;

	private float floatTimer;

	private float ballScaleRatio;

	private bool enableShoot;

	private float ballRotateSpeed;

	public MonsterState state
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

	public void ApplyOneShootCount(bool isClockWise, float rotateSpeed)
	{
		enableShoot = true;
		rotateAttackClockwise = isClockWise;
		ballRotateSpeed = rotateSpeed;
	}

	private void OnEnable()
	{
		EventMgr.SoundVolumeChange = (Action)Delegate.Combine(EventMgr.SoundVolumeChange, new Action(SoundVolumeChange));
		SoundVolumeChange();
		state = MonsterState.BornIdle;
		stateExistTime = 0f;
		floatTimer = 0f;
		ballScaleRatio = 1f;
		if (tsf_Model != null)
		{
			modelOriginLocalPos = tsf_Model.localPosition;
		}
		if (tsf_Ball != null)
		{
			tsf_Ball.localScale = ballOriginLocalScale;
		}
		ShooterEntity = Entity.Null;
		enableShoot = false;
	}

	private void OnDisable()
	{
		EventMgr.SoundVolumeChange = (Action)Delegate.Remove(EventMgr.SoundVolumeChange, new Action(SoundVolumeChange));
	}

	private void SoundVolumeChange()
	{
		as_Charge.volume = DataMgr.settingData.GetFinalSound();
	}

	public void InitializeData(Entity shooterEntity, bool isClockWiseRotate, float rotateSpeed)
	{
		ShooterEntity = shooterEntity;
		rotateAttackClockwise = isClockWiseRotate;
		enableShoot = true;
		stateExistTime = 0f;
		ballRotateSpeed = rotateSpeed;
	}

	public void Update()
	{
		if (ShooterEntity == Entity.Null)
		{
			return;
		}
		UpdateModelFloat();
		UpdateBallScale();
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
		case MonsterState.BornIdle:
			state = MonsterState.RandomMove;
			break;
		case MonsterState.RandomMove:
			if (stateExistTime > attackCDTime && enableShoot)
			{
				state = MonsterState.Charge;
				enableShoot = false;
			}
			break;
		case MonsterState.Charge:
			if (changedState)
			{
				chargeParticle.Play();
				as_Charge.Play();
			}
			if (stateExistTime > attackChargeTime)
			{
				state = MonsterState.Attack;
			}
			break;
		case MonsterState.Attack:
			if (changedState)
			{
				chargeParticle.Stop();
				attackParticle.Play();
				ShootRotateBullet();
				ballScaleRatio = 0f;
				if (tsf_Ball != null)
				{
					tsf_Ball.localScale = Vector3.zero;
				}
				as_Charge.Stop();
				SEMgr.Inst.elite53Shoot.PlaySE();
			}
			if (stateExistTime > attackAfterTime)
			{
				state = MonsterState.RandomMove;
			}
			break;
		}
	}

	private void UpdateModelFloat()
	{
		if (!(tsf_Model == null))
		{
			floatTimer += Time.deltaTime * MathF.PI * 2f * floatFrequency;
			if (floatTimer > MathF.PI * 2f)
			{
				floatTimer -= MathF.PI * 2f;
			}
			tsf_Model.localPosition = modelOriginLocalPos + new Vector3(0f, Mathf.Sin(floatTimer) * floatAmplitude, 0f);
		}
	}

	private void ShootRotateBullet()
	{
		rotateAttackClockwise = !rotateAttackClockwise;
		Vector3 dir = Tool2D.GetDir();
		float num = 360f / (float)rotateBulletCount;
		for (int i = 0; i < rotateBulletCount; i++)
		{
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Elite53_Bullet", Tool2D.IgnoreZPoint(base.transform.position, -0.9f)).GetComponent<Monster319_Bullet>().InitializeRotate(Tool2D.GetDir(dir, num * (float)i), ShooterEntity, buffed: false, rotateAttackClockwise, ballRotateSpeed);
		}
	}

	private void UpdateBallScale()
	{
		if (!(tsf_Ball == null) && !(ballScaleRatio >= 1f))
		{
			if (ballScaleTime <= 0f)
			{
				ballScaleRatio = 1f;
			}
			else
			{
				ballScaleRatio = Mathf.Min(1f, ballScaleRatio + Time.deltaTime / ballScaleTime);
			}
			tsf_Ball.localScale = ballOriginLocalScale * ballScaleRatio;
		}
	}
}
