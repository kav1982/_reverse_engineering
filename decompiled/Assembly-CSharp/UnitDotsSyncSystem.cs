using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Internal;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Extensions;
using Unity.Physics.GraphicsIntegration;
using Unity.Physics.Stateful;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Scripting;

[UpdateBefore(typeof(UnitTakeDamageGroup))]
[CompilerGenerated]
public class UnitDotsSyncSystem : SystemBase
{
	public struct RayCastHitResult
	{
		public Entity entity;

		public Vector3 point;

		public Vector3 normal;
	}

	public struct DistanceHitResult
	{
		public Entity entity;

		public float distance;

		public Vector3 point;
	}

	public struct TakeDamageRequest
	{
		public Entity target;

		public TakeDamageInfo_Dots info;
	}

	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_898147355_0
	{
		public struct ResolvedChunk
		{
			public ManagedComponentAccessor<UnitPptReference> item1_ManagedComponentAccessor;

			public IntPtr Entity_IntPtr;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public QueryEnumerableWithEntity<UnitPptReference> Get(int index)
			{
				return new QueryEnumerableWithEntity<UnitPptReference>(item1_ManagedComponentAccessor[index], InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(Entity_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			public EntityManager _entityManager;

			[ReadOnly]
			private ComponentTypeHandle<UnitPptReference> item1_ManagedComponentTypeHandle_RO;

			private EntityTypeHandle Entity_TypeHandle;

			public TypeHandle(ref SystemState systemState)
			{
				_entityManager = systemState.EntityManager;
				item1_ManagedComponentTypeHandle_RO = systemState.EntityManager.GetComponentTypeHandle<UnitPptReference>(isReadOnly: false);
				Entity_TypeHandle = systemState.GetEntityTypeHandle();
			}

			public void Update(ref SystemState systemState)
			{
				item1_ManagedComponentTypeHandle_RO.Update(ref systemState);
				Entity_TypeHandle.Update(ref systemState);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ResolvedChunk Resolve(ArchetypeChunk archetypeChunk)
			{
				ResolvedChunk result = default(ResolvedChunk);
				result.item1_ManagedComponentAccessor = archetypeChunk.GetManagedComponentAccessor(ref item1_ManagedComponentTypeHandle_RO, _entityManager);
				result.Entity_IntPtr = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(archetypeChunk, Entity_TypeHandle);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<QueryEnumerableWithEntity<UnitPptReference>>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public QueryEnumerableWithEntity<UnitPptReference> Current => _resolvedChunk.Get(_currentEntityIndex);

			object IEnumerator.Current
			{
				get
				{
					throw new NotImplementedException();
				}
			}

			public Enumerator(EntityQuery entityQuery, TypeHandle typeHandle, ref SystemState state)
			{
				if (!entityQuery.IsEmptyIgnoreFilter)
				{
					CompleteDependencies(ref state);
					typeHandle.Update(ref state);
				}
				_entityQueryEnumerator = new InternalEntityQueryEnumerator(entityQuery);
				_currentEntityIndex = -1;
				_endEntityIndex = -1;
				_typeHandle = typeHandle;
				_resolvedChunk = default(ResolvedChunk);
			}

			public void Dispose()
			{
				_entityQueryEnumerator.Dispose();
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public bool MoveNext()
			{
				_currentEntityIndex++;
				if (_currentEntityIndex >= _endEntityIndex)
				{
					if (_entityQueryEnumerator.MoveNextEntityRange(out var movedToNewChunk, out var chunk, out var entityStartIndex, out var entityEndIndex))
					{
						if (movedToNewChunk)
						{
							_resolvedChunk = _typeHandle.Resolve(chunk);
						}
						_currentEntityIndex = entityStartIndex;
						_endEntityIndex = entityEndIndex;
						return true;
					}
					return false;
				}
				return true;
			}

			public Enumerator GetEnumerator()
			{
				return this;
			}

			public void Reset()
			{
				throw new NotImplementedException();
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Enumerator Query(EntityQuery entityQuery, TypeHandle typeHandle, ref SystemState state)
		{
			return new Enumerator(entityQuery, typeHandle, ref state);
		}

		public static void CompleteDependencies(ref SystemState state)
		{
			state.EntityManager.CompleteDependencyBeforeRW<UnitPptReference>();
		}
	}

	private struct TypeHandle
	{
		public IFE_898147355_0.TypeHandle __IFE_898147355_0_TypeHandle;

		[ReadOnly]
		public ComponentLookup<UnitProperty_Dots> __UnitProperty_Dots_RO_ComponentLookup;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__IFE_898147355_0_TypeHandle = new IFE_898147355_0.TypeHandle(ref state);
			__UnitProperty_Dots_RO_ComponentLookup = state.GetComponentLookup<UnitProperty_Dots>(isReadOnly: true);
		}
	}

	public Entity SrcCapsule;

	public Entity SrcBox;

	public Entity SrcNoCollider;

	public EntityQuery allUnitEttQuery;

	public static EntityManager entityMgr;

	public static bool initialized;

	public static PhysicsWorldSingleton pws;

	public static EntityQuery SpellBaseQuery;

	public static UnitDotsSyncSystem Inst;

	public static bool needClear;

	public static SpellSingleton spellSingleton;

	public static List<UnitProperty> existingUnit = new List<UnitProperty>();

	public static List<TakeDamageRequest> takeDamageRequestList = new List<TakeDamageRequest>();

	public static List<SpellSpawnParams> shootSpellParamList = new List<SpellSpawnParams>();

	private TypeHandle __TypeHandle;

	private EntityQuery __query_898147355_0;

	private EntityQuery __query_898147355_1;

	private EntityQuery __query_898147355_2;

	public void Reset()
	{
		takeDamageRequestList.Clear();
		shootSpellParamList.Clear();
		SrcCapsule = Entity.Null;
		SrcBox = Entity.Null;
		EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer(Allocator.TempJob);
		foreach (QueryEnumerableWithEntity<UnitPptReference> item2 in IFE_898147355_0.Query(__query_898147355_0, __TypeHandle.__IFE_898147355_0_TypeHandle, ref base.CheckedStateRef))
		{
			item2.Deconstruct(out var _, out var entity);
			Entity entity2 = entity;
			if ((bool)LevelMgr.Inst.CurrentRoomCtrller)
			{
				if (LevelMgr.Inst.CurrentRoomCtrller.monsterEttList.Contains(entity2))
				{
					LevelMgr.Inst.CurrentRoomCtrller.monsterEttList.Remove(entity2);
				}
				if (LevelMgr.Inst.CurrentRoomCtrller.targetableEttList.Contains(entity2))
				{
					LevelMgr.Inst.CurrentRoomCtrller.targetableEttList.Remove(entity2);
				}
			}
			entityCommandBuffer.DestroyEntity(entity2);
		}
		entityCommandBuffer.Playback(entityMgr);
		entityCommandBuffer.Dispose();
	}

	public void CheckSrcEntityLoaded()
	{
		if (SrcCapsule == Entity.Null)
		{
			AllUnitEtt singleton = allUnitEttQuery.GetSingleton<AllUnitEtt>();
			SrcBox = singleton.map[999998];
			SrcCapsule = singleton.map[999999];
			SrcNoCollider = singleton.map[999995];
		}
	}

	public unsafe static void CreateEntity(UnitProperty ppt)
	{
		Inst.CheckSrcEntityLoaded();
		UnityEngine.Collider collider = ppt.GetComponent<UnityEngine.Collider>();
		UnityEngine.BoxCollider boxCollider = null;
		bool num = collider != null;
		Entity entity;
		if (!num)
		{
			entity = entityMgr.Instantiate(Inst.SrcNoCollider);
		}
		else
		{
			boxCollider = ppt.GetComponent<UnityEngine.BoxCollider>();
			if (boxCollider == null)
			{
				entity = entityMgr.Instantiate(Inst.SrcCapsule);
			}
			else
			{
				collider = boxCollider;
				entity = entityMgr.Instantiate(Inst.SrcBox);
			}
		}
		entityMgr.AddComponentObject(componentData: new UnitPptReference
		{
			unitPpt = ppt
		}, entity: entity);
		if (num)
		{
			PhysicsCollider collider2 = entityMgr.GetComponentData<PhysicsCollider>(entity);
			collider2.MakeUnique(in entity, entityMgr);
			if (!collider.enabled)
			{
				collider2.ColliderPtr->SetCollisionResponse(CollisionResponsePolicy.None);
			}
			else if (collider.isTrigger || ppt.UnitBas is IDotsTriggerReceiver)
			{
				collider2.ColliderPtr->SetCollisionResponse(CollisionResponsePolicy.RaiseTriggerEvents);
			}
			else if (ppt.UnitBas is IDotsCollisionReceiver)
			{
				collider2.ColliderPtr->SetCollisionResponse(CollisionResponsePolicy.CollideRaiseCollisionEvents);
			}
			else
			{
				collider2.ColliderPtr->SetCollisionResponse(CollisionResponsePolicy.Collide);
			}
			collider2.ColliderPtr->SetRestitution(collider.material.bounciness);
			if (boxCollider != null)
			{
				Unity.Physics.BoxCollider* colliderPtr = (Unity.Physics.BoxCollider*)collider2.ColliderPtr;
				BoxGeometry geometry = colliderPtr->Geometry;
				geometry.Center = boxCollider.center;
				geometry.Size = boxCollider.size;
				colliderPtr->Geometry = geometry;
			}
			else
			{
				Unity.Physics.CapsuleCollider* colliderPtr2 = (Unity.Physics.CapsuleCollider*)collider2.ColliderPtr;
				CapsuleGeometry geometry2 = colliderPtr2->Geometry;
				float height = ppt.CC_Self.height;
				Vector3 center = ppt.CC_Self.center;
				geometry2.Vertex0 = center - new Vector3(0f, 0f, height / 2f);
				geometry2.Vertex1 = center + new Vector3(0f, 0f, height / 2f);
				geometry2.Radius = ppt.CC_Self.radius;
				colliderPtr2->Geometry = geometry2;
			}
			switch (ppt.gameObject.layer)
			{
			case 11:
				DTool.SetCollider(in collider2, 2048u);
				break;
			case 12:
				DTool.SetCollider(in collider2, 4096u);
				break;
			case 13:
				DTool.SetCollider(in collider2, 8192u);
				break;
			case 15:
				DTool.SetCollider(in collider2, 32768u);
				break;
			case 17:
				DTool.SetCollider(in collider2, 131072u);
				break;
			case 9:
				DTool.SetCollider(in collider2, 512u);
				break;
			}
			if (ppt.unitCfg.isSolidObj)
			{
				CollisionFilter collisionFilter = collider2.ColliderPtr->GetCollisionFilter();
				collisionFilter.BelongsTo |= 256u;
				collider2.ColliderPtr->SetCollisionFilter(collisionFilter);
			}
			entityMgr.SetComponentData(entity, collider2);
			PhysicsMassOverride componentData2 = default(PhysicsMassOverride);
			if (ppt.Rigid.isKinematic)
			{
				componentData2.IsKinematic = 1;
				componentData2.SetVelocityToZero = 1;
			}
			else
			{
				componentData2.IsKinematic = 0;
				componentData2.SetVelocityToZero = 0;
			}
			entityMgr.AddComponentData(entity, componentData2);
			if (ppt.Rigid.interpolation != 0 || ppt.isRigidLerp_Dots)
			{
				ppt.Rigid.interpolation = RigidbodyInterpolation.None;
				ppt.isRigidLerp_Dots = true;
				PhysicsGraphicalSmoothing componentData3 = default(PhysicsGraphicalSmoothing);
				componentData3.ApplySmoothing = 0;
				entityMgr.AddComponentData(entity, componentData3);
				entityMgr.AddComponentData(entity, default(PhysicsGraphicalInterpolationBuffer));
				LocalToWorld componentData4 = GetComponentData<LocalToWorld>(entity);
				Vector3 position = ppt.transform.position;
				componentData4.Value.c3 = new float4(position.x, position.y, position.z, componentData4.Value.c3.w);
				entityMgr.SetComponentData(entity, componentData4);
			}
			if (ppt.UnitBas is IDotsTriggerReceiver)
			{
				entityMgr.AddBuffer<StatefulTriggerEvent>(entity);
				UnitTriggerReference componentData5 = new UnitTriggerReference
				{
					reference = (ppt.UnitBas as IDotsTriggerReceiver)
				};
				entityMgr.AddComponentObject(entity, componentData5);
			}
			if (ppt.UnitBas is IDotsCollisionReceiver)
			{
				entityMgr.AddBuffer<StatefulCollisionEvent>(entity);
				StatefulCollisionEventDetails statefulCollisionEventDetails = default(StatefulCollisionEventDetails);
				statefulCollisionEventDetails.CalculateDetails = true;
				StatefulCollisionEventDetails componentData6 = statefulCollisionEventDetails;
				entityMgr.AddComponentData(entity, componentData6);
				UnitCollisionReference componentData7 = new UnitCollisionReference
				{
					reference = (ppt.UnitBas as IDotsCollisionReceiver)
				};
				entityMgr.AddComponentObject(entity, componentData7);
			}
		}
		LocalTransform componentData8 = entityMgr.GetComponentData<LocalTransform>(entity);
		componentData8.Position = ppt.transform.position;
		entityMgr.SetComponentData(entity, componentData8);
		UnitProperty_Dots componentData9 = entityMgr.GetComponentData<UnitProperty_Dots>(entity);
		componentData9.id = ppt.unitCfg.id;
		componentData9.Initialize(UnitConfig.map);
		componentData9.unitCfg = ppt.unitCfg;
		componentData9.registered = true;
		entityMgr.SetComponentData(entity, componentData9);
		LevelMgr.Inst.CurrentRoomCtrller.UnitRegister(entity);
		ppt.myEntity = entity;
		existingUnit.Add(ppt);
	}

	[Preserve]
	protected override void OnCreate()
	{
		entityMgr = World.DefaultGameObjectInjectionWorld.EntityManager;
		initialized = true;
		allUnitEttQuery = entityMgr.CreateEntityQuery(typeof(AllUnitEtt));
		Inst = this;
		EventMgr.DotsSystemReset = (Action)Delegate.Combine(EventMgr.DotsSystemReset, (Action)delegate
		{
			takeDamageRequestList.Clear();
			shootSpellParamList.Clear();
			SrcCapsule = Entity.Null;
			SrcBox = Entity.Null;
			EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer(Allocator.TempJob);
			foreach (QueryEnumerableWithEntity<UnitPptReference> item2 in IFE_898147355_0.Query(__query_898147355_0, __TypeHandle.__IFE_898147355_0_TypeHandle, ref base.CheckedStateRef))
			{
				item2.Deconstruct(out var _, out var entity);
				Entity entity2 = entity;
				if ((bool)LevelMgr.Inst.CurrentRoomCtrller)
				{
					if (LevelMgr.Inst.CurrentRoomCtrller.monsterEttList.Contains(entity2))
					{
						LevelMgr.Inst.CurrentRoomCtrller.monsterEttList.Remove(entity2);
					}
					if (LevelMgr.Inst.CurrentRoomCtrller.targetableEttList.Contains(entity2))
					{
						LevelMgr.Inst.CurrentRoomCtrller.targetableEttList.Remove(entity2);
					}
				}
				entityCommandBuffer.DestroyEntity(entity2);
			}
			entityCommandBuffer.Playback(entityMgr);
			entityCommandBuffer.Dispose();
		});
	}

	[Preserve]
	protected override void OnDestroy()
	{
		allUnitEttQuery.Dispose();
		base.OnDestroy();
		EventMgr.DotsSystemReset = (Action)Delegate.Remove(EventMgr.DotsSystemReset, (Action)delegate
		{
			takeDamageRequestList.Clear();
			shootSpellParamList.Clear();
			SrcCapsule = Entity.Null;
			SrcBox = Entity.Null;
			EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer(Allocator.TempJob);
			foreach (QueryEnumerableWithEntity<UnitPptReference> item2 in IFE_898147355_0.Query(__query_898147355_0, __TypeHandle.__IFE_898147355_0_TypeHandle, ref base.CheckedStateRef))
			{
				item2.Deconstruct(out var _, out var entity);
				Entity entity2 = entity;
				if ((bool)LevelMgr.Inst.CurrentRoomCtrller)
				{
					if (LevelMgr.Inst.CurrentRoomCtrller.monsterEttList.Contains(entity2))
					{
						LevelMgr.Inst.CurrentRoomCtrller.monsterEttList.Remove(entity2);
					}
					if (LevelMgr.Inst.CurrentRoomCtrller.targetableEttList.Contains(entity2))
					{
						LevelMgr.Inst.CurrentRoomCtrller.targetableEttList.Remove(entity2);
					}
				}
				entityCommandBuffer.DestroyEntity(entity2);
			}
			entityCommandBuffer.Playback(entityMgr);
			entityCommandBuffer.Dispose();
		});
	}

	[Preserve]
	protected override void OnUpdate()
	{
		pws = __query_898147355_1.GetSingleton<PhysicsWorldSingleton>();
		using EntityQuery entityQuery = entityMgr.CreateEntityQuery(typeof(SpellSingleton));
		if (entityQuery.CalculateEntityCount() > 0)
		{
			spellSingleton = __query_898147355_2.GetSingleton<SpellSingleton>();
		}
		if (allUnitEttQuery.CalculateEntityCount() > 0)
		{
			AllUnitEtt singleton = allUnitEttQuery.GetSingleton<AllUnitEtt>();
			SrcBox = singleton.map[999998];
			SrcCapsule = singleton.map[999999];
			SrcNoCollider = singleton.map[999995];
		}
		for (int i = 0; i < takeDamageRequestList.Count; i++)
		{
			Entity target = takeDamageRequestList[i].target;
			if (InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__UnitProperty_Dots_RO_ComponentLookup, ref base.CheckedStateRef, target))
			{
				TakeDamageInfo_Dots info = takeDamageRequestList[i].info;
				entityMgr.GetBuffer<TakeDamageInfo_Dots>(target).Add(info);
			}
		}
		takeDamageRequestList.Clear();
		for (int j = 0; j < shootSpellParamList.Count; j++)
		{
			SpellShootSystem.Shoot(shootSpellParamList[j]);
		}
		shootSpellParamList.Clear();
	}

	public static bool SphereCast(UnityEngine.Ray ray, float radius, float distance, CollisionFilter filter)
	{
		RayCastHitResult result;
		return SphereCast(ray.origin, ray.direction, radius, distance, filter, out result);
	}

	public static bool SphereCast(UnityEngine.Ray ray, float radius, float distance, CollisionFilter filter, out RayCastHitResult result)
	{
		return SphereCast(ray.origin, ray.direction, radius, distance, filter, out result);
	}

	public static bool SphereCast(Vector3 startPos, Vector3 direction, float radius, float distance, CollisionFilter filter, out RayCastHitResult result)
	{
		result = default(RayCastHitResult);
		if (!initialized)
		{
			return false;
		}
		RaycastInput raycastInput = default(RaycastInput);
		raycastInput.Start = startPos;
		raycastInput.End = startPos + distance * direction.normalized;
		raycastInput.Filter = filter;
		RaycastInput raycastInput2 = raycastInput;
		Debug.DrawLine(startPos, raycastInput2.End);
		if (pws.SphereCast(startPos, radius, direction.normalized, distance, out var hitInfo, filter) && EntityIsValid(hitInfo.Entity))
		{
			result = new RayCastHitResult
			{
				entity = hitInfo.Entity,
				point = hitInfo.Position,
				normal = hitInfo.SurfaceNormal
			};
			return true;
		}
		return false;
	}

	public static RayCastHitResult[] SphereCastAll(Vector3 startPos, Vector3 direction, float radius, float distance, CollisionFilter filter)
	{
		if (!initialized)
		{
			return new RayCastHitResult[0];
		}
		NativeList<ColliderCastHit> outHits = new NativeList<ColliderCastHit>(Allocator.Temp);
		if (pws.SphereCastAll(startPos, radius, direction.normalized, distance, ref outHits, filter))
		{
			for (int num = outHits.Length - 1; num >= 0; num--)
			{
				if (!EntityIsValid(outHits[num].Entity))
				{
					outHits.RemoveAt(num);
				}
			}
			RayCastHitResult[] array = new RayCastHitResult[outHits.Length];
			for (int i = 0; i < outHits.Length; i++)
			{
				array[i] = new RayCastHitResult
				{
					entity = outHits[i].Entity,
					point = outHits[i].Position,
					normal = outHits[i].SurfaceNormal
				};
			}
			return array;
		}
		return new RayCastHitResult[0];
	}

	public static RayCastHitResult[] RayCastAll(Vector3 startPos, Vector3 direction, float distance, CollisionFilter filter)
	{
		if (!initialized)
		{
			return new RayCastHitResult[0];
		}
		NativeList<ColliderCastHit> outHits = new NativeList<ColliderCastHit>(Allocator.Temp);
		if (pws.SphereCastAll(startPos, 0.01f, direction.normalized, distance, ref outHits, filter))
		{
			for (int num = outHits.Length - 1; num >= 0; num--)
			{
				if (!EntityIsValid(outHits[num].Entity))
				{
					outHits.RemoveAt(num);
				}
			}
			RayCastHitResult[] array = new RayCastHitResult[outHits.Length];
			for (int i = 0; i < outHits.Length; i++)
			{
				array[i] = new RayCastHitResult
				{
					entity = outHits[i].Entity,
					point = outHits[i].Position,
					normal = outHits[i].SurfaceNormal
				};
			}
			return array;
		}
		return new RayCastHitResult[0];
	}

	public static bool Raycast(UnityEngine.Ray ray, float distance, CollisionFilter filter)
	{
		RayCastHitResult result;
		return Raycast(ray.origin, ray.direction, distance, filter, out result);
	}

	public static bool Raycast(UnityEngine.Ray ray, float distance, CollisionFilter filter, out RayCastHitResult result)
	{
		return Raycast(ray.origin, ray.direction, distance, filter, out result);
	}

	public static bool Raycast(Vector3 startPos, Vector3 direction, float distance, CollisionFilter filter, out RayCastHitResult result)
	{
		result = default(RayCastHitResult);
		if (!initialized)
		{
			return false;
		}
		RaycastInput raycastInput = default(RaycastInput);
		raycastInput.Start = startPos;
		raycastInput.End = startPos + distance * direction.normalized;
		raycastInput.Filter = filter;
		RaycastInput input = raycastInput;
		if (pws.CastRay(input, out var closestHit) && EntityIsValid(closestHit.Entity))
		{
			result = new RayCastHitResult
			{
				entity = closestHit.Entity,
				point = closestHit.Position,
				normal = closestHit.SurfaceNormal
			};
			return true;
		}
		return false;
	}

	public static bool HaveCollider(Vector3 centerPoint, float radius, CollisionFilter filter)
	{
		if (!initialized)
		{
			return false;
		}
		return pws.CheckSphere(centerPoint, radius, filter);
	}

	public static void GetCollidersInRangeWithAngle(Vector3 centerPoint, float radius, Vector3 fromDir, float halfAngle, CollisionFilter filter, List<DistanceHitResult> results)
	{
		results.Clear();
		if (!initialized)
		{
			return;
		}
		NativeList<DistanceHit> outHits = new NativeList<DistanceHit>(Allocator.Temp);
		pws.OverlapSphere(centerPoint, radius, ref outHits, filter);
		for (int i = 0; i < outHits.Length; i++)
		{
			if (Vector3.Angle(fromDir, Tool2D.IgnoreZV2ToV1Normal(outHits[i].Position, centerPoint)) <= halfAngle && EntityIsValid(outHits[i].Entity))
			{
				results.Add(new DistanceHitResult
				{
					entity = outHits[i].Entity,
					point = outHits[i].Position,
					distance = outHits[i].Distance
				});
			}
		}
	}

	public static void GetCollidersInRange(Vector3 centerPoint, float radius, CollisionFilter filter, List<DistanceHitResult> results)
	{
		results.Clear();
		if (!initialized)
		{
			return;
		}
		NativeList<DistanceHit> outHits = new NativeList<DistanceHit>(Allocator.Temp);
		pws.OverlapSphere(centerPoint, radius, ref outHits, filter);
		for (int i = 0; i < outHits.Length; i++)
		{
			if (EntityIsValid(outHits[i].Entity))
			{
				results.Add(new DistanceHitResult
				{
					entity = outHits[i].Entity,
					point = outHits[i].Position,
					distance = outHits[i].Distance
				});
			}
		}
		outHits.Dispose();
	}

	public static void OverlapBox(Vector3 centerPoint, Vector3 halfSize, Quaternion orientation, CollisionFilter filter, List<DistanceHitResult> results)
	{
		results.Clear();
		if (!initialized)
		{
			return;
		}
		NativeList<DistanceHit> outHits = new NativeList<DistanceHit>(Allocator.Temp);
		pws.OverlapBox(centerPoint, orientation, halfSize, ref outHits, filter);
		for (int i = 0; i < outHits.Length; i++)
		{
			if (EntityIsValid(outHits[i].Entity))
			{
				results.Add(new DistanceHitResult
				{
					entity = outHits[i].Entity,
					point = outHits[i].Position,
					distance = outHits[i].Distance
				});
			}
		}
		outHits.Dispose();
	}

	private static void CreateOtherCampFilter(UnitType self, bool containsBrittleness, out CollisionFilter res)
	{
		res = CollisionFilter.Default;
		res.CollidesWith = (UnitConfig.IsSameCamp(self, UnitType.Player) ? 14336u : 2097664u);
		if (containsBrittleness)
		{
			res.CollidesWith |= 163840u;
		}
	}

	public static void GetAttackableEntitiesInRange(Vector3 centerPoint, float radius, UnitType selfCamp, bool containsBrittleness, ref NativeList<Entity> results, bool checkUnitCamp = true)
	{
		CollisionFilter res;
		if (checkUnitCamp)
		{
			CreateOtherCampFilter(selfCamp, containsBrittleness, out res);
		}
		else
		{
			CreateOtherCampFilter(UnitType.Player, containsBrittleness: true, out res);
			CreateOtherCampFilter(UnitType.Monster, containsBrittleness: true, out var res2);
			res.BelongsTo |= res2.BelongsTo;
			res.CollidesWith |= res2.CollidesWith;
		}
		results.Clear();
		NativeList<DistanceHit> outHits = new NativeList<DistanceHit>(Allocator.Temp);
		pws.OverlapSphere(centerPoint, radius, ref outHits, res);
		for (int i = 0; i < outHits.Length; i++)
		{
			if (EntityIsValid(outHits[i].Entity))
			{
				Entity value = outHits[i].Entity;
				results.Add(in value);
			}
		}
		CollisionFilter @default = CollisionFilter.Default;
		@default.CollidesWith = (DTool.IsSameCamp(selfCamp, UnitType.Player) ? 8388608u : 16777216u);
		NativeList<DistanceHit> outHits2 = new NativeList<DistanceHit>(Allocator.Temp);
		pws.OverlapSphere(centerPoint, radius, ref outHits2, @default);
		foreach (DistanceHit item in outHits2)
		{
			if (EntityIsValid(item.Entity))
			{
				Entity value = item.Entity;
				results.Add(in value);
			}
		}
	}

	public static SpellTools.HitType TryAttackEntity(in Entity targetEtt, in TakeDamageInfo_Dots damageInfo, EntityManager ettMgr, bool checkCamp = true)
	{
		if (ettMgr.HasComponent<UnitProperty_Dots>(targetEtt))
		{
			UnitProperty_Dots componentData = ettMgr.GetComponentData<UnitProperty_Dots>(targetEtt);
			if (damageInfo.attackerEntity != Entity.Null)
			{
				UnitProperty_Dots componentData2 = ettMgr.GetComponentData<UnitProperty_Dots>(damageInfo.attackerEntity);
				if (checkCamp && DTool.IsSameCamp(componentData.unitCfg.unitType, componentData2.unitCfg.unitType))
				{
					return SpellTools.HitType.SameCamp;
				}
			}
			if (componentData.unitCfg.unitType == UnitType.Brittleness)
			{
				AddTakeDamageRequest(targetEtt, damageInfo);
				return SpellTools.HitType.Brittleness;
			}
			if (componentData.isDead)
			{
				return SpellTools.HitType.Unknown;
			}
			AddTakeDamageRequest(targetEtt, damageInfo);
			return SpellTools.HitType.Unit;
		}
		if (ettMgr.HasComponent<SpellConfigComponentData>(targetEtt))
		{
			SpellConfigComponentData rollballConfig = ettMgr.GetComponentData<SpellConfigComponentData>(targetEtt);
			UnitProperty_Dots componentData3 = ettMgr.GetComponentData<UnitProperty_Dots>(damageInfo.attackerEntity);
			if (checkCamp && DTool.IsSameCamp(componentData3.unitCfg.unitType, rollballConfig.ShooterType))
			{
				return SpellTools.HitType.SameCamp;
			}
			if (rollballConfig.AbilityType == SpellAbilityType.Rollball && rollballConfig.Float1 > 0f)
			{
				SpellTools.AttackRollball(in ettMgr, damageInfo.damage, ref rollballConfig, in targetEtt);
				ettMgr.SetComponentData(targetEtt, rollballConfig);
				return SpellTools.HitType.RollBall;
			}
			if (rollballConfig.AbilityType == SpellAbilityType.Butterfly)
			{
				ettMgr.SetComponentEnabled<Spell1003ButterflyBeAttackedTag>(targetEtt, value: true);
				return SpellTools.HitType.Butterfly;
			}
			return SpellTools.HitType.IgnoreSpell;
		}
		return SpellTools.HitType.Unknown;
	}

	public static void UnitRecoveryHP(Entity unitEtt, float recoveryHP, EntityManager ettMgr, bool needTextFloat = true, bool needCreateEF = true)
	{
		if (!ettMgr.HasComponent<UnitProperty_Dots>(unitEtt))
		{
			return;
		}
		UnitProperty_Dots componentData = ettMgr.GetComponentData<UnitProperty_Dots>(unitEtt);
		if (componentData.unitCfg.unitType == UnitType.Player && PlayerMgr.Inst.ItemCtrller.relic_GreedSeed != null && PlayerMgr.Inst.ItemCtrller.relic_GreedSeed.RelicCfg.intTimer < PlayerMgr.Inst.ItemCtrller.relic_GreedSeed.RelicCfg.int1.result)
		{
			float num = recoveryHP;
			PlayerMgr.Inst.ItemCtrller.relic_GreedSeed.RelicCfg.intTimer += (int)recoveryHP;
			if (PlayerMgr.Inst.ItemCtrller.relic_GreedSeed.RelicCfg.intTimer >= PlayerMgr.Inst.ItemCtrller.relic_GreedSeed.RelicCfg.int1.result)
			{
				recoveryHP = PlayerMgr.Inst.ItemCtrller.relic_GreedSeed.RelicCfg.intTimer - PlayerMgr.Inst.ItemCtrller.relic_GreedSeed.RelicCfg.int1.result;
				PlayerMgr.Inst.ItemCtrller.relic_GreedSeed.RelicCfg.intTimer = PlayerMgr.Inst.ItemCtrller.relic_GreedSeed.RelicCfg.int1.result;
				ObjPoolMgr.Inst.GetUIGO("Prefabs/UI/UITextFloat").GetComponent<UITextFloat>().Initialize(PlayerMgr.Inst.ItemCtrller.relic_GreedSeed.RelicCfg.GetName() + 1002008.GetText(), UITextFloatType.Recover, PlayerMgr.Inst.ItemCtrller.relic_GreedSeed.transform.position);
				PlayerMgr.Inst.ChangeHPMax(PlayerMgr.Inst.ItemCtrller.relic_GreedSeed.RelicCfg.int2.result);
				componentData = ettMgr.GetComponentData<UnitProperty_Dots>(unitEtt);
				UnitRecoveryHP(unitEtt, PlayerMgr.Inst.ItemCtrller.relic_GreedSeed.RelicCfg.int2.result, ettMgr, needTextFloat: false);
				componentData = ettMgr.GetComponentData<UnitProperty_Dots>(unitEtt);
				PlayerMgr.Inst.ItemCtrller.relic_GreedSeed.HideSelf();
			}
			else
			{
				recoveryHP = 0f;
				ObjPoolMgr.Inst.GetUIGO("Prefabs/UI/UITextFloat").GetComponent<UITextFloat>().Initialize(PlayerMgr.Inst.ItemCtrller.relic_GreedSeed.RelicCfg.GetName() + "+" + (num - recoveryHP), UITextFloatType.Recover, PlayerMgr.Inst.ItemCtrller.relic_GreedSeed.transform.position);
			}
			if (recoveryHP > 0f)
			{
				componentData = ettMgr.GetComponentData<UnitProperty_Dots>(unitEtt);
				componentData.unitCfg.currentHP = Mathf.Min(componentData.unitCfg.currentHP + recoveryHP, componentData.unitCfg.maxHP);
				PlayerMgr.Inst.PlayerCtrller.TextFloatQueueAdd("+" + recoveryHP, UITextFloatType.Recover);
				ettMgr.SetComponentData(unitEtt, componentData);
			}
			return;
		}
		componentData.unitCfg.currentHP = Mathf.Min(componentData.unitCfg.currentHP + recoveryHP, componentData.unitCfg.maxHP);
		ettMgr.SetComponentData(unitEtt, componentData);
		if (needCreateEF)
		{
			using EntityQuery entityQuery = ettMgr.CreateEntityQuery(typeof(SpellSingleton));
			entityQuery.GetSingleton<SpellSingleton>().Prefabs.TryGetValue("3108_Heal", out var item);
			Entity entity = ettMgr.Instantiate(item);
			ettMgr.SetComponentData(entity, new FollowEntity
			{
				ett = unitEtt
			});
		}
		if (needTextFloat)
		{
			if (GameMgr.IsSupportVFX)
			{
				if (DataMgr.settingData.textFloat || componentData.unitCfg.unitType == UnitType.Player)
				{
					using EntityQuery entityQuery2 = ettMgr.CreateEntityQuery(typeof(TextFloatVFXBED));
					Entity singletonEntity = entityQuery2.GetSingletonEntity();
					ettMgr.GetBuffer<TextFloatVFXBED>(singletonEntity).Add(new TextFloatVFXBED
					{
						number = Mathf.Ceil(recoveryHP),
						type = UITextFloatType.Recover,
						worldPos = ettMgr.GetComponentData<LocalTransform>(unitEtt).Position
					});
				}
			}
			else if (componentData.unitCfg.unitType == UnitType.Player)
			{
				ObjPoolMgr.Inst.GetUIGO("Prefabs/UI/UITextFloat").GetComponent<UITextFloat>().Initialize("+" + Mathf.Ceil(recoveryHP), UITextFloatType.Recover, ettMgr.GetComponentData<LocalTransform>(unitEtt).Position);
			}
		}
		if (componentData.unitCfg.unitType == UnitType.Player)
		{
			UIPlayerDataMgr.Inst.UpdateHP();
			EventMgr.PlayerHPOrShiledChange?.Invoke();
		}
	}

	public static bool TryGetComponent<T>(Entity entity, out T result) where T : unmanaged, IComponentData
	{
		result = new T();
		if (entityMgr.HasComponent<T>(entity))
		{
			result = entityMgr.GetComponentData<T>(entity);
			return true;
		}
		return false;
	}

	public static void AddTakeDamageRequestEndless(Entity target, TakeDamageInfo_Dots info)
	{
		info.damage *= GameConstManaged.endlessMonsterDamageRatio;
		EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
		if (entityManager.HasComponent<UnitProperty_Dots>(info.attackerEntity) && entityManager.GetComponentData<UnitProperty_Dots>(info.attackerEntity).unitCfg.unitType == UnitType.Boss)
		{
			info.damage *= GameConstManaged.EndlessBossDamageRatio;
		}
		takeDamageRequestList.Add(new TakeDamageRequest
		{
			target = target,
			info = info
		});
	}

	public static void AddTakeDamageRequest(Entity target, TakeDamageInfo_Dots info)
	{
		takeDamageRequestList.Add(new TakeDamageRequest
		{
			target = target,
			info = info
		});
	}

	public static SpellSpawnParams GetSpellPrototype(int id, UnitType type = UnitType.Monster)
	{
		SpellSpawnParams result = spellSingleton.BuildSpellSpawnParamsPrototype(id, Vector3.zero, Vector3.zero);
		result.ConfigComponentData.ColorType = SpellColorType.Monster;
		result.ConfigComponentData.ShooterType = type;
		return result;
	}

	public static void ShootSpell(SpellSpawnParams param)
	{
		shootSpellParamList.Add(param);
	}

	public unsafe static void SetColliderEnable(bool enabled, UnitBase unitBase)
	{
		PhysicsCollider componentData = entityMgr.GetComponentData<PhysicsCollider>(unitBase.myPpt.myEntity);
		CollisionFilter collisionFilter = componentData.ColliderPtr->GetCollisionFilter();
		if (collisionFilter.BelongsTo != 0)
		{
			unitBase.entityFilter = collisionFilter;
		}
		if (!enabled)
		{
			componentData.ColliderPtr->SetCollisionResponse(CollisionResponsePolicy.None);
			componentData.ColliderPtr->SetCollisionFilter(GameConst.Filter_None);
		}
		else if (unitBase is IDotsTriggerReceiver || ((bool)unitBase.myPpt.CC_Self && unitBase.myPpt.CC_Self.isTrigger) || ((bool)unitBase.myPpt.BC_Self && unitBase.myPpt.BC_Self.isTrigger))
		{
			componentData.ColliderPtr->SetCollisionResponse(CollisionResponsePolicy.RaiseTriggerEvents);
			componentData.ColliderPtr->SetCollisionFilter(unitBase.entityFilter);
		}
		else if (unitBase is IDotsCollisionReceiver)
		{
			componentData.ColliderPtr->SetCollisionResponse(CollisionResponsePolicy.CollideRaiseCollisionEvents);
			componentData.ColliderPtr->SetCollisionFilter(unitBase.entityFilter);
		}
		else
		{
			componentData.ColliderPtr->SetCollisionResponse(CollisionResponsePolicy.Collide);
			componentData.ColliderPtr->SetCollisionFilter(unitBase.entityFilter);
		}
		entityMgr.SetComponentData(unitBase.myPpt.myEntity, componentData);
	}

	public static bool EntityIsValid(Entity entity)
	{
		if (entity != Entity.Null && entityMgr.Exists(entity))
		{
			return entityMgr.HasComponent<LocalTransform>(entity);
		}
		return false;
	}

	public static bool ProcessHitSpell(Entity entity, float damage, out bool hitRollBall)
	{
		hitRollBall = false;
		if (TryGetComponent<SpellConfigComponentData>(entity, out var result))
		{
			if (result.AbilityType == SpellAbilityType.Butterfly)
			{
				entityMgr.SetComponentEnabled<Spell1003ButterflyBeAttackedTag>(entity, value: true);
			}
			else if (result.AbilityType == SpellAbilityType.Rollball)
			{
				SpellTools.AttackRollball(in entityMgr, damage, ref result, in entity);
				entityMgr.SetComponentData(entity, result);
				hitRollBall = true;
			}
			return true;
		}
		return false;
	}

	public static bool ProcessHitUnit(Entity entity, TakeDamageInfo_Dots info)
	{
		if (HasComponent<UnitProperty_Dots>(entity))
		{
			entityMgr.GetBuffer<TakeDamageInfo_Dots>(entity).Add(info);
			return true;
		}
		return false;
	}

	public static DynamicBuffer<T> GetBuffer<T>(Entity entity) where T : unmanaged, IBufferElementData
	{
		return entityMgr.GetBuffer<T>(entity);
	}

	public static T GetComponentData<T>(Entity entity) where T : unmanaged, IComponentData
	{
		return entityMgr.GetComponentData<T>(entity);
	}

	public static T GetComponentObject<T>(Entity entity)
	{
		return entityMgr.GetComponentObject<T>(entity);
	}

	public static void SetComponentData<T>(T component, Entity entity) where T : unmanaged, IComponentData
	{
		entityMgr.SetComponentData(entity, component);
	}

	public new static bool HasComponent<T>(Entity entity)
	{
		return entityMgr.HasComponent<T>(entity);
	}

	public unsafe static uint GetLayer(Entity entity)
	{
		if (TryGetComponent<PhysicsCollider>(entity, out var result))
		{
			CollisionFilter collisionFilter = result.ColliderPtr->GetCollisionFilter();
			uint belongsTo = collisionFilter.BelongsTo;
			switch (belongsTo)
			{
			case 512u:
			case 2048u:
			case 4096u:
			case 8192u:
			case 32768u:
			case 131072u:
			case 2097152u:
				if (HasComponent<UnitProperty_Dots>(entity))
				{
					return belongsTo;
				}
				return 0u;
			case 131328u:
				if (HasComponent<UnitProperty_Dots>(entity))
				{
					return 131072u;
				}
				return 0u;
			case 8388608u:
			case 16777216u:
				if (HasComponent<SpellConfigComponentData>(entity))
				{
					return belongsTo;
				}
				return 0u;
			default:
				return collisionFilter.BelongsTo;
			}
		}
		Debug.LogError("提供的entity不含碰撞体，怎么回事");
		return 0u;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAllRW<UnitPptReference>();
		__query_898147355_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<PhysicsWorldSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_898147355_1 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_898147355_2 = entityQueryBuilder2.Build(ref state);
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
	public UnitDotsSyncSystem()
	{
	}
}
