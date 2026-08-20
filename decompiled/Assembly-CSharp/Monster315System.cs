using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Internal;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
[UpdateInGroup(typeof(UnitBaseSystemGroup))]
[CompilerGenerated]
public struct Monster315System : ISystem, ISystemCompilerGenerated
{
	public struct Monster315CreateShieldRequest : IBufferElementData
	{
		public Entity ownerEntity;

		public float3 pos;
	}

	public struct Monster315DeadRequest : IBufferElementData
	{
		public Entity deadEntity;
	}

	public struct Monster315EntityInRange : IBufferElementData
	{
		public Entity inRangeEntity;
	}

	[CompilerGenerated]
	public struct Monster315Job : IJobEntity, IJobChunk
	{
		public struct InternalCompilerQueryAndHandleData
		{
			public struct TypeHandle
			{
				public ComponentTypeHandle<Monster315_Dots> __Monster315_Dots_RW_ComponentTypeHandle;

				public ComponentTypeHandle<UnitBase_Dots> __UnitBase_Dots_RW_ComponentTypeHandle;

				public ComponentTypeHandle<PathFinding> __PathFinding_RW_ComponentTypeHandle;

				[ReadOnly]
				public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public void __AssignHandles(ref SystemState state)
				{
					__Monster315_Dots_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Monster315_Dots>();
					__UnitBase_Dots_RW_ComponentTypeHandle = state.GetComponentTypeHandle<UnitBase_Dots>();
					__PathFinding_RW_ComponentTypeHandle = state.GetComponentTypeHandle<PathFinding>();
					__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
				}

				public void Update(ref SystemState state)
				{
					__Monster315_Dots_RW_ComponentTypeHandle.Update(ref state);
					__UnitBase_Dots_RW_ComponentTypeHandle.Update(ref state);
					__PathFinding_RW_ComponentTypeHandle.Update(ref state);
					__Unity_Entities_Entity_TypeHandle.Update(ref state);
				}
			}

			public TypeHandle __TypeHandle;

			public EntityQuery DefaultQuery;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private void __AssignQueries(ref SystemState state)
			{
				EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
				EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAllRW<Monster315_Dots>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<UnitBase_Dots>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<PathFinding>();
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
			public void Run(ref Monster315Job job, EntityQuery query)
			{
				job.__TypeHandle = __TypeHandle;
				JobChunkExtensions.RunByRef(ref job, query);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle Schedule(ref Monster315Job job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = __TypeHandle;
				return JobChunkExtensions.ScheduleByRef(ref job, query, dependency);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle ScheduleParallel(ref Monster315Job job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = __TypeHandle;
				return JobChunkExtensions.ScheduleParallelByRef(ref job, query, dependency);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void UpdateBaseEntityIndexArray(ref Monster315Job job, EntityQuery query, ref SystemState state)
			{
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle UpdateBaseEntityIndexArray(ref Monster315Job job, EntityQuery query, JobHandle dependency, ref SystemState state)
			{
				return dependency;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void AssignEntityManager(ref Monster315Job job, EntityManager entityManager)
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

		public EntityCommandBuffer.ParallelWriter ecb;

		public CurrentRoomEntitiesSingleton CurrentRoomEntities;

		[NativeDisableParallelForRestriction]
		public ComponentLookup<LocalTransform> LocalTransformLookUp;

		[NativeDisableParallelForRestriction]
		public ComponentLookup<AnimaPlay> AnimaLookUp;

		[NativeDisableParallelForRestriction]
		public ComponentLookup<UnitProperty_Dots> pptLookUp;

		[NativeDisableParallelForRestriction]
		public ComponentLookup<Monster315Shield_Dots> ShieldLookUp;

		[NativeDisableParallelForRestriction]
		public ComponentLookup<PhysicsCollider> physicsColliderLookUp;

		[ReadOnly]
		public NativeList<Entity> canFollowEntity;

		[ReadOnly]
		public NativeList<float3> canFollowEntityPos;

		public Entity CreateShieldBufferEntity;

		public Entity DeadEntity;

		public Entity InRangeEntity;

		public float3 playerPoint;

		private InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

		public unsafe void Execute([ChunkIndexInQuery] int index, ref Monster315_Dots monster, ref UnitBase_Dots unitBase, ref PathFinding pathFinding, Entity entity)
		{
			ref UnitProperty_Dots valueRW = ref pptLookUp.GetRefRW(entity).ValueRW;
			LocalTransform localTransform = LocalTransformLookUp[entity];
			ref AnimaPlay valueRW2 = ref AnimaLookUp.GetRefRW(unitBase.ett_AnimaRoot).ValueRW;
			if (!monster.Initialized)
			{
				monster.Initialized = true;
				ecb.AppendToBuffer(index, CreateShieldBufferEntity, new Monster315CreateShieldRequest
				{
					ownerEntity = entity,
					pos = localTransform.Position
				});
				valueRW2.Play(0);
			}
			valueRW2.SetLockMotion(valueRW.LockMotion);
			if (valueRW.LockMotion)
			{
				return;
			}
			if (Tool2D.IgnoreZDistanceSqr(localTransform.Position, playerPoint) < 12f)
			{
				monster.playerNear = true;
			}
			else
			{
				monster.playerNear = false;
			}
			if (!monster.playerNear)
			{
				for (int i = 0; i < CurrentRoomEntities.TargetableEntities.Length; i++)
				{
					int id = CurrentRoomEntities.TargetableUnitProperties[i].id;
					Vector3 v = CurrentRoomEntities.TargetableTransforms[i].Position;
					if (id != 131502 && Tool2D.IgnoreZDistanceSqr(localTransform.Position, v) < 9f)
					{
						ecb.AppendToBuffer(index, InRangeEntity, new Monster315EntityInRange
						{
							inRangeEntity = CurrentRoomEntities.TargetableEntities[i]
						});
					}
				}
			}
			if (!LocalTransformLookUp.HasComponent(monster.followEntity))
			{
				if (FindNearestFollowEntity(in canFollowEntity, in canFollowEntityPos, localTransform.Position, out var followEntity))
				{
					monster.followEntity = followEntity;
				}
				else
				{
					monster.followEntity = Entity.Null;
				}
			}
			else
			{
				pathFinding.UpdatePath(localTransform.Position, LocalTransformLookUp[monster.followEntity].Position, 32);
				unitBase.SetMove(valueRW.MoveSpeed * math.normalizesafe(pathFinding.walkToPoint - localTransform.Position));
			}
			if (!(monster.shieldEntity != Entity.Null))
			{
				return;
			}
			if (LocalTransformLookUp.HasComponent(monster.shieldEntity))
			{
				LocalTransformLookUp.GetRefRW(monster.shieldEntity).ValueRW.Position = localTransform.Position;
				ref Monster315Shield_Dots valueRW3 = ref ShieldLookUp.GetRefRW(monster.shieldEntity).ValueRW;
				if (valueRW3.shieldInactive != monster.playerNear)
				{
					valueRW3.shieldInactive = monster.playerNear;
					ecb.SetComponentEnabled<MaterialMeshInfo>(index, valueRW3.ShieldOn, !monster.playerNear);
					ecb.SetComponentEnabled<MaterialMeshInfo>(index, valueRW3.ShieldOn1, !monster.playerNear);
					physicsColliderLookUp.GetRefRW(monster.shieldEntity).ValueRW.ColliderPtr->SetCollisionResponse(valueRW3.shieldInactive ? CollisionResponsePolicy.None : CollisionResponsePolicy.RaiseTriggerEvents);
				}
			}
			else
			{
				ecb.AppendToBuffer(index, DeadEntity, new Monster315DeadRequest
				{
					deadEntity = entity
				});
			}
		}

		public bool FindNearestFollowEntity(in NativeList<Entity> canFollowEntity, in NativeList<float3> canFollowEntityPos, float3 checkPoint, out Entity followEntity)
		{
			followEntity = Entity.Null;
			int num = -1;
			float num2 = float.MaxValue;
			for (int i = 0; i < canFollowEntity.Length; i++)
			{
				float num3 = math.distancesq(canFollowEntityPos[i], checkPoint);
				if (!(num3 > num2))
				{
					num2 = num3;
					num = i;
				}
			}
			if (num < 0)
			{
				return false;
			}
			followEntity = canFollowEntity[num];
			return true;
		}

		[CompilerGenerated]
		public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
		{
			IntPtr nativeArrayPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Monster315_Dots_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr2 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__UnitBase_Dots_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr3 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__PathFinding_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr4 = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(chunk, __TypeHandle.__Unity_Entities_Entity_TypeHandle);
			int count = chunk.Count;
			int num = 0;
			if (!useEnabledMask)
			{
				for (int i = 0; i < count; i++)
				{
					ref Monster315_Dots monster = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Monster315_Dots>(nativeArrayPtr, i);
					ref UnitBase_Dots unitBase = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitBase_Dots>(nativeArrayPtr2, i);
					ref PathFinding pathFinding = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PathFinding>(nativeArrayPtr3, i);
					Entity entity = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr4, i);
					Execute(chunkIndexInQuery, ref monster, ref unitBase, ref pathFinding, entity);
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
						ref Monster315_Dots monster2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Monster315_Dots>(nativeArrayPtr, nextRangeBegin);
						ref UnitBase_Dots unitBase2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitBase_Dots>(nativeArrayPtr2, nextRangeBegin);
						ref PathFinding pathFinding2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PathFinding>(nativeArrayPtr3, nextRangeBegin);
						Entity entity2 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr4, nextRangeBegin);
						Execute(chunkIndexInQuery, ref monster2, ref unitBase2, ref pathFinding2, entity2);
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
					ref Monster315_Dots monster3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Monster315_Dots>(nativeArrayPtr, j);
					ref UnitBase_Dots unitBase3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitBase_Dots>(nativeArrayPtr2, j);
					ref PathFinding pathFinding3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PathFinding>(nativeArrayPtr3, j);
					Entity entity3 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr4, j);
					Execute(chunkIndexInQuery, ref monster3, ref unitBase3, ref pathFinding3, entity3);
					num++;
				}
				num2 >>= 1;
			}
			num2 = chunkEnabledMask.ULong1;
			for (int k = 64; k < count; k++)
			{
				if ((num2 & 1) != 0L)
				{
					ref Monster315_Dots monster4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Monster315_Dots>(nativeArrayPtr, k);
					ref UnitBase_Dots unitBase4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitBase_Dots>(nativeArrayPtr2, k);
					ref PathFinding pathFinding4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PathFinding>(nativeArrayPtr3, k);
					Entity entity4 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr4, k);
					Execute(chunkIndexInQuery, ref monster4, ref unitBase4, ref pathFinding4, entity4);
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

	private struct TypeHandle
	{
		public ComponentLookup<UnitProperty_Dots> __UnitProperty_Dots_RW_ComponentLookup;

		public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<UnitProperty_Dots> __UnitProperty_Dots_RO_ComponentLookup;

		public ComponentLookup<AnimaPlay> __AnimaPlay_RW_ComponentLookup;

		public ComponentLookup<Monster315Shield_Dots> __Monster315Shield_Dots_RW_ComponentLookup;

		public ComponentLookup<PhysicsCollider> __Unity_Physics_PhysicsCollider_RW_ComponentLookup;

		public Monster315Job.InternalCompilerQueryAndHandleData __Monster315System_Monster315Job_WithDefaultQuery_JobEntityTypeHandle;

		[ReadOnly]
		public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RO_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<Monster315ShieldEffect> __Monster315ShieldEffect_RO_ComponentLookup;

		public ComponentLookup<Monster315ShieldEffect> __Monster315ShieldEffect_RW_ComponentLookup;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__UnitProperty_Dots_RW_ComponentLookup = state.GetComponentLookup<UnitProperty_Dots>();
			__Unity_Transforms_LocalTransform_RW_ComponentLookup = state.GetComponentLookup<LocalTransform>();
			__UnitProperty_Dots_RO_ComponentLookup = state.GetComponentLookup<UnitProperty_Dots>(isReadOnly: true);
			__AnimaPlay_RW_ComponentLookup = state.GetComponentLookup<AnimaPlay>();
			__Monster315Shield_Dots_RW_ComponentLookup = state.GetComponentLookup<Monster315Shield_Dots>();
			__Unity_Physics_PhysicsCollider_RW_ComponentLookup = state.GetComponentLookup<PhysicsCollider>();
			__Monster315System_Monster315Job_WithDefaultQuery_JobEntityTypeHandle.Init(ref state, assignDefaultQuery: true);
			__Unity_Transforms_LocalTransform_RO_ComponentLookup = state.GetComponentLookup<LocalTransform>(isReadOnly: true);
			__Monster315ShieldEffect_RO_ComponentLookup = state.GetComponentLookup<Monster315ShieldEffect>(isReadOnly: true);
			__Monster315ShieldEffect_RW_ComponentLookup = state.GetComponentLookup<Monster315ShieldEffect>();
		}
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void __codegen__OnDestroy_00008BBC_0024PostfixBurstDelegate(IntPtr self, IntPtr state);

	internal static class __codegen__OnDestroy_00008BBC_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<__codegen__OnDestroy_00008BBC_0024PostfixBurstDelegate>(delegate(IntPtr self, IntPtr state)
				{
					Invoke(self, state);
				}).Value;
			}
			P_0 = Pointer;
		}

		private static IntPtr GetFunctionPointer()
		{
			nint result = 0;
			GetFunctionPointerDiscard(ref result);
			return result;
		}

		public unsafe static void Invoke(IntPtr self, IntPtr state)
		{
			if (BurstCompiler.IsEnabled)
			{
				IntPtr functionPointer = GetFunctionPointer();
				if (functionPointer != (IntPtr)0)
				{
					((delegate* unmanaged[Cdecl]<IntPtr, IntPtr, void>)functionPointer)(self, state);
					return;
				}
			}
			__codegen__OnDestroy_0024BurstManaged(self, state);
		}
	}

	private NativeList<Entity> protectedEntities;

	private EntityQuery monsterQuery;

	private EntityQuery effectQuery;

	private TypeHandle __TypeHandle;

	private EntityQuery __query_1191368475_0;

	private EntityQuery __query_1191368475_1;

	private EntityQuery __query_1191368475_2;

	private EntityQuery __query_1191368475_3;

	private EntityQuery __query_1191368475_4;

	private EntityQuery __query_1191368475_5;

	private EntityQuery __query_1191368475_6;

	public void OnCreate(ref SystemState state)
	{
		state.EntityManager.CreateSingletonBuffer<Monster315CreateShieldRequest>();
		state.EntityManager.CreateSingletonBuffer<Monster315DeadRequest>();
		state.EntityManager.CreateSingletonBuffer<Monster315EntityInRange>();
		protectedEntities = new NativeList<Entity>(Allocator.Persistent);
		monsterQuery = state.EntityManager.CreateEntityQuery(typeof(Monster315_Dots));
		effectQuery = state.EntityManager.CreateEntityQuery(typeof(Monster315ShieldEffect));
	}

	[BurstCompile]
	public void OnDestroy(ref SystemState state)
	{
		protectedEntities.Dispose();
		monsterQuery.Dispose();
		effectQuery.Dispose();
	}

	public void OnUpdate(ref SystemState state)
	{
		state.EntityManager.CompleteDependencyBeforeRW<UnitProperty_Dots>();
		state.EntityManager.CompleteDependencyBeforeRW<LocalTransform>();
		ComponentLookup<UnitProperty_Dots> componentLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__UnitProperty_Dots_RW_ComponentLookup, ref state);
		ComponentLookup<LocalTransform> componentLookup2 = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state);
		__query_1191368475_0.GetSingleton<CurrentRoomEntitiesSingleton>();
		if (monsterQuery.ToEntityArray(Allocator.Temp).Length == 0)
		{
			if (protectedEntities.Length == 0)
			{
				return;
			}
			foreach (Entity protectedEntity in protectedEntities)
			{
				if (componentLookup2.HasComponent(protectedEntity))
				{
					UnitProperty_Dots componentAfterCompletingDependency = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__UnitProperty_Dots_RO_ComponentLookup, ref state, protectedEntity);
					componentAfterCompletingDependency.InvincibleUnregister();
					InternalCompilerInterface.SetComponentAfterCompletingDependency(ref __TypeHandle.__UnitProperty_Dots_RW_ComponentLookup, ref state, componentAfterCompletingDependency, protectedEntity);
				}
			}
			protectedEntities.Clear();
			NativeArray<Entity> nativeArray = effectQuery.ToEntityArray(Allocator.Temp);
			for (int i = 0; i < nativeArray.Length; i++)
			{
				state.EntityManager.DestroyEntity(nativeArray[i]);
			}
			nativeArray.Dispose();
			return;
		}
		NativeList<Entity> canFollowEntity = new NativeList<Entity>(Allocator.TempJob);
		NativeList<float3> canFollowEntityPos = new NativeList<float3>(Allocator.TempJob);
		foreach (Entity targetableEntity in __query_1191368475_0.GetSingleton<CurrentRoomEntitiesSingleton>().TargetableEntities)
		{
			Entity value = targetableEntity;
			UnitProperty_Dots componentData = state.EntityManager.GetComponentData<UnitProperty_Dots>(value);
			if (componentData.CanBeTarget && componentData.id != 131501 && componentData.id != 131502)
			{
				canFollowEntity.Add(in value);
				LocalTransform localTransform = componentLookup2[value];
				canFollowEntityPos.Add(in localTransform.Position);
			}
		}
		EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer(Allocator.TempJob);
		state.Dependency = __ScheduleViaJobChunkExtension_0(new Monster315Job
		{
			ecb = entityCommandBuffer.AsParallelWriter(),
			CurrentRoomEntities = __query_1191368475_0.GetSingleton<CurrentRoomEntitiesSingleton>(),
			LocalTransformLookUp = componentLookup2,
			AnimaLookUp = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__AnimaPlay_RW_ComponentLookup, ref state),
			pptLookUp = componentLookup,
			ShieldLookUp = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__Monster315Shield_Dots_RW_ComponentLookup, ref state),
			physicsColliderLookUp = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__Unity_Physics_PhysicsCollider_RW_ComponentLookup, ref state),
			canFollowEntity = canFollowEntity,
			canFollowEntityPos = canFollowEntityPos,
			CreateShieldBufferEntity = __query_1191368475_1.GetSingletonEntity(),
			DeadEntity = __query_1191368475_2.GetSingletonEntity(),
			InRangeEntity = __query_1191368475_3.GetSingletonEntity(),
			playerPoint = PlayerMgr.Inst.PlayerPoint
		}, __TypeHandle.__Monster315System_Monster315Job_WithDefaultQuery_JobEntityTypeHandle.DefaultQuery, state.Dependency, ref state, hasUserDefinedQuery: false);
		state.CompleteDependency();
		entityCommandBuffer.Playback(state.EntityManager);
		entityCommandBuffer.Dispose();
		canFollowEntity.Dispose();
		canFollowEntityPos.Dispose();
		NativeList<Entity> list = new NativeList<Entity>(Allocator.Temp);
		DynamicBuffer<Monster315EntityInRange> singletonBuffer = __query_1191368475_4.GetSingletonBuffer<Monster315EntityInRange>();
		NativeArray<Monster315EntityInRange> nativeArray2 = singletonBuffer.ToNativeArray(Allocator.Temp);
		singletonBuffer.Clear();
		foreach (Monster315EntityInRange item in nativeArray2)
		{
			Entity value2 = item.inRangeEntity;
			if (!list.Contains(value2))
			{
				list.Add(in value2);
				if (!protectedEntities.Contains(value2))
				{
					UnitProperty_Dots componentAfterCompletingDependency2 = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__UnitProperty_Dots_RO_ComponentLookup, ref state, value2);
					componentAfterCompletingDependency2.InvincibleRegister();
					InternalCompilerInterface.SetComponentAfterCompletingDependency(ref __TypeHandle.__UnitProperty_Dots_RW_ComponentLookup, ref state, componentAfterCompletingDependency2, value2);
					Entity entity = QuickCreateSystem.Inst.CreateMixedEtt("EF_EndlessMonsterShieldEffect", InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref state, value2).Position);
					Monster315ShieldEffect componentAfterCompletingDependency3 = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__Monster315ShieldEffect_RO_ComponentLookup, ref state, entity);
					componentAfterCompletingDependency3.followEntity = value2;
					InternalCompilerInterface.SetComponentAfterCompletingDependency(ref __TypeHandle.__Monster315ShieldEffect_RW_ComponentLookup, ref state, componentAfterCompletingDependency3, entity);
					LocalTransform componentAfterCompletingDependency4 = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref state, componentAfterCompletingDependency3.scaleRoot);
					componentAfterCompletingDependency4.Scale = componentAfterCompletingDependency2.size;
					InternalCompilerInterface.SetComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state, componentAfterCompletingDependency4, componentAfterCompletingDependency3.scaleRoot);
				}
			}
		}
		nativeArray2.Dispose();
		componentLookup2.Update(ref state);
		NativeArray<Entity> nativeArray3 = effectQuery.ToEntityArray(Allocator.Temp);
		NativeArray<Monster315ShieldEffect> nativeArray4 = effectQuery.ToComponentDataArray<Monster315ShieldEffect>(Allocator.Temp);
		NativeList<Entity> nativeList = new NativeList<Entity>(Allocator.Temp);
		foreach (Entity protectedEntity2 in protectedEntities)
		{
			if (list.Contains(protectedEntity2) || !componentLookup2.HasComponent(protectedEntity2))
			{
				continue;
			}
			UnitProperty_Dots componentAfterCompletingDependency5 = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__UnitProperty_Dots_RO_ComponentLookup, ref state, protectedEntity2);
			componentAfterCompletingDependency5.InvincibleUnregister();
			InternalCompilerInterface.SetComponentAfterCompletingDependency(ref __TypeHandle.__UnitProperty_Dots_RW_ComponentLookup, ref state, componentAfterCompletingDependency5, protectedEntity2);
			for (int j = 0; j < nativeArray3.Length; j++)
			{
				if (nativeArray4[j].followEntity == protectedEntity2)
				{
					Entity value3 = nativeArray3[j];
					nativeList.Add(in value3);
					break;
				}
			}
		}
		nativeArray3.Dispose();
		nativeArray4.Dispose();
		for (int k = 0; k < nativeList.Length; k++)
		{
			state.EntityManager.DestroyEntity(nativeList[k]);
		}
		nativeList.Dispose();
		protectedEntities.Clear();
		protectedEntities.AddRange(list.AsArray());
		list.Dispose();
		DynamicBuffer<Monster315CreateShieldRequest> singletonBuffer2 = __query_1191368475_5.GetSingletonBuffer<Monster315CreateShieldRequest>();
		NativeArray<Monster315CreateShieldRequest> nativeArray5 = __query_1191368475_5.GetSingletonBuffer<Monster315CreateShieldRequest>().ToNativeArray(Allocator.Temp);
		singletonBuffer2.Clear();
		foreach (Monster315CreateShieldRequest item2 in nativeArray5)
		{
			Monster315_Dots componentData2 = state.EntityManager.GetComponentData<Monster315_Dots>(item2.ownerEntity);
			componentData2.shieldEntity = QuickCreateSystem.Inst.CreateUnit(131502, item2.pos);
			state.EntityManager.SetComponentData(item2.ownerEntity, componentData2);
			UnitProperty_Dots componentData3 = state.EntityManager.GetComponentData<UnitProperty_Dots>(componentData2.shieldEntity);
			componentData3.Initialize(UnitConfig.map);
			componentData3.CanTouch = false;
			componentData3.showAffect = false;
			componentData3.unitCfg.maxHP *= SpecialObj301EndlessMonsterSpawner.Inst.hpRatioFix;
			componentData3.unitCfg.currentHP *= SpecialObj301EndlessMonsterSpawner.Inst.hpRatioFix;
			componentData3.ImmuneKnockbackRegister();
			state.EntityManager.SetComponentData(componentData2.shieldEntity, componentData3);
			Monster315Shield_Dots componentData4 = state.EntityManager.GetComponentData<Monster315Shield_Dots>(componentData2.shieldEntity);
			componentData4.Master = item2.ownerEntity;
			state.EntityManager.SetComponentData(componentData2.shieldEntity, componentData4);
		}
		nativeArray5.Dispose();
		DynamicBuffer<Monster315DeadRequest> singletonBuffer3 = __query_1191368475_6.GetSingletonBuffer<Monster315DeadRequest>();
		NativeArray<Monster315DeadRequest> nativeArray6 = singletonBuffer3.ToNativeArray(Allocator.Temp);
		singletonBuffer3.Clear();
		foreach (Monster315DeadRequest item3 in nativeArray6)
		{
			UnitProperty_Dots componentData5 = state.EntityManager.GetComponentData<UnitProperty_Dots>(item3.deadEntity);
			componentData5.AnnouncedDeath(item3.deadEntity);
			state.EntityManager.SetComponentData(item3.deadEntity, componentData5);
		}
		nativeArray6.Dispose();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private JobHandle __ScheduleViaJobChunkExtension_0(Monster315Job job, EntityQuery query, JobHandle dependency, ref SystemState state, bool hasUserDefinedQuery)
	{
		dependency = __TypeHandle.__Monster315System_Monster315Job_WithDefaultQuery_JobEntityTypeHandle.UpdateBaseEntityIndexArray(ref job, query, dependency, ref state);
		__TypeHandle.__Monster315System_Monster315Job_WithDefaultQuery_JobEntityTypeHandle.AssignEntityManager(ref job, state.EntityManager);
		__TypeHandle.__Monster315System_Monster315Job_WithDefaultQuery_JobEntityTypeHandle.__TypeHandle.Update(ref state);
		return __TypeHandle.__Monster315System_Monster315Job_WithDefaultQuery_JobEntityTypeHandle.ScheduleParallel(ref job, query, dependency);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<CurrentRoomEntitiesSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1191368475_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<Monster315CreateShieldRequest>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1191368475_1 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<Monster315DeadRequest>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1191368475_2 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<Monster315EntityInRange>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1191368475_3 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAllRW<Monster315EntityInRange>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1191368475_4 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAllRW<Monster315CreateShieldRequest>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1191368475_5 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAllRW<Monster315DeadRequest>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1191368475_6 = entityQueryBuilder2.Build(ref state);
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
		((Monster315System*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnUpdate(IntPtr self, IntPtr state)
	{
		((Monster315System*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
	}

	[BurstCompile]
	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal static void __codegen__OnDestroy(IntPtr self, IntPtr state)
	{
		__codegen__OnDestroy_00008BBC_0024BurstDirectCall.Invoke(self, state);
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
	{
		((Monster315System*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	[BurstCompile]
	internal unsafe static void __codegen__OnDestroy_0024BurstManaged(IntPtr self, IntPtr state)
	{
		((Monster315System*)self.ToPointer())->OnDestroy(ref *(SystemState*)state.ToPointer());
	}
}
