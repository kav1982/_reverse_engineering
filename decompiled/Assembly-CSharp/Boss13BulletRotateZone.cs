using System.Collections.Generic;
using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

public class Boss13BulletRotateZone : MonoBehaviour, IDotsTriggerReceiver, IDotsPhysicsReciever
{
	public float moveSpeed;

	public UnityEngine.CapsuleCollider CC;

	public float catchRadius;

	public float rotateAngle;

	public float rotateSpeed;

	public float bulletMoveSpeed;

	public List<Boss13ReboundBullet> bullets = new List<Boss13ReboundBullet>();

	public Entity thisEntity { get; set; }

	private void OnEnable()
	{
		CollisionFilter collisionFilter = default(CollisionFilter);
		collisionFilter.BelongsTo = 2048u;
		collisionFilter.CollidesWith = 16777216u;
		collisionFilter.GroupIndex = 0;
		CollisionFilter filter = collisionFilter;
		UnitPhysicsSyncSystem.RegisterReciever(this, filter, CC);
	}

	private void OnDisable()
	{
		UnitPhysicsSyncSystem.UnregisterReciever(this);
	}

	private void Update()
	{
		rotateAngle += rotateSpeed * Time.deltaTime;
		if (rotateAngle > 360f)
		{
			rotateAngle = 0f;
		}
		for (int i = 0; i < bullets.Count; i++)
		{
			Vector3 target = base.transform.position + Tool2D.GetDir(Vector3.up, rotateAngle + (float)(i * 360 / bullets.Count)) * catchRadius;
			bullets[i].transform.position = Vector3.MoveTowards(bullets[i].transform.position, target, bulletMoveSpeed * Time.deltaTime);
		}
		LocalTransform componentData = UnitDotsSyncSystem.GetComponentData<LocalTransform>(thisEntity);
		componentData.Position = base.transform.position;
		UnitDotsSyncSystem.SetComponentData(componentData, thisEntity);
	}

	public void OnTriggerEnter_Dots(Entity other)
	{
		UnitDotsSyncSystem.GetLayer(other);
		_ = 16777216;
	}

	public void OnTriggerStay_Dots(Entity other)
	{
	}

	public void OnTriggerExit_Dots(Entity other)
	{
	}
}
