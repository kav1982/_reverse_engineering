using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Entities;
using UnityEngine;

public class Boss56Elite58SpecialBall : MonoBehaviour
{
	public Transform BallTransform;

	public Transform EffectRangeTransform;

	public float ExplosionPullForce;

	public float InDetectRangeExplosionRangeThreshold;

	public float MineRecheckTargetInterval;

	private float recheckTargetTimer;

	private Vector3 moveDirection;

	private float moveDuration;

	private float moveTimer;

	private float moveSpeed;

	private float explosionRange;

	private float explosionDamage;

	private float explosionDelayEffect;

	private float groundEffectExistDuration;

	private float moveSpeedDownRatio;

	private float bulletSpeedDownLerpRatio;

	private bool isStart;

	private bool hastarget;

	private float rotateSpeedUp;

	private float maxRotateAngle;

	private float currentRotateSpeed;

	private float stopRotationTime;

	private void OnEnable()
	{
		moveTimer = 0f;
		hastarget = false;
		isStart = false;
		BallTransform.localScale = Vector3.zero;
		EffectRangeTransform.localScale = Vector3.zero;
		currentRotateSpeed = 0f;
		rotateSpeedUp = 0f;
		maxRotateAngle = 0f;
	}

	public void InitialData(Vector3 moveDirection, float duration, float speed, float explosionDamage, float explosionRange, float explosionExistTime, float explosionDelayEffect, float moveSpeedRatio, float bulletSpeedRatio, float AngleSpeedUp, float maxAngle, float stopRotationTime)
	{
		this.moveDirection = moveDirection;
		moveDuration = duration;
		moveSpeed = speed;
		this.explosionDamage = explosionDamage;
		this.explosionRange = explosionRange;
		this.explosionDelayEffect = explosionDelayEffect;
		groundEffectExistDuration = explosionExistTime;
		moveSpeedDownRatio = moveSpeedRatio;
		bulletSpeedDownLerpRatio = bulletSpeedRatio;
		BallTransform.DOScale(Vector3.one, 1f);
		EffectRangeTransform.DOScale(Vector3.one * explosionRange * InDetectRangeExplosionRangeThreshold, 1f);
		isStart = true;
		rotateSpeedUp = AngleSpeedUp;
		maxRotateAngle = maxAngle;
		this.stopRotationTime = stopRotationTime;
	}

	private void Update()
	{
		if (SpecialObj301EndlessMonsterSpawner.Inst.StageFinished)
		{
			ObjPoolMgr.Inst.RecycleGO(base.gameObject);
		}
		else
		{
			if (!isStart)
			{
				return;
			}
			moveTimer += Time.deltaTime;
			currentRotateSpeed += rotateSpeedUp * Time.deltaTime;
			if (rotateSpeedUp < 0f)
			{
				currentRotateSpeed = Mathf.Max(currentRotateSpeed, maxRotateAngle);
			}
			else
			{
				currentRotateSpeed = Mathf.Min(currentRotateSpeed, maxRotateAngle);
			}
			if (moveTimer < stopRotationTime)
			{
				moveDirection = Tool2D.GetDir(moveDirection, currentRotateSpeed * Time.deltaTime);
			}
			base.transform.position += moveDirection * moveSpeed * Time.deltaTime;
			recheckTargetTimer += Time.deltaTime;
			if (recheckTargetTimer >= MineRecheckTargetInterval)
			{
				recheckTargetTimer -= MineRecheckTargetInterval;
				List<UnitDotsSyncSystem.DistanceHitResult> list = new List<UnitDotsSyncSystem.DistanceHitResult>();
				UnitDotsSyncSystem.GetCollidersInRange(base.transform.position, explosionRange * InDetectRangeExplosionRangeThreshold * BallTransform.localScale.x, GameConst.Filter_MonsterAoe, list);
				for (int i = 0; i < list.Count; i++)
				{
					uint layer = UnitDotsSyncSystem.GetLayer(list[i].entity);
					if (layer == 512 || layer == 2097152)
					{
						hastarget = true;
					}
				}
			}
			if (hastarget || moveTimer >= moveDuration)
			{
				StartCoroutine(BombExplosion());
			}
		}
	}

	private IEnumerator BombExplosion()
	{
		isStart = false;
		ObjPoolMgr.Inst.GetGO("Prefabs/Mixed/WarningArea_Circle", Tool2D.IgnoreZPoint(base.transform.position)).GetComponent<WarningArea>().Initialize(explosionRange, explosionDelayEffect);
		BallTransform.DOScale(0.2f, explosionDelayEffect);
		yield return new WaitForSeconds(explosionDelayEffect);
		ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Elite58_Explosion", base.transform.position.IgnoreZ()).transform.localScale = Vector3.one * explosionRange / 2f;
		SEMgr.Inst.elite58Explosion.PlaySE();
		List<UnitDotsSyncSystem.DistanceHitResult> list = new List<UnitDotsSyncSystem.DistanceHitResult>();
		UnitDotsSyncSystem.GetCollidersInRange(base.transform.position, explosionRange, GameConst.Filter_MonsterAoe, list);
		for (int i = 0; i < list.Count; i++)
		{
			UnitDotsSyncSystem.DistanceHitResult distanceHitResult = list[i];
			Entity entity = distanceHitResult.entity;
			switch (UnitDotsSyncSystem.GetLayer(entity))
			{
			case 16777216u:
			{
				UnitDotsSyncSystem.ProcessHitSpell(entity, explosionDamage, out var _);
				break;
			}
			case 512u:
			case 32768u:
			case 131072u:
			case 2097152u:
				if (UnitDotsSyncSystem.HasComponent<UnitProperty_Dots>(distanceHitResult.entity))
				{
					TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(AttackerType.NothingSpecial);
					info.damage = explosionDamage;
					UnitDotsSyncSystem.AddTakeDamageRequest(distanceHitResult.entity, info);
				}
				break;
			}
		}
		ObjPoolMgr.Inst.RecycleGO(base.gameObject);
	}

	public void ForceEnd()
	{
		ObjPoolMgr.Inst.RecycleGO(base.gameObject);
	}
}
