using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

namespace Unity.Physics.Stateful;

public static class StatefulEventCollectionJobs
{
	[BurstCompile]
	public struct CollectTriggerEvents : ITriggerEventsJob, ITriggerEventsJobBase
	{
		public NativeList<StatefulTriggerEvent> TriggerEvents;

		public void Execute(TriggerEvent triggerEvent)
		{
			ref NativeList<StatefulTriggerEvent> triggerEvents = ref TriggerEvents;
			StatefulTriggerEvent value = new StatefulTriggerEvent(triggerEvent);
			triggerEvents.Add(in value);
		}
	}

	[BurstCompile]
	public struct CollectCollisionEvents : ICollisionEventsJob, ICollisionEventsJobBase
	{
		public NativeList<StatefulCollisionEvent> CollisionEvents;

		public void Execute(CollisionEvent collisionEvent)
		{
			ref NativeList<StatefulCollisionEvent> collisionEvents = ref CollisionEvents;
			StatefulCollisionEvent value = new StatefulCollisionEvent(collisionEvent);
			collisionEvents.Add(in value);
		}
	}

	[BurstCompile]
	public struct CollectCollisionEventsWithDetails : ICollisionEventsJob, ICollisionEventsJobBase
	{
		public NativeList<StatefulCollisionEvent> CollisionEvents;

		[ReadOnly]
		public PhysicsWorld PhysicsWorld;

		[ReadOnly]
		public ComponentLookup<StatefulCollisionEventDetails> EventDetails;

		public bool ForceCalculateDetails;

		public void Execute(CollisionEvent collisionEvent)
		{
			StatefulCollisionEvent value = new StatefulCollisionEvent(collisionEvent);
			bool flag = ForceCalculateDetails;
			if (!flag && EventDetails.HasComponent(collisionEvent.EntityA))
			{
				flag = EventDetails[collisionEvent.EntityA].CalculateDetails;
			}
			if (!flag && EventDetails.HasComponent(collisionEvent.EntityB))
			{
				flag = EventDetails[collisionEvent.EntityB].CalculateDetails;
			}
			if (flag)
			{
				CollisionEvent.Details details = collisionEvent.CalculateDetails(ref PhysicsWorld);
				value.CollisionDetails = new StatefulCollisionEvent.Details(details.EstimatedContactPointPositions.Length, details.EstimatedImpulse, details.AverageContactPointPosition, details.EstimatedContactPointPositions[0]);
			}
			CollisionEvents.Add(in value);
		}
	}

	[BurstCompile]
	public struct ConvertEventStreamToDynamicBufferJob<T, C> : IJob where T : unmanaged, IBufferElementData, IStatefulSimulationEvent<T> where C : unmanaged, IComponentData
	{
		public NativeList<T> PreviousEvents;

		public NativeList<T> CurrentEvents;

		public BufferLookup<T> EventLookup;

		public bool UseExcludeComponent;

		[ReadOnly]
		public ComponentLookup<C> EventExcludeLookup;

		public void Execute()
		{
			NativeList<T> statefulEvents = new NativeList<T>(CurrentEvents.Length, Allocator.Temp);
			StatefulSimulationEventBuffers<T>.GetStatefulEvents(PreviousEvents, CurrentEvents, statefulEvents);
			for (int i = 0; i < statefulEvents.Length; i++)
			{
				T elem = statefulEvents[i];
				bool num = EventLookup.HasBuffer(elem.EntityA) && (!UseExcludeComponent || !EventExcludeLookup.HasComponent(elem.EntityA));
				bool flag = EventLookup.HasBuffer(elem.EntityB) && (!UseExcludeComponent || !EventExcludeLookup.HasComponent(elem.EntityA));
				if (num)
				{
					EventLookup[elem.EntityA].Add(elem);
				}
				if (flag)
				{
					EventLookup[elem.EntityB].Add(elem);
				}
			}
		}
	}
}
