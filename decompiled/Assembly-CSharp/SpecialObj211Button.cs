using Unity.Entities;
using Unity.Physics;
using UnityEngine;

public class SpecialObj211Button : LayerCorrect, IDotsTriggerReceiver, IDotsPhysicsReciever
{
	[Space(50f)]
	public SpriteRenderer sr;

	public UnityEngine.BoxCollider thisCollider;

	private SpecialObj211 so211;

	private bool isInvalid;

	public SO211ColorType CurrentColorType { get; private set; }

	Entity IDotsPhysicsReciever.thisEntity { get; set; }

	private void UpdateColor()
	{
		switch (CurrentColorType)
		{
		case SO211ColorType.Color1:
			sr.color = so211.color1;
			break;
		case SO211ColorType.Color2:
			sr.color = so211.color2;
			break;
		case SO211ColorType.Color3:
			sr.color = so211.color3;
			break;
		default:
			Debug.LogError(CurrentColorType);
			break;
		}
		sr.material.SetColor("_Color", sr.color);
	}

	public void Initialize(SpecialObj211 so211, SO211ColorType correctColorType)
	{
		this.so211 = so211;
		switch (correctColorType)
		{
		case SO211ColorType.Color1:
			CurrentColorType = ((Random.Range(0, 2) == 0) ? SO211ColorType.Color2 : SO211ColorType.Color3);
			break;
		case SO211ColorType.Color2:
			CurrentColorType = ((Random.Range(0, 2) == 0) ? SO211ColorType.Color3 : SO211ColorType.Color1);
			break;
		case SO211ColorType.Color3:
			CurrentColorType = ((Random.Range(0, 2) != 0) ? SO211ColorType.Color2 : SO211ColorType.Color1);
			break;
		default:
			Debug.LogError(correctColorType);
			break;
		}
		UpdateColor();
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

	public void SetInvalid()
	{
		isInvalid = true;
	}

	void IDotsTriggerReceiver.OnTriggerEnter_Dots(Entity other)
	{
		if (!isInvalid && other == PlayerMgr.Inst.PlayerEtt)
		{
			switch (CurrentColorType)
			{
			case SO211ColorType.Color1:
				CurrentColorType = SO211ColorType.Color2;
				break;
			case SO211ColorType.Color2:
				CurrentColorType = SO211ColorType.Color3;
				break;
			case SO211ColorType.Color3:
				CurrentColorType = SO211ColorType.Color1;
				break;
			default:
				Debug.LogError(CurrentColorType);
				break;
			}
			SEMgr.Inst.puzzleClick.PlaySE();
			UpdateColor();
			so211.CheckAnswer();
		}
	}

	void IDotsTriggerReceiver.OnTriggerStay_Dots(Entity other)
	{
	}

	void IDotsTriggerReceiver.OnTriggerExit_Dots(Entity other)
	{
	}
}
