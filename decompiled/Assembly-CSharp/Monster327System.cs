using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Internal;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

[UpdateInGroup(typeof(UnitBaseSystemGroup))]
[CompilerGenerated]
internal struct Monster327System : ISystem, ISystemCompilerGenerated
{
	[CompilerGenerated]
	public struct Monster327Job : IJobEntity, IJobChunk
	{
		public struct InternalCompilerQueryAndHandleData
		{
			public struct TypeHandle
			{
				public ComponentTypeHandle<Monster327_Dots> __Monster327_Dots_RW_ComponentTypeHandle;

				public ComponentTypeHandle<UnitBase_Dots> __UnitBase_Dots_RW_ComponentTypeHandle;

				[ReadOnly]
				public ComponentTypeHandle<LocalTransform> __Unity_Transforms_LocalTransform_RO_ComponentTypeHandle;

				[ReadOnly]
				public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public void __AssignHandles(ref SystemState state)
				{
					__Monster327_Dots_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Monster327_Dots>();
					__UnitBase_Dots_RW_ComponentTypeHandle = state.GetComponentTypeHandle<UnitBase_Dots>();
					__Unity_Transforms_LocalTransform_RO_ComponentTypeHandle = state.GetComponentTypeHandle<LocalTransform>(isReadOnly: true);
					__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
				}

				public void Update(ref SystemState state)
				{
					__Monster327_Dots_RW_ComponentTypeHandle.Update(ref state);
					__UnitBase_Dots_RW_ComponentTypeHandle.Update(ref state);
					__Unity_Transforms_LocalTransform_RO_ComponentTypeHandle.Update(ref state);
					__Unity_Entities_Entity_TypeHandle.Update(ref state);
				}
			}

			public TypeHandle __TypeHandle;

			public EntityQuery DefaultQuery;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private void __AssignQueries(ref SystemState state)
			{
				EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
				EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<LocalTransform>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Monster327_Dots>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<UnitBase_Dots>();
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
			public void Run(ref Monster327Job job, EntityQuery query)
			{
				job.__TypeHandle = __TypeHandle;
				JobChunkExtensions.RunByRef(ref job, query);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle Schedule(ref Monster327Job job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = __TypeHandle;
				return JobChunkExtensions.ScheduleByRef(ref job, query, dependency);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle ScheduleParallel(ref Monster327Job job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = __TypeHandle;
				return JobChunkExtensions.ScheduleParallelByRef(ref job, query, dependency);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void UpdateBaseEntityIndexArray(ref Monster327Job job, EntityQuery query, ref SystemState state)
			{
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle UpdateBaseEntityIndexArray(ref Monster327Job job, EntityQuery query, JobHandle dependency, ref SystemState state)
			{
				return dependency;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void AssignEntityManager(ref Monster327Job job, EntityManager entityManager)
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

		[ReadOnly]
		public ComponentLookup<LocalToWorld> LocalToWorldLookup;

		public float DeltaTime;

		public EntityCommandBuffer.ParallelWriter Ecb;

		public Entity SEBufferEntity;

		[NativeDisableParallelForRestriction]
		public ComponentLookup<AnimaPlay> AnimaLookUp;

		private InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

		public void Execute([ChunkIndexInQuery] int sortKey, ref Monster327_Dots launcher, ref UnitBase_Dots unitBase, in LocalTransform localTsf, Entity entity)
		{
			unitBase.SetMove(float3.zero);
			ref AnimaPlay valueRW = ref AnimaLookUp.GetRefRW(unitBase.ett_AnimaRoot).ValueRW;
			if (launcher.isFiringVolley)
			{
				launcher.turretDirection = math.normalizesafe(launcher.lockedFireDirection, new float3(0f, 1f, 0f));
				UpdateVolley(sortKey, ref launcher, localTsf.Position, entity, ref valueRW);
				return;
			}
			if (launcher.isAfterVolleyLocked)
			{
				launcher.turretDirection = math.normalizesafe(launcher.lockedFireDirection, new float3(0f, 1f, 0f));
				launcher.afterVolleyLockTimer += DeltaTime;
				if (launcher.afterVolleyLockTimer >= launcher.afterVolleyLockTime)
				{
					launcher.isAfterVolleyLocked = false;
					launcher.afterVolleyLockTimer = 0f;
					launcher.volleyTarget = Entity.Null;
					launcher.fireTimer = 0f;
				}
				return;
			}
			Entity target;
			float3 targetPosition;
			UnitProperty_Dots targetPpt;
			bool flag = CurrentRoomEntities.FindNearestTarget(localTsf.Position, UnitType.Monster, out target, out targetPosition, out targetPpt);
			if (flag)
			{
				float3 desiredDirection = math.normalizesafe(new float3(targetPosition.x - localTsf.Position.x, targetPosition.y - localTsf.Position.y, 0f), launcher.turretDirection);
				launcher.turretDirection = RotateDirectionTowards(launcher.turretDirection, desiredDirection, launcher.turretRotateSpeed * DeltaTime);
			}
			launcher.fireTimer += DeltaTime;
			if (!(launcher.fireTimer < launcher.fireInterval) && flag && !(launcher.missilePrefab == Entity.Null) && IsAimedAtTarget(launcher.turretDirection, localTsf.Position, targetPosition, launcher.maxFireAngleError))
			{
				launcher.isFiringVolley = true;
				launcher.missilesFiredInVolley = 0;
				launcher.nextTubeIndex = 0;
				launcher.missileFireTimer = launcher.missileFireInterval;
				launcher.volleyTarget = target;
				launcher.lockedFireDirection = math.normalizesafe(launcher.turretDirection, new float3(0f, 1f, 0f));
				launcher.turretDirection = launcher.lockedFireDirection;
				UpdateVolley(sortKey, ref launcher, localTsf.Position, entity, ref valueRW);
			}
		}

		private void UpdateVolley(int sortKey, ref Monster327_Dots launcher, float3 launcherPosition, Entity shooterEntity, ref AnimaPlay anima)
		{
			int num = math.max(1, launcher.missilesPerVolley);
			if (launcher.missilesFiredInVolley >= num)
			{
				FinishVolley(ref launcher, lockAfterVolley: true);
				return;
			}
			launcher.missileFireTimer += DeltaTime;
			if (!(launcher.missileFireTimer < launcher.missileFireInterval))
			{
				Entity entity = GetValidVolleyTargetOrNull(launcher.volleyTarget);
				if (entity == Entity.Null && CurrentRoomEntities.FindNearestTarget(launcherPosition, UnitType.Monster, out var target, out var _, out var _))
				{
					entity = target;
					launcher.volleyTarget = target;
				}
				float3 initialDirection = math.normalizesafe(launcher.lockedFireDirection, new float3(0f, 1f, 0f));
				Entity muzzleEntity = ((launcher.nextTubeIndex == 0) ? launcher.leftMuzzle : launcher.rightMuzzle);
				float3 muzzleSpawnPosition = GetMuzzleSpawnPosition(muzzleEntity, launcherPosition, launcher.missileSpawnYOffset);
				SpawnMissile(sortKey, Ecb, launcher.missilePrefab, muzzleSpawnPosition, initialDirection, entity, shooterEntity);
				Ecb.AppendToBuffer(sortKey, SEBufferEntity, new SEData("SE_Elite51_MissileMove"));
				if (launcher.nextTubeIndex == 0)
				{
					anima.Play(1);
				}
				else
				{
					anima.Play(2);
				}
				launcher.missilesFiredInVolley++;
				launcher.nextTubeIndex = ((launcher.nextTubeIndex == 0) ? 1 : 0);
				launcher.missileFireTimer = 0f;
				if (launcher.missilesFiredInVolley >= num)
				{
					FinishVolley(ref launcher, lockAfterVolley: true);
				}
			}
		}

		private float3 GetMuzzleSpawnPosition(Entity muzzleEntity, float3 fallbackPosition, float missileSpawnYOffset)
		{
			float3 result = fallbackPosition;
			if (muzzleEntity != Entity.Null && LocalToWorldLookup.HasComponent(muzzleEntity))
			{
				result = LocalToWorldLookup[muzzleEntity].Position;
			}
			result.y += missileSpawnYOffset;
			return result;
		}

		private static void FinishVolley(ref Monster327_Dots launcher, bool lockAfterVolley)
		{
			launcher.isFiringVolley = false;
			launcher.missilesFiredInVolley = 0;
			launcher.nextTubeIndex = 0;
			launcher.missileFireTimer = 0f;
			launcher.fireTimer = 0f;
			launcher.afterVolleyLockTimer = 0f;
			if (lockAfterVolley && launcher.afterVolleyLockTime > 0f)
			{
				launcher.isAfterVolleyLocked = true;
				return;
			}
			launcher.isAfterVolleyLocked = false;
			launcher.volleyTarget = Entity.Null;
		}

		private Entity GetValidVolleyTargetOrNull(Entity targetEntity)
		{
			if (targetEntity == Entity.Null)
			{
				return Entity.Null;
			}
			for (int i = 0; i < CurrentRoomEntities.TargetablePlayerTeamEntities.Length; i++)
			{
				if (!(CurrentRoomEntities.TargetablePlayerTeamEntities[i] != targetEntity))
				{
					UnitProperty_Dots unitProperty_Dots = CurrentRoomEntities.TargetablePlayerTeamProperties[i];
					if (unitProperty_Dots.isDead || !unitProperty_Dots.CanBeTarget)
					{
						return Entity.Null;
					}
					return targetEntity;
				}
			}
			return Entity.Null;
		}

		private static bool IsAimedAtTarget(float3 currentDirection, float3 launcherPosition, float3 targetPosition, float maxFireAngleError)
		{
			currentDirection = math.normalizesafe(new float3(currentDirection.x, currentDirection.y, 0f), new float3(0f, 1f, 0f));
			float3 y = math.normalizesafe(new float3(targetPosition.x - launcherPosition.x, targetPosition.y - launcherPosition.y, 0f), currentDirection);
			float num = math.cos(math.radians(math.clamp(maxFireAngleError, 0f, 180f)));
			return math.dot(currentDirection, y) >= num;
		}

		private static float3 RotateDirectionTowards(float3 currentDirection, float3 desiredDirection, float maxDeltaDegree)
		{
			currentDirection = math.normalizesafe(new float3(currentDirection.x, currentDirection.y, 0f), new float3(0f, 1f, 0f));
			desiredDirection = math.normalizesafe(new float3(desiredDirection.x, desiredDirection.y, 0f), currentDirection);
			float num = math.atan2(currentDirection.y, currentDirection.x);
			float x = math.atan2(desiredDirection.y, desiredDirection.x) - num;
			x = math.atan2(math.sin(x), math.cos(x));
			float num2 = math.radians(math.max(0f, maxDeltaDegree));
			float x2 = num + math.clamp(x, 0f - num2, num2);
			return new float3(math.cos(x2), math.sin(x2), 0f);
		}

		private static void SpawnMissile(int sortKey, EntityCommandBuffer.ParallelWriter ecb, Entity missilePrefab, float3 spawnPosition, float3 initialDirection, Entity targetEntity, Entity shooterEntity)
		{
			initialDirection = math.normalizesafe(initialDirection, new float3(0f, 1f, 0f));
			Entity e = ecb.Instantiate(sortKey, missilePrefab);
			ecb.SetComponent(sortKey, e, new LocalTransform
			{
				Position = spawnPosition,
				Rotation = quaternion.identity,
				Scale = 1f
			});
			ecb.SetComponent(sortKey, e, new Monster327MissileLaunch_Dots
			{
				initialDirection = initialDirection,
				target = targetEntity,
				shooter = shooterEntity
			});
		}

		[CompilerGenerated]
		public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
		{
			IntPtr nativeArrayPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Monster327_Dots_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr2 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__UnitBase_Dots_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr3 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr(chunk, ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentTypeHandle);
			IntPtr nativeArrayPtr4 = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(chunk, __TypeHandle.__Unity_Entities_Entity_TypeHandle);
			int count = chunk.Count;
			int num = 0;
			if (!useEnabledMask)
			{
				for (int i = 0; i < count; i++)
				{
					ref Monster327_Dots launcher = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Monster327_Dots>(nativeArrayPtr, i);
					ref UnitBase_Dots unitBase = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitBase_Dots>(nativeArrayPtr2, i);
					ref LocalTransform localTsf = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr3, i);
					Entity entity = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr4, i);
					Execute(chunkIndexInQuery, ref launcher, ref unitBase, in localTsf, entity);
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
						ref Monster327_Dots launcher2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Monster327_Dots>(nativeArrayPtr, nextRangeBegin);
						ref UnitBase_Dots unitBase2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitBase_Dots>(nativeArrayPtr2, nextRangeBegin);
						ref LocalTransform localTsf2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr3, nextRangeBegin);
						Entity entity2 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr4, nextRangeBegin);
						Execute(chunkIndexInQuery, ref launcher2, ref unitBase2, in localTsf2, entity2);
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
					ref Monster327_Dots launcher3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Monster327_Dots>(nativeArrayPtr, j);
					ref UnitBase_Dots unitBase3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitBase_Dots>(nativeArrayPtr2, j);
					ref LocalTransform localTsf3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr3, j);
					Entity entity3 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr4, j);
					Execute(chunkIndexInQuery, ref launcher3, ref unitBase3, in localTsf3, entity3);
					num++;
				}
				num2 >>= 1;
			}
			num2 = chunkEnabledMask.ULong1;
			for (int k = 64; k < count; k++)
			{
				if ((num2 & 1) != 0L)
				{
					ref Monster327_Dots launcher4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Monster327_Dots>(nativeArrayPtr, k);
					ref UnitBase_Dots unitBase4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitBase_Dots>(nativeArrayPtr2, k);
					ref LocalTransform localTsf4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr3, k);
					Entity entity4 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr4, k);
					Execute(chunkIndexInQuery, ref launcher4, ref unitBase4, in localTsf4, entity4);
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
	public struct Monster327TurretRotateJob : IJobEntity, IJobChunk
	{
		public struct InternalCompilerQueryAndHandleData
		{
			public struct TypeHandle
			{
				[ReadOnly]
				public ComponentTypeHandle<Monster327_Dots> __Monster327_Dots_RO_ComponentTypeHandle;

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public void __AssignHandles(ref SystemState state)
				{
					__Monster327_Dots_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Monster327_Dots>(isReadOnly: true);
				}

				public void Update(ref SystemState state)
				{
					__Monster327_Dots_RO_ComponentTypeHandle.Update(ref state);
				}
			}

			public TypeHandle __TypeHandle;

			public EntityQuery DefaultQuery;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private void __AssignQueries(ref SystemState state)
			{
				EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
				DefaultQuery = entityQueryBuilder.WithAll<Monster327_Dots>().Build(ref state);
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
			public void Run(ref Monster327TurretRotateJob job, EntityQuery query)
			{
				job.__TypeHandle = __TypeHandle;
				JobChunkExtensions.RunByRef(ref job, query);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle Schedule(ref Monster327TurretRotateJob job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = __TypeHandle;
				return JobChunkExtensions.ScheduleByRef(ref job, query, dependency);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle ScheduleParallel(ref Monster327TurretRotateJob job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = __TypeHandle;
				return JobChunkExtensions.ScheduleParallelByRef(ref job, query, dependency);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void UpdateBaseEntityIndexArray(ref Monster327TurretRotateJob job, EntityQuery query, ref SystemState state)
			{
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle UpdateBaseEntityIndexArray(ref Monster327TurretRotateJob job, EntityQuery query, JobHandle dependency, ref SystemState state)
			{
				return dependency;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void AssignEntityManager(ref Monster327TurretRotateJob job, EntityManager entityManager)
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

		private InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

		public void Execute(in Monster327_Dots launcher)
		{
			if (!(launcher.turretRoot == Entity.Null) && TransformLookup.HasComponent(launcher.turretRoot))
			{
				RefRW<LocalTransform> refRW = TransformLookup.GetRefRW(launcher.turretRoot);
				float3 up = math.normalizesafe(launcher.turretDirection, new float3(0f, 1f, 0f));
				refRW.ValueRW.Rotation = quaternion.LookRotationSafe(math.forward(), up);
			}
		}

		[CompilerGenerated]
		public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
		{
			IntPtr nativeArrayPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr(chunk, ref __TypeHandle.__Monster327_Dots_RO_ComponentTypeHandle);
			int count = chunk.Count;
			int num = 0;
			if (!useEnabledMask)
			{
				for (int i = 0; i < count; i++)
				{
					Execute(in InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Monster327_Dots>(nativeArrayPtr, i));
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
						Execute(in InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Monster327_Dots>(nativeArrayPtr, nextRangeBegin));
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
					Execute(in InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Monster327_Dots>(nativeArrayPtr, j));
					num++;
				}
				num2 >>= 1;
			}
			num2 = chunkEnabledMask.ULong1;
			for (int k = 64; k < count; k++)
			{
				if ((num2 & 1) != 0L)
				{
					Execute(in InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Monster327_Dots>(nativeArrayPtr, k));
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
		public ComponentLookup<LocalToWorld> __Unity_Transforms_LocalToWorld_RO_ComponentLookup;

		public ComponentLookup<AnimaPlay> __AnimaPlay_RW_ComponentLookup;

		public Monster327Job.InternalCompilerQueryAndHandleData __Monster327System_Monster327Job_WithDefaultQuery_JobEntityTypeHandle;

		public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentLookup;

		public Monster327TurretRotateJob.InternalCompilerQueryAndHandleData __Monster327System_Monster327TurretRotateJob_WithDefaultQuery_JobEntityTypeHandle;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__Unity_Transforms_LocalToWorld_RO_ComponentLookup = state.GetComponentLookup<LocalToWorld>(isReadOnly: true);
			__AnimaPlay_RW_ComponentLookup = state.GetComponentLookup<AnimaPlay>();
			__Monster327System_Monster327Job_WithDefaultQuery_JobEntityTypeHandle.Init(ref state, assignDefaultQuery: true);
			__Unity_Transforms_LocalTransform_RW_ComponentLookup = state.GetComponentLookup<LocalTransform>();
			__Monster327System_Monster327TurretRotateJob_WithDefaultQuery_JobEntityTypeHandle.Init(ref state, assignDefaultQuery: true);
		}
	}

	private TypeHandle __TypeHandle;

	private EntityQuery __query_268780983_0;

	private EntityQuery __query_268780983_1;

	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<CurrentRoomEntitiesSingleton>();
		state.RequireForUpdate<Monster327_Dots>();
	}

	public void OnUpdate(ref SystemState state)
	{
		EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer(Allocator.TempJob);
		state.Dependency = __ScheduleViaJobChunkExtension_0(new Monster327Job
		{
			CurrentRoomEntities = __query_268780983_0.GetSingleton<CurrentRoomEntitiesSingleton>(),
			DeltaTime = state.WorldUnmanaged.Time.DeltaTime,
			LocalToWorldLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__Unity_Transforms_LocalToWorld_RO_ComponentLookup, ref state),
			AnimaLookUp = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__AnimaPlay_RW_ComponentLookup, ref state),
			Ecb = entityCommandBuffer.AsParallelWriter(),
			SEBufferEntity = __query_268780983_1.GetSingletonEntity()
		}, __TypeHandle.__Monster327System_Monster327Job_WithDefaultQuery_JobEntityTypeHandle.DefaultQuery, state.Dependency, ref state, hasUserDefinedQuery: false);
		state.Dependency = __ScheduleViaJobChunkExtension_1(new Monster327TurretRotateJob
		{
			TransformLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state)
		}, __TypeHandle.__Monster327System_Monster327TurretRotateJob_WithDefaultQuery_JobEntityTypeHandle.DefaultQuery, state.Dependency, ref state, hasUserDefinedQuery: false);
		state.Dependency.Complete();
		entityCommandBuffer.Playback(state.EntityManager);
		entityCommandBuffer.Dispose();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private JobHandle __ScheduleViaJobChunkExtension_0(Monster327Job job, EntityQuery query, JobHandle dependency, ref SystemState state, bool hasUserDefinedQuery)
	{
		dependency = __TypeHandle.__Monster327System_Monster327Job_WithDefaultQuery_JobEntityTypeHandle.UpdateBaseEntityIndexArray(ref job, query, dependency, ref state);
		__TypeHandle.__Monster327System_Monster327Job_WithDefaultQuery_JobEntityTypeHandle.AssignEntityManager(ref job, state.EntityManager);
		__TypeHandle.__Monster327System_Monster327Job_WithDefaultQuery_JobEntityTypeHandle.__TypeHandle.Update(ref state);
		return __TypeHandle.__Monster327System_Monster327Job_WithDefaultQuery_JobEntityTypeHandle.ScheduleParallel(ref job, query, dependency);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private JobHandle __ScheduleViaJobChunkExtension_1(Monster327TurretRotateJob job, EntityQuery query, JobHandle dependency, ref SystemState state, bool hasUserDefinedQuery)
	{
		dependency = __TypeHandle.__Monster327System_Monster327TurretRotateJob_WithDefaultQuery_JobEntityTypeHandle.UpdateBaseEntityIndexArray(ref job, query, dependency, ref state);
		__TypeHandle.__Monster327System_Monster327TurretRotateJob_WithDefaultQuery_JobEntityTypeHandle.AssignEntityManager(ref job, state.EntityManager);
		__TypeHandle.__Monster327System_Monster327TurretRotateJob_WithDefaultQuery_JobEntityTypeHandle.__TypeHandle.Update(ref state);
		return __TypeHandle.__Monster327System_Monster327TurretRotateJob_WithDefaultQuery_JobEntityTypeHandle.ScheduleParallel(ref job, query, dependency);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<CurrentRoomEntitiesSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_268780983_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SEData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_268780983_1 = entityQueryBuilder2.Build(ref state);
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
		((Monster327System*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnUpdate(IntPtr self, IntPtr state)
	{
		((Monster327System*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
	{
		((Monster327System*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
	}
}
