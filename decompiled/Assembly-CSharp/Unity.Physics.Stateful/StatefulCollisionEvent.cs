using System;
using Unity.Entities;
using Unity.Mathematics;

namespace Unity.Physics.Stateful;

public struct StatefulCollisionEvent : IBufferElementData, IStatefulSimulationEvent<StatefulCollisionEvent>, ISimulationEvent<StatefulCollisionEvent>, IComparable<StatefulCollisionEvent>
{
	public struct Details
	{
		internal bool IsValid;

		public int NumberOfContactPoints;

		public float EstimatedImpulse;

		public float3 AverageContactPointPosition;

		public float3 FirstContactPosition;

		public Details(int numContactPoints, float estimatedImpulse, float3 averageContactPosition, float3 firstContactPosition)
		{
			IsValid = 0 < numContactPoints;
			NumberOfContactPoints = numContactPoints;
			EstimatedImpulse = estimatedImpulse;
			AverageContactPointPosition = averageContactPosition;
			FirstContactPosition = firstContactPosition;
		}
	}

	public float3 Normal;

	internal Details CollisionDetails;

	public Entity EntityA { get; set; }

	public Entity EntityB { get; set; }

	public int BodyIndexA { get; set; }

	public int BodyIndexB { get; set; }

	public ColliderKey ColliderKeyA { get; set; }

	public ColliderKey ColliderKeyB { get; set; }

	public StatefulEventState State { get; set; }

	public StatefulCollisionEvent(CollisionEvent collisionEvent)
	{
		EntityA = collisionEvent.EntityA;
		EntityB = collisionEvent.EntityB;
		BodyIndexA = collisionEvent.BodyIndexA;
		BodyIndexB = collisionEvent.BodyIndexB;
		ColliderKeyA = collisionEvent.ColliderKeyA;
		ColliderKeyB = collisionEvent.ColliderKeyB;
		State = StatefulEventState.Undefined;
		Normal = collisionEvent.Normal;
		CollisionDetails = default(Details);
	}

	public Entity GetOtherEntity(Entity entity)
	{
		if (!(entity == EntityA))
		{
			return EntityA;
		}
		return EntityB;
	}

	public float3 GetNormalFrom(Entity entity)
	{
		return math.select(-Normal, Normal, entity == EntityB);
	}

	public bool TryGetDetails(out Details details)
	{
		details = CollisionDetails;
		return CollisionDetails.IsValid;
	}

	public int CompareTo(StatefulCollisionEvent other)
	{
		return ISimulationEventUtilities.CompareEvents(this, other);
	}
}
