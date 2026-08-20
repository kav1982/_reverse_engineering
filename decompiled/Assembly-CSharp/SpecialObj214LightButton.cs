using Unity.Entities;
using Unity.Physics;
using UnityEngine;

public class SpecialObj214LightButton : LayerCorrect, ITrap, IDotsTriggerReceiver, IDotsPhysicsReciever
{
	public UnityEngine.BoxCollider thisCollider;

	private SpecialObj214MainButton mainButton;

	private bool isValid = true;

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

	public void SetMainButton(SpecialObj214MainButton mainButton)
	{
		this.mainButton = mainButton;
	}

	public void SetTrapInvalid()
	{
		isValid = false;
	}

	void IDotsTriggerReceiver.OnTriggerEnter_Dots(Entity other)
	{
	}

	void IDotsTriggerReceiver.OnTriggerStay_Dots(Entity other)
	{
		if (isValid && other == PlayerMgr.Inst.PlayerEtt)
		{
			mainButton.PlayerEnter();
		}
	}

	void IDotsTriggerReceiver.OnTriggerExit_Dots(Entity other)
	{
	}
}
