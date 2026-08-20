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
using Unity.Transforms;

[BurstCompile]
[UpdateBefore(typeof(GlobalParticleEmitSystem))]
[UpdateAfter(typeof(TransformSystemGroup))]
[CompilerGenerated]
public struct GlobalParticleEmitterEntitySystem : ISystem, ISystemCompilerGenerated
{
	[BurstCompile]
	[CompilerGenerated]
	private struct ByTimeEmitterJob : IJobEntity, IJobChunk
	{
		public struct InternalCompilerQueryAndHandleData
		{
			public struct TypeHandle
			{
				[ReadOnly]
				public ComponentTypeHandle<LocalToWorld> __Unity_Transforms_LocalToWorld_RO_ComponentTypeHandle;

				[ReadOnly]
				public ComponentTypeHandle<GlobalParticle.Emitter> __GlobalParticle_Emitter_RO_ComponentTypeHandle;

				public ComponentTypeHandle<GlobalParticle.EmitTimer> __GlobalParticle_EmitTimer_RW_ComponentTypeHandle;

				[ReadOnly]
				public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public void __AssignHandles(ref SystemState state)
				{
					__Unity_Transforms_LocalToWorld_RO_ComponentTypeHandle = state.GetComponentTypeHandle<LocalToWorld>(isReadOnly: true);
					__GlobalParticle_Emitter_RO_ComponentTypeHandle = state.GetComponentTypeHandle<GlobalParticle.Emitter>(isReadOnly: true);
					__GlobalParticle_EmitTimer_RW_ComponentTypeHandle = state.GetComponentTypeHandle<GlobalParticle.EmitTimer>();
					__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
				}

				public void Update(ref SystemState state)
				{
					__Unity_Transforms_LocalToWorld_RO_ComponentTypeHandle.Update(ref state);
					__GlobalParticle_Emitter_RO_ComponentTypeHandle.Update(ref state);
					__GlobalParticle_EmitTimer_RW_ComponentTypeHandle.Update(ref state);
					__Unity_Entities_Entity_TypeHandle.Update(ref state);
				}
			}

			public TypeHandle __TypeHandle;

			public EntityQuery DefaultQuery;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private void __AssignQueries(ref SystemState state)
			{
				EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
				EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<LocalToWorld>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAll<GlobalParticle.Emitter>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<GlobalParticle.EmitTimer>();
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
			public void Run(ref ByTimeEmitterJob job, EntityQuery query)
			{
				job.__TypeHandle = __TypeHandle;
				JobChunkExtensions.RunByRef(ref job, query);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle Schedule(ref ByTimeEmitterJob job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = __TypeHandle;
				return JobChunkExtensions.ScheduleByRef(ref job, query, dependency);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle ScheduleParallel(ref ByTimeEmitterJob job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = __TypeHandle;
				return JobChunkExtensions.ScheduleParallelByRef(ref job, query, dependency);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void UpdateBaseEntityIndexArray(ref ByTimeEmitterJob job, EntityQuery query, ref SystemState state)
			{
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle UpdateBaseEntityIndexArray(ref ByTimeEmitterJob job, EntityQuery query, JobHandle dependency, ref SystemState state)
			{
				return dependency;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void AssignEntityManager(ref ByTimeEmitterJob job, EntityManager entityManager)
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

		public EntityCommandBuffer.ParallelWriter CMD;

		public Entity EmitBufferEntity;

		public float DeltaTime;

		public GlobalRandom Random;

		[ReadOnly]
		public ComponentLookup<SpellTransparentSystem.SpellTransparentMaterialOverride> TransparentLookup;

		private InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

		[BurstCompile]
		private void Execute(in LocalToWorld worldTransform, in GlobalParticle.Emitter emitter, ref GlobalParticle.EmitTimer timer, [ChunkIndexInQuery] int sortKey, Entity emitterEntity)
		{
			timer.Timer += DeltaTime;
			while (timer.Timer >= timer.Interval)
			{
				timer.Timer -= timer.Interval;
				EmitParticle(CMD, sortKey, worldTransform.Position, GetWorldScale(worldTransform), emitter, ref Random, EmitBufferEntity, TransparentLookup, emitterEntity);
			}
		}

		[CompilerGenerated]
		public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
		{
			IntPtr nativeArrayPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr(chunk, ref __TypeHandle.__Unity_Transforms_LocalToWorld_RO_ComponentTypeHandle);
			IntPtr nativeArrayPtr2 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr(chunk, ref __TypeHandle.__GlobalParticle_Emitter_RO_ComponentTypeHandle);
			IntPtr nativeArrayPtr3 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__GlobalParticle_EmitTimer_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr4 = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(chunk, __TypeHandle.__Unity_Entities_Entity_TypeHandle);
			int count = chunk.Count;
			int num = 0;
			if (!useEnabledMask)
			{
				for (int i = 0; i < count; i++)
				{
					ref LocalToWorld worldTransform = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalToWorld>(nativeArrayPtr, i);
					ref GlobalParticle.Emitter emitter = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<GlobalParticle.Emitter>(nativeArrayPtr2, i);
					ref GlobalParticle.EmitTimer timer = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<GlobalParticle.EmitTimer>(nativeArrayPtr3, i);
					Entity emitterEntity = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr4, i);
					Execute(in worldTransform, in emitter, ref timer, chunkIndexInQuery, emitterEntity);
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
						ref LocalToWorld worldTransform2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalToWorld>(nativeArrayPtr, nextRangeBegin);
						ref GlobalParticle.Emitter emitter2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<GlobalParticle.Emitter>(nativeArrayPtr2, nextRangeBegin);
						ref GlobalParticle.EmitTimer timer2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<GlobalParticle.EmitTimer>(nativeArrayPtr3, nextRangeBegin);
						Entity emitterEntity2 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr4, nextRangeBegin);
						Execute(in worldTransform2, in emitter2, ref timer2, chunkIndexInQuery, emitterEntity2);
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
					ref LocalToWorld worldTransform3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalToWorld>(nativeArrayPtr, j);
					ref GlobalParticle.Emitter emitter3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<GlobalParticle.Emitter>(nativeArrayPtr2, j);
					ref GlobalParticle.EmitTimer timer3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<GlobalParticle.EmitTimer>(nativeArrayPtr3, j);
					Entity emitterEntity3 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr4, j);
					Execute(in worldTransform3, in emitter3, ref timer3, chunkIndexInQuery, emitterEntity3);
					num++;
				}
				num2 >>= 1;
			}
			num2 = chunkEnabledMask.ULong1;
			for (int k = 64; k < count; k++)
			{
				if ((num2 & 1) != 0L)
				{
					ref LocalToWorld worldTransform4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalToWorld>(nativeArrayPtr, k);
					ref GlobalParticle.Emitter emitter4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<GlobalParticle.Emitter>(nativeArrayPtr2, k);
					ref GlobalParticle.EmitTimer timer4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<GlobalParticle.EmitTimer>(nativeArrayPtr3, k);
					Entity emitterEntity4 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr4, k);
					Execute(in worldTransform4, in emitter4, ref timer4, chunkIndexInQuery, emitterEntity4);
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

	[CompilerGenerated]
	[BurstCompile]
	private struct ByDistanceEmitterJob : IJobEntity, IJobChunk
	{
		public struct InternalCompilerQueryAndHandleData
		{
			public struct TypeHandle
			{
				[ReadOnly]
				public ComponentTypeHandle<LocalToWorld> __Unity_Transforms_LocalToWorld_RO_ComponentTypeHandle;

				[ReadOnly]
				public ComponentTypeHandle<GlobalParticle.Emitter> __GlobalParticle_Emitter_RO_ComponentTypeHandle;

				public ComponentTypeHandle<GlobalParticle.EmitDistanceCounter> __GlobalParticle_EmitDistanceCounter_RW_ComponentTypeHandle;

				[ReadOnly]
				public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public void __AssignHandles(ref SystemState state)
				{
					__Unity_Transforms_LocalToWorld_RO_ComponentTypeHandle = state.GetComponentTypeHandle<LocalToWorld>(isReadOnly: true);
					__GlobalParticle_Emitter_RO_ComponentTypeHandle = state.GetComponentTypeHandle<GlobalParticle.Emitter>(isReadOnly: true);
					__GlobalParticle_EmitDistanceCounter_RW_ComponentTypeHandle = state.GetComponentTypeHandle<GlobalParticle.EmitDistanceCounter>();
					__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
				}

				public void Update(ref SystemState state)
				{
					__Unity_Transforms_LocalToWorld_RO_ComponentTypeHandle.Update(ref state);
					__GlobalParticle_Emitter_RO_ComponentTypeHandle.Update(ref state);
					__GlobalParticle_EmitDistanceCounter_RW_ComponentTypeHandle.Update(ref state);
					__Unity_Entities_Entity_TypeHandle.Update(ref state);
				}
			}

			public TypeHandle __TypeHandle;

			public EntityQuery DefaultQuery;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private void __AssignQueries(ref SystemState state)
			{
				EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
				EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<LocalToWorld>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAll<GlobalParticle.Emitter>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<GlobalParticle.EmitDistanceCounter>();
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
			public void Run(ref ByDistanceEmitterJob job, EntityQuery query)
			{
				job.__TypeHandle = __TypeHandle;
				JobChunkExtensions.RunByRef(ref job, query);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle Schedule(ref ByDistanceEmitterJob job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = __TypeHandle;
				return JobChunkExtensions.ScheduleByRef(ref job, query, dependency);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle ScheduleParallel(ref ByDistanceEmitterJob job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = __TypeHandle;
				return JobChunkExtensions.ScheduleParallelByRef(ref job, query, dependency);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void UpdateBaseEntityIndexArray(ref ByDistanceEmitterJob job, EntityQuery query, ref SystemState state)
			{
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle UpdateBaseEntityIndexArray(ref ByDistanceEmitterJob job, EntityQuery query, JobHandle dependency, ref SystemState state)
			{
				return dependency;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void AssignEntityManager(ref ByDistanceEmitterJob job, EntityManager entityManager)
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

		public EntityCommandBuffer.ParallelWriter CMD;

		public Entity EmitBufferEntity;

		public GlobalRandom Random;

		[ReadOnly]
		public ComponentLookup<SpellTransparentSystem.SpellTransparentMaterialOverride> TransparentLookup;

		private InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

		[BurstCompile]
		private void Execute(in LocalToWorld worldTransform, in GlobalParticle.Emitter emitter, ref GlobalParticle.EmitDistanceCounter counter, [ChunkIndexInQuery] int sortKey, Entity emitterEntity)
		{
			if (!counter.startCounter)
			{
				counter.lastCountPoint = worldTransform.Position;
				counter.lastEmitPoint = worldTransform.Position;
				counter.startCounter = true;
				counter.Counter = 0f;
				return;
			}
			counter.Counter += math.distance(worldTransform.Position, counter.lastCountPoint);
			counter.lastCountPoint = worldTransform.Position;
			if (counter.Counter >= counter.Interval)
			{
				float3 lastEmitPoint = counter.lastEmitPoint;
				float3 position = worldTransform.Position;
				float num = math.length(position - lastEmitPoint);
				while (counter.Counter >= counter.Interval)
				{
					float t = 1f - math.clamp(counter.Counter / num, 0f, 1f);
					float3 @float = math.lerp(lastEmitPoint, position, t);
					EmitParticle(CMD, sortKey, @float, GetWorldScale(worldTransform), emitter, ref Random, EmitBufferEntity, TransparentLookup, emitterEntity);
					counter.lastEmitPoint = @float;
					counter.Counter -= counter.Interval;
				}
			}
		}

		[CompilerGenerated]
		public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
		{
			IntPtr nativeArrayPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr(chunk, ref __TypeHandle.__Unity_Transforms_LocalToWorld_RO_ComponentTypeHandle);
			IntPtr nativeArrayPtr2 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr(chunk, ref __TypeHandle.__GlobalParticle_Emitter_RO_ComponentTypeHandle);
			IntPtr nativeArrayPtr3 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__GlobalParticle_EmitDistanceCounter_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr4 = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(chunk, __TypeHandle.__Unity_Entities_Entity_TypeHandle);
			int count = chunk.Count;
			int num = 0;
			if (!useEnabledMask)
			{
				for (int i = 0; i < count; i++)
				{
					ref LocalToWorld worldTransform = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalToWorld>(nativeArrayPtr, i);
					ref GlobalParticle.Emitter emitter = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<GlobalParticle.Emitter>(nativeArrayPtr2, i);
					ref GlobalParticle.EmitDistanceCounter counter = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<GlobalParticle.EmitDistanceCounter>(nativeArrayPtr3, i);
					Entity emitterEntity = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr4, i);
					Execute(in worldTransform, in emitter, ref counter, chunkIndexInQuery, emitterEntity);
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
						ref LocalToWorld worldTransform2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalToWorld>(nativeArrayPtr, nextRangeBegin);
						ref GlobalParticle.Emitter emitter2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<GlobalParticle.Emitter>(nativeArrayPtr2, nextRangeBegin);
						ref GlobalParticle.EmitDistanceCounter counter2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<GlobalParticle.EmitDistanceCounter>(nativeArrayPtr3, nextRangeBegin);
						Entity emitterEntity2 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr4, nextRangeBegin);
						Execute(in worldTransform2, in emitter2, ref counter2, chunkIndexInQuery, emitterEntity2);
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
					ref LocalToWorld worldTransform3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalToWorld>(nativeArrayPtr, j);
					ref GlobalParticle.Emitter emitter3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<GlobalParticle.Emitter>(nativeArrayPtr2, j);
					ref GlobalParticle.EmitDistanceCounter counter3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<GlobalParticle.EmitDistanceCounter>(nativeArrayPtr3, j);
					Entity emitterEntity3 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr4, j);
					Execute(in worldTransform3, in emitter3, ref counter3, chunkIndexInQuery, emitterEntity3);
					num++;
				}
				num2 >>= 1;
			}
			num2 = chunkEnabledMask.ULong1;
			for (int k = 64; k < count; k++)
			{
				if ((num2 & 1) != 0L)
				{
					ref LocalToWorld worldTransform4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalToWorld>(nativeArrayPtr, k);
					ref GlobalParticle.Emitter emitter4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<GlobalParticle.Emitter>(nativeArrayPtr2, k);
					ref GlobalParticle.EmitDistanceCounter counter4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<GlobalParticle.EmitDistanceCounter>(nativeArrayPtr3, k);
					Entity emitterEntity4 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr4, k);
					Execute(in worldTransform4, in emitter4, ref counter4, chunkIndexInQuery, emitterEntity4);
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
		[ReadOnly]
		public ComponentLookup<SpellTransparentSystem.SpellTransparentMaterialOverride> __SpellTransparentSystem_SpellTransparentMaterialOverride_RO_ComponentLookup;

		public ByTimeEmitterJob.InternalCompilerQueryAndHandleData __GlobalParticleEmitterEntitySystem_ByTimeEmitterJob_WithDefaultQuery_JobEntityTypeHandle;

		public ByDistanceEmitterJob.InternalCompilerQueryAndHandleData __GlobalParticleEmitterEntitySystem_ByDistanceEmitterJob_WithDefaultQuery_JobEntityTypeHandle;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__SpellTransparentSystem_SpellTransparentMaterialOverride_RO_ComponentLookup = state.GetComponentLookup<SpellTransparentSystem.SpellTransparentMaterialOverride>(isReadOnly: true);
			__GlobalParticleEmitterEntitySystem_ByTimeEmitterJob_WithDefaultQuery_JobEntityTypeHandle.Init(ref state, assignDefaultQuery: true);
			__GlobalParticleEmitterEntitySystem_ByDistanceEmitterJob_WithDefaultQuery_JobEntityTypeHandle.Init(ref state, assignDefaultQuery: true);
		}
	}

	private TypeHandle __TypeHandle;

	private EntityQuery __query_525405871_0;

	private EntityQuery __query_525405871_1;

	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<GlobalRandom>();
		state.RequireForUpdate<GlobalParticleEmitParams>();
		state.RequireForUpdate<GlobalParticle.Emitter>();
	}

	public void OnUpdate(ref SystemState state)
	{
		EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer(Allocator.TempJob);
		EntityCommandBuffer.ParallelWriter cMD = entityCommandBuffer.AsParallelWriter();
		RefRW<GlobalRandom> singletonRW = __query_525405871_0.GetSingletonRW<GlobalRandom>();
		singletonRW.ValueRW.NextFloatByChunkIndex(12345);
		state.Dependency = __ScheduleViaJobChunkExtension_0(new ByTimeEmitterJob
		{
			CMD = cMD,
			DeltaTime = state.WorldUnmanaged.Time.DeltaTime,
			EmitBufferEntity = __query_525405871_1.GetSingletonEntity(),
			Random = singletonRW.ValueRW,
			TransparentLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__SpellTransparentSystem_SpellTransparentMaterialOverride_RO_ComponentLookup, ref state)
		}, __TypeHandle.__GlobalParticleEmitterEntitySystem_ByTimeEmitterJob_WithDefaultQuery_JobEntityTypeHandle.DefaultQuery, state.Dependency, ref state, hasUserDefinedQuery: false);
		singletonRW.ValueRW.NextFloatByChunkIndex(12345);
		state.Dependency = __ScheduleViaJobChunkExtension_1(new ByDistanceEmitterJob
		{
			CMD = cMD,
			EmitBufferEntity = __query_525405871_1.GetSingletonEntity(),
			Random = singletonRW.ValueRW,
			TransparentLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__SpellTransparentSystem_SpellTransparentMaterialOverride_RO_ComponentLookup, ref state)
		}, __TypeHandle.__GlobalParticleEmitterEntitySystem_ByDistanceEmitterJob_WithDefaultQuery_JobEntityTypeHandle.DefaultQuery, state.Dependency, ref state, hasUserDefinedQuery: false);
		state.CompleteDependency();
		entityCommandBuffer.Playback(state.EntityManager);
		entityCommandBuffer.Dispose();
	}

	private static float GetWorldScale(LocalToWorld worldTransform)
	{
		float3 xyz = worldTransform.Value.c0.xyz;
		float3 xyz2 = worldTransform.Value.c1.xyz;
		float3 xyz3 = worldTransform.Value.c2.xyz;
		return (math.length(xyz) + math.length(xyz2) + math.length(xyz3)) * (1f / 3f);
	}

	private static void EmitParticle(EntityCommandBuffer.ParallelWriter cmd, int sortKey, float3 position, float scale, GlobalParticle.Emitter emitter, ref GlobalRandom random, Entity emitBufferEntity, ComponentLookup<SpellTransparentSystem.SpellTransparentMaterialOverride> transparentLookup, Entity emitterEntity)
	{
		GlobalParticleEmitParams element = new GlobalParticleEmitParams(emitter.Type, emitter.ParticleName, position)
		{
			Size = scale
		};
		element.Position.x += random.NextFloatByChunkIndex(sortKey, 0f - emitter.RandomPositionOffset, emitter.RandomPositionOffset) * scale;
		element.Position.y += random.NextFloatByChunkIndex(sortKey, 0f - emitter.RandomPositionOffset, emitter.RandomPositionOffset) * scale;
		if (transparentLookup.TryGetComponent(emitterEntity, out var componentData))
		{
			element.Alpha = componentData.Alpha;
		}
		cmd.AppendToBuffer(sortKey, emitBufferEntity, element);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private JobHandle __ScheduleViaJobChunkExtension_0(ByTimeEmitterJob job, EntityQuery query, JobHandle dependency, ref SystemState state, bool hasUserDefinedQuery)
	{
		dependency = __TypeHandle.__GlobalParticleEmitterEntitySystem_ByTimeEmitterJob_WithDefaultQuery_JobEntityTypeHandle.UpdateBaseEntityIndexArray(ref job, query, dependency, ref state);
		__TypeHandle.__GlobalParticleEmitterEntitySystem_ByTimeEmitterJob_WithDefaultQuery_JobEntityTypeHandle.AssignEntityManager(ref job, state.EntityManager);
		__TypeHandle.__GlobalParticleEmitterEntitySystem_ByTimeEmitterJob_WithDefaultQuery_JobEntityTypeHandle.__TypeHandle.Update(ref state);
		return __TypeHandle.__GlobalParticleEmitterEntitySystem_ByTimeEmitterJob_WithDefaultQuery_JobEntityTypeHandle.ScheduleParallel(ref job, query, dependency);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private JobHandle __ScheduleViaJobChunkExtension_1(ByDistanceEmitterJob job, EntityQuery query, JobHandle dependency, ref SystemState state, bool hasUserDefinedQuery)
	{
		dependency = __TypeHandle.__GlobalParticleEmitterEntitySystem_ByDistanceEmitterJob_WithDefaultQuery_JobEntityTypeHandle.UpdateBaseEntityIndexArray(ref job, query, dependency, ref state);
		__TypeHandle.__GlobalParticleEmitterEntitySystem_ByDistanceEmitterJob_WithDefaultQuery_JobEntityTypeHandle.AssignEntityManager(ref job, state.EntityManager);
		__TypeHandle.__GlobalParticleEmitterEntitySystem_ByDistanceEmitterJob_WithDefaultQuery_JobEntityTypeHandle.__TypeHandle.Update(ref state);
		return __TypeHandle.__GlobalParticleEmitterEntitySystem_ByDistanceEmitterJob_WithDefaultQuery_JobEntityTypeHandle.ScheduleParallel(ref job, query, dependency);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAllRW<GlobalRandom>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_525405871_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<GlobalParticleEmitParams>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_525405871_1 = entityQueryBuilder2.Build(ref state);
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
		((GlobalParticleEmitterEntitySystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnUpdate(IntPtr self, IntPtr state)
	{
		((GlobalParticleEmitterEntitySystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
	{
		((GlobalParticleEmitterEntitySystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
	}
}
