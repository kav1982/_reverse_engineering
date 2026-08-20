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

[BurstCompile]
[CompilerGenerated]
public struct Spell1002Job : IJobEntity, IJobChunk
{
	public struct InternalCompilerQueryAndHandleData
	{
		public struct TypeHandle
		{
			public ComponentTypeHandle<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentTypeHandle;

			public ComponentTypeHandle<Spell1002RollBallBeHitTimer> __Spell1002RollBallBeHitTimer_RW_ComponentTypeHandle;

			[ReadOnly]
			public ComponentTypeHandle<SpellComponentData> __SpellComponentData_RO_ComponentTypeHandle;

			[ReadOnly]
			public ComponentTypeHandle<PhysicsVelocity> __Unity_Physics_PhysicsVelocity_RO_ComponentTypeHandle;

			[ReadOnly]
			public ComponentTypeHandle<PhysicsCollider> __Unity_Physics_PhysicsCollider_RO_ComponentTypeHandle;

			[ReadOnly]
			public ComponentTypeHandle<SpellConfigComponentData> __SpellConfigComponentData_RO_ComponentTypeHandle;

			public ComponentTypeHandle<Spell1002CreateLiquid> __Spell1002CreateLiquid_RW_ComponentTypeHandle;

			public ComponentTypeHandle<SpellMovementComponentData> __SpellMovementComponentData_RW_ComponentTypeHandle;

			[ReadOnly]
			public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void __AssignHandles(ref SystemState state)
			{
				__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle = state.GetComponentTypeHandle<LocalTransform>();
				__Spell1002RollBallBeHitTimer_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Spell1002RollBallBeHitTimer>();
				__SpellComponentData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<SpellComponentData>(isReadOnly: true);
				__Unity_Physics_PhysicsVelocity_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PhysicsVelocity>(isReadOnly: true);
				__Unity_Physics_PhysicsCollider_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PhysicsCollider>(isReadOnly: true);
				__SpellConfigComponentData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<SpellConfigComponentData>(isReadOnly: true);
				__Spell1002CreateLiquid_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Spell1002CreateLiquid>();
				__SpellMovementComponentData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellMovementComponentData>();
				__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
			}

			public void Update(ref SystemState state)
			{
				__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle.Update(ref state);
				__Spell1002RollBallBeHitTimer_RW_ComponentTypeHandle.Update(ref state);
				__SpellComponentData_RO_ComponentTypeHandle.Update(ref state);
				__Unity_Physics_PhysicsVelocity_RO_ComponentTypeHandle.Update(ref state);
				__Unity_Physics_PhysicsCollider_RO_ComponentTypeHandle.Update(ref state);
				__SpellConfigComponentData_RO_ComponentTypeHandle.Update(ref state);
				__Spell1002CreateLiquid_RW_ComponentTypeHandle.Update(ref state);
				__SpellMovementComponentData_RW_ComponentTypeHandle.Update(ref state);
				__Unity_Entities_Entity_TypeHandle.Update(ref state);
			}
		}

		public TypeHandle __TypeHandle;

		public EntityQuery DefaultQuery;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void __AssignQueries(ref SystemState state)
		{
			EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
			EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellComponentData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAll<PhysicsVelocity>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAll<PhysicsCollider>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAll<SpellConfigComponentData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<LocalTransform>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Spell1002RollBallBeHitTimer>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Spell1002CreateLiquid>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellMovementComponentData>();
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
		public void Run(ref Spell1002Job job, EntityQuery query)
		{
			job.__TypeHandle = __TypeHandle;
			JobChunkExtensions.RunByRef(ref job, query);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle Schedule(ref Spell1002Job job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle ScheduleParallel(ref Spell1002Job job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleParallelByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void UpdateBaseEntityIndexArray(ref Spell1002Job job, EntityQuery query, ref SystemState state)
		{
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle UpdateBaseEntityIndexArray(ref Spell1002Job job, EntityQuery query, JobHandle dependency, ref SystemState state)
		{
			return dependency;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AssignEntityManager(ref Spell1002Job job, EntityManager entityManager)
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

	public EntityCommandBuffer.ParallelWriter CMD;

	[ReadOnly]
	public SpellSingleton SpellSingleton;

	[NativeDisableContainerSafetyRestriction]
	[NativeDisableParallelForRestriction]
	public ComponentLookup<LocalTransform> TransformLookup;

	[NativeDisableParallelForRestriction]
	public ComponentLookup<Spell1002BeHitMaterialProperty> BeHitMaterialLookup;

	[NativeDisableParallelForRestriction]
	public ComponentLookup<EffectsCollectorData> EffectCollectorLookup;

	[NativeDisableParallelForRestriction]
	public ComponentLookup<Spell1002RollBallFallToAbyssTag> FallToAbyssLookup;

	[ReadOnly]
	public PhysicsWorldSingleton Physics;

	public Entity MucusBufferEntity;

	public Entity WaterBufferEntity;

	public Entity VenomBufferEntity;

	private InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

	public unsafe void Execute(ref LocalTransform transform, ref Spell1002RollBallBeHitTimer beHitTimer, in SpellComponentData spellData, in PhysicsVelocity velocity, in PhysicsCollider collider, in SpellConfigComponentData config, ref Spell1002CreateLiquid liquid, ref SpellMovementComponentData move, Entity entity, [ChunkIndexInQuery] int chunkIndex)
	{
		if (!move.IsFallSpell)
		{
			float num = 0f;
			SphereCollider* collider2 = (SphereCollider*)((CompoundCollider*)collider.ColliderPtr)->Children[0].Collider;
			num = collider2->Radius * transform.Scale;
			transform.Position.z = 0f - num + 0.2f;
		}
		if (spellData.SpellEffectEntity != Entity.Null)
		{
			Entity effect = EffectCollectorLookup[spellData.SpellEffectEntity].Effect1;
			ref LocalTransform valueRW = ref TransformLookup.GetRefRW(effect).ValueRW;
			float num2 = MathF.PI * 4f / 5f * transform.Scale;
			float3 @float = velocity.Linear;
			if (move.Type == SpellSpecialMovementType.Rotation)
			{
				@float = move.Direction * move.Speed;
			}
			float3 float2 = new float3(@float.y / num2 * 360f, 0f, (0f - @float.x) / num2 * 360f) * (MathF.PI / 180f);
			valueRW.Rotation = math.mul(quaternion.Euler(float2 * DeltaTime), valueRW.Rotation);
			if (config.DurationTimer <= config.Duration.Calculate())
			{
				float num3 = math.distancesq(liquid.lastCreatePos, transform.Position);
				if ((double)num3 >= 0.09 && num3 <= 4f)
				{
					if (config.ColorType == SpellColorType.Venom)
					{
						CMD.AppendToBuffer(chunkIndex, VenomBufferEntity, new CreateVenomRequest(liquid.lastCreatePos, transform.Position, transform.Scale / 4f, 2f));
					}
					else if (config.ColorType == SpellColorType.Mucus)
					{
						CMD.AppendToBuffer(chunkIndex, MucusBufferEntity, new CreateMucusRequest(liquid.lastCreatePos, transform.Position, transform.Scale / 4f));
					}
					else if (config.ColorType == SpellColorType.Frozen)
					{
						CMD.AppendToBuffer(chunkIndex, WaterBufferEntity, new CreateWaterRequest(liquid.lastCreatePos, transform.Position, transform.Scale / 4f));
					}
					liquid.lastCreatePos = transform.Position;
				}
			}
		}
		float beHitTimer2 = beHitTimer.BeHitTimer;
		beHitTimer.BeHitTimer -= DeltaTime;
		if (beHitTimer.BeHitTimer > 0f)
		{
			BeHitMaterialLookup.GetRefRW(EffectCollectorLookup[spellData.SpellEffectEntity].Effect1).ValueRW.Value = 1f;
		}
		else if (beHitTimer2 > 0f && beHitTimer.BeHitTimer <= 0f)
		{
			BeHitMaterialLookup.GetRefRW(EffectCollectorLookup[spellData.SpellEffectEntity].Effect1).ValueRW.Value = 0f;
		}
		if (!move.IsFallSpell)
		{
			if (FallToAbyssLookup.IsComponentEnabled(entity))
			{
				transform.Scale -= 0.05f;
				if (transform.Scale <= 0f)
				{
					CMD.SetComponentEnabled<SpellDestroyTag>(chunkIndex, entity, value: true);
				}
			}
			else
			{
				float3 position = transform.Position.IgnoreZ();
				CollisionFilter @default = CollisionFilter.Default;
				@default.CollidesWith = 1024u;
				if (Physics.CheckSphere(position, 0.1f, @default))
				{
					CMD.SetComponentEnabled<Spell1002RollBallFallToAbyssTag>(chunkIndex, entity, value: true);
					move.Speed = 0f;
				}
			}
		}
		if (config.Float1 <= 0f)
		{
			ref EntityCommandBuffer.ParallelWriter cMD = ref CMD;
			ref SpellSingleton spellSingleton = ref SpellSingleton;
			ref float3 position2 = ref transform.Position;
			float3 direction = move.Direction;
			cMD.CreateSpellHitEffect(chunkIndex, in spellSingleton, in config, in spellData, in position2, in direction, transform.Scale);
			CMD.SetComponentEnabled<SpellDestroyTag>(chunkIndex, entity, value: true);
		}
	}

	[CompilerGenerated]
	public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
	{
		IntPtr nativeArrayPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr2 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Spell1002RollBallBeHitTimer_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr3 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr(chunk, ref __TypeHandle.__SpellComponentData_RO_ComponentTypeHandle);
		IntPtr nativeArrayPtr4 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr(chunk, ref __TypeHandle.__Unity_Physics_PhysicsVelocity_RO_ComponentTypeHandle);
		IntPtr nativeArrayPtr5 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr(chunk, ref __TypeHandle.__Unity_Physics_PhysicsCollider_RO_ComponentTypeHandle);
		IntPtr nativeArrayPtr6 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr(chunk, ref __TypeHandle.__SpellConfigComponentData_RO_ComponentTypeHandle);
		IntPtr nativeArrayPtr7 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Spell1002CreateLiquid_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr8 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellMovementComponentData_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr9 = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(chunk, __TypeHandle.__Unity_Entities_Entity_TypeHandle);
		int count = chunk.Count;
		int num = 0;
		if (!useEnabledMask)
		{
			for (int i = 0; i < count; i++)
			{
				ref LocalTransform transform = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr, i);
				ref Spell1002RollBallBeHitTimer beHitTimer = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1002RollBallBeHitTimer>(nativeArrayPtr2, i);
				ref SpellComponentData spellData = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr3, i);
				ref PhysicsVelocity velocity = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsVelocity>(nativeArrayPtr4, i);
				ref PhysicsCollider collider = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsCollider>(nativeArrayPtr5, i);
				ref SpellConfigComponentData config = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr6, i);
				ref Spell1002CreateLiquid liquid = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1002CreateLiquid>(nativeArrayPtr7, i);
				ref SpellMovementComponentData move = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr8, i);
				Entity entity = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr9, i);
				Execute(ref transform, ref beHitTimer, in spellData, in velocity, in collider, in config, ref liquid, ref move, entity, chunkIndexInQuery);
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
					ref LocalTransform transform2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr, nextRangeBegin);
					ref Spell1002RollBallBeHitTimer beHitTimer2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1002RollBallBeHitTimer>(nativeArrayPtr2, nextRangeBegin);
					ref SpellComponentData spellData2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr3, nextRangeBegin);
					ref PhysicsVelocity velocity2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsVelocity>(nativeArrayPtr4, nextRangeBegin);
					ref PhysicsCollider collider2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsCollider>(nativeArrayPtr5, nextRangeBegin);
					ref SpellConfigComponentData config2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr6, nextRangeBegin);
					ref Spell1002CreateLiquid liquid2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1002CreateLiquid>(nativeArrayPtr7, nextRangeBegin);
					ref SpellMovementComponentData move2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr8, nextRangeBegin);
					Entity entity2 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr9, nextRangeBegin);
					Execute(ref transform2, ref beHitTimer2, in spellData2, in velocity2, in collider2, in config2, ref liquid2, ref move2, entity2, chunkIndexInQuery);
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
				ref LocalTransform transform3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr, j);
				ref Spell1002RollBallBeHitTimer beHitTimer3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1002RollBallBeHitTimer>(nativeArrayPtr2, j);
				ref SpellComponentData spellData3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr3, j);
				ref PhysicsVelocity velocity3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsVelocity>(nativeArrayPtr4, j);
				ref PhysicsCollider collider3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsCollider>(nativeArrayPtr5, j);
				ref SpellConfigComponentData config3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr6, j);
				ref Spell1002CreateLiquid liquid3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1002CreateLiquid>(nativeArrayPtr7, j);
				ref SpellMovementComponentData move3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr8, j);
				Entity entity3 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr9, j);
				Execute(ref transform3, ref beHitTimer3, in spellData3, in velocity3, in collider3, in config3, ref liquid3, ref move3, entity3, chunkIndexInQuery);
				num++;
			}
			num2 >>= 1;
		}
		num2 = chunkEnabledMask.ULong1;
		for (int k = 64; k < count; k++)
		{
			if ((num2 & 1) != 0L)
			{
				ref LocalTransform transform4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr, k);
				ref Spell1002RollBallBeHitTimer beHitTimer4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1002RollBallBeHitTimer>(nativeArrayPtr2, k);
				ref SpellComponentData spellData4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr3, k);
				ref PhysicsVelocity velocity4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsVelocity>(nativeArrayPtr4, k);
				ref PhysicsCollider collider4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsCollider>(nativeArrayPtr5, k);
				ref SpellConfigComponentData config4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr6, k);
				ref Spell1002CreateLiquid liquid4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1002CreateLiquid>(nativeArrayPtr7, k);
				ref SpellMovementComponentData move4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr8, k);
				Entity entity4 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr9, k);
				Execute(ref transform4, ref beHitTimer4, in spellData4, in velocity4, in collider4, in config4, ref liquid4, ref move4, entity4, chunkIndexInQuery);
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
