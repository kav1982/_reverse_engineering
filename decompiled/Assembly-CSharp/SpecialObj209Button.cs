using Unity.Entities;
using Unity.Physics;
using UnityEngine;

public class SpecialObj209Button : LayerCorrect, IDotsTriggerReceiver, IDotsPhysicsReciever
{
	public enum ButtonColor
	{
		Blue,
		Red,
		Green
	}

	[Space(50f)]
	public MeshRenderer mr;

	public UnityEngine.BoxCollider thisCollider;

	public Color Blue;

	public Color Red;

	public Color Green;

	private SpecialObj209 so209;

	private bool isInvalid;

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

	public void Initialize(SpecialObj209 so209, ButtonColor bc)
	{
		this.so209 = so209;
		if (bc == ButtonColor.Blue)
		{
			mr.material.color = Blue;
		}
		if (bc == ButtonColor.Red)
		{
			mr.material.color = Red;
		}
		if (bc == ButtonColor.Green)
		{
			mr.material.color = Green;
		}
	}

	public void SetInvalid()
	{
		isInvalid = true;
	}

	void IDotsTriggerReceiver.OnTriggerEnter_Dots(Entity other)
	{
		if (!isInvalid && other == PlayerMgr.Inst.PlayerEtt)
		{
			if (mr.material.color == Blue)
			{
				so209.BlueChange();
			}
			if (mr.material.color == Red)
			{
				so209.RedChange();
			}
			if (mr.material.color == Green)
			{
				so209.GreenChange();
			}
			SEMgr.Inst.puzzleClick.PlaySE();
		}
	}

	void IDotsTriggerReceiver.OnTriggerStay_Dots(Entity other)
	{
	}

	void IDotsTriggerReceiver.OnTriggerExit_Dots(Entity other)
	{
	}
}
