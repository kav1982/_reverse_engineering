using System;
using System.Collections.Generic;
using UnityEngine;

public class Boss3_Stage2 : UnitBase
{
	private enum UnitState
	{
		BornIdle,
		Idle,
		BlinkWarning,
		Blink,
		AttackWarning,
		Attack,
		Attack2Before,
		Attack2,
		AttackIdle
	}

	[Header("Shadow")]
	public Shadow shadow;

	public Transform shadowReferTsf;

	public Vector2 shadowScaleRemapIn;

	public Vector2 shadowScaleRemapOut;

	[Header("Performance")]
	public float suspensionSpeed;

	public float suspensionDistance;

	public GameObject go_Move;

	public GameObject go_RT;

	public GameObject go_MR;

	public float fireAudioGrowSpeed;

	[Header("Blink")]
	public Animator anima_Body;

	public VariableFloat idleTime;

	public VariableFloat blinkRadius;

	public float blinkSideOffset;

	public float blinkLerpSpeed;

	public int bornForceBlinkCount;

	public float attackForceBlinkRadius;

	public int moveWarningNodes;

	public float warningLineHeight;

	public LineRenderer lr_moveWarning;

	public LineRenderer lr_moveWarning_H;

	public float blinkWarningTime;

	private float blinkWarningTimer;

	[Header("Attack")]
	public GameObject pfb_Laser;

	public int laserintialCount;

	public LayerMask attackLayer;

	public LayerMask laserStopMask;

	public VariableInt blinkCountToAttack;

	public float laserOffset;

	public float laserHeight;

	public float laserMaxLength;

	public float laserWidth;

	public ShockParam laserShock;

	public float damageInterval;

	public int damage;

	public float summonDamageRatio;

	public float attackWarningTime;

	private float attackWarningCheckInterval;

	public float attackTime;

	public float attackIdleTime;

	[Range(0f, 1f)]
	public float stage3HPRatio;

	[Range(0f, 1f)]
	public float stage2HPRatio;

	public int attack1Stage1Count;

	public int attack1Stage2Count;

	public int attack1Stage3Count;

	public float attack1Stage1RotateSpeed;

	public float attack1Stage2RotateSpeed;

	public float attack1Stage3RotateSpeed;

	[Range(0f, 1f)]
	public float attack2Chance;

	public GameObject pfb_Ball;

	public int ballInitailCount;

	public int attack2Stage1Count;

	public int attack2Stage2Count;

	public int attack2Stage3Count;

	public float attack2BallHeight;

	public float attack2ForwardOffset;

	public float attack2UpSpeed;

	public float attack2Gravity;

	public float attack2LaserHeight;

	[Header("Audio")]
	public AudioSource as_Fire;

	public AudioSource as_Born;

	public AudioSource as_Fly;

	public AudioSource as_LaserLoop;

	public AudioSource as_LaserEnd;

	public AudioSource as_Vomit;

	[Header("手游版粒子防卡顿处理")]
	public GameObject particle_VFX;

	public GameObject particle_Normal;

	private UnitState state;

	private float suspensionTimer;

	private float fireAudioTimer;

	private float idleTimer;

	private Vector3 blinkBeforePoint;

	private Vector3 blinkPoint;

	private Vector3 blinkSidePoint;

	private float blinkLerpTimer;

	private int blinkCounter;

	private List<Boss3Laser> lasers = new List<Boss3Laser>();

	private UnitDotsSyncSystem.RayCastHitResult raycastHit;

	private float damageIntervalTimer;

	private float attackTimer;

	private float attackIdleTimer;

	private int attack1UseCount;

	private bool attack1RotateLR = true;

	private float attack1RotateSpeed;

	private float attack1RotateAngle;

	private List<Boss3_Stage2_Ball> balls = new List<Boss3_Stage2_Ball>();

	private int attack2BallUseCount;

	private int attack2LaserUseCount;

	[Header("和谐模式")]
	public List<AnimationClip> harmonyAnimations = new List<AnimationClip>();

	public MeshRenderer mr;

	public Material material_H;

	public GameObject pfb_ball_H;

	private void OnEnable()
	{
		EventMgr.SoundVolumeChange = (Action)Delegate.Combine(EventMgr.SoundVolumeChange, new Action(SoundVolumeChange));
	}

	private void OnDisable()
	{
		EventMgr.SoundVolumeChange = (Action)Delegate.Remove(EventMgr.SoundVolumeChange, new Action(SoundVolumeChange));
	}

	private void SoundVolumeChange()
	{
		as_Born.volume = DataMgr.settingData.GetFinalSound();
		as_Fly.volume = DataMgr.settingData.GetFinalSound();
		as_LaserLoop.volume = DataMgr.settingData.GetFinalSound();
		as_LaserEnd.volume = DataMgr.settingData.GetFinalSound();
		as_Vomit.volume = DataMgr.settingData.GetFinalSound();
	}

	public override void SingleInitialCallback()
	{
		if (GameMgr.IsMobile_Static)
		{
			attack2Gravity *= 0.8f;
			attack1RotateSpeed *= 0.8f;
		}
	}

	public override void EveryInitialCallback()
	{
		if (GameMgr.IsMobile_Static)
		{
			particle_VFX.SetActive(value: false);
			particle_Normal.SetActive(value: true);
		}
		else
		{
			particle_VFX.SetActive(value: true);
			particle_Normal.SetActive(value: false);
		}
		go_RT.transform.parent = base.transform.parent;
		go_MR.transform.parent = base.transform.parent;
		go_RT.transform.position = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint;
		go_MR.transform.position = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint;
		go_Move.transform.position = Tool2D.IgnoreZPoint(Tool2D.GetLayerPoint(base.transform.position), go_Move.transform.position.z);
		go_MR.transform.position = new Vector3(go_MR.transform.position.x, go_MR.transform.position.y, Tool2D.GetLayerPoint(base.transform).z);
		idleTime.RandomResult();
		for (int i = 0; i < laserintialCount; i++)
		{
			lasers.Add(UnityEngine.Object.Instantiate(pfb_Laser, base.transform.parent).GetComponent<Boss3Laser>());
		}
		for (int j = 0; j < ballInitailCount; j++)
		{
			if (GameMgr.IsHarmony_Static)
			{
				balls.Add(UnityEngine.Object.Instantiate(pfb_ball_H, Vector3.zero, Quaternion.identity, base.transform.parent).GetComponent<Boss3_Stage2_Ball>());
			}
			else
			{
				balls.Add(UnityEngine.Object.Instantiate(pfb_Ball, Vector3.zero, Quaternion.identity, base.transform.parent).GetComponent<Boss3_Stage2_Ball>());
			}
		}
		blinkCountToAttack.result = bornForceBlinkCount;
		base.CC_Self.enabled = false;
		SetDotsCCEnable(isOpen: false);
		as_Fire.volume = 0f;
		SoundVolumeChange();
		if (GameMgr.IsMobile_Static)
		{
			attack2Gravity *= 0.8f;
			attack1RotateSpeed *= 0.8f;
		}
		if (GameMgr.IsHarmony_Static)
		{
			AnimatorOverrideController animatorOverrideController = new AnimatorOverrideController(base.Anima.runtimeAnimatorController);
			base.Anima.runtimeAnimatorController = animatorOverrideController;
			for (int k = 0; k < harmonyAnimations.Count; k++)
			{
				string text = harmonyAnimations[k].name.Substring(0, harmonyAnimations[k].name.Length - 2);
				if (animatorOverrideController[text] != null)
				{
					animatorOverrideController[text] = harmonyAnimations[k];
				}
			}
			UnityEngine.Object.Destroy(mr.material);
			mr.material = material_H;
			lr_moveWarning.enabled = false;
			lr_moveWarning = lr_moveWarning_H;
		}
		lr_moveWarning.positionCount = moveWarningNodes;
		lr_moveWarning.enabled = false;
		base.transform.position = Tool2D.IgnoreZPoint(base.transform, -0.5f);
		SyncDotsPosition();
		GameUISingletonMono<UIBossHP>.HideIfInited();
		UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
		componentData.unitCfg.unitType = UnitType.Boss;
		componentData.InvincibleRegister();
		componentData.CanTouch = false;
		SetComponentData(componentData);
	}

	public override void Update()
	{
		go_Move.transform.position = Tool2D.IgnoreZPoint(Tool2D.GetLayerPoint(base.transform.position), go_Move.transform.position.z);
		go_MR.transform.position = new Vector3(go_MR.transform.position.x, go_MR.transform.position.y, Tool2D.GetLayerPoint(base.transform).z);
		if (fireAudioTimer != 1f)
		{
			fireAudioTimer += fireAudioGrowSpeed * Time.deltaTime;
			if (fireAudioTimer > 1f)
			{
				fireAudioTimer = 1f;
			}
			as_Fire.volume = DataMgr.settingData.GetFinalSound() * fireAudioTimer;
		}
		if (state == UnitState.Attack || state == UnitState.Attack2)
		{
			bool flag = false;
			damageIntervalTimer += Time.deltaTime;
			if (damageIntervalTimer >= damageInterval)
			{
				damageIntervalTimer = 0f;
				flag = true;
				CamController.Inst.SetShock(laserShock.radius, laserShock.speed, damageInterval);
			}
			if (state == UnitState.Attack)
			{
				for (int i = 0; i < attack1UseCount; i++)
				{
					Vector3 dir = Tool2D.GetDir(attack1RotateAngle + (float)(360 / attack1UseCount * i));
					Vector3 vector = Tool2D.IgnoreZPoint(base.transform.position, 0f - laserHeight) + dir * laserOffset;
					Vector3 vector2 = vector + dir * laserMaxLength;
					if (UnitDotsSyncSystem.Raycast(vector, dir, laserMaxLength, GameConst.Filter_Wall, out raycastHit))
					{
						vector2 = raycastHit.point;
					}
					lasers[i].SetLaser(vector, vector2);
					if (!flag)
					{
						continue;
					}
					UnitDotsSyncSystem.RayCastHitResult[] array = UnitDotsSyncSystem.SphereCastAll(vector, dir, laserWidth, Vector3.Distance(vector, vector2), GameConst.Filter_MonsterAoeNoSpell);
					for (int j = 0; j < array.Length; j++)
					{
						if (EntityIsValid(array[j].entity))
						{
							UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>(array[j].entity);
							TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(myPpt.myEntity);
							info.damage = damage;
							if (componentData.unitCfg.unitType != 0)
							{
								info.damage *= summonDamageRatio;
							}
							UnitDotsSyncSystem.AddTakeDamageRequest(array[j].entity, info);
						}
					}
				}
			}
			else
			{
				int num = 0;
				for (int k = 0; k < attack2BallUseCount - 1; k++)
				{
					Vector3 vector3 = Tool2D.IgnoreZPoint(balls[k].transform.position, 0f - attack2LaserHeight);
					for (int l = k + 1; l < attack2BallUseCount; l++)
					{
						Vector3 vector4 = Tool2D.IgnoreZPoint(balls[l].transform.position, 0f - attack2LaserHeight);
						lasers[num].SetLaser(vector3, vector4);
						num++;
						if (!flag)
						{
							continue;
						}
						UnitDotsSyncSystem.RayCastHitResult[] array2 = UnitDotsSyncSystem.SphereCastAll(vector3, vector4 - vector3, laserWidth, Vector3.Distance(vector3, vector4), GameConst.Filter_MonsterAoeNoSpell);
						for (int m = 0; m < array2.Length; m++)
						{
							if (EntityIsValid(array2[m].entity))
							{
								UnitProperty_Dots componentData2 = GetComponentData<UnitProperty_Dots>(array2[m].entity);
								TakeDamageInfo_Dots info2 = TakeDamageInfo_Dots.NewInfo(myPpt.myEntity);
								info2.damage = damage;
								if (componentData2.unitCfg.unitType != 0)
								{
									info2.damage *= summonDamageRatio;
								}
								UnitDotsSyncSystem.AddTakeDamageRequest(array2[m].entity, info2);
							}
						}
					}
				}
			}
		}
		base.Update();
		if (base.IsLocked)
		{
			return;
		}
		float num2 = 0f;
		if (state != 0)
		{
			suspensionTimer += Time.deltaTime * suspensionSpeed;
			num2 = (Mathf.Cos(suspensionTimer) / 2f + 0.5f) * suspensionDistance;
			base.transform.position = Tool2D.IgnoreZPoint(base.transform, 0f - num2);
		}
		switch (state)
		{
		case UnitState.BornIdle:
			SetMove(Vector3.zero);
			shadow.ShadowGO.transform.localScale = Vector3.one * GeneralTool.Remap(shadowScaleRemapIn, shadowScaleRemapOut, shadowReferTsf.localScale.x);
			break;
		case UnitState.Idle:
			SetMove(Vector3.zero);
			idleTimer += Time.deltaTime;
			if (idleTimer >= idleTime.result)
			{
				idleTimer = 0f;
				idleTime.RandomResult();
				Blink();
			}
			break;
		case UnitState.BlinkWarning:
			SetMove(Vector3.zero);
			blinkWarningTimer += Time.deltaTime;
			if (blinkWarningTimer > blinkWarningTime)
			{
				blinkWarningTimer = 0f;
				state = UnitState.Blink;
				as_Fly.Play();
			}
			break;
		case UnitState.Blink:
			blinkLerpTimer += Time.deltaTime * blinkLerpSpeed;
			base.transform.position = Tool2D.IgnoreZPoint(GeneralTool.QuadraticBezierCurve(blinkBeforePoint, blinkSidePoint, blinkPoint, blinkLerpTimer), 0f - num2);
			SyncDotsPosition();
			if (!(blinkLerpTimer >= 1f))
			{
				break;
			}
			blinkLerpTimer = 0f;
			blinkCounter++;
			lr_moveWarning.enabled = false;
			if (blinkCounter >= blinkCountToAttack.result)
			{
				blinkCounter = 0;
				blinkCountToAttack.RandomResult();
				if (UnityEngine.Random.value <= attack2Chance)
				{
					state = UnitState.Attack2Before;
					base.Anima.SetTrigger("Attack2");
					break;
				}
				state = UnitState.AttackWarning;
				attack1UseCount = attack1Stage1Count;
				attack1RotateSpeed = attack1Stage1RotateSpeed;
				if (base.CurrentHPRatio <= stage3HPRatio)
				{
					attack1UseCount = attack1Stage3Count;
					attack1RotateSpeed = attack1Stage3RotateSpeed;
				}
				else if (base.CurrentHPRatio <= stage2HPRatio)
				{
					attack1UseCount = attack1Stage2Count;
					attack1RotateSpeed = attack1Stage2RotateSpeed;
				}
				attack1RotateAngle = UnityEngine.Random.Range(0, 360);
				attack1RotateLR = ((UnityEngine.Random.Range(0, 2) == 0) ? true : false);
				for (int num4 = 0; num4 < attack1UseCount; num4++)
				{
					Vector3 dir3 = Tool2D.GetDir(attack1RotateAngle + (float)(360 / attack1UseCount * num4));
					Vector3 vector6 = Tool2D.IgnoreZPoint(base.transform.position, 0f - laserHeight) + dir3 * laserOffset;
					Vector3 point2 = vector6 + dir3 * laserMaxLength;
					if (UnitDotsSyncSystem.Raycast(vector6, dir3, laserMaxLength, GameConst.Filter_Wall, out raycastHit))
					{
						point2 = raycastHit.point;
					}
					lasers[num4].SetWarning(vector6, point2);
				}
			}
			else
			{
				state = UnitState.Idle;
			}
			break;
		case UnitState.AttackWarning:
			attackTimer += Time.deltaTime;
			attackWarningCheckInterval += Time.deltaTime;
			if (attackWarningCheckInterval > damageInterval)
			{
				attackWarningCheckInterval = 0f;
				for (int num3 = 0; num3 < attack1UseCount; num3++)
				{
					Vector3 dir2 = Tool2D.GetDir(attack1RotateAngle + (float)(360 / attack1UseCount * num3));
					Vector3 vector5 = Tool2D.IgnoreZPoint(base.transform.position, 0f - laserHeight) + dir2 * laserOffset;
					Vector3 point = vector5 + dir2 * laserMaxLength;
					if (UnitDotsSyncSystem.Raycast(vector5, dir2, laserMaxLength, GameConst.Filter_Wall, out raycastHit))
					{
						point = raycastHit.point;
					}
					lasers[num3].SetWarning(vector5, point);
				}
			}
			if (attackTimer >= attackWarningTime)
			{
				attackTimer = 0f;
				state = UnitState.Attack;
				base.Anima.SetTrigger("Attack");
				anima_Body.SetTrigger("Attack");
				as_LaserLoop.Play();
			}
			break;
		case UnitState.Attack:
			attack1RotateAngle += (attack1RotateLR ? attack1RotateSpeed : (0f - attack1RotateSpeed)) * Time.deltaTime;
			attackTimer += Time.deltaTime;
			if (attackTimer >= attackTime)
			{
				attackTimer = 0f;
				state = UnitState.AttackIdle;
				as_LaserLoop.Stop();
				as_LaserEnd.Play();
				base.Anima.SetTrigger("Idle");
				anima_Body.SetTrigger("Idle");
				for (int num5 = 0; num5 < attack1UseCount; num5++)
				{
					lasers[num5].Stop();
				}
			}
			break;
		case UnitState.Attack2:
			attackTimer += Time.deltaTime;
			if (attackTimer >= attackTime)
			{
				attackTimer = 0f;
				as_LaserLoop.Stop();
				as_LaserEnd.Play();
				anima_Body.SetTrigger("Idle");
				for (int n = 0; n < attack2LaserUseCount; n++)
				{
					lasers[n].Stop();
				}
				Blink();
			}
			break;
		case UnitState.AttackIdle:
			attackIdleTimer += Time.deltaTime;
			if (attackIdleTimer >= attackIdleTime)
			{
				attackIdleTimer = 0f;
				Blink();
			}
			break;
		default:
			Debug.LogError(state);
			break;
		case UnitState.Attack2Before:
			break;
		}
	}

	private void Blink()
	{
		state = UnitState.BlinkWarning;
		blinkBeforePoint = Tool2D.IgnoreZPoint(base.transform.position);
		if (blinkCounter == blinkCountToAttack.result - 1)
		{
			for (int i = 0; i < 15; i++)
			{
				blinkPoint = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint + Tool2D.GetDir() * UnityEngine.Random.Range(0f, attackForceBlinkRadius);
				if ((blinkBeforePoint - blinkPoint).sqrMagnitude > blinkRadius.value1 * blinkRadius.value1 && (blinkBeforePoint - blinkPoint).sqrMagnitude < blinkRadius.value2 * blinkRadius.value2)
				{
					break;
				}
			}
		}
		else
		{
			blinkPoint = Tool2D.GetNavMeshPointIngoreZ(base.transform.position, blinkRadius);
		}
		blinkSidePoint = (blinkBeforePoint + blinkPoint) / 2f + Tool2D.GetDir((blinkBeforePoint - blinkPoint).normalized, (UnityEngine.Random.Range(0, 2) == 0) ? 90 : (-90)) * blinkSideOffset;
		for (int j = 0; j < moveWarningNodes; j++)
		{
			lr_moveWarning.SetPosition(j, Tool2D.GetLayerPoint(GeneralTool.QuadraticBezierCurve(blinkBeforePoint, blinkSidePoint, blinkPoint, (float)j / (float)moveWarningNodes), LayerCorrectType.GroundEffect));
		}
		lr_moveWarning.enabled = true;
	}

	public void BallLand()
	{
		if (state == UnitState.Attack2Before)
		{
			state = UnitState.Attack2;
			as_LaserLoop.Play();
			for (int i = 0; i < attack2BallUseCount; i++)
			{
				balls[i].SetLand();
			}
		}
	}

	public override void AnimaAction(string animaName)
	{
		switch (animaName)
		{
		case "BornSE":
			as_Born.Play();
			GameUISingletonMono<UIBossHP>.ShowInit(myPpt.myEntity);
			base.CC_Self.enabled = true;
			SetDotsCCEnable(isOpen: true);
			break;
		case "AppearFinish":
		{
			UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
			componentData.CanTouch = true;
			componentData.unitCfg.unitType = UnitType.Boss;
			componentData.InvincibleUnregister();
			SetComponentData(componentData);
			Blink();
			MusicMgr.Inst.ForcePlayMusic(GameConstManaged.bgm_Boss);
			break;
		}
		case "Attack2":
		{
			attack2LaserUseCount = 0;
			attack2BallUseCount = attack2Stage1Count;
			if (base.CurrentHPRatio <= stage3HPRatio)
			{
				attack2BallUseCount = attack2Stage3Count;
			}
			else if (base.CurrentHPRatio <= stage2HPRatio)
			{
				attack2BallUseCount = attack2Stage2Count;
			}
			float num = UnityEngine.Random.Range(0, 360);
			for (int i = 0; i < attack2BallUseCount; i++)
			{
				attack2LaserUseCount += i;
				Vector3 dir = Tool2D.GetDir(num + (float)(360 / attack2BallUseCount * i));
				balls[i].transform.position = dir * attack2ForwardOffset + Tool2D.IgnoreZPoint(base.transform, 0f - attack2BallHeight);
				Vector3 zero = Vector3.zero;
				if (UnitDotsSyncSystem.Raycast(base.transform.position, dir, 20f, GameConst.Filter_Wall, out raycastHit))
				{
					zero = Tool2D.IgnoreZPoint(raycastHit.point);
				}
				else
				{
					Debug.LogWarning("不应该射不中墙");
					zero = Tool2D.IgnoreZPoint(base.transform) + dir * 20f;
				}
				balls[i].Initialize(this, dir, zero);
			}
			as_Vomit.Play();
			break;
		}
		case "Attack2Finish":
			base.Anima.SetTrigger("Idle");
			break;
		default:
			Debug.LogError(animaName);
			break;
		}
	}

	public override void BeforeTakeDamage_Dots(ref TakeDamageInfo_Dots info)
	{
		if (state == UnitState.BornIdle)
		{
			info.immuneDamage = true;
		}
	}

	protected override void BossDeadStay()
	{
		base.Anima.SetTrigger("Dead");
		base.enabled = false;
		base.Rigid.isKinematic = true;
		SyncDotsRigidKindmatic();
		base.CC_Self.enabled = false;
		SetDotsCCEnable(isOpen: false);
		UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
		componentData.BossDeadStay();
		SetComponentData(componentData);
		GetComponent<BossDeadCreateEffect>().CreateEffect();
	}

	public override void AfterDead(ref TakeDamageInfo_Dots info)
	{
		base.AfterDead(ref info);
		UnityEngine.Object.Destroy(go_RT);
		UnityEngine.Object.Destroy(go_MR);
		for (int num = lasers.Count - 1; num >= 0; num--)
		{
			UnityEngine.Object.Destroy(lasers[num].gameObject);
		}
		as_LaserLoop.Stop();
	}
}
