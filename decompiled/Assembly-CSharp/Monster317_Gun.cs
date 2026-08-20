using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class Monster317_Gun : LayerCorrect
{
	public enum GunState
	{
		Fly,
		End
	}

	public float explosionRadius;

	public int damage;

	public float knockback;

	public ShockParam shock;

	public ParticleSystem flyParticle;

	public Transform bulletHead;

	public GunState state;

	private float horizontalSpeed;

	private float fallSpeed;

	private float endAfterTimer;

	private Vector3 direction;

	private Vector3 endPoint;

	private Entity master;

	private bool initialized;

	private bool buffed;

	private List<UnitDotsSyncSystem.DistanceHitResult> targetsInRange = new List<UnitDotsSyncSystem.DistanceHitResult>();

	public void InitializeGun(Vector3 startPoint, Vector3 endPoint, float time, Entity master, bool buffed)
	{
		initialized = true;
		state = GunState.Fly;
		endAfterTimer = 0f;
		base.transform.position = startPoint;
		this.endPoint = endPoint;
		this.master = master;
		this.buffed = buffed;
		float num = Mathf.Max(time, 0.01f);
		direction = -Tool2D.IgnoreZPoint(startPoint - endPoint).normalized;
		horizontalSpeed = Tool2D.IgnoreZPoint(startPoint - endPoint).magnitude / num;
		fallSpeed = (endPoint.z - startPoint.z) / num;
		flyParticle.Play();
		bulletHead.gameObject.SetActive(value: true);
	}

	private void Update()
	{
		if (!initialized)
		{
			return;
		}
		switch (state)
		{
		case GunState.Fly:
		{
			Vector3 vector = horizontalSpeed * direction + new Vector3(0f, 0f, fallSpeed);
			base.transform.position += vector * Time.deltaTime;
			Vector3 to = new Vector3(vector.x, vector.y - vector.z);
			bulletHead.transform.localEulerAngles = new Vector3(0f, 0f, Tool2D.IgnoreZAngleWithSign(Vector3.right, to));
			if (base.transform.position.z >= 0f)
			{
				Explode();
			}
			break;
		}
		case GunState.End:
			endAfterTimer += Time.deltaTime;
			if (endAfterTimer > 3f)
			{
				ObjPoolMgr.Inst.RecycleGO(base.gameObject);
			}
			break;
		}
	}

	private void Explode()
	{
		state = GunState.End;
		base.transform.position = endPoint;
		flyParticle.Stop();
		bulletHead.gameObject.SetActive(value: false);
		ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Monster317_Hit", base.transform.position, Quaternion.identity, Vector3.one * explosionRadius, 3f);
		CamController.Inst.SetShock(shock);
		SEMgr.Inst.monster317_Hit.PlaySE().pitch = Random.Range(0.9f, 1f);
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
					if (buffed)
					{
						info.damage *= 1f;
					}
					info.knockbackForce = Tool2D.IgnoreZV2ToV1Normal(distanceHitResult.point, base.transform.position) * knockback;
					UnitDotsSyncSystem.AddTakeDamageRequest(distanceHitResult.entity, info);
				}
				break;
			}
		}
	}
}
