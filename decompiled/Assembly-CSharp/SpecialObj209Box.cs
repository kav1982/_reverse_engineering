using Unity.Entities;
using Unity.Physics;
using UnityEngine;
using UnityEngine.AI;

public class SpecialObj209Box : LayerCorrect, IDotsPhysicsHolder, IDotsPhysicsReciever
{
	[Space(50f)]
	public Animator anima;

	public UnityEngine.BoxCollider boxCollider;

	public NavMeshObstacle nmo;

	public bool isdown;

	Entity IDotsPhysicsReciever.thisEntity { get; set; }

	private void Start()
	{
		CollisionFilter collisionFilter = default(CollisionFilter);
		collisionFilter.BelongsTo = 256u;
		collisionFilter.CollidesWith = DTool.GetCollidesWith(256u);
		collisionFilter.GroupIndex = -1;
		CollisionFilter filter = collisionFilter;
		UnitPhysicsSyncSystem.RegisterReciever(this, filter, boxCollider);
	}

	private void OnDestroy()
	{
		UnitPhysicsSyncSystem.UnregisterReciever(this);
	}

	private void _OpenFinish()
	{
		boxCollider.enabled = false;
		nmo.enabled = false;
		correctType = LayerCorrectType.GroundEffectLow;
		CorrectLayerOnce();
	}

	private void _CloseFinish()
	{
		boxCollider.enabled = true;
		nmo.enabled = true;
		correctType = LayerCorrectType.Coordinate;
		CorrectLayerOnce();
	}

	public void Down()
	{
		anima.SetTrigger("Down");
		isdown = true;
	}

	public void Up()
	{
		anima.SetTrigger("Up");
		isdown = false;
		_CloseFinish();
	}
}
