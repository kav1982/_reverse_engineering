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

[UpdateAfter(typeof(SpellMoveSystem))]
[UpdateInGroup(typeof(SpellPhysicsSystemGroup), OrderLast = true)]
[BurstCompile]
[CompilerGenerated]
internal struct Spell1010SnakeWalkSystem : ISystem, ISystemCompilerGenerated
{
	public struct HitRec
	{
		public Entity Spell;

		public Entity Victim;

		public float3 Direction;
	}

	[BurstCompile]
	[CompilerGenerated]
	public struct Spell1010MoveJob : IJobEntity, IJobChunk
	{
		public struct InternalCompilerQueryAndHandleData
		{
			public struct TypeHandle
			{
				public ComponentTypeHandle<SpellMovementComponentData> __SpellMovementComponentData_RW_ComponentTypeHandle;

				public ComponentTypeHandle<Spell1010SnakeData> __Spell1010SnakeData_RW_ComponentTypeHandle;

				[ReadOnly]
				public ComponentTypeHandle<LocalTransform> __Unity_Transforms_LocalTransform_RO_ComponentTypeHandle;

				public ComponentTypeHandle<PhysicsVelocity> __Unity_Physics_PhysicsVelocity_RW_ComponentTypeHandle;

				[ReadOnly]
				public ComponentTypeHandle<SpellConfigComponentData> __SpellConfigComponentData_RO_ComponentTypeHandle;

				[ReadOnly]
				public ComponentTypeHandle<SpellComponentData> __SpellComponentData_RO_ComponentTypeHandle;

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public void __AssignHandles(ref SystemState state)
				{
					__SpellMovementComponentData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellMovementComponentData>();
					__Spell1010SnakeData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Spell1010SnakeData>();
					__Unity_Transforms_LocalTransform_RO_ComponentTypeHandle = state.GetComponentTypeHandle<LocalTransform>(isReadOnly: true);
					__Unity_Physics_PhysicsVelocity_RW_ComponentTypeHandle = state.GetComponentTypeHandle<PhysicsVelocity>();
					__SpellConfigComponentData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<SpellConfigComponentData>(isReadOnly: true);
					__SpellComponentData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<SpellComponentData>(isReadOnly: true);
				}

				public void Update(ref SystemState state)
				{
					__SpellMovementComponentData_RW_ComponentTypeHandle.Update(ref state);
					__Spell1010SnakeData_RW_ComponentTypeHandle.Update(ref state);
					__Unity_Transforms_LocalTransform_RO_ComponentTypeHandle.Update(ref state);
					__Unity_Physics_PhysicsVelocity_RW_ComponentTypeHandle.Update(ref state);
					__SpellConfigComponentData_RO_ComponentTypeHandle.Update(ref state);
					__SpellComponentData_RO_ComponentTypeHandle.Update(ref state);
				}
			}

			public TypeHandle __TypeHandle;

			public EntityQuery DefaultQuery;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private void __AssignQueries(ref SystemState state)
			{
				EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
				EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<LocalTransform>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAll<SpellConfigComponentData>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAll<SpellComponentData>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellMovementComponentData>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Spell1010SnakeData>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<PhysicsVelocity>();
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
			public void Run(ref Spell1010MoveJob job, EntityQuery query)
			{
				job.__TypeHandle = __TypeHandle;
				JobChunkExtensions.RunByRef(ref job, query);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle Schedule(ref Spell1010MoveJob job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = __TypeHandle;
				return JobChunkExtensions.ScheduleByRef(ref job, query, dependency);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle ScheduleParallel(ref Spell1010MoveJob job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = __TypeHandle;
				return JobChunkExtensions.ScheduleParallelByRef(ref job, query, dependency);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void UpdateBaseEntityIndexArray(ref Spell1010MoveJob job, EntityQuery query, ref SystemState state)
			{
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle UpdateBaseEntityIndexArray(ref Spell1010MoveJob job, EntityQuery query, JobHandle dependency, ref SystemState state)
			{
				return dependency;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void AssignEntityManager(ref Spell1010MoveJob job, EntityManager entityManager)
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

		public float DeltaTime;

		[ReadOnly]
		public PlayerController_Dots PlayerCtrller;

		private InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

		[BurstCompile]
		private void Execute(ref SpellMovementComponentData movement, ref Spell1010SnakeData data, in LocalTransform t, ref PhysicsVelocity velocity, in SpellConfigComponentData config, in SpellComponentData componentData)
		{
			switch (movement.Type)
			{
			case SpellSpecialMovementType.Normal:
			case SpellSpecialMovementType.ChaseEnemy:
			case SpellSpecialMovementType.ChaseMouse:
			case SpellSpecialMovementType.ChaseOwner:
				if (!movement.IsFallSpell && (!(config.HoverDuration > 0f) || !(config.HoverTimer > 0f)))
				{
					data.TargetDirection = movement.Direction;
					data.TotalTime += DeltaTime;
					float degree = 45f * math.cos(10f * data.TotalTime);
					float3 oldDir = movement.Direction;
					float3 @float = (movement.Direction = DTool.GetShiftedDir(in oldDir, degree));
					float3 float2 = (velocity.Linear = math.normalize(movement.Direction) * movement.Speed);
				}
				break;
			case SpellSpecialMovementType.Rotation:
				break;
			}
		}

		[CompilerGenerated]
		public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
		{
			IntPtr nativeArrayPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellMovementComponentData_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr2 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Spell1010SnakeData_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr3 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr(chunk, ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentTypeHandle);
			IntPtr nativeArrayPtr4 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Unity_Physics_PhysicsVelocity_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr5 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr(chunk, ref __TypeHandle.__SpellConfigComponentData_RO_ComponentTypeHandle);
			IntPtr nativeArrayPtr6 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr(chunk, ref __TypeHandle.__SpellComponentData_RO_ComponentTypeHandle);
			int count = chunk.Count;
			int num = 0;
			if (!useEnabledMask)
			{
				for (int i = 0; i < count; i++)
				{
					Execute(ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr, i), ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1010SnakeData>(nativeArrayPtr2, i), in InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr3, i), ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsVelocity>(nativeArrayPtr4, i), in InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr5, i), in InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr6, i));
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
						Execute(ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr, nextRangeBegin), ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1010SnakeData>(nativeArrayPtr2, nextRangeBegin), in InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr3, nextRangeBegin), ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsVelocity>(nativeArrayPtr4, nextRangeBegin), in InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr5, nextRangeBegin), in InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr6, nextRangeBegin));
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
					Execute(ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr, j), ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1010SnakeData>(nativeArrayPtr2, j), in InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr3, j), ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsVelocity>(nativeArrayPtr4, j), in InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr5, j), in InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr6, j));
					num++;
				}
				num2 >>= 1;
			}
			num2 = chunkEnabledMask.ULong1;
			for (int k = 64; k < count; k++)
			{
				if ((num2 & 1) != 0L)
				{
					Execute(ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr, k), ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1010SnakeData>(nativeArrayPtr2, k), in InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr3, k), ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsVelocity>(nativeArrayPtr4, k), in InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr5, k), in InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr6, k));
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
	public struct SnakeBodyRecordJob : IJobEntity, IJobChunk
	{
		public struct InternalCompilerQueryAndHandleData
		{
			public struct TypeHandle
			{
				public ComponentTypeHandle<Spell1010SnakeData> __Spell1010SnakeData_RW_ComponentTypeHandle;

				public ComponentTypeHandle<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentTypeHandle;

				public BufferTypeHandle<SnakeBodyPoint> __SnakeBodyPoint_RW_BufferTypeHandle;

				public BufferTypeHandle<SnakeTouchGroundPoint> __SnakeTouchGroundPoint_RW_BufferTypeHandle;

				[ReadOnly]
				public ComponentTypeHandle<SpellMovementComponentData> __SpellMovementComponentData_RO_ComponentTypeHandle;

				[ReadOnly]
				public ComponentTypeHandle<SpellConfigComponentData> __SpellConfigComponentData_RO_ComponentTypeHandle;

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public void __AssignHandles(ref SystemState state)
				{
					__Spell1010SnakeData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Spell1010SnakeData>();
					__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle = state.GetComponentTypeHandle<LocalTransform>();
					__SnakeBodyPoint_RW_BufferTypeHandle = state.GetBufferTypeHandle<SnakeBodyPoint>();
					__SnakeTouchGroundPoint_RW_BufferTypeHandle = state.GetBufferTypeHandle<SnakeTouchGroundPoint>();
					__SpellMovementComponentData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<SpellMovementComponentData>(isReadOnly: true);
					__SpellConfigComponentData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<SpellConfigComponentData>(isReadOnly: true);
				}

				public void Update(ref SystemState state)
				{
					__Spell1010SnakeData_RW_ComponentTypeHandle.Update(ref state);
					__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle.Update(ref state);
					__SnakeBodyPoint_RW_BufferTypeHandle.Update(ref state);
					__SnakeTouchGroundPoint_RW_BufferTypeHandle.Update(ref state);
					__SpellMovementComponentData_RO_ComponentTypeHandle.Update(ref state);
					__SpellConfigComponentData_RO_ComponentTypeHandle.Update(ref state);
				}
			}

			public TypeHandle __TypeHandle;

			public EntityQuery DefaultQuery;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private void __AssignQueries(ref SystemState state)
			{
				EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
				EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellMovementComponentData>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAll<SpellConfigComponentData>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Spell1010SnakeData>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<LocalTransform>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SnakeBodyPoint>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SnakeTouchGroundPoint>();
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
			public void Run(ref SnakeBodyRecordJob job, EntityQuery query)
			{
				job.__TypeHandle = __TypeHandle;
				JobChunkExtensions.RunByRef(ref job, query);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle Schedule(ref SnakeBodyRecordJob job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = __TypeHandle;
				return JobChunkExtensions.ScheduleByRef(ref job, query, dependency);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle ScheduleParallel(ref SnakeBodyRecordJob job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = __TypeHandle;
				return JobChunkExtensions.ScheduleParallelByRef(ref job, query, dependency);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void UpdateBaseEntityIndexArray(ref SnakeBodyRecordJob job, EntityQuery query, ref SystemState state)
			{
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle UpdateBaseEntityIndexArray(ref SnakeBodyRecordJob job, EntityQuery query, JobHandle dependency, ref SystemState state)
			{
				return dependency;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void AssignEntityManager(ref SnakeBodyRecordJob job, EntityManager entityManager)
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

		private const float minDisSqr = 0.09f;

		public float DeltaTime;

		private InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

		private void Execute(ref Spell1010SnakeData data, ref LocalTransform trans, DynamicBuffer<SnakeBodyPoint> bodyPoints, DynamicBuffer<SnakeTouchGroundPoint> touchGroundPoints, in SpellMovementComponentData movement, in SpellConfigComponentData spellConfigData)
		{
			if (movement.Type == SpellSpecialMovementType.Rotation)
			{
				float3 newPos = trans.Position - movement.AroundCenter;
				TryAddNewPoint(newPos, ref data, bodyPoints, touchGroundPoints);
			}
			else
			{
				TryAddNewPoint(trans.Position, ref data, bodyPoints, touchGroundPoints);
			}
			while (data.LineLength > spellConfigData.Float1)
			{
				ref float lineLength = ref data.LineLength;
				lineLength -= bodyPoints[bodyPoints.Length - 2].distance;
				bodyPoints.RemoveAt(bodyPoints.Length - 1);
			}
		}

		private void TryAddNewPoint(float3 newPos, ref Spell1010SnakeData snakeData, DynamicBuffer<SnakeBodyPoint> bodyPoints, DynamicBuffer<SnakeTouchGroundPoint> touchGroundPoints)
		{
			float num = math.distancesq(newPos, snakeData.LastPos);
			float num2 = math.sqrt(num);
			bodyPoints.ElementAt(0).Value = newPos;
			bodyPoints.ElementAt(0).distance = num2;
			if (num < 0.09f)
			{
				return;
			}
			SnakeBodyPoint elem = default(SnakeBodyPoint);
			elem.Value = newPos;
			snakeData.LastPos = newPos;
			snakeData.LineLength += num2;
			bodyPoints.Insert(0, elem);
			for (int num3 = touchGroundPoints.Length - 1; num3 >= 0; num3--)
			{
				SnakeTouchGroundPoint value = touchGroundPoints[num3];
				value.distanceToHead += num2;
				touchGroundPoints[num3] = value;
				if (value.distanceToHead > snakeData.LineLength)
				{
					touchGroundPoints.RemoveAt(num3);
				}
			}
		}

		[CompilerGenerated]
		public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
		{
			IntPtr nativeArrayPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Spell1010SnakeData_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr2 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle);
			BufferAccessor<SnakeBodyPoint> bufferAccessor = chunk.GetBufferAccessor(ref __TypeHandle.__SnakeBodyPoint_RW_BufferTypeHandle);
			BufferAccessor<SnakeTouchGroundPoint> bufferAccessor2 = chunk.GetBufferAccessor(ref __TypeHandle.__SnakeTouchGroundPoint_RW_BufferTypeHandle);
			IntPtr nativeArrayPtr3 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr(chunk, ref __TypeHandle.__SpellMovementComponentData_RO_ComponentTypeHandle);
			IntPtr nativeArrayPtr4 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr(chunk, ref __TypeHandle.__SpellConfigComponentData_RO_ComponentTypeHandle);
			int count = chunk.Count;
			int num = 0;
			if (!useEnabledMask)
			{
				for (int i = 0; i < count; i++)
				{
					ref Spell1010SnakeData data = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1010SnakeData>(nativeArrayPtr, i);
					ref LocalTransform trans = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr2, i);
					DynamicBuffer<SnakeBodyPoint> bodyPoints = bufferAccessor[i];
					DynamicBuffer<SnakeTouchGroundPoint> touchGroundPoints = bufferAccessor2[i];
					Execute(ref data, ref trans, bodyPoints, touchGroundPoints, in InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr3, i), in InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr4, i));
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
						ref Spell1010SnakeData data2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1010SnakeData>(nativeArrayPtr, nextRangeBegin);
						ref LocalTransform trans2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr2, nextRangeBegin);
						DynamicBuffer<SnakeBodyPoint> bodyPoints2 = bufferAccessor[nextRangeBegin];
						DynamicBuffer<SnakeTouchGroundPoint> touchGroundPoints2 = bufferAccessor2[nextRangeBegin];
						Execute(ref data2, ref trans2, bodyPoints2, touchGroundPoints2, in InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr3, nextRangeBegin), in InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr4, nextRangeBegin));
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
					ref Spell1010SnakeData data3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1010SnakeData>(nativeArrayPtr, j);
					ref LocalTransform trans3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr2, j);
					DynamicBuffer<SnakeBodyPoint> bodyPoints3 = bufferAccessor[j];
					DynamicBuffer<SnakeTouchGroundPoint> touchGroundPoints3 = bufferAccessor2[j];
					Execute(ref data3, ref trans3, bodyPoints3, touchGroundPoints3, in InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr3, j), in InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr4, j));
					num++;
				}
				num2 >>= 1;
			}
			num2 = chunkEnabledMask.ULong1;
			for (int k = 64; k < count; k++)
			{
				if ((num2 & 1) != 0L)
				{
					ref Spell1010SnakeData data4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1010SnakeData>(nativeArrayPtr, k);
					ref LocalTransform trans4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr2, k);
					DynamicBuffer<SnakeBodyPoint> bodyPoints4 = bufferAccessor[k];
					DynamicBuffer<SnakeTouchGroundPoint> touchGroundPoints4 = bufferAccessor2[k];
					Execute(ref data4, ref trans4, bodyPoints4, touchGroundPoints4, in InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr3, k), in InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr4, k));
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

	[BurstCompile]
	public struct SnakeBodyCollectChunkJob : IJobChunk
	{
		public float DeltaTime;

		[ReadOnly]
		public PhysicsWorldSingleton PhysicsWorld;

		[ReadOnly]
		public ComponentLookup<UnitProperty_Dots> UnitPropertyLookup;

		[NativeDisableContainerSafetyRestriction]
		[ReadOnly]
		[NativeDisableParallelForRestriction]
		public ComponentLookup<SpellConfigComponentData> SpellConfigLookup;

		[ReadOnly]
		public EntityTypeHandle EntityType;

		[ReadOnly]
		public ComponentTypeHandle<LocalTransform> TransType;

		public ComponentTypeHandle<Spell1010SnakeData> SnakeType;

		[ReadOnly]
		public BufferTypeHandle<SnakeBodyPoint> BodyType;

		public ComponentTypeHandle<SpellConfigComponentData> ConfigType;

		[ReadOnly]
		public ComponentTypeHandle<SpellMovementComponentData> MoveType;

		public NativeStream.Writer Writer;

		public void Execute(in ArchetypeChunk chunk, int batchIndex, bool useEnabledMask, in v128 chunkEnabledMask)
		{
			NativeArray<Entity> nativeArray = chunk.GetNativeArray(EntityType);
			NativeArray<SpellConfigComponentData> nativeArray2 = chunk.GetNativeArray(ref ConfigType);
			NativeArray<SpellMovementComponentData> nativeArray3 = chunk.GetNativeArray(ref MoveType);
			BufferAccessor<SnakeBodyPoint> bufferAccessor = chunk.GetBufferAccessor(ref BodyType);
			NativeList<ColliderCastHit> hitList = new NativeList<ColliderCastHit>(Allocator.Temp);
			NativeParallelHashMap<Entity, float3> nativeParallelHashMap = new NativeParallelHashMap<Entity, float3>(32, Allocator.Temp);
			NativeStream.Writer writer = Writer;
			writer.BeginForEachIndex(batchIndex);
			for (int i = 0; i < chunk.Count; i++)
			{
				Entity spell = nativeArray[i];
				SpellConfigComponentData value = nativeArray2[i];
				SpellMovementComponentData spellMovementComponentData = nativeArray3[i];
				DynamicBuffer<SnakeBodyPoint> dynamicBuffer = bufferAccessor[i];
				if (dynamicBuffer.Length < 2 || spellMovementComponentData.IsFallSpell)
				{
					continue;
				}
				value.DamageTimer += DeltaTime;
				if (value.DamageTimer < value.DamageInterval)
				{
					nativeArray2[i] = value;
					continue;
				}
				value.DamageTimer -= value.DamageInterval;
				nativeArray2[i] = value;
				bool flag = spellMovementComponentData.Type == SpellSpecialMovementType.Rotation;
				float3 aroundCenter = spellMovementComponentData.AroundCenter;
				nativeParallelHashMap.Clear();
				for (int num = dynamicBuffer.Length - 1; num > 0; num -= 2)
				{
					float3 rayStart = dynamicBuffer[num].Value;
					float3 rayEnd = dynamicBuffer[math.max(0, num - 1)].Value;
					if (flag)
					{
						rayStart += aroundCenter;
						rayEnd += aroundCenter;
						rayStart.z = 0f;
						rayEnd.z = 0f;
					}
					hitList.Clear();
					float width = 0.2f;
					SpellTools.GetAttackableEntitiesInSphereCast(in rayStart, in rayEnd, in width, in value.ShooterType, containsBrittleness: true, in UnitPropertyLookup, in SpellConfigLookup, in PhysicsWorld, ref hitList);
					float3 item = math.normalizesafe(rayEnd - rayStart);
					for (int j = 0; j < hitList.Length; j++)
					{
						Entity entity = hitList[j].Entity;
						nativeParallelHashMap.TryAdd(entity, item);
					}
				}
				foreach (KeyValue<Entity, float3> item2 in nativeParallelHashMap)
				{
					writer.Write(new HitRec
					{
						Spell = spell,
						Victim = item2.Key,
						Direction = item2.Value
					});
				}
			}
			writer.EndForEachIndex();
		}

		void IJobChunk.Execute(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask)
		{
			Execute(in chunk, unfilteredChunkIndex, useEnabledMask, in chunkEnabledMask);
		}
	}

	[BurstCompile]
	public struct SnakeBodyApplyParallelJob : IJobParallelFor
	{
		public NativeStream.Reader Reader;

		public EntityCommandBuffer.ParallelWriter CMD;

		[NativeDisableContainerSafetyRestriction]
		[NativeDisableParallelForRestriction]
		[ReadOnly]
		public ComponentLookup<LocalTransform> TransformLookup;

		[NativeDisableParallelForRestriction]
		[ReadOnly]
		[NativeDisableContainerSafetyRestriction]
		public ComponentLookup<UnitProperty_Dots> UnitPropertyLookup;

		[ReadOnly]
		[NativeDisableParallelForRestriction]
		[NativeDisableContainerSafetyRestriction]
		public ComponentLookup<SpellMovementComponentData> MoveLookup;

		[NativeDisableParallelForRestriction]
		[ReadOnly]
		[NativeDisableContainerSafetyRestriction]
		public ComponentLookup<SpellComponentData> SpellCompLookup;

		[NativeDisableContainerSafetyRestriction]
		[ReadOnly]
		[NativeDisableParallelForRestriction]
		public ComponentLookup<SpellElementEffectComponentData> ElemLookup;

		[NativeDisableContainerSafetyRestriction]
		[ReadOnly]
		[NativeDisableParallelForRestriction]
		public ComponentLookup<SpellConfigComponentData> SpellConfigLookup;

		public DynamicOptimizeData OptimizeData;

		public Entity GlobalParticleSystemBufferEntity;

		public void Execute(int forEachIndex)
		{
			NativeStream.Reader reader = Reader;
			int num = reader.BeginForEachIndex(forEachIndex);
			for (int i = 0; i < num; i++)
			{
				HitRec hitRec = reader.Read<HitRec>();
				Entity spell = hitRec.Spell;
				Entity target = hitRec.Victim;
				float3 direction = hitRec.Direction;
				if (SpellConfigLookup.HasComponent(spell) && MoveLookup.HasComponent(spell) && TransformLookup.HasComponent(spell) && SpellCompLookup.HasComponent(spell) && ElemLookup.HasComponent(spell) && TransformLookup.HasComponent(target))
				{
					SpellConfigComponentData config = SpellConfigLookup[spell];
					SpellMovementComponentData movement = MoveLookup[spell];
					LocalTransform transform = TransformLookup[spell];
					SpellComponentData data = SpellCompLookup[spell];
					SpellElementEffectComponentData element = ElemLookup[spell];
					SpellTools.GetSpellElementDataWithTimeScale(in element, in OptimizeData, out var result);
					TakeDamageInfo_Dots.NewInfo(spell, CostPenetrate: false, in config, in movement, in transform, in result, in data, out var info);
					info.spell.HitPosition = TransformLookup[target].Position + new float3(0f, 0f, -0.2f);
					info.spell.CostRefraction = true;
					if (CMD.TryAttackEntity(forEachIndex, in target, in info, in UnitPropertyLookup, in SpellConfigLookup) != SpellTools.HitType.IgnoreSpell)
					{
						HitGlobalEffect(direction, info.spell.HitPosition, in config, forEachIndex);
					}
				}
			}
			reader.EndForEachIndex();
		}

		[BurstCompile]
		private void HitGlobalEffect(float3 dir, float3 pos, in SpellConfigComponentData config, [ChunkIndexInQuery] int chunkIndex)
		{
			config.ColorType.ColorEnumToString(out var result);
			float3 layerPosition = DTool.GetLayerPosition(in pos, LayerCorrectType.Coordinate);
			GlobalParticleEmitParams globalParticleEmitParams = default(GlobalParticleEmitParams);
			globalParticleEmitParams.Name = $"1010_Hit_{result}";
			globalParticleEmitParams.Alpha = 1f;
			globalParticleEmitParams.Position = new float3(pos) + layerPosition;
			globalParticleEmitParams.Velocity = dir;
			GlobalParticleEmitParams element = globalParticleEmitParams;
			CMD.AppendToBuffer(chunkIndex, GlobalParticleSystemBufferEntity, element);
		}
	}

	private struct TypeHandle
	{
		public Spell1010MoveJob.InternalCompilerQueryAndHandleData __Spell1010SnakeWalkSystem_Spell1010MoveJob_WithDefaultQuery_JobEntityTypeHandle;

		public SnakeBodyRecordJob.InternalCompilerQueryAndHandleData __Spell1010SnakeWalkSystem_SnakeBodyRecordJob_WithDefaultQuery_JobEntityTypeHandle;

		[ReadOnly]
		public ComponentLookup<UnitProperty_Dots> __UnitProperty_Dots_RO_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<SpellConfigComponentData> __SpellConfigComponentData_RO_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RO_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<SpellMovementComponentData> __SpellMovementComponentData_RO_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<SpellComponentData> __SpellComponentData_RO_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<SpellElementEffectComponentData> __SpellElementEffectComponentData_RO_ComponentLookup;

		public ComponentLookup<SpellConfigComponentData> __SpellConfigComponentData_RW_ComponentLookup;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__Spell1010SnakeWalkSystem_Spell1010MoveJob_WithDefaultQuery_JobEntityTypeHandle.Init(ref state, assignDefaultQuery: true);
			__Spell1010SnakeWalkSystem_SnakeBodyRecordJob_WithDefaultQuery_JobEntityTypeHandle.Init(ref state, assignDefaultQuery: true);
			__UnitProperty_Dots_RO_ComponentLookup = state.GetComponentLookup<UnitProperty_Dots>(isReadOnly: true);
			__SpellConfigComponentData_RO_ComponentLookup = state.GetComponentLookup<SpellConfigComponentData>(isReadOnly: true);
			__Unity_Transforms_LocalTransform_RO_ComponentLookup = state.GetComponentLookup<LocalTransform>(isReadOnly: true);
			__SpellMovementComponentData_RO_ComponentLookup = state.GetComponentLookup<SpellMovementComponentData>(isReadOnly: true);
			__SpellComponentData_RO_ComponentLookup = state.GetComponentLookup<SpellComponentData>(isReadOnly: true);
			__SpellElementEffectComponentData_RO_ComponentLookup = state.GetComponentLookup<SpellElementEffectComponentData>(isReadOnly: true);
			__SpellConfigComponentData_RW_ComponentLookup = state.GetComponentLookup<SpellConfigComponentData>();
		}
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void __codegen__OnUpdate_000064EC_0024PostfixBurstDelegate(IntPtr self, IntPtr state);

	internal static class __codegen__OnUpdate_000064EC_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<__codegen__OnUpdate_000064EC_0024PostfixBurstDelegate>(delegate(IntPtr self, IntPtr state)
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
			__codegen__OnUpdate_0024BurstManaged(self, state);
		}
	}

	private ComponentTypeHandle<LocalTransform> _ltTypeHandle;

	private ComponentTypeHandle<Spell1010SnakeData> _snakeTypeHandle;

	private BufferTypeHandle<SnakeBodyPoint> _snakeBodyTypeHandleRO;

	private ComponentTypeHandle<SpellConfigComponentData> _spellConfigDataTypeHandle;

	private ComponentTypeHandle<SpellMovementComponentData> _spellMovementTypeHandle;

	private EntityTypeHandle _entityTypeHandle;

	private TypeHandle __TypeHandle;

	private EntityQuery __query_272089959_0;

	private EntityQuery __query_272089959_1;

	private EntityQuery __query_272089959_2;

	private EntityQuery __query_272089959_3;

	private EntityQuery __query_272089959_4;

	private EntityQuery __query_272089959_5;

	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<GlobalParticleEmitParams>();
		state.RequireForUpdate<DynamicOptimizeData>();
		state.RequireForUpdate<EndSpellSimulationEntityCommandBufferSystem.Singleton>();
		state.RequireForUpdate<PhysicsWorldSingleton>();
		state.RequireForUpdate<PlayerController_Dots>();
		state.RequireForUpdate<Spell1010SnakeData>();
		_ltTypeHandle = state.GetComponentTypeHandle<LocalTransform>();
		_snakeTypeHandle = state.GetComponentTypeHandle<Spell1010SnakeData>();
		_snakeBodyTypeHandleRO = state.GetBufferTypeHandle<SnakeBodyPoint>(isReadOnly: true);
		_spellConfigDataTypeHandle = state.GetComponentTypeHandle<SpellConfigComponentData>();
		_spellMovementTypeHandle = state.GetComponentTypeHandle<SpellMovementComponentData>(isReadOnly: true);
		_entityTypeHandle = state.GetEntityTypeHandle();
	}

	[BurstCompile]
	public void OnUpdate(ref SystemState state)
	{
		state.Dependency = __ScheduleViaJobChunkExtension_0(new Spell1010MoveJob
		{
			DeltaTime = state.WorldUnmanaged.Time.DeltaTime,
			PlayerCtrller = __query_272089959_1.GetSingleton<PlayerController_Dots>()
		}, __TypeHandle.__Spell1010SnakeWalkSystem_Spell1010MoveJob_WithDefaultQuery_JobEntityTypeHandle.DefaultQuery, state.Dependency, ref state, hasUserDefinedQuery: false);
		state.Dependency = __ScheduleViaJobChunkExtension_1(new SnakeBodyRecordJob
		{
			DeltaTime = state.WorldUnmanaged.Time.DeltaTime
		}, __TypeHandle.__Spell1010SnakeWalkSystem_SnakeBodyRecordJob_WithDefaultQuery_JobEntityTypeHandle.DefaultQuery, state.Dependency, ref state, hasUserDefinedQuery: false);
		EntityCommandBuffer entityCommandBuffer = __query_272089959_2.GetSingleton<EndSpellSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
		_ltTypeHandle.Update(ref state);
		_snakeBodyTypeHandleRO.Update(ref state);
		_snakeTypeHandle.Update(ref state);
		_spellConfigDataTypeHandle.Update(ref state);
		_spellMovementTypeHandle.Update(ref state);
		_entityTypeHandle.Update(ref state);
		EntityQuery _query_272089959_ = __query_272089959_0;
		int num = _query_272089959_.CalculateChunkCount();
		NativeStream nativeStream = new NativeStream(num, Allocator.TempJob);
		SnakeBodyCollectChunkJob jobData = default(SnakeBodyCollectChunkJob);
		jobData.DeltaTime = state.WorldUnmanaged.Time.DeltaTime;
		jobData.PhysicsWorld = __query_272089959_3.GetSingleton<PhysicsWorldSingleton>();
		jobData.UnitPropertyLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__UnitProperty_Dots_RO_ComponentLookup, ref state);
		jobData.SpellConfigLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__SpellConfigComponentData_RO_ComponentLookup, ref state);
		jobData.EntityType = _entityTypeHandle;
		jobData.TransType = _ltTypeHandle;
		jobData.SnakeType = _snakeTypeHandle;
		jobData.BodyType = _snakeBodyTypeHandleRO;
		jobData.ConfigType = _spellConfigDataTypeHandle;
		jobData.MoveType = _spellMovementTypeHandle;
		jobData.Writer = nativeStream.AsWriter();
		JobHandle dependsOn = JobChunkExtensions.ScheduleParallel(jobData, _query_272089959_, state.Dependency);
		SnakeBodyApplyParallelJob jobData2 = default(SnakeBodyApplyParallelJob);
		jobData2.Reader = nativeStream.AsReader();
		jobData2.CMD = entityCommandBuffer.AsParallelWriter();
		jobData2.TransformLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref state);
		jobData2.UnitPropertyLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__UnitProperty_Dots_RO_ComponentLookup, ref state);
		jobData2.MoveLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__SpellMovementComponentData_RO_ComponentLookup, ref state);
		jobData2.SpellCompLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__SpellComponentData_RO_ComponentLookup, ref state);
		jobData2.ElemLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__SpellElementEffectComponentData_RO_ComponentLookup, ref state);
		jobData2.SpellConfigLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__SpellConfigComponentData_RW_ComponentLookup, ref state);
		jobData2.OptimizeData = __query_272089959_4.GetSingleton<DynamicOptimizeData>();
		jobData2.GlobalParticleSystemBufferEntity = __query_272089959_5.GetSingletonEntity();
		JobHandle inputDeps = IJobParallelForExtensions.Schedule(jobData2, num, 32, dependsOn);
		state.Dependency = nativeStream.Dispose(inputDeps);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private JobHandle __ScheduleViaJobChunkExtension_0(Spell1010MoveJob job, EntityQuery query, JobHandle dependency, ref SystemState state, bool hasUserDefinedQuery)
	{
		dependency = __TypeHandle.__Spell1010SnakeWalkSystem_Spell1010MoveJob_WithDefaultQuery_JobEntityTypeHandle.UpdateBaseEntityIndexArray(ref job, query, dependency, ref state);
		__TypeHandle.__Spell1010SnakeWalkSystem_Spell1010MoveJob_WithDefaultQuery_JobEntityTypeHandle.AssignEntityManager(ref job, state.EntityManager);
		__TypeHandle.__Spell1010SnakeWalkSystem_Spell1010MoveJob_WithDefaultQuery_JobEntityTypeHandle.__TypeHandle.Update(ref state);
		return __TypeHandle.__Spell1010SnakeWalkSystem_Spell1010MoveJob_WithDefaultQuery_JobEntityTypeHandle.ScheduleParallel(ref job, query, dependency);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private JobHandle __ScheduleViaJobChunkExtension_1(SnakeBodyRecordJob job, EntityQuery query, JobHandle dependency, ref SystemState state, bool hasUserDefinedQuery)
	{
		dependency = __TypeHandle.__Spell1010SnakeWalkSystem_SnakeBodyRecordJob_WithDefaultQuery_JobEntityTypeHandle.UpdateBaseEntityIndexArray(ref job, query, dependency, ref state);
		__TypeHandle.__Spell1010SnakeWalkSystem_SnakeBodyRecordJob_WithDefaultQuery_JobEntityTypeHandle.AssignEntityManager(ref job, state.EntityManager);
		__TypeHandle.__Spell1010SnakeWalkSystem_SnakeBodyRecordJob_WithDefaultQuery_JobEntityTypeHandle.__TypeHandle.Update(ref state);
		return __TypeHandle.__Spell1010SnakeWalkSystem_SnakeBodyRecordJob_WithDefaultQuery_JobEntityTypeHandle.ScheduleParallel(ref job, query, dependency);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<Spell1010SnakeData, LocalTransform, SpellElementEffectComponentData, SpellComponentData, SnakeBodyPoint>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellConfigComponentData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAll<SpellMovementComponentData>();
		__query_272089959_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<PlayerController_Dots>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_272089959_1 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<EndSpellSimulationEntityCommandBufferSystem.Singleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_272089959_2 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<PhysicsWorldSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_272089959_3 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<DynamicOptimizeData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_272089959_4 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<GlobalParticleEmitParams>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_272089959_5 = entityQueryBuilder2.Build(ref state);
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
		((Spell1010SnakeWalkSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	[BurstCompile]
	internal static void __codegen__OnUpdate(IntPtr self, IntPtr state)
	{
		__codegen__OnUpdate_000064EC_0024BurstDirectCall.Invoke(self, state);
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
	{
		((Spell1010SnakeWalkSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[BurstCompile]
	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnUpdate_0024BurstManaged(IntPtr self, IntPtr state)
	{
		((Spell1010SnakeWalkSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
	}
}
