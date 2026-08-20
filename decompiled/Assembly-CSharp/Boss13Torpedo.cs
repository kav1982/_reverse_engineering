using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class Boss13Torpedo : MonoBehaviour
{
	public Vector3 fallSpeed;

	public float warningHeight;

	public float warningTime;

	public float warningTimer;

	private Vector2 berlinSeed;

	public float shakeFrequency;

	public float shakeAmplitude;

	private Vector3 originModelLocalPosition;

	public Transform motion;

	[Header("爆炸参数")]
	public ShockParam shockParam;

	public float knockBack;

	public float explosionRadius;

	public int boomDamage;

	private List<UnitDotsSyncSystem.DistanceHitResult> distanceHits = new List<UnitDotsSyncSystem.DistanceHitResult>();

	private void OnEnable()
	{
		originModelLocalPosition = motion.localPosition;
		warningTimer = 0f;
		berlinSeed = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
	}

	private void Update()
	{
		base.transform.position -= fallSpeed * Time.deltaTime;
		if (base.transform.position.z > warningHeight)
		{
			warningTimer += Time.deltaTime;
			Vector2 vector = berlinSeed * warningTimer * shakeFrequency;
			float x = Mathf.PerlinNoise(vector.x, vector.y) - 0.5f;
			float y = Mathf.PerlinNoise(vector.y, vector.x) - 0.5f;
			motion.localPosition = originModelLocalPosition + new Vector3(x, y, 0f) * shakeAmplitude * warningTimer / warningTime;
		}
		if (base.transform.position.z > 0f)
		{
			ExplodeOnce(base.transform.position);
			ObjPoolMgr.Inst.RecycleGO(base.gameObject);
		}
	}

	private void ExplodeOnce(Vector3 explodePoint)
	{
		ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss13Explosion", explodePoint, 3f).transform.localScale = new Vector3(explosionRadius / 2f + 0.3f, explosionRadius / 2f + 0.3f, 1f);
		CamController.Inst.SetShock(shockParam);
		SEMgr.Inst.monster34Explosion.PlaySE();
		UnitDotsSyncSystem.GetCollidersInRange(explodePoint, explosionRadius, GameConst.Filter_MonsterAoeUndiffer, distanceHits);
		for (int i = 0; i < distanceHits.Count; i++)
		{
			Entity entity = distanceHits[i].entity;
			uint layer = UnitDotsSyncSystem.GetLayer(entity);
			switch (layer)
			{
			case 16777216u:
			{
				UnitDotsSyncSystem.ProcessHitSpell(entity, boomDamage, out var _);
				break;
			}
			case 512u:
			case 32768u:
			case 131072u:
			case 2097152u:
			{
				TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(Boss13.Inst.myPpt.myEntity);
				info.knockbackForce = Tool2D.IgnoreZV2ToV1Normal(distanceHits[i].point, explodePoint) * knockBack;
				info.damage = boomDamage;
				info.isUndifferDamage = true;
				if (layer == 131072)
				{
					info.ignoreFloatText = true;
				}
				UnitDotsSyncSystem.AddTakeDamageRequest(entity, info);
				break;
			}
			}
		}
	}
}
