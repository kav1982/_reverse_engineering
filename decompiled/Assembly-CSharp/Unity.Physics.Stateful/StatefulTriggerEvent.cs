using System;
using Unity.Entities;

namespace Unity.Physics.Stateful;

public struct StatefulTriggerEvent : IBufferElementData, IStatefulSimulationEvent<StatefulTriggerEvent>, ISimulationEvent<StatefulTriggerEvent>, IComparable<StatefulTriggerEvent>
{
	public Entity EntityA { get; set; }

	public Entity EntityB { get; set; }

	public int BodyIndexA { get; set; }

	public int BodyIndexB { get; set; }

	public ColliderKey ColliderKeyA { get; set; }

	public ColliderKey ColliderKeyB { get; set; }

	public StatefulEventState State { get; set; }

	public StatefulTriggerEvent(TriggerEvent triggerEvent)
	{
		EntityA = triggerEvent.EntityA;
		EntityB = triggerEvent.EntityB;
		BodyIndexA = triggerEvent.BodyIndexA;
		BodyIndexB = triggerEvent.BodyIndexB;
		ColliderKeyA = triggerEvent.ColliderKeyA;
		ColliderKeyB = triggerEvent.ColliderKeyB;
		State = StatefulEventState.Undefined;
	}

	public int CompareTo(StatefulTriggerEvent other)
	{
		return ISimulationEventUtilities.CompareEvents(this, other);
	}

	public int GetSelfBodyIndex(Entity self)
	{
		if (!(self == EntityA))
		{
			return BodyIndexB;
		}
		return BodyIndexA;
	}

	public Entity GetOtherEntity(Entity self)
	{
		if (!(self == EntityA))
		{
			return EntityA;
		}
		return EntityB;
	}

	public ColliderKey GetOtherColliderKey(Entity self)
	{
		if (!(self == EntityA))
		{
			return ColliderKeyA;
		}
		return ColliderKeyB;
	}
}
