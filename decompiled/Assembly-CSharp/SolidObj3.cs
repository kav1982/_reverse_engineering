using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

public class SolidObj3 : UnitBase, ITrap, IDotsTriggerReceiver, IDotsPhysicsReciever
{
	[Space(50f)]
	public float knockbackForce;

	public float knockbackForcePlayer;

	public float attackInterval;

	public int damageForPlayer;

	[Header("Rotate")]
	public Transform[] tsf_Rotates;

	public float rotateSpeed;

	private List<Entity> attackedCollider = new List<Entity>();

	private List<float> attackedIntervals = new List<float>();

	public Entity thisEntity { get; set; }

	public unsafe override void EveryInitialCallback()
	{
		attackedCollider.Clear();
		attackedIntervals.Clear();
		CollisionFilter collisionFilter = default(CollisionFilter);
		collisionFilter.BelongsTo = 131072u;
		collisionFilter.CollidesWith = DTool.GetCollidesWith(131072u) | 0x40000u;
		collisionFilter.GroupIndex = 0;
		CollisionFilter collisionFilter2 = collisionFilter;
		PhysicsCollider componentData = GetComponentData<PhysicsCollider>();
		componentData.ColliderPtr->SetCollisionFilter(collisionFilter2);
		SetComponentData(componentData);
	}

	public override void Update()
	{
		base.Update();
		for (int i = 0; i < tsf_Rotates.Length; i++)
		{
			tsf_Rotates[i].Rotate(0f, 0f, rotateSpeed * Time.deltaTime);
		}
		for (int num = attackedCollider.Count - 1; num >= 0; num--)
		{
			attackedIntervals[num] -= Time.deltaTime;
			if (attackedIntervals[num] <= 0f)
			{
				attackedIntervals.RemoveAt(num);
				attackedCollider.RemoveAt(num);
			}
		}
	}

	public void SetTrapInvalid()
	{
		DotsAnnouncedDeath();
	}

	void IDotsTriggerReceiver.OnTriggerStay_Dots(Entity other)
	{
		if (attackedCollider.Contains(other) || !UnitDotsSyncSystem.EntityIsValid(other))
		{
			return;
		}
		uint layer = UnitDotsSyncSystem.GetLayer(other);
		Vector3 vector = UnitDotsSyncSystem.GetComponentData<LocalTransform>(other).Position;
		switch (layer)
		{
		case 262144u:
		{
			attackedCollider.Add(other);
			attackedIntervals.Add(attackInterval);
			PhysicsVelocity componentData = GetComponentData<PhysicsVelocity>(other);
			componentData.Linear += (float3)ToPointDir(vector) * knockbackForce;
			SetComponentData(componentData, other);
			break;
		}
		case 512u:
		case 2048u:
		case 4096u:
		case 8192u:
		case 32768u:
		case 131072u:
		case 2097152u:
		{
			TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(myPpt.myEntity);
			info.damage = damageForPlayer;
			info.knockbackForce = (vector - base.transform.position).normalized * knockbackForcePlayer;
			info.teammateTakeDamageRatio = 3f;
			info.isTrapDamage = true;
			if (GetComponentData<UnitProperty_Dots>(other).unitCfg.IsSameCamp(UnitType.Monster))
			{
				info.damage *= 15f;
				info.knockbackForce = (vector - base.transform.position).normalized * knockbackForce;
			}
			attackedCollider.Add(other);
			attackedIntervals.Add(attackInterval);
			UnitDotsSyncSystem.AddTakeDamageRequest(other, info);
			if (!GameMgr.IsHarmony_Static)
			{
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_DropBlood", vector, 1f);
			}
			break;
		}
		case 8388608u:
		case 16777216u:
		{
			UnitDotsSyncSystem.ProcessHitSpell(other, 999f, out var _);
			break;
		}
		}
	}

	void IDotsTriggerReceiver.OnTriggerEnter_Dots(Entity other)
	{
	}

	void IDotsTriggerReceiver.OnTriggerExit_Dots(Entity other)
	{
	}
}
