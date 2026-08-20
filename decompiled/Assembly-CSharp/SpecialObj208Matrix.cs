using Unity.Entities;
using Unity.Physics;
using UnityEngine;

public class SpecialObj208Matrix : MonoBehaviour, ITrap, IDotsTriggerReceiver, IDotsPhysicsReciever
{
	public GameObject pfb_SO208Hit;

	public Transform tsf_EF;

	public Transform tsf_GroundEF;

	public Animator anima;

	private SpecialObj208Hit so208Hit;

	private bool isRight;

	public UnityEngine.BoxCollider thisCollider;

	Entity IDotsPhysicsReciever.thisEntity { get; set; }

	private void Start()
	{
		tsf_EF.position = Tool2D.GetLayerPoint(base.transform);
		tsf_GroundEF.position = Tool2D.GetLayerPoint(base.transform, LayerCorrectType.GroundEffectLow);
		if (!isRight)
		{
			so208Hit = Object.Instantiate(pfb_SO208Hit, base.transform.position, Quaternion.identity, base.transform.parent).GetComponent<SpecialObj208Hit>();
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

	public void SetRight()
	{
		isRight = true;
	}

	public void SetTrapInvalid()
	{
		isRight = true;
		if (so208Hit != null)
		{
			Object.Destroy(so208Hit.gameObject);
		}
		anima.SetTrigger("Grey");
	}

	void IDotsTriggerReceiver.OnTriggerEnter_Dots(Entity other)
	{
		if (!isRight && other == PlayerMgr.Inst.PlayerEtt)
		{
			anima.SetTrigger("Red");
		}
	}

	void IDotsTriggerReceiver.OnTriggerStay_Dots(Entity other)
	{
	}

	void IDotsTriggerReceiver.OnTriggerExit_Dots(Entity other)
	{
		if (!isRight && other == PlayerMgr.Inst.PlayerEtt)
		{
			anima.SetTrigger("Idle");
		}
	}
}
