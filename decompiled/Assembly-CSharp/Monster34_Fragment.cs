using System.Collections.Generic;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Stateful;
using UnityEngine;

public class Monster34_Fragment : LayerCorrect, IDotsCollisionReceiver, IDotsPhysicsReciever
{
	[Space(50f)]
	private float horizontalSpeed;

	public float gravity;

	public VariableFloat upSpeed;

	public Rigidbody rigid;

	public UnityEngine.CapsuleCollider cc;

	public Transform tsf_Rotate;

	public float rotateSpeed;

	public float colliderRadius;

	public float fallInClifDisntace;

	public Shadow shadow;

	public ParticleSystem fireParticle;

	private Vector3 landPoint;

	private float currentZSpeed;

	public ShockParam shockParam;

	public float knockback;

	public float boomRadius;

	public int boomDamage;

	private bool explodeNow;

	private int bounceTime;

	private int bounceCount;

	private bool done;

	private bool thisFrameCollides;

	public Entity thisEntity { get; set; }

	private void OnDisable()
	{
		UnitPhysicsSyncSystem.UnregisterReciever(this);
	}

	public void Iniaitlize(Vector3 landPoint, int bounceTime = 0, bool explodeNow = false)
	{
		this.explodeNow = explodeNow;
		if (explodeNow)
		{
			landPoint = base.transform.position;
		}
		this.bounceTime = bounceTime;
		bounceCount = 0;
		done = false;
		fireParticle.Play();
		this.landPoint = landPoint;
		float horizontalDistance = Tool2D.IgnoreZDistance(base.transform.position, landPoint);
		tsf_Rotate.gameObject.SetActive(value: true);
		shadow.ShadowGO.SetActive(value: true);
		horizontalSpeed = GeneralTool.CannonSpeed(upSpeed.RandomResult(), 0f - base.transform.position.z, gravity, horizontalDistance);
		rigid.linearVelocity = Tool2D.IgnoreZV2ToV1Normal(landPoint, base.transform.position) * horizontalSpeed;
		currentZSpeed = upSpeed.result;
		CollisionFilter collisionFilter = default(CollisionFilter);
		collisionFilter.BelongsTo = 1073741824u;
		collisionFilter.CollidesWith = 256u;
		CollisionFilter filter = collisionFilter;
		UnitPhysicsSyncSystem.RegisterReciever(this, filter, cc);
	}

	private void Update()
	{
		thisFrameCollides = false;
		if (bounceCount != bounceTime && !explodeNow)
		{
			currentZSpeed += gravity * Time.deltaTime;
			base.transform.position -= new Vector3(0f, 0f, currentZSpeed) * Time.deltaTime;
			tsf_Rotate.Rotate(0f, 0f, rotateSpeed * Time.deltaTime);
		}
		if (((base.transform.position.z > 0f && bounceCount < bounceTime && currentZSpeed < 0f) || explodeNow) && !done)
		{
			bounceCount++;
			if (GeneralTool.HaveCollider(Tool2D.IgnoreZPoint(base.transform), colliderRadius * base.transform.localScale.x, "Abyss", "Abyss") != null)
			{
				bounceCount = bounceTime;
			}
			if ((Tool2D.GetNavMeshPointIngoreZ(Tool2D.IgnoreZPoint(base.transform), 8) - base.transform.position).sqrMagnitude > fallInClifDisntace * fallInClifDisntace)
			{
				bounceCount = bounceTime;
			}
			if (bounceCount >= bounceTime || explodeNow)
			{
				done = true;
				fireParticle.Stop();
				tsf_Rotate.gameObject.SetActive(value: false);
				shadow.ShadowGO.SetActive(value: false);
				rigid.linearVelocity = Vector3.zero;
				ObjPoolMgr.Inst.RecycleGO(base.gameObject, 1f);
			}
			ExplodeOnce(Tool2D.IgnoreZPoint(base.transform.position));
			currentZSpeed = 0f - currentZSpeed;
		}
	}

	void IDotsCollisionReceiver.OnCollisionEnter_Dots(StatefulCollisionEvent collision)
	{
		if (!thisFrameCollides)
		{
			Vector3 inNormal = collision.GetNormalFrom(collision.GetOtherEntity(thisEntity));
			rigid.linearVelocity = Vector3.Reflect(rigid.linearVelocity, inNormal);
			rigid.linearVelocity = rigid.linearVelocity.normalized * horizontalSpeed;
		}
	}

	void IDotsCollisionReceiver.OnCollisionStay_Dots(StatefulCollisionEvent collision)
	{
	}

	void IDotsCollisionReceiver.OnCollisionExit_Dots(StatefulCollisionEvent collision)
	{
	}

	private void ExplodeOnce(Vector3 explodePoint)
	{
		ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Monster34_ExplosionSingle", explodePoint, 6f);
		ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Monster34_Trace", explodePoint, 10f);
		CamController.Inst.SetShock(shockParam);
		SEMgr.Inst.monster34Explosion.PlaySE();
		List<UnitDotsSyncSystem.DistanceHitResult> list = new List<UnitDotsSyncSystem.DistanceHitResult>();
		UnitDotsSyncSystem.GetCollidersInRange(explodePoint, boomRadius, GameConst.Filter_MonsterAoeUndiffer, list);
		for (int i = 0; i < list.Count; i++)
		{
			UnitDotsSyncSystem.DistanceHitResult distanceHitResult = list[i];
			Entity entity = distanceHitResult.entity;
			switch (UnitDotsSyncSystem.GetLayer(entity))
			{
			case 16777216u:
			{
				UnitDotsSyncSystem.ProcessHitSpell(entity, boomDamage, out var _);
				break;
			}
			case 512u:
			case 2048u:
			case 4096u:
			case 8192u:
			case 32768u:
			case 131072u:
			case 2097152u:
				if (UnitDotsSyncSystem.HasComponent<UnitProperty_Dots>(distanceHitResult.entity))
				{
					TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(AttackerType.NothingSpecial);
					info.damage = boomDamage;
					info.knockbackForce = Tool2D.IgnoreZV2ToV1Normal(distanceHitResult.point, explodePoint) * knockback;
					info.isUndifferDamage = true;
					UnitDotsSyncSystem.AddTakeDamageRequest(distanceHitResult.entity, info);
				}
				break;
			}
		}
	}
}
