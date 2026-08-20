using System;
using UnityEngine;

public class Elite53 : UnitBase
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

	[Header("移动")]
	public VariableFloat randomMoveTime;

	public VariableFloat randomMoveRadius;

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

	public float attackAngleRange;

	public float attackChargeTime;

	public float attackAfterTime;

	[Header("旋转攻击")]
	public int rotateBulletCount;

	private bool rotateAttackClockwise;

	[Header("音效")]
	public AudioSource as_Charge;

	private UIEndlessEliteHpBar hpBar;

	private Vector3 modelOriginLocalPos;

	private Vector3 ballOriginLocalScale;

	private float floatTimer;

	private float ballScaleRatio;

	private bool isRotateAttack;

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

	public override void SingleInitialCallback()
	{
		hpBar = UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/UI/UIEndlessEliteHpBar"), myPpt.tsf_Layer.position + new Vector3(0f, myPpt.unitCfg.relicShowHPUIHight - 0.2f, 0f) * myPpt.tsf_Layer.lossyScale.y, Quaternion.identity, myPpt.tsf_Layer).GetComponent<UIEndlessEliteHpBar>();
		hpBar.Initialize(this);
		if (tsf_Ball != null)
		{
			ballOriginLocalScale = tsf_Ball.localScale;
		}
	}

	public override void EveryInitialCallback()
	{
		state = MonsterState.BornIdle;
		stateExistTime = 0f;
		hpBar.gameObject.SetActive(value: true);
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
		isRotateAttack = GeneralTool.ChanceResult(0.5f);
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
		as_Charge.volume = DataMgr.settingData.GetFinalSound();
	}

	public override void Update()
	{
		base.Update();
		if (base.IsLocked)
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
			_ = changedState;
			SetMove(Vector3.zero, isFlip: false);
			if (stateExistTime > 0.5f)
			{
				state = MonsterState.RandomMove;
			}
			break;
		case MonsterState.RandomMove:
			if (changedState)
			{
				randomMoveTime.RandomResult();
				randomMoveRadius.RandomResult();
				GetNavInfo(base.transform.position + Tool2D.GetDir() * randomMoveRadius.result);
			}
			if (navInfo.allCornerArrived)
			{
				randomMoveTime.RandomResult();
				randomMoveRadius.RandomResult();
				GetNavInfo(base.transform.position + Tool2D.GetDir() * randomMoveRadius.result);
			}
			else
			{
				SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed);
				CheckNavInfo();
			}
			if (stateExistTime > attackCDTime)
			{
				state = MonsterState.Charge;
			}
			break;
		case MonsterState.Charge:
			if (changedState)
			{
				chargeParticle.Play();
				as_Charge.Play();
			}
			if (navInfo.allCornerArrived)
			{
				randomMoveTime.RandomResult();
				randomMoveRadius.RandomResult();
				GetNavInfo(base.transform.position + Tool2D.GetDir() * randomMoveRadius.result);
			}
			else
			{
				SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed);
				CheckNavInfo();
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
				isRotateAttack = !isRotateAttack;
				if (isRotateAttack)
				{
					ShootRotateBullet();
				}
				else
				{
					ShootExplodeBullet();
				}
				ballScaleRatio = 0f;
				if (tsf_Ball != null)
				{
					tsf_Ball.localScale = Vector3.zero;
				}
				as_Charge.Stop();
				SEMgr.Inst.elite53Shoot.PlaySE();
			}
			if (navInfo.allCornerArrived)
			{
				randomMoveTime.RandomResult();
				randomMoveRadius.RandomResult();
				GetNavInfo(base.transform.position + Tool2D.GetDir() * randomMoveRadius.result);
			}
			else
			{
				SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed);
				CheckNavInfo();
			}
			if (stateExistTime > attackAfterTime)
			{
				state = MonsterState.RandomMove;
			}
			break;
		}
	}

	public override void AnimaAction(string animaName)
	{
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

	private void ShootExplodeBullet()
	{
		Vector3 position = base.transform.position;
		Vector3 oldDir = Tool2D.IgnoreZV2ToV1Normal(PlayerMgr.Inst.PlayerPoint, position);
		float num = attackAngleRange * 2f / 3f;
		for (int i = 0; i < 3; i++)
		{
			Monster319_Bullet component = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Elite53_ExplodeBullet", Tool2D.IgnoreZPoint(position, -1.2f)).GetComponent<Monster319_Bullet>();
			Vector3 dir = Tool2D.GetDir(oldDir, 0f - attackAngleRange + num * ((float)i + UnityEngine.Random.value));
			component.InitializeExplode(dir, myPpt.myEntity, buffed: false);
		}
	}

	private void ShootRotateBullet()
	{
		rotateAttackClockwise = !rotateAttackClockwise;
		Vector3 dir = Tool2D.GetDir();
		float num = 360f / (float)rotateBulletCount;
		for (int i = 0; i < rotateBulletCount; i++)
		{
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Elite53_Bullet", Tool2D.IgnoreZPoint(base.transform.position, -0.9f)).GetComponent<Monster319_Bullet>().InitializeRotate(Tool2D.GetDir(dir, num * (float)i), myPpt.myEntity, buffed: false, rotateAttackClockwise);
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
