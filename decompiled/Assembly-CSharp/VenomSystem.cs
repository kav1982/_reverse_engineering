using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Entities.Internal;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Extensions;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Scripting;

[CompilerGenerated]
[UpdateInGroup(typeof(LiquidSystemGroup))]
public class VenomSystem : SystemBase
{
	[CompilerGenerated]
	public struct VenomJob : IJobEntity, IJobChunk
	{
		public struct InternalCompilerQueryAndHandleData
		{
			public struct TypeHandle
			{
				public ComponentTypeHandle<Liquid_Dots> __Liquid_Dots_RW_ComponentTypeHandle;

				public ComponentTypeHandle<Venom_Dots> __Venom_Dots_RW_ComponentTypeHandle;

				[ReadOnly]
				public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public void __AssignHandles(ref SystemState state)
				{
					__Liquid_Dots_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Liquid_Dots>();
					__Venom_Dots_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Venom_Dots>();
					__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
				}

				public void Update(ref SystemState state)
				{
					__Liquid_Dots_RW_ComponentTypeHandle.Update(ref state);
					__Venom_Dots_RW_ComponentTypeHandle.Update(ref state);
					__Unity_Entities_Entity_TypeHandle.Update(ref state);
				}
			}

			public TypeHandle __TypeHandle;

			public EntityQuery DefaultQuery;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private void __AssignQueries(ref SystemState state)
			{
				EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
				EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAllRW<Liquid_Dots>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Venom_Dots>();
				DefaultQuery = entityQueryBuilder2.Build(ref state);
				entityQueryBuilder.Reset();
				entityQueryBuilder.Dispose();
			}

			public void Init(ref SystemState state, bool assignDefaultQuery)
			{
				if (assignDefaultQuery)
				{
					__AssignQueries(ref state);
				}
				__TypeHandle.__AssignHandles(ref state);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void Run(ref VenomJob job, EntityQuery query)
			{
				job.__TypeHandle = __TypeHandle;
				JobChunkExtensions.RunByRef(ref job, query);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle Schedule(ref VenomJob job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = __TypeHandle;
				return JobChunkExtensions.ScheduleByRef(ref job, query, dependency);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle ScheduleParallel(ref VenomJob job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = __TypeHandle;
				return JobChunkExtensions.ScheduleParallelByRef(ref job, query, dependency);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void UpdateBaseEntityIndexArray(ref VenomJob job, EntityQuery query, ref SystemState state)
			{
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle UpdateBaseEntityIndexArray(ref VenomJob job, EntityQuery query, JobHandle dependency, ref SystemState state)
			{
				return dependency;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void AssignEntityManager(ref VenomJob job, EntityManager entityManager)
			{
			}
		}

		[StructLayout(LayoutKind.Sequential, Size = 1)]
		public struct InternalCompiler
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
			public static void CheckForErrors(int scheduleType)
			{
			}
		}

		public float deltaTime;

		public EntityCommandBuffer.ParallelWriter ecb;

		public Entity bubbleBufferEntity;

		[NativeDisableUnsafePtrRestriction]
		public RefRW<GlobalRandom> globalRandom;

		[NativeDisableParallelForRestriction]
		public ComponentLookup<PhysicsCollider> colliderLookup;

		[NativeDisableParallelForRestriction]
		public ComponentLookup<LocalTransform> transformLookup;

		[NativeDisableParallelForRestriction]
		public ComponentLookup<PostTransformMatrix> matrixLookup;

		private InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

		public unsafe void Execute([ChunkIndexInQuery] int index, ref Liquid_Dots lquid, ref Venom_Dots venom, Entity entity)
		{
			if (!venom.durationFinish)
			{
				venom.durationTimer += deltaTime;
				if (venom.durationTimer > venom.duration)
				{
					venom.durationFinish = true;
				}
			}
			else
			{
				venom.radius -= deltaTime;
				if (venom.radius > 0f)
				{
					if (venom.isRectangle)
					{
						matrixLookup.GetRefRW(lquid.imageEntity).ValueRW.Value = float4x4.TRS(Vector3.zero, quaternion.LookRotation(Vector3.forward, venom.rectangleDir), new float3(venom.radius * 2f, venom.rectangleDistance, 1f));
						Unity.Physics.BoxCollider* colliderPtr = (Unity.Physics.BoxCollider*)colliderLookup.GetRefRW(lquid.colliderEntity).ValueRW.ColliderPtr;
						BoxGeometry geometry = colliderPtr->Geometry;
						geometry.Size = new float3(venom.radius * 2f, venom.rectangleDistance, 1f);
						colliderPtr->Geometry = geometry;
					}
					else
					{
						transformLookup.GetRefRW(lquid.colliderEntity).ValueRW.Scale = venom.radius * 2f;
						transformLookup.GetRefRW(lquid.imageEntity).ValueRW.Scale = venom.radius * 2f;
					}
				}
				else
				{
					ecb.DestroyEntity(index, entity);
				}
			}
			if (venom.isRectangle)
			{
				venom.createBubbleSpeed = venom.radius * 2f * venom.rectangleDistance * 15f * 0.8f;
			}
			else
			{
				venom.createBubbleSpeed = venom.radius * venom.radius * 4f * 15f * 0.8f * 0.8f;
			}
			venom.createBubbleTimer += deltaTime * venom.createBubbleSpeed;
			GlobalRandom valueRW = globalRandom.ValueRW;
			valueRW.NextFloatByChunkIndex(index * 100000);
			while (venom.createBubbleTimer > 1f)
			{
				venom.createBubbleTimer -= 1f;
				valueRW.NewRandom();
				float3 @float = transformLookup[entity].Position;
				if (venom.isRectangle)
				{
					float3 float2 = new float3(0f - venom.rectangleDir.y, venom.rectangleDir.x, 0f);
					float3 point = DTool.Random(ref valueRW.random, -1f, 1f) * float2 * venom.radius * 0.8f + DTool.Random(ref valueRW.random, -0.5f, 0.5f) * venom.rectangleDir * venom.rectangleDistance + @float;
					point.z = 1.14f;
					ecb.AppendToBuffer(index, bubbleBufferEntity, new CreateVenomBubble
					{
						point = point
					});
					continue;
				}
				float3 point2 = new float3(DTool.Random(ref valueRW.random, -1f, 1f), DTool.Random(ref valueRW.random, -1f, 1f), 0f) * venom.radius * 0.8f + @float;
				if (DTool.IgnoreZDistanceSqr(in point2, in @float) < venom.radius * venom.radius * 0.8f * 0.8f)
				{
					point2.z = 1.14f;
					ecb.AppendToBuffer(index, bubbleBufferEntity, new CreateVenomBubble
					{
						point = point2
					});
				}
			}
		}

		[CompilerGenerated]
		public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
		{
			IntPtr nativeArrayPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Liquid_Dots_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr2 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Venom_Dots_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr3 = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(chunk, __TypeHandle.__Unity_Entities_Entity_TypeHandle);
			int count = chunk.Count;
			int num = 0;
			if (!useEnabledMask)
			{
				for (int i = 0; i < count; i++)
				{
					ref Liquid_Dots lquid = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Liquid_Dots>(nativeArrayPtr, i);
					ref Venom_Dots venom = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Venom_Dots>(nativeArrayPtr2, i);
					Entity entity = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr3, i);
					Execute(chunkIndexInQuery, ref lquid, ref venom, entity);
					num++;
				}
				return;
			}
			if (math.countbits(chunkEnabledMask.ULong0 ^ (chunkEnabledMask.ULong0 << 1)) + math.countbits(chunkEnabledMask.ULong1 ^ (chunkEnabledMask.ULong1 << 1)) - 1 <= 4)
			{
				int nextRangeBegin = 0;
				int nextRangeEnd = 0;
				while (InternalCompilerInterface.UnsafeTryGetNextEnabledBitRange(chunkEnabledMask, nextRangeEnd, out nextRangeBegin, out nextRangeEnd))
				{
					while (nextRangeBegin < nextRangeEnd)
					{
						ref Liquid_Dots lquid2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Liquid_Dots>(nativeArrayPtr, nextRangeBegin);
						ref Venom_Dots venom2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Venom_Dots>(nativeArrayPtr2, nextRangeBegin);
						Entity entity2 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr3, nextRangeBegin);
						Execute(chunkIndexInQuery, ref lquid2, ref venom2, entity2);
						nextRangeBegin++;
						num++;
					}
				}
				return;
			}
			ulong num2 = chunkEnabledMask.ULong0;
			int num3 = math.min(64, count);
			for (int j = 0; j < num3; j++)
			{
				if ((num2 & 1) != 0L)
				{
					ref Liquid_Dots lquid3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Liquid_Dots>(nativeArrayPtr, j);
					ref Venom_Dots venom3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Venom_Dots>(nativeArrayPtr2, j);
					Entity entity3 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr3, j);
					Execute(chunkIndexInQuery, ref lquid3, ref venom3, entity3);
					num++;
				}
				num2 >>= 1;
			}
			num2 = chunkEnabledMask.ULong1;
			for (int k = 64; k < count; k++)
			{
				if ((num2 & 1) != 0L)
				{
					ref Liquid_Dots lquid4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Liquid_Dots>(nativeArrayPtr, k);
					ref Venom_Dots venom4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Venom_Dots>(nativeArrayPtr2, k);
					Entity entity4 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr3, k);
					Execute(chunkIndexInQuery, ref lquid4, ref venom4, entity4);
					num++;
				}
				num2 >>= 1;
			}
		}

		private JobHandle __ThrowCodeGenException()
		{
			throw new Exception("This method should have been replaced by source gen.");
		}

		public void Run()
		{
			__ThrowCodeGenException();
		}

		public void RunByRef()
		{
			__ThrowCodeGenException();
		}

		public void Run(EntityQuery query)
		{
			__ThrowCodeGenException();
		}

		public void RunByRef(EntityQuery query)
		{
			__ThrowCodeGenException();
		}

		public JobHandle Schedule(JobHandle dependsOn)
		{
			return __ThrowCodeGenException();
		}

		public JobHandle ScheduleByRef(JobHandle dependsOn)
		{
			return __ThrowCodeGenException();
		}

		public JobHandle Schedule(EntityQuery query, JobHandle dependsOn)
		{
			return __ThrowCodeGenException();
		}

		public JobHandle ScheduleByRef(EntityQuery query, JobHandle dependsOn)
		{
			return __ThrowCodeGenException();
		}

		public void Schedule()
		{
			__ThrowCodeGenException();
		}

		public void ScheduleByRef()
		{
			__ThrowCodeGenException();
		}

		public void Schedule(EntityQuery query)
		{
			__ThrowCodeGenException();
		}

		public void ScheduleByRef(EntityQuery query)
		{
			__ThrowCodeGenException();
		}

		public JobHandle ScheduleParallel(JobHandle dependsOn)
		{
			return __ThrowCodeGenException();
		}

		public JobHandle ScheduleParallelByRef(JobHandle dependsOn)
		{
			return __ThrowCodeGenException();
		}

		public JobHandle ScheduleParallel(EntityQuery query, JobHandle dependsOn)
		{
			return __ThrowCodeGenException();
		}

		public JobHandle ScheduleParallelByRef(EntityQuery query, JobHandle dependsOn)
		{
			return __ThrowCodeGenException();
		}

		public JobHandle ScheduleParallel(EntityQuery query, JobHandle dependsOn, NativeArray<int> chunkBaseEntityIndices)
		{
			return __ThrowCodeGenException();
		}

		public JobHandle ScheduleParallelByRef(EntityQuery query, JobHandle dependsOn, NativeArray<int> chunkBaseEntityIndices)
		{
			return __ThrowCodeGenException();
		}

		public void ScheduleParallel()
		{
			__ThrowCodeGenException();
		}

		public void ScheduleParallelByRef()
		{
			__ThrowCodeGenException();
		}

		public void ScheduleParallel(EntityQuery query)
		{
			__ThrowCodeGenException();
		}

		public void ScheduleParallelByRef(EntityQuery query)
		{
			__ThrowCodeGenException();
		}

		void IJobChunk.Execute(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask)
		{
			Execute(in chunk, unfilteredChunkIndex, useEnabledMask, in chunkEnabledMask);
		}
	}

	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_1658030752_0
	{
		public struct ResolvedChunk
		{
			public IntPtr item1_IntPtr;

			public IntPtr item2_IntPtr;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public (InternalCompilerInterface.UncheckedRefRW<Venom_Dots>, InternalCompilerInterface.UncheckedRefRW<Venom_DotsParticle>) Get(int index)
			{
				return (InternalCompilerInterface.UnsafeGetUncheckedRefRW<Venom_Dots>(item1_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<Venom_DotsParticle>(item2_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			private ComponentTypeHandle<Venom_Dots> item1_ComponentTypeHandle_RW;

			private ComponentTypeHandle<Venom_DotsParticle> item2_ComponentTypeHandle_RW;

			public TypeHandle(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<Venom_Dots>();
				item2_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<Venom_DotsParticle>();
			}

			public void Update(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW.Update(ref systemState);
				item2_ComponentTypeHandle_RW.Update(ref systemState);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ResolvedChunk Resolve(ArchetypeChunk archetypeChunk)
			{
				ResolvedChunk result = default(ResolvedChunk);
				result.item1_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item1_ComponentTypeHandle_RW);
				result.item2_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item2_ComponentTypeHandle_RW);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<(InternalCompilerInterface.UncheckedRefRW<Venom_Dots>, InternalCompilerInterface.UncheckedRefRW<Venom_DotsParticle>)>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public (InternalCompilerInterface.UncheckedRefRW<Venom_Dots>, InternalCompilerInterface.UncheckedRefRW<Venom_DotsParticle>) Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRW<Venom_Dots>();
			state.EntityManager.CompleteDependencyBeforeRW<Venom_DotsParticle>();
		}
	}

	private struct TypeHandle
	{
		public IFE_1658030752_0.TypeHandle __IFE_1658030752_0_TypeHandle;

		public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentLookup;

		public ComponentLookup<PhysicsCollider> __Unity_Physics_PhysicsCollider_RW_ComponentLookup;

		public ComponentLookup<PostTransformMatrix> __Unity_Transforms_PostTransformMatrix_RW_ComponentLookup;

		public VenomJob.InternalCompilerQueryAndHandleData __VenomSystem_VenomJob_WithDefaultQuery_JobEntityTypeHandle;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__IFE_1658030752_0_TypeHandle = new IFE_1658030752_0.TypeHandle(ref state);
			__Unity_Transforms_LocalTransform_RW_ComponentLookup = state.GetComponentLookup<LocalTransform>();
			__Unity_Physics_PhysicsCollider_RW_ComponentLookup = state.GetComponentLookup<PhysicsCollider>();
			__Unity_Transforms_PostTransformMatrix_RW_ComponentLookup = state.GetComponentLookup<PostTransformMatrix>();
			__VenomSystem_VenomJob_WithDefaultQuery_JobEntityTypeHandle.Init(ref state, assignDefaultQuery: true);
		}
	}

	public static List<CreateVenomRequest> requests = new List<CreateVenomRequest>();

	public static VenomSystem Inst;

	public EntityQuery venomQuery;

	private TypeHandle __TypeHandle;

	private EntityQuery __query_1658030752_0;

	private EntityQuery __query_1658030752_1;

	private EntityQuery __query_1658030752_2;

	private EntityQuery __query_1658030752_3;

	private EntityQuery __query_1658030752_4;

	private EntityQuery __query_1658030752_5;

	private EntityQuery __query_1658030752_6;

	public static void CreateVenom(float3 point, float radius, float duration)
	{
		if (!GameMgr.IsMobile_Static)
		{
			requests.Add(new CreateVenomRequest(point, radius, duration));
		}
	}

	public static void CreateVenom(float3 point1, float3 point2, float radius, float duration)
	{
		if (!GameMgr.IsMobile_Static)
		{
			requests.Add(new CreateVenomRequest(point1, point2, radius, duration));
		}
	}

	public void Clear()
	{
		NativeArray<Entity> entities = venomQuery.ToEntityArray(Allocator.Temp);
		__query_1658030752_1.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(base.World.Unmanaged).DestroyEntity(entities);
		requests.Clear();
		DynamicBuffer<CreateVenomRequest> singletonBuffer = __query_1658030752_2.GetSingletonBuffer<CreateVenomRequest>();
		if (singletonBuffer.Length > 0)
		{
			singletonBuffer.Clear();
		}
	}

	[Preserve]
	protected override void OnCreate()
	{
		base.EntityManager.CreateSingletonBuffer<CreateVenomRequest>();
		base.EntityManager.CreateSingletonBuffer<CreateVenomBubble>();
		RequireForUpdate<AllMixedEtt>();
		Inst = this;
		venomQuery = base.EntityManager.CreateEntityQuery(typeof(Venom_Dots));
		EventMgr.DotsSystemReset = (Action)Delegate.Combine(EventMgr.DotsSystemReset, (Action)delegate
		{
			NativeArray<Entity> entities = venomQuery.ToEntityArray(Allocator.Temp);
			__query_1658030752_1.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(base.World.Unmanaged).DestroyEntity(entities);
			requests.Clear();
			DynamicBuffer<CreateVenomRequest> singletonBuffer = __query_1658030752_2.GetSingletonBuffer<CreateVenomRequest>();
			if (singletonBuffer.Length > 0)
			{
				singletonBuffer.Clear();
			}
		});
	}

	[Preserve]
	protected override void OnDestroy()
	{
		EventMgr.DotsSystemReset = (Action)Delegate.Remove(EventMgr.DotsSystemReset, (Action)delegate
		{
			NativeArray<Entity> entities = venomQuery.ToEntityArray(Allocator.Temp);
			__query_1658030752_1.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(base.World.Unmanaged).DestroyEntity(entities);
			requests.Clear();
			DynamicBuffer<CreateVenomRequest> singletonBuffer = __query_1658030752_2.GetSingletonBuffer<CreateVenomRequest>();
			if (singletonBuffer.Length > 0)
			{
				singletonBuffer.Clear();
			}
		});
	}

	[Preserve]
	protected unsafe override void OnUpdate()
	{
		if (!LevelMgr.Inst || !LevelMgr.Inst.CurrentRoomCtrller)
		{
			return;
		}
		DynamicBuffer<CreateVenomRequest> singletonBuffer = __query_1658030752_2.GetSingletonBuffer<CreateVenomRequest>();
		if (singletonBuffer.Length > 0)
		{
			for (int i = 0; i < singletonBuffer.Length; i++)
			{
				requests.Add(singletonBuffer[i]);
			}
			singletonBuffer.Clear();
		}
		if (GameMgr.IsMobile_Static)
		{
			requests.Clear();
			return;
		}
		if (requests.Count > 0)
		{
			AllMixedEtt singleton = __query_1658030752_3.GetSingleton<AllMixedEtt>();
			CollisionFilter collisionFilter = default(CollisionFilter);
			collisionFilter.BelongsTo = 128u;
			collisionFilter.CollidesWith = 1073741824u;
			collisionFilter.GroupIndex = 0;
			CollisionFilter collisionFilter2 = collisionFilter;
			for (int j = 0; j < requests.Count; j++)
			{
				CreateVenomRequest createVenomRequest = requests[j];
				RefRW<LocalTransform> componentRWAfterCompletingDependency;
				RefRW<PhysicsCollider> componentRWAfterCompletingDependency2;
				if (createVenomRequest.isCircle)
				{
					Entity entity = base.EntityManager.Instantiate(singleton.map["LiquidSphere"]);
					Venom_Dots venom_Dots = default(Venom_Dots);
					venom_Dots.duration = createVenomRequest.duration;
					venom_Dots.isRectangle = false;
					venom_Dots.radius = createVenomRequest.radius;
					Venom_Dots componentData = venom_Dots;
					base.EntityManager.AddComponentData(entity, componentData);
					Liquid_Dots componentData2 = base.EntityManager.GetComponentData<Liquid_Dots>(entity);
					componentRWAfterCompletingDependency = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref base.CheckedStateRef, entity);
					componentRWAfterCompletingDependency.ValueRW.Position = createVenomRequest.point;
					componentRWAfterCompletingDependency = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref base.CheckedStateRef, componentData2.colliderEntity);
					ref LocalTransform valueRW = ref componentRWAfterCompletingDependency.ValueRW;
					valueRW.Position = Vector3.zero;
					valueRW.Scale = createVenomRequest.radius / 0.5f;
					componentRWAfterCompletingDependency2 = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Physics_PhysicsCollider_RW_ComponentLookup, ref base.CheckedStateRef, componentData2.colliderEntity);
					ref PhysicsCollider valueRW2 = ref componentRWAfterCompletingDependency2.ValueRW;
					valueRW2.MakeUnique(in componentData2.colliderEntity, base.EntityManager);
					valueRW2.ColliderPtr->SetCollisionFilter(collisionFilter2);
					componentRWAfterCompletingDependency = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref base.CheckedStateRef, componentData2.imageEntity);
					ref LocalTransform valueRW3 = ref componentRWAfterCompletingDependency.ValueRW;
					valueRW3.Position = Tool2D.IgnoreZPoint(Vector3.zero, -130f);
					valueRW3.Scale = createVenomRequest.radius / 0.5f;
					base.EntityManager.AddComponent<VenomTag>(componentData2.colliderEntity);
				}
				else
				{
					Entity entity = base.EntityManager.Instantiate(singleton.map["LiquidRectangle"]);
					Venom_Dots venom_Dots = default(Venom_Dots);
					venom_Dots.radius = createVenomRequest.radius;
					venom_Dots.duration = createVenomRequest.duration;
					venom_Dots.isRectangle = true;
					venom_Dots.rectangleDir = Tool2D.IgnoreZV2ToV1Normal(createVenomRequest.point2, createVenomRequest.point1);
					venom_Dots.rectangleDistance = Tool2D.IgnoreZDistance(createVenomRequest.point1, createVenomRequest.point2) * 1.1f;
					venom_Dots.createBubbleTimer = 1f;
					Venom_Dots componentData3 = venom_Dots;
					base.EntityManager.AddComponentData(entity, componentData3);
					Liquid_Dots componentData4 = base.EntityManager.GetComponentData<Liquid_Dots>(entity);
					componentRWAfterCompletingDependency = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref base.CheckedStateRef, entity);
					componentRWAfterCompletingDependency.ValueRW.Position = createVenomRequest.point;
					componentRWAfterCompletingDependency = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref base.CheckedStateRef, componentData4.colliderEntity);
					componentRWAfterCompletingDependency.ValueRW.Position = Vector3.zero;
					componentRWAfterCompletingDependency2 = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Physics_PhysicsCollider_RW_ComponentLookup, ref base.CheckedStateRef, componentData4.colliderEntity);
					ref PhysicsCollider valueRW4 = ref componentRWAfterCompletingDependency2.ValueRW;
					valueRW4.MakeUnique(in componentData4.colliderEntity, base.EntityManager);
					valueRW4.ColliderPtr->SetCollisionFilter(collisionFilter2);
					Unity.Physics.BoxCollider* colliderPtr = (Unity.Physics.BoxCollider*)valueRW4.ColliderPtr;
					BoxGeometry geometry = colliderPtr->Geometry;
					geometry.Size = new float3(createVenomRequest.radius * 2f, componentData3.rectangleDistance, 1f);
					colliderPtr->Geometry = geometry;
					InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_PostTransformMatrix_RW_ComponentLookup, ref base.CheckedStateRef, componentData4.colliderEntity).ValueRW.Value = float4x4.TRS(Vector3.zero, quaternion.LookRotation(Vector3.forward, componentData3.rectangleDir), new float3(1f, 1f, 1f));
					componentRWAfterCompletingDependency = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref base.CheckedStateRef, componentData4.imageEntity);
					componentRWAfterCompletingDependency.ValueRW.Position = Tool2D.IgnoreZPoint(Vector3.zero, -130f);
					InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_PostTransformMatrix_RW_ComponentLookup, ref base.CheckedStateRef, componentData4.imageEntity).ValueRW.Value = float4x4.TRS(Vector3.zero, quaternion.LookRotation(Vector3.forward, componentData3.rectangleDir), new float3(createVenomRequest.radius * 2f, componentData3.rectangleDistance, 1f));
					base.EntityManager.AddComponent<VenomTag>(componentData4.colliderEntity);
				}
			}
			requests.Clear();
		}
		if (venomQuery.IsEmpty)
		{
			return;
		}
		foreach (var item3 in IFE_1658030752_0.Query(__query_1658030752_0, __TypeHandle.__IFE_1658030752_0_TypeHandle, ref base.CheckedStateRef))
		{
			InternalCompilerInterface.UncheckedRefRW<Venom_Dots> item = item3.Item1;
			InternalCompilerInterface.UncheckedRefRW<Venom_DotsParticle> item2 = item3.Item2;
			ref Venom_Dots valueRW5 = ref item.ValueRW;
			ref Venom_DotsParticle valueRW6 = ref item2.ValueRW;
			if (valueRW5.durationFinish)
			{
				if (valueRW5.isRectangle)
				{
					valueRW6.particle.Value.UpdateBubbleCircle(valueRW5.radius);
				}
				else
				{
					valueRW6.particle.Value.UpdateBubbleRectangle(valueRW5.radius, valueRW5.rectangleDistance);
				}
			}
		}
		EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer(Allocator.TempJob);
		RefRW<GlobalRandom> singletonRW = __query_1658030752_4.GetSingletonRW<GlobalRandom>();
		singletonRW.ValueRW.NewRandom();
		base.Dependency = __ScheduleViaJobChunkExtension_0(new VenomJob
		{
			bubbleBufferEntity = __query_1658030752_5.GetSingletonEntity(),
			globalRandom = singletonRW,
			matrixLookup = GetComponentLookup<PostTransformMatrix>(),
			colliderLookup = GetComponentLookup<PhysicsCollider>(),
			transformLookup = GetComponentLookup<LocalTransform>(),
			deltaTime = base.CheckedStateRef.WorldUnmanaged.Time.DeltaTime,
			ecb = entityCommandBuffer.AsParallelWriter()
		}, __TypeHandle.__VenomSystem_VenomJob_WithDefaultQuery_JobEntityTypeHandle.DefaultQuery, base.Dependency, ref base.CheckedStateRef, hasUserDefinedQuery: false);
		base.Dependency.Complete();
		entityCommandBuffer.Playback(base.EntityManager);
		entityCommandBuffer.Dispose();
		DynamicBuffer<CreateVenomBubble> singletonBuffer2 = __query_1658030752_6.GetSingletonBuffer<CreateVenomBubble>();
		ParticleSystem globalVenomParticle = LevelMgr.Inst.CurrentRoomCtrller.venomCtrller.globalVenomParticle;
		for (int k = 0; k < singletonBuffer2.Length; k++)
		{
			ParticleSystem.EmitParams emitParams = default(ParticleSystem.EmitParams);
			emitParams.position = singletonBuffer2[k].point;
			globalVenomParticle.Emit(emitParams, 1);
		}
		singletonBuffer2.Clear();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private JobHandle __ScheduleViaJobChunkExtension_0(VenomJob job, EntityQuery query, JobHandle dependency, ref SystemState state, bool hasUserDefinedQuery)
	{
		dependency = __TypeHandle.__VenomSystem_VenomJob_WithDefaultQuery_JobEntityTypeHandle.UpdateBaseEntityIndexArray(ref job, query, dependency, ref state);
		__TypeHandle.__VenomSystem_VenomJob_WithDefaultQuery_JobEntityTypeHandle.AssignEntityManager(ref job, state.EntityManager);
		__TypeHandle.__VenomSystem_VenomJob_WithDefaultQuery_JobEntityTypeHandle.__TypeHandle.Update(ref state);
		return __TypeHandle.__VenomSystem_VenomJob_WithDefaultQuery_JobEntityTypeHandle.ScheduleParallel(ref job, query, dependency);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAllRW<Venom_Dots>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Venom_DotsParticle>();
		__query_1658030752_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<EndSimulationEntityCommandBufferSystem.Singleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1658030752_1 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAllRW<CreateVenomRequest>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1658030752_2 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<AllMixedEtt>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1658030752_3 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAllRW<GlobalRandom>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1658030752_4 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<CreateVenomBubble>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1658030752_5 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAllRW<CreateVenomBubble>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1658030752_6 = entityQueryBuilder2.Build(ref state);
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
	public VenomSystem()
	{
	}
}
