using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class Boss51_ParaMissile : LayerCorrect
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

	public Transform bulletHead;

	private Entity master;

	private BallState state;

	private float currentUpSpeed;

	private float horizontalSpeed;

	private Vector3 direction;

	private float explodeAfterTimer;

	[Header("爆炸分裂尝试")]
	public int explodeBulletCount;

	private List<UnitDotsSyncSystem.DistanceHitResult> targetsInRange = new List<UnitDotsSyncSystem.DistanceHitResult>();

	public void InitializeCannon(Vector3 startPoint, Vector3 endPoint, float time, Entity master)
	{
		this.master = master;
		state = BallState.Fly;
		currentUpSpeed = upSpeed;
		shadow.Show();
		direction = -Tool2D.IgnoreZPoint(startPoint - endPoint).normalized;
		horizontalSpeed = Tool2D.IgnoreZPoint(startPoint - endPoint).magnitude / time;
		gravity = GeneralTool.CannonAcceleration(startPoint.z, upSpeed, time);
		ObjPoolMgr.Inst.GetGO("Prefabs/Mixed/WarningArea_Circle", Tool2D.IgnoreZPoint(endPoint)).GetComponent<WarningArea>().Initialize(explosionRadius, time);
		explodeAfterTimer = 0f;
		bulletHead.gameObject.SetActive(value: true);
	}

	private void Update()
	{
		switch (state)
		{
		case BallState.Fly:
		{
			currentUpSpeed += gravity * Time.deltaTime;
			Vector3 vector = horizontalSpeed * direction + new Vector3(0f, 0f, 0f - currentUpSpeed);
			base.transform.position += vector * Time.deltaTime;
			Vector3 to = new Vector3(vector.x, vector.y - vector.z);
			bulletHead.transform.localEulerAngles = new Vector3(0f, 0f, Tool2D.IgnoreZAngleWithSign(Vector3.right, to));
			if (base.transform.position.z >= 0f)
			{
				Explode();
				state = BallState.Explode;
			}
			break;
		}
		case BallState.Explode:
			explodeAfterTimer += Time.deltaTime;
			if (explodeAfterTimer > 3f)
			{
				ObjPoolMgr.Inst.RecycleGO(base.gameObject);
			}
			break;
		}
	}

	private void Explode()
	{
		state = BallState.Explode;
		flyParticle.Stop();
		bulletHead.gameObject.SetActive(value: false);
		ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Monster309_Explosion", base.transform.position, 3f);
		shadow.Hide();
		CamController.Inst.SetShock(shock);
		SEMgr.Inst.monster34Explosion.PlaySE();
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
					TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(master);
					info.damage = damage;
					info.knockbackForce = Tool2D.IgnoreZV2ToV1Normal(distanceHitResult.point, base.transform.position) * knockback;
					UnitDotsSyncSystem.AddTakeDamageRequestEndless(distanceHitResult.entity, info);
				}
				break;
			}
		}
		Boss51.Inst.CreateGroundFire(base.transform.position);
	}
}
