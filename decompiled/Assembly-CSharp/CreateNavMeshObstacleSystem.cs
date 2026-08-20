using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Internal;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.AI;

[CompilerGenerated]
public struct CreateNavMeshObstacleSystem : ISystem, ISystemCompilerGenerated
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_1074473396_0
	{
		public struct ResolvedChunk
		{
			public IntPtr item1_IntPtr;

			public IntPtr item2_IntPtr;

			public IntPtr item3_IntPtr;

			public IntPtr item4_IntPtr;

			public IntPtr Entity_IntPtr;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<CreateNavMeshObstacle>, InternalCompilerInterface.UncheckedRefRW<CreateNavMeshObstacleCleanUp>, LocalTransform, LocalToWorld> Get(int index)
			{
				return new QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<CreateNavMeshObstacle>, InternalCompilerInterface.UncheckedRefRW<CreateNavMeshObstacleCleanUp>, LocalTransform, LocalToWorld>(InternalCompilerInterface.UnsafeGetUncheckedRefRW<CreateNavMeshObstacle>(item1_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<CreateNavMeshObstacleCleanUp>(item2_IntPtr, index), InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<LocalTransform>(item3_IntPtr, index), InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<LocalToWorld>(item4_IntPtr, index), InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(Entity_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			private ComponentTypeHandle<CreateNavMeshObstacle> item1_ComponentTypeHandle_RW;

			private ComponentTypeHandle<CreateNavMeshObstacleCleanUp> item2_ComponentTypeHandle_RW;

			[ReadOnly]
			private ComponentTypeHandle<LocalTransform> item3_ComponentTypeHandle_RO;

			[ReadOnly]
			private ComponentTypeHandle<LocalToWorld> item4_ComponentTypeHandle_RO;

			private EntityTypeHandle Entity_TypeHandle;

			public TypeHandle(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<CreateNavMeshObstacle>();
				item2_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<CreateNavMeshObstacleCleanUp>();
				item3_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<LocalTransform>(isReadOnly: true);
				item4_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<LocalToWorld>(isReadOnly: true);
				Entity_TypeHandle = systemState.GetEntityTypeHandle();
			}

			public void Update(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW.Update(ref systemState);
				item2_ComponentTypeHandle_RW.Update(ref systemState);
				item3_ComponentTypeHandle_RO.Update(ref systemState);
				item4_ComponentTypeHandle_RO.Update(ref systemState);
				Entity_TypeHandle.Update(ref systemState);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ResolvedChunk Resolve(ArchetypeChunk archetypeChunk)
			{
				ResolvedChunk result = default(ResolvedChunk);
				result.item1_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item1_ComponentTypeHandle_RW);
				result.item2_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item2_ComponentTypeHandle_RW);
				result.item3_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks(in archetypeChunk, ref item3_ComponentTypeHandle_RO);
				result.item4_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks(in archetypeChunk, ref item4_ComponentTypeHandle_RO);
				result.Entity_IntPtr = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(archetypeChunk, Entity_TypeHandle);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<CreateNavMeshObstacle>, InternalCompilerInterface.UncheckedRefRW<CreateNavMeshObstacleCleanUp>, LocalTransform, LocalToWorld>>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<CreateNavMeshObstacle>, InternalCompilerInterface.UncheckedRefRW<CreateNavMeshObstacleCleanUp>, LocalTransform, LocalToWorld> Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRW<CreateNavMeshObstacle>();
			state.EntityManager.CompleteDependencyBeforeRW<CreateNavMeshObstacleCleanUp>();
			state.EntityManager.CompleteDependencyBeforeRO<LocalTransform>();
			state.EntityManager.CompleteDependencyBeforeRO<LocalToWorld>();
		}
	}

	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_1074473396_1
	{
		public struct ResolvedChunk
		{
			public IntPtr item1_IntPtr;

			public IntPtr Entity_IntPtr;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<CreateNavMeshObstacleCleanUp>> Get(int index)
			{
				return new QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<CreateNavMeshObstacleCleanUp>>(InternalCompilerInterface.UnsafeGetUncheckedRefRW<CreateNavMeshObstacleCleanUp>(item1_IntPtr, index), InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(Entity_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			private ComponentTypeHandle<CreateNavMeshObstacleCleanUp> item1_ComponentTypeHandle_RW;

			private EntityTypeHandle Entity_TypeHandle;

			public TypeHandle(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<CreateNavMeshObstacleCleanUp>();
				Entity_TypeHandle = systemState.GetEntityTypeHandle();
			}

			public void Update(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW.Update(ref systemState);
				Entity_TypeHandle.Update(ref systemState);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ResolvedChunk Resolve(ArchetypeChunk archetypeChunk)
			{
				ResolvedChunk result = default(ResolvedChunk);
				result.item1_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item1_ComponentTypeHandle_RW);
				result.Entity_IntPtr = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(archetypeChunk, Entity_TypeHandle);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<CreateNavMeshObstacleCleanUp>>>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<CreateNavMeshObstacleCleanUp>> Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRW<CreateNavMeshObstacleCleanUp>();
		}
	}

	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_1074473396_2
	{
		public struct ResolvedChunk
		{
			public IntPtr item1_IntPtr;

			public IntPtr item2_IntPtr;

			public IntPtr Entity_IntPtr;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<CreateNavMeshObstacle>, LocalTransform> Get(int index)
			{
				return new QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<CreateNavMeshObstacle>, LocalTransform>(InternalCompilerInterface.UnsafeGetUncheckedRefRW<CreateNavMeshObstacle>(item1_IntPtr, index), InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<LocalTransform>(item2_IntPtr, index), InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(Entity_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			private ComponentTypeHandle<CreateNavMeshObstacle> item1_ComponentTypeHandle_RW;

			[ReadOnly]
			private ComponentTypeHandle<LocalTransform> item2_ComponentTypeHandle_RO;

			private EntityTypeHandle Entity_TypeHandle;

			public TypeHandle(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<CreateNavMeshObstacle>();
				item2_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<LocalTransform>(isReadOnly: true);
				Entity_TypeHandle = systemState.GetEntityTypeHandle();
			}

			public void Update(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW.Update(ref systemState);
				item2_ComponentTypeHandle_RO.Update(ref systemState);
				Entity_TypeHandle.Update(ref systemState);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ResolvedChunk Resolve(ArchetypeChunk archetypeChunk)
			{
				ResolvedChunk result = default(ResolvedChunk);
				result.item1_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item1_ComponentTypeHandle_RW);
				result.item2_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks(in archetypeChunk, ref item2_ComponentTypeHandle_RO);
				result.Entity_IntPtr = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(archetypeChunk, Entity_TypeHandle);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<CreateNavMeshObstacle>, LocalTransform>>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<CreateNavMeshObstacle>, LocalTransform> Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRW<CreateNavMeshObstacle>();
			state.EntityManager.CompleteDependencyBeforeRO<LocalTransform>();
		}
	}

	private struct TypeHandle
	{
		public IFE_1074473396_0.TypeHandle __IFE_1074473396_0_TypeHandle;

		public IFE_1074473396_1.TypeHandle __IFE_1074473396_1_TypeHandle;

		public IFE_1074473396_2.TypeHandle __IFE_1074473396_2_TypeHandle;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__IFE_1074473396_0_TypeHandle = new IFE_1074473396_0.TypeHandle(ref state);
			__IFE_1074473396_1_TypeHandle = new IFE_1074473396_1.TypeHandle(ref state);
			__IFE_1074473396_2_TypeHandle = new IFE_1074473396_2.TypeHandle(ref state);
		}
	}

	private EntityQuery query;

	private EntityQuery query1;

	private TypeHandle __TypeHandle;

	private EntityQuery __query_1074473396_0;

	private EntityQuery __query_1074473396_1;

	private EntityQuery __query_1074473396_2;

	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<GlobalRandom>();
		query = state.EntityManager.CreateEntityQuery(typeof(CreateNavMeshObstacleCleanUp));
		query1 = state.EntityManager.CreateEntityQuery(typeof(CreateNavMeshObstacle));
		state.RequireAnyForUpdate(query, query1);
	}

	public void OnDestroy(ref SystemState state)
	{
		query.Dispose();
		query1.Dispose();
	}

	public void OnUpdate(ref SystemState state)
	{
		EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer(Allocator.Temp);
		InternalCompilerInterface.UncheckedRefRW<CreateNavMeshObstacle> item;
		InternalCompilerInterface.UncheckedRefRW<CreateNavMeshObstacleCleanUp> item2;
		LocalTransform item3;
		Entity entity;
		foreach (QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<CreateNavMeshObstacle>, InternalCompilerInterface.UncheckedRefRW<CreateNavMeshObstacleCleanUp>, LocalTransform, LocalToWorld> item5 in IFE_1074473396_0.Query(__query_1074473396_0, __TypeHandle.__IFE_1074473396_0_TypeHandle, ref state))
		{
			item5.Deconstruct(out item, out item2, out item3, out var item4, out entity);
			InternalCompilerInterface.UncheckedRefRW<CreateNavMeshObstacle> uncheckedRefRW = item;
			InternalCompilerInterface.UncheckedRefRW<CreateNavMeshObstacleCleanUp> uncheckedRefRW2 = item2;
			LocalTransform localTransform = item3;
			LocalToWorld localToWorld = item4;
			Entity e = entity;
			ref CreateNavMeshObstacle valueRW = ref uncheckedRefRW.ValueRW;
			ref CreateNavMeshObstacleCleanUp valueRW2 = ref uncheckedRefRW2.ValueRW;
			ref float3 lastPosition = ref valueRW.lastPosition;
			float3 f = localToWorld.Position;
			if (!DTool.IsTotallySame(in lastPosition, in f))
			{
				valueRW.lastPosition = localToWorld.Position;
				Quaternion rotation = GetRotation(localTransform.Rotation, in valueRW);
				valueRW2.obstacle.Value.transform.position = GetPosition(localToWorld.Position, localTransform.Rotation, rotation, in valueRW);
			}
			if (valueRW.onT6RockDestroyed)
			{
				if (valueRW2.obstacle.IsValid())
				{
					ObjPoolMgr.Inst.RecycleGO(valueRW2.obstacle.Value.gameObject);
				}
				entityCommandBuffer.DestroyEntity(e);
			}
			if (valueRW.chestDisable && valueRW2.obstacle.IsValid())
			{
				ObjPoolMgr.Inst.RecycleGO(valueRW2.obstacle.Value.gameObject);
			}
		}
		entityCommandBuffer.Playback(state.EntityManager);
		entityCommandBuffer.Dispose();
		entityCommandBuffer = new EntityCommandBuffer(Allocator.Temp);
		foreach (QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<CreateNavMeshObstacleCleanUp>> item6 in IFE_1074473396_1.Query(__query_1074473396_1, __TypeHandle.__IFE_1074473396_1_TypeHandle, ref state))
		{
			item6.Deconstruct(out item2, out entity);
			InternalCompilerInterface.UncheckedRefRW<CreateNavMeshObstacleCleanUp> uncheckedRefRW3 = item2;
			Entity e2 = entity;
			ref CreateNavMeshObstacleCleanUp valueRW3 = ref uncheckedRefRW3.ValueRW;
			entityCommandBuffer.RemoveComponent<CreateNavMeshObstacleCleanUp>(e2);
			if (!valueRW3.obstacle.IsValid())
			{
				continue;
			}
			if (valueRW3.isObjPool)
			{
				if (valueRW3.obstacle.Value.gameObject.activeSelf)
				{
					ObjPoolMgr.Inst.RecycleGO(valueRW3.obstacle.Value.gameObject);
				}
			}
			else
			{
				UnityEngine.Object.Destroy(valueRW3.obstacle.Value.gameObject);
			}
		}
		entityCommandBuffer.Playback(state.EntityManager);
		entityCommandBuffer.Dispose();
		entityCommandBuffer = new EntityCommandBuffer(Allocator.Temp);
		foreach (QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<CreateNavMeshObstacle>, LocalTransform> item7 in IFE_1074473396_2.Query(__query_1074473396_2, __TypeHandle.__IFE_1074473396_2_TypeHandle, ref state))
		{
			item7.Deconstruct(out item, out item3, out entity);
			InternalCompilerInterface.UncheckedRefRW<CreateNavMeshObstacle> uncheckedRefRW4 = item;
			LocalTransform localTransform2 = item3;
			Entity e3 = entity;
			ref CreateNavMeshObstacle valueRW4 = ref uncheckedRefRW4.ValueRW;
			if (!valueRW4.isInitialized)
			{
				valueRW4.isInitialized = true;
				NavMeshObstacle navMeshObstacle = (valueRW4.isCamp ? UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Mixed/NavMeshObstacle")).GetComponent<NavMeshObstacle>() : ObjPoolMgr.Inst.GetGO("Prefabs/Mixed/NavMeshObstacle").GetComponent<NavMeshObstacle>());
				Quaternion rotation2 = GetRotation(localTransform2.Rotation, in valueRW4);
				navMeshObstacle.shape = valueRW4.shape;
				navMeshObstacle.center = Vector3.zero;
				navMeshObstacle.size = valueRW4.size;
				navMeshObstacle.transform.rotation = rotation2;
				navMeshObstacle.transform.position = GetPosition(localTransform2.Position, localTransform2.Rotation, rotation2, in valueRW4);
				navMeshObstacle.gameObject.SetActive(value: true);
				if (valueRW4.shape == NavMeshObstacleShape.Capsule)
				{
					navMeshObstacle.center = Vector3.zero;
					navMeshObstacle.radius = valueRW4.radius;
					navMeshObstacle.height = valueRW4.height;
				}
				if (!valueRW4.isAbyss)
				{
					entityCommandBuffer.AddComponent<CreateNavMeshObstacleCleanUp>(e3);
					entityCommandBuffer.SetComponent(e3, new CreateNavMeshObstacleCleanUp
					{
						isObjPool = !valueRW4.isCamp,
						obstacle = navMeshObstacle
					});
				}
			}
		}
		entityCommandBuffer.Playback(state.EntityManager);
		entityCommandBuffer.Dispose();
	}

	private Quaternion GetRotation(quaternion rotation, in CreateNavMeshObstacle createNMO)
	{
		Quaternion result = rotation * Quaternion.Euler(0f, 0f, createNMO.extraAngle);
		if (createNMO.shape == NavMeshObstacleShape.Capsule)
		{
			result *= GetCapsuleDirectionRotation(createNMO.Direction);
		}
		return result;
	}

	private Vector3 GetPosition(float3 position, quaternion entityRotation, Quaternion finalRotation, in CreateNavMeshObstacle createNMO)
	{
		Quaternion quaternion = ((createNMO.shape == NavMeshObstacleShape.Capsule) ? ((Quaternion)entityRotation) : finalRotation);
		return (Vector3)position + quaternion * createNMO.center;
	}

	private Quaternion GetCapsuleDirectionRotation(NavMeshObstacleCapsuleDirection direction)
	{
		return direction switch
		{
			NavMeshObstacleCapsuleDirection.X => Quaternion.Euler(0f, 0f, 90f), 
			NavMeshObstacleCapsuleDirection.Z => Quaternion.Euler(90f, 0f, 0f), 
			_ => Quaternion.identity, 
		};
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<LocalTransform>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAll<LocalToWorld>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<CreateNavMeshObstacle>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<CreateNavMeshObstacleCleanUp>();
		__query_1074473396_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAllRW<CreateNavMeshObstacleCleanUp>();
		entityQueryBuilder2 = entityQueryBuilder2.WithNone<LocalTransform>();
		__query_1074473396_1 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<LocalTransform>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<CreateNavMeshObstacle>();
		entityQueryBuilder2 = entityQueryBuilder2.WithNone<CreateNavMeshObstacleCleanUp>();
		__query_1074473396_2 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder.Dispose();
	}

	public void OnCreateForCompiler(ref SystemState state)
	{
		__AssignQueries(ref state);
		__TypeHandle.__AssignHandles(ref state);
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreate(IntPtr self, IntPtr state)
	{
		((CreateNavMeshObstacleSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnUpdate(IntPtr self, IntPtr state)
	{
		((CreateNavMeshObstacleSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnDestroy(IntPtr self, IntPtr state)
	{
		((CreateNavMeshObstacleSystem*)self.ToPointer())->OnDestroy(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
	{
		((CreateNavMeshObstacleSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
	}
}
