using Unity.Entities;
using Unity.Physics;
using UnityEngine;

public class SpecialObj224 : MonoBehaviour, IDotsPhysicsHolder, IDotsPhysicsReciever
{
	public UnityEngine.BoxCollider boxCollider;

	public GameObject sprite;

	public Entity thisEntity { get; set; }

	private void OnEnable()
	{
		CollisionFilter collisionFilter = default(CollisionFilter);
		collisionFilter.BelongsTo = 256u;
		collisionFilter.CollidesWith = DTool.GetCollidesWith(256u);
		collisionFilter.GroupIndex = 0;
		CollisionFilter filter = collisionFilter;
		UnitPhysicsSyncSystem.RegisterReciever(this, filter, boxCollider);
	}

	private void OnDisable()
	{
		UnitPhysicsSyncSystem.UnregisterReciever(this);
	}

	private void Start()
	{
		sprite.SetActive(value: false);
	}
}
