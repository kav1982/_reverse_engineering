using Unity.Entities;
using Unity.Physics;
using UnityEngine;

public class SpecialObj220Block : LayerCorrect, IDotsTriggerReceiver, IDotsPhysicsReciever
{
	public UnityEngine.BoxCollider thisCollider;

	public SpecialObj220 SpecialObj220;

	public SpriteRenderer spriterenderer;

	public Vector2 BlockPosition;

	public Vector2 BlockTargtPosition;

	public int id;

	Entity IDotsPhysicsReciever.thisEntity { get; set; }

	private void Start()
	{
		CollisionFilter collisionFilter = default(CollisionFilter);
		collisionFilter.BelongsTo = 67108864u;
		collisionFilter.CollidesWith = 512u;
		collisionFilter.GroupIndex = 0;
		CollisionFilter filter = collisionFilter;
		UnitPhysicsSyncSystem.RegisterReciever(this, filter, thisCollider);
	}

	private void OnDestroy()
	{
		UnitPhysicsSyncSystem.UnregisterReciever(this);
	}

	private void Update()
	{
	}

	void IDotsTriggerReceiver.OnTriggerEnter_Dots(Entity other)
	{
		if (id != 6 && !SpecialObj220.IsComplete && other == PlayerMgr.Inst.PlayerEtt && !SpecialObj220.moving)
		{
			SpecialObj220.MoveBolock(this);
		}
	}

	void IDotsTriggerReceiver.OnTriggerStay_Dots(Entity other)
	{
	}

	void IDotsTriggerReceiver.OnTriggerExit_Dots(Entity other)
	{
		if (!(SpecialObj220 == null) && !SpecialObj220.IsComplete && other == PlayerMgr.Inst.PlayerEtt && id == SpecialObj220.hight * SpecialObj220.width)
		{
			SpecialObj220.CurrentEmptyPosition = BlockPosition;
		}
	}
}
