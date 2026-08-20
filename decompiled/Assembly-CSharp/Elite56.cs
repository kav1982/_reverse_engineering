using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Elite56 : UnitBase
{
	private enum Elite56State
	{
		Idle,
		Move,
		Stack,
		Shoot
	}

	private enum Elite56CannonState
	{
		Aiming,
		Shoot,
		Reloading
	}

	private enum Elite56SkillCannon
	{
		HMissile,
		VMissile
	}

	private static readonly int Shoot = Animator.StringToHash("Shoot");

	private float moveTimer;

	private float skillTimer;

	private bool isFaceRight = true;

	private UIEndlessEliteHpBar hpBar;

	public Transform ModelTransform;

	public float FaceDirectionChangeDuration;

	public Transform CannonTransform;

	public Transform ShootPointTransform;

	public List<LineRenderer> LaserRendererList;

	public List<Transform> LaserPointTransformList;

	public ParticleSystem GlowingEyeParticle;

	public Transform EyeTransform;

	public float CloseToTargetStopLaserDistance;

	public ParticleSystem MuzzleParticle;

	public List<ParticleSystem> CannonShootFlipParticleList;

	public float AfterShootReloadSEDelayTime;

	public int GaintBombShootRequire;

	private int shootCount;

	[Header("移动相关参数")]
	public float CloseToTargetStopMotionDistance;

	public float CloseToTargetShootIntervalDecreaseRatio;

	public float MoveInterval;

	private float shootTime;

	[Header("平面子母导弹")]
	public int HMissileMagazine;

	public float HMissileShootInterval;

	public float HMissileDamage;

	public float HMissileSpeed;

	public float HMissileRange;

	public int HSubMissileCount;

	public float HSubMissileSpeed;

	public float HSubMissileDamage;

	public float HSubMissleExplosionRange;

	public float HSubMissileMaxScatter;

	public float HSubMissleMaxMoveDistance;

	private int HMissileRemainCount;

	private float HMissileShootTimer;

	[Header("垂直巨型导弹")]
	public float VMissileLandTime;

	public float VMissileExplosionWaitTime;

	public float VMissileLandDamage;

	public float VMissileLandRange;

	public float VMissileExplosionDamage;

	public float VMissileExplosionRange;

	public float VLockTime;

	public float VMarkShowAt;

	private float vLockTimer;

	private bool spawnVLock;

	public float VLockMaxRange;

	public float VMissileSkillDuration;

	private bool isVMissileLaunched;

	public float VAfterSkillDelayMoveDuration;

	private Elite56Marker VMissileMark;

	private Elite56State eliteState;

	private Elite56CannonState cannonState;

	private Elite56SkillCannon currentSkill;

	private void OnEnable()
	{
		ResetAllLasers();
		GlowingEyeParticle.Stop();
	}

	public override void SingleInitialCallback()
	{
		hpBar = UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/UI/UIEndlessEliteHpBar"), myPpt.tsf_Layer.position + new Vector3(0f, myPpt.unitCfg.relicShowHPUIHight - 0.2f, 0f) * myPpt.tsf_Layer.lossyScale.y, Quaternion.identity, myPpt.tsf_Layer).GetComponent<UIEndlessEliteHpBar>();
		hpBar.Initialize(this);
	}

	public override void EveryInitialCallback()
	{
		eliteState = Elite56State.Idle;
		cannonState = Elite56CannonState.Aiming;
		moveTimer = 0f;
		skillTimer = 0f;
		LaserRendererList[0].enabled = true;
		LaserPointTransformList[0].gameObject.SetActive(value: true);
		vLockTimer = 0f;
		VMissileMark = null;
		shootCount = 0;
	}

	public override void Update()
	{
		base.Update();
		UpdateState();
		if (base.deadStayed && VMissileMark != null && VMissileMark.gameObject.activeInHierarchy)
		{
			ObjPoolMgr.Inst.RecycleGO(VMissileMark.gameObject);
			VMissileMark = null;
		}
	}

	private void UpdateState()
	{
		switch (eliteState)
		{
		case Elite56State.Idle:
			EnterState(Elite56State.Move);
			break;
		case Elite56State.Move:
			if (!base.HaveTarget)
			{
				GetNearestTargetPlayerFirst();
			}
			if (base.HaveTarget)
			{
				if (Tool2D.IgnoreZDistance(base.transform.position, base.TargetPoint) <= CloseToTargetStopMotionDistance)
				{
					SetMove(Vector3.zero);
					moveTimer += Time.deltaTime / CloseToTargetShootIntervalDecreaseRatio;
				}
				else
				{
					GetNavInfo(base.TargetPoint);
					SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed);
					CheckNavInfo();
					moveTimer += Time.deltaTime;
				}
			}
			else
			{
				SetMove(Vector3.zero, isFlip: false);
			}
			FaceToPlayer();
			UpdateFaceDirection();
			UpdateCannonState();
			if (base.HaveTarget && cannonState != Elite56CannonState.Reloading && moveTimer >= MoveInterval)
			{
				EnterState(Elite56State.Shoot);
			}
			break;
		case Elite56State.Shoot:
			SetMove(Vector3.zero, isFlip: false);
			FaceToPlayer();
			UpdateFaceDirection();
			UpdateCannonState();
			skillTimer += Time.deltaTime;
			switch (currentSkill)
			{
			case Elite56SkillCannon.HMissile:
				HMissileShootTimer += Time.deltaTime;
				if (HMissileShootTimer >= HMissileShootInterval)
				{
					if (HMissileRemainCount <= 0)
					{
						EnterState(Elite56State.Move);
						HMissileRemainCount = HMissileMagazine;
					}
					else
					{
						HMissileShootTimer -= HMissileShootInterval;
						base.Anima.SetTrigger(Shoot);
						HMissileRemainCount--;
					}
				}
				break;
			case Elite56SkillCannon.VMissile:
			{
				if (skillTimer >= VMissileSkillDuration)
				{
					EnterState(Elite56State.Move);
					moveTimer = 0f - VAfterSkillDelayMoveDuration;
				}
				if (isVMissileLaunched)
				{
					break;
				}
				vLockTimer += Time.deltaTime;
				Vector3 normalized = (base.TargetPoint - EyeTransform.position).normalized;
				for (int i = 0; i < LaserRendererList.Count; i++)
				{
					float num = Mathf.Clamp(vLockTimer / VMarkShowAt, 0f, 1f);
					Vector3 position = base.TargetPoint + Tool2D.GetDir(normalized, 120 * i) * (1f - num) * VLockMaxRange;
					LaserRendererList[i].SetPosition(0, EyeTransform.position);
					LaserRendererList[i].SetPosition(1, position);
					LaserPointTransformList[i].transform.position = position;
				}
				if (vLockTimer >= VMarkShowAt && !spawnVLock)
				{
					spawnVLock = true;
					VMissileMark = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Elite56_Marker", PlayerMgr.Inst.PlayerPoint).GetComponent<Elite56Marker>();
					VMissileMark.MarkStart(VLockTime - VMarkShowAt - 0.1f, VMissileExplosionWaitTime);
					SEMgr.Inst.elite56LockTarget.PlaySE();
				}
				if (GlowingEyeParticle.isPlaying)
				{
					Vector3 eulerAngles = GlowingEyeParticle.transform.localRotation.eulerAngles;
					eulerAngles.y = (isFaceRight ? 60f : 120f);
					GlowingEyeParticle.transform.localRotation = Quaternion.Euler(eulerAngles);
				}
				if (vLockTimer >= VLockTime)
				{
					ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Elite56_VGiantBomb", PlayerMgr.Inst.PlayerPoint).GetComponent<Elite56GiantBomb>().InitialBomb(PlayerMgr.Inst.PlayerPoint, VMissileLandTime, base.transform.position, VMissileLandRange, VMissileLandDamage, VMissileExplosionRange, VMissileExplosionDamage, VMissileExplosionWaitTime);
					isVMissileLaunched = true;
					GlowingEyeParticle.Stop();
					ResetAllLasers();
					if (VMissileMark != null)
					{
						VMissileMark.StopChaseTarget();
					}
				}
				break;
			}
			}
			break;
		default:
			throw new ArgumentOutOfRangeException();
		case Elite56State.Stack:
			break;
		}
	}

	private void EnterState(Elite56State state)
	{
		eliteState = state;
		switch (eliteState)
		{
		case Elite56State.Move:
			moveTimer = 0f;
			skillTimer = 0f;
			ResetAllLasers();
			base.Rigid.isKinematic = false;
			SyncDotsRigidKindmatic();
			break;
		case Elite56State.Shoot:
			skillTimer = 0f;
			shootCount++;
			if (shootCount >= GaintBombShootRequire)
			{
				shootCount = 0;
				SelectNextSkill(Elite56SkillCannon.VMissile);
			}
			else
			{
				SelectNextSkill(Elite56SkillCannon.HMissile);
			}
			if (currentSkill == Elite56SkillCannon.VMissile)
			{
				SEMgr.Inst.elite56LaserLock.PlaySE();
			}
			base.Rigid.isKinematic = true;
			SyncDotsRigidKindmatic();
			break;
		default:
			throw new ArgumentOutOfRangeException();
		case Elite56State.Idle:
		case Elite56State.Stack:
			break;
		}
	}

	private void SelectNextSkill(Elite56SkillCannon targetSkill)
	{
		currentSkill = targetSkill;
		switch (targetSkill)
		{
		case Elite56SkillCannon.HMissile:
			HMissileRemainCount = HMissileMagazine;
			HMissileShootTimer = 0f;
			shootTime = HMissileShootInterval;
			LaserRendererList[0].enabled = true;
			LaserPointTransformList[0].gameObject.SetActive(value: true);
			break;
		case Elite56SkillCannon.VMissile:
			VMissileMark = null;
			spawnVLock = false;
			vLockTimer = 0f;
			isVMissileLaunched = false;
			foreach (LineRenderer laserRenderer in LaserRendererList)
			{
				laserRenderer.enabled = true;
			}
			foreach (Transform laserPointTransform in LaserPointTransformList)
			{
				laserPointTransform.gameObject.SetActive(value: true);
			}
			GlowingEyeParticle.Play();
			break;
		}
	}

	private void ResetAllLasers()
	{
		foreach (LineRenderer laserRenderer in LaserRendererList)
		{
			laserRenderer.enabled = false;
		}
		foreach (Transform laserPointTransform in LaserPointTransformList)
		{
			laserPointTransform.gameObject.SetActive(value: false);
		}
		for (int i = 0; i < LaserRendererList.Count; i++)
		{
			SetLaserPos(i, ShootPointTransform.position, ShootPointTransform.position);
		}
	}

	private void FaceToPlayer()
	{
		if (base.HaveTarget)
		{
			isFaceRight = base.TargetPoint.x >= base.transform.position.x;
		}
	}

	private void UpdateFaceDirection(bool instantLerp = false)
	{
		float num = (isFaceRight ? 1f : (-1f));
		if (instantLerp)
		{
			num = Mathf.Lerp(base.transform.localScale.x, num, 10f * Time.deltaTime);
			ModelTransform.localScale = new Vector3(num, ModelTransform.localScale.y, ModelTransform.localScale.z);
		}
		else
		{
			ModelTransform.DOScaleX(num, FaceDirectionChangeDuration);
		}
		bool flag = ModelTransform.localScale.x >= 0f;
		foreach (ParticleSystem cannonShootFlipParticle in CannonShootFlipParticleList)
		{
			cannonShootFlipParticle.transform.localRotation = Quaternion.Euler(new Vector3(0f, flag ? 180 : 0, 0f));
		}
	}

	private void UpdateCannonState()
	{
		if (currentSkill == Elite56SkillCannon.HMissile && base.HaveTarget)
		{
			float num = Tool2D.IgnoreZDistance(base.TargetPoint, base.transform.position);
			num = Mathf.Clamp(num, 0f, Mathf.Min(HMissileRange, num));
			Vector3 vector = CannonTransform.position + (base.TargetPoint - CannonTransform.position).IgnoreZ().normalized * num;
			Vector3 to = ((eliteState == Elite56State.Move) ? Vector3.right : (isFaceRight ? (base.TargetPoint - CannonTransform.position).IgnoreZ().normalized : (CannonTransform.position - base.TargetPoint).IgnoreZ().normalized));
			int num2 = ((eliteState == Elite56State.Move) ? 120 : 200);
			CannonTransform.right = Tool2D.RotateTowardsAroundZAxis(CannonTransform.right, to, (float)num2 * Time.deltaTime);
			bool num3 = Vector3.Angle(vector - ShootPointTransform.position, CannonTransform.right) <= 5f;
			float a = Tool2D.IgnoreZDistance(PlayerMgr.Inst.PlayerPoint + new Vector3(0f, 0.3f, 0f), ShootPointTransform.position);
			a = ((!num3) ? HMissileRange : Mathf.Min(a, HMissileRange));
			vector = ShootPointTransform.position + ((ModelTransform.localScale.x >= 0f) ? CannonTransform.right : (-CannonTransform.right)) * a;
			if (a <= CloseToTargetStopLaserDistance)
			{
				vector = ShootPointTransform.position;
			}
			SetLaserPos(0, ShootPointTransform.position, vector);
		}
	}

	private void SetLaserPos(int index, Vector3 shootPosition, Vector3 targetPosition)
	{
		LaserRendererList[index].SetPosition(0, shootPosition);
		LaserRendererList[index].SetPosition(1, targetPosition);
		LaserPointTransformList[index].position = targetPosition;
	}

	public override void AnimaAction(string animaName)
	{
		if (animaName == "Shoot")
		{
			Vector3 direction = Tool2D.IgnoreZV2ToV1Normal(PlayerMgr.Inst.PlayerPoint, ShootPointTransform.position);
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Elite56_HMissile", ShootPointTransform.position + new Vector3(0f, 0f, -0.7f)).GetComponent<Elite56HMissile>().InitMissile(direction, HMissileRange, HMissileSpeed, HMissileDamage, HSubMissileCount, HSubMissileMaxScatter, HSubMissileDamage, HSubMissleExplosionRange, HSubMissileSpeed, HSubMissleMaxMoveDistance, myPpt.myEntity);
			MuzzleParticle.Play();
			SEMgr.Inst.elite56Shoot.PlaySE();
			StartCoroutine(PlayReloadSE(AfterShootReloadSEDelayTime));
		}
	}

	private IEnumerator PlayReloadSE(float delayTime)
	{
		yield return new WaitForSeconds(delayTime);
		if (!myPpt.isUnitDead && !base.deadStayed)
		{
			SEMgr.Inst.elite56Reload.PlaySE();
		}
	}
}
