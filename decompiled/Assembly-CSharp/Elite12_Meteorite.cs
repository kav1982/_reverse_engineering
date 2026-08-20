using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class Elite12_Meteorite : LayerCorrect
{
	private enum BallState
	{
		Fly,
		FlyEnd
	}

	[Space(50f)]
	public SpriteRenderer SR_Bullet;

	public ParticleSystem[] pss;

	public float upSpeed;

	public float gravity;

	public float explosionRadius;

	public float flyEndWaitTime;

	public float flyTime;

	public int damage;

	public float knockback;

	public ShockParam shock;

	public Shadow shadow;

	private BallState state;

	private MiniObjPool miniPool;

	private float currentUpSpeed;

	private float horizontalSpeed;

	private Vector3 direction;

	private float flyEndWaitTimer;

	private List<UnitDotsSyncSystem.DistanceHitResult> targetsInRange = new List<UnitDotsSyncSystem.DistanceHitResult>();

	public void Initialize(MiniObjPool miniPool, Vector3 startPoint, Vector3 endPoint, float time)
	{
		SR_Bullet.enabled = true;
		this.miniPool = miniPool;
		state = BallState.Fly;
		for (int i = 0; i < pss.Length; i++)
		{
			pss[i].Play();
		}
		currentUpSpeed = upSpeed;
		flyEndWaitTimer = 0f;
		shadow.Show();
		if (GameMgr.IsMobile_Static)
		{
			time *= 1.15f;
		}
		direction = -Tool2D.IgnoreZPoint(startPoint - endPoint).normalized;
		horizontalSpeed = Tool2D.IgnoreZPoint(startPoint - endPoint).magnitude / time;
		gravity = GeneralTool.CannonAcceleration(startPoint.z, upSpeed, time);
		miniPool.GetGO("Prefabs/Mixed/WarningArea_Circle" + (GameMgr.IsHarmony_Static ? " Purple" : ""), Tool2D.IgnoreZPoint(endPoint)).GetComponent<WarningArea>().Initialize(explosionRadius, time);
		SR_Bullet.flipX = direction.x > 0f;
	}

	private void Update()
	{
		switch (state)
		{
		case BallState.Fly:
			currentUpSpeed += gravity * Time.deltaTime;
			base.transform.position += horizontalSpeed * direction * Time.deltaTime + new Vector3(0f, 0f, 0f - currentUpSpeed) * Time.deltaTime;
			if (base.transform.position.z >= 0f)
			{
				base.transform.IgnoreZPoint();
				SR_Bullet.enabled = false;
				Explode();
				shadow.Hide();
				state = BallState.FlyEnd;
				for (int i = 0; i < pss.Length; i++)
				{
					pss[i].Stop();
				}
				miniPool.GetGO("Prefabs/EF/EF_Elite12_Trace", Tool2D.GetLayerPoint(base.transform.position, LayerCorrectType.Shadow), 22f);
				miniPool.GetGO("Prefabs/EF/EF_Elite12_MeteoriteExplosion" + (GameMgr.IsHarmony_Static ? " H" : ""), base.transform.position, 2f);
			}
			break;
		case BallState.FlyEnd:
			flyEndWaitTimer += Time.deltaTime;
			if (flyEndWaitTimer > flyEndWaitTime)
			{
				flyEndWaitTimer = 0f;
				miniPool.RecycleGO(base.gameObject);
			}
			break;
		default:
			Debug.LogError(state);
			break;
		}
	}

	private void Explode()
	{
		CamController.Inst.SetShock(shock);
		SEMgr.Inst.elite12MeteorHit.PlaySE();
		UnitDotsSyncSystem.GetCollidersInRange(base.transform.position, explosionRadius, GameConst.Filter_MonsterAoe, targetsInRange);
		for (int i = 0; i < targetsInRange.Count; i++)
		{
			UnitDotsSyncSystem.DistanceHitResult distanceHitResult = targetsInRange[i];
			Entity entity = targetsInRange[i].entity;
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
					TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(Elite12_2.Inst.myPpt.myEntity);
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
	}
}
