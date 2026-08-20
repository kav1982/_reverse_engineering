using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class Monster49_Trample : MonoBehaviour
{
	public Transform tsf_LayerExplosion;

	public Transform tsf_LayerCrack;

	public float damageRadius;

	public int damageForPlayer;

	public float frozenTime;

	public float afterTime;

	public ShockParam shock;

	private float afterTimer;

	public Monster49 master;

	private bool Frame1Initialized;

	private float mobileFix;

	private void OnEnable()
	{
		afterTimer = 0f;
		mobileFix = (GameMgr.IsMobile_Static ? 0.8f : 1f);
		tsf_LayerExplosion.transform.localScale = Vector3.one * mobileFix;
		tsf_LayerExplosion.position = Tool2D.GetLayerPoint(base.transform);
		tsf_LayerCrack.transform.localScale = Vector3.one * mobileFix;
		tsf_LayerCrack.position = Tool2D.GetLayerPoint(base.transform, LayerCorrectType.ExplosionTrace);
		SEMgr.Inst.monster49_Trample.PlaySE();
		CamController.Inst.SetShock(shock);
		Frame1Initialized = false;
	}

	private void Update()
	{
		if (!Frame1Initialized)
		{
			Frame1Initialized = true;
			List<UnitDotsSyncSystem.DistanceHitResult> list = new List<UnitDotsSyncSystem.DistanceHitResult>();
			UnitDotsSyncSystem.GetCollidersInRange(base.transform.position, mobileFix * damageRadius, GameConst.Filter_MonsterAoe, list);
			for (int i = 0; i < list.Count; i++)
			{
				UnitDotsSyncSystem.DistanceHitResult distanceHitResult = list[i];
				Entity entity = distanceHitResult.entity;
				switch (UnitDotsSyncSystem.GetLayer(entity))
				{
				case 16777216u:
				{
					UnitDotsSyncSystem.ProcessHitSpell(entity, damageForPlayer, out var _);
					break;
				}
				case 512u:
				case 32768u:
				case 131072u:
				case 2097152u:
				{
					if (UnitDotsSyncSystem.TryGetComponent<UnitProperty_Dots>(distanceHitResult.entity, out var result))
					{
						result.SetFrozen(frozenTime);
						UnitDotsSyncSystem.SetComponentData(result, distanceHitResult.entity);
						TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(master.myPpt.myEntity);
						info.damage = damageForPlayer;
						info.teammateTakeDamageRatio = 3f;
						UnitDotsSyncSystem.AddTakeDamageRequest(distanceHitResult.entity, info);
					}
					break;
				}
				}
			}
		}
		afterTimer += Time.deltaTime;
		if (afterTimer >= afterTime)
		{
			ObjPoolMgr.Inst.RecycleGO(base.gameObject);
		}
	}
}
