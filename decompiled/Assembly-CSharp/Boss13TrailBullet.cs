using Unity.Entities;
using Unity.Physics;
using UnityEngine;

public class Boss13TrailBullet : MonoBehaviour, IDotsTriggerReceiver, IDotsPhysicsReciever
{
	public UnityEngine.CapsuleCollider CC;

	public Rigidbody Rigid;

	public float damage;

	public float knockbackForce;

	public float exitDuration;

	public float exitDurationTimer;

	public bool startTiming;

	public Entity thisEntity { get; set; }

	private void OnEnable()
	{
		CollisionFilter collisionFilter = default(CollisionFilter);
		collisionFilter.BelongsTo = 1073741824u;
		collisionFilter.CollidesWith = 2231040u;
		collisionFilter.GroupIndex = 0;
		CollisionFilter filter = collisionFilter;
		UnitPhysicsSyncSystem.RegisterReciever(this, filter, CC);
		startTiming = false;
		exitDurationTimer = 0f;
	}

	private void OnDisable()
	{
		UnitPhysicsSyncSystem.UnregisterReciever(this);
	}

	private void Update()
	{
		exitDurationTimer += Time.deltaTime;
		if (exitDurationTimer > exitDuration)
		{
			ObjPoolMgr.Inst.RecycleGO(base.gameObject);
		}
	}

	public void OnTriggerEnter_Dots(Entity other)
	{
		switch (UnitDotsSyncSystem.GetLayer(other))
		{
		case 512u:
		case 2097152u:
		{
			TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(Boss13.Inst.myPpt.myEntity);
			info.damage = damage;
			info.knockbackForce = Rigid.linearVelocity.normalized * knockbackForce;
			UnitDotsSyncSystem.AddTakeDamageRequest(other, info);
			ObjPoolMgr.Inst.RecycleGO(base.gameObject);
			break;
		}
		case 256u:
			ObjPoolMgr.Inst.RecycleGO(base.gameObject);
			break;
		}
	}

	public void OnTriggerStay_Dots(Entity other)
	{
	}

	public void OnTriggerExit_Dots(Entity other)
	{
	}
}
