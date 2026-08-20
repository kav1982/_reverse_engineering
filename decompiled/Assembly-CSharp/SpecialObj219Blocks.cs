using Unity.Entities;
using Unity.Physics;
using UnityEngine;

public class SpecialObj219Blocks : LayerCorrect, IDotsTriggerReceiver, IDotsPhysicsReciever
{
	public SpecialObj219 SpecialObj219;

	public Vector2Int Position;

	public bool isNumberedBlock;

	public int number;

	public bool interacted;

	public SpriteRenderer spriterenderer;

	public SpriteRenderer colorBlindnessFriendly;

	public SpriteRenderer spriterendererFrame;

	public GameObject spriterendererFrameSmall;

	public UnityEngine.BoxCollider thisCollider;

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

	void IDotsTriggerReceiver.OnTriggerEnter_Dots(Entity other)
	{
		if (!SpecialObj219.IsComplete && !interacted && other == PlayerMgr.Inst.PlayerEtt)
		{
			if (isNumberedBlock)
			{
				SpecialObj219.Add(Position.x, Position.y, this, number);
			}
			else
			{
				SpecialObj219.Add(Position.x, Position.y, this);
			}
		}
	}

	void IDotsTriggerReceiver.OnTriggerExit_Dots(Entity other)
	{
	}

	void IDotsTriggerReceiver.OnTriggerStay_Dots(Entity other)
	{
	}
}
