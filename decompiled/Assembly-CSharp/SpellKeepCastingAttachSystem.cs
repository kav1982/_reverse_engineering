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

[UpdateBefore(typeof(SpellEndSystemGroup))]
[UpdateAfter(typeof(PlayerControllerSystem))]
[BurstCompile]
[CompilerGenerated]
public struct SpellKeepCastingAttachSystem : ISystem, ISystemCompilerGenerated
{
	[CompilerGenerated]
	[BurstCompile]
	private struct SpellKeepCastingAttachJob : IJobEntity, IJobChunk
	{
		public struct InternalCompilerQueryAndHandleData
		{
			public struct TypeHandle
			{
				public ComponentTypeHandle<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentTypeHandle;

				public ComponentTypeHandle<SpellMovementComponentData> __SpellMovementComponentData_RW_ComponentTypeHandle;

				public ComponentTypeHandle<SpellComponentData> __SpellComponentData_RW_ComponentTypeHandle;

				[ReadOnly]
				public ComponentTypeHandle<SpellConfigComponentData> __SpellConfigComponentData_RO_ComponentTypeHandle;

				[ReadOnly]
				public ComponentTypeHandle<SpellKeepCastingAttach> __SpellKeepCastingAttach_RO_ComponentTypeHandle;

				[ReadOnly]
				public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public void __AssignHandles(ref SystemState state)
				{
					__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle = state.GetComponentTypeHandle<LocalTransform>();
					__SpellMovementComponentData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellMovementComponentData>();
					__SpellComponentData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellComponentData>();
					__SpellConfigComponentData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<SpellConfigComponentData>(isReadOnly: true);
					__SpellKeepCastingAttach_RO_ComponentTypeHandle = state.GetComponentTypeHandle<SpellKeepCastingAttach>(isReadOnly: true);
					__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
				}

				public void Update(ref SystemState state)
				{
					__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle.Update(ref state);
					__SpellMovementComponentData_RW_ComponentTypeHandle.Update(ref state);
					__SpellComponentData_RW_ComponentTypeHandle.Update(ref state);
					__SpellConfigComponentData_RO_ComponentTypeHandle.Update(ref state);
					__SpellKeepCastingAttach_RO_ComponentTypeHandle.Update(ref state);
					__Unity_Entities_Entity_TypeHandle.Update(ref state);
				}
			}

			public TypeHandle __TypeHandle;

			public EntityQuery DefaultQuery;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private void __AssignQueries(ref SystemState state)
			{
				EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
				EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellConfigComponentData>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAll<SpellKeepCastingAttach>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<LocalTransform>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellMovementComponentData>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellComponentData>();
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
			public void Run(ref SpellKeepCastingAttachJob job, EntityQuery query)
			{
				job.__TypeHandle = __TypeHandle;
				JobChunkExtensions.RunByRef(ref job, query);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle Schedule(ref SpellKeepCastingAttachJob job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = __TypeHandle;
				return JobChunkExtensions.ScheduleByRef(ref job, query, dependency);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle ScheduleParallel(ref SpellKeepCastingAttachJob job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = __TypeHandle;
				return JobChunkExtensions.ScheduleParallelByRef(ref job, query, dependency);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void UpdateBaseEntityIndexArray(ref SpellKeepCastingAttachJob job, EntityQuery query, ref SystemState state)
			{
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle UpdateBaseEntityIndexArray(ref SpellKeepCastingAttachJob job, EntityQuery query, JobHandle dependency, ref SystemState state)
			{
				return dependency;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void AssignEntityManager(ref SpellKeepCastingAttachJob job, EntityManager entityManager)
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

		public float3 PlayerShootPosition;

		public float3 PlayerDir;

		public float3 PlayerAimPosition;

		public Entity PlayerEntity;

		[NativeDisableContainerSafetyRestriction]
		[NativeDisableParallelForRestriction]
		public ComponentLookup<LocalTransform> LocalTransformLookup;

		[NativeDisableParallelForRestriction]
		[NativeDisableContainerSafetyRestriction]
		public ComponentLookup<SpellMovementComponentData> MovementDataLookup;

		[NativeDisableParallelForRestriction]
		[NativeDisableContainerSafetyRestriction]
		public ComponentLookup<Spell4005WandSpiritData> WandSpiritDataLookUp;

		[NativeDisableParallelForRestriction]
		[NativeDisableContainerSafetyRestriction]
		public ComponentLookup<PhysicsVelocity> VelocityLookup;

		[ReadOnly]
		public ComponentLookup<SpellChargingTag> ChargingTagLookup;

		[ReadOnly]
		public ComponentLookup<SpellFromChargeModeStar> FromChargeModeLookup;

		[ReadOnly]
		public ComponentLookup<Spell4004StartData> Spell4004StarLookup;

		[ReadOnly]
		public ComponentLookup<SpellFromFourDirectionWandData> SpellFromFourDirectionWandLookup;

		[ReadOnly]
		public ComponentLookup<Spell2005GrimoireData> Spell2005BookLookup;

		private InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

		[BurstCompile]
		private void Execute(ref LocalTransform transform, ref SpellMovementComponentData movement, ref SpellComponentData data, in SpellConfigComponentData config, in SpellKeepCastingAttach attach, Entity spell)
		{
			if (VelocityLookup.HasComponent(spell))
			{
				VelocityLookup.GetRefRW(spell).ValueRW.Linear = float3.zero;
			}
			if (movement.Type == SpellSpecialMovementType.Rotation && !ChargingTagLookup.HasComponent(spell))
			{
				transform.Position = movement.UpdateAroundFollowAndGetAroundPositionWhenAround(LocalTransformLookup);
				movement.Direction = Tool2D.GetDir(movement.AroundAngle + 90f);
				return;
			}
			if (movement.IsFallSpell)
			{
				if (movement.Type == SpellSpecialMovementType.Rotation)
				{
					transform.Position = movement.UpdateAroundFollowAndGetAroundPositionWhenAround(LocalTransformLookup);
					transform.Position.z = -7f;
				}
				else if (data.Shooter == PlayerEntity)
				{
					transform.Position = PlayerAimPosition + math.normalizesafe(PlayerShootPosition - PlayerAimPosition) * 2f;
					transform.Position.z = -7f;
					transform.Position.x += attach.FallPositionOffset.x;
					transform.Position.y += attach.FallPositionOffset.y;
					float3 @float = (movement.Direction = DTool.GetShiftedDir(in PlayerDir, attach.DirOffset));
					movement.FallTargetPosition = transform.Position + movement.Direction * 2f;
				}
				return;
			}
			float3 float2 = PlayerShootPosition;
			float3 oldDir = PlayerDir;
			Spell4005WandSpiritData componentData2;
			Spell2005GrimoireData componentData3;
			LocalTransform componentData4;
			LocalTransform componentData5;
			if (FromChargeModeLookup.TryGetComponent(spell, out var componentData) && LocalTransformLookup.HasComponent(componentData.StarEntity))
			{
				float2 = LocalTransformLookup[componentData.StarEntity].Position;
				oldDir = Spell4004StarLookup[componentData.StarEntity].WandShootDirection;
			}
			else if (WandSpiritDataLookUp.TryGetComponent(data.Shooter, out componentData2))
			{
				oldDir = componentData2.WandLookDirection;
				float2 = componentData2.WandLookDirection * 0.35f + new float3(0f, 0f, -0.4f) + LocalTransformLookup[data.Shooter].Position;
			}
			else if (Spell2005BookLookup.TryGetComponent(data.Shooter, out componentData3) && LocalTransformLookup.TryGetComponent(data.Shooter, out componentData4))
			{
				oldDir = MovementDataLookup[data.Shooter].Direction;
				float2 = componentData4.Position - new float3(0f, 0f, componentData3.BookFloatingHeight);
			}
			else if (data.Shooter != PlayerEntity && LocalTransformLookup.TryGetComponent(data.Shooter, out componentData5))
			{
				oldDir = MovementDataLookup[data.Shooter].Direction;
				float2 = componentData5.Position;
			}
			float3 shiftedDir2 = DTool.GetShiftedDir(in oldDir, 90f);
			float3 float3 = attach.Offset * shiftedDir2;
			transform.Position = float2 + float3;
			if (SpellFromFourDirectionWandLookup.TryGetComponent(spell, out var componentData6))
			{
				oldDir = DTool.GetShiftedDir(in oldDir, componentData6.Angle);
			}
			if (config.AbilityType != SpellAbilityType.DragonBreath || movement.Type == SpellSpecialMovementType.Normal)
			{
				SpellAbilityType abilityType = config.AbilityType;
				if (abilityType == SpellAbilityType.DragonBreath || abilityType == SpellAbilityType.MagicBreaker)
				{
					movement.Direction = oldDir;
				}
				else
				{
					movement.Direction = DTool.GetShiftedDir(in oldDir, attach.DirOffset);
				}
			}
		}

		[CompilerGenerated]
		public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
		{
			IntPtr nativeArrayPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr2 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellMovementComponentData_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr3 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellComponentData_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr4 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr(chunk, ref __TypeHandle.__SpellConfigComponentData_RO_ComponentTypeHandle);
			IntPtr nativeArrayPtr5 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr(chunk, ref __TypeHandle.__SpellKeepCastingAttach_RO_ComponentTypeHandle);
			IntPtr nativeArrayPtr6 = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(chunk, __TypeHandle.__Unity_Entities_Entity_TypeHandle);
			int count = chunk.Count;
			int num = 0;
			if (!useEnabledMask)
			{
				for (int i = 0; i < count; i++)
				{
					ref LocalTransform transform = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr, i);
					ref SpellMovementComponentData movement = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr2, i);
					ref SpellComponentData data = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr3, i);
					ref SpellConfigComponentData config = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr4, i);
					ref SpellKeepCastingAttach attach = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellKeepCastingAttach>(nativeArrayPtr5, i);
					Entity spell = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr6, i);
					Execute(ref transform, ref movement, ref data, in config, in attach, spell);
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
						ref SpellMovementComponentData movement2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr2, nextRangeBegin);
						ref SpellComponentData data2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr3, nextRangeBegin);
						ref SpellConfigComponentData config2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr4, nextRangeBegin);
						ref SpellKeepCastingAttach attach2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellKeepCastingAttach>(nativeArrayPtr5, nextRangeBegin);
						Entity spell2 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr6, nextRangeBegin);
						Execute(ref transform2, ref movement2, ref data2, in config2, in attach2, spell2);
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
					ref SpellMovementComponentData movement3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr2, j);
					ref SpellComponentData data3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr3, j);
					ref SpellConfigComponentData config3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr4, j);
					ref SpellKeepCastingAttach attach3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellKeepCastingAttach>(nativeArrayPtr5, j);
					Entity spell3 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr6, j);
					Execute(ref transform3, ref movement3, ref data3, in config3, in attach3, spell3);
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
					ref SpellMovementComponentData movement4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr2, k);
					ref SpellComponentData data4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr3, k);
					ref SpellConfigComponentData config4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr4, k);
					ref SpellKeepCastingAttach attach4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellKeepCastingAttach>(nativeArrayPtr5, k);
					Entity spell4 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr6, k);
					Execute(ref transform4, ref movement4, ref data4, in config4, in attach4, spell4);
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

		public ComponentLookup<SpellChargingTag> __SpellChargingTag_RW_ComponentLookup;

		public ComponentLookup<SpellMovementComponentData> __SpellMovementComponentData_RW_ComponentLookup;

		public ComponentLookup<Spell4005WandSpiritData> __Spell4005WandSpiritData_RW_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<SpellFromChargeModeStar> __SpellFromChargeModeStar_RO_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<Spell4004StartData> __Spell4004StartData_RO_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<Spell2005GrimoireData> __Spell2005GrimoireData_RO_ComponentLookup;

		public ComponentLookup<PhysicsVelocity> __Unity_Physics_PhysicsVelocity_RW_ComponentLookup;

		public ComponentLookup<SpellFromFourDirectionWandData> __SpellFromFourDirectionWandData_RW_ComponentLookup;

		public SpellKeepCastingAttachJob.InternalCompilerQueryAndHandleData __SpellKeepCastingAttachSystem_SpellKeepCastingAttachJob_WithDefaultQuery_JobEntityTypeHandle;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__Unity_Transforms_LocalTransform_RW_ComponentLookup = state.GetComponentLookup<LocalTransform>();
			__SpellChargingTag_RW_ComponentLookup = state.GetComponentLookup<SpellChargingTag>();
			__SpellMovementComponentData_RW_ComponentLookup = state.GetComponentLookup<SpellMovementComponentData>();
			__Spell4005WandSpiritData_RW_ComponentLookup = state.GetComponentLookup<Spell4005WandSpiritData>();
			__SpellFromChargeModeStar_RO_ComponentLookup = state.GetComponentLookup<SpellFromChargeModeStar>(isReadOnly: true);
			__Spell4004StartData_RO_ComponentLookup = state.GetComponentLookup<Spell4004StartData>(isReadOnly: true);
			__Spell2005GrimoireData_RO_ComponentLookup = state.GetComponentLookup<Spell2005GrimoireData>(isReadOnly: true);
			__Unity_Physics_PhysicsVelocity_RW_ComponentLookup = state.GetComponentLookup<PhysicsVelocity>();
			__SpellFromFourDirectionWandData_RW_ComponentLookup = state.GetComponentLookup<SpellFromFourDirectionWandData>();
			__SpellKeepCastingAttachSystem_SpellKeepCastingAttachJob_WithDefaultQuery_JobEntityTypeHandle.Init(ref state, assignDefaultQuery: true);
		}
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void __codegen__OnCreate_000083C9_0024PostfixBurstDelegate(IntPtr self, IntPtr state);

	internal static class __codegen__OnCreate_000083C9_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<__codegen__OnCreate_000083C9_0024PostfixBurstDelegate>(delegate(IntPtr self, IntPtr state)
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

	private TypeHandle __TypeHandle;

	private EntityQuery __query_1953946845_0;

	[BurstCompile]
	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<PlayerController_Dots>();
		state.RequireForUpdate<SpellKeepCastingAttach>();
	}

	public void OnUpdate(ref SystemState state)
	{
		state.Dependency = __ScheduleViaJobChunkExtension_0(new SpellKeepCastingAttachJob
		{
			PlayerDir = PlayerMgr.Inst.PlayerDir,
			PlayerShootPosition = PlayerMgr.Inst.ShootPoint,
			PlayerAimPosition = PlayerMgr.Inst.GetMousePoint(),
			LocalTransformLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state),
			PlayerEntity = __query_1953946845_0.GetSingletonEntity(),
			ChargingTagLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__SpellChargingTag_RW_ComponentLookup, ref state),
			MovementDataLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__SpellMovementComponentData_RW_ComponentLookup, ref state),
			WandSpiritDataLookUp = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__Spell4005WandSpiritData_RW_ComponentLookup, ref state),
			FromChargeModeLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__SpellFromChargeModeStar_RO_ComponentLookup, ref state),
			Spell4004StarLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__Spell4004StartData_RO_ComponentLookup, ref state),
			Spell2005BookLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__Spell2005GrimoireData_RO_ComponentLookup, ref state),
			VelocityLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__Unity_Physics_PhysicsVelocity_RW_ComponentLookup, ref state),
			SpellFromFourDirectionWandLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__SpellFromFourDirectionWandData_RW_ComponentLookup, ref state)
		}, __TypeHandle.__SpellKeepCastingAttachSystem_SpellKeepCastingAttachJob_WithDefaultQuery_JobEntityTypeHandle.DefaultQuery, state.Dependency, ref state, hasUserDefinedQuery: false);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private JobHandle __ScheduleViaJobChunkExtension_0(SpellKeepCastingAttachJob job, EntityQuery query, JobHandle dependency, ref SystemState state, bool hasUserDefinedQuery)
	{
		dependency = __TypeHandle.__SpellKeepCastingAttachSystem_SpellKeepCastingAttachJob_WithDefaultQuery_JobEntityTypeHandle.UpdateBaseEntityIndexArray(ref job, query, dependency, ref state);
		__TypeHandle.__SpellKeepCastingAttachSystem_SpellKeepCastingAttachJob_WithDefaultQuery_JobEntityTypeHandle.AssignEntityManager(ref job, state.EntityManager);
		__TypeHandle.__SpellKeepCastingAttachSystem_SpellKeepCastingAttachJob_WithDefaultQuery_JobEntityTypeHandle.__TypeHandle.Update(ref state);
		return __TypeHandle.__SpellKeepCastingAttachSystem_SpellKeepCastingAttachJob_WithDefaultQuery_JobEntityTypeHandle.ScheduleParallel(ref job, query, dependency);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<PlayerController_Dots>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1953946845_0 = entityQueryBuilder2.Build(ref state);
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
		__codegen__OnCreate_000083C9_0024BurstDirectCall.Invoke(self, state);
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnUpdate(IntPtr self, IntPtr state)
	{
		((SpellKeepCastingAttachSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
	{
		((SpellKeepCastingAttachSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[BurstCompile]
	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreate_0024BurstManaged(IntPtr self, IntPtr state)
	{
		((SpellKeepCastingAttachSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
	}
}
