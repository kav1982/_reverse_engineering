using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using AOT;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Entities.Internal;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Stateful;
using Unity.Transforms;
using UnityEngine;

[UpdateInGroup(typeof(SpellPhysicsSystemGroup))]
[CompilerGenerated]
[BurstCompile]
internal struct Spell1021MagicBreakerDamageSystem : ISystem, ISystemCompilerGenerated
{
	[WithNone(new Type[] { typeof(SpellFallTag) })]
	[BurstCompile]
	[WithNone(new Type[] { typeof(Spell1021InitEffectTag) })]
	[CompilerGenerated]
	public struct Spell1021DamageJob : IJobEntity, IJobChunk
	{
		public struct InternalCompilerQueryAndHandleData
		{
			public struct TypeHandle
			{
				public ComponentTypeHandle<Spell1021MagicBreakerData> __Spell1021MagicBreakerData_RW_ComponentTypeHandle;

				[ReadOnly]
				public BufferTypeHandle<StatefulTriggerEvent> __Unity_Physics_Stateful_StatefulTriggerEvent_RO_BufferTypeHandle;

				public ComponentTypeHandle<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentTypeHandle;

				[ReadOnly]
				public ComponentTypeHandle<SpellConfigComponentData> __SpellConfigComponentData_RO_ComponentTypeHandle;

				[ReadOnly]
				public ComponentTypeHandle<SpellComponentData> __SpellComponentData_RO_ComponentTypeHandle;

				[ReadOnly]
				public ComponentTypeHandle<PhysicsCollider> __Unity_Physics_PhysicsCollider_RO_ComponentTypeHandle;

				[ReadOnly]
				public ComponentTypeHandle<SpellMovementComponentData> __SpellMovementComponentData_RO_ComponentTypeHandle;

				[ReadOnly]
				public ComponentTypeHandle<SpellElementEffectComponentData> __SpellElementEffectComponentData_RO_ComponentTypeHandle;

				[ReadOnly]
				public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public void __AssignHandles(ref SystemState state)
				{
					__Spell1021MagicBreakerData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Spell1021MagicBreakerData>();
					__Unity_Physics_Stateful_StatefulTriggerEvent_RO_BufferTypeHandle = state.GetBufferTypeHandle<StatefulTriggerEvent>(isReadOnly: true);
					__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle = state.GetComponentTypeHandle<LocalTransform>();
					__SpellConfigComponentData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<SpellConfigComponentData>(isReadOnly: true);
					__SpellComponentData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<SpellComponentData>(isReadOnly: true);
					__Unity_Physics_PhysicsCollider_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PhysicsCollider>(isReadOnly: true);
					__SpellMovementComponentData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<SpellMovementComponentData>(isReadOnly: true);
					__SpellElementEffectComponentData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<SpellElementEffectComponentData>(isReadOnly: true);
					__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
				}

				public void Update(ref SystemState state)
				{
					__Spell1021MagicBreakerData_RW_ComponentTypeHandle.Update(ref state);
					__Unity_Physics_Stateful_StatefulTriggerEvent_RO_BufferTypeHandle.Update(ref state);
					__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle.Update(ref state);
					__SpellConfigComponentData_RO_ComponentTypeHandle.Update(ref state);
					__SpellComponentData_RO_ComponentTypeHandle.Update(ref state);
					__Unity_Physics_PhysicsCollider_RO_ComponentTypeHandle.Update(ref state);
					__SpellMovementComponentData_RO_ComponentTypeHandle.Update(ref state);
					__SpellElementEffectComponentData_RO_ComponentTypeHandle.Update(ref state);
					__Unity_Entities_Entity_TypeHandle.Update(ref state);
				}
			}

			public TypeHandle __TypeHandle;

			public EntityQuery DefaultQuery;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private void __AssignQueries(ref SystemState state)
			{
				EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
				EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<StatefulTriggerEvent>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAll<SpellConfigComponentData>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAll<SpellComponentData>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAll<PhysicsCollider>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAll<SpellMovementComponentData>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAll<SpellElementEffectComponentData>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Spell1021MagicBreakerData>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<LocalTransform>();
				entityQueryBuilder2 = entityQueryBuilder2.WithNone<Spell1021InitEffectTag>();
				entityQueryBuilder2 = entityQueryBuilder2.WithNone<SpellFallTag>();
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
			public void Run(ref Spell1021DamageJob job, EntityQuery query)
			{
				job.__TypeHandle = __TypeHandle;
				JobChunkExtensions.RunByRef(ref job, query);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle Schedule(ref Spell1021DamageJob job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = __TypeHandle;
				return JobChunkExtensions.ScheduleByRef(ref job, query, dependency);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle ScheduleParallel(ref Spell1021DamageJob job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = __TypeHandle;
				return JobChunkExtensions.ScheduleParallelByRef(ref job, query, dependency);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void UpdateBaseEntityIndexArray(ref Spell1021DamageJob job, EntityQuery query, ref SystemState state)
			{
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle UpdateBaseEntityIndexArray(ref Spell1021DamageJob job, EntityQuery query, JobHandle dependency, ref SystemState state)
			{
				return dependency;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void AssignEntityManager(ref Spell1021DamageJob job, EntityManager entityManager)
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

		[NativeDisableContainerSafetyRestriction]
		[NativeDisableParallelForRestriction]
		public ComponentLookup<LocalTransform> LocalTransformLookUp;

		[NativeDisableParallelForRestriction]
		[NativeDisableContainerSafetyRestriction]
		public ComponentLookup<SpellConfigComponentData> SpellConfigLookup;

		[NativeDisableParallelForRestriction]
		[NativeDisableContainerSafetyRestriction]
		public ComponentLookup<SpellMovementComponentData> MovementLookUp;

		[NativeDisableContainerSafetyRestriction]
		[NativeDisableParallelForRestriction]
		public ComponentLookup<UnitProperty_Dots> UnitPropertyLookup;

		[NativeDisableContainerSafetyRestriction]
		[NativeDisableParallelForRestriction]
		public ComponentLookup<SpellComponentData> SpellDataLookup;

		public Entity SEPlayerSingleton;

		public GlobalRandom Random;

		public EntityCommandBuffer.ParallelWriter CMD;

		public SpellSingleton SpellSingleton;

		public Entity SpawnParamsEntity;

		private InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

		[BurstCompile]
		private void Execute([ChunkIndexInQuery] int chunkIndex, ref Spell1021MagicBreakerData magicBreaker, in DynamicBuffer<StatefulTriggerEvent> triggerEvent, ref LocalTransform transform, in SpellConfigComponentData config, in SpellComponentData data, in PhysicsCollider collider, in SpellMovementComponentData movement, in SpellElementEffectComponentData spellElementEffect, Entity entity)
		{
			TakeDamageInfo_Dots.NewInfo(entity, CostPenetrate: false, in config, in movement, in transform, in spellElementEffect, in data, out var info);
			foreach (StatefulTriggerEvent item in triggerEvent)
			{
				Entity target = item.GetOtherEntity(entity);
				if (item.State == StatefulEventState.Enter)
				{
					info.spell.HitPosition = LocalTransformLookUp[target].Position;
					info.SetKnockbackForceIgnoreZBySpell(info.spell.HitPosition - transform.Position);
					info.spell.IgnoreHitEffect = true;
					switch (CMD.TryAttackEntity(chunkIndex, in target, in info, in UnitPropertyLookup, in SpellConfigLookup))
					{
					case SpellTools.HitType.Unit:
					{
						ref EntityCommandBuffer.ParallelWriter cMD = ref CMD;
						ref SpellSingleton spellSingleton = ref SpellSingleton;
						float3 position = info.spell.HitPosition + new float3(0f, 0.3f, 0f);
						float3 direction = movement.Direction;
						cMD.CreateSpellHitEffect(chunkIndex, in spellSingleton, in config, in data, in position, in direction, transform.Scale);
						ref EntityCommandBuffer.ParallelWriter cMD2 = ref CMD;
						Entity sEPlayerSingleton = SEPlayerSingleton;
						FixedString32Bytes seName = "Hit";
						cMD2.AppendToBuffer(chunkIndex, sEPlayerSingleton, new SEData(DTool.GetSpellSEName(1021, in seName), SEPlayMode.Replay, 3, 0.05f, Random.random.NextFloat(0.8f, 1.2f)));
						break;
					}
					case SpellTools.HitType.IgnoreSpell:
					case SpellTools.HitType.RollBall:
					case SpellTools.HitType.Butterfly:
						TryReflectTargetSpell(ref CMD, in SpellSingleton, in target, in SpawnParamsEntity, in transform, in config, in data, chunkIndex, in SpellConfigLookup, in LocalTransformLookUp, in SpellDataLookup, in MovementLookUp);
						break;
					}
				}
			}
		}

		[CompilerGenerated]
		public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
		{
			IntPtr nativeArrayPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Spell1021MagicBreakerData_RW_ComponentTypeHandle);
			BufferAccessor<StatefulTriggerEvent> bufferAccessor = chunk.GetBufferAccessor(ref __TypeHandle.__Unity_Physics_Stateful_StatefulTriggerEvent_RO_BufferTypeHandle);
			IntPtr nativeArrayPtr2 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr3 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr(chunk, ref __TypeHandle.__SpellConfigComponentData_RO_ComponentTypeHandle);
			IntPtr nativeArrayPtr4 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr(chunk, ref __TypeHandle.__SpellComponentData_RO_ComponentTypeHandle);
			IntPtr nativeArrayPtr5 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr(chunk, ref __TypeHandle.__Unity_Physics_PhysicsCollider_RO_ComponentTypeHandle);
			IntPtr nativeArrayPtr6 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr(chunk, ref __TypeHandle.__SpellMovementComponentData_RO_ComponentTypeHandle);
			IntPtr nativeArrayPtr7 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr(chunk, ref __TypeHandle.__SpellElementEffectComponentData_RO_ComponentTypeHandle);
			IntPtr nativeArrayPtr8 = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(chunk, __TypeHandle.__Unity_Entities_Entity_TypeHandle);
			int count = chunk.Count;
			int num = 0;
			if (!useEnabledMask)
			{
				for (int i = 0; i < count; i++)
				{
					ref Spell1021MagicBreakerData magicBreaker = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1021MagicBreakerData>(nativeArrayPtr, i);
					DynamicBuffer<StatefulTriggerEvent> triggerEvent = bufferAccessor[i];
					ref LocalTransform transform = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr2, i);
					ref SpellConfigComponentData config = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr3, i);
					ref SpellComponentData data = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr4, i);
					ref PhysicsCollider collider = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsCollider>(nativeArrayPtr5, i);
					ref SpellMovementComponentData movement = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr6, i);
					ref SpellElementEffectComponentData spellElementEffect = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellElementEffectComponentData>(nativeArrayPtr7, i);
					Entity entity = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr8, i);
					Execute(chunkIndexInQuery, ref magicBreaker, in triggerEvent, ref transform, in config, in data, in collider, in movement, in spellElementEffect, entity);
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
						ref Spell1021MagicBreakerData magicBreaker2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1021MagicBreakerData>(nativeArrayPtr, nextRangeBegin);
						DynamicBuffer<StatefulTriggerEvent> triggerEvent2 = bufferAccessor[nextRangeBegin];
						ref LocalTransform transform2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr2, nextRangeBegin);
						ref SpellConfigComponentData config2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr3, nextRangeBegin);
						ref SpellComponentData data2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr4, nextRangeBegin);
						ref PhysicsCollider collider2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsCollider>(nativeArrayPtr5, nextRangeBegin);
						ref SpellMovementComponentData movement2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr6, nextRangeBegin);
						ref SpellElementEffectComponentData spellElementEffect2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellElementEffectComponentData>(nativeArrayPtr7, nextRangeBegin);
						Entity entity2 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr8, nextRangeBegin);
						Execute(chunkIndexInQuery, ref magicBreaker2, in triggerEvent2, ref transform2, in config2, in data2, in collider2, in movement2, in spellElementEffect2, entity2);
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
					ref Spell1021MagicBreakerData magicBreaker3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1021MagicBreakerData>(nativeArrayPtr, j);
					DynamicBuffer<StatefulTriggerEvent> triggerEvent3 = bufferAccessor[j];
					ref LocalTransform transform3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr2, j);
					ref SpellConfigComponentData config3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr3, j);
					ref SpellComponentData data3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr4, j);
					ref PhysicsCollider collider3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsCollider>(nativeArrayPtr5, j);
					ref SpellMovementComponentData movement3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr6, j);
					ref SpellElementEffectComponentData spellElementEffect3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellElementEffectComponentData>(nativeArrayPtr7, j);
					Entity entity3 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr8, j);
					Execute(chunkIndexInQuery, ref magicBreaker3, in triggerEvent3, ref transform3, in config3, in data3, in collider3, in movement3, in spellElementEffect3, entity3);
					num++;
				}
				num2 >>= 1;
			}
			num2 = chunkEnabledMask.ULong1;
			for (int k = 64; k < count; k++)
			{
				if ((num2 & 1) != 0L)
				{
					ref Spell1021MagicBreakerData magicBreaker4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell1021MagicBreakerData>(nativeArrayPtr, k);
					DynamicBuffer<StatefulTriggerEvent> triggerEvent4 = bufferAccessor[k];
					ref LocalTransform transform4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr2, k);
					ref SpellConfigComponentData config4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr3, k);
					ref SpellComponentData data4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr4, k);
					ref PhysicsCollider collider4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsCollider>(nativeArrayPtr5, k);
					ref SpellMovementComponentData movement4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr6, k);
					ref SpellElementEffectComponentData spellElementEffect4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellElementEffectComponentData>(nativeArrayPtr7, k);
					Entity entity4 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr8, k);
					Execute(chunkIndexInQuery, ref magicBreaker4, in triggerEvent4, ref transform4, in config4, in data4, in collider4, in movement4, in spellElementEffect4, entity4);
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

		public ComponentLookup<SpellConfigComponentData> __SpellConfigComponentData_RW_ComponentLookup;

		public ComponentLookup<UnitProperty_Dots> __UnitProperty_Dots_RW_ComponentLookup;

		public ComponentLookup<SpellComponentData> __SpellComponentData_RW_ComponentLookup;

		public ComponentLookup<SpellMovementComponentData> __SpellMovementComponentData_RW_ComponentLookup;

		public Spell1021DamageJob.InternalCompilerQueryAndHandleData __Spell1021MagicBreakerDamageSystem_Spell1021DamageJob_WithDefaultQuery_JobEntityTypeHandle;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__Unity_Transforms_LocalTransform_RW_ComponentLookup = state.GetComponentLookup<LocalTransform>();
			__SpellConfigComponentData_RW_ComponentLookup = state.GetComponentLookup<SpellConfigComponentData>();
			__UnitProperty_Dots_RW_ComponentLookup = state.GetComponentLookup<UnitProperty_Dots>();
			__SpellComponentData_RW_ComponentLookup = state.GetComponentLookup<SpellComponentData>();
			__SpellMovementComponentData_RW_ComponentLookup = state.GetComponentLookup<SpellMovementComponentData>();
			__Spell1021MagicBreakerDamageSystem_Spell1021DamageJob_WithDefaultQuery_JobEntityTypeHandle.Init(ref state, assignDefaultQuery: true);
		}
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void TryRecycleSpell_00006A03_0024PostfixBurstDelegate(ref EntityCommandBuffer.ParallelWriter CMD, in SpellSingleton spellSingleton, in Entity targetSpell, in SpellConfigComponentData targetConfig, int chunkIndex, in float3 targetSpellPos, in SpellConfigComponentData spellConfig, in SpellComponentData spellData, float scale, bool PlayHitAnim = true);

	internal static class TryRecycleSpell_00006A03_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<TryRecycleSpell_00006A03_0024PostfixBurstDelegate>(delegate(ref EntityCommandBuffer.ParallelWriter CMD, in SpellSingleton spellSingleton, in Entity targetSpell, in SpellConfigComponentData targetConfig, int chunkIndex, in float3 targetSpellPos, in SpellConfigComponentData spellConfig, in SpellComponentData spellData, float scale, bool PlayHitAnim = true)
				{
					Invoke(ref CMD, in spellSingleton, in targetSpell, in targetConfig, chunkIndex, in targetSpellPos, in spellConfig, in spellData, scale, PlayHitAnim);
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

		public unsafe static void Invoke(ref EntityCommandBuffer.ParallelWriter CMD, in SpellSingleton spellSingleton, in Entity targetSpell, in SpellConfigComponentData targetConfig, int chunkIndex, in float3 targetSpellPos, in SpellConfigComponentData spellConfig, in SpellComponentData spellData, float scale, bool PlayHitAnim = true)
		{
			if (BurstCompiler.IsEnabled)
			{
				IntPtr functionPointer = GetFunctionPointer();
				if (functionPointer != (IntPtr)0)
				{
					((delegate* unmanaged[Cdecl]<ref EntityCommandBuffer.ParallelWriter, ref SpellSingleton, ref Entity, ref SpellConfigComponentData, int, ref float3, ref SpellConfigComponentData, ref SpellComponentData, float, bool, void>)functionPointer)(ref CMD, ref spellSingleton, ref targetSpell, ref targetConfig, chunkIndex, ref targetSpellPos, ref spellConfig, ref spellData, scale, PlayHitAnim);
					return;
				}
			}
			TryRecycleSpell_0024BurstManaged(ref CMD, in spellSingleton, in targetSpell, in targetConfig, chunkIndex, in targetSpellPos, in spellConfig, in spellData, scale, PlayHitAnim);
		}
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void TryReflectTargetSpell_00006A04_0024PostfixBurstDelegate(ref EntityCommandBuffer.ParallelWriter CMD, in SpellSingleton spellSingleton, in Entity targetSpell, in Entity SpawnParamsEntity, in LocalTransform transform, in SpellConfigComponentData config, in SpellComponentData data, int ChunkIndex, in ComponentLookup<SpellConfigComponentData> SpellConfigLookup, in ComponentLookup<LocalTransform> LocalTransformLookUp, in ComponentLookup<SpellComponentData> SpellDataLookup, in ComponentLookup<SpellMovementComponentData> MovementLookUp);

	internal static class TryReflectTargetSpell_00006A04_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<TryReflectTargetSpell_00006A04_0024PostfixBurstDelegate>(delegate(ref EntityCommandBuffer.ParallelWriter CMD, in SpellSingleton spellSingleton, in Entity targetSpell, in Entity SpawnParamsEntity, in LocalTransform transform, in SpellConfigComponentData config, in SpellComponentData data, int ChunkIndex, in ComponentLookup<SpellConfigComponentData> SpellConfigLookup, in ComponentLookup<LocalTransform> LocalTransformLookUp, in ComponentLookup<SpellComponentData> SpellDataLookup, in ComponentLookup<SpellMovementComponentData> MovementLookUp)
				{
					Invoke(ref CMD, in spellSingleton, in targetSpell, in SpawnParamsEntity, in transform, in config, in data, ChunkIndex, in SpellConfigLookup, in LocalTransformLookUp, in SpellDataLookup, in MovementLookUp);
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

		public unsafe static void Invoke(ref EntityCommandBuffer.ParallelWriter CMD, in SpellSingleton spellSingleton, in Entity targetSpell, in Entity SpawnParamsEntity, in LocalTransform transform, in SpellConfigComponentData config, in SpellComponentData data, int ChunkIndex, in ComponentLookup<SpellConfigComponentData> SpellConfigLookup, in ComponentLookup<LocalTransform> LocalTransformLookUp, in ComponentLookup<SpellComponentData> SpellDataLookup, in ComponentLookup<SpellMovementComponentData> MovementLookUp)
		{
			if (BurstCompiler.IsEnabled)
			{
				IntPtr functionPointer = GetFunctionPointer();
				if (functionPointer != (IntPtr)0)
				{
					((delegate* unmanaged[Cdecl]<ref EntityCommandBuffer.ParallelWriter, ref SpellSingleton, ref Entity, ref Entity, ref LocalTransform, ref SpellConfigComponentData, ref SpellComponentData, int, ref ComponentLookup<SpellConfigComponentData>, ref ComponentLookup<LocalTransform>, ref ComponentLookup<SpellComponentData>, ref ComponentLookup<SpellMovementComponentData>, void>)functionPointer)(ref CMD, ref spellSingleton, ref targetSpell, ref SpawnParamsEntity, ref transform, ref config, ref data, ChunkIndex, ref SpellConfigLookup, ref LocalTransformLookUp, ref SpellDataLookup, ref MovementLookUp);
					return;
				}
			}
			TryReflectTargetSpell_0024BurstManaged(ref CMD, in spellSingleton, in targetSpell, in SpawnParamsEntity, in transform, in config, in data, ChunkIndex, in SpellConfigLookup, in LocalTransformLookUp, in SpellDataLookup, in MovementLookUp);
		}
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void __codegen__OnCreate_00006A0A_0024PostfixBurstDelegate(IntPtr self, IntPtr state);

	internal static class __codegen__OnCreate_00006A0A_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<__codegen__OnCreate_00006A0A_0024PostfixBurstDelegate>(delegate(IntPtr self, IntPtr state)
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

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void __codegen__OnUpdate_00006A0B_0024PostfixBurstDelegate(IntPtr self, IntPtr state);

	internal static class __codegen__OnUpdate_00006A0B_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<__codegen__OnUpdate_00006A0B_0024PostfixBurstDelegate>(delegate(IntPtr self, IntPtr state)
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

	private TypeHandle __TypeHandle;

	private EntityQuery __query_2075748057_0;

	private EntityQuery __query_2075748057_1;

	private EntityQuery __query_2075748057_2;

	private EntityQuery __query_2075748057_3;

	private EntityQuery __query_2075748057_4;

	[BurstCompile]
	[AOT.MonoPInvokeCallback(typeof(TryRecycleSpell_00006A03_0024PostfixBurstDelegate))]
	public static void TryRecycleSpell(ref EntityCommandBuffer.ParallelWriter CMD, in SpellSingleton spellSingleton, in Entity targetSpell, in SpellConfigComponentData targetConfig, int chunkIndex, in float3 targetSpellPos, in SpellConfigComponentData spellConfig, in SpellComponentData spellData, float scale, bool PlayHitAnim = true)
	{
		TryRecycleSpell_00006A03_0024BurstDirectCall.Invoke(ref CMD, in spellSingleton, in targetSpell, in targetConfig, chunkIndex, in targetSpellPos, in spellConfig, in spellData, scale, PlayHitAnim);
	}

	[BurstCompile]
	[AOT.MonoPInvokeCallback(typeof(TryReflectTargetSpell_00006A04_0024PostfixBurstDelegate))]
	public static void TryReflectTargetSpell(ref EntityCommandBuffer.ParallelWriter CMD, in SpellSingleton spellSingleton, in Entity targetSpell, in Entity SpawnParamsEntity, in LocalTransform transform, in SpellConfigComponentData config, in SpellComponentData data, int ChunkIndex, in ComponentLookup<SpellConfigComponentData> SpellConfigLookup, in ComponentLookup<LocalTransform> LocalTransformLookUp, in ComponentLookup<SpellComponentData> SpellDataLookup, in ComponentLookup<SpellMovementComponentData> MovementLookUp)
	{
		TryReflectTargetSpell_00006A04_0024BurstDirectCall.Invoke(ref CMD, in spellSingleton, in targetSpell, in SpawnParamsEntity, in transform, in config, in data, ChunkIndex, in SpellConfigLookup, in LocalTransformLookUp, in SpellDataLookup, in MovementLookUp);
	}

	[BurstCompile]
	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<PhysicsWorldSingleton>();
		state.RequireForUpdate<CurrentRoomEntitiesSingleton>();
		state.RequireForUpdate<SpellSpawnParams>();
		state.RequireForUpdate<SpellSingleton>();
		state.RequireForUpdate<EndSpellSimulationEntityCommandBufferSystem.Singleton>();
		state.RequireForUpdate<SEData>();
		state.RequireForUpdate<SpellEffectSystem.UnfollowingRequire>();
		state.RequireForUpdate<ScreenShakeData>();
		state.RequireForUpdate<GlobalRandom>();
		state.RequireForUpdate<Spell1021MagicBreakerData>();
	}

	[BurstCompile]
	public void OnUpdate(ref SystemState state)
	{
		EntityCommandBuffer.ParallelWriter cMD = __query_2075748057_0.GetSingleton<EndSpellSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter();
		SpellSingleton singleton = __query_2075748057_1.GetSingleton<SpellSingleton>();
		state.Dependency = __ScheduleViaJobChunkExtension_0(new Spell1021DamageJob
		{
			LocalTransformLookUp = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state),
			SpellConfigLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__SpellConfigComponentData_RW_ComponentLookup, ref state),
			UnitPropertyLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__UnitProperty_Dots_RW_ComponentLookup, ref state),
			SEPlayerSingleton = __query_2075748057_2.GetSingletonEntity(),
			SpellSingleton = singleton,
			SpellDataLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__SpellComponentData_RW_ComponentLookup, ref state),
			MovementLookUp = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__SpellMovementComponentData_RW_ComponentLookup, ref state),
			Random = __query_2075748057_3.GetSingleton<GlobalRandom>(),
			SpawnParamsEntity = __query_2075748057_4.GetSingletonEntity(),
			CMD = cMD
		}, __TypeHandle.__Spell1021MagicBreakerDamageSystem_Spell1021DamageJob_WithDefaultQuery_JobEntityTypeHandle.DefaultQuery, state.Dependency, ref state, hasUserDefinedQuery: false);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private JobHandle __ScheduleViaJobChunkExtension_0(Spell1021DamageJob job, EntityQuery query, JobHandle dependency, ref SystemState state, bool hasUserDefinedQuery)
	{
		dependency = __TypeHandle.__Spell1021MagicBreakerDamageSystem_Spell1021DamageJob_WithDefaultQuery_JobEntityTypeHandle.UpdateBaseEntityIndexArray(ref job, query, dependency, ref state);
		__TypeHandle.__Spell1021MagicBreakerDamageSystem_Spell1021DamageJob_WithDefaultQuery_JobEntityTypeHandle.AssignEntityManager(ref job, state.EntityManager);
		__TypeHandle.__Spell1021MagicBreakerDamageSystem_Spell1021DamageJob_WithDefaultQuery_JobEntityTypeHandle.__TypeHandle.Update(ref state);
		return __TypeHandle.__Spell1021MagicBreakerDamageSystem_Spell1021DamageJob_WithDefaultQuery_JobEntityTypeHandle.ScheduleParallel(ref job, query, dependency);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<EndSpellSimulationEntityCommandBufferSystem.Singleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_2075748057_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_2075748057_1 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SEData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_2075748057_2 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<GlobalRandom>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_2075748057_3 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellSpawnParams>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_2075748057_4 = entityQueryBuilder2.Build(ref state);
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
		__codegen__OnCreate_00006A0A_0024BurstDirectCall.Invoke(self, state);
	}

	[BurstCompile]
	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal static void __codegen__OnUpdate(IntPtr self, IntPtr state)
	{
		__codegen__OnUpdate_00006A0B_0024BurstDirectCall.Invoke(self, state);
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
	{
		((Spell1021MagicBreakerDamageSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[BurstCompile]
	internal static void TryRecycleSpell_0024BurstManaged(ref EntityCommandBuffer.ParallelWriter CMD, in SpellSingleton spellSingleton, in Entity targetSpell, in SpellConfigComponentData targetConfig, int chunkIndex, in float3 targetSpellPos, in SpellConfigComponentData spellConfig, in SpellComponentData spellData, float scale, bool PlayHitAnim = true)
	{
		if (targetConfig.AbilityType != SpellAbilityType.Dash)
		{
			if (PlayHitAnim)
			{
				float3 position = targetSpellPos + new float3(0f, 0.3f, 0f);
				float3 direction = new float3(0f, 1f, 0f);
				CMD.CreateSpellHitEffect(chunkIndex, in spellSingleton, in spellConfig, in spellData, in position, in direction, scale);
			}
			CMD.SetComponentEnabled<SpellDestroyTag>(chunkIndex, targetSpell, value: true);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[BurstCompile]
	internal static void TryReflectTargetSpell_0024BurstManaged(ref EntityCommandBuffer.ParallelWriter CMD, in SpellSingleton spellSingleton, in Entity targetSpell, in Entity SpawnParamsEntity, in LocalTransform transform, in SpellConfigComponentData config, in SpellComponentData data, int ChunkIndex, in ComponentLookup<SpellConfigComponentData> SpellConfigLookup, in ComponentLookup<LocalTransform> LocalTransformLookUp, in ComponentLookup<SpellComponentData> SpellDataLookup, in ComponentLookup<SpellMovementComponentData> MovementLookUp)
	{
		if (!SpellConfigLookup.TryGetComponent(targetSpell, out var componentData) || !LocalTransformLookUp.TryGetComponent(targetSpell, out var componentData2) || !SpellDataLookup.TryGetComponent(targetSpell, out var componentData3) || !MovementLookUp.TryGetComponent(targetSpell, out var componentData4) || DTool.IsSameCamp(componentData.ShooterType, config.ShooterType))
		{
			return;
		}
		int num;
		if (config.Level != 1)
		{
			int id = componentData.Id;
			if (id != 90381 && id != 90301 && id != 90431 && id != 90391)
			{
				num = ((!DTool.IsSameCamp(componentData.ShooterType, UnitType.Monster)) ? (DTool.IsSameCamp(componentData.ShooterType, UnitType.Player) ? 3 : 7) : 0);
				goto IL_00e5;
			}
		}
		num = 7;
		goto IL_00e5;
		IL_00e5:
		UnitType unitType = (UnitType)num;
		TryRecycleSpell(ref CMD, in spellSingleton, in targetSpell, in componentData, ChunkIndex, in componentData2.Position, in config, in data, transform.Scale, unitType == UnitType.NotAttack);
		if (unitType != UnitType.NotAttack)
		{
			UnityEngine.Debug.Log("refract");
			float3 direction = -componentData4.Direction;
			if (LocalTransformLookUp.TryGetComponent(componentData3.OwnerEntity, out var componentData5) && config.Level == 3)
			{
				direction = DTool.IgnoreZDir(in componentData5.Position, in componentData2.Position);
			}
			SpellSpawnParams element = spellSingleton.SpellSpawnParamsStorage[targetSpell].BuildMagicBreakerRefractBullet(unitType, componentData2.Position, direction);
			CMD.AppendToBuffer(ChunkIndex, SpawnParamsEntity, element);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	[BurstCompile]
	internal unsafe static void __codegen__OnCreate_0024BurstManaged(IntPtr self, IntPtr state)
	{
		((Spell1021MagicBreakerDamageSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[BurstCompile]
	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnUpdate_0024BurstManaged(IntPtr self, IntPtr state)
	{
		((Spell1021MagicBreakerDamageSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
	}
}
