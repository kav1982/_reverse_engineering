using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class Elite58ManaDrainMine : MonoBehaviour
{
	private static readonly int Progress = Shader.PropertyToID("_Progress");

	private static readonly int Transparency = Shader.PropertyToID("_Transparency");

	public TrailRenderer MineTrail;

	public Transform MineRotateTransform;

	public Transform MineRangeAlertTransform;

	public SpriteRenderer OuterRingSprite;

	public SpriteRenderer BaseColorSprite;

	private float MineExplosionRange;

	private float MineExplosionDamage;

	private float MineTriggerDelayExplosionTime;

	private float MineManaDrainPercent;

	private float MineExistTime;

	public float MineLandTime;

	public float MineFallGravity;

	public float MineRecheckTargetInterval;

	public float MineKnockBackForce;

	private float verticalSpeed;

	private bool isLand;

	private float mineTimer;

	private Vector3 moveDirection = Vector3.zero;

	private float horizontalSpeed;

	private bool isMineTriggered;

	private float recheckTargetTimer;

	private float remainDistance;

	private float finalGravity;

	private void OnEnable()
	{
		isLand = false;
		mineTimer = 0f;
		verticalSpeed = 0f;
		isMineTriggered = false;
		MineTrail.enabled = false;
		MineRangeAlertTransform.gameObject.SetActive(value: false);
		MineTrail.Clear();
		horizontalSpeed = 0f;
		recheckTargetTimer = 0f;
		OuterRingSprite.material.SetFloat(Progress, 0f);
		BaseColorSprite.material.SetFloat(Transparency, 0f);
	}

	public void InitialMineData(float mineDamage, float mineRadius, float mineExistDuration, float mineDelayTriggerTime, float manaDrainPercent, Vector3 shootDir, float targetRadius, float initialHeight, float overridePullForce = -1f)
	{
		MineExplosionDamage = mineDamage;
		MineExplosionRange = mineRadius;
		MineExistTime = mineExistDuration;
		MineTriggerDelayExplosionTime = mineDelayTriggerTime;
		MineManaDrainPercent = manaDrainPercent;
		remainDistance = targetRadius;
		moveDirection = shootDir;
		horizontalSpeed = targetRadius / MineLandTime;
		verticalSpeed = 0f - (MineFallGravity * MineLandTime * 0.5f - initialHeight / MineLandTime);
		MineTrail.Clear();
		MineTrail.enabled = true;
		finalGravity = ((overridePullForce >= 0f) ? overridePullForce : MineKnockBackForce);
		MineRangeAlertTransform.localScale = Vector3.one * mineRadius;
		MineRotateTransform.rotation = quaternion.Euler(0f, 0f, Mathf.Atan2(shootDir.y, shootDir.x) * 57.29578f);
		MineRotateTransform.DORotate(Vector3.zero, MineLandTime);
	}

	private void Update()
	{
		if (SpecialObj301EndlessMonsterSpawner.Inst.StageFinished)
		{
			if (!isLand || isMineTriggered)
			{
				ObjPoolMgr.Inst.RecycleGO(base.gameObject);
			}
			else
			{
				Explosion();
			}
			return;
		}
		mineTimer += Time.deltaTime;
		if (!isLand)
		{
			float num = Mathf.Min(base.transform.position.z + verticalSpeed * Time.deltaTime, 0f);
			float num2 = Mathf.Min(horizontalSpeed * Time.deltaTime, remainDistance);
			remainDistance -= num2;
			base.transform.position = base.transform.position.IgnoreZ() + moveDirection * num2 + new Vector3(0f, 0f, num);
			verticalSpeed += MineFallGravity * Time.deltaTime;
			if (num >= 0f)
			{
				isLand = true;
				MineTrail.emitting = false;
				if (remainDistance >= 0f)
				{
					base.transform.position += moveDirection * remainDistance;
				}
				if (!Tool2D.PointOnNavMesh(base.transform.position.IgnoreZ()))
				{
					isMineTriggered = true;
					Explosion(playSE: false);
				}
				MineRangeAlertTransform.gameObject.SetActive(value: true);
				OuterRingSprite.material.DOFloat(1f, Progress, 0.8f);
				BaseColorSprite.material.DOFloat(0.3f, Transparency, 0.8f);
			}
		}
		else
		{
			if (isMineTriggered)
			{
				return;
			}
			if (mineTimer >= MineExistTime)
			{
				isMineTriggered = true;
				Explosion();
				return;
			}
			recheckTargetTimer += Time.deltaTime;
			if (recheckTargetTimer < MineRecheckTargetInterval)
			{
				return;
			}
			recheckTargetTimer -= MineRecheckTargetInterval;
			List<UnitDotsSyncSystem.DistanceHitResult> list = new List<UnitDotsSyncSystem.DistanceHitResult>();
			UnitDotsSyncSystem.GetCollidersInRange(base.transform.position, MineExplosionRange, GameConst.Filter_MonsterAoe, list);
			for (int i = 0; i < list.Count; i++)
			{
				UnitDotsSyncSystem.DistanceHitResult distanceHitResult = list[i];
				uint layer = UnitDotsSyncSystem.GetLayer(distanceHitResult.entity);
				if ((layer == 512 || layer == 2097152) && UnitDotsSyncSystem.HasComponent<UnitProperty_Dots>(distanceHitResult.entity))
				{
					StartCoroutine(TriggerMine());
					break;
				}
			}
		}
	}

	private IEnumerator TriggerMine(bool playSE = true)
	{
		isMineTriggered = true;
		yield return new WaitForSeconds(MineTriggerDelayExplosionTime);
		Explosion(playSE);
	}

	private void Explosion(bool playSE = true)
	{
		ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Elite58_Explosion", base.transform.position.IgnoreZ());
		List<UnitDotsSyncSystem.DistanceHitResult> list = new List<UnitDotsSyncSystem.DistanceHitResult>();
		UnitDotsSyncSystem.GetCollidersInRange(base.transform.position, MineExplosionRange, GameConst.Filter_MonsterAoe, list);
		for (int i = 0; i < list.Count; i++)
		{
			UnitDotsSyncSystem.DistanceHitResult distanceHitResult = list[i];
			Entity entity = distanceHitResult.entity;
			switch (UnitDotsSyncSystem.GetLayer(entity))
			{
			case 16777216u:
			{
				UnitDotsSyncSystem.ProcessHitSpell(entity, MineExplosionDamage + 10f, out var _);
				break;
			}
			case 512u:
			{
				if (!UnitDotsSyncSystem.HasComponent<UnitProperty_Dots>(distanceHitResult.entity))
				{
					break;
				}
				TakeDamageInfo_Dots info2 = TakeDamageInfo_Dots.NewInfo(AttackerType.NothingSpecial);
				info2.damage = MineExplosionDamage;
				info2.knockbackForce = -Tool2D.IgnoreZV2ToV1Normal(distanceHitResult.point, base.transform.position) * 5f * finalGravity;
				UnitDotsSyncSystem.AddTakeDamageRequest(distanceHitResult.entity, info2);
				foreach (Wand wand in PlayerMgr.Inst.Wands)
				{
					wand.CostMp(Mathf.Min(wand.CurrentMP, wand.MaxMP * MineManaDrainPercent / 100f));
				}
				break;
			}
			case 32768u:
			case 131072u:
			case 2097152u:
				if (UnitDotsSyncSystem.HasComponent<UnitProperty_Dots>(distanceHitResult.entity))
				{
					TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(AttackerType.NothingSpecial);
					info.damage = MineExplosionDamage;
					info.knockbackForce = -Tool2D.IgnoreZV2ToV1Normal(distanceHitResult.point, base.transform.position) * 5f * finalGravity;
					UnitDotsSyncSystem.AddTakeDamageRequest(distanceHitResult.entity, info);
				}
				break;
			}
		}
		if (playSE)
		{
			SEMgr.Inst.elite58MineExplosion.PlaySE();
		}
		ObjPoolMgr.Inst.RecycleGO(base.gameObject);
	}
}
