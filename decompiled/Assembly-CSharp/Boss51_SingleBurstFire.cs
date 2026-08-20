using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class Boss51_SingleBurstFire : MonoBehaviour
{
	public float delayTime;

	public float damageTime;

	public float range;

	public float damage;

	private float delayTimer;

	private float damageCheckTimer;

	private List<Entity> damagedTarget = new List<Entity>();

	private List<UnitDotsSyncSystem.DistanceHitResult> targetsInRange = new List<UnitDotsSyncSystem.DistanceHitResult>();

	private void OnEnable()
	{
		delayTimer = 0f;
		damageCheckTimer = 0f;
	}

	private void Update()
	{
		delayTimer += Time.deltaTime;
		if (delayTimer > delayTime && delayTimer < delayTime + damageTime)
		{
			damageCheckTimer -= Time.deltaTime;
			if (damageCheckTimer < 0f)
			{
				damageCheckTimer += 0.1f;
				Burn();
			}
		}
		if (delayTimer > 2f)
		{
			ObjPoolMgr.Inst.RecycleGO(base.gameObject);
		}
	}

	private void Burn()
	{
		SEMgr.Inst.Boss51_BurstFire.PlaySE(base.transform.position, SEPlayMode.Replay, 10, 0.15f);
		UnitDotsSyncSystem.GetCollidersInRange(base.transform.position, range, GameConst.Filter_MonsterAoeNoSpell, targetsInRange);
		for (int i = 0; i < targetsInRange.Count; i++)
		{
			UnitDotsSyncSystem.DistanceHitResult distanceHitResult = targetsInRange[i];
			if (!damagedTarget.Contains(distanceHitResult.entity))
			{
				damagedTarget.Add(distanceHitResult.entity);
				UnitDotsSyncSystem.GetComponentData<UnitProperty_Dots>(distanceHitResult.entity);
				TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(AttackerType.NothingSpecial);
				info.damage = damage;
				UnitDotsSyncSystem.AddTakeDamageRequestEndless(distanceHitResult.entity, info);
				SEMgr.Inst.elite9Burn.PlaySE();
			}
		}
	}
}
