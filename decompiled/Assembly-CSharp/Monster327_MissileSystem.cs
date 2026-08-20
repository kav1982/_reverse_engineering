using System;
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

[CompilerGenerated]
[UpdateInGroup(typeof(UnitBaseSystemGroup))]
internal struct Monster327_MissileSystem : ISystem, ISystemCompilerGenerated
{
	[CompilerGenerated]
	public struct Monster327MissileJob : IJobEntity, IJobChunk
	{
		public struct InternalCompilerQueryAndHandleData
		{
			public struct TypeHandle
			{
				public ComponentTypeHandle<Monster327Missle_Dots> __Monster327Missle_Dots_RW_ComponentTypeHandle;

				public ComponentTypeHandle<Monster327MissileLaunch_Dots> __Monster327MissileLaunch_Dots_RW_ComponentTypeHandle;

				public ComponentTypeHandle<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentTypeHandle;

				public ComponentTypeHandle<UnitProperty_Dots> __UnitProperty_Dots_RW_ComponentTypeHandle;

				[ReadOnly]
				public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public void __AssignHandles(ref SystemState state)
				{
					__Monster327Missle_Dots_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Monster327Missle_Dots>();
					__Monster327MissileLaunch_Dots_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Monster327MissileLaunch_Dots>();
					__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle = state.GetComponentTypeHandle<LocalTransform>();
					__UnitProperty_Dots_RW_ComponentTypeHandle = state.GetComponentTypeHandle<UnitProperty_Dots>();
					__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
				}

				public void Update(ref SystemState state)
				{
					__Monster327Missle_Dots_RW_ComponentTypeHandle.Update(ref state);
					__Monster327MissileLaunch_Dots_RW_ComponentTypeHandle.Update(ref state);
					__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle.Update(ref state);
					__UnitProperty_Dots_RW_ComponentTypeHandle.Update(ref state);
					__Unity_Entities_Entity_TypeHandle.Update(ref state);
				}
			}

			public TypeHandle __TypeHandle;

			public EntityQuery DefaultQuery;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private void __AssignQueries(ref SystemState state)
			{
				EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
				EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAllRW<Monster327Missle_Dots>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Monster327MissileLaunch_Dots>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<LocalTransform>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<UnitProperty_Dots>();
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
			public void Run(ref Monster327MissileJob job, EntityQuery query)
			{
				job.__TypeHandle = __TypeHandle;
				JobChunkExtensions.RunByRef(ref job, query);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle Schedule(ref Monster327MissileJob job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = __TypeHandle;
				return JobChunkExtensions.ScheduleByRef(ref job, query, dependency);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle ScheduleParallel(ref Monster327MissileJob job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = __TypeHandle;
				return JobChunkExtensions.ScheduleParallelByRef(ref job, query, dependency);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void UpdateBaseEntityIndexArray(ref Monster327MissileJob job, EntityQuery query, ref SystemState state)
			{
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle UpdateBaseEntityIndexArray(ref Monster327MissileJob job, EntityQuery query, JobHandle dependency, ref SystemState state)
			{
				return dependency;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void AssignEntityManager(ref Monster327MissileJob job, EntityManager entityManager)
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

		[ReadOnly]
		public CurrentRoomEntitiesSingleton CurrentRoomEntities;

		public float DeltaTime;

		public NativeQueue<Monster327MissileExplosionRequest>.ParallelWriter Requests;

		public EntityCommandBuffer.ParallelWriter ecb;

		[NativeDisableUnsafePtrRestriction]
		public RefRW<GlobalRandom> random;

		public Entity SEBufferEntity;

		private InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

		public void Execute([ChunkIndexInQuery] int index, ref Monster327Missle_Dots missile, ref Monster327MissileLaunch_Dots launch, ref LocalTransform localTsf, ref UnitProperty_Dots selfPpt, Entity entity)
		{
			if (selfPpt.isDead)
			{
				return;
			}
			if (!missile.initialized)
			{
				missile.initialized = true;
				missile.lifeTimer = 0f;
				missile.straightTimer = 0f;
				missile.explosionTimer = 0f;
				missile.isExploding = false;
				missile.state = Monster327MissileState.Straight;
				missile.currentDirection = math.normalizesafe(launch.initialDirection, new float3(0f, 1f, 0f));
				missile.maxTurnAnglePerSecond.RandomResult(ref random.ValueRW.random);
			}
			if (missile.isExploding)
			{
				missile.explosionTimer += DeltaTime;
				if (missile.explosionTimer >= missile.explosionTouchDuration)
				{
					ecb.AppendToBuffer(index, SEBufferEntity, new SEData("SE_Elite56_MissileExplosion"));
					Requests.Enqueue(new Monster327MissileExplosionRequest
					{
						missileEntity = entity,
						shooter = launch.shooter,
						dontCreateDeadEF = true,
						kill = true
					});
				}
				return;
			}
			missile.lifeTimer += DeltaTime;
			if (missile.lifeTimer >= missile.lifeTime)
			{
				Requests.Enqueue(new Monster327MissileExplosionRequest
				{
					missileEntity = entity,
					shooter = launch.shooter,
					dontCreateDeadEF = false,
					kill = true
				});
				return;
			}
			float num = math.max(0.05f, selfPpt.size * 0.5f);
			bool flag = false;
			for (int i = 0; i < CurrentRoomEntities.TargetablePlayerTeamEntities.Length; i++)
			{
				UnitProperty_Dots unitProperty_Dots = CurrentRoomEntities.TargetablePlayerTeamProperties[i];
				if (!unitProperty_Dots.isDead && unitProperty_Dots.CanBeTarget)
				{
					float3 position = CurrentRoomEntities.TargetablePlayerTeamTransforms[i].Position;
					float num2 = math.max(0.05f, unitProperty_Dots.size * 0.5f);
					float num3 = num + num2;
					if (math.distancesq(localTsf.Position, position) <= num3 * num3)
					{
						flag = true;
						break;
					}
				}
			}
			if (flag)
			{
				missile.isExploding = true;
				missile.explosionTimer = 0f;
				Requests.Enqueue(new Monster327MissileExplosionRequest
				{
					missileEntity = entity,
					shooter = launch.shooter,
					position = localTsf.Position + missile.explosionOffset,
					effectScale = missile.explosionEffectScale,
					explosionColliderRadius = missile.explosionColliderRadius,
					kill = false
				});
				return;
			}
			bool flag2 = false;
			float3 targetPosition = float3.zero;
			if (launch.target != Entity.Null)
			{
				for (int j = 0; j < CurrentRoomEntities.TargetablePlayerTeamEntities.Length; j++)
				{
					if (!(CurrentRoomEntities.TargetablePlayerTeamEntities[j] != launch.target))
					{
						UnitProperty_Dots unitProperty_Dots2 = CurrentRoomEntities.TargetablePlayerTeamProperties[j];
						if (!unitProperty_Dots2.isDead && unitProperty_Dots2.CanBeTarget)
						{
							targetPosition = CurrentRoomEntities.TargetablePlayerTeamTransforms[j].Position;
							flag2 = true;
						}
						break;
					}
				}
			}
			if (!flag2 && CurrentRoomEntities.FindNearestTarget(localTsf.Position, UnitType.Monster, out var target, out targetPosition, out var _))
			{
				launch.target = target;
				flag2 = true;
			}
			switch (missile.state)
			{
			case Monster327MissileState.Straight:
				missile.straightTimer += DeltaTime;
				localTsf.Position += missile.currentDirection * missile.straightSpeed * DeltaTime;
				if (missile.straightTimer >= missile.straightTime)
				{
					missile.state = Monster327MissileState.Homing;
				}
				break;
			case Monster327MissileState.Homing:
				if (flag2)
				{
					float3 defaultvalue = math.normalizesafe(new float3(missile.currentDirection.x, missile.currentDirection.y, 0f), new float3(0f, 1f, 0f));
					float3 @float = math.normalizesafe(new float3(targetPosition.x - localTsf.Position.x, targetPosition.y - localTsf.Position.y, 0f), defaultvalue);
					float num4 = math.atan2(defaultvalue.y, defaultvalue.x);
					float x = math.atan2(@float.y, @float.x) - num4;
					x = math.atan2(math.sin(x), math.cos(x));
					float num5 = math.radians(missile.maxTurnAnglePerSecond.result * DeltaTime);
					float x2 = num4 + math.clamp(x, 0f - num5, num5);
					missile.currentDirection = new float3(math.cos(x2), math.sin(x2), 0f);
				}
				localTsf.Position += missile.currentDirection * missile.homingSpeed * DeltaTime;
				break;
			}
		}

		[CompilerGenerated]
		public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
		{
			IntPtr nativeArrayPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Monster327Missle_Dots_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr2 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Monster327MissileLaunch_Dots_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr3 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr4 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__UnitProperty_Dots_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr5 = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(chunk, __TypeHandle.__Unity_Entities_Entity_TypeHandle);
			int count = chunk.Count;
			int num = 0;
			if (!useEnabledMask)
			{
				for (int i = 0; i < count; i++)
				{
					ref Monster327Missle_Dots missile = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Monster327Missle_Dots>(nativeArrayPtr, i);
					ref Monster327MissileLaunch_Dots launch = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Monster327MissileLaunch_Dots>(nativeArrayPtr2, i);
					ref LocalTransform localTsf = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr3, i);
					ref UnitProperty_Dots selfPpt = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitProperty_Dots>(nativeArrayPtr4, i);
					Entity entity = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr5, i);
					Execute(chunkIndexInQuery, ref missile, ref launch, ref localTsf, ref selfPpt, entity);
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
						ref Monster327Missle_Dots missile2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Monster327Missle_Dots>(nativeArrayPtr, nextRangeBegin);
						ref Monster327MissileLaunch_Dots launch2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Monster327MissileLaunch_Dots>(nativeArrayPtr2, nextRangeBegin);
						ref LocalTransform localTsf2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr3, nextRangeBegin);
						ref UnitProperty_Dots selfPpt2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitProperty_Dots>(nativeArrayPtr4, nextRangeBegin);
						Entity entity2 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr5, nextRangeBegin);
						Execute(chunkIndexInQuery, ref missile2, ref launch2, ref localTsf2, ref selfPpt2, entity2);
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
					ref Monster327Missle_Dots missile3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Monster327Missle_Dots>(nativeArrayPtr, j);
					ref Monster327MissileLaunch_Dots launch3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Monster327MissileLaunch_Dots>(nativeArrayPtr2, j);
					ref LocalTransform localTsf3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr3, j);
					ref UnitProperty_Dots selfPpt3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitProperty_Dots>(nativeArrayPtr4, j);
					Entity entity3 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr5, j);
					Execute(chunkIndexInQuery, ref missile3, ref launch3, ref localTsf3, ref selfPpt3, entity3);
					num++;
				}
				num2 >>= 1;
			}
			num2 = chunkEnabledMask.ULong1;
			for (int k = 64; k < count; k++)
			{
				if ((num2 & 1) != 0L)
				{
					ref Monster327Missle_Dots missile4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Monster327Missle_Dots>(nativeArrayPtr, k);
					ref Monster327MissileLaunch_Dots launch4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Monster327MissileLaunch_Dots>(nativeArrayPtr2, k);
					ref LocalTransform localTsf4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr3, k);
					ref UnitProperty_Dots selfPpt4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitProperty_Dots>(nativeArrayPtr4, k);
					Entity entity4 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr5, k);
					Execute(chunkIndexInQuery, ref missile4, ref launch4, ref localTsf4, ref selfPpt4, entity4);
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
	public struct Monster327MissileRotateJob : IJobEntity, IJobChunk
	{
		public struct InternalCompilerQueryAndHandleData
		{
			public struct TypeHandle
			{
				[ReadOnly]
				public ComponentTypeHandle<Monster327Missle_Dots> __Monster327Missle_Dots_RO_ComponentTypeHandle;

				[ReadOnly]
				public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public void __AssignHandles(ref SystemState state)
				{
					__Monster327Missle_Dots_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Monster327Missle_Dots>(isReadOnly: true);
					__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
				}

				public void Update(ref SystemState state)
				{
					__Monster327Missle_Dots_RO_ComponentTypeHandle.Update(ref state);
					__Unity_Entities_Entity_TypeHandle.Update(ref state);
				}
			}

			public TypeHandle __TypeHandle;

			public EntityQuery DefaultQuery;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private void __AssignQueries(ref SystemState state)
			{
				EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
				DefaultQuery = entityQueryBuilder.WithAll<Monster327Missle_Dots>().Build(ref state);
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
			public void Run(ref Monster327MissileRotateJob job, EntityQuery query)
			{
				job.__TypeHandle = __TypeHandle;
				JobChunkExtensions.RunByRef(ref job, query);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle Schedule(ref Monster327MissileRotateJob job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = __TypeHandle;
				return JobChunkExtensions.ScheduleByRef(ref job, query, dependency);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle ScheduleParallel(ref Monster327MissileRotateJob job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = __TypeHandle;
				return JobChunkExtensions.ScheduleParallelByRef(ref job, query, dependency);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void UpdateBaseEntityIndexArray(ref Monster327MissileRotateJob job, EntityQuery query, ref SystemState state)
			{
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle UpdateBaseEntityIndexArray(ref Monster327MissileRotateJob job, EntityQuery query, JobHandle dependency, ref SystemState state)
			{
				return dependency;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void AssignEntityManager(ref Monster327MissileRotateJob job, EntityManager entityManager)
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

		[NativeDisableParallelForRestriction]
		public ComponentLookup<LocalTransform> TransformLookup;

		[ReadOnly]
		public ComponentLookup<UnitProperty_Dots> UnitPropertyLookup;

		private InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

		public void Execute(in Monster327Missle_Dots missile, Entity entity)
		{
			if (!(missile.rotateRoot == Entity.Null) && !(missile.rotateShadow == Entity.Null) && TransformLookup.HasComponent(missile.rotateRoot) && TransformLookup.HasComponent(missile.rotateShadow) && (!UnitPropertyLookup.HasComponent(entity) || !UnitPropertyLookup[entity].isDead))
			{
				TransformLookup.GetRefRW(missile.rotateRoot).ValueRW.Rotation = quaternion.LookRotationSafe(math.forward(), missile.currentDirection);
				TransformLookup.GetRefRW(missile.rotateShadow).ValueRW.Rotation = quaternion.LookRotationSafe(math.forward(), missile.currentDirection);
			}
		}

		[CompilerGenerated]
		public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
		{
			IntPtr nativeArrayPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr(chunk, ref __TypeHandle.__Monster327Missle_Dots_RO_ComponentTypeHandle);
			IntPtr nativeArrayPtr2 = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(chunk, __TypeHandle.__Unity_Entities_Entity_TypeHandle);
			int count = chunk.Count;
			int num = 0;
			if (!useEnabledMask)
			{
				for (int i = 0; i < count; i++)
				{
					ref Monster327Missle_Dots missile = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Monster327Missle_Dots>(nativeArrayPtr, i);
					Entity entity = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr2, i);
					Execute(in missile, entity);
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
						ref Monster327Missle_Dots missile2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Monster327Missle_Dots>(nativeArrayPtr, nextRangeBegin);
						Entity entity2 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr2, nextRangeBegin);
						Execute(in missile2, entity2);
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
					ref Monster327Missle_Dots missile3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Monster327Missle_Dots>(nativeArrayPtr, j);
					Entity entity3 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr2, j);
					Execute(in missile3, entity3);
					num++;
				}
				num2 >>= 1;
			}
			num2 = chunkEnabledMask.ULong1;
			for (int k = 64; k < count; k++)
			{
				if ((num2 & 1) != 0L)
				{
					ref Monster327Missle_Dots missile4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Monster327Missle_Dots>(nativeArrayPtr, k);
					Entity entity4 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr2, k);
					Execute(in missile4, entity4);
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
		public Monster327MissileJob.InternalCompilerQueryAndHandleData __Monster327_MissileSystem_Monster327MissileJob_WithDefaultQuery_JobEntityTypeHandle;

		public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<UnitProperty_Dots> __UnitProperty_Dots_RO_ComponentLookup;

		public Monster327MissileRotateJob.InternalCompilerQueryAndHandleData __Monster327_MissileSystem_Monster327MissileRotateJob_WithDefaultQuery_JobEntityTypeHandle;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__Monster327_MissileSystem_Monster327MissileJob_WithDefaultQuery_JobEntityTypeHandle.Init(ref state, assignDefaultQuery: true);
			__Unity_Transforms_LocalTransform_RW_ComponentLookup = state.GetComponentLookup<LocalTransform>();
			__UnitProperty_Dots_RO_ComponentLookup = state.GetComponentLookup<UnitProperty_Dots>(isReadOnly: true);
			__Monster327_MissileSystem_Monster327MissileRotateJob_WithDefaultQuery_JobEntityTypeHandle.Init(ref state, assignDefaultQuery: true);
		}
	}

	private TypeHandle __TypeHandle;

	private EntityQuery __query_198389241_0;

	private EntityQuery __query_198389241_1;

	private EntityQuery __query_198389241_2;

	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<CurrentRoomEntitiesSingleton>();
		state.RequireForUpdate<Monster327Missle_Dots>();
	}

	public unsafe void OnUpdate(ref SystemState state)
	{
		NativeQueue<Monster327MissileExplosionRequest> nativeQueue = new NativeQueue<Monster327MissileExplosionRequest>(Allocator.TempJob);
		EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer(Allocator.TempJob);
		state.Dependency = __ScheduleViaJobChunkExtension_0(new Monster327MissileJob
		{
			CurrentRoomEntities = __query_198389241_0.GetSingleton<CurrentRoomEntitiesSingleton>(),
			DeltaTime = state.WorldUnmanaged.Time.DeltaTime,
			Requests = nativeQueue.AsParallelWriter(),
			random = __query_198389241_1.GetSingletonRW<GlobalRandom>(),
			ecb = entityCommandBuffer.AsParallelWriter(),
			SEBufferEntity = __query_198389241_2.GetSingletonEntity()
		}, __TypeHandle.__Monster327_MissileSystem_Monster327MissileJob_WithDefaultQuery_JobEntityTypeHandle.DefaultQuery, state.Dependency, ref state, hasUserDefinedQuery: false);
		state.Dependency = __ScheduleViaJobChunkExtension_1(new Monster327MissileRotateJob
		{
			TransformLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state),
			UnitPropertyLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__UnitProperty_Dots_RO_ComponentLookup, ref state)
		}, __TypeHandle.__Monster327_MissileSystem_Monster327MissileRotateJob_WithDefaultQuery_JobEntityTypeHandle.DefaultQuery, state.Dependency, ref state, hasUserDefinedQuery: false);
		state.Dependency.Complete();
		entityCommandBuffer.Playback(state.EntityManager);
		entityCommandBuffer.Dispose();
		Monster327MissileExplosionRequest item;
		while (nativeQueue.TryDequeue(out item))
		{
			if (!state.EntityManager.HasComponent<UnitProperty_Dots>(item.missileEntity))
			{
				continue;
			}
			if (item.kill)
			{
				UnitProperty_Dots componentData = state.EntityManager.GetComponentData<UnitProperty_Dots>(item.missileEntity);
				if (componentData.isDead)
				{
					continue;
				}
				TakeDamageInfo_Dots takeDamageInfo_Dots = TakeDamageInfo_Dots.NewInfo(item.shooter);
				takeDamageInfo_Dots.isTargetDead = true;
				takeDamageInfo_Dots.dontCreateDeadEF = item.dontCreateDeadEF;
				if (componentData.unitCfg.triggerDeadEvent)
				{
					takeDamageInfo_Dots.isTriggerDeadEvent = true;
				}
				if (state.EntityManager.HasBuffer<TakeDamageInfo_Dots>(item.missileEntity))
				{
					DynamicBuffer<TakeDamageInfo_Dots> buffer = state.EntityManager.GetBuffer<TakeDamageInfo_Dots>(item.missileEntity);
					buffer.Add(takeDamageInfo_Dots);
					if (state.EntityManager.HasComponent<UnitDead>(item.missileEntity))
					{
						UnitDead componentData2 = state.EntityManager.GetComponentData<UnitDead>(item.missileEntity);
						componentData2.deadlyInfo = takeDamageInfo_Dots;
						componentData2.ignoreBeforeAnnouncedDeath = true;
						componentData2.deadlyInfoIndex = buffer.Length - 1;
						state.EntityManager.SetComponentData(item.missileEntity, componentData2);
					}
				}
				componentData.isDead = true;
				state.EntityManager.SetComponentData(item.missileEntity, componentData);
				continue;
			}
			if (item.explosionColliderRadius > 0f && state.EntityManager.HasComponent<PhysicsCollider>(item.missileEntity))
			{
				PhysicsCollider collider = state.EntityManager.GetComponentData<PhysicsCollider>(item.missileEntity);
				collider.MakeUnique(in item.missileEntity, state.EntityManager);
				if (collider.ColliderPtr->Type == ColliderType.Capsule)
				{
					Unity.Physics.CapsuleCollider* colliderPtr = (Unity.Physics.CapsuleCollider*)collider.ColliderPtr;
					CapsuleGeometry geometry = colliderPtr->Geometry;
					geometry.Radius = item.explosionColliderRadius;
					colliderPtr->Geometry = geometry;
				}
				else if (collider.ColliderPtr->Type == ColliderType.Compound)
				{
					CompoundCollider* colliderPtr2 = (CompoundCollider*)collider.ColliderPtr;
					for (int i = 0; i < colliderPtr2->NumChildren; i++)
					{
						Unity.Physics.Collider* collider2 = colliderPtr2->Children[i].Collider;
						if (collider2->Type == ColliderType.Capsule)
						{
							Unity.Physics.CapsuleCollider* ptr = (Unity.Physics.CapsuleCollider*)collider2;
							CapsuleGeometry geometry2 = ptr->Geometry;
							geometry2.Radius = item.explosionColliderRadius;
							ptr->Geometry = geometry2;
						}
					}
				}
				state.EntityManager.SetComponentData(item.missileEntity, collider);
			}
			Vector3 vector = Vector3.one * item.effectScale;
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Monster327_Explosion", item.position, Quaternion.identity, vector, 3f).transform.localScale = vector;
		}
		nativeQueue.Dispose();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private JobHandle __ScheduleViaJobChunkExtension_0(Monster327MissileJob job, EntityQuery query, JobHandle dependency, ref SystemState state, bool hasUserDefinedQuery)
	{
		dependency = __TypeHandle.__Monster327_MissileSystem_Monster327MissileJob_WithDefaultQuery_JobEntityTypeHandle.UpdateBaseEntityIndexArray(ref job, query, dependency, ref state);
		__TypeHandle.__Monster327_MissileSystem_Monster327MissileJob_WithDefaultQuery_JobEntityTypeHandle.AssignEntityManager(ref job, state.EntityManager);
		__TypeHandle.__Monster327_MissileSystem_Monster327MissileJob_WithDefaultQuery_JobEntityTypeHandle.__TypeHandle.Update(ref state);
		return __TypeHandle.__Monster327_MissileSystem_Monster327MissileJob_WithDefaultQuery_JobEntityTypeHandle.ScheduleParallel(ref job, query, dependency);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private JobHandle __ScheduleViaJobChunkExtension_1(Monster327MissileRotateJob job, EntityQuery query, JobHandle dependency, ref SystemState state, bool hasUserDefinedQuery)
	{
		dependency = __TypeHandle.__Monster327_MissileSystem_Monster327MissileRotateJob_WithDefaultQuery_JobEntityTypeHandle.UpdateBaseEntityIndexArray(ref job, query, dependency, ref state);
		__TypeHandle.__Monster327_MissileSystem_Monster327MissileRotateJob_WithDefaultQuery_JobEntityTypeHandle.AssignEntityManager(ref job, state.EntityManager);
		__TypeHandle.__Monster327_MissileSystem_Monster327MissileRotateJob_WithDefaultQuery_JobEntityTypeHandle.__TypeHandle.Update(ref state);
		return __TypeHandle.__Monster327_MissileSystem_Monster327MissileRotateJob_WithDefaultQuery_JobEntityTypeHandle.ScheduleParallel(ref job, query, dependency);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<CurrentRoomEntitiesSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_198389241_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAllRW<GlobalRandom>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_198389241_1 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SEData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_198389241_2 = entityQueryBuilder2.Build(ref state);
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
		((Monster327_MissileSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnUpdate(IntPtr self, IntPtr state)
	{
		((Monster327_MissileSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
	{
		((Monster327_MissileSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
	}
}
