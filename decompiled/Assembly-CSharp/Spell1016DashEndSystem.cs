using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Entities.Internal;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

[UpdateInGroup(typeof(SpellEndSystemGroup))]
[UpdateBefore(typeof(SpellDestroySystem))]
[CompilerGenerated]
public struct Spell1016DashEndSystem : ISystem, ISystemCompilerGenerated
{
	[BurstCompile]
	[WithAll(new Type[]
	{
		typeof(SpellDestroyTag),
		typeof(Spell1016DashData)
	})]
	[CompilerGenerated]
	public struct DashEndDamageJob : IJobEntity, IJobChunk
	{
		public struct InternalCompilerQueryAndHandleData
		{
			public struct TypeHandle
			{
				public SpellAspect.TypeHandle __SpellAspect_RW_AspectTypeHandle;

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public void __AssignHandles(ref SystemState state)
				{
					__SpellAspect_RW_AspectTypeHandle = new SpellAspect.TypeHandle(ref state);
				}

				public void Update(ref SystemState state)
				{
					__SpellAspect_RW_AspectTypeHandle.Update(ref state);
				}
			}

			public TypeHandle __TypeHandle;

			public EntityQuery DefaultQuery;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private void __AssignQueries(ref SystemState state)
			{
				EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
				EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellDestroyTag>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAll<Spell1016DashData>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAspect<SpellAspect>();
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
			public void Run(ref DashEndDamageJob job, EntityQuery query)
			{
				job.__TypeHandle = __TypeHandle;
				JobChunkExtensions.RunByRef(ref job, query);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle Schedule(ref DashEndDamageJob job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = __TypeHandle;
				return JobChunkExtensions.ScheduleByRef(ref job, query, dependency);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle ScheduleParallel(ref DashEndDamageJob job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = __TypeHandle;
				return JobChunkExtensions.ScheduleParallelByRef(ref job, query, dependency);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void UpdateBaseEntityIndexArray(ref DashEndDamageJob job, EntityQuery query, ref SystemState state)
			{
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle UpdateBaseEntityIndexArray(ref DashEndDamageJob job, EntityQuery query, JobHandle dependency, ref SystemState state)
			{
				return dependency;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void AssignEntityManager(ref DashEndDamageJob job, EntityManager entityManager)
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

		[NativeDisableContainerSafetyRestriction]
		[NativeDisableParallelForRestriction]
		public ComponentLookup<UnitProperty_Dots> UnitPropertyLookup;

		[NativeDisableParallelForRestriction]
		[NativeDisableContainerSafetyRestriction]
		public ComponentLookup<SpellConfigComponentData> SpellConfigLookup;

		[NativeDisableContainerSafetyRestriction]
		[NativeDisableParallelForRestriction]
		public ComponentLookup<LocalTransform> TransformLookup;

		[ReadOnly]
		public PhysicsWorldSingleton PhysicsWorld;

		public Entity GlobalParticleEntity;

		public Entity ScreenShakeSingleton;

		private InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

		[BurstCompile]
		private void Execute(SpellAspect spellAspect, [ChunkIndexInQuery] int chunkIndex)
		{
			if (spellAspect.Config.ValueRO.Level <= 1)
			{
				return;
			}
			float radius = spellAspect.Config.ValueRO.Radius.Calculate() * spellAspect.Config.ValueRO.Float1;
			NativeList<Entity> entities = new NativeList<Entity>(Allocator.Temp);
			SpellTools.GetAttackableEntitiesInRange(in spellAspect.Transform.ValueRO.Position, in radius, in spellAspect.Config.ValueRO.ShooterType, containsBrittleness: true, in UnitPropertyLookup, in SpellConfigLookup, in PhysicsWorld, ref entities);
			TakeDamageInfo_Dots damage = spellAspect.MakeDamageInfo(costPenetrate: false);
			foreach (Entity item in entities)
			{
				Entity target = item;
				float3 position = TransformLookup[target].Position;
				damage.spell.HitPosition = position;
				damage.spell.IgnoreHitEffect = true;
				CMD.TryAttackEntity(chunkIndex, in target, in damage, in UnitPropertyLookup, in SpellConfigLookup);
			}
			spellAspect.Config.ValueRO.ColorType.ColorEnumToString(out var result);
			CMD.AppendToBuffer(chunkIndex, GlobalParticleEntity, new GlobalParticleEmitParams
			{
				Position = spellAspect.Transform.ValueRO.Position,
				Size = radius,
				Name = $"1016_Fall_{result}"
			});
			CMD.AppendToBuffer(chunkIndex, ScreenShakeSingleton, new ScreenShakeData
			{
				Radius = 0.1f,
				Speed = 1f,
				Time = 0.08f
			});
			CollisionFilter @default = CollisionFilter.Default;
			@default.CollidesWith = 8388608u;
			NativeList<DistanceHit> outHits = new NativeList<DistanceHit>(Allocator.Temp);
			PhysicsWorld.OverlapSphere(spellAspect.Transform.ValueRO.Position, radius, ref outHits, @default);
			foreach (DistanceHit item2 in outHits)
			{
				if (SpellConfigLookup.TryGetComponent(item2.Entity, out var componentData) && componentData.AbilityType != SpellAbilityType.Dash && !DTool.IsSameCamp(spellAspect.Config.ValueRO.ShooterType, componentData.ShooterType))
				{
					CMD.SetComponentEnabled<SpellDestroyTag>(chunkIndex, item2.Entity, value: true);
				}
			}
		}

		[CompilerGenerated]
		public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
		{
			SpellAspect.ResolvedChunk resolvedChunk = __TypeHandle.__SpellAspect_RW_AspectTypeHandle.Resolve(chunk);
			int count = chunk.Count;
			int num = 0;
			if (!useEnabledMask)
			{
				for (int i = 0; i < count; i++)
				{
					SpellAspect spellAspect = resolvedChunk[i];
					Execute(spellAspect, chunkIndexInQuery);
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
						SpellAspect spellAspect2 = resolvedChunk[nextRangeBegin];
						Execute(spellAspect2, chunkIndexInQuery);
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
					SpellAspect spellAspect3 = resolvedChunk[j];
					Execute(spellAspect3, chunkIndexInQuery);
					num++;
				}
				num2 >>= 1;
			}
			num2 = chunkEnabledMask.ULong1;
			for (int k = 64; k < count; k++)
			{
				if ((num2 & 1) != 0L)
				{
					SpellAspect spellAspect4 = resolvedChunk[k];
					Execute(spellAspect4, chunkIndexInQuery);
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
		public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentLookup;

		public ComponentLookup<UnitProperty_Dots> __UnitProperty_Dots_RW_ComponentLookup;

		public ComponentLookup<SpellConfigComponentData> __SpellConfigComponentData_RW_ComponentLookup;

		public DashEndDamageJob.InternalCompilerQueryAndHandleData __Spell1016DashEndSystem_DashEndDamageJob_WithDefaultQuery_JobEntityTypeHandle;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__Unity_Transforms_LocalTransform_RW_ComponentLookup = state.GetComponentLookup<LocalTransform>();
			__UnitProperty_Dots_RW_ComponentLookup = state.GetComponentLookup<UnitProperty_Dots>();
			__SpellConfigComponentData_RW_ComponentLookup = state.GetComponentLookup<SpellConfigComponentData>();
			__Spell1016DashEndSystem_DashEndDamageJob_WithDefaultQuery_JobEntityTypeHandle.Init(ref state, assignDefaultQuery: true);
		}
	}

	private TypeHandle __TypeHandle;

	private EntityQuery __query_538090989_0;

	private EntityQuery __query_538090989_1;

	private EntityQuery __query_538090989_2;

	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<GlobalParticleEmitParams>();
		state.RequireForUpdate<ScreenShakeData>();
		state.RequireForUpdate<PhysicsWorldSingleton>();
		state.RequireForUpdate<EndSpellSimulationEntityCommandBufferSystem.Singleton>();
		state.RequireForUpdate<SpellEffectSystem.UnfollowingRequire>();
		state.RequireForUpdate<SpellDashDriverSingleton>();
		state.RequireForUpdate<PlayerController_Dots>();
		state.RequireForUpdate<Spell1016DashData>();
	}

	public void OnUpdate(ref SystemState state)
	{
		Entity singletonEntity = __query_538090989_0.GetSingletonEntity();
		EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer(Allocator.TempJob);
		__ScheduleViaJobChunkExtension_0(new DashEndDamageJob
		{
			CMD = entityCommandBuffer.AsParallelWriter(),
			PhysicsWorld = __query_538090989_1.GetSingleton<PhysicsWorldSingleton>(),
			TransformLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state),
			UnitPropertyLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__UnitProperty_Dots_RW_ComponentLookup, ref state),
			SpellConfigLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__SpellConfigComponentData_RW_ComponentLookup, ref state),
			GlobalParticleEntity = __query_538090989_2.GetSingletonEntity(),
			ScreenShakeSingleton = singletonEntity
		}, __TypeHandle.__Spell1016DashEndSystem_DashEndDamageJob_WithDefaultQuery_JobEntityTypeHandle.DefaultQuery, state.Dependency, ref state, hasUserDefinedQuery: false).Complete();
		entityCommandBuffer.Playback(state.EntityManager);
		entityCommandBuffer.Dispose();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private JobHandle __ScheduleViaJobChunkExtension_0(DashEndDamageJob job, EntityQuery query, JobHandle dependency, ref SystemState state, bool hasUserDefinedQuery)
	{
		dependency = __TypeHandle.__Spell1016DashEndSystem_DashEndDamageJob_WithDefaultQuery_JobEntityTypeHandle.UpdateBaseEntityIndexArray(ref job, query, dependency, ref state);
		__TypeHandle.__Spell1016DashEndSystem_DashEndDamageJob_WithDefaultQuery_JobEntityTypeHandle.AssignEntityManager(ref job, state.EntityManager);
		__TypeHandle.__Spell1016DashEndSystem_DashEndDamageJob_WithDefaultQuery_JobEntityTypeHandle.__TypeHandle.Update(ref state);
		return __TypeHandle.__Spell1016DashEndSystem_DashEndDamageJob_WithDefaultQuery_JobEntityTypeHandle.ScheduleParallel(ref job, query, dependency);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<ScreenShakeData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_538090989_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<PhysicsWorldSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_538090989_1 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<GlobalParticleEmitParams>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_538090989_2 = entityQueryBuilder2.Build(ref state);
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
		((Spell1016DashEndSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnUpdate(IntPtr self, IntPtr state)
	{
		((Spell1016DashEndSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
	{
		((Spell1016DashEndSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
	}
}
