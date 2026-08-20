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
using Unity.Rendering;
using Unity.Transforms;

[BurstCompile]
[UpdateInGroup(typeof(SimulationSystemGroup), OrderLast = true)]
[UpdateAfter(typeof(SpellEndSystemGroup))]
[UpdateBefore(typeof(EndSimulationEntityCommandBufferSystem))]
[CompilerGenerated]
public struct SpellTransparentSystem : ISystem, ISystemCompilerGenerated
{
	[CompilerGenerated]
	[WithAll(new Type[] { typeof(SpellTag) })]
	private struct SpellTransparentJob : IJobEntity, IJobChunk
	{
		public struct InternalCompilerQueryAndHandleData
		{
			public struct TypeHandle
			{
				public ComponentTypeHandle<SpellTag> __SpellTransparentSystem_SpellTag_RW_ComponentTypeHandle;

				public ComponentTypeHandle<SpellTransparentMaterialOverride> __SpellTransparentSystem_SpellTransparentMaterialOverride_RW_ComponentTypeHandle;

				[ReadOnly]
				public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public void __AssignHandles(ref SystemState state)
				{
					__SpellTransparentSystem_SpellTag_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellTag>();
					__SpellTransparentSystem_SpellTransparentMaterialOverride_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellTransparentMaterialOverride>();
					__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
				}

				public void Update(ref SystemState state)
				{
					__SpellTransparentSystem_SpellTag_RW_ComponentTypeHandle.Update(ref state);
					__SpellTransparentSystem_SpellTransparentMaterialOverride_RW_ComponentTypeHandle.Update(ref state);
					__Unity_Entities_Entity_TypeHandle.Update(ref state);
				}
			}

			public TypeHandle __TypeHandle;

			public EntityQuery DefaultQuery;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private void __AssignQueries(ref SystemState state)
			{
				EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
				EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAllRW<SpellTag>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellTransparentMaterialOverride>();
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
			public void Run(ref SpellTransparentJob job, EntityQuery query)
			{
				job.__TypeHandle = __TypeHandle;
				JobChunkExtensions.RunByRef(ref job, query);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle Schedule(ref SpellTransparentJob job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = __TypeHandle;
				return JobChunkExtensions.ScheduleByRef(ref job, query, dependency);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle ScheduleParallel(ref SpellTransparentJob job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = __TypeHandle;
				return JobChunkExtensions.ScheduleParallelByRef(ref job, query, dependency);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void UpdateBaseEntityIndexArray(ref SpellTransparentJob job, EntityQuery query, ref SystemState state)
			{
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle UpdateBaseEntityIndexArray(ref SpellTransparentJob job, EntityQuery query, JobHandle dependency, ref SystemState state)
			{
				return dependency;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void AssignEntityManager(ref SpellTransparentJob job, EntityManager entityManager)
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

		public float SpellTransparency;

		[ReadOnly]
		public ComponentLookup<MaybeShootFromMonsterTag> MaybeShootFromMonsterLookup;

		[ReadOnly]
		public ComponentLookup<ShootFromMonsterTag> ShootFromMonsterLookup;

		[ReadOnly]
		public ComponentLookup<Parent> ParentLookup;

		[ReadOnly]
		public ComponentLookup<SpellConfigComponentData> ConfigLookup;

		private InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

		private void Execute(EnabledRefRW<SpellTag> enabled, ref SpellTransparentMaterialOverride alpha, Entity entity)
		{
			alpha.Alpha = SpellTransparency;
			if (MaybeShootFromMonsterLookup.HasComponent(entity))
			{
				Entity rootEntity = DTool.GetRootEntity(in entity, ParentLookup);
				if (ShootFromMonsterLookup.HasComponent(rootEntity) || (ConfigLookup.TryGetComponent(rootEntity, out var componentData) && DTool.IsSameCamp(componentData.ShooterType, UnitType.Monster)))
				{
					alpha.Alpha = 1f;
				}
			}
			enabled.ValueRW = false;
		}

		[CompilerGenerated]
		public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
		{
			EnabledMask enabledMask = chunk.GetEnabledMask(ref __TypeHandle.__SpellTransparentSystem_SpellTag_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellTransparentSystem_SpellTransparentMaterialOverride_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr2 = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(chunk, __TypeHandle.__Unity_Entities_Entity_TypeHandle);
			int count = chunk.Count;
			int num = 0;
			if (!useEnabledMask)
			{
				for (int i = 0; i < count; i++)
				{
					ref SpellTransparentMaterialOverride alpha = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellTransparentMaterialOverride>(nativeArrayPtr, i);
					Entity entity = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr2, i);
					Execute(enabledMask.GetEnabledRefRW<SpellTag>(i), ref alpha, entity);
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
						ref SpellTransparentMaterialOverride alpha2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellTransparentMaterialOverride>(nativeArrayPtr, nextRangeBegin);
						Entity entity2 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr2, nextRangeBegin);
						Execute(enabledMask.GetEnabledRefRW<SpellTag>(nextRangeBegin), ref alpha2, entity2);
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
					ref SpellTransparentMaterialOverride alpha3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellTransparentMaterialOverride>(nativeArrayPtr, j);
					Entity entity3 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr2, j);
					Execute(enabledMask.GetEnabledRefRW<SpellTag>(j), ref alpha3, entity3);
					num++;
				}
				num2 >>= 1;
			}
			num2 = chunkEnabledMask.ULong1;
			for (int k = 64; k < count; k++)
			{
				if ((num2 & 1) != 0L)
				{
					ref SpellTransparentMaterialOverride alpha4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellTransparentMaterialOverride>(nativeArrayPtr, k);
					Entity entity4 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr2, k);
					Execute(enabledMask.GetEnabledRefRW<SpellTag>(k), ref alpha4, entity4);
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

	[WithAll(new Type[] { typeof(TeammateTag) })]
	[CompilerGenerated]
	private struct TeammateTransparentJob : IJobEntity, IJobChunk
	{
		public struct InternalCompilerQueryAndHandleData
		{
			public struct TypeHandle
			{
				public ComponentTypeHandle<TeammateTag> __SpellTransparentSystem_TeammateTag_RW_ComponentTypeHandle;

				public ComponentTypeHandle<SpellTransparentMaterialOverride> __SpellTransparentSystem_SpellTransparentMaterialOverride_RW_ComponentTypeHandle;

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public void __AssignHandles(ref SystemState state)
				{
					__SpellTransparentSystem_TeammateTag_RW_ComponentTypeHandle = state.GetComponentTypeHandle<TeammateTag>();
					__SpellTransparentSystem_SpellTransparentMaterialOverride_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellTransparentMaterialOverride>();
				}

				public void Update(ref SystemState state)
				{
					__SpellTransparentSystem_TeammateTag_RW_ComponentTypeHandle.Update(ref state);
					__SpellTransparentSystem_SpellTransparentMaterialOverride_RW_ComponentTypeHandle.Update(ref state);
				}
			}

			public TypeHandle __TypeHandle;

			public EntityQuery DefaultQuery;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private void __AssignQueries(ref SystemState state)
			{
				EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
				EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAllRW<TeammateTag>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellTransparentMaterialOverride>();
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
			public void Run(ref TeammateTransparentJob job, EntityQuery query)
			{
				job.__TypeHandle = __TypeHandle;
				JobChunkExtensions.RunByRef(ref job, query);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle Schedule(ref TeammateTransparentJob job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = __TypeHandle;
				return JobChunkExtensions.ScheduleByRef(ref job, query, dependency);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle ScheduleParallel(ref TeammateTransparentJob job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = __TypeHandle;
				return JobChunkExtensions.ScheduleParallelByRef(ref job, query, dependency);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void UpdateBaseEntityIndexArray(ref TeammateTransparentJob job, EntityQuery query, ref SystemState state)
			{
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle UpdateBaseEntityIndexArray(ref TeammateTransparentJob job, EntityQuery query, JobHandle dependency, ref SystemState state)
			{
				return dependency;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void AssignEntityManager(ref TeammateTransparentJob job, EntityManager entityManager)
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

		public float TeammateTransparency;

		private InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

		private void Execute(EnabledRefRW<TeammateTag> enabled, ref SpellTransparentMaterialOverride alpha)
		{
			alpha.Alpha = TeammateTransparency;
			enabled.ValueRW = false;
		}

		[CompilerGenerated]
		public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
		{
			EnabledMask enabledMask = chunk.GetEnabledMask(ref __TypeHandle.__SpellTransparentSystem_TeammateTag_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellTransparentSystem_SpellTransparentMaterialOverride_RW_ComponentTypeHandle);
			int count = chunk.Count;
			int num = 0;
			if (!useEnabledMask)
			{
				for (int i = 0; i < count; i++)
				{
					Execute(alpha: ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellTransparentMaterialOverride>(nativeArrayPtr, i), enabled: enabledMask.GetEnabledRefRW<TeammateTag>(i));
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
						Execute(alpha: ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellTransparentMaterialOverride>(nativeArrayPtr, nextRangeBegin), enabled: enabledMask.GetEnabledRefRW<TeammateTag>(nextRangeBegin));
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
					Execute(alpha: ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellTransparentMaterialOverride>(nativeArrayPtr, j), enabled: enabledMask.GetEnabledRefRW<TeammateTag>(j));
					num++;
				}
				num2 >>= 1;
			}
			num2 = chunkEnabledMask.ULong1;
			for (int k = 64; k < count; k++)
			{
				if ((num2 & 1) != 0L)
				{
					Execute(alpha: ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellTransparentMaterialOverride>(nativeArrayPtr, k), enabled: enabledMask.GetEnabledRefRW<TeammateTag>(k));
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

	[WithAll(new Type[] { typeof(ByShooterTypeTag) })]
	[CompilerGenerated]
	private struct ByShooterTypeTransparentJob : IJobEntity, IJobChunk
	{
		public struct InternalCompilerQueryAndHandleData
		{
			public struct TypeHandle
			{
				public ComponentTypeHandle<ByShooterTypeTag> __SpellTransparentSystem_ByShooterTypeTag_RW_ComponentTypeHandle;

				public ComponentTypeHandle<SpellTransparentMaterialOverride> __SpellTransparentSystem_SpellTransparentMaterialOverride_RW_ComponentTypeHandle;

				[ReadOnly]
				public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public void __AssignHandles(ref SystemState state)
				{
					__SpellTransparentSystem_ByShooterTypeTag_RW_ComponentTypeHandle = state.GetComponentTypeHandle<ByShooterTypeTag>();
					__SpellTransparentSystem_SpellTransparentMaterialOverride_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellTransparentMaterialOverride>();
					__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
				}

				public void Update(ref SystemState state)
				{
					__SpellTransparentSystem_ByShooterTypeTag_RW_ComponentTypeHandle.Update(ref state);
					__SpellTransparentSystem_SpellTransparentMaterialOverride_RW_ComponentTypeHandle.Update(ref state);
					__Unity_Entities_Entity_TypeHandle.Update(ref state);
				}
			}

			public TypeHandle __TypeHandle;

			public EntityQuery DefaultQuery;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private void __AssignQueries(ref SystemState state)
			{
				EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
				EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAllRW<ByShooterTypeTag>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellTransparentMaterialOverride>();
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
			public void Run(ref ByShooterTypeTransparentJob job, EntityQuery query)
			{
				job.__TypeHandle = __TypeHandle;
				JobChunkExtensions.RunByRef(ref job, query);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle Schedule(ref ByShooterTypeTransparentJob job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = __TypeHandle;
				return JobChunkExtensions.ScheduleByRef(ref job, query, dependency);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle ScheduleParallel(ref ByShooterTypeTransparentJob job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = __TypeHandle;
				return JobChunkExtensions.ScheduleParallelByRef(ref job, query, dependency);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void UpdateBaseEntityIndexArray(ref ByShooterTypeTransparentJob job, EntityQuery query, ref SystemState state)
			{
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle UpdateBaseEntityIndexArray(ref ByShooterTypeTransparentJob job, EntityQuery query, JobHandle dependency, ref SystemState state)
			{
				return dependency;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void AssignEntityManager(ref ByShooterTypeTransparentJob job, EntityManager entityManager)
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

		public float SpellTransparency;

		public float TeammateTransparency;

		[ReadOnly]
		public ComponentLookup<SpellConfigComponentData> SpellConfigLookup;

		[ReadOnly]
		public ComponentLookup<Parent> ParentLookup;

		private InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

		private void Execute(EnabledRefRW<ByShooterTypeTag> enabled, ref SpellTransparentMaterialOverride alpha, Entity entity)
		{
			float alpha2 = SpellTransparency;
			Entity rootEntity = DTool.GetRootEntity(in entity, ParentLookup);
			if (SpellConfigLookup.TryGetComponent(rootEntity, out var componentData))
			{
				if (DTool.IsSameCamp(componentData.ShooterType, UnitType.Monster))
				{
					alpha2 = 1f;
				}
				else if (componentData.IsTeammate)
				{
					alpha2 = TeammateTransparency;
				}
			}
			alpha.Alpha = alpha2;
			enabled.ValueRW = false;
		}

		[CompilerGenerated]
		public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
		{
			EnabledMask enabledMask = chunk.GetEnabledMask(ref __TypeHandle.__SpellTransparentSystem_ByShooterTypeTag_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellTransparentSystem_SpellTransparentMaterialOverride_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr2 = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(chunk, __TypeHandle.__Unity_Entities_Entity_TypeHandle);
			int count = chunk.Count;
			int num = 0;
			if (!useEnabledMask)
			{
				for (int i = 0; i < count; i++)
				{
					ref SpellTransparentMaterialOverride alpha = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellTransparentMaterialOverride>(nativeArrayPtr, i);
					Entity entity = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr2, i);
					Execute(enabledMask.GetEnabledRefRW<ByShooterTypeTag>(i), ref alpha, entity);
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
						ref SpellTransparentMaterialOverride alpha2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellTransparentMaterialOverride>(nativeArrayPtr, nextRangeBegin);
						Entity entity2 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr2, nextRangeBegin);
						Execute(enabledMask.GetEnabledRefRW<ByShooterTypeTag>(nextRangeBegin), ref alpha2, entity2);
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
					ref SpellTransparentMaterialOverride alpha3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellTransparentMaterialOverride>(nativeArrayPtr, j);
					Entity entity3 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr2, j);
					Execute(enabledMask.GetEnabledRefRW<ByShooterTypeTag>(j), ref alpha3, entity3);
					num++;
				}
				num2 >>= 1;
			}
			num2 = chunkEnabledMask.ULong1;
			for (int k = 64; k < count; k++)
			{
				if ((num2 & 1) != 0L)
				{
					ref SpellTransparentMaterialOverride alpha4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellTransparentMaterialOverride>(nativeArrayPtr, k);
					Entity entity4 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr2, k);
					Execute(enabledMask.GetEnabledRefRW<ByShooterTypeTag>(k), ref alpha4, entity4);
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
	public struct ByShooterTypeTag : IComponentData, IQueryTypeParameter, IEnableableComponent
	{
	}

	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct SpellTag : IComponentData, IQueryTypeParameter, IEnableableComponent
	{
	}

	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct TeammateTag : IComponentData, IQueryTypeParameter, IEnableableComponent
	{
	}

	[MaterialProperty("_Alpha", -1)]
	public struct SpellTransparentMaterialOverride : IComponentData, IQueryTypeParameter
	{
		public float Alpha;
	}

	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct MaybeShootFromMonsterTag : IComponentData, IQueryTypeParameter
	{
	}

	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct ShootFromMonsterTag : IComponentData, IQueryTypeParameter
	{
	}

	private struct TypeHandle
	{
		public ComponentLookup<ByShooterTypeTag> __SpellTransparentSystem_ByShooterTypeTag_RW_ComponentLookup;

		public ComponentLookup<SpellTag> __SpellTransparentSystem_SpellTag_RW_ComponentLookup;

		public ComponentLookup<TeammateTag> __SpellTransparentSystem_TeammateTag_RW_ComponentLookup;

		public TeammateTransparentJob.InternalCompilerQueryAndHandleData __SpellTransparentSystem_TeammateTransparentJob_WithDefaultQuery_JobEntityTypeHandle;

		[ReadOnly]
		public ComponentLookup<Parent> __Unity_Transforms_Parent_RO_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<MaybeShootFromMonsterTag> __SpellTransparentSystem_MaybeShootFromMonsterTag_RO_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<ShootFromMonsterTag> __SpellTransparentSystem_ShootFromMonsterTag_RO_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<SpellConfigComponentData> __SpellConfigComponentData_RO_ComponentLookup;

		public SpellTransparentJob.InternalCompilerQueryAndHandleData __SpellTransparentSystem_SpellTransparentJob_WithDefaultQuery_JobEntityTypeHandle;

		public ByShooterTypeTransparentJob.InternalCompilerQueryAndHandleData __SpellTransparentSystem_ByShooterTypeTransparentJob_WithDefaultQuery_JobEntityTypeHandle;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__SpellTransparentSystem_ByShooterTypeTag_RW_ComponentLookup = state.GetComponentLookup<ByShooterTypeTag>();
			__SpellTransparentSystem_SpellTag_RW_ComponentLookup = state.GetComponentLookup<SpellTag>();
			__SpellTransparentSystem_TeammateTag_RW_ComponentLookup = state.GetComponentLookup<TeammateTag>();
			__SpellTransparentSystem_TeammateTransparentJob_WithDefaultQuery_JobEntityTypeHandle.Init(ref state, assignDefaultQuery: true);
			__Unity_Transforms_Parent_RO_ComponentLookup = state.GetComponentLookup<Parent>(isReadOnly: true);
			__SpellTransparentSystem_MaybeShootFromMonsterTag_RO_ComponentLookup = state.GetComponentLookup<MaybeShootFromMonsterTag>(isReadOnly: true);
			__SpellTransparentSystem_ShootFromMonsterTag_RO_ComponentLookup = state.GetComponentLookup<ShootFromMonsterTag>(isReadOnly: true);
			__SpellConfigComponentData_RO_ComponentLookup = state.GetComponentLookup<SpellConfigComponentData>(isReadOnly: true);
			__SpellTransparentSystem_SpellTransparentJob_WithDefaultQuery_JobEntityTypeHandle.Init(ref state, assignDefaultQuery: true);
			__SpellTransparentSystem_ByShooterTypeTransparentJob_WithDefaultQuery_JobEntityTypeHandle.Init(ref state, assignDefaultQuery: true);
		}
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void __codegen__OnCreate_000085CC_0024PostfixBurstDelegate(IntPtr self, IntPtr state);

	internal static class __codegen__OnCreate_000085CC_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<__codegen__OnCreate_000085CC_0024PostfixBurstDelegate>(delegate(IntPtr self, IntPtr state)
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
			__codegen__OnCreate_0024BurstManaged(self, state);
		}
	}

	private EntityQuery _shadowQuery;

	private EntityQuery _spellQuery;

	private EntityQuery _teammateQuery;

	private float _lastCheckSpellTransparent;

	private float _lastCheckTeammateTransparent;

	private TypeHandle __TypeHandle;

	private EntityQuery __query_1947120442_0;

	private EntityQuery __query_1947120442_1;

	private EntityQuery __query_1947120442_2;

	[BurstCompile]
	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
		_shadowQuery = __query_1947120442_0;
		_spellQuery = __query_1947120442_1;
		_teammateQuery = __query_1947120442_2;
	}

	[BurstCompile]
	private void CheckTransparentChange(float spellTransparent, float teammateTransparent, ref SystemState state)
	{
		bool flag = spellTransparent != _lastCheckSpellTransparent;
		bool flag2 = teammateTransparent != _lastCheckTeammateTransparent;
		_lastCheckSpellTransparent = spellTransparent;
		_lastCheckTeammateTransparent = teammateTransparent;
		if (flag || flag2)
		{
			foreach (Entity item in _shadowQuery.ToEntityArray(Allocator.Temp))
			{
				InternalCompilerInterface.SetComponentEnabledAfterCompletingDependency(ref __TypeHandle.__SpellTransparentSystem_ByShooterTypeTag_RW_ComponentLookup, ref state, item, value: true);
			}
		}
		if (flag)
		{
			foreach (Entity item2 in _spellQuery.ToEntityArray(Allocator.Temp))
			{
				InternalCompilerInterface.SetComponentEnabledAfterCompletingDependency(ref __TypeHandle.__SpellTransparentSystem_SpellTag_RW_ComponentLookup, ref state, item2, value: true);
			}
		}
		if (!flag2)
		{
			return;
		}
		foreach (Entity item3 in _teammateQuery.ToEntityArray(Allocator.Temp))
		{
			InternalCompilerInterface.SetComponentEnabledAfterCompletingDependency(ref __TypeHandle.__SpellTransparentSystem_TeammateTag_RW_ComponentLookup, ref state, item3, value: true);
		}
	}

	public void OnUpdate(ref SystemState state)
	{
		if (DataMgr.settingData != null)
		{
			CheckTransparentChange(DataMgr.settingData.SpellTransparent, DataMgr.settingData.SummonTransparent, ref state);
			state.Dependency = __ScheduleViaJobChunkExtension_0(new TeammateTransparentJob
			{
				TeammateTransparency = DataMgr.settingData.SummonTransparent
			}, __TypeHandle.__SpellTransparentSystem_TeammateTransparentJob_WithDefaultQuery_JobEntityTypeHandle.DefaultQuery, state.Dependency, ref state, hasUserDefinedQuery: false);
			state.Dependency = __ScheduleViaJobChunkExtension_1(new SpellTransparentJob
			{
				SpellTransparency = DataMgr.settingData.SpellTransparent,
				ParentLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__Unity_Transforms_Parent_RO_ComponentLookup, ref state),
				MaybeShootFromMonsterLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__SpellTransparentSystem_MaybeShootFromMonsterTag_RO_ComponentLookup, ref state),
				ShootFromMonsterLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__SpellTransparentSystem_ShootFromMonsterTag_RO_ComponentLookup, ref state),
				ConfigLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__SpellConfigComponentData_RO_ComponentLookup, ref state)
			}, __TypeHandle.__SpellTransparentSystem_SpellTransparentJob_WithDefaultQuery_JobEntityTypeHandle.DefaultQuery, state.Dependency, ref state, hasUserDefinedQuery: false);
			state.Dependency = __ScheduleViaJobChunkExtension_2(new ByShooterTypeTransparentJob
			{
				SpellTransparency = DataMgr.settingData.SpellTransparent,
				TeammateTransparency = DataMgr.settingData.SummonTransparent,
				ParentLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__Unity_Transforms_Parent_RO_ComponentLookup, ref state),
				SpellConfigLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__SpellConfigComponentData_RO_ComponentLookup, ref state)
			}, __TypeHandle.__SpellTransparentSystem_ByShooterTypeTransparentJob_WithDefaultQuery_JobEntityTypeHandle.DefaultQuery, state.Dependency, ref state, hasUserDefinedQuery: false);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private JobHandle __ScheduleViaJobChunkExtension_0(TeammateTransparentJob job, EntityQuery query, JobHandle dependency, ref SystemState state, bool hasUserDefinedQuery)
	{
		dependency = __TypeHandle.__SpellTransparentSystem_TeammateTransparentJob_WithDefaultQuery_JobEntityTypeHandle.UpdateBaseEntityIndexArray(ref job, query, dependency, ref state);
		__TypeHandle.__SpellTransparentSystem_TeammateTransparentJob_WithDefaultQuery_JobEntityTypeHandle.AssignEntityManager(ref job, state.EntityManager);
		__TypeHandle.__SpellTransparentSystem_TeammateTransparentJob_WithDefaultQuery_JobEntityTypeHandle.__TypeHandle.Update(ref state);
		return __TypeHandle.__SpellTransparentSystem_TeammateTransparentJob_WithDefaultQuery_JobEntityTypeHandle.ScheduleParallel(ref job, query, dependency);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private JobHandle __ScheduleViaJobChunkExtension_1(SpellTransparentJob job, EntityQuery query, JobHandle dependency, ref SystemState state, bool hasUserDefinedQuery)
	{
		dependency = __TypeHandle.__SpellTransparentSystem_SpellTransparentJob_WithDefaultQuery_JobEntityTypeHandle.UpdateBaseEntityIndexArray(ref job, query, dependency, ref state);
		__TypeHandle.__SpellTransparentSystem_SpellTransparentJob_WithDefaultQuery_JobEntityTypeHandle.AssignEntityManager(ref job, state.EntityManager);
		__TypeHandle.__SpellTransparentSystem_SpellTransparentJob_WithDefaultQuery_JobEntityTypeHandle.__TypeHandle.Update(ref state);
		return __TypeHandle.__SpellTransparentSystem_SpellTransparentJob_WithDefaultQuery_JobEntityTypeHandle.ScheduleParallel(ref job, query, dependency);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private JobHandle __ScheduleViaJobChunkExtension_2(ByShooterTypeTransparentJob job, EntityQuery query, JobHandle dependency, ref SystemState state, bool hasUserDefinedQuery)
	{
		dependency = __TypeHandle.__SpellTransparentSystem_ByShooterTypeTransparentJob_WithDefaultQuery_JobEntityTypeHandle.UpdateBaseEntityIndexArray(ref job, query, dependency, ref state);
		__TypeHandle.__SpellTransparentSystem_ByShooterTypeTransparentJob_WithDefaultQuery_JobEntityTypeHandle.AssignEntityManager(ref job, state.EntityManager);
		__TypeHandle.__SpellTransparentSystem_ByShooterTypeTransparentJob_WithDefaultQuery_JobEntityTypeHandle.__TypeHandle.Update(ref state);
		return __TypeHandle.__SpellTransparentSystem_ByShooterTypeTransparentJob_WithDefaultQuery_JobEntityTypeHandle.ScheduleParallel(ref job, query, dependency);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithDisabled<ByShooterTypeTag>();
		__query_1947120442_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithDisabled<SpellTag>();
		__query_1947120442_1 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithDisabled<TeammateTag>();
		__query_1947120442_2 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder.Dispose();
	}

	public void OnCreateForCompiler(ref SystemState state)
	{
		__AssignQueries(ref state);
		__TypeHandle.__AssignHandles(ref state);
	}

	[BurstCompile]
	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal static void __codegen__OnCreate(IntPtr self, IntPtr state)
	{
		__codegen__OnCreate_000085CC_0024BurstDirectCall.Invoke(self, state);
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnUpdate(IntPtr self, IntPtr state)
	{
		((SpellTransparentSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
	{
		((SpellTransparentSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[BurstCompile]
	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreate_0024BurstManaged(IntPtr self, IntPtr state)
	{
		((SpellTransparentSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
	}
}
