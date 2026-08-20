using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Entities;
using UnityEngine;

public class Boss56Elite56DogShooter : MonoBehaviour
{
	private enum Elite56SkillCannon
	{
		HMissile,
		VMissile
	}

	private static readonly int Shoot = Animator.StringToHash("Shoot");

	private float moveTimer;

	private float skillTimer;

	private bool isFaceRight = true;

	public Animator Anima;

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

	[Header("平面子母导弹")]
	public float HMissileDamage;

	public float HMissileSpeed;

	public float HMissileRange;

	public float HMissileLaserDistance;

	public int HSubMissileCount;

	public float HSubMissileSpeed;

	public float HSubMissileDamage;

	public float HSubMissleExplosionRange;

	public float HSubMissileMaxScatter;

	public float HSubMissleMaxMoveDistance;

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

	private bool isVMissileLaunched;

	private Elite56Marker VMissileMark;

	private Elite56SkillCannon currentSkill;

	private Entity shooterEntity;

	private Vector3 giantBombTargetPoint;

	private Vector3 baseDir;

	private bool isShootGiantBomb;

	private Vector3 targetPoint;

	private float shootGiantBombWaitTime;

	private bool isLockStart;

	private float SM_ShootTimer;

	private float SM_RotateSpeed;

	private float SM_MaxRotateAngle;

	private float SM_RotateTimer;

	private float SM_Duration;

	private float SM_ShootInterval;

	private Vector3 SM_BaseDirection;

	private Vector3 SM_ShootDirection;

	public float SM_KnockBackForce;

	private List<float> SM_RotateAngleList;

	private float SM_ShiftAngle;

	private void OnEnable()
	{
		ResetAllLasers();
		GlowingEyeParticle.Stop();
		moveTimer = 0f;
		skillTimer = 0f;
		LaserRendererList[0].enabled = true;
		LaserPointTransformList[0].gameObject.SetActive(value: true);
		vLockTimer = 0f;
		VMissileMark = null;
		SM_ShiftAngle = 0f;
	}

	public void EndSkill()
	{
		if (VMissileMark != null && VMissileMark.gameObject.activeInHierarchy)
		{
			ObjPoolMgr.Inst.RecycleGO(VMissileMark.gameObject);
			VMissileMark = null;
		}
		ObjPoolMgr.Inst.RecycleGO(base.gameObject);
	}

	public void InitialOwnerData(Entity shooterEntity)
	{
		this.shooterEntity = shooterEntity;
		FaceToPlayer();
		UpdateFaceDirection();
	}

	public void SetCannonShiftAngle(float shiftAngle)
	{
		SM_ShiftAngle = shiftAngle;
	}

	public void ShootSplitBomb()
	{
		if (!isShootGiantBomb)
		{
			currentSkill = Elite56SkillCannon.HMissile;
			Anima.SetTrigger(Shoot);
			CastTargetSkill();
			StartCoroutine(ShootCannnon());
		}
	}

	public void ShootGiantBomb(Vector3 targetPos, float initialDelay)
	{
		currentSkill = Elite56SkillCannon.VMissile;
		isShootGiantBomb = true;
		targetPoint = targetPos;
		isLockStart = false;
		shootGiantBombWaitTime = initialDelay;
		FaceToPlayer();
		UpdateFaceDirection();
		CastTargetSkill();
	}

	public void Update()
	{
		UpdateState();
	}

	private void UpdateState()
	{
		UpdateCannonState();
		skillTimer += Time.deltaTime;
		Elite56SkillCannon elite56SkillCannon = currentSkill;
		if (elite56SkillCannon == Elite56SkillCannon.HMissile || elite56SkillCannon != Elite56SkillCannon.VMissile || isVMissileLaunched || skillTimer < shootGiantBombWaitTime)
		{
			return;
		}
		if (!isLockStart)
		{
			SEMgr.Inst.elite56LaserLock.PlaySE();
			isLockStart = true;
		}
		vLockTimer += Time.deltaTime;
		Vector3 normalized = (targetPoint - EyeTransform.position).normalized;
		for (int i = 0; i < LaserRendererList.Count; i++)
		{
			float num = Mathf.Clamp(vLockTimer / VMarkShowAt, 0f, 1f);
			Vector3 position = targetPoint + Tool2D.GetDir(normalized, 120 * i) * (1f - num) * VLockMaxRange;
			LaserRendererList[i].SetPosition(0, EyeTransform.position);
			LaserRendererList[i].SetPosition(1, position);
			LaserPointTransformList[i].transform.position = position;
		}
		if (vLockTimer >= VMarkShowAt && !spawnVLock)
		{
			spawnVLock = true;
			VMissileMark = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Elite56_Marker", targetPoint).GetComponent<Elite56Marker>();
			VMissileMark.MarkStart(VLockTime - VMarkShowAt - 0.1f, VMissileExplosionWaitTime, isfollowPlayer: false);
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
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Elite56_VGiantBomb", targetPoint).GetComponent<Elite56GiantBomb>().InitialBomb(targetPoint, VMissileLandTime, base.transform.position, VMissileLandRange, VMissileLandDamage, VMissileExplosionRange, VMissileExplosionDamage, VMissileExplosionWaitTime);
			isVMissileLaunched = true;
			GlowingEyeParticle.Stop();
			ResetAllLasers();
			if (VMissileMark != null)
			{
				VMissileMark.StopChaseTarget();
			}
			isShootGiantBomb = false;
		}
	}

	private void CastTargetSkill()
	{
		skillTimer = 0f;
		if (isShootGiantBomb)
		{
			SelectNextSkill(Elite56SkillCannon.VMissile);
		}
		else
		{
			SelectNextSkill(Elite56SkillCannon.HMissile);
		}
	}

	private Vector3 GetCurrentShiftDirection()
	{
		float num = SM_RotateTimer * (SM_RotateSpeed / SM_MaxRotateAngle) / 2f % 1f;
		float num2 = ((num < 0.5f) ? (num * 2f) : (2f - num * 2f));
		return Tool2D.GetDir(SM_BaseDirection, (0f - SM_MaxRotateAngle) / 2f + num2 * SM_MaxRotateAngle);
	}

	private void SelectNextSkill(Elite56SkillCannon targetSkill)
	{
		currentSkill = targetSkill;
		switch (targetSkill)
		{
		case Elite56SkillCannon.HMissile:
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
		isFaceRight = targetPoint.x >= base.transform.position.x;
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
		if (!isShootGiantBomb)
		{
			SM_ShootDirection = Tool2D.GetDir(PlayerMgr.Inst.PlayerPoint - ShootPointTransform.position, SM_ShiftAngle).IgnoreZ().normalized;
			Vector3 vector = CannonTransform.position + SM_ShootDirection;
			Vector3 to = (isFaceRight ? (vector - CannonTransform.position).IgnoreZ().normalized : (CannonTransform.position - vector).IgnoreZ().normalized);
			int num = 200;
			CannonTransform.right = Tool2D.RotateTowardsAroundZAxis(CannonTransform.right, to, (float)num * Time.deltaTime);
			float num2 = Mathf.Min(HMissileLaserDistance, Tool2D.IgnoreZDistance(ShootPointTransform.position, PlayerMgr.Inst.PlayerPoint + new Vector3(0f, 0.5f, 0f)));
			vector = ShootPointTransform.position + ((ModelTransform.localScale.x >= 0f) ? CannonTransform.right : (-CannonTransform.right)) * num2;
			if (num2 <= CloseToTargetStopLaserDistance)
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

	private IEnumerator ShootCannnon()
	{
		yield return new WaitForSeconds(0.02f);
		ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Elite56_HMissile", ShootPointTransform.position + new Vector3(0f, 0f, -0.7f)).GetComponent<Elite56HMissile>().InitMissile(SM_ShootDirection, HMissileRange, HMissileSpeed, HMissileDamage, HSubMissileCount, HSubMissileMaxScatter, HSubMissileDamage, HSubMissleExplosionRange, HSubMissileSpeed, HSubMissleMaxMoveDistance, shooterEntity, SM_KnockBackForce);
		MuzzleParticle.Play();
		SEMgr.Inst.elite56Shoot.PlaySE();
		yield return new WaitForSeconds(AfterShootReloadSEDelayTime);
		if (base.gameObject.activeInHierarchy)
		{
			SEMgr.Inst.elite56Reload.PlaySE();
		}
	}
}
