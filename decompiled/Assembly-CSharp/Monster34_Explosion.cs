using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

public class Monster34_Explosion : MonoBehaviour
{
	public ShockParam shockParam;

	public float explodeDistanceDelta;

	public float interval;

	public int chainedExplodeTimes;

	public float knockback;

	public float boomRadius;

	public int boomDamage;

	private int explodedTimer;

	private float intervalTimer;

	private void OnEnable()
	{
		CamController.Inst.SetShock(shockParam);
		SEMgr.Inst.monster34Explosion.PlaySE();
		ExplodeOnce(base.transform.position);
		explodedTimer = 0;
		intervalTimer = 0f;
	}

	private void Start()
	{
		if (GameMgr.IsMobile_Static)
		{
			chainedExplodeTimes--;
		}
	}

	private void Update()
	{
		if (explodedTimer >= chainedExplodeTimes)
		{
			base.gameObject.SetActive(value: false);
			return;
		}
		intervalTimer += Time.deltaTime;
		if (intervalTimer > interval * (float)(explodedTimer + 1))
		{
			CamController.Inst.SetShock(shockParam);
			SEMgr.Inst.monster34Explosion.PlaySE();
			ExplodeOnce(base.transform.position + Tool2D.GetDir(45f) * (explodedTimer + 1) * explodeDistanceDelta);
			ExplodeOnce(base.transform.position + Tool2D.GetDir(135f) * (explodedTimer + 1) * explodeDistanceDelta);
			ExplodeOnce(base.transform.position + Tool2D.GetDir(225f) * (explodedTimer + 1) * explodeDistanceDelta);
			ExplodeOnce(base.transform.position + Tool2D.GetDir(315f) * (explodedTimer + 1) * explodeDistanceDelta);
			explodedTimer++;
		}
	}

	private void ExplodeOnce(Vector3 explodePoint)
	{
		if (GameMgr.IsHarmony_Static)
		{
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Monster34_ExplosionSingle_H", explodePoint, 6f);
		}
		else
		{
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Monster34_ExplosionSingle", explodePoint, 6f);
		}
		ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Monster34_Trace", explodePoint, 10f);
		EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer(Allocator.TempJob);
		List<UnitDotsSyncSystem.DistanceHitResult> list = new List<UnitDotsSyncSystem.DistanceHitResult>();
		UnitDotsSyncSystem.GetCollidersInRange(explodePoint, boomRadius, GameConst.Filter_MonsterAoeUndiffer, list);
		for (int i = 0; i < list.Count; i++)
		{
			UnitDotsSyncSystem.DistanceHitResult distanceHitResult = list[i];
			Entity entity = distanceHitResult.entity;
			switch (UnitDotsSyncSystem.GetLayer(entity))
			{
			case 16777216u:
			{
				UnitDotsSyncSystem.ProcessHitSpell(entity, boomDamage, out var _);
				break;
			}
			case 512u:
			case 2048u:
			case 4096u:
			case 8192u:
			case 32768u:
			case 131072u:
			case 2097152u:
				if (UnitDotsSyncSystem.HasComponent<UnitProperty_Dots>(distanceHitResult.entity))
				{
					TakeDamageInfo_Dots element = TakeDamageInfo_Dots.NewInfo(AttackerType.NothingSpecial);
					element.damage = boomDamage;
					element.knockbackForce = Tool2D.IgnoreZV2ToV1Normal(distanceHitResult.point, explodePoint) * knockback;
					element.isUndifferDamage = true;
					entityCommandBuffer.AppendToBuffer(distanceHitResult.entity, element);
				}
				break;
			}
		}
		entityCommandBuffer.Playback(UnitDotsSyncSystem.entityMgr);
		entityCommandBuffer.Dispose();
	}
}
