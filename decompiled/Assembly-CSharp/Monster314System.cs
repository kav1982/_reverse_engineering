using System;
using System.Collections;
using System.Collections.Generic;
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
using UnityEngine;

[CompilerGenerated]
[BurstCompile]
internal struct Monster314System : ISystem, ISystemCompilerGenerated
{
	[CompilerGenerated]
	[BurstCompile]
	public struct Monster314Job : IJobEntity, IJobChunk
	{
		public struct InternalCompilerQueryAndHandleData
		{
			public struct TypeHandle
			{
				public ComponentTypeHandle<Monster314_Dots> __Monster314_Dots_RW_ComponentTypeHandle;

				public ComponentTypeHandle<UnitBase_Dots> __UnitBase_Dots_RW_ComponentTypeHandle;

				public ComponentTypeHandle<PathFinding> __PathFinding_RW_ComponentTypeHandle;

				[ReadOnly]
				public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public void __AssignHandles(ref SystemState state)
				{
					__Monster314_Dots_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Monster314_Dots>();
					__UnitBase_Dots_RW_ComponentTypeHandle = state.GetComponentTypeHandle<UnitBase_Dots>();
					__PathFinding_RW_ComponentTypeHandle = state.GetComponentTypeHandle<PathFinding>();
					__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
				}

				public void Update(ref SystemState state)
				{
					__Monster314_Dots_RW_ComponentTypeHandle.Update(ref state);
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
				EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAllRW<Monster314_Dots>();
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
			public void Run(ref Monster314Job job, EntityQuery query)
			{
				job.__TypeHandle = __TypeHandle;
				JobChunkExtensions.RunByRef(ref job, query);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle Schedule(ref Monster314Job job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = __TypeHandle;
				return JobChunkExtensions.ScheduleByRef(ref job, query, dependency);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle ScheduleParallel(ref Monster314Job job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = __TypeHandle;
				return JobChunkExtensions.ScheduleParallelByRef(ref job, query, dependency);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void UpdateBaseEntityIndexArray(ref Monster314Job job, EntityQuery query, ref SystemState state)
			{
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle UpdateBaseEntityIndexArray(ref Monster314Job job, EntityQuery query, JobHandle dependency, ref SystemState state)
			{
				return dependency;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void AssignEntityManager(ref Monster314Job job, EntityManager entityManager)
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

		public CurrentRoomEntitiesSingleton CurrentRoomEntities;

		[NativeDisableParallelForRestriction]
		public ComponentLookup<LocalTransform> LocalTsfLookUp;

		[NativeDisableParallelForRestriction]
		public ComponentLookup<AnimaPlay> AnimaLookUp;

		[NativeDisableParallelForRestriction]
		public ComponentLookup<UnitProperty_Dots> PPtLookUp;

		[NativeDisableUnsafePtrRestriction]
		public RefRW<GlobalRandom> globalRandom;

		public float deltaTime;

		public EntityCommandBuffer.ParallelWriter ecb;

		public Entity textFloatVFXBufferEtt;

		public Entity healEffectEntity;

		public float3 roomCenter;

		public float roomWidth;

		public float roomHeight;

		private InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

		public void Execute([ChunkIndexInQuery] int index, ref Monster314_Dots monster, ref UnitBase_Dots unitBase, ref PathFinding pathFinding, Entity entity)
		{
			LocalTransform localTransform = LocalTsfLookUp[entity];
			ref AnimaPlay valueRW = ref AnimaLookUp.GetRefRW(unitBase.ett_AnimaRoot).ValueRW;
			RefRW<UnitProperty_Dots> refRW = PPtLookUp.GetRefRW(entity);
			ref UnitProperty_Dots valueRW2 = ref refRW.ValueRW;
			if (!monster.Initialized)
			{
				monster.Initialized = true;
				monster.state = Monster314State.RandomMove;
			}
			valueRW.SetLockMotion(valueRW2.LockMotion);
			if (valueRW2.LockMotion)
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
			if (monster.state == Monster314State.RandomMove)
			{
				if (monster.changedState)
				{
					monster.randomMoveTimer = 0f;
					monster.randomMoveDir = DTool.GetDir(ref globalRandom.ValueRW.random);
				}
				monster.randomMoveTimer += deltaTime;
				if (Tool2D.PointWithinRange(localTransform.Position, roomCenter, roomWidth - 2f, roomHeight - 2f) != (Vector3)localTransform.Position)
				{
					monster.randomMoveDir = Tool2D.IgnoreZV2ToV1Normal(roomCenter, localTransform.Position);
				}
				if (monster.randomMoveTimer > 2f)
				{
					monster.randomMoveTimer = 0f;
					if (!LocalTsfLookUp.HasComponent(monster.followEntity))
					{
						if (CurrentRoomEntities.FindNearestTarget(localTransform.Position, UnitType.Player, out var target, out var _, out var _))
						{
							refRW = PPtLookUp.GetRefRW(target);
							ref UnitProperty_Dots valueRW3 = ref refRW.ValueRW;
							if (valueRW2.unitCfg.id != valueRW3.unitCfg.id)
							{
								monster.randomMoveDir = Tool2D.IgnoreZV2ToV1Normal(LocalTsfLookUp.GetRefRO(target).ValueRO.Position, localTransform.Position);
								monster.followEntity = target;
							}
							else
							{
								monster.randomMoveDir = DTool.GetDir(ref globalRandom.ValueRW.random);
							}
						}
						else
						{
							monster.randomMoveDir = DTool.GetDir(ref globalRandom.ValueRW.random);
						}
					}
					else
					{
						monster.randomMoveDir = Tool2D.IgnoreZV2ToV1Normal(LocalTsfLookUp.GetRefRO(monster.followEntity).ValueRO.Position, localTransform.Position);
					}
				}
				pathFinding.UpdatePath(localTransform.Position, localTransform.Position + monster.randomMoveDir, 32);
				unitBase.SetMove(valueRW2.MoveSpeed * monster.randomMoveDir);
			}
			monster.cureCDTimer += deltaTime;
			if (!(monster.cureCDTimer > 1f) || !(monster.cureHpPercentTotal > 0f))
			{
				return;
			}
			monster.cureCDTimer = 0f;
			CurrentRoomEntities.FindValidTargetsInRange(localTransform.Position, monster.cureRadius, UnitType.Player, out var target2, out var _, out var _);
			foreach (Entity item in target2)
			{
				if (monster.cureHpPercentTotal < 0f)
				{
					break;
				}
				if (!(Tool2D.IgnoreZDistanceSqr(localTransform.Position, LocalTsfLookUp.GetRefRO(item).ValueRO.Position) < monster.cureRadius * monster.cureRadius))
				{
					continue;
				}
				refRW = PPtLookUp.GetRefRW(item);
				ref UnitProperty_Dots valueRW4 = ref refRW.ValueRW;
				if (valueRW4.unitCfg.currentHP != valueRW4.unitCfg.maxHP && valueRW4.id != 131401)
				{
					if (valueRW4.unitCfg.currentHP + monster.cureHpPercent * valueRW2.unitCfg.maxHP <= valueRW4.unitCfg.maxHP)
					{
						valueRW4.unitCfg.currentHP += monster.cureHpPercent * valueRW2.unitCfg.maxHP;
						monster.cureHpPercentTotal -= monster.cureHpPercent;
						ecb.AppendToBuffer(index, textFloatVFXBufferEtt, new TextFloatVFXBED
						{
							number = monster.cureHpPercent * valueRW2.unitCfg.maxHP,
							type = UITextFloatType.Recover,
							worldPos = LocalTsfLookUp.GetRefRO(item).ValueRO.Position
						});
					}
					else
					{
						float num = valueRW4.unitCfg.maxHP - valueRW4.unitCfg.currentHP;
						valueRW4.unitCfg.currentHP = valueRW4.unitCfg.maxHP;
						monster.cureHpPercentTotal -= num / valueRW2.unitCfg.maxHP;
						ecb.AppendToBuffer(index, textFloatVFXBufferEtt, new TextFloatVFXBED
						{
							number = num,
							type = UITextFloatType.Recover,
							worldPos = LocalTsfLookUp.GetRefRO(item).ValueRO.Position
						});
					}
					ecb.AppendToBuffer(index, healEffectEntity, new Monster314HealEffect
					{
						entity = item
					});
				}
			}
			target2.Dispose();
		}

		[CompilerGenerated]
		public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
		{
			IntPtr nativeArrayPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Monster314_Dots_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr2 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__UnitBase_Dots_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr3 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__PathFinding_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr4 = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(chunk, __TypeHandle.__Unity_Entities_Entity_TypeHandle);
			int count = chunk.Count;
			int num = 0;
			if (!useEnabledMask)
			{
				for (int i = 0; i < count; i++)
				{
					ref Monster314_Dots monster = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Monster314_Dots>(nativeArrayPtr, i);
					ref UnitBase_Dots unitBase = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitBase_Dots>(nativeArrayPtr2, i);
					ref PathFinding pathFinding = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PathFinding>(nativeArrayPtr3, i);
					Entity entity = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr4, i);
					Execute(chunkIndexInQuery, ref monster, ref unitBase, ref pathFinding, entity);
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
						ref Monster314_Dots monster2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Monster314_Dots>(nativeArrayPtr, nextRangeBegin);
						ref UnitBase_Dots unitBase2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitBase_Dots>(nativeArrayPtr2, nextRangeBegin);
						ref PathFinding pathFinding2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PathFinding>(nativeArrayPtr3, nextRangeBegin);
						Entity entity2 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr4, nextRangeBegin);
						Execute(chunkIndexInQuery, ref monster2, ref unitBase2, ref pathFinding2, entity2);
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
					ref Monster314_Dots monster3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Monster314_Dots>(nativeArrayPtr, j);
					ref UnitBase_Dots unitBase3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitBase_Dots>(nativeArrayPtr2, j);
					ref PathFinding pathFinding3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PathFinding>(nativeArrayPtr3, j);
					Entity entity3 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr4, j);
					Execute(chunkIndexInQuery, ref monster3, ref unitBase3, ref pathFinding3, entity3);
					num++;
				}
				num2 >>= 1;
			}
			num2 = chunkEnabledMask.ULong1;
			for (int k = 64; k < count; k++)
			{
				if ((num2 & 1) != 0L)
				{
					ref Monster314_Dots monster4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Monster314_Dots>(nativeArrayPtr, k);
					ref UnitBase_Dots unitBase4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitBase_Dots>(nativeArrayPtr2, k);
					ref PathFinding pathFinding4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PathFinding>(nativeArrayPtr3, k);
					Entity entity4 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr4, k);
					Execute(chunkIndexInQuery, ref monster4, ref unitBase4, ref pathFinding4, entity4);
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

	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_174171817_0
	{
		public struct ResolvedChunk
		{
			public IntPtr item1_IntPtr;

			public IntPtr item2_IntPtr;

			public IntPtr item3_IntPtr;

			public IntPtr Entity_IntPtr;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public QueryEnumerableWithEntity<Monster314_Dots, InternalCompilerInterface.UncheckedRefRW<Monster314RingEffect>, LocalTransform> Get(int index)
			{
				return new QueryEnumerableWithEntity<Monster314_Dots, InternalCompilerInterface.UncheckedRefRW<Monster314RingEffect>, LocalTransform>(InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Monster314_Dots>(item1_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<Monster314RingEffect>(item2_IntPtr, index), InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<LocalTransform>(item3_IntPtr, index), InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(Entity_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			[ReadOnly]
			private ComponentTypeHandle<Monster314_Dots> item1_ComponentTypeHandle_RO;

			private ComponentTypeHandle<Monster314RingEffect> item2_ComponentTypeHandle_RW;

			[ReadOnly]
			private ComponentTypeHandle<LocalTransform> item3_ComponentTypeHandle_RO;

			private EntityTypeHandle Entity_TypeHandle;

			public TypeHandle(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<Monster314_Dots>(isReadOnly: true);
				item2_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<Monster314RingEffect>();
				item3_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<LocalTransform>(isReadOnly: true);
				Entity_TypeHandle = systemState.GetEntityTypeHandle();
			}

			public void Update(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RO.Update(ref systemState);
				item2_ComponentTypeHandle_RW.Update(ref systemState);
				item3_ComponentTypeHandle_RO.Update(ref systemState);
				Entity_TypeHandle.Update(ref systemState);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ResolvedChunk Resolve(ArchetypeChunk archetypeChunk)
			{
				ResolvedChunk result = default(ResolvedChunk);
				result.item1_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks(in archetypeChunk, ref item1_ComponentTypeHandle_RO);
				result.item2_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item2_ComponentTypeHandle_RW);
				result.item3_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks(in archetypeChunk, ref item3_ComponentTypeHandle_RO);
				result.Entity_IntPtr = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(archetypeChunk, Entity_TypeHandle);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<QueryEnumerableWithEntity<Monster314_Dots, InternalCompilerInterface.UncheckedRefRW<Monster314RingEffect>, LocalTransform>>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public QueryEnumerableWithEntity<Monster314_Dots, InternalCompilerInterface.UncheckedRefRW<Monster314RingEffect>, LocalTransform> Current => _resolvedChunk.Get(_currentEntityIndex);

			object IEnumerator.Current
			{
				get
				{
					throw new NotImplementedException();
				}
			}

			public Enumerator(EntityQuery entityQuery, TypeHandle typeHandle, ref SystemState state)
			{
				if (!entityQuery.IsEmptyIgnoreFilter)
				{
					CompleteDependencies(ref state);
					typeHandle.Update(ref state);
				}
				_entityQueryEnumerator = new InternalEntityQueryEnumerator(entityQuery);
				_currentEntityIndex = -1;
				_endEntityIndex = -1;
				_typeHandle = typeHandle;
				_resolvedChunk = default(ResolvedChunk);
			}

			public void Dispose()
			{
				_entityQueryEnumerator.Dispose();
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public bool MoveNext()
			{
				_currentEntityIndex++;
				if (_currentEntityIndex >= _endEntityIndex)
				{
					if (_entityQueryEnumerator.MoveNextEntityRange(out var movedToNewChunk, out var chunk, out var entityStartIndex, out var entityEndIndex))
					{
						if (movedToNewChunk)
						{
							_resolvedChunk = _typeHandle.Resolve(chunk);
						}
						_currentEntityIndex = entityStartIndex;
						_endEntityIndex = entityEndIndex;
						return true;
					}
					return false;
				}
				return true;
			}

			public Enumerator GetEnumerator()
			{
				return this;
			}

			public void Reset()
			{
				throw new NotImplementedException();
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Enumerator Query(EntityQuery entityQuery, TypeHandle typeHandle, ref SystemState state)
		{
			return new Enumerator(entityQuery, typeHandle, ref state);
		}

		public static void CompleteDependencies(ref SystemState state)
		{
			state.EntityManager.CompleteDependencyBeforeRO<Monster314_Dots>();
			state.EntityManager.CompleteDependencyBeforeRW<Monster314RingEffect>();
			state.EntityManager.CompleteDependencyBeforeRO<LocalTransform>();
		}
	}

	private struct TypeHandle
	{
		public IFE_174171817_0.TypeHandle __IFE_174171817_0_TypeHandle;

		public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentLookup;

		public ComponentLookup<AnimaPlay> __AnimaPlay_RW_ComponentLookup;

		public ComponentLookup<UnitProperty_Dots> __UnitProperty_Dots_RW_ComponentLookup;

		public Monster314Job.InternalCompilerQueryAndHandleData __Monster314System_Monster314Job_WithDefaultQuery_JobEntityTypeHandle;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__IFE_174171817_0_TypeHandle = new IFE_174171817_0.TypeHandle(ref state);
			__Unity_Transforms_LocalTransform_RW_ComponentLookup = state.GetComponentLookup<LocalTransform>();
			__AnimaPlay_RW_ComponentLookup = state.GetComponentLookup<AnimaPlay>();
			__UnitProperty_Dots_RW_ComponentLookup = state.GetComponentLookup<UnitProperty_Dots>();
			__Monster314System_Monster314Job_WithDefaultQuery_JobEntityTypeHandle.Init(ref state, assignDefaultQuery: true);
		}
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void __codegen__OnCreate_00008B74_0024PostfixBurstDelegate(IntPtr self, IntPtr state);

	internal static class __codegen__OnCreate_00008B74_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<__codegen__OnCreate_00008B74_0024PostfixBurstDelegate>(delegate(IntPtr self, IntPtr state)
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
	internal delegate void __codegen__OnDestroy_00008B76_0024PostfixBurstDelegate(IntPtr self, IntPtr state);

	internal static class __codegen__OnDestroy_00008B76_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<__codegen__OnDestroy_00008B76_0024PostfixBurstDelegate>(delegate(IntPtr self, IntPtr state)
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
			__codegen__OnDestroy_0024BurstManaged(self, state);
		}
	}

	private TypeHandle __TypeHandle;

	private EntityQuery __query_174171817_0;

	private EntityQuery __query_174171817_1;

	private EntityQuery __query_174171817_2;

	private EntityQuery __query_174171817_3;

	private EntityQuery __query_174171817_4;

	private EntityQuery __query_174171817_5;

	private EntityQuery __query_174171817_6;

	[BurstCompile]
	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<Monster314_Dots>();
		state.EntityManager.CreateSingletonBuffer<Monster314HealEffect>();
	}

	public void OnUpdate(ref SystemState state)
	{
		foreach (QueryEnumerableWithEntity<Monster314_Dots, InternalCompilerInterface.UncheckedRefRW<Monster314RingEffect>, LocalTransform> item5 in IFE_174171817_0.Query(__query_174171817_0, __TypeHandle.__IFE_174171817_0_TypeHandle, ref state))
		{
			item5.Deconstruct(out var item, out var item2, out var item3, out var _);
			Monster314_Dots monster314_Dots = item;
			InternalCompilerInterface.UncheckedRefRW<Monster314RingEffect> uncheckedRefRW = item2;
			LocalTransform localTransform = item3;
			if (!monster314_Dots.Initialized)
			{
				uncheckedRefRW.ValueRW.ringEffect.Value = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Monster314_RepairRing", localTransform.Position);
			}
			else
			{
				uncheckedRefRW.ValueRO.ringEffect.Value.transform.position = localTransform.Position;
			}
		}
		EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer(Allocator.TempJob);
		state.Dependency = __ScheduleViaJobChunkExtension_0(new Monster314Job
		{
			CurrentRoomEntities = __query_174171817_1.GetSingleton<CurrentRoomEntitiesSingleton>(),
			LocalTsfLookUp = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state),
			AnimaLookUp = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__AnimaPlay_RW_ComponentLookup, ref state),
			deltaTime = state.WorldUnmanaged.Time.DeltaTime,
			ecb = entityCommandBuffer.AsParallelWriter(),
			globalRandom = __query_174171817_2.GetSingletonRW<GlobalRandom>(),
			PPtLookUp = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__UnitProperty_Dots_RW_ComponentLookup, ref state),
			textFloatVFXBufferEtt = __query_174171817_3.GetSingletonEntity(),
			healEffectEntity = __query_174171817_4.GetSingletonEntity(),
			roomCenter = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint,
			roomHeight = LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme8Height,
			roomWidth = LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme8Width
		}, __TypeHandle.__Monster314System_Monster314Job_WithDefaultQuery_JobEntityTypeHandle.DefaultQuery, state.Dependency, ref state, hasUserDefinedQuery: false);
		state.Dependency.Complete();
		entityCommandBuffer.Playback(state.EntityManager);
		entityCommandBuffer.Dispose();
		DynamicBuffer<Monster314HealEffect> singletonBuffer = __query_174171817_5.GetSingletonBuffer<Monster314HealEffect>();
		if (singletonBuffer.Length > 0)
		{
			NativeArray<Monster314HealEffect> nativeArray = singletonBuffer.ToNativeArray(Allocator.Temp);
			singletonBuffer.Clear();
			__query_174171817_6.GetSingleton<SpellSingleton>().Prefabs.TryGetValue("3108_Heal", out var item4);
			for (int i = 0; i < nativeArray.Length; i++)
			{
				Entity entity2 = state.EntityManager.Instantiate(item4);
				state.EntityManager.SetComponentData(entity2, new FollowEntity
				{
					ett = nativeArray[i].entity
				});
			}
			nativeArray.Dispose();
		}
	}

	[BurstCompile]
	public void OnDestroy(ref SystemState state)
	{
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private JobHandle __ScheduleViaJobChunkExtension_0(Monster314Job job, EntityQuery query, JobHandle dependency, ref SystemState state, bool hasUserDefinedQuery)
	{
		dependency = __TypeHandle.__Monster314System_Monster314Job_WithDefaultQuery_JobEntityTypeHandle.UpdateBaseEntityIndexArray(ref job, query, dependency, ref state);
		__TypeHandle.__Monster314System_Monster314Job_WithDefaultQuery_JobEntityTypeHandle.AssignEntityManager(ref job, state.EntityManager);
		__TypeHandle.__Monster314System_Monster314Job_WithDefaultQuery_JobEntityTypeHandle.__TypeHandle.Update(ref state);
		return __TypeHandle.__Monster314System_Monster314Job_WithDefaultQuery_JobEntityTypeHandle.ScheduleParallel(ref job, query, dependency);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<Monster314_Dots>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAll<LocalTransform>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Monster314RingEffect>();
		__query_174171817_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<CurrentRoomEntitiesSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_174171817_1 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAllRW<GlobalRandom>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_174171817_2 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<TextFloatVFXBED>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_174171817_3 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<Monster314HealEffect>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_174171817_4 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAllRW<Monster314HealEffect>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_174171817_5 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_174171817_6 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder.Dispose();
	}

	public void OnCreateForCompiler(ref SystemState state)
	{
		__AssignQueries(ref state);
		__TypeHandle.__AssignHandles(ref state);
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	[BurstCompile]
	internal static void __codegen__OnCreate(IntPtr self, IntPtr state)
	{
		__codegen__OnCreate_00008B74_0024BurstDirectCall.Invoke(self, state);
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnUpdate(IntPtr self, IntPtr state)
	{
		((Monster314System*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	[BurstCompile]
	internal static void __codegen__OnDestroy(IntPtr self, IntPtr state)
	{
		__codegen__OnDestroy_00008B76_0024BurstDirectCall.Invoke(self, state);
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
	{
		((Monster314System*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[BurstCompile]
	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreate_0024BurstManaged(IntPtr self, IntPtr state)
	{
		((Monster314System*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[BurstCompile]
	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnDestroy_0024BurstManaged(IntPtr self, IntPtr state)
	{
		((Monster314System*)self.ToPointer())->OnDestroy(ref *(SystemState*)state.ToPointer());
	}
}
