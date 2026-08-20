using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class Monster310_Drop : MonoBehaviour
{
	[Header("伤害")]
	public float range;

	public int damage;

	public bool exploded;

	public float knockback;

	public ShockParam shockParam;

	public Entity master;

	private bool buffed;

	private List<UnitDotsSyncSystem.DistanceHitResult> targetsInRange = new List<UnitDotsSyncSystem.DistanceHitResult>();

	private void Update()
	{
	}

	public void Initialize(Entity master, bool buffed)
	{
		this.master = master;
		this.buffed = buffed;
		Explode();
	}

	private void Explode()
	{
		SEMgr.Inst.monster310_Drop.PlaySE().pitch = Random.Range(0.9f, 1f);
		CamController.Inst.SetShock(shockParam);
		UnitDotsSyncSystem.GetCollidersInRange(base.transform.position, range, GameConst.Filter_MonsterAoe, targetsInRange);
		for (int i = 0; i < targetsInRange.Count; i++)
		{
			UnitDotsSyncSystem.DistanceHitResult distanceHitResult = targetsInRange[i];
			Entity entity = distanceHitResult.entity;
			switch (UnitDotsSyncSystem.GetLayer(entity))
			{
			case 16777216u:
			{
				UnitDotsSyncSystem.ProcessHitSpell(entity, damage, out var _);
				break;
			}
			case 512u:
			case 32768u:
			case 131072u:
			case 2097152u:
				if (UnitDotsSyncSystem.HasComponent<UnitProperty_Dots>(distanceHitResult.entity))
				{
					TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(master);
					info.damage = damage;
					if (buffed)
					{
						info.damage *= 1f;
					}
					info.knockbackForce = Tool2D.IgnoreZV2ToV1Normal(distanceHitResult.point, base.transform.position) * knockback;
					UnitDotsSyncSystem.AddTakeDamageRequestEndless(distanceHitResult.entity, info);
				}
				break;
			}
		}
	}
}
