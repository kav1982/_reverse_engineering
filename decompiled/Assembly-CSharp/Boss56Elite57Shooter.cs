using System;
using DG.Tweening;
using Unity.Entities;
using UnityEngine;

public class Boss56Elite57Shooter : MonoBehaviour
{
	public enum Elite57State
	{
		Idle,
		Move,
		CastSpell
	}

	public enum Elite57Skills
	{
		VMissile,
		HMissile
	}

	private static readonly int Progress = Shader.PropertyToID("_Progress");

	private UIEndlessEliteHpBar hpBar;

	public Transform RMissileHiveTransform;

	public Transform RMissileHiveFirstMissileTransform;

	public Transform LMissileHiveTransform;

	public Transform LMissileHiveFirstMissileTransform;

	public Transform BackMissileFirstMissileTransform;

	public Transform ModelTransform;

	private int currentMissileIndex;

	private Elite57State eliteState;

	private Elite57Skills currentSkill;

	private float eliteTimer;

	public float CloseToTargetStopMotionDistance;

	private bool isFaceRight = true;

	public float FaceDirectionChangeDuration;

	public float SkillInterval;

	public float MissileHDistance;

	public float MissileVDistance;

	public int SideMissileMagazine;

	public float BackMissileHDistance;

	public float BackMissileVDistance;

	public int BackMissileMagazine;

	public float BackMissileVHorizontalAngle;

	public float BackMissileVVerticalAngle;

	private float missileShootTimer;

	public float MissileHiveRotateBackAngleSpeed;

	private bool isRightMissile;

	private int currentSkillCounter;

	public SpriteRenderer LHiveRedSprite;

	public SpriteRenderer RHiveRedSprite;

	public SpriteRenderer BackHiveSprite;

	[Header("垂直导弹洗地")]
	public float VBeforeShootTime;

	public float VShootInterval;

	public float VFlySpeed;

	public float VInitialHeight;

	public float VMissileLifeDuration;

	public float VSubLineBonusWaitTimePerShoot;

	public float VSubMissileDelayStartTimePerCount;

	public float VSubMissileStartFallHeight;

	public float VSubMissileLandTime;

	public float VSubMissileDistance;

	public int VSubMissileCount;

	public float VExplosionDamage;

	public float VExplosionRange;

	private bool VRightToLeft;

	public float VAfterSkillBonusWaitTime;

	[Header("横向迷你导弹连射")]
	public float HShootInterval;

	public float HHiveRotateBackAngleSpeed;

	public float HBeforeShootTime;

	public float Hp1Speed;

	public float HMissileExplosionRange;

	public float HMissileDamage;

	public float HLandExplosionWaitTime;

	public float HMissilePreStopDis;

	public float HVerticalShootMaxBonusWaitTime;

	public float HAfterSkillBonusWaitTime;

	public Animator Anima;

	private Entity shooterEntity;

	private Vector3 HCurrentAttackPoint;

	private Vector3 HAttackPointMoveDir;

	private float HAttackPointMoveSpeed;

	private float HShootDelay;

	private Vector3 VFinalHitPosShift;

	private Vector3 VFinalSpawnDir;

	private float VDelayAppearTime;

	private float VLockTime;

	private void OnEnable()
	{
		eliteState = Elite57State.Idle;
		currentMissileIndex = 0;
		currentSkill = Elite57Skills.VMissile;
		eliteTimer = 0f;
		isRightMissile = false;
		currentSkillCounter = 0;
		VDelayAppearTime = 0f;
		VLockTime = 0f;
		RHiveRedSprite.material.SetFloat(Progress, 0f);
		LHiveRedSprite.material.SetFloat(Progress, 0f);
		BackHiveSprite.material.SetFloat(Progress, 0f);
	}

	public void Initialdata(Entity shooterEntity)
	{
		this.shooterEntity = shooterEntity;
	}

	public void CastHorizonAttack(Vector3 shootDirection, float shootDelay, float VSkillLockTime)
	{
		VLockTime = VSkillLockTime;
		currentSkill = Elite57Skills.HMissile;
		HCurrentAttackPoint = base.transform.position + shootDirection * 30f;
		HAttackPointMoveDir = shootDirection;
		HShootDelay = shootDelay;
		EnterState(Elite57State.CastSpell);
	}

	public void CastVerticalAttack(Vector3 shootDirection, Vector3 baseShiftPos, float delayAppearTime)
	{
		if (!(VLockTime > 0f))
		{
			currentSkill = Elite57Skills.VMissile;
			VFinalHitPosShift = baseShiftPos;
			VFinalSpawnDir = shootDirection;
			VDelayAppearTime = delayAppearTime;
			EnterState(Elite57State.CastSpell);
			bool flag = currentMissileIndex < 3;
			int num = currentMissileIndex % 3;
			int num2 = (isFaceRight ? 1 : (-1));
			Vector3 vector = Tool2D.GetDir(BackMissileFirstMissileTransform.right, BackMissileVHorizontalAngle) * num2 * 2f * BackMissileHDistance * ((float)num + (isFaceRight ? 0.3f : 0f)) + Tool2D.GetDir(BackMissileFirstMissileTransform.right, BackMissileVVerticalAngle) * (flag ? 0f : BackMissileVDistance);
			Vector3 moveDirection = ((VFinalSpawnDir == default(Vector3)) ? (Tool2D.GetDir(90f + UnityEngine.Random.Range(-60f, 60f)) * ((!VRightToLeft) ? 1 : (-1))) : VFinalSpawnDir);
			Vector3 vector2 = BackMissileFirstMissileTransform.position + vector;
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Elite57_Boss56SpecialLongMissile", vector2).GetComponent<Boss56Elite57SpecialLongMissile>().InitialBombData(durationPeriod: VMissileLifeDuration + VDelayAppearTime + (float)currentMissileIndex * VSubLineBonusWaitTimePerShoot, flySpeed: VFlySpeed, initialHeight: VInitialHeight, startFallHeight: VSubMissileStartFallHeight, explosionRange: VExplosionRange, explosionWaitTime: VSubMissileLandTime, explosionDamage: VExplosionDamage, explosionPosDistance: VSubMissileDistance, subMissileCount: VSubMissileCount, moveDirection: moveDirection, bonusWaitTime: VSubMissileDelayStartTimePerCount, targetPosShift: VFinalHitPosShift, useShortEffect: true);
			GameObject gO = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Elite56_MissileHit", vector2 + new Vector3(0f, 0f, -0.2f), 1.2f);
			gO.transform.localScale = Vector3.one * 0.6f;
			gO.transform.right = Tool2D.GetDir(0f);
			SEMgr.Inst.elite57VMissileLaunch.PlaySE();
			VRightToLeft = !VRightToLeft;
			currentMissileIndex++;
		}
	}

	public void Update()
	{
		UpdateState();
	}

	private void UpdateState()
	{
		if (VLockTime > 0f)
		{
			VLockTime -= Time.deltaTime;
			if (VLockTime < 0f)
			{
				VLockTime = 0f;
			}
		}
		switch (eliteState)
		{
		case Elite57State.Idle:
			EnterState(Elite57State.Move);
			break;
		case Elite57State.Move:
			RMissileHiveTransform.right = Tool2D.RotateTowardsAroundZAxis(RMissileHiveTransform.right, Tool2D.GetDir(-90f), MissileHiveRotateBackAngleSpeed * Time.deltaTime);
			LMissileHiveTransform.right = RMissileHiveTransform.right;
			FaceToPlayer();
			UpdateFaceDirection();
			break;
		case Elite57State.CastSpell:
			FaceToPlayer();
			UpdateFaceDirection();
			eliteTimer += Time.deltaTime;
			switch (currentSkill)
			{
			case Elite57Skills.VMissile:
				if (!(eliteTimer < VBeforeShootTime))
				{
					missileShootTimer += Time.deltaTime;
				}
				break;
			case Elite57Skills.HMissile:
			{
				Vector3 to = (isFaceRight ? (HCurrentAttackPoint - RMissileHiveTransform.position).IgnoreZ().normalized : (RMissileHiveTransform.position - HCurrentAttackPoint).IgnoreZ().normalized);
				RMissileHiveTransform.right = Tool2D.RotateTowardsAroundZAxis(RMissileHiveTransform.right, to, HHiveRotateBackAngleSpeed * Time.deltaTime);
				LMissileHiveTransform.right = RMissileHiveTransform.right;
				if (eliteTimer < HShootDelay)
				{
					break;
				}
				missileShootTimer += Time.deltaTime;
				if (currentMissileIndex + 1 >= SideMissileMagazine)
				{
					EnterState(Elite57State.Move);
					eliteTimer -= HAfterSkillBonusWaitTime;
				}
				if (missileShootTimer >= HShootInterval && currentMissileIndex + 1 <= SideMissileMagazine)
				{
					missileShootTimer -= HShootInterval;
					int num = Mathf.FloorToInt((float)currentMissileIndex / 5f);
					int num2 = currentMissileIndex % 5;
					bool flag = num2 <= 2;
					Vector3 vector = Tool2D.GetDir(RMissileHiveFirstMissileTransform.right, 90f) * 2f * MissileVDistance * num + (flag ? (RMissileHiveFirstMissileTransform.right * num2 * MissileHDistance) : (RMissileHiveFirstMissileTransform.right * ((float)(num2 - 3) + 0.5f) * MissileHDistance + Tool2D.GetDir(RMissileHiveFirstMissileTransform.right, 90f) * MissileVDistance));
					if (isRightMissile)
					{
						Vector3 vector2 = RMissileHiveFirstMissileTransform.position + vector;
						float phase1FlyDistance = Tool2D.IgnoreZDistance(vector2, HCurrentAttackPoint) - HMissilePreStopDis;
						Vector3 right = RMissileHiveTransform.right * (isFaceRight ? 1 : (-1));
						float explosionWaitTime = HLandExplosionWaitTime + HVerticalShootMaxBonusWaitTime * Mathf.Abs(right.y);
						ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Elite57_MiniMissile", vector2).GetComponent<Elite57MiniMissile>().MissileInitialize(Hp1Speed, phase1FlyDistance, RMissileHiveTransform.right * (isFaceRight ? 1 : (-1)), HMissileDamage, HMissileExplosionRange, explosionWaitTime, shooterEntity, -0.3f);
						GameObject gO = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Elite56_MissileHit", vector2 + new Vector3(0f, 0f, -0.3f), 3f);
						gO.transform.localScale = Vector3.one * 0.3f;
						gO.transform.right = right;
						Anima.Play("RightShoot", 1, 0f);
						currentMissileIndex++;
					}
					else
					{
						Vector3 vector3 = LMissileHiveFirstMissileTransform.position + vector;
						float phase1FlyDistance2 = Tool2D.IgnoreZDistance(vector3, HCurrentAttackPoint) - HMissilePreStopDis;
						Vector3 vector4 = RMissileHiveTransform.right * (isFaceRight ? 1 : (-1));
						float explosionWaitTime2 = HLandExplosionWaitTime + HVerticalShootMaxBonusWaitTime * Mathf.Abs(vector4.y);
						ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Elite57_MiniMissile", vector3).GetComponent<Elite57MiniMissile>().MissileInitialize(Hp1Speed, phase1FlyDistance2, vector4, HMissileDamage, HMissileExplosionRange, explosionWaitTime2, shooterEntity, -0.3f);
						GameObject gO2 = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Elite56_MissileHit", vector3 + new Vector3(0f, 0f, -0.3f), 3f);
						gO2.transform.localScale = Vector3.one * 0.3f;
						gO2.transform.right = vector4;
						Anima.Play("LeftShoot", 2, 0f);
					}
					SEMgr.Inst.elite57HMissileLaunch.PlaySE(SEPlayMode.Replay, 3, 0.15f);
					isRightMissile = !isRightMissile;
				}
				break;
			}
			}
			break;
		default:
			throw new ArgumentOutOfRangeException();
		}
	}

	private void EnterState(Elite57State state)
	{
		eliteState = state;
		eliteTimer = 0f;
		switch (state)
		{
		case Elite57State.Move:
			BackHiveSprite.material.DOFloat(0f, Progress, 1f);
			RHiveRedSprite.material.DOFloat(0f, Progress, 1f);
			LHiveRedSprite.material.DOFloat(0f, Progress, 1f);
			break;
		case Elite57State.CastSpell:
			missileShootTimer = 0f;
			isRightMissile = false;
			switch (currentSkill)
			{
			case Elite57Skills.VMissile:
				BackHiveSprite.material.DOFloat(1f, Progress, 0.7f);
				break;
			case Elite57Skills.HMissile:
				RHiveRedSprite.material.DOFloat(1f, Progress, 0.7f);
				LHiveRedSprite.material.DOFloat(1f, Progress, 0.7f);
				break;
			}
			break;
		default:
			throw new ArgumentOutOfRangeException("state", state, null);
		case Elite57State.Idle:
			break;
		}
	}

	private void FaceToPlayer()
	{
		isFaceRight = HCurrentAttackPoint.x >= base.transform.position.x;
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
	}
}
