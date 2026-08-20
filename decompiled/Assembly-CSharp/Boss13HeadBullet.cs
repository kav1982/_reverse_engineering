using Unity.Entities;
using Unity.Physics;
using UnityEngine;

public class Boss13HeadBullet : MonoBehaviour, IDotsTriggerReceiver, IDotsPhysicsReciever
{
	public UnityEngine.CapsuleCollider CC;

	public Rigidbody Rigid;

	public float moveSpeed;

	public float chiledMoveSpeed;

	public float bulletDistance;

	private float movedDistance;

	public float damage;

	public float knockbackForce;

	public float duration;

	public float durationTimer;

	public Entity thisEntity { get; set; }

	private void OnEnable()
	{
		CollisionFilter collisionFilter = default(CollisionFilter);
		collisionFilter.BelongsTo = 1073741824u;
		collisionFilter.CollidesWith = 2231040u;
		collisionFilter.GroupIndex = 0;
		CollisionFilter filter = collisionFilter;
		UnitPhysicsSyncSystem.RegisterReciever(this, filter, CC);
	}

	private void OnDisable()
	{
		durationTimer = 0f;
		UnitPhysicsSyncSystem.UnregisterReciever(this);
	}

	private void FixedUpdate()
	{
		movedDistance += moveSpeed * Time.fixedDeltaTime;
		durationTimer += Time.deltaTime;
		if (durationTimer > duration)
		{
			ObjPoolMgr.Inst.RecycleGO(base.gameObject);
		}
		if (movedDistance > bulletDistance)
		{
			movedDistance -= bulletDistance;
			Boss13TrailBullet component = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss13TrailBullet", base.transform.position).GetComponent<Boss13TrailBullet>();
			component.transform.up = Rigid.linearVelocity.normalized;
			component.Rigid.linearVelocity = Rigid.linearVelocity.normalized * chiledMoveSpeed;
		}
	}

	public void OnTriggerEnter_Dots(Entity other)
	{
		uint layer = UnitDotsSyncSystem.GetLayer(other);
		if (layer != 256 && (layer == 512 || layer == 2097152))
		{
			TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(Boss13.Inst.myPpt.myEntity);
			info.damage = damage;
			info.knockbackForce = Rigid.linearVelocity.normalized * knockbackForce;
			UnitDotsSyncSystem.AddTakeDamageRequest(other, info);
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss13HeadBulletHit", base.transform.position, 2f);
		}
	}

	public void OnTriggerStay_Dots(Entity other)
	{
	}

	public void OnTriggerExit_Dots(Entity other)
	{
	}
}
