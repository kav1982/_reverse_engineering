using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class Elite51_Missile : MonoBehaviour
{
	public Transform MissileTransform;

	public ParticleSystem MissileParticle;

	private float missileTimer;

	private Vector3 targetPosition;

	private float currentSpeed;

	private Vector3 currentMoveDirection;

	private Vector3 currentFaceDirection;

	private float startMoveTime;

	private float moveAngleSpeed;

	private float missileDuration;

	private float startMoveLerpSpeed;

	private float startMoveSpeed;

	private float missileDamage;

	private float missileRange;

	private float missilePreExplosionDuration;

	private float missileCloseToTargetExplosionRangeRatio;

	public float MissileReCheckTargetInterval;

	private float missileRecheckTargetTimer;

	public static float AngleChaseSpeedRatio = 1f;

	private void OnEnable()
	{
		missileTimer = 0f;
		currentSpeed = 0f;
		missileRecheckTargetTimer = 0f;
	}

	private void OnDisable()
	{
		MissileParticle.Stop();
	}

	public void InitData(Vector3 missileFaceDirection, Vector3 initialMoveDirection, Vector3 TargetPoint, float initialMoveSpeed, float startMoveSpeed, float startMoveLerpSpeed, float startMoveTime, float moveAngleSpeed, float missileDuration, float missileDamage, float missileRange, float missilePreExplosionDuration, float missileCloseToTargetExplosionRangeRatio)
	{
		currentFaceDirection = missileFaceDirection;
		currentMoveDirection = initialMoveDirection;
		targetPosition = TargetPoint;
		currentSpeed = initialMoveSpeed;
		this.startMoveSpeed = startMoveSpeed;
		this.startMoveLerpSpeed = startMoveLerpSpeed;
		this.startMoveTime = startMoveTime;
		this.moveAngleSpeed = moveAngleSpeed;
		this.missileDuration = missileDuration;
		this.missileDamage = missileDamage;
		this.missileRange = missileRange;
		this.missilePreExplosionDuration = missilePreExplosionDuration;
		this.missileCloseToTargetExplosionRangeRatio = missileCloseToTargetExplosionRangeRatio;
		MissileTransform.right = missileFaceDirection;
	}

	private void Update()
	{
		if (missileTimer <= startMoveTime)
		{
			missileTimer += Time.deltaTime;
			currentSpeed = Mathf.Lerp(currentSpeed, 0f, 2f * Time.deltaTime);
			base.transform.position += currentMoveDirection * currentSpeed * Time.deltaTime;
			if (missileTimer >= startMoveTime)
			{
				MissileParticle.Play();
				currentMoveDirection = currentFaceDirection;
				SEMgr.Inst.elite51Move.PlaySE();
			}
			return;
		}
		missileTimer += Time.deltaTime;
		currentSpeed = Mathf.Lerp(currentSpeed, startMoveSpeed, startMoveLerpSpeed * Time.deltaTime);
		targetPosition = Vector3.Lerp(targetPosition, PlayerMgr.Inst.PlayerPoint, 7f * Time.deltaTime * AngleChaseSpeedRatio);
		base.transform.position += currentMoveDirection * currentSpeed * Time.deltaTime;
		Vector3 to = Tool2D.IgnoreZV2ToV1Normal(targetPosition, base.transform.position);
		currentMoveDirection = Tool2D.RotateTowardsAroundZAxis(currentMoveDirection, to, moveAngleSpeed * Time.deltaTime * AngleChaseSpeedRatio);
		MissileTransform.right = currentMoveDirection;
		missileRecheckTargetTimer += Time.deltaTime;
		if (missileRecheckTargetTimer >= MissileReCheckTargetInterval)
		{
			missileRecheckTargetTimer -= MissileReCheckTargetInterval;
			List<UnitDotsSyncSystem.DistanceHitResult> list = new List<UnitDotsSyncSystem.DistanceHitResult>();
			UnitDotsSyncSystem.GetCollidersInRange(base.transform.position, missileRange, GameConst.Filter_MonsterAoe, list);
			bool flag = false;
			for (int i = 0; i < list.Count; i++)
			{
				uint layer = UnitDotsSyncSystem.GetLayer(list[i].entity);
				if (layer == 512 || layer == 2097152)
				{
					flag = true;
				}
			}
			if (flag)
			{
				for (int j = 0; j < list.Count; j++)
				{
					UnitDotsSyncSystem.DistanceHitResult distanceHitResult = list[j];
					Entity entity = distanceHitResult.entity;
					switch (UnitDotsSyncSystem.GetLayer(entity))
					{
					case 16777216u:
					{
						UnitDotsSyncSystem.ProcessHitSpell(entity, missileDamage, out var _);
						break;
					}
					case 512u:
					case 32768u:
					case 131072u:
					case 2097152u:
						if (UnitDotsSyncSystem.HasComponent<UnitProperty_Dots>(distanceHitResult.entity))
						{
							TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(AttackerType.NothingSpecial);
							info.damage = missileDamage;
							info.knockbackForce = Tool2D.IgnoreZV2ToV1Normal(distanceHitResult.point, base.transform.position) * 6f;
							UnitDotsSyncSystem.AddTakeDamageRequest(distanceHitResult.entity, info);
						}
						break;
					}
				}
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss50Explosion", base.transform.position, Quaternion.identity, Vector3.one * missileRange / 2f, 3f);
				SEMgr.Inst.elite51Explosion.PlaySE();
				ObjPoolMgr.Inst.RecycleGO(base.gameObject);
			}
		}
		if (missileTimer >= missileDuration)
		{
			MissileParticle.Stop();
			ObjPoolMgr.Inst.RecycleGO(base.gameObject);
		}
	}
}
