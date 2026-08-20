using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class Boss13StrafeBullet : MonoBehaviour
{
	public TrailRenderer trail;

	public Vector3 fallDir;

	public float fallSpeed;

	[Header("爆炸参数")]
	public ShockParam shockParam;

	public float knockBack;

	public float explosionRadius;

	public int boomDamage;

	public float objDamageFactor;

	private List<UnitDotsSyncSystem.DistanceHitResult> distanceHits = new List<UnitDotsSyncSystem.DistanceHitResult>();

	private void OnEnable()
	{
		trail.Clear();
	}

	private void Update()
	{
		base.transform.position += (fallDir + new Vector3(0f, 0f, fallSpeed)) * Time.deltaTime;
		if (base.transform.position.z > 0f)
		{
			ExplodeOnce(base.transform.position);
			ObjPoolMgr.Inst.RecycleGO(base.gameObject);
		}
	}

	private void ExplodeOnce(Vector3 explodePoint)
	{
		ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss13Dust", explodePoint, 3f);
		CamController.Inst.SetShock(shockParam);
		SEMgr.Inst.monster34Explosion.PlaySE(SEPlayMode.Replay, 3, 0.2f);
		UnitDotsSyncSystem.GetCollidersInRange(explodePoint, explosionRadius, GameConst.Filter_MonsterAoeUndiffer, distanceHits);
		for (int i = 0; i < distanceHits.Count; i++)
		{
			Entity entity = distanceHits[i].entity;
			uint layer = UnitDotsSyncSystem.GetLayer(entity);
			TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(Boss13.Inst.myPpt.myEntity);
			switch (layer)
			{
			case 16777216u:
			{
				UnitDotsSyncSystem.ProcessHitSpell(entity, boomDamage, out var _);
				break;
			}
			case 512u:
			case 32768u:
			case 2097152u:
				info.knockbackForce = Tool2D.IgnoreZV2ToV1Normal(distanceHits[i].point, explodePoint) * knockBack;
				info.damage = boomDamage;
				info.teammateTakeDamageRatio = 4f;
				UnitDotsSyncSystem.AddTakeDamageRequest(entity, info);
				break;
			case 131072u:
				info.knockbackForce = Tool2D.IgnoreZV2ToV1Normal(distanceHits[i].point, explodePoint) * knockBack;
				info.damage = (float)boomDamage * objDamageFactor;
				info.ignoreFloatText = true;
				UnitDotsSyncSystem.AddTakeDamageRequest(entity, info);
				break;
			}
		}
	}
}
