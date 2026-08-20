using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class Elite13_Lightning : MonoBehaviour
{
	public ParticleSystem chargeParticle;

	public ParticleSystem attackParticle;

	public int damage;

	public float knockback;

	public float attackRadius;

	public VariableFloat chargeTime;

	public float particleDelayTime;

	public float arrowSpeed;

	private float existTime;

	private bool exploded;

	private bool particleStarted;

	public ShockParam shock;

	public AnimationCurve warningLightCurve;

	public AnimationCurve attackLightCurve;

	public float lightFadeTime;

	private List<UnitDotsSyncSystem.DistanceHitResult> results = new List<UnitDotsSyncSystem.DistanceHitResult>();

	public void Initialize()
	{
		existTime = 0f;
		exploded = false;
		chargeTime.RandomResult();
		particleStarted = false;
	}

	private void Update()
	{
		existTime += Time.deltaTime;
		if (existTime > particleDelayTime && !particleStarted)
		{
			chargeParticle.Play();
			particleStarted = true;
		}
		if (existTime > chargeTime.result + 2f)
		{
			Elite13.MiniPool.RecycleGO(base.gameObject);
		}
		if (!(existTime > chargeTime.result) || exploded)
		{
			return;
		}
		chargeParticle.Stop();
		attackParticle.Play();
		CamController.Inst.SetShock(shock);
		exploded = true;
		SEMgr.Inst.elite13Lightning.PlaySE();
		UnitDotsSyncSystem.GetCollidersInRange(base.transform.position, attackRadius, GameConst.Filter_MonsterAoe, results);
		for (int i = 0; i < results.Count; i++)
		{
			UnitDotsSyncSystem.DistanceHitResult distanceHitResult = results[i];
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
			{
				if (UnitDotsSyncSystem.TryGetComponent<UnitProperty_Dots>(distanceHitResult.entity, out var result))
				{
					TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(Elite13.Inst.myPpt.myEntity);
					info.damage = damage;
					if (result.unitCfg.unitType == UnitType.NotAttack)
					{
						info.damage = 999999f;
						info.ignoreFloatText = true;
					}
					info.knockbackForce = Tool2D.IgnoreZV2ToV1Normal(distanceHitResult.point, base.transform.position) * knockback;
					info.teammateTakeDamageRatio = 4f;
					UnitDotsSyncSystem.AddTakeDamageRequest(distanceHitResult.entity, info);
				}
				break;
			}
			}
		}
		for (int j = 0; j < 8; j++)
		{
			Vector3 dir = Tool2D.GetDir(45 * j);
			Elite13.MiniPool.GetGO("Prefabs/EF/EF_Elite13_Arrow" + (GameMgr.IsHarmony_Static ? " H" : ""), base.transform.position).GetComponent<Elite13_Arrow>().Initialize(dir, arrowSpeed);
		}
	}
}
