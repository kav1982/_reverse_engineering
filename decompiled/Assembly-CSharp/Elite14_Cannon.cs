using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class Elite14_Cannon : LayerCorrect
{
	private enum BallState
	{
		Fly,
		Explode
	}

	[Space(50f)]
	public float upSpeed;

	public float gravity;

	public float explosionRadius;

	public float flyTime;

	public int damage;

	public float knockback;

	public ShockParam shock;

	public Shadow shadow;

	public ParticleSystem flyParticle;

	public GameObject bulletHead;

	public ParticleSystem explodeParticle;

	private Elite14_Child master;

	private BallState state;

	private float currentUpSpeed;

	private float horizontalSpeed;

	private Vector3 direction;

	private bool immidiatelyExplode;

	private float explodeAfterTimer;

	private List<UnitDotsSyncSystem.DistanceHitResult> targetsInRange = new List<UnitDotsSyncSystem.DistanceHitResult>();

	public void InitializeCannon(Vector3 startPoint, Vector3 endPoint, float time, Elite14_Child master)
	{
		this.master = master;
		immidiatelyExplode = false;
		state = BallState.Fly;
		currentUpSpeed = upSpeed;
		shadow.Show();
		direction = -Tool2D.IgnoreZPoint(startPoint - endPoint).normalized;
		horizontalSpeed = Tool2D.IgnoreZPoint(startPoint - endPoint).magnitude / time;
		gravity = GeneralTool.CannonAcceleration(startPoint.z, upSpeed, time);
		Elite14.MiniPool.GetGO("Prefabs/Mixed/WarningArea_Circle purple", Tool2D.IgnoreZPoint(endPoint)).GetComponent<WarningArea>().Initialize(explosionRadius, time);
		explodeAfterTimer = 0f;
		bulletHead.SetActive(value: true);
	}

	public void InitializeExplosion()
	{
		immidiatelyExplode = true;
		state = BallState.Fly;
		explodeAfterTimer = 0f;
	}

	private void Update()
	{
		if (immidiatelyExplode && state != BallState.Explode)
		{
			Explode();
		}
		switch (state)
		{
		case BallState.Fly:
			currentUpSpeed += gravity * Time.deltaTime;
			base.transform.position += horizontalSpeed * direction * Time.deltaTime + new Vector3(0f, 0f, 0f - currentUpSpeed) * Time.deltaTime;
			if (base.transform.position.z >= 0f)
			{
				Explode();
				state = BallState.Explode;
			}
			break;
		case BallState.Explode:
			explodeAfterTimer += Time.deltaTime;
			if (explodeAfterTimer > 3f)
			{
				Elite14.MiniPool.RecycleGO(base.gameObject);
			}
			break;
		}
	}

	private void Explode()
	{
		state = BallState.Explode;
		flyParticle.Stop();
		bulletHead.SetActive(value: false);
		explodeParticle.Play();
		shadow.Hide();
		CamController.Inst.SetShock(shock);
		SEMgr.Inst.elite12MeteorHit.PlaySE();
		UnitDotsSyncSystem.GetCollidersInRange(base.transform.position, explosionRadius, GameConst.Filter_MonsterAoe, targetsInRange);
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
					TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(master.myPpt.myEntity);
					info.damage = damage;
					info.knockbackForce = Tool2D.IgnoreZV2ToV1Normal(distanceHitResult.point, base.transform.position) * knockback;
					info.teammateTakeDamageRatio = 4f;
					UnitDotsSyncSystem.AddTakeDamageRequest(distanceHitResult.entity, info);
				}
				break;
			}
		}
	}
}
