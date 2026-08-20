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
using Unity.Transforms;

[BurstCompile]
[CompilerGenerated]
public struct Spell4013MoveJob : IJobEntity, IJobChunk
{
	public struct InternalCompilerQueryAndHandleData
	{
		public struct TypeHandle
		{
			public ComponentTypeHandle<SpellComponentData> __SpellComponentData_RW_ComponentTypeHandle;

			public ComponentTypeHandle<SpellMovementComponentData> __SpellMovementComponentData_RW_ComponentTypeHandle;

			public ComponentTypeHandle<Spell4013TransformRightData> __Spell4013TransformRightData_RW_ComponentTypeHandle;

			public ComponentTypeHandle<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentTypeHandle;

			public ComponentTypeHandle<Spell4013RuneHammerData> __Spell4013RuneHammerData_RW_ComponentTypeHandle;

			[ReadOnly]
			public ComponentTypeHandle<SpellSplitComponentData> __SpellSplitComponentData_RO_ComponentTypeHandle;

			[ReadOnly]
			public ComponentTypeHandle<SpellConfigComponentData> __SpellConfigComponentData_RO_ComponentTypeHandle;

			[ReadOnly]
			public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;

			public BufferTypeHandle<LinkedEntityGroup> __Unity_Entities_LinkedEntityGroup_RW_BufferTypeHandle;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void __AssignHandles(ref SystemState state)
			{
				__SpellComponentData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellComponentData>();
				__SpellMovementComponentData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellMovementComponentData>();
				__Spell4013TransformRightData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Spell4013TransformRightData>();
				__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle = state.GetComponentTypeHandle<LocalTransform>();
				__Spell4013RuneHammerData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Spell4013RuneHammerData>();
				__SpellSplitComponentData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<SpellSplitComponentData>(isReadOnly: true);
				__SpellConfigComponentData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<SpellConfigComponentData>(isReadOnly: true);
				__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
				__Unity_Entities_LinkedEntityGroup_RW_BufferTypeHandle = state.GetBufferTypeHandle<LinkedEntityGroup>();
			}

			public void Update(ref SystemState state)
			{
				__SpellComponentData_RW_ComponentTypeHandle.Update(ref state);
				__SpellMovementComponentData_RW_ComponentTypeHandle.Update(ref state);
				__Spell4013TransformRightData_RW_ComponentTypeHandle.Update(ref state);
				__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle.Update(ref state);
				__Spell4013RuneHammerData_RW_ComponentTypeHandle.Update(ref state);
				__SpellSplitComponentData_RO_ComponentTypeHandle.Update(ref state);
				__SpellConfigComponentData_RO_ComponentTypeHandle.Update(ref state);
				__Unity_Entities_Entity_TypeHandle.Update(ref state);
				__Unity_Entities_LinkedEntityGroup_RW_BufferTypeHandle.Update(ref state);
			}
		}

		public TypeHandle __TypeHandle;

		public EntityQuery DefaultQuery;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void __AssignQueries(ref SystemState state)
		{
			EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
			EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellSplitComponentData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAll<SpellConfigComponentData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellComponentData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellMovementComponentData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Spell4013TransformRightData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<LocalTransform>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Spell4013RuneHammerData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<LinkedEntityGroup>();
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
		public void Run(ref Spell4013MoveJob job, EntityQuery query)
		{
			job.__TypeHandle = __TypeHandle;
			JobChunkExtensions.RunByRef(ref job, query);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle Schedule(ref Spell4013MoveJob job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle ScheduleParallel(ref Spell4013MoveJob job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleParallelByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void UpdateBaseEntityIndexArray(ref Spell4013MoveJob job, EntityQuery query, ref SystemState state)
		{
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle UpdateBaseEntityIndexArray(ref Spell4013MoveJob job, EntityQuery query, JobHandle dependency, ref SystemState state)
		{
			return dependency;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AssignEntityManager(ref Spell4013MoveJob job, EntityManager entityManager)
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

	public float3 mousePosition;

	public CurrentRoomEntitiesSingleton CurrentRoomEntitiesSingleton;

	[NativeDisableParallelForRestriction]
	public ComponentLookup<Spell4013SpiltEntityData> Spell4013SplitLookUp;

	[NativeDisableParallelForRestriction]
	[NativeDisableContainerSafetyRestriction]
	public ComponentLookup<Spell4013TransformRightData> TransformRightDataLookUp;

	[NativeDisableParallelForRestriction]
	[NativeDisableContainerSafetyRestriction]
	public ComponentLookup<LocalTransform> transformLookup;

	private InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

	[BurstCompile]
	private void Execute(ref SpellComponentData data, ref SpellMovementComponentData movement, Spell4013TransformRightData transformRight, ref LocalTransform transform, ref Spell4013RuneHammerData hammerData, in SpellSplitComponentData splitData, in SpellConfigComponentData config, Entity entity, DynamicBuffer<LinkedEntityGroup> childBuffer)
	{
		UpdateSpellPosition(IsSplit: false, hammerData.currentIndex, hammerData.maxHammerCount, transformRight.TransformRight, transform.Position, ref data, ref movement, ref transform, ref hammerData, in config, entity);
		int count = splitData.Count;
		int num = 1;
		foreach (LinkedEntityGroup item in childBuffer)
		{
			if (Spell4013SplitLookUp.HasComponent(item.Value))
			{
				LocalTransform transform2 = transformLookup[item.Value];
				UpdateSpellPosition(IsSplit: true, num, count, TransformRightDataLookUp[item.Value].TransformRight, transform.Position, ref data, ref movement, ref transform2, ref hammerData, in config, item.Value);
				transformLookup[item.Value] = transform2;
				num++;
			}
		}
	}

	[BurstCompile]
	private float GetFallHeight(float percent)
	{
		percent = math.clamp(percent, 0f, 1f);
		if (percent <= 0.2f)
		{
			return 0f;
		}
		if (percent <= 0.5f)
		{
			return Bezier4(0f, 0.3f, 0.65f, 0.88f, (percent - 0.2f) / 0.3f);
		}
		if (percent <= 0.8f)
		{
			return Bezier4(0.88f, 0.965f, 0.995f, 1f, (percent - 0.5f) / 0.3f);
		}
		return Bezier4(1f, 0.95f, 0.5f, 0f, (percent - 0.8f) / 0.2f);
		static float Bezier4(float p0, float p1, float p2, float p3, float t)
		{
			float num = 1f - t;
			float num2 = num * num;
			float num3 = num2 * num;
			float num4 = t * t;
			float num5 = num4 * t;
			return num3 * p0 + 3f * num2 * t * p1 + 3f * num * num4 * p2 + num5 * p3;
		}
	}

	[BurstCompile]
	private void UpdateSpellPosition(bool IsSplit, int curIndex, int maxIndex, float3 Direction, float3 SpellTransformPosition, ref SpellComponentData data, ref SpellMovementComponentData movement, ref LocalTransform transform, ref Spell4013RuneHammerData hammerData, in SpellConfigComponentData config, Entity entity)
	{
		if (!IsSplit && transformLookup.TryGetComponent(movement.AroundTarget, out var componentData))
		{
			if (componentData.Position.z == 0f)
			{
				movement.AroundCenter = componentData.Position + new float3(0f, 0f, -0.3f);
			}
			else
			{
				movement.AroundCenter = componentData.Position;
			}
		}
		float3 from = (movement.IsFallSpell ? (IsSplit ? SpellTransformPosition : movement.AroundCenter) : (IsSplit ? (SpellTransformPosition + hammerData.HammerLength * transform.Scale * movement.Direction) : movement.AroundCenter));
		if (!hammerData.IsInitialized)
		{
			hammerData.IsInitialized = true;
			if (hammerData.IsRotateAroundWandSpirit && movement.Type == SpellSpecialMovementType.ChaseMouse)
			{
				float degree = -30f * (float)(maxIndex - 1) / 2f + 30f * (float)(curIndex - 1);
				movement.Direction = DTool.RotateDir(movement.Direction, degree);
			}
		}
		Entity target;
		UnitProperty_Dots targetPpt;
		if (!movement.IsFallSpell)
		{
			switch (movement.Type)
			{
			case SpellSpecialMovementType.Normal:
			case SpellSpecialMovementType.ChaseOwner:
				transform.Position = from + Direction * 0.4f;
				break;
			case SpellSpecialMovementType.Rotation:
				transform.Position = from + Direction * (movement.AroundRadius * 0.8f);
				break;
			case SpellSpecialMovementType.ChaseMouse:
			{
				float degree3 = -30f * (float)(maxIndex - 1) / 2f + 30f * (float)(curIndex - 1);
				float3 float2 = DTool.IgnoreZDir(in mousePosition, in movement.AroundCenter);
				float2 = (IsSplit ? movement.Direction : float2);
				if (hammerData.IsRotateAroundWandSpirit)
				{
					float3 from2 = transformLookup[data.Shooter].Position;
					Direction = DTool.IgnoreZDir(in mousePosition, in from2);
					if (IsSplit)
					{
						degree3 = -30f * (float)maxIndex / 2f + 30f * (float)curIndex;
						Direction = DTool.RotateDir(Direction, degree3);
					}
				}
				else
				{
					Direction = DTool.RotateDir(float2, degree3);
				}
				if (!IsSplit)
				{
					transform.Position = from + Direction * 0.3f;
				}
				else
				{
					transform.Position = from;
				}
				break;
			}
			case SpellSpecialMovementType.ChaseEnemy:
			{
				float degree2 = -30f * (float)(maxIndex - 1) / 2f + 30f * (float)(curIndex - 1);
				float3 source = math.mul(transform.Rotation, math.right());
				if (CurrentRoomEntitiesSingleton.FindNearestTarget(from, config.ShooterType, out target, out var targetPosition, out targetPpt))
				{
					float3 oldDir = DTool.IgnoreZDir(in targetPosition, in from);
					oldDir = DTool.RotateDir(oldDir, degree2);
					Direction = DTool.DirMoveTowardsIgnoreZ(in source, in oldDir, 12f * movement.ChaseRotateSpeed * deltaTime);
					if (!IsSplit)
					{
						transform.Position = from + Direction * 0.3f;
					}
					else
					{
						transform.Position = from;
					}
					break;
				}
				float3 @float = DTool.IgnoreZDir(in mousePosition, in movement.AroundCenter);
				@float = (IsSplit ? movement.Direction : @float);
				Direction = DTool.RotateDir(@float, degree2);
				Direction = DTool.DirMoveTowardsIgnoreZ(in source, in Direction, 12f * movement.ChaseRotateSpeed * deltaTime);
				if (!IsSplit)
				{
					transform.Position = from + Direction * 0.3f;
				}
				else
				{
					transform.Position = from;
				}
				break;
			}
			}
			float2 dir = Direction.xy;
			transform.Rotation = DTool.DirectionToRotation(in dir);
			if (!IsSplit)
			{
				movement.Direction = Direction;
			}
		}
		else
		{
			float num = hammerData.HammerLength;
			from.z = 0f;
			float3 float3 = new float3(0f, 2f * GetFallHeight(config.DamageTimer / 0.5f), -1f * GetFallHeight(config.DamageTimer / 0.5f));
			switch (movement.Type)
			{
			case SpellSpecialMovementType.Normal:
				if (IsSplit)
				{
					float3 += new float3(0f, 0.75f * (1f + config.Radius.AddRatio) * config.Radius.MulRatio, 0f);
				}
				break;
			case SpellSpecialMovementType.Rotation:
				num = movement.AroundRadius;
				if (IsSplit)
				{
					float3 += new float3(0f, 0.75f * (1f + config.Radius.AddRatio) * config.Radius.MulRatio, 0f);
				}
				break;
			case SpellSpecialMovementType.ChaseMouse:
			{
				float degree5 = -30f * (float)(maxIndex - 1) / 2f + 30f * ((float)curIndex - 1f);
				float3 oldDir3 = DTool.IgnoreZDir(in mousePosition, in movement.AroundCenter);
				if (IsSplit)
				{
					oldDir3 = movement.Direction;
				}
				Direction = DTool.RotateDir(oldDir3, degree5);
				float3.z = 0f;
				break;
			}
			case SpellSpecialMovementType.ChaseEnemy:
			{
				Spell4013TransformRightData value = TransformRightDataLookUp[entity];
				float3 float4 = DTool.IgnoreZDir(in mousePosition, in movement.AroundCenter);
				if (IsSplit)
				{
					float4 = movement.Direction;
				}
				float3 transformRight = value.TransformRight;
				if (transformRight.x == 0f && transformRight.y == 0f && transformRight.z == 0f)
				{
					value.TransformRight = float4;
				}
				float degree4 = -30f * (float)(maxIndex - 1) / 2f + 30f * ((float)curIndex - 1f);
				if (CurrentRoomEntitiesSingleton.FindNearestTarget(from, config.ShooterType, out target, out var targetPosition2, out targetPpt))
				{
					float3 oldDir2 = DTool.IgnoreZDir(in targetPosition2, in from);
					oldDir2 = DTool.RotateDir(oldDir2, degree4);
					Direction = DTool.DirMoveTowardsIgnoreZ(in value.TransformRight, in oldDir2, 12f * movement.ChaseRotateSpeed * deltaTime);
					value.TransformRight = Direction;
				}
				else
				{
					float4 = DTool.RotateDir(float4, degree4);
					Direction = DTool.DirMoveTowardsIgnoreZ(in value.TransformRight, in float4, 12f * movement.ChaseRotateSpeed * deltaTime);
					value.TransformRight = Direction;
				}
				TransformRightDataLookUp[entity] = value;
				break;
			}
			}
			transform.Position = from + float3 + Direction * num;
			if (!IsSplit)
			{
				movement.Direction = Direction;
			}
		}
		float3 float5 = (movement.IsFallSpell ? new float3(0f, 1f, 0f) : Direction);
		if (float.IsNaN(transform.Position.x))
		{
			return;
		}
		float3 float6 = new float3(transform.Position.x, transform.Position.y - transform.Position.z, (transform.Position.y + transform.Position.z) * 0.01f);
		Spell4013SpiltEntityData componentData2;
		if (!IsSplit)
		{
			if (transformLookup.HasComponent(hammerData.EmberEntity))
			{
				transformLookup.GetRefRW(hammerData.EmberEntity).ValueRW.Position = float6 + float5 * transform.Scale * hammerData.HammerLength / 2f;
			}
		}
		else if (Spell4013SplitLookUp.TryGetComponent(entity, out componentData2) && transformLookup.HasComponent(componentData2.EmberEntity))
		{
			transformLookup.GetRefRW(componentData2.EmberEntity).ValueRW.Position = float6 + float5 * transform.Scale * hammerData.HammerLength / 2f;
		}
	}

	[CompilerGenerated]
	public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
	{
		IntPtr nativeArrayPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellComponentData_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr2 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellMovementComponentData_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr3 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Spell4013TransformRightData_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr4 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr5 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Spell4013RuneHammerData_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr6 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr(chunk, ref __TypeHandle.__SpellSplitComponentData_RO_ComponentTypeHandle);
		IntPtr nativeArrayPtr7 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr(chunk, ref __TypeHandle.__SpellConfigComponentData_RO_ComponentTypeHandle);
		IntPtr nativeArrayPtr8 = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(chunk, __TypeHandle.__Unity_Entities_Entity_TypeHandle);
		BufferAccessor<LinkedEntityGroup> bufferAccessor = chunk.GetBufferAccessor(ref __TypeHandle.__Unity_Entities_LinkedEntityGroup_RW_BufferTypeHandle);
		int count = chunk.Count;
		int num = 0;
		if (!useEnabledMask)
		{
			for (int i = 0; i < count; i++)
			{
				ref SpellComponentData data = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr, i);
				ref SpellMovementComponentData movement = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr2, i);
				ref Spell4013TransformRightData reference = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell4013TransformRightData>(nativeArrayPtr3, i);
				ref LocalTransform transform = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr4, i);
				ref Spell4013RuneHammerData hammerData = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell4013RuneHammerData>(nativeArrayPtr5, i);
				ref SpellSplitComponentData splitData = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellSplitComponentData>(nativeArrayPtr6, i);
				ref SpellConfigComponentData config = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr7, i);
				Entity entity = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr8, i);
				DynamicBuffer<LinkedEntityGroup> childBuffer = bufferAccessor[i];
				Execute(ref data, ref movement, reference, ref transform, ref hammerData, in splitData, in config, entity, childBuffer);
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
					ref SpellComponentData data2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr, nextRangeBegin);
					ref SpellMovementComponentData movement2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr2, nextRangeBegin);
					ref Spell4013TransformRightData reference2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell4013TransformRightData>(nativeArrayPtr3, nextRangeBegin);
					ref LocalTransform transform2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr4, nextRangeBegin);
					ref Spell4013RuneHammerData hammerData2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell4013RuneHammerData>(nativeArrayPtr5, nextRangeBegin);
					ref SpellSplitComponentData splitData2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellSplitComponentData>(nativeArrayPtr6, nextRangeBegin);
					ref SpellConfigComponentData config2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr7, nextRangeBegin);
					Entity entity2 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr8, nextRangeBegin);
					DynamicBuffer<LinkedEntityGroup> childBuffer2 = bufferAccessor[nextRangeBegin];
					Execute(ref data2, ref movement2, reference2, ref transform2, ref hammerData2, in splitData2, in config2, entity2, childBuffer2);
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
				ref SpellComponentData data3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr, j);
				ref SpellMovementComponentData movement3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr2, j);
				ref Spell4013TransformRightData reference3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell4013TransformRightData>(nativeArrayPtr3, j);
				ref LocalTransform transform3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr4, j);
				ref Spell4013RuneHammerData hammerData3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell4013RuneHammerData>(nativeArrayPtr5, j);
				ref SpellSplitComponentData splitData3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellSplitComponentData>(nativeArrayPtr6, j);
				ref SpellConfigComponentData config3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr7, j);
				Entity entity3 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr8, j);
				DynamicBuffer<LinkedEntityGroup> childBuffer3 = bufferAccessor[j];
				Execute(ref data3, ref movement3, reference3, ref transform3, ref hammerData3, in splitData3, in config3, entity3, childBuffer3);
				num++;
			}
			num2 >>= 1;
		}
		num2 = chunkEnabledMask.ULong1;
		for (int k = 64; k < count; k++)
		{
			if ((num2 & 1) != 0L)
			{
				ref SpellComponentData data4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr, k);
				ref SpellMovementComponentData movement4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr2, k);
				ref Spell4013TransformRightData reference4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell4013TransformRightData>(nativeArrayPtr3, k);
				ref LocalTransform transform4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr4, k);
				ref Spell4013RuneHammerData hammerData4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell4013RuneHammerData>(nativeArrayPtr5, k);
				ref SpellSplitComponentData splitData4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellSplitComponentData>(nativeArrayPtr6, k);
				ref SpellConfigComponentData config4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr7, k);
				Entity entity4 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr8, k);
				DynamicBuffer<LinkedEntityGroup> childBuffer4 = bufferAccessor[k];
				Execute(ref data4, ref movement4, reference4, ref transform4, ref hammerData4, in splitData4, in config4, entity4, childBuffer4);
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
