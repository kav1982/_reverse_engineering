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
using UnityEngine;

[UpdateInGroup(typeof(UnitBaseSystemGroup))]
[CompilerGenerated]
[BurstCompile]
internal struct Monster310System : ISystem, ISystemCompilerGenerated
{
	public struct Monster310AttackRequest : IBufferElementData
	{
		public Vector3 requestPoint;

		public Entity masterEntity;

		public bool buffed;
	}

	[CompilerGenerated]
	[BurstCompile]
	public struct Monster310Job : IJobEntity, IJobChunk
	{
		public struct InternalCompilerQueryAndHandleData
		{
			public struct TypeHandle
			{
				public ComponentTypeHandle<Monster310_Dots> __Monster310_Dots_RW_ComponentTypeHandle;

				public ComponentTypeHandle<PhysicsVelocity> __Unity_Physics_PhysicsVelocity_RW_ComponentTypeHandle;

				public ComponentTypeHandle<UnitProperty_Dots> __UnitProperty_Dots_RW_ComponentTypeHandle;

				public ComponentTypeHandle<EndlessMonsterTag> __EndlessMonsterTag_RW_ComponentTypeHandle;

				public ComponentTypeHandle<UnitBase_Dots> __UnitBase_Dots_RW_ComponentTypeHandle;

				public ComponentTypeHandle<PathFinding> __PathFinding_RW_ComponentTypeHandle;

				[ReadOnly]
				public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public void __AssignHandles(ref SystemState state)
				{
					__Monster310_Dots_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Monster310_Dots>();
					__Unity_Physics_PhysicsVelocity_RW_ComponentTypeHandle = state.GetComponentTypeHandle<PhysicsVelocity>();
					__UnitProperty_Dots_RW_ComponentTypeHandle = state.GetComponentTypeHandle<UnitProperty_Dots>();
					__EndlessMonsterTag_RW_ComponentTypeHandle = state.GetComponentTypeHandle<EndlessMonsterTag>();
					__UnitBase_Dots_RW_ComponentTypeHandle = state.GetComponentTypeHandle<UnitBase_Dots>();
					__PathFinding_RW_ComponentTypeHandle = state.GetComponentTypeHandle<PathFinding>();
					__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
				}

				public void Update(ref SystemState state)
				{
					__Monster310_Dots_RW_ComponentTypeHandle.Update(ref state);
					__Unity_Physics_PhysicsVelocity_RW_ComponentTypeHandle.Update(ref state);
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
				EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAllRW<Monster310_Dots>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<PhysicsVelocity>();
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
			public void Run(ref Monster310Job job, EntityQuery query)
			{
				job.__TypeHandle = __TypeHandle;
				JobChunkExtensions.RunByRef(ref job, query);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle Schedule(ref Monster310Job job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = __TypeHandle;
				return JobChunkExtensions.ScheduleByRef(ref job, query, dependency);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle ScheduleParallel(ref Monster310Job job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = __TypeHandle;
				return JobChunkExtensions.ScheduleParallelByRef(ref job, query, dependency);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void UpdateBaseEntityIndexArray(ref Monster310Job job, EntityQuery query, ref SystemState state)
			{
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle UpdateBaseEntityIndexArray(ref Monster310Job job, EntityQuery query, JobHandle dependency, ref SystemState state)
			{
				return dependency;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void AssignEntityManager(ref Monster310Job job, EntityManager entityManager)
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

		public EntityCommandBuffer.ParallelWriter ecb;

		public Entity playerEtt;

		public float3 pattern2PlayerMotion;

		public Entity attackRequestEntity;

		public Entity SEBufferEntity;

		public CurrentRoomEntitiesSingleton CurrentRoomEntities;

		[NativeDisableParallelForRestriction]
		public ComponentLookup<LocalTransform> LocalTsfLookUp;

		[NativeDisableParallelForRestriction]
		public ComponentLookup<AnimaPlay> AnimaLookUp;

		[NativeDisableUnsafePtrRestriction]
		public RefRW<GlobalRandom> globalRandom;

		public float deltaTime;

		private InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

		public void Execute([ChunkIndexInQuery] int index, ref Monster310_Dots monster, ref PhysicsVelocity velocity, ref UnitProperty_Dots ppt, ref EndlessMonsterTag endlessTag, ref UnitBase_Dots unitBase, ref PathFinding pathFinding, Entity entity)
		{
			LocalTransform localTransform = LocalTsfLookUp[entity];
			ref AnimaPlay valueRW = ref AnimaLookUp.GetRefRW(unitBase.ett_AnimaRoot).ValueRW;
			Monster310_Dots.Monster310_Data monster310_Data = default(Monster310_Dots.Monster310_Data);
			if (monster.pattern == AIPattern.Pattern1)
			{
				monster310_Data = monster.data.Value;
			}
			else
			{
				Monster310_Dots.Monster310_Data monster310_Data2 = default(Monster310_Dots.Monster310_Data);
				monster310_Data2.gravity = monster.data2.Value.gravity;
				monster310_Data2.maxJumpDistance = monster.data2.Value.maxJumpDistance;
				monster310_Data2.upSpeed = monster.data2.Value.upSpeed;
				monster310_Data = monster310_Data2;
			}
			if (!monster.Initialized)
			{
				monster.Initialized = true;
				monster.state = Monster310State.Move;
				monster.stateExistTime = 2f;
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
			float num = Tool2D.IgnoreZDistanceSqr(@float, localTransform.Position);
			switch (monster.state)
			{
			case Monster310State.Move:
				if (monster.changedState)
				{
					valueRW.Play(0);
				}
				if (flag)
				{
					pathFinding.UpdatePath(localTransform.Position, @float, 16, monster.changedState);
				}
				unitBase.SetMove(ppt.MoveSpeed * Tool2D.IgnoreZV2ToV1Normal(pathFinding.walkToPoint, localTransform.Position));
				if (monster.stateExistTime > 3f && flag && num < monster310_Data.maxJumpDistance * monster310_Data.maxJumpDistance)
				{
					monster.state = Monster310State.JumpPrepare;
					monster.lastAimPoint = @float;
				}
				break;
			case Monster310State.JumpPrepare:
				if (monster.changedState)
				{
					valueRW.Play(1);
				}
				unitBase.SetMove(float3.zero, thisTimeShouldFlip: false);
				if (monster.stateExistTime > 0.5f)
				{
					monster.state = Monster310State.Jump;
				}
				if (flag)
				{
					monster.lastAimPoint = @float;
				}
				unitBase.SetFlip(monster.lastAimPoint.x < localTransform.Position.x);
				break;
			case Monster310State.Jump:
				if (monster.changedState)
				{
					if (monster.pattern == AIPattern.Pattern2)
					{
						ecb.AppendToBuffer(index, SEBufferEntity, new SEData("SE_Monster310_Jump"));
					}
					valueRW.Play(2);
					if (flag)
					{
						monster.lastAimPoint = @float;
					}
					if (localTransform.Position.z > 0f)
					{
						localTransform.Position.z = 0f;
					}
					float3 dir = DTool.GetDir(ref globalRandom.ValueRW.random, in monster.jumpOffsetRange);
					float3 float2 = monster.lastAimPoint + dir;
					if (monster.pattern == AIPattern.Pattern2 && flag && unitBase.targetEtt == playerEtt)
					{
						float2 += pattern2PlayerMotion;
					}
					pathFinding.samplePointRequest.SetRequest(float2);
				}
				if (pathFinding.samplePointRequest.requestState == NavMeshRequestState.Completed)
				{
					pathFinding.samplePointRequest.requestState = NavMeshRequestState.Unused;
					Vector3 vector = (float3)pathFinding.samplePointRequest.result - localTransform.Position;
					velocity.Linear = GeneralTool.CannonSpeed(monster310_Data.upSpeed, 0f, monster310_Data.gravity, Mathf.Min(monster310_Data.maxJumpDistance, vector.magnitude)) * vector.normalized;
					unitBase.JumpStart(monster310_Data.upSpeed, monster310_Data.gravity);
					ppt.JumpStartSetting();
					ppt.CanTouch = false;
					if (localTransform.Position.z > 0f)
					{
						localTransform.Position.z = 0f;
					}
				}
				unitBase.SetMove(float3.zero, thisTimeShouldFlip: false);
				if (localTransform.Position.z > 0f && unitBase.baseIsJumping)
				{
					localTransform.Position.z = 0f;
					unitBase.JumpStop();
					ppt.JumpStopSetting();
					monster.state = Monster310State.JumpAfter;
				}
				break;
			case Monster310State.JumpAfter:
				if (monster.changedState)
				{
					valueRW.Play(3);
					if (monster.pattern == AIPattern.Pattern2)
					{
						ecb.AppendToBuffer(index, attackRequestEntity, new Monster310AttackRequest
						{
							masterEntity = entity,
							requestPoint = localTransform.Position,
							buffed = endlessTag.has316Buff
						});
					}
				}
				velocity.Linear = float3.zero;
				unitBase.SetMove(float3.zero, thisTimeShouldFlip: false);
				if (monster.stateExistTime > 0.5f)
				{
					ppt.CanTouch = true;
					monster.state = Monster310State.Move;
				}
				break;
			}
		}

		[CompilerGenerated]
		public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
		{
			IntPtr nativeArrayPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Monster310_Dots_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr2 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Unity_Physics_PhysicsVelocity_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr3 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__UnitProperty_Dots_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr4 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__EndlessMonsterTag_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr5 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__UnitBase_Dots_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr6 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__PathFinding_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr7 = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(chunk, __TypeHandle.__Unity_Entities_Entity_TypeHandle);
			int count = chunk.Count;
			int num = 0;
			if (!useEnabledMask)
			{
				for (int i = 0; i < count; i++)
				{
					ref Monster310_Dots monster = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Monster310_Dots>(nativeArrayPtr, i);
					ref PhysicsVelocity velocity = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsVelocity>(nativeArrayPtr2, i);
					ref UnitProperty_Dots ppt = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitProperty_Dots>(nativeArrayPtr3, i);
					ref EndlessMonsterTag endlessTag = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<EndlessMonsterTag>(nativeArrayPtr4, i);
					ref UnitBase_Dots unitBase = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitBase_Dots>(nativeArrayPtr5, i);
					ref PathFinding pathFinding = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PathFinding>(nativeArrayPtr6, i);
					Entity entity = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr7, i);
					Execute(chunkIndexInQuery, ref monster, ref velocity, ref ppt, ref endlessTag, ref unitBase, ref pathFinding, entity);
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
						ref Monster310_Dots monster2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Monster310_Dots>(nativeArrayPtr, nextRangeBegin);
						ref PhysicsVelocity velocity2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsVelocity>(nativeArrayPtr2, nextRangeBegin);
						ref UnitProperty_Dots ppt2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitProperty_Dots>(nativeArrayPtr3, nextRangeBegin);
						ref EndlessMonsterTag endlessTag2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<EndlessMonsterTag>(nativeArrayPtr4, nextRangeBegin);
						ref UnitBase_Dots unitBase2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitBase_Dots>(nativeArrayPtr5, nextRangeBegin);
						ref PathFinding pathFinding2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PathFinding>(nativeArrayPtr6, nextRangeBegin);
						Entity entity2 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr7, nextRangeBegin);
						Execute(chunkIndexInQuery, ref monster2, ref velocity2, ref ppt2, ref endlessTag2, ref unitBase2, ref pathFinding2, entity2);
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
					ref Monster310_Dots monster3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Monster310_Dots>(nativeArrayPtr, j);
					ref PhysicsVelocity velocity3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsVelocity>(nativeArrayPtr2, j);
					ref UnitProperty_Dots ppt3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitProperty_Dots>(nativeArrayPtr3, j);
					ref EndlessMonsterTag endlessTag3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<EndlessMonsterTag>(nativeArrayPtr4, j);
					ref UnitBase_Dots unitBase3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitBase_Dots>(nativeArrayPtr5, j);
					ref PathFinding pathFinding3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PathFinding>(nativeArrayPtr6, j);
					Entity entity3 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr7, j);
					Execute(chunkIndexInQuery, ref monster3, ref velocity3, ref ppt3, ref endlessTag3, ref unitBase3, ref pathFinding3, entity3);
					num++;
				}
				num2 >>= 1;
			}
			num2 = chunkEnabledMask.ULong1;
			for (int k = 64; k < count; k++)
			{
				if ((num2 & 1) != 0L)
				{
					ref Monster310_Dots monster4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Monster310_Dots>(nativeArrayPtr, k);
					ref PhysicsVelocity velocity4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsVelocity>(nativeArrayPtr2, k);
					ref UnitProperty_Dots ppt4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitProperty_Dots>(nativeArrayPtr3, k);
					ref EndlessMonsterTag endlessTag4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<EndlessMonsterTag>(nativeArrayPtr4, k);
					ref UnitBase_Dots unitBase4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitBase_Dots>(nativeArrayPtr5, k);
					ref PathFinding pathFinding4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PathFinding>(nativeArrayPtr6, k);
					Entity entity4 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr7, k);
					Execute(chunkIndexInQuery, ref monster4, ref velocity4, ref ppt4, ref endlessTag4, ref unitBase4, ref pathFinding4, entity4);
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

		public ComponentLookup<AnimaPlay> __AnimaPlay_RW_ComponentLookup;

		public Monster310Job.InternalCompilerQueryAndHandleData __Monster310System_Monster310Job_WithDefaultQuery_JobEntityTypeHandle;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__Unity_Transforms_LocalTransform_RW_ComponentLookup = state.GetComponentLookup<LocalTransform>();
			__AnimaPlay_RW_ComponentLookup = state.GetComponentLookup<AnimaPlay>();
			__Monster310System_Monster310Job_WithDefaultQuery_JobEntityTypeHandle.Init(ref state, assignDefaultQuery: true);
		}
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void __codegen__OnCreate_00008A8E_0024PostfixBurstDelegate(IntPtr self, IntPtr state);

	internal static class __codegen__OnCreate_00008A8E_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<__codegen__OnCreate_00008A8E_0024PostfixBurstDelegate>(delegate(IntPtr self, IntPtr state)
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

	private EntityQuery __query_797997377_0;

	private EntityQuery __query_797997377_1;

	private EntityQuery __query_797997377_2;

	private EntityQuery __query_797997377_3;

	private EntityQuery __query_797997377_4;

	[BurstCompile]
	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<SEData>();
		state.RequireForUpdate<CurrentRoomEntitiesSingleton>();
		state.RequireForUpdate<Monster310AttackRequest>();
		state.RequireForUpdate<Monster310_Dots>();
		state.EntityManager.CreateSingletonBuffer<Monster310AttackRequest>();
	}

	public void OnUpdate(ref SystemState state)
	{
		RefRW<GlobalRandom> singletonRW = __query_797997377_0.GetSingletonRW<GlobalRandom>();
		singletonRW.ValueRW.NewRandom();
		EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer(Allocator.TempJob);
		state.Dependency = __ScheduleViaJobChunkExtension_0(new Monster310Job
		{
			ecb = entityCommandBuffer.AsParallelWriter(),
			playerEtt = PlayerMgr.Inst.PlayerEtt,
			pattern2PlayerMotion = PlayerMgr.Inst.PlayerCtrller.CurrentMotion * 0.1f,
			attackRequestEntity = __query_797997377_1.GetSingletonEntity(),
			SEBufferEntity = __query_797997377_2.GetSingletonEntity(),
			CurrentRoomEntities = __query_797997377_3.GetSingleton<CurrentRoomEntitiesSingleton>(),
			LocalTsfLookUp = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state),
			AnimaLookUp = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__AnimaPlay_RW_ComponentLookup, ref state),
			deltaTime = state.WorldUnmanaged.Time.DeltaTime,
			globalRandom = singletonRW
		}, __TypeHandle.__Monster310System_Monster310Job_WithDefaultQuery_JobEntityTypeHandle.DefaultQuery, state.Dependency, ref state, hasUserDefinedQuery: false);
		state.Dependency.Complete();
		entityCommandBuffer.Playback(state.EntityManager);
		entityCommandBuffer.Dispose();
		state.Dependency.Complete();
		DynamicBuffer<Monster310AttackRequest> singletonBuffer = __query_797997377_4.GetSingletonBuffer<Monster310AttackRequest>();
		if (singletonBuffer.Length <= 0)
		{
			return;
		}
		foreach (Monster310AttackRequest item in singletonBuffer)
		{
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Monster310_Drop", item.requestPoint, 3f).GetComponent<Monster310_Drop>().Initialize(item.masterEntity, item.buffed);
		}
		singletonBuffer.Clear();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private JobHandle __ScheduleViaJobChunkExtension_0(Monster310Job job, EntityQuery query, JobHandle dependency, ref SystemState state, bool hasUserDefinedQuery)
	{
		dependency = __TypeHandle.__Monster310System_Monster310Job_WithDefaultQuery_JobEntityTypeHandle.UpdateBaseEntityIndexArray(ref job, query, dependency, ref state);
		__TypeHandle.__Monster310System_Monster310Job_WithDefaultQuery_JobEntityTypeHandle.AssignEntityManager(ref job, state.EntityManager);
		__TypeHandle.__Monster310System_Monster310Job_WithDefaultQuery_JobEntityTypeHandle.__TypeHandle.Update(ref state);
		return __TypeHandle.__Monster310System_Monster310Job_WithDefaultQuery_JobEntityTypeHandle.ScheduleParallel(ref job, query, dependency);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAllRW<GlobalRandom>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_797997377_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<Monster310AttackRequest>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_797997377_1 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SEData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_797997377_2 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<CurrentRoomEntitiesSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_797997377_3 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAllRW<Monster310AttackRequest>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_797997377_4 = entityQueryBuilder2.Build(ref state);
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
		__codegen__OnCreate_00008A8E_0024BurstDirectCall.Invoke(self, state);
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnUpdate(IntPtr self, IntPtr state)
	{
		((Monster310System*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
	{
		((Monster310System*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[BurstCompile]
	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreate_0024BurstManaged(IntPtr self, IntPtr state)
	{
		((Monster310System*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
	}
}
