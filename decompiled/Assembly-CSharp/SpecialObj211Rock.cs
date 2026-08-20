using Unity.Entities;
using Unity.Physics;
using UnityEngine;

public class SpecialObj211Rock : LayerCorrect, IDotsPhysicsHolder, IDotsPhysicsReciever
{
	[Space(50f)]
	public SpriteRenderer sr_Word;

	public UnityEngine.BoxCollider thisCollider;

	public SO211ColorType ColorType { get; private set; }

	Entity IDotsPhysicsReciever.thisEntity { get; set; }

	public void Initialize(SpecialObj211 so211)
	{
		ColorType = (SO211ColorType)Random.Range(0, 3);
		switch (ColorType)
		{
		case SO211ColorType.Color1:
			sr_Word.color = so211.color1;
			break;
		case SO211ColorType.Color2:
			sr_Word.color = so211.color2;
			break;
		case SO211ColorType.Color3:
			sr_Word.color = so211.color3;
			break;
		default:
			Debug.LogError(ColorType);
			break;
		}
		CollisionFilter collisionFilter = default(CollisionFilter);
		collisionFilter.BelongsTo = 256u;
		collisionFilter.CollidesWith = DTool.GetCollidesWith(256u);
		collisionFilter.GroupIndex = 0;
		CollisionFilter filter = collisionFilter;
		UnitPhysicsSyncSystem.RegisterReciever(this, filter, thisCollider);
	}

	public void OnDestroy()
	{
		UnitPhysicsSyncSystem.UnregisterReciever(this);
	}
}
