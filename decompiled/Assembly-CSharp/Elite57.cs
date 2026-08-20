using System;
using DG.Tweening;
using UnityEngine;

public class Elite57 : UnitBase
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

	public override void SingleInitialCallback()
	{
		hpBar = UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/UI/UIEndlessEliteHpBar"), myPpt.tsf_Layer.position + new Vector3(0f, myPpt.unitCfg.relicShowHPUIHight - 0.2f, 0f) * myPpt.tsf_Layer.lossyScale.y, Quaternion.identity, myPpt.tsf_Layer).GetComponent<UIEndlessEliteHpBar>();
		hpBar.Initialize(this);
	}

	public override void EveryInitialCallback()
	{
		base.EveryInitialCallback();
		eliteState = Elite57State.Idle;
		currentMissileIndex = 0;
		currentSkill = Elite57Skills.VMissile;
		eliteTimer = 0f;
		base.Rigid.isKinematic = true;
		isRightMissile = false;
		currentSkillCounter = 0;
		SyncDotsRigidKindmatic();
		RHiveRedSprite.material.SetFloat(Progress, 0f);
		LHiveRedSprite.material.SetFloat(Progress, 0f);
		BackHiveSprite.material.SetFloat(Progress, 0f);
	}

	public override void Update()
	{
		base.Update();
		UpdateState();
	}

	private void UpdateState()
	{
		switch (eliteState)
		{
		case Elite57State.Idle:
			EnterState(Elite57State.Move);
			break;
		case Elite57State.Move:
			RMissileHiveTransform.right = Tool2D.RotateTowardsAroundZAxis(RMissileHiveTransform.right, Tool2D.GetDir(-90f), MissileHiveRotateBackAngleSpeed * Time.deltaTime);
			LMissileHiveTransform.right = RMissileHiveTransform.right;
			if (!base.HaveTarget)
			{
				GetNearestTargetPlayerFirst();
			}
			FaceToPlayer();
			UpdateFaceDirection();
			if (base.HaveTarget)
			{
				if (Tool2D.IgnoreZDistance(base.transform.position, base.TargetPoint) <= CloseToTargetStopMotionDistance)
				{
					SetMove(Vector3.zero);
					eliteTimer += Time.deltaTime;
				}
				else
				{
					GetNavInfo(base.TargetPoint);
					SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed);
					CheckNavInfo();
					eliteTimer += Time.deltaTime;
				}
			}
			else
			{
				SetMove(Vector3.zero, isFlip: false);
			}
			if (eliteTimer >= SkillInterval)
			{
				EnterState(Elite57State.CastSpell);
			}
			break;
		case Elite57State.CastSpell:
			if (!base.HaveTarget)
			{
				GetNearestTargetPlayerFirst();
			}
			FaceToPlayer();
			UpdateFaceDirection();
			SetMove(Vector3.zero, isFlip: false);
			eliteTimer += Time.deltaTime;
			switch (currentSkill)
			{
			case Elite57Skills.VMissile:
				if (!(eliteTimer < VBeforeShootTime))
				{
					missileShootTimer += Time.deltaTime;
					if (currentMissileIndex >= BackMissileMagazine)
					{
						EnterState(Elite57State.Move);
						eliteTimer -= VAfterSkillBonusWaitTime;
					}
					if (missileShootTimer >= VShootInterval && currentMissileIndex <= BackMissileMagazine)
					{
						missileShootTimer -= VShootInterval;
						bool flag2 = currentMissileIndex < 3;
						int num3 = currentMissileIndex % 3;
						int num4 = (isFaceRight ? 1 : (-1));
						Vector3 vector5 = Tool2D.GetDir(BackMissileFirstMissileTransform.right, BackMissileVHorizontalAngle) * num4 * 2f * BackMissileHDistance * ((float)num3 + (isFaceRight ? 0.3f : 0f)) + Tool2D.GetDir(BackMissileFirstMissileTransform.right, BackMissileVVerticalAngle) * (flag2 ? 0f : BackMissileVDistance);
						Vector3 vector6 = BackMissileFirstMissileTransform.position + vector5;
						ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Elite57_LongMissile", vector6).GetComponent<Elite57LongMissile>().InitialBombData(durationPeriod: VMissileLifeDuration + (float)currentMissileIndex * VSubLineBonusWaitTimePerShoot, flySpeed: VFlySpeed, initialHeight: VInitialHeight, startFallHeight: VSubMissileStartFallHeight, explosionRange: VExplosionRange, explosionWaitTime: VSubMissileLandTime, explosionDamage: VExplosionDamage, explosionPosDistance: VSubMissileDistance, subMissileCount: VSubMissileCount, moveDirection: Tool2D.GetDir(90f + UnityEngine.Random.Range(-60f, 60f)) * ((!VRightToLeft) ? 1 : (-1)), bonusWaitTime: VSubMissileDelayStartTimePerCount);
						GameObject gO3 = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Elite56_MissileHit", vector6 + new Vector3(0f, 0f, -0.2f), 3f);
						gO3.transform.localScale = Vector3.one * 0.6f;
						gO3.transform.right = Tool2D.GetDir(0f);
						SEMgr.Inst.elite57VMissileLaunch.PlaySE();
						VRightToLeft = !VRightToLeft;
						currentMissileIndex++;
					}
				}
				break;
			case Elite57Skills.HMissile:
			{
				Vector3 to = (isFaceRight ? (base.TargetPoint - RMissileHiveTransform.position).IgnoreZ().normalized : (RMissileHiveTransform.position - base.TargetPoint).IgnoreZ().normalized);
				RMissileHiveTransform.right = Tool2D.RotateTowardsAroundZAxis(RMissileHiveTransform.right, to, HHiveRotateBackAngleSpeed * Time.deltaTime);
				LMissileHiveTransform.right = RMissileHiveTransform.right;
				if (eliteTimer < HBeforeShootTime)
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
						float phase1FlyDistance = Tool2D.IgnoreZDistance(vector2, base.TargetPoint) - HMissilePreStopDis;
						Vector3 right = RMissileHiveTransform.right * (isFaceRight ? 1 : (-1));
						float explosionWaitTime = HLandExplosionWaitTime + HVerticalShootMaxBonusWaitTime * Mathf.Abs(right.y);
						ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Elite57_MiniMissile", vector2).GetComponent<Elite57MiniMissile>().MissileInitialize(Hp1Speed, phase1FlyDistance, RMissileHiveTransform.right * (isFaceRight ? 1 : (-1)), HMissileDamage, HMissileExplosionRange, explosionWaitTime, myPpt.myEntity, -0.3f);
						GameObject gO = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Elite56_MissileHit", vector2 + new Vector3(0f, 0f, -0.3f), 3f);
						gO.transform.localScale = Vector3.one * 0.3f;
						gO.transform.right = right;
						base.Anima.Play("RightShoot", 1, 0f);
						currentMissileIndex++;
					}
					else
					{
						Vector3 vector3 = LMissileHiveFirstMissileTransform.position + vector;
						float phase1FlyDistance2 = Tool2D.IgnoreZDistance(vector3, base.TargetPoint) - HMissilePreStopDis;
						Vector3 vector4 = RMissileHiveTransform.right * (isFaceRight ? 1 : (-1));
						float explosionWaitTime2 = HLandExplosionWaitTime + HVerticalShootMaxBonusWaitTime * Mathf.Abs(vector4.y);
						ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Elite57_MiniMissile", vector3).GetComponent<Elite57MiniMissile>().MissileInitialize(Hp1Speed, phase1FlyDistance2, vector4, HMissileDamage, HMissileExplosionRange, explosionWaitTime2, myPpt.myEntity, -0.3f);
						GameObject gO2 = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Elite56_MissileHit", vector3 + new Vector3(0f, 0f, -0.3f), 3f);
						gO2.transform.localScale = Vector3.one * 0.3f;
						gO2.transform.right = vector4;
						base.Anima.Play("LeftShoot", 2, 0f);
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
			currentMissileIndex = 0;
			missileShootTimer = 0f;
			isRightMissile = false;
			CastSpell();
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

	private void CastSpell()
	{
		if (currentSkillCounter < 2)
		{
			currentSkill = Elite57Skills.HMissile;
			currentSkillCounter++;
		}
		else
		{
			currentSkill = Elite57Skills.VMissile;
			currentSkillCounter = 0;
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
	}
}
