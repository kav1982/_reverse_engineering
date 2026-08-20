using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Internal;
using Unity.Physics;
using Unity.Physics.Extensions;
using Unity.Physics.Stateful;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Scripting;

[UpdateBefore(typeof(UnitDotsSyncSystem))]
[CompilerGenerated]
public class UnitPhysicsSyncSystem : SystemBase
{
	public struct ReceiverInfo
	{
		public IDotsPhysicsReciever reciever;

		public UnityEngine.Collider collider;

		public Entity entity;

		public CollisionFilter originFilter;

		public bool lastFrameEnabled;
	}

	private struct TypeHandle
	{
		[ReadOnly]
		public EntityStorageInfoLookup __EntityStorageInfoLookup;

		[ReadOnly]
		public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RO_ComponentLookup;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__EntityStorageInfoLookup = state.GetEntityStorageInfoLookup();
			__Unity_Transforms_LocalTransform_RO_ComponentLookup = state.GetComponentLookup<LocalTransform>(isReadOnly: true);
		}
	}

	public static List<CollisionFilter> createFilter = new List<CollisionFilter>();

	public static List<IDotsPhysicsReciever> createRequest = new List<IDotsPhysicsReciever>();

	public static List<UnityEngine.Collider> createCollider = new List<UnityEngine.Collider>();

	public static List<bool> createIgnoreSpell = new List<bool>();

	public static List<ReceiverInfo> receiverInfos = new List<ReceiverInfo>();

	public static HashSet<IDotsPhysicsReciever> destroyRequest = new HashSet<IDotsPhysicsReciever>();

	public static Entity SrcCapsule;

	public static Entity SrcBox;

	public static EntityManager entityMgr;

	public static bool initialized;

	public static PhysicsWorldSingleton pws;

	public static bool lookUpRefreshed = false;

	public static UnitPhysicsSyncSystem Inst;

	private TypeHandle __TypeHandle;

	private EntityQuery __query_631585033_0;

	public static void UnregisterReciever(IDotsPhysicsReciever reciever)
	{
		destroyRequest.Add(reciever);
	}

	public void Reset()
	{
		createFilter.Clear();
		createRequest.Clear();
		createCollider.Clear();
		createIgnoreSpell.Clear();
	}

	public static void RegisterReciever(IDotsPhysicsReciever reciever, CollisionFilter filter, UnityEngine.Collider collider, bool ignoredBySpell = false)
	{
		createRequest.Add(reciever);
		createFilter.Add(filter);
		createCollider.Add(collider);
		createIgnoreSpell.Add(ignoredBySpell);
	}

	public unsafe static void CreateReciever(IDotsPhysicsReciever receiver, CollisionFilter filter, UnityEngine.Collider collider, bool ignoredByspell)
	{
		bool num = collider is UnityEngine.BoxCollider;
		Entity entity = ((!num) ? entityMgr.Instantiate(SrcCapsule) : entityMgr.Instantiate(SrcBox));
		if (ignoredByspell || (filter.BelongsTo & 0x40000000u) != 0)
		{
			entityMgr.AddComponent<IgnoreSpellHitTag>(entity);
		}
		if (receiver is IDotsPhysicsHolder)
		{
			entityMgr.AddComponentData(componentData: new PhysicsMassOverride
			{
				IsKinematic = 1,
				SetVelocityToZero = 1
			}, entity: entity);
		}
		else if (collider.isTrigger)
		{
			entityMgr.AddComponentObject(componentData: new UnitTriggerReference
			{
				reference = (receiver as IDotsTriggerReceiver)
			}, entity: entity);
			entityMgr.AddBuffer<StatefulTriggerEvent>(entity);
		}
		else
		{
			entityMgr.AddComponentObject(componentData: new UnitCollisionReference
			{
				reference = (receiver as IDotsCollisionReceiver)
			}, entity: entity);
			entityMgr.AddBuffer<StatefulCollisionEvent>(entity);
			entityMgr.AddComponentData(componentData: new StatefulCollisionEventDetails
			{
				CalculateDetails = true
			}, entity: entity);
		}
		PhysicsCollider collider2 = entityMgr.GetComponentData<PhysicsCollider>(entity);
		collider2.MakeUnique(in entity, entityMgr);
		ReceiverInfo item = new ReceiverInfo
		{
			reciever = receiver,
			collider = collider,
			entity = entity
		};
		if (!collider.enabled)
		{
			collider2.ColliderPtr->SetCollisionResponse(CollisionResponsePolicy.None);
		}
		else if (collider.isTrigger)
		{
			collider2.ColliderPtr->SetCollisionResponse(CollisionResponsePolicy.RaiseTriggerEvents);
		}
		else
		{
			collider2.ColliderPtr->SetCollisionResponse(CollisionResponsePolicy.CollideRaiseCollisionEvents);
		}
		item.lastFrameEnabled = collider.enabled;
		if (num)
		{
			UnityEngine.BoxCollider boxCollider = collider as UnityEngine.BoxCollider;
			Unity.Physics.BoxCollider* colliderPtr = (Unity.Physics.BoxCollider*)collider2.ColliderPtr;
			BoxGeometry geometry = colliderPtr->Geometry;
			geometry.Size = boxCollider.size;
			geometry.Center = boxCollider.center;
			colliderPtr->Geometry = geometry;
			collider2.ColliderPtr->SetRestitution(collider.material.bounciness);
		}
		else
		{
			UnityEngine.CapsuleCollider capsuleCollider = collider as UnityEngine.CapsuleCollider;
			Unity.Physics.CapsuleCollider* colliderPtr2 = (Unity.Physics.CapsuleCollider*)collider2.ColliderPtr;
			CapsuleGeometry geometry2 = colliderPtr2->Geometry;
			float height = capsuleCollider.height;
			Vector3 center = capsuleCollider.center;
			geometry2.Vertex0 = center - new Vector3(0f, 0f, height / 2f);
			geometry2.Vertex1 = center + new Vector3(0f, 0f, height / 2f);
			geometry2.Radius = capsuleCollider.radius;
			colliderPtr2->Geometry = geometry2;
			collider2.ColliderPtr->SetRestitution(collider.material.bounciness);
		}
		collider2.ColliderPtr->SetCollisionFilter(filter);
		item.originFilter = filter;
		entityMgr.SetComponentData(entity, collider2);
		LocalTransform componentData5 = entityMgr.GetComponentData<LocalTransform>(entity);
		componentData5.Position = Tool2D.IgnoreZPoint(collider.transform.position);
		entityMgr.SetComponentData(entity, componentData5);
		receiverInfos.Add(item);
		receiver.thisEntity = entity;
	}

	[Preserve]
	protected override void OnCreate()
	{
		entityMgr = World.DefaultGameObjectInjectionWorld.EntityManager;
		Reset();
		initialized = true;
		RequireForUpdate<AllUnitEtt>();
		Inst = this;
		EventMgr.DotsSystemReset = (Action)Delegate.Combine(EventMgr.DotsSystemReset, (Action)delegate
		{
			createFilter.Clear();
			createRequest.Clear();
			createCollider.Clear();
			createIgnoreSpell.Clear();
		});
	}

	[Preserve]
	protected override void OnDestroy()
	{
		EventMgr.DotsSystemReset = (Action)Delegate.Remove(EventMgr.DotsSystemReset, (Action)delegate
		{
			createFilter.Clear();
			createRequest.Clear();
			createCollider.Clear();
			createIgnoreSpell.Clear();
		});
	}

	[Preserve]
	protected unsafe override void OnUpdate()
	{
		AllUnitEtt singleton = __query_631585033_0.GetSingleton<AllUnitEtt>();
		SrcBox = singleton.map[999996];
		SrcCapsule = singleton.map[999997];
		for (int i = 0; i < createRequest.Count; i++)
		{
			if (createRequest[i] as MonoBehaviour != null)
			{
				CreateReciever(createRequest[i], createFilter[i], createCollider[i], createIgnoreSpell[i]);
			}
		}
		createRequest.Clear();
		createFilter.Clear();
		createCollider.Clear();
		createIgnoreSpell.Clear();
		if (destroyRequest.Count > 0)
		{
			EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer(Allocator.TempJob);
			for (int num = receiverInfos.Count - 1; num >= 0; num--)
			{
				if (!InternalCompilerInterface.DoesEntityExist(ref __TypeHandle.__EntityStorageInfoLookup, ref base.CheckedStateRef, receiverInfos[num].entity) || !InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref base.CheckedStateRef, receiverInfos[num].entity))
				{
					receiverInfos.RemoveAt(num);
				}
				else if (receiverInfos[num].reciever as MonoBehaviour == null || destroyRequest.Contains(receiverInfos[num].reciever))
				{
					entityCommandBuffer.DestroyEntity(receiverInfos[num].entity);
					receiverInfos.RemoveAt(num);
				}
			}
			entityCommandBuffer.Playback(entityMgr);
			entityCommandBuffer.Dispose();
			destroyRequest.Clear();
		}
		for (int j = 0; j < receiverInfos.Count; j++)
		{
			UnityEngine.Collider collider = receiverInfos[j].collider;
			LocalTransform componentData = base.EntityManager.GetComponentData<LocalTransform>(receiverInfos[j].entity);
			componentData.Position = collider.transform.position;
			componentData.Rotation = Quaternion.Euler(collider.transform.eulerAngles);
			base.EntityManager.SetComponentData(receiverInfos[j].entity, componentData);
			if (collider.enabled == receiverInfos[j].lastFrameEnabled)
			{
				continue;
			}
			ReceiverInfo value = receiverInfos[j];
			value.lastFrameEnabled = collider.enabled;
			receiverInfos[j] = value;
			PhysicsCollider componentData2 = base.EntityManager.GetComponentData<PhysicsCollider>(receiverInfos[j].entity);
			if (collider.enabled)
			{
				if (collider.isTrigger)
				{
					componentData2.ColliderPtr->SetCollisionResponse(CollisionResponsePolicy.RaiseTriggerEvents);
				}
				else
				{
					componentData2.ColliderPtr->SetCollisionResponse(CollisionResponsePolicy.CollideRaiseCollisionEvents);
				}
				componentData2.ColliderPtr->SetCollisionFilter(receiverInfos[j].originFilter);
				base.EntityManager.SetComponentData(receiverInfos[j].entity, componentData2);
			}
			else if (!collider.enabled)
			{
				componentData2.ColliderPtr->SetCollisionResponse(CollisionResponsePolicy.None);
				componentData2.ColliderPtr->SetCollisionFilter(GameConst.Filter_None);
				base.EntityManager.SetComponentData(receiverInfos[j].entity, componentData2);
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<AllUnitEtt>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_631585033_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder.Dispose();
	}

	protected override void OnCreateForCompiler()
	{
		base.OnCreateForCompiler();
		__AssignQueries(ref base.CheckedStateRef);
		__TypeHandle.__AssignHandles(ref base.CheckedStateRef);
	}

	[Preserve]
	public UnitPhysicsSyncSystem()
	{
	}
}
