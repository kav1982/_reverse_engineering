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
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

[UpdateInGroup(typeof(UnitBaseSystemGroup))]
[CompilerGenerated]
[BurstCompile]
internal struct Monster312System : ISystem, ISystemCompilerGenerated
{
	[CompilerGenerated]
	[BurstCompile]
	public struct Monster312Job : IJobEntity, IJobChunk
	{
		public struct InternalCompilerQueryAndHandleData
		{
			public struct TypeHandle
			{
				public ComponentTypeHandle<Monster312_Dots> __Monster312_Dots_RW_ComponentTypeHandle;

				public ComponentTypeHandle<UnitProperty_Dots> __UnitProperty_Dots_RW_ComponentTypeHandle;

				public ComponentTypeHandle<EndlessMonsterTag> __EndlessMonsterTag_RW_ComponentTypeHandle;

				public ComponentTypeHandle<UnitBase_Dots> __UnitBase_Dots_RW_ComponentTypeHandle;

				public ComponentTypeHandle<PathFinding> __PathFinding_RW_ComponentTypeHandle;

				[ReadOnly]
				public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public void __AssignHandles(ref SystemState state)
				{
					__Monster312_Dots_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Monster312_Dots>();
					__UnitProperty_Dots_RW_ComponentTypeHandle = state.GetComponentTypeHandle<UnitProperty_Dots>();
					__EndlessMonsterTag_RW_ComponentTypeHandle = state.GetComponentTypeHandle<EndlessMonsterTag>();
					__UnitBase_Dots_RW_ComponentTypeHandle = state.GetComponentTypeHandle<UnitBase_Dots>();
					__PathFinding_RW_ComponentTypeHandle = state.GetComponentTypeHandle<PathFinding>();
					__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
				}

				public void Update(ref SystemState state)
				{
					__Monster312_Dots_RW_ComponentTypeHandle.Update(ref state);
					__UnitProperty_Dots_RW_ComponentTypeHandle.Update(ref state);
					__EndlessMonsterTag_RW_ComponentTypeHandle.Update(ref state);
					__UnitBase_Dots_RW_ComponentTypeHandle.Update(ref state);
					__PathFinding_RW_ComponentTypeHandle.Update(ref state);
					__Unity_Entities_Entity_TypeHandle.Update(ref state);
				}
			}

			public TypeHandle __TypeHandle;

			public EntityQuery DefaultQuery;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private void __AssignQueries(ref SystemState state)
			{
				EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
				EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAllRW<Monster312_Dots>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<UnitProperty_Dots>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<EndlessMonsterTag>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<UnitBase_Dots>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<PathFinding>();
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
			public void Run(ref Monster312Job job, EntityQuery query)
			{
				job.__TypeHandle = __TypeHandle;
				JobChunkExtensions.RunByRef(ref job, query);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle Schedule(ref Monster312Job job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = __TypeHandle;
				return JobChunkExtensions.ScheduleByRef(ref job, query, dependency);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle ScheduleParallel(ref Monster312Job job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = __TypeHandle;
				return JobChunkExtensions.ScheduleParallelByRef(ref job, query, dependency);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void UpdateBaseEntityIndexArray(ref Monster312Job job, EntityQuery query, ref SystemState state)
			{
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle UpdateBaseEntityIndexArray(ref Monster312Job job, EntityQuery query, JobHandle dependency, ref SystemState state)
			{
				return dependency;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void AssignEntityManager(ref Monster312Job job, EntityManager entityManager)
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

		public SpellSpawnParams ssp;

		public Entity ShootSpellBufferEntity;

		public Entity SEBufferEntity;

		public CurrentRoomEntitiesSingleton CurrentRoomEntities;

		[NativeDisableParallelForRestriction]
		public ComponentLookup<LocalTransform> LocalTsfLookUp;

		[NativeDisableParallelForRestriction]
		public ComponentLookup<PostTransformMatrix> PostTransformMatrixLookUp;

		[NativeDisableParallelForRestriction]
		public ComponentLookup<AnimaPlay> AnimaLookUp;

		[NativeDisableUnsafePtrRestriction]
		public RefRW<GlobalRandom> globalRandom;

		[NativeDisableParallelForRestriction]
		public ComponentLookup<MatOverrideMixPercent> matLookUp;

		[NativeDisableParallelForRestriction]
		public ComponentLookup<MatOverrideFill> matFillLookUp;

		[NativeDisableParallelForRestriction]
		public ComponentLookup<MatOverrideColor> matColorLookUp;

		public float deltaTime;

		public EntityCommandBuffer.ParallelWriter ecb;

		public Entity tpBufferEntity;

		public Entity tpEffectBufferEntity;

		private InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

		public void Execute([ChunkIndexInQuery] int index, ref Monster312_Dots monster, ref UnitProperty_Dots ppt, ref EndlessMonsterTag endlessTag, ref UnitBase_Dots unitBase, ref PathFinding pathFinding, Entity entity)
		{
			LocalTransform localTransform = LocalTsfLookUp[entity];
			ref AnimaPlay valueRW = ref AnimaLookUp.GetRefRW(unitBase.ett_AnimaRoot).ValueRW;
			ref MatOverrideMixPercent valueRW2 = ref matLookUp.GetRefRW(monster.matEntity).ValueRW;
			if (!monster.Initialized)
			{
				monster.Initialized = true;
				monster.state = Monster312State.Move;
				ecb.SetComponentEnabled<MaterialMeshInfo>(index, monster.warningEntity, value: false);
				if (monster.aIPattern == AIPattern.Pattern2)
				{
					ecb.SetComponentEnabled<MaterialMeshInfo>(index, monster.warningEntity2, value: false);
				}
			}
			valueRW.SetLockMotion(ppt.LockMotion);
			if (ppt.LockMotion)
			{
				return;
			}
			if (monster.stateQuit)
			{
				monster.stateQuit = false;
				monster.changedState = true;
			}
			else
			{
				monster.changedState = false;
			}
			monster.stateExistTime += deltaTime;
			bool flag = false;
			float3 @float = default(float3);
			if (!LocalTsfLookUp.HasComponent(unitBase.targetEtt))
			{
				if (CurrentRoomEntities.FindNearestTarget(localTransform.Position, UnitType.Monster, out var target, out var _, out var _))
				{
					unitBase.targetEtt = target;
				}
				else
				{
					unitBase.targetEtt = Entity.Null;
				}
			}
			else
			{
				@float = LocalTsfLookUp[unitBase.targetEtt].Position;
				flag = true;
			}
			switch (monster.state)
			{
			case Monster312State.Move:
				if (monster.changedState)
				{
					valueRW.Play(1);
					monster.moveDir = DTool.GetDir(ref globalRandom.ValueRW.random);
				}
				if (flag)
				{
					pathFinding.UpdatePath(localTransform.Position, @float, 32);
				}
				unitBase.SetMove(ppt.MoveSpeed * Tool2D.IgnoreZV2ToV1Normal(pathFinding.walkToPoint, localTransform.Position));
				if (monster.stateExistTime > 1.5f)
				{
					if (flag)
					{
						monster.state = Monster312State.TeleportBefore;
					}
					else
					{
						monster.state = Monster312State.Move;
					}
				}
				break;
			case Monster312State.TeleportBefore:
				if (monster.changedState)
				{
					valueRW.Play(2);
					monster.tpPosition = @float;
					ecb.AppendToBuffer(index, tpBufferEntity, new Monster312TpBuffer
					{
						targetPosition = @float,
						targetEntity = unitBase.targetEtt,
						selfEntity = entity,
						flip = unitBase.currentFlip,
						pattern2 = (monster.aIPattern == AIPattern.Pattern2)
					});
					ecb.SetComponentEnabled<MaterialMeshInfo>(index, monster.warningEntity, value: true);
					RefRW<PostTransformMatrix> refRW = PostTransformMatrixLookUp.GetRefRW(monster.warningEntity);
					refRW.ValueRW.Value = Matrix4x4.Scale(new Vector3((!unitBase.currentFlip) ? 1 : (-1), 1f, 1f) * monster.warningScale);
					if (monster.aIPattern == AIPattern.Pattern2)
					{
						ecb.SetComponentEnabled<MaterialMeshInfo>(index, monster.warningEntity2, value: true);
						refRW = PostTransformMatrixLookUp.GetRefRW(monster.warningEntity2);
						refRW.ValueRW.Value = Matrix4x4.Scale(new Vector3((!unitBase.currentFlip) ? 1 : (-1), 1f, 1f) * monster.warningScale2);
					}
				}
				unitBase.SetMove(float3.zero, thisTimeShouldFlip: false);
				LocalTsfLookUp.GetRefRW(monster.warningEntity).ValueRW.Position = (float3)Tool2D.GetLayerPoint(monster.tpPosition) + monster.warningOffset - localTransform.Position;
				valueRW2.mixPercent = monster.stateExistTime / 0.9f;
				matFillLookUp.GetRefRW(monster.warningEntity).ValueRW.fill = valueRW2.mixPercent;
				if (monster.aIPattern == AIPattern.Pattern2)
				{
					LocalTsfLookUp.GetRefRW(monster.warningEntity2).ValueRW.Position = (float3)Tool2D.GetLayerPoint(monster.tpPosition) + monster.warningOffset - localTransform.Position;
					matColorLookUp.GetRefRW(monster.warningEntity2).ValueRW.color = new Color(1f, 1f, 1f, Mathf.Clamp01(monster.stateExistTime / 0.5f));
				}
				if (monster.stateExistTime > 0.9f)
				{
					monster.state = Monster312State.Teleport;
				}
				break;
			case Monster312State.Teleport:
				if (monster.changedState)
				{
					valueRW.Play(3);
					ecb.SetComponentEnabled<MaterialMeshInfo>(index, monster.warningEntity, value: false);
					if (monster.aIPattern == AIPattern.Pattern2)
					{
						ecb.SetComponentEnabled<MaterialMeshInfo>(index, monster.warningEntity2, value: false);
					}
					LocalTsfLookUp.GetRefRW(entity).ValueRW.Position = monster.tpPosition;
					ecb.AppendToBuffer(index, tpEffectBufferEntity, new Monster312TpEffectBuffer
					{
						selfPosition = monster.tpPosition,
						isPattern1 = (monster.aIPattern == AIPattern.Pattern1)
					});
					ecb.AppendToBuffer(index, tpEffectBufferEntity, new Monster312TpEffectBuffer
					{
						selfPosition = localTransform.Position,
						isPattern1 = (monster.aIPattern == AIPattern.Pattern1)
					});
					ecb.AppendToBuffer(index, SEBufferEntity, new SEData("SE_Monster312_Teleport"));
					if (monster.aIPattern == AIPattern.Pattern2)
					{
						Vector3 up = Vector3.up;
						if (endlessTag.has316Buff)
						{
							ssp.ConfigComponentData.Damage.Base *= 1f;
						}
						for (int i = 0; i < 6; i++)
						{
							ssp.SetShooter(entity, entity);
							ssp.SpawnPosition = monster.tpPosition + new float3(0f, 0f, -0.5f);
							Vector3 dir = Tool2D.GetDir(up, 60f * (float)i);
							ssp.MovementComponentData.Speed *= ppt.affect_MucusSpellSpeedRatio;
							ssp.MovementComponentData.Direction = dir;
							ecb.AppendToBuffer(index, ShootSpellBufferEntity, ssp);
						}
					}
				}
				valueRW2.mixPercent = 1f - monster.stateExistTime / 0.4f;
				unitBase.SetMove(float3.zero, thisTimeShouldFlip: false);
				if (monster.stateExistTime > 0.4f)
				{
					monster.state = Monster312State.Move;
					valueRW2.mixPercent = 0f;
				}
				break;
			}
		}

		[CompilerGenerated]
		public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
		{
			IntPtr nativeArrayPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Monster312_Dots_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr2 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__UnitProperty_Dots_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr3 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__EndlessMonsterTag_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr4 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__UnitBase_Dots_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr5 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__PathFinding_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr6 = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(chunk, __TypeHandle.__Unity_Entities_Entity_TypeHandle);
			int count = chunk.Count;
			int num = 0;
			if (!useEnabledMask)
			{
				for (int i = 0; i < count; i++)
				{
					ref Monster312_Dots monster = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Monster312_Dots>(nativeArrayPtr, i);
					ref UnitProperty_Dots ppt = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitProperty_Dots>(nativeArrayPtr2, i);
					ref EndlessMonsterTag endlessTag = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<EndlessMonsterTag>(nativeArrayPtr3, i);
					ref UnitBase_Dots unitBase = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitBase_Dots>(nativeArrayPtr4, i);
					ref PathFinding pathFinding = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PathFinding>(nativeArrayPtr5, i);
					Entity entity = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr6, i);
					Execute(chunkIndexInQuery, ref monster, ref ppt, ref endlessTag, ref unitBase, ref pathFinding, entity);
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
						ref Monster312_Dots monster2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Monster312_Dots>(nativeArrayPtr, nextRangeBegin);
						ref UnitProperty_Dots ppt2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitProperty_Dots>(nativeArrayPtr2, nextRangeBegin);
						ref EndlessMonsterTag endlessTag2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<EndlessMonsterTag>(nativeArrayPtr3, nextRangeBegin);
						ref UnitBase_Dots unitBase2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitBase_Dots>(nativeArrayPtr4, nextRangeBegin);
						ref PathFinding pathFinding2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PathFinding>(nativeArrayPtr5, nextRangeBegin);
						Entity entity2 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr6, nextRangeBegin);
						Execute(chunkIndexInQuery, ref monster2, ref ppt2, ref endlessTag2, ref unitBase2, ref pathFinding2, entity2);
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
					ref Monster312_Dots monster3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Monster312_Dots>(nativeArrayPtr, j);
					ref UnitProperty_Dots ppt3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitProperty_Dots>(nativeArrayPtr2, j);
					ref EndlessMonsterTag endlessTag3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<EndlessMonsterTag>(nativeArrayPtr3, j);
					ref UnitBase_Dots unitBase3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitBase_Dots>(nativeArrayPtr4, j);
					ref PathFinding pathFinding3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PathFinding>(nativeArrayPtr5, j);
					Entity entity3 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr6, j);
					Execute(chunkIndexInQuery, ref monster3, ref ppt3, ref endlessTag3, ref unitBase3, ref pathFinding3, entity3);
					num++;
				}
				num2 >>= 1;
			}
			num2 = chunkEnabledMask.ULong1;
			for (int k = 64; k < count; k++)
			{
				if ((num2 & 1) != 0L)
				{
					ref Monster312_Dots monster4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Monster312_Dots>(nativeArrayPtr, k);
					ref UnitProperty_Dots ppt4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitProperty_Dots>(nativeArrayPtr2, k);
					ref EndlessMonsterTag endlessTag4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<EndlessMonsterTag>(nativeArrayPtr3, k);
					ref UnitBase_Dots unitBase4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitBase_Dots>(nativeArrayPtr4, k);
					ref PathFinding pathFinding4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PathFinding>(nativeArrayPtr5, k);
					Entity entity4 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr6, k);
					Execute(chunkIndexInQuery, ref monster4, ref ppt4, ref endlessTag4, ref unitBase4, ref pathFinding4, entity4);
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

		public ComponentLookup<PostTransformMatrix> __Unity_Transforms_PostTransformMatrix_RW_ComponentLookup;

		public ComponentLookup<AnimaPlay> __AnimaPlay_RW_ComponentLookup;

		public ComponentLookup<MatOverrideMixPercent> __MatOverrideMixPercent_RW_ComponentLookup;

		public ComponentLookup<MatOverrideFill> __MatOverrideFill_RW_ComponentLookup;

		public ComponentLookup<MatOverrideColor> __MatOverrideColor_RW_ComponentLookup;

		public Monster312Job.InternalCompilerQueryAndHandleData __Monster312System_Monster312Job_WithDefaultQuery_JobEntityTypeHandle;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__Unity_Transforms_LocalTransform_RW_ComponentLookup = state.GetComponentLookup<LocalTransform>();
			__Unity_Transforms_PostTransformMatrix_RW_ComponentLookup = state.GetComponentLookup<PostTransformMatrix>();
			__AnimaPlay_RW_ComponentLookup = state.GetComponentLookup<AnimaPlay>();
			__MatOverrideMixPercent_RW_ComponentLookup = state.GetComponentLookup<MatOverrideMixPercent>();
			__MatOverrideFill_RW_ComponentLookup = state.GetComponentLookup<MatOverrideFill>();
			__MatOverrideColor_RW_ComponentLookup = state.GetComponentLookup<MatOverrideColor>();
			__Monster312System_Monster312Job_WithDefaultQuery_JobEntityTypeHandle.Init(ref state, assignDefaultQuery: true);
		}
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void __codegen__OnCreate_00008AF6_0024PostfixBurstDelegate(IntPtr self, IntPtr state);

	internal static class __codegen__OnCreate_00008AF6_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<__codegen__OnCreate_00008AF6_0024PostfixBurstDelegate>(delegate(IntPtr self, IntPtr state)
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

	private EntityQuery __query_1986125832_0;

	private EntityQuery __query_1986125832_1;

	private EntityQuery __query_1986125832_2;

	private EntityQuery __query_1986125832_3;

	private EntityQuery __query_1986125832_4;

	private EntityQuery __query_1986125832_5;

	private EntityQuery __query_1986125832_6;

	private EntityQuery __query_1986125832_7;

	[BurstCompile]
	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<SEData>();
		state.RequireForUpdate<SpellSpawnParams>();
		state.RequireForUpdate<Monster312TpEffectBuffer>();
		state.RequireForUpdate<Monster312TpBuffer>();
		state.RequireForUpdate<CurrentRoomEntitiesSingleton>();
		state.RequireForUpdate<Monster312_Dots>();
		state.EntityManager.CreateSingletonBuffer<Monster312TpBuffer>();
		state.EntityManager.CreateSingletonBuffer<Monster312TpEffectBuffer>();
	}

	public void OnUpdate(ref SystemState state)
	{
		SpellSpawnParams ssp = UnitDotsSyncSystem.GetSpellPrototype(90471);
		UnitBase.UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
		sSPModifier.Duration = 10f;
		sSPModifier.Damage = 10f;
		sSPModifier.Damage *= GameConstManaged.endlessMonsterDamageRatio;
		sSPModifier.Speed = 4f;
		sSPModifier.ApplyToSSP(ref ssp);
		ssp.DisableResize = true;
		EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer(Allocator.TempJob);
		state.Dependency = __ScheduleViaJobChunkExtension_0(new Monster312Job
		{
			ssp = ssp,
			CurrentRoomEntities = __query_1986125832_0.GetSingleton<CurrentRoomEntitiesSingleton>(),
			LocalTsfLookUp = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state),
			PostTransformMatrixLookUp = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__Unity_Transforms_PostTransformMatrix_RW_ComponentLookup, ref state),
			AnimaLookUp = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__AnimaPlay_RW_ComponentLookup, ref state),
			deltaTime = state.WorldUnmanaged.Time.DeltaTime,
			ecb = entityCommandBuffer.AsParallelWriter(),
			globalRandom = __query_1986125832_1.GetSingletonRW<GlobalRandom>(),
			tpBufferEntity = __query_1986125832_2.GetSingletonEntity(),
			tpEffectBufferEntity = __query_1986125832_3.GetSingletonEntity(),
			ShootSpellBufferEntity = __query_1986125832_4.GetSingletonEntity(),
			matLookUp = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__MatOverrideMixPercent_RW_ComponentLookup, ref state),
			matFillLookUp = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__MatOverrideFill_RW_ComponentLookup, ref state),
			matColorLookUp = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__MatOverrideColor_RW_ComponentLookup, ref state),
			SEBufferEntity = __query_1986125832_5.GetSingletonEntity()
		}, __TypeHandle.__Monster312System_Monster312Job_WithDefaultQuery_JobEntityTypeHandle.DefaultQuery, state.Dependency, ref state, hasUserDefinedQuery: false);
		state.Dependency.Complete();
		entityCommandBuffer.Playback(state.EntityManager);
		entityCommandBuffer.Dispose();
		NativeArray<Monster312TpBuffer> nativeArray = __query_1986125832_6.GetSingletonBuffer<Monster312TpBuffer>().ToNativeArray(Allocator.Temp);
		foreach (Monster312TpBuffer item in nativeArray)
		{
			Vector3 startPoint = (Vector3)item.targetPosition + Tool2D.GetDir() * UnityEngine.Random.Range(3f, 5f);
			startPoint = Tool2D.GetNavMeshPointIngoreZ(startPoint);
			Monster312_Dots componentData = state.EntityManager.GetComponentData<Monster312_Dots>(item.selfEntity);
			componentData.tpPosition = startPoint;
			state.EntityManager.SetComponentData(item.selfEntity, componentData);
		}
		nativeArray.Dispose();
		__query_1986125832_6.GetSingletonBuffer<Monster312TpBuffer>().Clear();
		NativeArray<Monster312TpEffectBuffer> nativeArray2 = __query_1986125832_7.GetSingletonBuffer<Monster312TpEffectBuffer>().ToNativeArray(Allocator.Temp);
		foreach (Monster312TpEffectBuffer item2 in nativeArray2)
		{
			if (item2.isPattern1)
			{
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Monster312TpEffect", Tool2D.IgnoreZPoint(item2.selfPosition), 2f);
			}
			else
			{
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Monster312TpEffect_2", Tool2D.IgnoreZPoint(item2.selfPosition), 2f);
			}
		}
		nativeArray2.Dispose();
		__query_1986125832_7.GetSingletonBuffer<Monster312TpEffectBuffer>().Clear();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private JobHandle __ScheduleViaJobChunkExtension_0(Monster312Job job, EntityQuery query, JobHandle dependency, ref SystemState state, bool hasUserDefinedQuery)
	{
		dependency = __TypeHandle.__Monster312System_Monster312Job_WithDefaultQuery_JobEntityTypeHandle.UpdateBaseEntityIndexArray(ref job, query, dependency, ref state);
		__TypeHandle.__Monster312System_Monster312Job_WithDefaultQuery_JobEntityTypeHandle.AssignEntityManager(ref job, state.EntityManager);
		__TypeHandle.__Monster312System_Monster312Job_WithDefaultQuery_JobEntityTypeHandle.__TypeHandle.Update(ref state);
		return __TypeHandle.__Monster312System_Monster312Job_WithDefaultQuery_JobEntityTypeHandle.ScheduleParallel(ref job, query, dependency);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<CurrentRoomEntitiesSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1986125832_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAllRW<GlobalRandom>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1986125832_1 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<Monster312TpBuffer>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1986125832_2 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<Monster312TpEffectBuffer>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1986125832_3 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellSpawnParams>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1986125832_4 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SEData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1986125832_5 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAllRW<Monster312TpBuffer>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1986125832_6 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAllRW<Monster312TpEffectBuffer>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1986125832_7 = entityQueryBuilder2.Build(ref state);
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
		__codegen__OnCreate_00008AF6_0024BurstDirectCall.Invoke(self, state);
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnUpdate(IntPtr self, IntPtr state)
	{
		((Monster312System*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
	{
		((Monster312System*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[BurstCompile]
	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreate_0024BurstManaged(IntPtr self, IntPtr state)
	{
		((Monster312System*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
	}
}
