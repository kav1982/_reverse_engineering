using Unity.Entities;
using Unity.Physics;
using UnityEngine;

public class SpecialObj202Button : LayerCorrect, IDotsTriggerReceiver, IDotsPhysicsReciever
{
	[Space(50f)]
	public Animator anima;

	public UnityEngine.BoxCollider thisCollider;

	private SpecialObj202 so104;

	private bool isCorrect;

	public int Index { get; private set; }

	public bool IsOn { get; private set; }

	Entity IDotsPhysicsReciever.thisEntity { get; set; }

	public void Initialize(SpecialObj202 so104, int index)
	{
		this.so104 = so104;
		Index = index;
		IsOn = ((Random.Range(0, 2) == 0) ? true : false);
		if (IsOn)
		{
			anima.SetTrigger("On");
		}
		else
		{
			anima.SetTrigger("Off");
		}
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

	public void Change()
	{
		if (IsOn)
		{
			IsOn = false;
			anima.SetTrigger("Off");
		}
		else
		{
			IsOn = true;
			anima.SetTrigger("On");
		}
	}

	public void Correct()
	{
		isCorrect = true;
	}

	void IDotsTriggerReceiver.OnTriggerEnter_Dots(Entity other)
	{
		if (!isCorrect && other == PlayerMgr.Inst.PlayerEtt)
		{
			so104.ButtonEntry(this);
		}
	}

	void IDotsTriggerReceiver.OnTriggerStay_Dots(Entity other)
	{
	}

	void IDotsTriggerReceiver.OnTriggerExit_Dots(Entity other)
	{
	}
}
